using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace More_Modifiers.Configuration
{
    class Config : PersistentSingleton<Config>
    {
        [UIValue("appearing-arrows")]
        public bool AppearingArrows;

        [UIValue("appearing-colors")]
        public bool AppearingColors;

        [UIValue("disappearing-colors")]
        public bool DisappearingColors;

        [UIValue("warp-notes")]
        public bool WarpNotes;

        [UIValue("hidden-blocks")]
        public bool HiddenBlocks;

        [UIValue("shrinking-blocks")]
        public bool ShrinkingBlocks;

        [UIValue("one-color")]
        public bool OneColor;

        [UIValue("mayhem")]
        public bool Mayhem;

        public bool ModEnabled
        {
            get
            {
                return (AppearingArrows || AppearingColors || DisappearingColors || WarpNotes || HiddenBlocks || ShrinkingBlocks || OneColor || Mayhem);
            }
        }

        [UIAction("set-one-color")]
        private void SetOneColorMode(bool value)
        {
            if (value)
            {
                Logger.log.Debug("Applying One Color patches...");
                var original = typeof(NoteBasicCutInfo).GetMethod("GetBasicCutInfo");
                var patch = typeof(HarmonyPatches.NoteBasicCutInfo_GetBasicCutInfo).GetMethod("Postfix");
                Plugin.HarmonyInstance.Patch(original, postfix: new HarmonyMethod(patch));

                original = typeof(GameNoteController).GetMethod("HandleCut");
                patch = typeof(HarmonyPatches.GameNoteController_HandleCut).GetMethod("Postfix");
                Plugin.HarmonyInstance.Patch(original, postfix: new HarmonyMethod(patch));

                original = typeof(ColorManager).GetMethod("ColorForSaberType");
                patch = typeof(HarmonyPatches.ColorManager_ColorForType).GetMethod("Prefix");
                Plugin.HarmonyInstance.Patch(original, prefix: new HarmonyMethod(patch));
                original = typeof(ColorManager).GetMethod("ColorForNoteType");
                Plugin.HarmonyInstance.Patch(original, prefix: new HarmonyMethod(patch));

                parserParams.EmitEvent("show-colors");
            }
            else
            {
                if (Harmony.HasAnyPatches("com.steven.BeatSaber.MoreModifiers"))
                {
                    Logger.log.Debug("Removing One Color patches...");
                    var original = typeof(NoteBasicCutInfo).GetMethod("GetBasicCutInfo");
                    var patch = typeof(HarmonyPatches.NoteBasicCutInfo_GetBasicCutInfo).GetMethod("Postfix");
                    Plugin.HarmonyInstance.Unpatch(original, patch);

                    original = typeof(GameNoteController).GetMethod("HandleCut");
                    patch = typeof(HarmonyPatches.GameNoteController_HandleCut).GetMethod("Postfix");
                    Plugin.HarmonyInstance.Unpatch(original, patch);

                    original = typeof(ColorManager).GetMethod("ColorForSaberType");
                    patch = typeof(HarmonyPatches.ColorManager_ColorForType).GetMethod("Prefix");
                    Plugin.HarmonyInstance.Unpatch(original, patch);
                    original = typeof(ColorManager).GetMethod("ColorForNoteType");
                    Plugin.HarmonyInstance.Unpatch(original, patch);

                    HarmonyPatches.GameNoteController_HandleCut.sabers = null;
                    HarmonyPatches.GameNoteController_HandleCut.spawner = null;
                }
            }
        }

        [UIParams]
        internal BSMLParserParams parserParams;

        [UIValue("one-color-choice")]
        public OneColor ColorToUse;

        [UIValue("list-colors")]
        public List<object> presetNJS = Enumerable.Range(0, Enum.GetNames(typeof(OneColor)).Count()).Select(x => (OneColor)x).Cast<object>().ToList();

        [UIAction("format-color")]
        public string OnFormatNJS(OneColor value) => value.ToString().Replace('_', ' ');
    }

    public enum OneColor
    {
        Right_Saber,
        Left_Saber,
        Combine
    }
}