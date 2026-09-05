using HarmonyLib;
using RimWorld;
using Verse;

namespace CEtaofcDifficulty
{
	[HarmonyPatch(typeof(IncidentQueue),"Add",new[] 
    {
        typeof(IncidentDef),
        typeof(int),
        typeof(IncidentParms),
        typeof(int)
    }
	)]
    public static class Patch_NecronoidRaidPoints
    {
        public static void Prefix(IncidentDef def, IncidentParms parms)
        {
            if (DifficultyMod.settings == null || parms == null)
                return;

            if (def != IncidentDefOf.RaidEnemy)
                return;

            if (parms.faction == null || parms.faction.def == null)
                return;

            if (parms.faction.def.defName != "Necronoid")
                return;

            parms.points *= DifficultyMod.settings.raidPointsMultiplier;
        }
    }



    [HarmonyPatch(typeof(Pawn), "HealthScale", MethodType.Getter)]
    public static class Patch_NecronoidHealth
    {
        public static void Postfix(Pawn __instance, ref float __result)
        {
            if (DifficultyMod.settings == null)
                return;

            if (__instance?.def?.defName == null)
                return;

            if (!__instance.def.defName.StartsWith("Necronoid_"))
                return;

            __result *= DifficultyMod.settings.healthMultiplier;
        }
    }

}