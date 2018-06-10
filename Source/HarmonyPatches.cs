using System.Reflection;
using Harmony;
using Verse;

namespace GetTogethers
{
    [StaticConstructorOnStartup]
    static public class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.cbornholdt.gettogethers");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
