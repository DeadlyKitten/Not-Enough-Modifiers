using IPA.Utilities;
using More_Modifiers.Configuration;
using More_Modifiers.Modifiers.Notes;
using System;
using UnityEngine;

namespace More_Modifiers.Controllers
{
    class NoteModifierController : MonoBehaviour
    {
        public ColorNoteVisuals ColorNoteVisuals { get; private set; }
        public MeshRenderer CubeMeshRenderer { get; private set; }
        private CutoutEffect _arrowCutoutEffect;
        public NoteMovement NoteMovement { get; private set; }
        public SpriteRenderer[] SpriteRenderers { get; private set; }
        public NoteController NoteController { get; set; }
        public float MinDistance { get; private set; }
        public float MaxDistance { get; private set; }

        private float _effectStart = 14f;
        private float _effectEnd = 8f;
        private float[] _initialSpriteAlphas;
        private bool _initialized;

        private NoteModifierSettings _settings;

        public Action OnNoteMovementDidInit;
        public Action OnColorNoteVisualsDidInit;
        public Action OnNoteDidStartJump;
        public Action OnNoteDidFinishJump;
        public Action OnUpdate;
        public Action OnNoteCut;
        public Action OnNoteSpawned;

        // Modifier Controllers
        AppearingArrowsController _appearingArrowsController;
        AppearingColorsController _appearingColorsController;
        DisappearingColorsController _disappearingColorsController;
        WarpNotesController _warpNotesController;
        HiddenNotesController _hiddenNotesController;
        ShrinkingNotesController _shrinkingNotesController;

        public static void Init(NoteController note, NoteModifierSettings settings)
        {
            NoteModifierController go;
            if (!(go = note.gameObject.GetComponent<NoteModifierController>()))
            {
                go = note.gameObject.AddComponent<NoteModifierController>();
            }

            go.NoteController = note;
            go._settings = settings ?? new NoteModifierSettings();



            go.NoteController.noteWasCutEvent += go.OnNoteCutEvent;

            go._appearingArrowsController.Enabled = go._settings.AppearingArrows;
            go._appearingColorsController.Enabled = go._settings.AppearingColors;
            go._disappearingColorsController.Enabled = go._settings.DisappearingColors;
            go._warpNotesController.Enabled = go._settings.WarpNotes;
            go._hiddenNotesController.Enabled = go._settings.HiddenBlocks;
            go._shrinkingNotesController.Enabled = go._settings.ShrinkingBlocks;

            var gnc = go.NoteController as GameNoteController;
            if (gnc)
            {
                if (gnc.ghostNote || go._settings.GhostNotes)
                    go.CubeMeshRenderer.enabled = false;
                if (go._settings.DisappearingArrows)
                    gnc.SetField("_disappearingArrow", true);
            }

            if (!go._initialized)
            {
                go.HandleNoteMovementDidInit();
                go.HandleColorNoteVisualsDidInitEvent();
            }

            go.OnNoteSpawned?.Invoke();
        }

        void Awake()
        {
            // should only run once, but just in case
            if (_initialized) return;

            // grab stuff from the DisappearingArrowController
            var dac = GetComponent<DisappearingArrowController>();
            ColorNoteVisuals = ReflectionUtil.GetField<ColorNoteVisuals, DisappearingArrowController>(dac, "_colorNoteVisuals");
            SpriteRenderers = ReflectionUtil.GetField<SpriteRenderer[], DisappearingArrowController>(dac, "_spriteRenderers");
            CubeMeshRenderer = ReflectionUtil.GetField<MeshRenderer, DisappearingArrowController>(dac, "_cubeMeshRenderer");
            _arrowCutoutEffect = ReflectionUtil.GetField<CutoutEffect, DisappearingArrowController>(dac, "_arrowCutoutEffect");
            NoteMovement = ReflectionUtil.GetField<NoteMovement, DisappearingArrowController>(dac, "_noteMovement");
            _effectStart = ReflectionUtil.GetField<float, DisappearingArrowController>(dac, "_disappearingNormalStart");
            _effectEnd = ReflectionUtil.GetField<float, DisappearingArrowController>(dac, "_disappearingNormalEnd");

            _initialSpriteAlphas = new float[SpriteRenderers.Length];
            for (var i = 0; i < _initialSpriteAlphas.Length; i++)
                _initialSpriteAlphas[i] = SpriteRenderers[i].color.a;

            // Set events
            ColorNoteVisuals.didInitEvent += HandleColorNoteVisualsDidInitEvent;
            NoteMovement.didInitEvent += HandleNoteMovementDidInit;
            NoteMovement.noteDidStartJumpEvent += OnNoteDidStartJumpEvent;
            NoteMovement.noteDidFinishJumpEvent += OnNoteDidFinishJumpEvent;

            // create Modifier Controllers
            _appearingArrowsController = new AppearingArrowsController(this);
            _appearingColorsController = new AppearingColorsController(this);
            _disappearingColorsController = new DisappearingColorsController(this);
            _warpNotesController = new WarpNotesController(this);
            _hiddenNotesController = new HiddenNotesController(this);
            _shrinkingNotesController = new ShrinkingNotesController(this);
        }

