using System;
using Logger = BepInEx.Logger;
using Harmony;

namespace DynamicTranslator
{
    public static class Hooks
    {
        public static void InstallHooks()
        {
            try
            {
                var harmony = HarmonyInstance.Create("meidodev.io.translator");
                harmony.PatchAll(typeof(Hooks));
            }
            catch (System.Exception e)
            {
                Logger.Log(BepInEx.Logging.LogLevel.Error, e.Message);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ADV_Loader), "LineLoad")]
        public static void ADV_Loader_LineLoad_PostHook(ADV_Loader __instance)
        {
            __instance.Name = TextTranslator.Translate(TranslationDomains.ADV, __instance.Name);
            __instance.Text = TextTranslator.Translate(TranslationDomains.ADV, __instance.Text);
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(UILabel))]
        //[HarmonyPatch("text", PropertyMethod.Setter)]
        //public static void UILabelSetTextHook(ref string value, UILabel __instance)
        //{
        //    Logger.Log(BepInEx.Logging.LogLevel.Debug, "[" + __instance.name + "] " + value);
        //}
    }
}
