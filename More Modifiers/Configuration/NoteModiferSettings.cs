using UnityEngine;

namespace More_Modifiers.Configuration
{
    class NoteModifierSettings
    {
        public bool AppearingArrows;
        public bool AppearingColors;
        public bool DisappearingColors;
        public bool WarpNotes;
        public bool OneColor;
        public bool HiddenBlocks;
        public bool ShrinkingBlocks;

        // Base Game
        public bool DisappearingArrows;
        public bool GhostNotes;

        public NoteModifierSettings()
        {
            if (Config.instance.Mayhem)
            {
                switch (Random.Range(0, 9))
                {
                    case 0:
                        AppearingArrows = true;
                        break;
                    case 1:
                        DisappearingArrows = true;
                        break;
                    case 2:
                        AppearingColors = true;
                        break;
                    case 3:
                        DisappearingColors = true;
                        break;
                    case 4:
                        WarpNotes = true;
                        break;
                    case 5:
                        HiddenBlocks = true;
                        break;
                    case 6:
                        GhostNotes = true;
                        break;
                    case 7:
                        ShrinkingBlocks = true;
                        break;
                    case 8:
                        break;
                }
            }
            else
            {
                AppearingArrows = Config.instance.AppearingArrows;
                AppearingColors = Config.instance.AppearingColors;
                DisappearingColors = Config.instance.DisappearingColors;
                WarpNotes = Config.instance.WarpNotes;
                OneColor = Config.instance.OneColor;
                HiddenBlocks = Config.instance.HiddenBlocks;
                ShrinkingBlocks = Config.instance.ShrinkingBlocks;
            }
        }

        //public static NoteModifierSettings GetRandomSettingsForNote()
        //{
        //    var appearingArrows = RandomToggle(3);
        //    var disappearingArrows = (appearingArrows) ? false : RandomToggle(3);
        //    var appearingColors = RandomToggle(3);
        //    var disappearingColors = (appearingColors) ? false : RandomToggle(3);
        //    var warpNotes = RandomToggle(3);
        //    var hiddenBlocks = (warpNotes) ? false : RandomToggle(3);
        //    var ghostNotes = RandomToggle(3);

        //    return new NoteModifierSettings
        //    {
        //        AppearingArrows = appearingArrows,
        //        AppearingColors = appearingColors,
        //        DisappearingColors = disappearingColors,
        //        WarpNotes = warpNotes,
        //        OneColor = false,
        //        HiddenBlocks = hiddenBlocks,
        //        DisappearingArrows = disappearingArrows,
        //        GhostNotes = ghostNotes
        //    };
        //}
    }
}
