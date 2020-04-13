namespace More_Modifiers.HarmonyPatches
{
    class NoteBasicCutInfo_GetBasicCutInfo
    {
        public static void Postfix(NoteBasicCutInfo __instance, ref bool saberTypeOK) => saberTypeOK = true;
    }
}