        public virtual void OnDestroy()
        {
            if (ColorNoteVisuals)
                ColorNoteVisuals.didInitEvent -= HandleColorNoteVisualsDidInitEvent;

            if (NoteMovement)
            {
                NoteMovement.didInitEvent -= HandleNoteMovementDidInit;
                NoteMovement.noteDidStartJumpEvent -= OnNoteDidStartJumpEvent;
                NoteMovement.noteDidFinishJumpEvent -= OnNoteDidFinishJumpEvent;
            }
        }

        void OnNoteDidStartJumpEvent() => OnNoteDidStartJump?.Invoke();

        void OnNoteDidFinishJumpEvent() => OnNoteDidFinishJump?.Invoke();

        void Update() => OnUpdate?.Invoke();

        void OnNoteCutEvent(NoteController noteController, NoteCutInfo noteCutInfo) => OnNoteCut?.Invoke();

        public virtual void HandleNoteMovementDidInit()
        {
            if (NoteMovement)
            {
                MaxDistance = Mathf.Min(NoteMovement.moveEndPos.z * 0.8f, _effectStart);
                MinDistance = Mathf.Min(NoteMovement.moveEndPos.z * 0.5f, _effectEnd);
            }
            else
            {
                MaxDistance = _effectStart;
                MinDistance = _effectEnd;
            }

            OnNoteMovementDidInit?.Invoke();
        }

        void HandleColorNoteVisualsDidInitEvent() => HandleColorNoteVisualsDidInitEvent(ColorNoteVisuals, GetComponent<NoteController>());

        public virtual void HandleColorNoteVisualsDidInitEvent(ColorNoteVisuals colorNoteVisuals, NoteController noteController)
        {
            if (!_initialized)
            {
                _initialized = true;
                for (var i = 0; i < _initialSpriteAlphas.Length; i++)
                    _initialSpriteAlphas[i] = SpriteRenderers[i].color.a;
            }
            OnColorNoteVisualsDidInit?.Invoke();
        }

        public virtual void SetArrowTransparency(float value)
        {
            for (var i = 0; i < _initialSpriteAlphas.Length; i++)
            {
                var spriteRenderer = SpriteRenderers[i];
                spriteRenderer.color = spriteRenderer.color.ColorWithAlpha(value * _initialSpriteAlphas[i]);
            }
            if (_arrowCutoutEffect.useRandomCutoutOffset)
            {
                _arrowCutoutEffect.SetCutout(1f - value);
                return;
            }
            _arrowCutoutEffect.SetCutout(1f - value, Vector3.zero);
        }

        public virtual void SetNoteColor(float value)
        {
            var materialPropertyBlockControllers = ReflectionUtil.GetField<MaterialPropertyBlockController[], ColorNoteVisuals>(ColorNoteVisuals, "_materialPropertyBlockControllers");
            var oldColor = ReflectionUtil.GetField<Color, ColorNoteVisuals>(ColorNoteVisuals, "_noteColor");
            var newColor = Color.Lerp(Color.gray, oldColor, value);
            SetNoteColor(newColor);
        }

        public virtual void SetNoteColor(Color color)
        {
            var materialPropertyBlockControllers = ReflectionUtil.GetField<MaterialPropertyBlockController[], ColorNoteVisuals>(ColorNoteVisuals, "_materialPropertyBlockControllers");
            var oldColor = ReflectionUtil.GetField<Color, ColorNoteVisuals>(ColorNoteVisuals, "_noteColor");
            foreach (var materialPropertyBlockController in materialPropertyBlockControllers)
            {
                materialPropertyBlockController.materialPropertyBlock.SetColor(Shader.PropertyToID("_Color"), color.ColorWithAlpha(oldColor.a));
                materialPropertyBlockController.ApplyChanges();
            }
            for (var i = 0; i < _initialSpriteAlphas.Length; i++)
            {
                var spriteRenderer = SpriteRenderers[i];
                spriteRenderer.color = color.ColorWithAlpha(spriteRenderer.color.a);
            }
        }
    }
}
