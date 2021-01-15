using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace CM_Callouts
{
    public class CalloutTracker : WorldComponent
    {
        private int ticksBetweenCallouts = 240;
        private int checkTicks = 60;
        private int hashCache = 0;

        private Dictionary<Pawn, int> pawnCalloutExpireTick = new Dictionary<Pawn, int>();

        public CalloutTracker(World world) : base(world)
        {
            hashCache = this.GetHashCode();
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

            int currentTickPlusHash = Find.TickManager.TicksGame + hashCache;

            // Replace dictionary with new one without expired values
            if (currentTickPlusHash % checkTicks == 0)
                pawnCalloutExpireTick = pawnCalloutExpireTick.Where(kvp => kvp.Value > currentTickPlusHash).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public void AttemptCallout(Pawn pawn, RulePackDef rulePack)
        {
            if (pawn != null && pawn.def.race.Humanlike && !pawn.Dead && pawn.Spawned)
            {
                // Throw some text somewhere
                string text = GrammarResolver.Resolve(rulePack.RulesPlusIncludes[0].keyword, new GrammarRequest { Includes = { rulePack } });
                if (!text.NullOrEmpty())
                    MoteMaker.ThrowText(pawn.Position.ToVector3Shifted(), pawn.Map, text);
                else
                    Logger.WarningFormat(this, " Could not find text for requested {1} by {0}", pawn, rulePack);

                // Mark tick when pawn can callout again
                int expireTick = Find.TickManager.TicksGame + ticksBetweenCallouts + hashCache;
                if (!pawnCalloutExpireTick.ContainsKey(pawn))
                    pawnCalloutExpireTick.Add(pawn, expireTick);
                else
                    pawnCalloutExpireTick[pawn] = expireTick;
            }
        }
    }
}
