using UnityEngine;

namespace ClassiNotes.Components;

internal class TrailNoteVisuals : MonoBehaviour, INoteControllerDidInitEvent, INoteControllerNoteDidStartJumpEvent, INoteControllerNoteDidStartDissolvingEvent
{
    private bool _trailPSEmitting;

    [SerializeField] private NoteController _noteController = null!;
    [SerializeField] private ParticleSystem _trailPS = null!;

    public bool TrailPSEnabled
    {
        get => _trailPSEmitting;
        set
        {
            var emission = _trailPS.emission;
            emission.enabled = value;
            _trailPSEmitting = value;
        }
    }

    public void Init(NoteController noteController, ParticleSystem trailPS)
    {
        _noteController = noteController;
        _trailPS = trailPS;
    }
    
    void Awake()
    {
        _noteController.didInitEvent.Add(this);
        _noteController.noteDidStartJumpEvent.Add(this);
        _noteController.noteDidStartDissolvingEvent.Add(this);

        TrailPSEnabled = false;
    }

    void OnDestroy()
    {
        if (_noteController == null) return;

        _noteController.didInitEvent.Remove(this);
        _noteController.noteDidStartJumpEvent.Remove(this);
        _noteController.noteDidStartDissolvingEvent.Remove(this);
    }

    public void HandleNoteControllerDidInit(NoteControllerBase noteController)
    {
        var main = _trailPS.main;
        main.startColor = Color.white;
    }

    public void HandleNoteControllerNoteDidStartJump(NoteController noteController)
    {
        TrailPSEnabled = false;
    }

    public void HandleNoteControllerNoteDidStartDissolving(NoteControllerBase noteController, float duration)
    {
        TrailPSEnabled = false;
    }
    
    void Update()
    {
        var isMovingOnTheFloor = _noteController._noteMovement.movementPhase == NoteMovement.MovementPhase.MovingOnTheFloor;
        if (isMovingOnTheFloor && !TrailPSEnabled && transform.localPosition.z < 30f)
            TrailPSEnabled = true;
    }
}
