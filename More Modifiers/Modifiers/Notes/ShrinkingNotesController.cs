using More_Modifiers.Controllers;
using UnityEngine;

namespace More_Modifiers.Modifiers.Notes
{
    class ShrinkingNotesController
    {
        private NoteModifierController Controller { get; set; }
        private bool _enabled;

        private Vector3 startScale;

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

        public ShrinkingNotesController(NoteModifierController controller)
        {
            Controller = controller;
            startScale = controller.gameObject.transform.localScale;

            SetEvents();
        }

        ~ShrinkingNotesController()
        {
            Cleanup();
        }

        public void SetEvents()
        {
            RemoveEvents();
            Controller.OnUpdate += OnUpdate;
            Controller.OnNoteDidFinishJump += Reset;
            Controller.OnNoteCut += Reset;
        }

        public void RemoveEvents()
        {
            Controller.OnUpdate -= OnUpdate;
            Controller.OnNoteDidFinishJump -= Reset;
            Controller.OnNoteCut -= Reset;
        }

        void Cleanup() => RemoveEvents();

        void OnUpdate()
        {
            if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.Jumping)
            {
                var value = Mathf.Clamp01((Controller.NoteMovement.distanceToPlayer - Controller.MinDistance) / (Controller.MaxDistance - Controller.MinDistance));
                Controller.gameObject.transform.localScale = startScale * (value * 0.5f + 0.5f);
            }
        }

        void Reset() => Controller.gameObject.transform.localScale = startScale;
    }
}
