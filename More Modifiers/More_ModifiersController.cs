using System.Collections;
using System.Linq;
using UnityEngine;

namespace More_Modifiers
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class More_ModifiersController : MonoBehaviour
    {
        public static More_ModifiersController Instance { get; private set; }

        internal BeatmapObjectManager beatmapObjectManager;

        private void Awake()
        {
            if (Instance != null)
            {
                Logger.log?.Warn($"Instance of {this.GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this);
            Instance = this;
        }

        private void Start()
        {
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += GameSceneLoaded;
            BS_Utils.Utilities.BSEvents.menuSceneActive += GameSceneLeft;
        }

        void OnNoteSpawned(NoteController noteController)
        {
            if (noteController.noteData.noteType == NoteType.Bomb) return;
            Controllers.NoteModifierController.Init(noteController, new Configuration.NoteModifierSettings());
        }

        private void OnDestroy() => Instance = null;

        void GameSceneLoaded()
        {
            if (Configuration.Config.instance.ModEnabled)
            {
                Logger.log.Debug("Mod enabled; disabling score submission");
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("More Modifiers");
            }
            StartCoroutine(Initialize());
        }

        void GameSceneLeft()
        {
            if (beatmapObjectManager)
            {
                beatmapObjectManager.noteWasSpawnedEvent -= OnNoteSpawned;
                beatmapObjectManager = null;
            }

            HarmonyPatches.GameNoteController_HandleCut.sabers = null;
            HarmonyPatches.GameNoteController_HandleCut.spawner = null;
        }

        IEnumerator Initialize()
        {
            yield return new WaitUntil(() => beatmapObjectManager = Resources.FindObjectsOfTypeAll<BeatmapObjectManager>().FirstOrDefault());

            beatmapObjectManager.noteWasSpawnedEvent += OnNoteSpawned;
        }
    }
}
