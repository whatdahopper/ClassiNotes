using ClassiNotes.AffinityPatches;
using ClassiNotes.Components;
using ClassiNotes.DataStores;
using SiraUtil.Extras;
using SiraUtil.Objects.Beatmap;
using SiraUtil.Objects.Beatmap.Debris;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace ClassiNotes.Installers;

internal class CNGameInstaller : Installer
{
    const int DECORATION_PRIORITY = -1;

    readonly PluginConfig _config;
    readonly NoteCutParticlesEffect? _noteCutParticlesEffect;
    readonly bool _proMode;
    readonly bool _reduceDebris;
    readonly Material _sparkleMaterial;

    public CNGameInstaller(
        PluginConfig config,
        [InjectOptional] NoteCutParticlesEffect? noteCutParticlesEffect,
        [InjectOptional] GameplayCoreSceneSetupData? gameplayCoreSceneSetupData)
    {
        _config = config;
        _noteCutParticlesEffect = noteCutParticlesEffect;
        _proMode = gameplayCoreSceneSetupData != null ? gameplayCoreSceneSetupData.gameplayModifiers.proMode : false;
        _reduceDebris = gameplayCoreSceneSetupData != null ? gameplayCoreSceneSetupData.playerSpecificSettings.reduceDebris : false;
        _sparkleMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(x => x.name == "Sparkle").FirstOrDefault();
    }

    public override void InstallBindings()
    {
        if (_noteCutParticlesEffect != null && _config.ExtraNoteEffects)
        {
            _noteCutParticlesEffect._explosionCorePS.gameObject.SetActive(true);
            _noteCutParticlesEffect._explosionPrePassBloomPS.gameObject.SetActive(true);
        }

        if (_config.ClassicDebrisPhysics) Container.BindInterfacesTo<NoteDebrisPatch>().AsSingle();

        if (Plugin.Data == null) return; // Everything else requires plugin data being loaded.

        if (_config.ClassicNoteShader || _config.NoteTrails)
        {
            // Note/Bomb
            if (!_proMode)
                Container.RegisterRedecorator(new BasicNoteRegistration(DecorateNote, DECORATION_PRIORITY));
            else
                Container.RegisterRedecorator(new ProModeNoteRegistration(DecorateNote, DECORATION_PRIORITY));

            Container.RegisterRedecorator(new BombNoteRegistration(DecorateBombNote, DECORATION_PRIORITY));

            // Slider
            Container.RegisterRedecorator(new BurstSliderHeadNoteRegistration(DecorateNote, DECORATION_PRIORITY));
            Container.RegisterRedecorator(new BurstSliderNoteRegistration(DecorateBurstSliderNote, DECORATION_PRIORITY));
        }

        if (_config.ClassicNoteShader)
        {
            // FIXME(whatdahopper): This check doesn't work if you play a BeatLeader replay and have reduce debris enabled.
            // You'll get the old note shader, but debris will be enabled and use the modern debris shader.
            //if (!_reduceDebris)
            {
                Container.RegisterRedecorator(new NormalNoteDebrisHDRegistration(DecorateNoteDebris, DECORATION_PRIORITY));
                Container.RegisterRedecorator(new NormalNoteDebrisLWRegistration(DecorateNoteDebris, DECORATION_PRIORITY));
                Container.RegisterRedecorator(new BurstSliderHeadNoteDebrisHDRegistration(DecorateNoteDebris, DECORATION_PRIORITY));
                Container.RegisterRedecorator(new BurstSliderHeadNoteDebrisLWRegistration(DecorateNoteDebris, DECORATION_PRIORITY));
                Container.RegisterRedecorator(new BurstSliderElementNoteHDRegistration(DecorateNoteDebris, DECORATION_PRIORITY));
                Container.RegisterRedecorator(new BurstSliderElementNoteLWRegistration(DecorateNoteDebris, DECORATION_PRIORITY));
            }

            UnityEngine.Object.Instantiate(Plugin.Data.ReflectionProbeContainer);
        }
    }

    void DecorateTrailNoteVisuals(NoteController original, GameObject trailPSPrefab)
    {
        if (!_config.NoteTrails) return;
        var prevActiveSelf = original.gameObject.activeSelf;
        original.gameObject.SetActive(false);
        var trailPS = UnityEngine.Object.Instantiate(trailPSPrefab);
        trailPS.GetComponent<ParticleSystemRenderer>().sharedMaterial = _sparkleMaterial;
        trailPS.transform.SetParent(original.transform, true);
        var trailNoteVisuals = original.gameObject.AddComponent<TrailNoteVisuals>();
        trailNoteVisuals.Init(original, trailPS.GetComponent<ParticleSystem>());
        original.gameObject.SetActive(prevActiveSelf);
    }

    void DecorateGenericNote(NoteController original)
    {
        if (Plugin.Data == null) return;
        DecorateTrailNoteVisuals(original, Plugin.Data.NoteTrailPS);
        if (!_config.ClassicNoteShader) return;
        var noteCubeGameObject = original.transform.Find("NoteCube").gameObject;
        var noteCubeMeshRenderer = noteCubeGameObject.GetComponent<MeshRenderer>();
#pragma warning disable CS0612
        UnityEngine.Object.DestroyImmediate(noteCubeGameObject.GetComponent<ConditionalMaterialSwitcher>());
#pragma warning restore CS0612
        noteCubeMeshRenderer.material = Plugin.Data.NoteMaterial;
        noteCubeMeshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Simple;
    }

    GameNoteController DecorateNote(GameNoteController original)
    {
        DecorateGenericNote(original);
        return original;
    }

    BombNoteController DecorateBombNote(BombNoteController original)
    {
        if (Plugin.Data == null) return original;
        DecorateTrailNoteVisuals(original, Plugin.Data.BombNoteTrailPS);
        return original;
    }

    BurstSliderGameNoteController DecorateBurstSliderNote(BurstSliderGameNoteController original)
    {
        DecorateGenericNote(original);
        return original;
    }

    NoteDebris DecorateNoteDebris(NoteDebris original)
    {
        if (Plugin.Data == null) return original;
        var noteCubeGameObject = original.transform.Find("NoteDebrisMesh").gameObject;
        var noteCubeMeshRenderer = noteCubeGameObject.GetComponent<MeshRenderer>();
        noteCubeMeshRenderer.material = Plugin.Data.NoteDebrisMaterial;
        noteCubeMeshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Simple;
        return original;
    }
}
