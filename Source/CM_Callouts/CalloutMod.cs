using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts
{
    public class CalloutMod : Mod
    {
        private static CalloutMod _instance;
        public static CalloutMod Instance => _instance;

        public CalloutMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("CM_Callouts");
            harmony.PatchAll();

            _instance = this;
        }
    }
}
