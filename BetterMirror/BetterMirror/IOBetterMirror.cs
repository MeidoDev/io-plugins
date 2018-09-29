using BepInEx;
using BepInEx.Logging;
using Logger = BepInEx.Logger;
using Harmony;

namespace BetterMirror
{
    [BepInPlugin(GUID: "meidodev.io.better-mirror", Name: "IO BetterMirror", Version: "1.0")]
    public class IOBetterMirror : BaseUnityPlugin
    {
        public void Awake()
        {
            try
            {
                var harmony = HarmonyInstance.Create("meidodev.io.demosaic");
                harmony.PatchAll(typeof(IOBetterMirror));
            }
            catch (System.Exception e)
            {
                Logger.Log(LogLevel.Error, e.Message);
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MirrorReflection), "CreateMirrorObjects")]
        public static void MirrorReflection_CreateMirrorObjects_Pre(MirrorReflection __instance)
        {
            __instance.m_TextureSize = 2048;
        }
    }
}
