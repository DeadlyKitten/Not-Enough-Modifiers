using More_Modifiers.Configuration;

namespace More_Modifiers.HarmonyPatches
{
    class BeatmapObjectSpawnController_SpawnNote
    {
        public static bool Prefix(NoteData noteData)
        {
            if (noteData.noteType == NoteType.Bomb) return true;

            switch (Config.instance.ColorNoteToHide)
            {
                case HideNoteType.Left:
                    return noteData.noteType != NoteType.NoteA;
                case HideNoteType.Right:
                    return noteData.noteType != NoteType.NoteB;
            }

            return true;
        }
    }
}