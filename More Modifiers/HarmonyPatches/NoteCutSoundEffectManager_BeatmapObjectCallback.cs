using More_Modifiers.Configuration;

namespace More_Modifiers.HarmonyPatches
{
    class NoteCutSoundEffectManager_BeatmapObjectCallback
    {
        public static bool Prefix(BeatmapObjectData beatmapObjectData)
        {
            if (beatmapObjectData.beatmapObjectType != BeatmapObjectType.Note)
            {
                return false;
            }

            var noteData = beatmapObjectData as NoteData;

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
