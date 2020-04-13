using IPA.Utilities;
using More_Modifiers.Controllers;
using UnityEngine;

namespace More_Modifiers.Modifiers.Notes
{
    class WarpNotesController : INoteModifier
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

        public WarpNotesController(NoteModifierController controller)
        {
            Controller = controller;

            SetEvents();
        }

        ~WarpNotesController()
        {
            Cleanup();
        }

        public void Cleanup() => RemoveEvents();

        void SetEvents()
        {
            Controller.OnNoteDidStartJump += OnNoteDidStartJump;
            Controller.OnNoteSpawned += OnColorNoteVisualsDidInit;
        }

        void RemoveEvents()
        {
            Controller.OnNoteDidStartJump -= OnNoteDidStartJump;
            Controller.OnNoteSpawned -= OnColorNoteVisualsDidInit;
        }

        void OnNoteDidStartJump()
        {
            if (Controller.NoteController.noteData.cutDirection == NoteCutDirection.Any)
                ReflectionUtil.GetField<SpriteRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_circleGlowSpriteRenderer").enabled = true;
            else
            {
                ReflectionUtil.GetField<MeshRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowMeshRenderer").enabled = true;
                ReflectionUtil.GetField<SpriteRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowGlowSpriteRenderer").enabled = true;
            }

            Controller.CubeMeshRenderer.enabled = true;
            Controller.gameObject.GetComponent<BaseNoteVisuals>().AnimateCutout(1f, 0f, 0.5f);
        }

        void OnColorNoteVisualsDidInit()
        {
            foreach (var s in Controller.SpriteRenderers) s.enabled = false;
            if (Controller.NoteController.noteData.cutDirection != NoteCutDirection.Any)
            {
                ReflectionUtil.GetField<MeshRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowMeshRenderer").enabled = false;
            }
            Controller.CubeMeshRenderer.enabled = false;
        }

        void Reset()
        {
            Controller.CubeMeshRenderer.enabled = true;
            if (Controller.NoteController.noteData.cutDirection != NoteCutDirection.Any)
            {
                ReflectionUtil.GetField<MeshRenderer, ColorNoteVisuals>(Controller.ColorNoteVisuals, "_arrowMeshRenderer").enabled = true;
            }
        }
    }
}
