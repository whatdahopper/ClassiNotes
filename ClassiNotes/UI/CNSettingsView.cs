using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using ClassiNotes.DataStores;
using System;
using Zenject;

namespace ClassiNotes.UI;

internal class CNSettingsView : IInitializable, IDisposable
{
    readonly PluginConfig _config;
    readonly GameplaySetup _gameplaySetup;

    [UIValue("noteshader")]
    public bool ClassicNoteShader
    {
        get => _config.ClassicNoteShader;
        set => _config.ClassicNoteShader = value;
    }

    [UIValue("debrisphysics")]
    public bool ClassicDebrisPhysics
    {
        get => _config.ClassicDebrisPhysics;
        set => _config.ClassicDebrisPhysics = value;
    }

    [UIValue("notefx")]
    public bool ExtraNoteEffects
    {
        get => _config.ExtraNoteEffects;
        set => _config.ExtraNoteEffects = value;
    }

    [UIValue("notetrails")]
    public bool NoteTrails
    {
        get => _config.NoteTrails;
        set => _config.NoteTrails = value;
    }

    public CNSettingsView(PluginConfig config, GameplaySetup gameplaySetup)
    {
        _config = config;
        _gameplaySetup = gameplaySetup;
    }

    public void Initialize() => _gameplaySetup.AddTab("ClassiNotes", "ClassiNotes.Views.settings-view.bsml", this);
    public void Dispose() => _gameplaySetup.RemoveTab("ClassiNotes");
}
