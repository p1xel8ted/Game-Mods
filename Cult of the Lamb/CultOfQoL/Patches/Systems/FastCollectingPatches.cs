using CultOfQoL.Core;

namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class FastCollectingPatches
{
    private static GameManager GI => GameManager.GetInstance();
    private static bool CollectBedsRunning { get; set; }
    private static bool CollectAllBuildingShrinesRunning { get; set; }
    private static bool CollectAllShrinesRunning { get; set; }
    private static bool CollectAllOuthouseRunning { get; set; }
    private static bool CompostBinDeadBodyRunning { get; set; }

    private static bool CollectAllHarvestTotemsRunning { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.Update))]
    public static void BuildingShrine_Update(BuildingShrine __instance)
    {
        if (!Plugin.FastCollecting.Value) return;
        __instance.ReduceDelay = 0.0f;
        __instance.Delay = 0.0f;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.Update))]
    public static void BuildingShrinePassive_Update(BuildingShrinePassive __instance)
    {
        if (!Plugin.FastCollecting.Value) return;
        __instance.Delay = 0.0f;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Bed), nameof(Interaction_Bed.OnSecondaryInteract))]
    public static void Interaction_Bed_OnSecondaryInteract(ref Interaction_Bed __instance)
    {
        if (!Plugin.MassCollectFromBeds.Value) return;
        if (!CollectBedsRunning)
        {
            GI.StartCoroutine(CollectBeds(__instance));
        }
    }

    private static IEnumerator CollectBeds(Interaction_Bed bedInteraction)
    {
        CollectBedsRunning = true;
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_Bed bed && bed && bed != bedInteraction && bed.StructureBrain?.SoulCount > 0)
            {
                bed.StartCoroutine(bed.GiveReward());
            }
        }

        yield return new WaitForSeconds(0.10f);
        CollectBedsRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.OnInteract), typeof(StateMachine))]
    public static void BuildingShrinePassive_OnInteract(ref BuildingShrinePassive __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromPassiveShrines.Value) return;
        if (!CollectAllBuildingShrinesRunning)
        {
            GI.StartCoroutine(CollectAllBuildingShrines(__instance, state));
        }
    }

    private static IEnumerator CollectAllBuildingShrines(BuildingShrinePassive __instance, StateMachine state)
    {
        CollectAllBuildingShrinesRunning = true;
        yield return new WaitForEndOfFrame();
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is BuildingShrinePassive shrine && shrine && shrine != __instance && shrine.StructureBrain?.SoulCount > 0)
            {
                shrine.OnInteract(state);
                yield return new WaitForSeconds(0.10f);
            }
        }

        CollectAllBuildingShrinesRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.OnInteract), typeof(StateMachine))]
    public static void Interaction_Outhouse_OnInteract(ref Interaction_Outhouse __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromOuthouses.Value) return;
        if (!CollectAllOuthouseRunning)
        {
            GI.StartCoroutine(CollectAllOuthouse(__instance, state));
        }
    }

    private static IEnumerator CollectAllOuthouse(Interaction_Outhouse __instance, StateMachine state)
    {
        CollectAllOuthouseRunning = true;
        yield return new WaitForEndOfFrame();
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_Outhouse outhouse && outhouse && outhouse != __instance && outhouse.StructureBrain?.GetPoopCount() > 0)
            {
                outhouse.OnInteract(state);
                yield return new WaitForSeconds(0.10f);
            }
        }

        CollectAllOuthouseRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_CompostBinDeadBody), nameof(Interaction_CompostBinDeadBody.OnInteract), typeof(StateMachine))]
    public static void Interaction_CompostBinDeadBody_OnInteract(ref Interaction_CompostBinDeadBody __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromCompost.Value) return;
        if (!CompostBinDeadBodyRunning)
        {
            GI.StartCoroutine(CollectAllCompostBinDeadBody(__instance, state));
        }
    }

    private static IEnumerator CollectAllCompostBinDeadBody(Interaction_CompostBinDeadBody __instance, StateMachine state)
    {
        CompostBinDeadBodyRunning = true;
        yield return new WaitForEndOfFrame();
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_CompostBinDeadBody cbd && cbd && cbd != __instance && cbd.StructureBrain?.PoopCount > 0)
            {
                cbd.OnInteract(state);
                yield return new WaitForSeconds(0.10f);
            }
        }

        CompostBinDeadBodyRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HarvestTotem), nameof(HarvestTotem.OnInteract), typeof(StateMachine))]
    public static void HarvestTotem_OnInteract(ref HarvestTotem __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromHarvestTotems.Value) return;
        if (!CollectAllHarvestTotemsRunning)
        {
            GI.StartCoroutine(CollectAllHarvestTotems(__instance, state));
        }
    }

    private static IEnumerator CollectAllHarvestTotems(HarvestTotem totem, StateMachine state)
    {
        CollectAllHarvestTotemsRunning = true;
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var t in HarvestTotem.HarvestTotems.ToList())
        {
            if (t && t != totem && t.StructureBrain?.SoulCount > 0)
            {
                t.OnInteract(state);
            }
        }

        yield return new WaitForSeconds(0.10f);
        CollectAllHarvestTotemsRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_OfferingShrine), nameof(Interaction_OfferingShrine.OnInteract), typeof(StateMachine))]
    public static void Interaction_OfferingShrine_OnInteract(ref Interaction_OfferingShrine __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromOfferingShrines.Value) return;
        if (!CollectAllShrinesRunning)
        {
            GI.StartCoroutine(CollectAllShrines(__instance, state));
        }
    }

    private static IEnumerator CollectAllShrines(Interaction_OfferingShrine __instance, StateMachine state)
    {
        CollectAllShrinesRunning = true;
        yield return new WaitForEndOfFrame();

        // Snapshot the list to avoid modification during iteration
        var shrines = Interaction.interactions.ToList()
            .OfType<Interaction_OfferingShrine>()
            .Where(s => s && s != __instance && s.StructureInfo?.Inventory?.Count > 0)
            .ToList();

        Plugin.WriteLog($"[MassCollectOfferingShrines] Collecting from {shrines.Count} additional shrines");

        foreach (var shrine in shrines)
        {
            // Re-check validity before interacting
            if (!shrine || shrine.StructureInfo?.Inventory?.Count <= 0) continue;

            yield return new WaitForSeconds(0.10f);

            // Try-catch must be outside yield
            try
            {
                shrine.OnInteract(state);
            }
            catch (Exception ex)
            {
                Plugin.WriteLog($"[MassCollectOfferingShrines] Error collecting from shrine: {ex.Message}", Plugin.LogType.Error);
            }
        }

        CollectAllShrinesRunning = false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Bed), nameof(Interaction_Bed.GiveReward))]
    [HarmonyPatch(typeof(Interaction_CollectedResources), nameof(Interaction_CollectedResources.GiveResourcesRoutine))]
    private static void Interaction_Filter(ref IEnumerator __result)
    {
        if (!Plugin.FastCollecting.Value) return;
        __result = Helpers.FilterEnumerator(__result, [typeof(WaitForSeconds)]);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    private static void Interaction_CollectResourceChest_Update(ref Interaction_CollectResourceChest __instance)
    {
        if (!Plugin.EnableAutoCollect.Value) return;

        if (!__instance.playerFarming)
        {
            return;
        }

        if (!InputManager.Gameplay.GetInteractButtonHeld())
        {
            if (__instance.StructureBrain is Structures_FarmerStation && !Plugin.AutoCollectFromFarmStationChests.Value)
            {
                return;
            }

            var range = 5f;
            

            if (!Mathf.Approximately(Plugin.AutoInteractRangeMulti.Value, 1.0f))
            {
                range *= Plugin.AutoInteractRangeMulti.Value;
            }

            __instance.DistanceToTriggerDeposits = range;
            __instance.Activating = true;


            var inventoryQty = __instance.StructureInfo.Inventory.Sum(item => item.quantity);
            var emptyInventory = inventoryQty <= 0;
            var triggerHit = inventoryQty >= Plugin.TriggerAmount.Value;
            var distance = Vector3.Distance(__instance.transform.position, __instance.playerFarming.transform.position);
            var tooFarAway = distance > __instance.DistanceToTriggerDeposits;


            if (!triggerHit || emptyInventory || tooFarAway)
            {
                __instance.Activating = false;
            }

            return;
        }

        __instance.DistanceToTriggerDeposits = 5f;

        if (__instance.Activating && (__instance.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || Vector3.Distance(__instance.transform.position, __instance.playerFarming.transform.position) > __instance.DistanceToTriggerDeposits))
        {
            __instance.Activating = false;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
    private static bool LumberjackStation_Update(ref LumberjackStation __instance)
    {
        if (!Plugin.EnableAutoCollect.Value) return true;

        if (!__instance.Player)
        {
            if (!__instance.playerFarming) return true;

            __instance.Player = PlayerFarming.Instance.gameObject;
        }

        if (Plugin.EnableAutoCollect.Value && !(InputManager.Gameplay.GetInteractButtonHeld() || InputManager.Gameplay.GetInteractButtonUp()))
        {
            var range = 5f;
            
            if (!Mathf.Approximately(Plugin.AutoInteractRangeMulti.Value, 1.0f))
            {
                range *= Plugin.AutoInteractRangeMulti.Value;
            }

            __instance.DistanceToTriggerDeposits = range;


            var inventoryQty = __instance.StructureInfo.Inventory.Sum(item => item.quantity);
            var emptyInventory = inventoryQty <= 0;
            var triggerHit = inventoryQty >= Plugin.TriggerAmount.Value;
            var distance = Vector3.Distance(__instance.transform.position, __instance.Player.transform.position);
            var tooFarAway = distance > __instance.DistanceToTriggerDeposits;


            if (!triggerHit || emptyInventory || tooFarAway)
            {
                return true;
            }

            __instance.Delay -= Time.deltaTime;
            if (__instance.Delay > 0f) return false;

            var station = __instance;
            foreach (var itemType in __instance.StructureInfo.Inventory.Select(item => (InventoryItem.ITEM_TYPE)item.type))
            {
                ResourceCustomTarget.Create(__instance.Player.gameObject, __instance.transform.position, itemType, delegate { station.GiveItem(itemType); });
            }

            __instance.StructureInfo.Inventory.Clear();


            AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", __instance.transform.position);
            __instance.ChestPosition.transform.DOKill();
            __instance.ChestPosition.transform.localScale = Vector3.one;
            __instance.ChestPosition.transform.DOPunchScale(__instance.PunchScale, 1f);
            __instance.UpdateChest();
            __instance.Delay = 0.1f;
            __instance.ExhaustedCheck();

            return false;
        }

        // __instance.DistanceToTriggerDeposits = 5f;
        //
        // if (__instance.Activating && (__instance.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || Vector3.Distance(__instance.transform.position, __instance.Player.transform.position) > __instance.DistanceToTriggerDeposits))
        // {
        //     __instance.Activating = false;
        // }
        return true;
    }

    //collection speed for Interaction_EntranceShrine (dungeon shrines) - default speed is 0.1f
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_RatauShrine), nameof(Interaction_RatauShrine.Update))]
    [HarmonyPatch(typeof(Interaction_EntranceShrine), nameof(Interaction_EntranceShrine.Update))]
    [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.Update))]
    public static IEnumerable<CodeInstruction> InteractionEntranceShrineTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        var original = instructions.ToList();
        if (!Plugin.FastCollecting.Value) return original;

        try
        {
            var codes = new List<CodeInstruction>(original);
            var typeName = originalMethod.GetRealDeclaringType().Name;
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            var found = false;

            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i + 1].StoresField(delayField))
                {
                    codes[i].operand = typeName.Contains("Outhouse") ? 0.025f : 0f;
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning($"[Transpiler] {typeName}.Update: Failed to find Delay field store.");
                return original;
            }

            Plugin.Log.LogInfo($"[Transpiler] {typeName}.Update: Reduced collection delay.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}.Update: {ex.Message}");
            return original;
        }
    }

    #region Mass Scarecrow Traps

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.OnInteract))]
    public static void Scarecrow_OnInteract(Scarecrow __instance)
    {
        if (!Plugin.MassOpenScarecrows.Value)
        {
            return;
        }

        foreach (var scarecrow in Scarecrow.Scarecrows.ToList())
        {
            if (scarecrow == null || scarecrow == __instance || !scarecrow.Brain.HasBird)
            {
                continue;
            }

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
        if (mode == MassWolfTrapMode.Disabled)
        {
            return;
        }

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

            // Then fill all other empty traps
            FillAllWolfTraps(triggered, chosenItem);
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
            if (Inventory.GetItemQuantity(baitType) <= 0)
            {
                break;
            }

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

    #region Mass Animal Actions

    // List of all feed commands to check against
    private static readonly HashSet<FollowerCommands> FeedCommands = new()
    {
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
    };

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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Ranchable), nameof(Interaction_Ranchable.OnAnimalCommandFinalized))]
    public static void Interaction_Ranchable_OnAnimalCommandFinalized_Postfix(Interaction_Ranchable __instance, FollowerCommands[] followerCommands)
    {
        if (followerCommands.Length == 0)
        {
            return;
        }

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
        if (!FeedCommandToItem.TryGetValue(feedCommand, out var foodType))
        {
            return;
        }

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

    #region Mass Sin Extraction

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Follower), nameof(Follower.GiveSinToPlayer), typeof(Action))]
    public static void Follower_GiveSinToPlayer_Postfix(Follower __instance)
    {
        if (!Plugin.MassSinExtract.Value)
        {
            return;
        }

        // Find all other eligible followers and extract sin from them
        foreach (var follower in Follower.Followers.ToList())
        {
            if (!follower || follower == __instance)
            {
                continue;
            }

            if (follower.Brain == null || !follower.Brain.CanGiveSin())
            {
                continue;
            }

            // Skip if already in sin sequence
            if (follower.InGiveSinSequence)
            {
                continue;
            }

            // Start the floating animation for visual effect
            follower.SetBodyAnimation("Sin/sin-start", false);
            follower.AddBodyAnimation("Sin/sin-floating", true, 0.0f);

            // Give sin reward and reset pleasure
            GI.StartCoroutine(CollectSinFromFollower(follower));
        }
    }

    private static IEnumerator CollectSinFromFollower(Follower follower)
    {
        // Wait for the animation to get into the floating state
        yield return new WaitForSeconds(1.5f);

        // Play the collect animation
        follower.SetBodyAnimation("Sin/sin-collect", false);
        follower.AddBodyAnimation("idle", true, 0.0f);

        // Reset pleasure bar UI
        if (follower.PleasureUI?.BarController != null)
        {
            follower.PleasureUI.BarController.SetBarSize(0.0f, false, true);
        }

        // Give reward
        Inventory.AddItem(154, 1); // Sin item

        // Play sound
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/sinned_vom", follower.gameObject);

        yield return new WaitForSeconds(0.4f);

        // Reset pleasure
        follower.Brain.Info.Pleasure = 0;

        // Let the follower return to normal behavior after animation
        yield return new WaitForSeconds(1f);
        follower.Brain.CheckChangeState();
    }

    #endregion

    #region Shrine God Tears and Devotion

    /// <summary>
    /// Before vanilla processes the shrine interaction, collect any extra god tears beyond the one
    /// vanilla will handle via GiveGodTearIE. This prefix runs before any other code can modify
    /// AbilityPoints (e.g. deferred SoulCustomTarget callbacks from instant devotion collection).
    /// Only applies when vanilla would give god tears directly (all upgrades unlocked, DeathCatBeaten).
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnInteract))]
    public static void BuildingShrine_OnInteract_GodTears_Prefix(BuildingShrine __instance)
    {
        if (!Plugin.CollectAllGodTearsAtOnce.Value) return;
        if (__instance.Activating) return;
        if (GameManager.HasUnlockAvailable()) return;
        if (!DataManager.Instance.DeathCatBeaten) return;

        var total = UpgradeSystem.AbilityPoints;
        Plugin.WriteLog($"[Collect All God Tears] AbilityPoints at interaction: {total}");
        if (total <= 1) return;

        var additional = total - 1;
        Inventory.AddItem((int)InventoryItem.ITEM_TYPE.GOD_TEAR, additional);
        UpgradeSystem.AbilityPoints = 1;

        Plugin.WriteLog($"[Collect All God Tears] Pre-collected {additional} additional god tears (was {total}, left 1 for vanilla)");
    }

    /// <summary>
    /// When collecting devotion from the shrine, collect all instantly on a single tap instead of holding.
    /// Hook OnInteract - when Activating is set to true, collect all souls immediately.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnInteract))]
    public static void BuildingShrine_OnInteract_Postfix(BuildingShrine __instance)
    {
        if (!Plugin.CollectShrineDevotionInstantly.Value)
        {
            return;
        }

        if (!__instance.Activating)
        {
            return;
        }

        if (__instance.StructureBrain?.SoulCount <= 0)
        {
            return;
        }

        if (__instance.StructureBrain != null)
        {
            var totalSouls = __instance.StructureBrain.SoulCount;
            __instance.StructureBrain.SoulCount = 0;

            // Check if player has unlocks available (determines soul vs black gold)
            var hasUnlockAvailable = GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten;

            if (hasUnlockAvailable)
            {
                // Spawn visual soul effects (cap at 10 for performance, rest go directly)
                var visualSouls = Math.Min(totalSouls, 10);
                var directSouls = totalSouls - visualSouls;

                // Spawn visual souls flying to player
                for (var i = 0; i < visualSouls; i++)
                {
                    SoulCustomTarget.Create(
                        PlayerFarming.Instance.gameObject,
                        __instance.ReceiveSoulPosition.transform.position,
                        Color.white,
                        () => PlayerFarming.Instance?.GetSoul(1)
                    );
                }

                // Give remaining souls directly - must loop because GetSoul only adds 1 XP per call
                for (var i = 0; i < directSouls; i++)
                {
                    PlayerFarming.Instance?.GetSoul(1);
                }
            }
            else
            {
                // Spawn visual black gold (cap at 10 for performance)
                var visualItems = Math.Min(totalSouls, 10);
                var directItems = totalSouls - visualItems;

                for (var i = 0; i < visualItems; i++)
                {
                    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, __instance.transform.position + Vector3.back, 0.0f)
                        .SetInitialSpeedAndDiraction(8f + Random.Range(-0.5f, 1f), 270 + Random.Range(-90, 90));
                }

                // Give remaining black gold directly
                if (directItems > 0)
                {
                    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, directItems);
                }
            }
        }

        // Stop the hold-to-collect behavior since we already collected everything
        __instance.Activating = false;

        // Update the UI bar
        __instance.UpdateBar();
    }

    #endregion
}