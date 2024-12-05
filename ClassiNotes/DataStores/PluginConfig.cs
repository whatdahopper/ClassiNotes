using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace ClassiNotes.DataStores;

internal class PluginConfig
{
    public virtual bool ClassicNoteShader { get; set; } = true;
    public virtual bool ClassicDebrisPhysics { get; set; } = true;
    public virtual bool ExtraNoteEffects { get; set; } = false;
    public virtual bool NoteTrails { get; set; } = true;
}
