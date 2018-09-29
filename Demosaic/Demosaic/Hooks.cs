using BepInEx.Logging;
using Logger = BepInEx.Logger;
using Harmony;
using UnityEngine;

namespace Demosaic
{
    public static class Hooks
    {
        public static void InstallHooks()
        {
            try
            {
                var harmony = HarmonyInstance.Create("meidodev.io.demosaic");
                harmony.PatchAll(typeof(Hooks));
            }
            catch (System.Exception e)
            {
                Logger.Log(LogLevel.Error, e.Message);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(TitleScript), "Awake")]
        public static void TitleScript_Awake_Post() =>
            ConfigClass.AnalMoza = false;

        [HarmonyPostfix, HarmonyPatch(typeof(CostumeSetUp_CH01), "CharacterSetUp")]
        public static void CostumeSetup_CharacterSetUp_Post(CostumeSetUp_CH01 __instance)
        {
            for (int i = 0; i < __instance.MeshObj.Count; i++)
                if (__instance.MeshObj[i].name.Contains("moza"))
                    __instance.MeshObj[i].GetComponent<Renderer>().enabled = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CostumeSetUp_CH02), "CharacterSetUp")]
        public static void CostumeSetup_CharacterSetUp_Post(CostumeSetUp_CH02 __instance)
        {
            for (int i = 0; i < __instance.MeshObj.Count; i++)
                if (__instance.MeshObj[i].name.Contains("moza"))
                    __instance.MeshObj[i].GetComponent<Renderer>().enabled = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CostumeSetUp_PC), "CharacterSetUp")]
        public static void CostumeSetup_CharacterSetUp_Post(CostumeSetUp_PC __instance)
        {
            for (int i = 0; i < __instance.MeshObj.Count; i++)
                if (__instance.MeshObj[i].name.Contains("moza"))
                    __instance.MeshObj[i].GetComponent<Renderer>().enabled = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CostumeSetUp_SY), "CharacterSetUp")]
        public static void CostumeSetup_CharacterSetUp_Post(CostumeSetUp_SY __instance)
        {
            for (int i = 0; i < __instance.MeshObj.Count; i++)
                if (__instance.MeshObj[i].name.Contains("moza"))
                    __instance.MeshObj[i].GetComponent<Renderer>().enabled = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MozaicSetUp), "Start")]
        public static void MozaicSetUp_Start_Post(MozaicSetUp __instance)
        {
            __instance.MozaObj.enabled = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(DanmenPixel), "Start")]
        public static void DanmenPixel_Start_Post()
        {
            Renderer r1 = GameObject.Find("PC00/PC0000/PC00_ute05_moza").gameObject.GetComponent<Renderer>();
            if (r1)
                r1.enabled = false;

            Renderer r2 = GameObject.Find("PC00/PC0000/PC00_ute05_moza_ANA").gameObject.GetComponent<Renderer>();
            if (r2)
                r2.enabled = false;
        }
    }
}
