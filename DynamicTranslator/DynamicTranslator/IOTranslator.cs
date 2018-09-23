using BepInEx;
using BepInEx.Logging;
using Logger = BepInEx.Logger;
using System;
using System.IO;


namespace DynamicTranslator
{
    [BepInPlugin(GUID: "meidodev.io.translator", Name: "IO Translator", Version: "1.0")]
    public class IOTranslator : BaseUnityPlugin
    {
        public void Awake()
        {
            Hooks.InstallHooks();
            LoadTranslations();
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
                TextTranslator.Initialize(translationDir);
            }
        }
    }
}
