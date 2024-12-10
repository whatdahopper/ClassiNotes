using UnityEngine;

namespace ClassiNotes.Components;

internal class TrailNoteVisuals : MonoBehaviour, INoteControllerDidInitEvent, INoteControllerNoteDidStartJumpEvent, INoteControllerNoteDidStartDissolvingEvent
{
    enum MovementPhase
    {
        None,
        MovingOnTheFloor,
        Jumping
    }
    
    MovementPhase _movementPhase;
    bool _trailPSEmitting;

    [SerializeField] NoteController _noteController = null!;
    [SerializeField] ParticleSystem _trailPS = null!;

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
        _movementPhase = MovementPhase.None;

        _noteController.didInitEvent.Add(this);
        _noteController.noteDidStartJumpEvent.Add(this);
        _noteController.noteDidStartDissolvingEvent.Add(this);

        _noteController._noteMovement.didInitEvent += HandleNoteMovementDidInit;
        _noteController._noteMovement.noteDidStartJumpEvent += HandleNoteDidStartJump;
        _noteController._noteMovement.noteDidFinishJumpEvent += HandleNoteDidFinishJump;

        TrailPSEnabled = false;
    }

    void OnDestroy()
    {
        if (_noteController == null) return;

        _noteController.didInitEvent.Remove(this);
        _noteController.noteDidStartJumpEvent.Remove(this);
        _noteController.noteDidStartDissolvingEvent.Remove(this);

        _noteController._noteMovement.didInitEvent -= HandleNoteMovementDidInit;
        _noteController._noteMovement.noteDidStartJumpEvent -= HandleNoteDidStartJump;
        _noteController._noteMovement.noteDidFinishJumpEvent -= HandleNoteDidFinishJump;
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
    
    void HandleNoteMovementDidInit()
    {
        _movementPhase = MovementPhase.MovingOnTheFloor;
    }

    void HandleNoteDidStartJump()
    {
        _movementPhase = MovementPhase.Jumping;
    }
    
    void HandleNoteDidFinishJump()
    {
        _movementPhase = MovementPhase.None;
    }

    void Update()
    {
        var isMovingOnTheFloor = _movementPhase == MovementPhase.MovingOnTheFloor;
        if (isMovingOnTheFloor && !TrailPSEnabled && transform.localPosition.z < 30f)
            TrailPSEnabled = true;
    }
}
