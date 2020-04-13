using IPA.Utilities;
using System.Linq;
using UnityEngine;

namespace More_Modifiers.HarmonyPatches
{
    class GameNoteController_HandleCut
    {
        public static Saber[] sabers;
        public static NoteCutEffectSpawner spawner;
        const float threshold = 1f;

        public static void Postfix(GameNoteController __instance, Saber saber)
        {
            if (sabers == null) sabers = Resources.FindObjectsOfTypeAll<Saber>();
            var otherSaber = sabers.Where((x) => x != saber).First();

            if (Vector3.Distance(otherSaber.saberBladeTopPos, __instance.gameObject.transform.position) < threshold)
            {
                if (spawner == null) spawner = Resources.FindObjectsOfTypeAll<NoteCutEffectSpawner>().FirstOrDefault();
                ReflectionUtil.GetField<NoteCutHapticEffect, NoteCutEffectSpawner>(spawner, "_noteCutHapticEffect").HitNote(otherSaber.saberType);
            }
        }
    }
}