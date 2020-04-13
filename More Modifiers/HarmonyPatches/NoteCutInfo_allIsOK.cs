namespace More_Modifiers.HarmonyPatches
{
    class ScoreController_HandleNoteWasCutEvent
    {
        public static void Prefix(ref NoteCutInfo noteCutInfo)
        {
            IPA.Utilities.ReflectionUtil.SetField<NoteCutInfo, bool>(noteCutInfo, "<saberTypeOK>k__BackingField", true);
        }
    }
}