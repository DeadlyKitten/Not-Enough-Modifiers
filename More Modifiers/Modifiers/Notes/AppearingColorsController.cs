using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using More_Modifiers.Controllers;
using UnityEngine;

namespace More_Modifiers.Modifiers.Notes
{
    class AppearingColorsController : INoteModifier
    {
        private NoteModifierController Controller { get; set; }
        private bool _enabled;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (value)
                    SetEvents();
                else
                    RemoveEvents();
                _enabled = value;
            }
        }

        public AppearingColorsController(NoteModifierController controller)
        {
            Controller = controller;

            SetEvents();
        }

        ~AppearingColorsController()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            RemoveEvents();
        }

        void SetEvents()
        {
            Controller.OnColorNoteVisualsDidInit += OnColorNoteVisualsDidInit;
            Controller.OnUpdate += OnUpdate;
        }

        void RemoveEvents()
        {
            Controller.OnColorNoteVisualsDidInit -= OnColorNoteVisualsDidInit;
            Controller.OnUpdate -= OnUpdate;
        }

        void OnUpdate()
        {
            if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.MovingOnTheFloor)
            {
                Controller.SetNoteColor(0);
            }
            else if (Controller.NoteMovement.movementPhase == NoteMovement.MovementPhase.Jumping)
            {
                float colorIntensity = 1 - Mathf.Clamp01((Controller.NoteMovement.distanceToPlayer - Controller.MinDistance) / (Controller.MaxDistance - Controller.MinDistance));
                Controller.SetNoteColor(colorIntensity);
            }
        }

        void OnColorNoteVisualsDidInit()
        {
            Controller.SetNoteColor(0f);
        }
    }
}
