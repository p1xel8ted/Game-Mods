using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using MonoMod.Utils;
using UnityEngine;

namespace Shared;

public static class Helpers
{

    public static void PrintModLoaded(string plugin, ManualLogSource logger)
    {
        var version = Application.version.Replace("\r", "").Replace("\n", "");
        var buildGuid = Application.buildGUID;
        var platform = PlatformHelper.Current;
        var store = StorefrontDetector.DetectStorefront();

        logger.LogInfo("==========================================");
        logger.LogInfo($"  Plugin Loaded: {plugin}");
        logger.LogInfo($"  Version   : {version} (BuildGUID: {buildGuid})");
        logger.LogInfo($"  Platform  : {platform}");
        logger.LogInfo($"  Storefront: {store}");
        logger.LogInfo("==========================================");
    }

    private static class StorefrontDetector
    {
        // List of piracy marker files and folders (expand as needed)
        private static readonly string[] PiracyFiles =
        [
            // Emulators and loaders
            "SmartSteamEmu.ini", "codex.ini", "steam_emu.ini", "goldberg_emulator.dll",
            "steamclient_loader.dll", "steam_api64_o.dll", "steam_api.cdx", "steam_api64.cdx.dll",
            "steam_interfaces.txt", "local_save.txt", "valve.ini", "codex64.dll",
            "coldclient.dll", "ColdClientLoader.ini", "steamless.dll", "GreenLuma",

            // DLC unlockers
            "CreamAPI.dll", "cream_api.ini", "ScreamAPI.dll",

            // Online fixes
            "OnlineFix.dll", "OnlineFix.url", "online-fix.me",

            // Scene groups
            "CODEX", "SKIDROW", "CPY", "PLAZA", "HOODLUM", "EMPRESS", "TENOKE",
            "PROPHET", "REVOLT", "DARKSiDERS", "RAZOR1911", "FLT", "FLT.dll",
            "RUNE", "RUNE.ini", "TiNYiSO", "RELOADED", "RLD!", "DOGE", "BAT", "P2P",
            "ElAmigos", "FitGirl", "DODI", "xatab", "KaOs", "IGG", "Masquerade",

            // Common crack files
            "3dmgame.dll", "ALI213.dll", "crack", "crack.exe", "Crack.nfo",
            "crackfix", "CrackOnly", "fix.exe", "gamefix.dll", "SKIDROW.ini",
            "nosTEAM", "NoSteam", "FCKDRM", "Goldberg", "VALVEEMPRESS",

            // Installers and misc
            "unarc.dll", "uninstall.exe", "setup.exe", "INSTALLER",
            "Launcher.exe", "Redist", "ReadMe.txt", "README.nfo"
        ];


        // Simple main detection method
        public static string DetectStorefront()
        {
            var dir = Directory.GetCurrentDirectory();
            var store = "Unknown";

            // 1. Check for Steam markers
            if (File.Exists(Path.Combine(dir, "steam_api.dll")) ||
                File.Exists(Path.Combine(dir, "steam_api64.dll")) ||
                File.Exists(Path.Combine(dir, "steam_appid.txt")) ||
                Directory.Exists(Path.Combine(dir, "steam_settings")))
                store = "Steam";
            // 2. Check for GOG markers
            else if (Directory.GetFiles(dir, "goggame-*.info").Any() ||
                     File.Exists(Path.Combine(dir, "galaxy.dll")))
                store = "GOG";
            // 3. Check for Epic markers
            else if (File.Exists(Path.Combine(dir, "EOSSDK-Win64-Shipping.dll")) ||
                     File.Exists(Path.Combine(dir, "EpicOnlineServices.dll")) ||
                     Directory.Exists(Path.Combine(dir, ".egstore")))
                store = "Epic";
            // 4. Check for running launcher process as backup
            else if (IsProcessRunning("steam")) store = "Steam (process only)";
            else if (IsProcessRunning("GalaxyClient")) store = "GOG (process only)";
            else if (IsProcessRunning("EpicGamesLauncher")) store = "Epic (process only)";

            // 5. Check for piracy markers
            var isPirated = PiracyFiles.Any(pirate =>
                File.Exists(Path.Combine(dir, pirate)) ||
                Directory.Exists(Path.Combine(dir, pirate))
            );

            // 6. Report
            if (isPirated)
                store += " + Possible Pirated/Cracked Files Found!";

            return store;
        }

