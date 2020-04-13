using More_Modifiers.Configuration;
using UnityEngine;

namespace More_Modifiers.HarmonyPatches
{
    class ColorManager_ColorForType
    {
        public static bool Prefix(ref Color __result, ref ColorScheme ____colorScheme)
        {
            switch (Config.instance.ColorToUse)
            {
                case OneColor.Right_Saber:
                    __result = ____colorScheme.saberBColor;
                    return false;
                case OneColor.Left_Saber:
                    __result = ____colorScheme.saberAColor;
                    return false;
                case OneColor.Combine:
                    __result = (____colorScheme.saberBColor + ____colorScheme.saberAColor) / 2;
                    return false;
                default:
                    return true;
            }
        }
    }
}