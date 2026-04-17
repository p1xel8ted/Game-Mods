namespace GerrysJunkTrunk;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal const float FullPriceModifier = 0.70f;
    internal const float PityPrice = 0.10f;
    internal const int LargeInvSize = 20;
    internal const int LargeMaxItemCount = 100;
    internal const string ModGerryTag = "mod_gerry";
    internal const float PriceModifier = 0.60f;
    internal const string ShippingBoxTag = "shipping_box";
    internal const string ShippingItem = "shipping";
    internal const int SmallInvSize = 10;
    internal const int SmallMaxItemCount = 50;
    internal const string ShippingBoxId = "mf_wood_builddesk:p:mf_shipping_box_place";

    internal static int _techCount;
    internal static int _oldTechCount;
    internal static readonly Dictionary<string, int> StackSizeBackups = new();

    internal static WorldGameObject _shippingBox;
    internal static WorldGameObject _interactedObject;
    internal static bool _shippingBuild;
    internal static bool _usingShippingBox;
    internal static bool _cinematicPlaying;
    internal static Transform _cinematicCameraTarget;
    internal static float _cinematicStartedAt;

    // Watchdog upper bound for the gerry routine. Routine is hard-capped at 20s by the
    // existing safety-net timer; anything past 25s means the timer chain broke (sleep,
    // save-load, scene unload, NPC dialog stomp) and HUD/control are stranded.
    internal const float CinematicMaxDurationSeconds = 25f;

    internal static readonly List<BaseItemCellGUI> AlreadyDone = [];
    internal static ObjectCraftDefinition NewItem { get; private set; }

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ManualLogSource Log { get; set; }
    internal static ConfigEntry<bool> ShowSoldMessagesOnPlayer { get; private set; }
    internal static ConfigEntry<bool> EnableGerry { get; private set; }
    internal static ConfigEntry<bool> CinematicMode { get; private set; }
    internal static ConfigEntry<bool> ShowItemPriceTooltips { get; private set; }
    internal static ConfigEntry<bool> InternalShippingBoxBuilt { get; private set; }
    internal static ConfigEntry<bool> InternalShowIntroMessage { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static readonly ItemDefinition.ItemType[] ExcludeItems =
    [
        ItemDefinition.ItemType.Axe, ItemDefinition.ItemType.Shovel, ItemDefinition.ItemType.Hammer,
        ItemDefinition.ItemType.Pickaxe, ItemDefinition.ItemType.FishingRod, ItemDefinition.ItemType.BodyArmor,
        ItemDefinition.ItemType.HeadArmor, ItemDefinition.ItemType.Sword, ItemDefinition.ItemType.Preach,
        ItemDefinition.ItemType.GraveStone, ItemDefinition.ItemType.GraveFence, ItemDefinition.ItemType.GraveCover,
        ItemDefinition.ItemType.GraveStoneReq, ItemDefinition.ItemType.GraveFenceReq,
        ItemDefinition.ItemType.GraveCoverReq
    ];

    internal static void SetNewItem(ObjectCraftDefinition value) => NewItem = value;

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitInternalConfiguration();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void InitInternalConfiguration()
    {
        InternalShippingBoxBuilt = Config.Bind("Internal (Dont Touch)", "Shipping Box Built", false,
            new ConfigDescription("Internal use.", null,
                new ConfigurationManagerAttributes
                    {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 497}));
        InternalShowIntroMessage = Config.Bind("Internal (Dont Touch)", "Show Intro Message", false,
            new ConfigDescription("Internal use.", null,
                new ConfigurationManagerAttributes
                    {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 496}));
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind("00. Advanced", "Debug Logging", false,
            new ConfigDescription("Toggle debug logging on or off", null,
                new ConfigurationManagerAttributes {Order = 0}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        EnableGerry = Config.Bind("01. Gerry", "Gerry", true,
            new ConfigDescription("Toggle Gerry", null, new ConfigurationManagerAttributes {Order = 6}));

        CinematicMode = Config.Bind("01. Gerry", "Cinematic Mode", true,
            new ConfigDescription("When on, the camera focuses on Gerry and you can't move during his visit. When off, Gerry still appears and speaks but the game keeps running normally around you.", null,
                new ConfigurationManagerAttributes {Order = 5, DispName = "    \u2514 Cinematic Mode"}));
        CinematicMode.SettingChanged += (_, _) =>
        {
            // Toggling off mid-cinematic should restore the HUD straight away — the user
            // is most likely flipping it precisely because they're staring at a gone HUD.
            if (!CinematicMode.Value && _cinematicPlaying)
            {
                if (DebugEnabled) WriteLog("[CinematicMode] toggled off mid-cinematic — restoring HUD now");
                HideCinematic();
            }
        };

        ShowSoldMessagesOnPlayer = Config.Bind("02. Messages", "Show Sold Messages On Player", true,
            new ConfigDescription("Display messages on the player when items are sold", null,
                new ConfigurationManagerAttributes {Order = 5}));

        ShowItemPriceTooltips = Config.Bind("03. Price Tooltips", "Show Item Price Tooltips", true,
            new ConfigDescription("Display tooltips with item prices in the user interface", null,
                new ConfigurationManagerAttributes {Order = 2}));

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    internal static void ShowCinematic(Transform transform)
    {
        if (_cinematicPlaying) return;
        // Cinematic Mode off: Gerry still spawns and speaks, but the game keeps running
        // around the player — no HUD hide, no player freeze, no camera takeover. The
        // _cinematicPlaying flag stays false so HideCinematic also skips, and the watchdog
        // never fires (nothing to recover).
        if (!CinematicMode.Value) return;
        _cinematicPlaying = true;
        _cinematicCameraTarget = transform;
        _cinematicStartedAt = Time.time;
        GS.AddCameraTarget(transform);
        GS.SetPlayerEnable(false, true);
    }

    internal static void HideCinematic()
    {
        if (!_cinematicPlaying) return;
        _cinematicPlaying = false;
        if (_cinematicCameraTarget != null)
        {
            GS.RemoveCameraTarget(_cinematicCameraTarget);
            _cinematicCameraTarget = null;
        }
        GS.AddCameraTarget(MainGame.me.player.transform);
        GS.SetPlayerEnable(true, true);
    }

    internal static void ClearGerryFlag(ChestGUI chestGui)
    {
        if (chestGui == null || !_usingShippingBox || StackSizeBackups.Count <= 0) return;

        var inventories = new[]
        {
            chestGui.player_panel.multi_inventory.all[0].data.inventory,
            chestGui.chest_panel.multi_inventory.all[0].data.inventory
        };

        foreach (var item in inventories.SelectMany(inventory => inventory))
        {
            if (StackSizeBackups.TryGetValue(item.id, out var value))
            {
                item.definition.stack_count = value;
            }
        }

        _usingShippingBox = false;
    }

    internal static float GetBoxEarnings(WorldGameObject shippingBox)
    {
        return shippingBox.data.inventory.Sum(GetItemEarnings);
    }

    internal static float GetItemEarnings(Item selectedItem)
    {
        var totalSalePrice = 0f;
        var totalCount = selectedItem.value;
        if (selectedItem.definition.base_price <= 0)
        {
            var lastChar = selectedItem.id[selectedItem.id.Length - 1];
            var multiplier = lastChar switch
            {
                '3' => 0.75f,
                '2' => 0.60f,
                '1' => 0.45f,
                _ => 0.25f
            };

            totalSalePrice += multiplier * totalCount;
            if (DebugEnabled) WriteLog($"Item: {selectedItem.id}, Multiplier: {multiplier}, total item count: {totalCount}, total item cost: {totalCount * multiplier}");
        }
        else
        {
            var singleItemCost = selectedItem.definition.GetPrice(totalCount);
            if (singleItemCost > selectedItem.definition.base_price)
            {
                singleItemCost = selectedItem.definition.base_price;
            }
            totalSalePrice += singleItemCost * totalCount;
            if (DebugEnabled) WriteLog($"Item: {selectedItem.id}, Single item cost: {singleItemCost}, total item count: {totalCount}, total item cost: {totalCount * singleItemCost}");
        }

        if (totalSalePrice <= 0)
        {
            var price = PityPrice * totalCount;
            return ApplyPriceModifier(price);
        }

        return ApplyPriceModifier(totalSalePrice);

        float ApplyPriceModifier(float price)
        {
            return UnlockedFullPrice() ? price * FullPriceModifier : price * PriceModifier;
        }
    }

    internal static void ShowIntroMessage()
    {
        GUIElements.me.dialog.OpenOK(Lang.Get("Message1"), null,
            $"{Lang.Get("Message2")}\n{Lang.Get("Message3")}\n{Lang.Get("Message4")}\n{Lang.Get("Message5")}\n{Lang.Get("Message6")}\n{Lang.Get("Message7")}",
            true, Lang.Get("Message8"));
    }

    internal static void StartGerryRoutine(float num)
    {
        Lang.Reload();
        var noSales = num <= 0;
        var money = Trading.FormatMoney(num, true);
        var gerry = SpawnGerry(_shippingBox.transform, _shippingBox.pos3);
        ShowCinematic(gerry.transform);
        GJTimer.AddTimer(2f,
            delegate
            {
                gerry.Say(noSales ? Lang.Get("Nothing") : Lang.Get("WorkWork"), delegate
                {
                    DestroyGerryWithDelay(gerry, 1f);
                });
            });

        if (noSales)
        {
            HideCinematic();
            return;
        }

        GJTimer.AddTimer(8f, delegate
        {
            var gerry2 = SpawnGerry(_shippingBox.transform, _shippingBox.pos3);
            ShowCinematic(gerry2.transform);
            GJTimer.AddTimer(2f, delegate
            {
                gerry2.Say($"{money}", delegate
                {
                    PlayCoinsSoundAndShowMessage(num, money);

                    GJTimer.AddTimer(2f, delegate
                    {
                        gerry2.Say(Lang.Get("Bye"), delegate
                        {
                            DestroyGerryWithDelay(gerry2, 1f);
                        });
                    });
                });
            });
        });

        GJTimer.AddTimer(20f, delegate
        {
            HideCinematic();
        });
    }

    private static WorldGameObject SpawnGerry(Transform parent, Vector3 pos)
    {
        var spawnPos = new Vector3(pos.x, pos.y + 43f, pos.z);
        var gerry = WorldMap.SpawnWGO(parent, "talking_skull", spawnPos);
        gerry.tag = ModGerryTag;
        gerry.custom_tag = ModGerryTag;
        gerry.ReplaceWithObject("talking_skull", true);
        gerry.tag = ModGerryTag;
        gerry.custom_tag = ModGerryTag;

        return gerry;
    }

    private static void DestroyGerryWithDelay(WorldGameObject gerry, float delay)
    {
        GJTimer.AddTimer(delay, delegate
        {
            gerry.ReplaceWithObject("talking_skull", true);
            gerry.tag = ModGerryTag;
            gerry.custom_tag = ModGerryTag;
            gerry.DestroyMe();
            HideCinematic();
        });
    }

    internal static void PlayCoinsSoundAndShowMessage(float num, string money)
    {
        var position = ShowSoldMessagesOnPlayer.Value
            ? MainGame.me.player_pos + new Vector3(0, 125f, 0)
            : _shippingBox.pos3 + new Vector3(0, 100f, 0);
        Sounds.PlaySound("coins_sound", ShowSoldMessagesOnPlayer.Value ? MainGame.me.player_pos : _shippingBox.pos3, true);
        EffectBubblesManager.ShowImmediately(position, $"{money}",
            num > 0 ? EffectBubblesManager.BubbleColor.Green : EffectBubblesManager.BubbleColor.Red,
            true, ShowSoldMessagesOnPlayer.Value ? 4f : 7f);
    }

    internal static void TryAdd<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key)) return;
        dictionary.Add(key, value);
    }

    internal static bool UnlockedFullPrice()
    {
        return UnlockedShippingBoxExpansion() &&
               MainGame.me.save.unlocked_techs.Exists(
                   a => a.ToLowerInvariant().Equals("Best friend".ToLowerInvariant()));
    }

    internal static bool UnlockedShippingBox()
    {
        return MainGame.me.save.unlocked_techs.Exists(a =>
            a.ToLowerInvariant().Equals("Wood processing".ToLowerInvariant()));
    }

    internal static bool UnlockedShippingBoxExpansion()
    {
        return UnlockedShippingBox() &&
               MainGame.me.save.unlocked_techs.Exists(a => a.ToLowerInvariant().Equals("Engineer".ToLowerInvariant()));
    }

    internal static void UpdateItemStates(ChestGUI instance)
    {
        foreach (var inventory in instance.player_panel.multi_inventory.all.Where(i => i.data.inventory.Count > 0))
        {
            foreach (var item in inventory.data.inventory)
            {
                var itemCellGui = instance.player_panel.GetItemCellGuiForItem(item);

                itemCellGui.SetInactiveState(false);

                if (item.definition.player_cant_throw_out && !ExcludeItems.Contains(item.definition.type))
                {
                    itemCellGui.SetInactiveState();
                }
            }
        }

        foreach (var inventory in instance.chest_panel.multi_inventory.all.Where(i => i.data.inventory.Count > 0))
        {
            inventory.is_locked = true;
            foreach (var item in inventory.data.inventory)
            {
                instance.chest_panel.GetItemCellGuiForItem(item)?.SetInactiveState();
            }
        }
    }

    internal static int GetTrunkTier()
    {
        var fullPriceUnlocked = UnlockedFullPrice();
        var shippingBoxExpansionUnlocked = UnlockedShippingBoxExpansion();

        if (fullPriceUnlocked) return 3;
        return shippingBoxExpansionUnlocked ? 2 : 1;
    }

    internal static void CheckShippingBox()
    {
        if (UnlockedShippingBox())
        {
            MainGame.me.save.UnlockCraft(ShippingBoxId);
            if (Plugin.DebugEnabled) WriteLog($"Tech requirements met, unlocking shipping box craft!");
        }
        else
        {
            MainGame.me.save.LockCraft(ShippingBoxId);
            if (Plugin.DebugEnabled) WriteLog($"Tech requirements not met, locking shipping box craft!");
        }
    }

    internal static void UpdateShippingBox(CraftDefinition sbCraft, WorldGameObject shippingBoxInstance = null)
    {
        if (!InternalShippingBoxBuilt.Value || _shippingBox != null) return;

        _shippingBox = shippingBoxInstance ? shippingBoxInstance : UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true)
            .FirstOrDefault(x => string.Equals(x.custom_tag, ShippingBoxTag));

        if (_shippingBox == null)
        {
            if (DebugEnabled)
            {
                Log.LogInfo("UpdateShippingBox: No Shipping Box Found!");
            }
            InternalShippingBoxBuilt.Value = false;
            sbCraft.hidden = false;
        }
        else
        {
            if (DebugEnabled)
            {
                Log.LogInfo($"UpdateShippingBox: Found Shipping Box at {_shippingBox.pos3}");
            }
            InternalShippingBoxBuilt.Value = true;
            _shippingBox.data.drop_zone_id = ShippingBoxTag;

            var invSize = UnlockedShippingBoxExpansion() ? LargeInvSize : SmallInvSize;
            _shippingBox.data.SetInventorySize(invSize);

            sbCraft.hidden = true;
        }
    }
}
