using UnityEngine;

namespace ClassiNotes.DataStores;

[CreateAssetMenu(fileName = "PluginDataSO", menuName = "ClassiNotes/PluginDataSO")]
public class PluginDataSO : ScriptableObject
{
    [SerializeField] private GameObject _noteTrailPS = null!;
    [SerializeField] private GameObject _bombNoteTrailPS = null!;

    [SerializeField] private GameObject _noteDirectionSparkles = null!;
    [SerializeField] private GameObject _noteExplosionSparkles = null!;
    [SerializeField] private GameObject _bombExplosionSparkles = null!;

    [SerializeField] private Material _noteMaterial = null!;
    [SerializeField] private Material _noteDebrisMaterial = null!;

    [SerializeField] private GameObject _reflectionProbeContainer = null!;

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
