using BepInEx.Logging;
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
            __instance.Name = TextTranslator.Translate(TranslationDomain.D_ADV, __instance.Name);
            __instance.Text = TextTranslator.Translate(TranslationDomain.D_ADV, __instance.Text);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UILabel))]
        [HarmonyPatch("text", PropertyMethod.Setter)]
        public static void UILabelSetTextHook(ref string value, UILabel __instance)
        {
            //Logger.Log(LogLevel.Debug, string.Format("[{0}][{1}] => {2}", __instance.parent.name, __instance.name, value));
            string domain = TranslationDomain.GetLabelTranslationDomain(__instance);
            value = TextTranslator.Translate(domain, value);

        }
    }
}
