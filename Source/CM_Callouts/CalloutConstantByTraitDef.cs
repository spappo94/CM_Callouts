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
    public class CalloutConstantByTraitDef : Def
    {
        public List<TraitDef> traits = new List<TraitDef>();

        public string name;
        public string value;
    }
}
