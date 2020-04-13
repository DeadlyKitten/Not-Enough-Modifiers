using IPA.Utilities;
using More_Modifiers.Controllers;
using UnityEngine;

namespace More_Modifiers.Modifiers.Notes
{
    class AppearingArrowsController : INoteModifier
    {
        private NoteModifierController Controller { get; set; }
        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value)
                    SetEvents();
                else
                    RemoveEvents();
                _enabled = value;
            }
        }

        public AppearingArrowsController(NoteModifierController controller)
        {
            Controller = controller;

            SetEvents();
        }

        ~AppearingArrowsController()
        {
            Cleanup();
        }

        public void SetEvents()
        {
            RemoveEvents();
            Controller.OnNoteSpawned += OnNoteSpawned;
            Controller.OnNoteDidStartJump += OnNoteDidStartJump;
            Controller.OnUpdate += OnUpdate;
        }

        public void RemoveEvents()
        {
            Controller.OnNoteSpawned -= OnNoteSpawned;
            Controller.OnNoteDidStartJump -= OnNoteDidStartJump;
            Controller.OnUpdate -= OnUpdate;
        }

        public void Cleanup() => RemoveEvents();

        public void OnNoteSpawned()
        {
            Controller.SetArrowTransparency(0f);
            foreach (var s in Controller.SpriteRenderers) s.enabled = false;
            if (Controller.NoteController.noteData.cutDirection != NoteCutDirection.Any)
            {
                ReflectionUtil.GetField<MeshRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowMeshRenderer").enabled = false;
            }
        }

        public void OnNoteDidStartJump()
        {
            if (Controller.NoteController.noteData.cutDirection == NoteCutDirection.Any)
            {
                ReflectionUtil.GetField<SpriteRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_circleGlowSpriteRenderer").enabled = true;
            }
            else
            {
                ReflectionUtil.GetField<MeshRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowMeshRenderer").enabled = true;
                ReflectionUtil.GetField<SpriteRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowGlowSpriteRenderer").enabled = true;
            }
        }

        public void OnUpdate()
        {
            if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.MovingOnTheFloor)
            {
                Controller.SetArrowTransparency(0);
            }
            else if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.Jumping)
            {
                var arrowTransparency = 1 - Mathf.Clamp01((Controller.NoteMovement.distanceToPlayer - Controller.MinDistance) / (Controller.MaxDistance - Controller.MinDistance));
                Controller.SetArrowTransparency(arrowTransparency);
            }
        }
    }
}
