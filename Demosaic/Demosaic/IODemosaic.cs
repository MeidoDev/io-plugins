using BepInEx;

namespace Demosaic
{
    [BepInPlugin(GUID: "meidodev.io.demosaic", Name: "IO Demosaic", Version: "1.0")]
    public class IODemosaic : BaseUnityPlugin
    {
        public void Awake()
        {
            Hooks.InstallHooks();
        }
    }
}
