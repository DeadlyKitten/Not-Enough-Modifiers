using More_Modifiers.Controllers;

namespace More_Modifiers.Modifiers.Notes
{
    class HiddenNotesController : INoteModifier
    {
        private NoteModifierController Controller { get; set; }
        private bool _enabled;
        private bool isHidden;

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

        public HiddenNotesController(NoteModifierController controller)
        {
            Controller = controller;

            SetEvents();
        }

        ~HiddenNotesController()
        {
            Cleanup();
        }

        public void Cleanup() => RemoveEvents();

        void SetEvents()
        {
            Controller.OnUpdate += OnUpdate;
            Controller.OnNoteMovementDidInit += OnNoteMovementDidInit;
        }

        void RemoveEvents()
        {
            Controller.OnUpdate -= OnUpdate;
            Controller.OnNoteSpawned -= OnNoteMovementDidInit;
        }

        void OnUpdate()
        {
            if (!isHidden && Controller.NoteMovement.distanceToPlayer < (Controller.MinDistance + Controller.MaxDistance) / 2)
            {
                isHidden = true;
                Controller.gameObject.GetComponent<BaseNoteVisuals>().AnimateCutout(0f, 1f, Controller.NoteMovement.jumpDuration / 4);
                foreach (var s in Controller.SpriteRenderers) s.enabled = false;
            }
        }

        void OnNoteMovementDidInit() => isHidden = false;
    }
}

