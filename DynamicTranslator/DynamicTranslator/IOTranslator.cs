using BepInEx;
using BepInEx.Logging;
using Logger = BepInEx.Logger;
using System;
using System.IO;
using UnityEngine;


namespace DynamicTranslator
{
    [BepInPlugin(GUID: "meidodev.io.translator", Name: "IO Translator", Version: "1.0")]
    public class IOTranslator : BaseUnityPlugin
    {
        public void Awake()
        {
            TranslationDomain.Initialize();
            LoadTranslations();
            Hooks.InstallHooks();
        }

        public void Update()
        {
            // Keypress handlers
            if (Input.GetKeyDown(KeyCode.F10))
            {
                try
                {
                    TextTranslator.Reload();
                    Logger.Log(LogLevel.Info, "Reloaded translations");
                }
                catch (TranslatorException)
                {
                    Logger.Log(LogLevel.Error, "Failed to reload translations");
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                TextTranslator.DumpMissingTranslations();
                Logger.Log(LogLevel.Info, "Dumped missing translations");
            }
        }

        protected void LoadTranslations()
        {
            string translationDir = Path.Combine(Paths.PluginPath, "translation");

            if (!Directory.Exists(translationDir))
            {
                Logger.Log(LogLevel.Debug, "Creating translation directory");
                Directory.CreateDirectory(translationDir);
            }
            else
            {
                try
                {
                    TextTranslator.Initialize(translationDir);
                }
                catch (TranslatorException)
                {
                    Logger.Log(LogLevel.Error, "Unable to initialize translator");
                }
                
            }
        }
    }
}