        // Helper for process detection
        private static bool IsProcessRunning(string name) =>
            Process.GetProcessesByName(name).Length > 0;
    }

    private static FollowerTask_ClearRubble GetRubbleTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var rubble = StructureManager.GetAllStructuresOfType<Structures_Rubble>();
        foreach (var r in rubble)
        {
            r.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_ClearRubble;
    }


    private static FollowerTask_Janitor GetJanitorTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var janitors = StructureManager.GetAllStructuresOfType<Structures_JanitorStation>();
        foreach (var janitor in janitors)
        {
            janitor.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Janitor;
    }

    private static FollowerTask_Refinery GetRefineryTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var refineries = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
        foreach (var refinery in refineries)
        {
            refinery.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Refinery;
    }

    private static FollowerTask_Undertaker GetMorgueTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var undertakers = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
        foreach (var undertaker in undertakers)
        {
            undertaker.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Undertaker;
    }

    private static void StartTask(FollowerBrain brain, FollowerTask task)
    {
        brain.CompleteCurrentTask();
        brain.HardSwapToTask(task);
    }

    private static void StartTaskFromCommand(FollowerBrain brain, FollowerCommands command)
    {
        switch (command)
        {
            case FollowerCommands.CutTrees:
                StartTask(brain, new FollowerTask_ChopTrees());
                break;
            case FollowerCommands.ClearRubble:
                StartTask(brain, GetRubbleTask());
                break;
            case FollowerCommands.WorshipAtShrine:
                StartTask(brain, GetPrayTask());
                break;
            case FollowerCommands.Farmer_2:
                StartTask(brain, GetFarmTask());
                break;
            case FollowerCommands.Build:
                StartTask(brain, GetBuildTask());
                break;
            case FollowerCommands.Cook_2:
                StartTask(brain, GetKitchenTask());
                break;
            case FollowerCommands.Janitor_2:
                StartTask(brain, GetJanitorTask());
                break;
            case FollowerCommands.Refiner_2:
                StartTask(brain, GetRefineryTask());
                break;
            case FollowerCommands.Undertaker:
                StartTask(brain, GetMorgueTask());
                break;
            case FollowerCommands.Brew:
                StartTask(brain, GetBrewTask());
                break;
            case FollowerCommands.None:
            case FollowerCommands.GiveWorkerCommand_2:
            case FollowerCommands.ChangeRole:
            case FollowerCommands.GiveItem:
            case FollowerCommands.Talk:
            case FollowerCommands.MakeDemand:
            case FollowerCommands.BedRest:
            case FollowerCommands.Murder:
            case FollowerCommands.ExtortMoney:
            case FollowerCommands.Dance:
            case FollowerCommands.DemandDevotion:
            case FollowerCommands.Gift:
            case FollowerCommands.Imprison:
            case FollowerCommands.SendToHospital:
            case FollowerCommands.ForageBerries:
            case FollowerCommands.ClearWeeds:
            case FollowerCommands.TellMeYourProblems:
            case FollowerCommands.DemandLoyalty:
            case FollowerCommands.Punish:
            case FollowerCommands.BeNice:
            case FollowerCommands.Romance:
            case FollowerCommands.WakeUp:
            case FollowerCommands.LevelUp:
            case FollowerCommands.EatSomething:
            case FollowerCommands.Sleep:
            case FollowerCommands.NoAvailablePrisons:
            case FollowerCommands.Meal:
            case FollowerCommands.MealGrass:
            case FollowerCommands.MealPoop:
            case FollowerCommands.MealGoodFish:
            case FollowerCommands.MealFollowerMeat:
            case FollowerCommands.MealGreat:
            case FollowerCommands.MealMushrooms:
            case FollowerCommands.MealMeat:
            case FollowerCommands.AreYouSure:
            case FollowerCommands.AreYouSureYes:
            case FollowerCommands.AreYouSureNo:
            case FollowerCommands.Study:
            case FollowerCommands.Intimidate:
            case FollowerCommands.Bribe:
            case FollowerCommands.Ascend:
            case FollowerCommands.Surveillance:
            case FollowerCommands.FaithEnforcer:
            case FollowerCommands.TaxEnforcer:
            case FollowerCommands.CollectTax:
            case FollowerCommands.Gift_Small:
            case FollowerCommands.Gift_Medium:
            case FollowerCommands.Gift_Necklace1:
            case FollowerCommands.Gift_Necklace2:
            case FollowerCommands.Gift_Necklace3:
            case FollowerCommands.Gift_Necklace4:
            case FollowerCommands.Gift_Necklace5:
            case FollowerCommands.Bless:
            case FollowerCommands.MealGreatFish:
            case FollowerCommands.MealBadFish:
            case FollowerCommands.RemoveNecklace:
            case FollowerCommands.Reeducate:
            case FollowerCommands.MealBerries:
            case FollowerCommands.MealMediumVeg:
            case FollowerCommands.MealMixedLow:
            case FollowerCommands.MealMixedMedium:
            case FollowerCommands.MealMixedHigh:
            case FollowerCommands.MealDeadly:
            case FollowerCommands.MealMeatLow:
            case FollowerCommands.MealMeatHigh:
            case FollowerCommands.ViewTraits:
            case FollowerCommands.NextPage:
            case FollowerCommands.PetDog:
            case FollowerCommands.Gift_Necklace_Light:
            case FollowerCommands.Gift_Necklace_Dark:
            case FollowerCommands.Gift_Necklace_Missionary:
            case FollowerCommands.Gift_Necklace_Demonic:
            case FollowerCommands.Gift_Necklace_Loyalty:
            case FollowerCommands.Gift_Necklace_Gold_Skull:
            case FollowerCommands.GiveLeaderItem:
            case FollowerCommands.MealBurnt:
            case FollowerCommands.Bully:
            case FollowerCommands.Reassure:
            case FollowerCommands.MealEgg:
            case FollowerCommands.DrinkSomething:
            case FollowerCommands.DrinkBeer:
            case FollowerCommands.DrinkGin:
            case FollowerCommands.DrinkCocktail:
            case FollowerCommands.DrinkWine:
            case FollowerCommands.DrinkMushroomJuice:
            case FollowerCommands.DrinkPoopJuice:
            case FollowerCommands.DrinkEggnog:
            case FollowerCommands.CuddleBaby:
            case FollowerCommands.HideNecklace:
            case FollowerCommands.ShowNecklace:
            case FollowerCommands.RemoveItem:
            case FollowerCommands.Hide:
            case FollowerCommands.Show:
            case FollowerCommands.SendToDaycare:
            case FollowerCommands.PetFollower:
                break;
            default:
                StartTask(brain, GetRandomTask(brain));
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
        }
    }

