using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;

namespace More_Modifiers
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "More Modifiers";
        internal static Harmony HarmonyInstance;

        [Init]
        public void Init(IPALogger logger)
        {
            instance = this;
            Logger.log = logger;
            Logger.log.Debug("Logger initialized.");
            HarmonyInstance = new Harmony("com.steven.BeatSaber.MoreModifiers");
        }

        [OnStart]
        public void OnApplicationStart()
        {
            new GameObject("More_ModifiersController").AddComponent<More_ModifiersController>();
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("More Modifiers", "More_Modifiers.Configuration.BSML.ModifierSettings.bsml", Configuration.Config.instance);
        }
    }
}
