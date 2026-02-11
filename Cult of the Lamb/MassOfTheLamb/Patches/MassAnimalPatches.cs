using MassOfTheLamb.Core;

namespace MassOfTheLamb.Patches;

[Harmony]
public static class MassAnimalPatches
{
    private static GameManager GI => GameManager.GetInstance();

    // Flag to prevent infinite recursion when mass petting animals
    private static bool _massPetAnimalsInProgress;

    #region Feed Command Mappings

    // List of all feed commands to check against
    internal static readonly HashSet<FollowerCommands> FeedCommands =
    [
        FollowerCommands.FeedGrass,
        FollowerCommands.FeedPoop,
        FollowerCommands.FeedFollowerMeat,
        FollowerCommands.FeedBerry,
        FollowerCommands.FeedFlowerRed,
        FollowerCommands.FeedMushroom,
        FollowerCommands.FeedPumpkin,
        FollowerCommands.FeedBeetroot,
        FollowerCommands.FeedCauliflower,
        FollowerCommands.FeedGrapes,
        FollowerCommands.FeedHops,
        FollowerCommands.FeedFish,
        FollowerCommands.FeedFishBig,
        FollowerCommands.FeedFishBlowfish,
        FollowerCommands.FeedFishCrab,
        FollowerCommands.FeedFishLobster,
        FollowerCommands.FeedFishOctopus,
        FollowerCommands.FeedFishSmall,
        FollowerCommands.FeedFishSquid,
        FollowerCommands.FeedFishSwordfish,
        FollowerCommands.FeedFishCod,
        FollowerCommands.FeedFishCatfish,
        FollowerCommands.FeedFishPike,
        FollowerCommands.FeedMeat,
        FollowerCommands.FeedMeatMorsel,
        FollowerCommands.FeedYolk,
        FollowerCommands.FeedChilli
    ];

    // Map feed commands to item types
    private static readonly Dictionary<FollowerCommands, InventoryItem.ITEM_TYPE> FeedCommandToItem = new()
    {
        { FollowerCommands.FeedGrass, InventoryItem.ITEM_TYPE.GRASS },
        { FollowerCommands.FeedPoop, InventoryItem.ITEM_TYPE.POOP },
        { FollowerCommands.FeedFollowerMeat, InventoryItem.ITEM_TYPE.FOLLOWER_MEAT },
        { FollowerCommands.FeedBerry, InventoryItem.ITEM_TYPE.BERRY },
        { FollowerCommands.FeedFlowerRed, InventoryItem.ITEM_TYPE.FLOWER_RED },
        { FollowerCommands.FeedMushroom, InventoryItem.ITEM_TYPE.MUSHROOM_BIG },
        { FollowerCommands.FeedPumpkin, InventoryItem.ITEM_TYPE.PUMPKIN },
        { FollowerCommands.FeedBeetroot, InventoryItem.ITEM_TYPE.BEETROOT },
        { FollowerCommands.FeedCauliflower, InventoryItem.ITEM_TYPE.CAULIFLOWER },
        { FollowerCommands.FeedGrapes, InventoryItem.ITEM_TYPE.GRAPES },
        { FollowerCommands.FeedHops, InventoryItem.ITEM_TYPE.HOPS },
        { FollowerCommands.FeedFish, InventoryItem.ITEM_TYPE.FISH },
        { FollowerCommands.FeedFishBig, InventoryItem.ITEM_TYPE.FISH_BIG },
        { FollowerCommands.FeedFishBlowfish, InventoryItem.ITEM_TYPE.FISH_BLOWFISH },
        { FollowerCommands.FeedFishCrab, InventoryItem.ITEM_TYPE.FISH_CRAB },
        { FollowerCommands.FeedFishLobster, InventoryItem.ITEM_TYPE.FISH_LOBSTER },
        { FollowerCommands.FeedFishOctopus, InventoryItem.ITEM_TYPE.FISH_OCTOPUS },
        { FollowerCommands.FeedFishSmall, InventoryItem.ITEM_TYPE.FISH_SMALL },
        { FollowerCommands.FeedFishSquid, InventoryItem.ITEM_TYPE.FISH_SQUID },
        { FollowerCommands.FeedFishSwordfish, InventoryItem.ITEM_TYPE.FISH_SWORDFISH },
        { FollowerCommands.FeedFishCod, InventoryItem.ITEM_TYPE.FISH_COD },
        { FollowerCommands.FeedFishCatfish, InventoryItem.ITEM_TYPE.FISH_CATFISH },
        { FollowerCommands.FeedFishPike, InventoryItem.ITEM_TYPE.FISH_PIKE },
        { FollowerCommands.FeedMeat, InventoryItem.ITEM_TYPE.MEAT },
        { FollowerCommands.FeedMeatMorsel, InventoryItem.ITEM_TYPE.MEAT_MORSEL },
        { FollowerCommands.FeedYolk, InventoryItem.ITEM_TYPE.YOLK },
        { FollowerCommands.FeedChilli, InventoryItem.ITEM_TYPE.CHILLI }
    };

