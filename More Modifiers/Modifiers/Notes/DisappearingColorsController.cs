using More_Modifiers.Controllers;
using UnityEngine;

namespace More_Modifiers.Modifiers.Notes
{
    class DisappearingColorsController : INoteModifier
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

        public DisappearingColorsController(NoteModifierController controller)
        {
            Controller = controller;

            SetEvents();
        }

        ~DisappearingColorsController()
        {
            Cleanup();
        }

        public void Cleanup() => RemoveEvents();

        void SetEvents() => Controller.OnUpdate += OnUpdate;

        void RemoveEvents() => Controller.OnUpdate -= OnUpdate;

        void OnUpdate()
        {
            if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.MovingOnTheFloor)
            {
                Controller.SetNoteColor(1);
            }
            if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.Jumping)
            {
                var colorIntensity = Mathf.Clamp01((Controller.NoteMovement.distanceToPlayer - Controller.MinDistance) / (Controller.MaxDistance - Controller.MinDistance));
                Controller.SetNoteColor(colorIntensity);
            }
        }
    }
}