    private static FollowerTask_Cook GetKitchenTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var kitchens = StructureManager.GetAllStructuresOfType<Structures_Kitchen>();
        foreach (var kitchen in kitchens)
        {
            kitchen.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Cook;
    }

    private static FollowerTask GetRandomTask(FollowerBrain brain)
    {
        var task = FollowerBrain.GetDesiredTask_Work(brain.Location);
        return task.FirstOrDefault();
    }

    private static FollowerTask_Brew GetBrewTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var breweries = StructureManager.GetAllStructuresOfType<Structures_Pub>();
        foreach (var brewery in breweries)
        {
            brewery.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Brew;
    }

    private static FollowerTask_Farm GetFarmTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var farms = StructureManager.GetAllStructuresOfType<Structures_FarmerStation>();
        foreach (var farm in farms)
        {
            farm.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Farm;
    }
    
    public static bool IsMultiplierActive(float value)
    {
        return !Mathf.Approximately(value, 1.0f);
    }

    private static FollowerTask_Pray GetPrayTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var temples = StructureManager.GetAllStructuresOfType<Structures_Shrine>();
        foreach (var temple in temples)
        {
            temple.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Pray;
    }

    private static FollowerTask_Build GetBuildTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var builders = StructureManager.GetAllStructuresOfType<Structures_BuildSite>();
        foreach (var builder in builders)
        {
            builder.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }

        return tasks.Values.FirstOrDefault() as FollowerTask_Build;
    }

    internal static List<Follower> AllFollowers => FollowerManager.Followers.SelectMany(followerList => followerList.Value).ToList();

    public static IEnumerator FilterEnumerator(IEnumerator original, Type[] typesToRemove)
    {
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && !typesToRemove.Contains(current.GetType()))
            {
                yield return current;
            }
        }
    }
}