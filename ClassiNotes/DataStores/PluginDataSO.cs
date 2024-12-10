using UnityEngine;

namespace ClassiNotes.DataStores;

[CreateAssetMenu(fileName = "PluginDataSO", menuName = "ClassiNotes/PluginDataSO")]
public class PluginDataSO : ScriptableObject
{
    [SerializeField] GameObject _noteTrailPS = null!;
    [SerializeField] GameObject _bombNoteTrailPS = null!;

    [SerializeField] GameObject _noteDirectionSparkles = null!;
    [SerializeField] GameObject _noteExplosionSparkles = null!;
    [SerializeField] GameObject _bombExplosionSparkles = null!;

    [SerializeField] Material _noteMaterial = null!;
    [SerializeField] Material _noteDebrisMaterial = null!;

    [SerializeField] GameObject _reflectionProbeContainer = null!;

    public GameObject NoteTrailPS => _noteTrailPS;
    public GameObject BombNoteTrailPS => _bombNoteTrailPS;

    public GameObject NoteDirectionSparkles => _noteDirectionSparkles;
    public GameObject NoteExplosionSparkles => _noteExplosionSparkles;
    public GameObject BombExplosionSparkles => _bombExplosionSparkles;

    public Material NoteMaterial => _noteMaterial;
    public Material NoteDebrisMaterial => _noteDebrisMaterial;

    public GameObject ReflectionProbeContainer => _reflectionProbeContainer;

    void OnEnable()
    {
        hideFlags |= HideFlags.DontUnloadUnusedAsset;
    }
}
