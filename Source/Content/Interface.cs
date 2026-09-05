using UnityEngine;
using Verse;
using HarmonyLib;

namespace CEtaofcDifficulty
{
    public class DifficultyModSettings : ModSettings
    {
        public float raidPointsMultiplier = 1.0f;
        public float healthMultiplier = 1.0f;
        public string currentPreset = "Normal";

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref raidPointsMultiplier, "raidPointsMultiplier", 1.0f);
            Scribe_Values.Look(ref healthMultiplier, "healthMultiplier", 1.0f);
            Scribe_Values.Look(ref currentPreset, "currentPreset", "Normal");
        }
    }

    public class DifficultyMod : Mod
    {
        public static DifficultyModSettings settings;

        public DifficultyMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<DifficultyModSettings>();

            var harmony = new Harmony("quminck.TAOFCpatchCE.DifficultySettings");
            harmony.PatchAll();
        }

        public override string SettingsCategory() => "Difficulty Settings";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label($"Current preset: {settings.currentPreset}");
            listing.Gap();

            if (listing.ButtonText("Easy"))
            {
                settings.raidPointsMultiplier = 0.75f;
                settings.healthMultiplier = 0.75f;
                settings.currentPreset = "Easy";
            }

            if (listing.ButtonText("Normal"))
            {
                settings.raidPointsMultiplier = 1.0f;
                settings.healthMultiplier = 1.0f;
                settings.currentPreset = "Normal";
            }

            if (listing.ButtonText("Hard"))
            {
                settings.raidPointsMultiplier = 1.75f;
                settings.healthMultiplier = 1.5f;
                settings.currentPreset = "Hard";
            }

            if (listing.ButtonText("Insane"))
            {
                settings.raidPointsMultiplier = 2.5f;
                settings.healthMultiplier = 2.0f;
                settings.currentPreset = "Insane";
            }

            listing.Gap();

            float oldRaidPoints = settings.raidPointsMultiplier;
            float oldHealth = settings.healthMultiplier;

            listing.Label($"Raid points multiplier: {settings.raidPointsMultiplier.ToString("F2")}");
            settings.raidPointsMultiplier = listing.Slider(settings.raidPointsMultiplier, 0.25f, 3.0f);

            listing.Gap();

            listing.Label($"Necronoid health multiplier: {settings.healthMultiplier.ToString("F2")}");
            settings.healthMultiplier = listing.Slider(settings.healthMultiplier, 0.25f, 3.0f);

            if (settings.raidPointsMultiplier != oldRaidPoints || settings.healthMultiplier != oldHealth)
            {
                settings.currentPreset = "Custom";
            }

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}