    #endregion

    #region Mass Animal Actions (Clean/Feed/Milk/Shear)

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Ranchable), nameof(Interaction_Ranchable.OnAnimalCommandFinalized))]
    public static void Interaction_Ranchable_OnAnimalCommandFinalized_Postfix(Interaction_Ranchable __instance, FollowerCommands[] followerCommands)
    {
        if (followerCommands.Length == 0) return;

        var command = followerCommands[0];

        // Mass Clean
        if (Plugin.MassCleanAnimals.Value && command == FollowerCommands.Clean)
        {
            MassCleanAllAnimals(__instance);
        }
        // Mass Feed
        else if (Plugin.MassFeedAnimals.Value && FeedCommands.Contains(command))
        {
            MassFeedAllAnimals(__instance, command);
        }
        // Mass Milk
        else if (Plugin.MassMilkAnimals.Value && command == FollowerCommands.MilkAnimal)
        {
            MassMilkAllAnimals(__instance);
        }
        // Mass Shear (Harvest command is used for shearing)
        else if (Plugin.MassShearAnimals.Value && command == FollowerCommands.Harvest)
        {
            MassShearAllAnimals(__instance);
        }
    }

    private static void MassCleanAllAnimals(Interaction_Ranchable triggeredAnimal)
    {
        var stinkyAnimals = Interaction_Ranchable.Ranchables
            .Where(a => a && a != triggeredAnimal && a.Animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
            .ToList();
        if (stinkyAnimals.Count == 0 || !MassActionCosts.TryDeductCosts(stinkyAnimals.Count)) return;

        foreach (var animal in stinkyAnimals)
        {
            animal.Clean(false);
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalNotStinky", animal.Animal.GetName());
        }
    }

    private static void MassFeedAllAnimals(Interaction_Ranchable triggeredAnimal, FollowerCommands feedCommand)
    {
        if (!FeedCommandToItem.TryGetValue(feedCommand, out var foodType)) return;

        // Count how many we can feed (excluding the one already fed by vanilla)
        var hungryAnimals = Interaction_Ranchable.Ranchables
            .Where(a => a && a != triggeredAnimal && !a.Animal.EatenToday)
            .ToList();
        if (hungryAnimals.Count == 0 || !MassActionCosts.TryDeductCosts(hungryAnimals.Count)) return;

        // Feed as many as we have inventory for
        var availableFood = Inventory.GetItemQuantity(foodType);
        var fedCount = 0;

        foreach (var animal in hungryAnimals.TakeWhile(_ => availableFood > 0))
        {
            // Set player reference so Feed() can use it for animations
            animal._playerFarming = PlayerFarming.Instance ??= Object.FindObjectOfType<PlayerFarming>();

            // Call the game's Feed method which handles everything:
            // - Sets EatenToday, moveTimer, Satiation
            // - Plays audio, completes objectives
            // - Handles special food effects (poop/follower meat)
            // - Updates happy state
            // - Consumes inventory item
            // - Spawns food visual flying to animal
            // - Plays eating animation
            animal.Feed(foodType);

            availableFood--;
            fedCount++;
        }

        if (fedCount > 0)
        {
            NotificationCentre.Instance.PlayGenericNotification($"Fed {fedCount} additional animal{(fedCount > 1 ? "s" : "")}");
        }
    }

    private static void MassMilkAllAnimals(Interaction_Ranchable triggeredAnimal)
    {
        var milkableAnimals = Interaction_Ranchable.Ranchables
            .Where(a => a && a != triggeredAnimal && !a.Animal.MilkedToday && a.Animal.MilkedReady)
            .ToList();
        if (milkableAnimals.Count == 0 || !MassActionCosts.TryDeductCosts(milkableAnimals.Count)) return;

        foreach (var animal in milkableAnimals)
        {
            animal.MilkAnimal();
            AudioManager.Instance.PlayOneShot("event:/dlc/animal/cow/milk", animal.transform.position);
        }

        NotificationCentre.Instance.PlayGenericNotification($"Milked {milkableAnimals.Count} additional animal{(milkableAnimals.Count > 1 ? "s" : "")}");
    }

    private static void MassShearAllAnimals(Interaction_Ranchable triggeredAnimal)
    {
        var shearableAnimals = Interaction_Ranchable.Ranchables
            .Where(a => a && a != triggeredAnimal && !a.Animal.WorkedToday && a.Animal.WorkedReady)
            .ToList();
        if (shearableAnimals.Count == 0 || !MassActionCosts.TryDeductCosts(shearableAnimals.Count)) return;

        foreach (var animal in shearableAnimals)
        {
            animal.Work();
            AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/shear", animal.transform.position);
        }

        NotificationCentre.Instance.PlayGenericNotification($"Sheared {shearableAnimals.Count} additional animal{(shearableAnimals.Count > 1 ? "s" : "")}");
    }

    #endregion

    #region Mass Pet Animals

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Ranchable), nameof(Interaction_Ranchable.OnAnimalCommandFinalized))]
    public static void Interaction_Ranchable_OnAnimalCommandFinalized_PetAnimals(
        ref Interaction_Ranchable __instance,
        params FollowerCommands[] followerCommands)
    {
        if (!Plugin.MassPetAnimals.Value) return;
        if (_massPetAnimalsInProgress) return;
        if (followerCommands.Length == 0 || followerCommands[0] != FollowerCommands.PetAnimal) return;

        GI.StartCoroutine(PetAllAnimals(__instance));
    }

    private static IEnumerator PetAllAnimals(Interaction_Ranchable pettedAnimal)
    {
        _massPetAnimalsInProgress = true;
        yield return new WaitForEndOfFrame();

        try
        {
            // Get all ranchable animals that haven't been petted today
            var animals = Interaction.interactions
                .OfType<Interaction_Ranchable>()
                .Where(r => r && r != pettedAnimal && !r.animal.PetToday)
                .ToList();

            if (animals.Count == 0 || !MassActionCosts.TryDeductCosts(animals.Count))
            {
                yield break;
            }

            Plugin.WriteLog($"[MassPetAnimals] Petting {animals.Count} additional animals");

            foreach (var animal in animals)
            {
                if (!animal || animal.animal.PetToday) continue;

                // Apply pet effects directly instead of calling PetIE
                // This avoids the coroutine's player-locking animation and long delays
                animal.AddAdoration(50f);
                animal.animal.PetToday = true;

                // Visual and audio feedback
                BiomeConstants.Instance.EmitHeartPickUpVFX(animal.transform.position, 0f, "red", "burst_small");
                AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", animal.transform.position);
            }
        }
        finally
        {
            _massPetAnimalsInProgress = false;
        }
    }

    #endregion

    #region Mass Scarecrow Traps

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.OnInteract))]
    public static void Scarecrow_OnInteract(Scarecrow __instance)
    {
        if (!Plugin.MassOpenScarecrows.Value) return;

        var eligibleScarecrows = Scarecrow.Scarecrows
            .Where(s => s != null && s != __instance && s.Brain.HasBird)
            .ToList();
        if (eligibleScarecrows.Count == 0 || !MassActionCosts.TryDeductCosts(eligibleScarecrows.Count)) return;

        foreach (var scarecrow in eligibleScarecrows)
        {
            scarecrow.OpenTrap();
            scarecrow.Brain.EmptyTrap();
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, Random.Range(2, 5), scarecrow.transform.position);
        }
    }

    #endregion

    #region Mass Wolf Traps

    // Track if the trap had a wolf when interaction started (before game clears it)
    private static bool _wolfTrapHadWolf;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_WolfTrap), nameof(Interaction_WolfTrap.OnInteract))]
    public static void Interaction_WolfTrap_OnInteract_Prefix(Interaction_WolfTrap __instance)
    {
        // Capture state BEFORE the game modifies it
        _wolfTrapHadWolf = __instance.structure.Brain.Data.HasBird;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_WolfTrap), nameof(Interaction_WolfTrap.OnInteract))]
    public static void Interaction_WolfTrap_OnInteract_Postfix(Interaction_WolfTrap __instance)
    {
        var mode = Plugin.MassWolfTraps.Value;
        if (mode == MassWolfTrapMode.Disabled) return;

        // Check if this was a collect action (trap HAD a wolf before game cleared it)
        if (_wolfTrapHadWolf)
        {
            // Collect from all other traps with caught wolves
            if (mode is MassWolfTrapMode.CollectOnly or MassWolfTrapMode.Both)
            {
                CollectAllWolfTraps(__instance);
            }
            return;
        }

        // This is a fill action - hook into the item selector
        if (mode is MassWolfTrapMode.FillOnly or MassWolfTrapMode.Both)
        {
            GI.StartCoroutine(HookBaitSelection(__instance));
        }
    }

    private static IEnumerator HookBaitSelection(Interaction_WolfTrap triggered)
    {
        yield return new WaitForEndOfFrame();

        // Find the UIItemSelectorOverlayController from the static list
        if (UIItemSelectorOverlayController.SelectorOverlays.Count == 0)
        {
            yield break;
        }

        // Get the most recently added selector
        var selector = UIItemSelectorOverlayController.SelectorOverlays[UIItemSelectorOverlayController.SelectorOverlays.Count - 1];
        if (selector == null)
        {
            yield break;
        }

        // Store original callback and wrap it
        var originalCallback = selector.OnItemChosen;

        selector.OnItemChosen = chosenItem =>
        {
            // Call original first (for the triggered trap)
            originalCallback?.Invoke(chosenItem);

            // Then fill all other empty traps (if costs are met)
            var emptyTraps = Interaction_WolfTrap.Traps
                .Count(t => t != null && t != triggered &&
                            !t.structure.Brain.Data.HasBird &&
                            t.structure.Brain.Data.Inventory.Count == 0);
            if (emptyTraps > 0 && MassActionCosts.TryDeductCosts(emptyTraps))
            {
                FillAllWolfTraps(triggered, chosenItem);
            }
        };
    }

    private static void FillAllWolfTraps(Interaction_WolfTrap triggered, InventoryItem.ITEM_TYPE baitType)
    {
        var emptyTraps = Interaction_WolfTrap.Traps
            .Where(t => t != null && t != triggered &&
                        !t.structure.Brain.Data.HasBird &&
                        t.structure.Brain.Data.Inventory.Count == 0)
            .ToList();

        foreach (var trap in emptyTraps)
        {
            // Check player has bait
            if (Inventory.GetItemQuantity(baitType) <= 0) break;

            // Deposit bait
            Inventory.ChangeItemQuantity((int)baitType, -1);
            trap.structure.Brain.Data.Inventory.Add(new InventoryItem(baitType, 1));
            trap.UpdateVisuals();
        }
    }

    private static void CollectAllWolfTraps(Interaction_WolfTrap triggered)
    {
        var trapsWithWolves = Interaction_WolfTrap.Traps
            .Where(t => t != null && t != triggered &&
                        t.structure.Brain.Data.HasBird)
            .ToList();
        if (trapsWithWolves.Count == 0 || !MassActionCosts.TryDeductCosts(trapsWithWolves.Count)) return;

        foreach (var trap in trapsWithWolves)
        {
            // Match game's OnInteract behavior exactly:
            trap.structure.Brain.Data.HasBird = false;
            trap.UpdateVisuals();

            // Spawn 2-5 individual meat items with random offsets (same as game)
            var meatCount = Random.Range(2, 6);
            for (var i = 0; i < meatCount; i++)
            {
                InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, 1, trap.transform.position + (Vector3)Random.insideUnitCircle);
            }

            // Play the trap open sound
            AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/trap_open", trap.transform.position);
        }
    }

    #endregion
}
