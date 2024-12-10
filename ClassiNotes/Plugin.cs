using ClassiNotes.DataStores;
using ClassiNotes.Installers;
using ClassiNotes.UI;
using IPA;
using IPA.Config.Stores;
using SiraUtil.Attributes;
using SiraUtil.Zenject;
using UnityEngine;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace ClassiNotes;

[Plugin(RuntimeOptions.DynamicInit), Slog]
public class Plugin
{
#if DEBUG
    internal const bool IsDebugBuild = true;
#else
    internal const bool IsDebugBuild = false;
#endif

    internal static PluginDataSO? Data { get; private set; }

    [Init]
    public Plugin(IPALogger logger, IPAConfig conf, Zenjector zenjector)
    {
        zenjector.UseLogger(logger);

        var config = conf.Generated<PluginConfig>();
        zenjector.Install(Location.App, Container => Container.BindInstance(config).AsSingle());
        zenjector.Install(Location.Menu, Container => Container.BindInterfacesTo<CNSettingsView>().AsSingle());

        zenjector.Expose<NoteCutParticlesEffect>("Gameplay");
        zenjector.Install<CNGameInstaller>(Location.Player | Location.Tutorial);
    }

    [OnEnable]
    public void OnEnable()
    {
        if (Data != null) return;

#if DEBUG
        var content = AssetBundle.LoadFromFile(@"F:\Unity Projects\ClassiNotes.Unity\AssetBundles\content");
#else
        using var mrs = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ClassiNotes.Resources.content");
        var content = AssetBundle.LoadFromStream(mrs);
#endif
        if (content != null)
        {
            Data = content.LoadAsset<PluginDataSO>("assets/plugindataso.asset");
            content.Unload(false);
        }
    }

    [OnDisable]
    public void OnDisable() { }
}
