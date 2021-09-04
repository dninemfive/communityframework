using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// This class patches all the Harmony classes
    /// </summary>
    [StaticConstructorOnStartup]
    public class PatchRunner
    {
        static PatchRunner()
        {
            DoPatching();
        }

        public static void DoPatching()
        {
            //Start patching all harmony patches.
            string id = "com.communityframework.harmonypatches";
            var harmony = new Harmony(id);
            Log.Message("Community Framework patching started.");

            // Run all annotated patches.
            harmony.PatchAll();

            // There are currently no manual harmony patches. But if there
            // were any, they would go here.
        }

    }
}
