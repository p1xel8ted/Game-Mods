using GerrysJunkTrunk.lang;
using HarmonyLib;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Tools = Helper.Tools;

namespace GerrysJunkTrunk;

[HarmonyPatch]
public static partial class MainPatcher
{
    private const float FullPriceModifier = 0.90f;
    private const float PityPrice = 0.10f;
    private const int LargeInvSize = 20;
    private const int LargeMaxItemCount = 100;
    private const float PriceModifier = 0.60f;
    private const string ShippingBoxTag = "shipping_box";
    private const string ShippingItem = "shipping";
    private const int SmallInvSize = 10;
    private const int SmallMaxItemCount = 50;
    private static readonly List<WorldGameObject> KnownVendors = new();
    private static int _techCount;
    private static int _oldTechCount;
    private static readonly Dictionary<string, int> StackSizeBackups = new();

    //private static readonly List<WorldGameObject> VendorWgos = new();
    private static Config.Options _cfg;
    private static InternalConfig.Options _internalCfg;
    private static WorldGameObject _myVendor;
    private static WorldGameObject _shippingBox;
    private static WorldGameObject _interactedObject;
    private static bool _shippingBuild;
    private static bool _usingShippingBox;
    private static List<VendorSale> _vendorSales = new();
    private static readonly List<ItemPrice> PriceCache = new();
    private static readonly List<BaseItemCellGUI> AlreadyDone = new();
    private static ObjectCraftDefinition _newItem;
    private const string ShippingBoxId = "mf_wood_builddesk:p:mf_shipping_box_place";

    public static void Patch()
    {
        try
        {
            _internalCfg = InternalConfig.GetOptions();
            _cfg = Config.GetOptions();
            
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.GerrysJunkTrunk");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void ShowSummary(string money)
    {
        if (!MainGame.game_started) return;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        var result = string.Empty;
        foreach (var vendor in _vendorSales)
        {
            var sales = vendor.GetSales().OrderBy(a => a.GetItem().id).ToList();

            foreach (var sale in sales)
            {
                result += $"{sale.GetItem().GetItemName()} {strings.For} {Trading.FormatMoney(sale.GetPrice())}\n";
            }
        }

        GUIElements.me.dialog.OpenOK($"[37ff00]{strings.Header}[-]", null, $"{result}", true, $"{money}");
    }

    private static void ClearGerryFlag(ChestGUI chestGui)
    {
        if (chestGui == null || !_usingShippingBox) return;
        if (StackSizeBackups.Count <= 0) return;
        foreach (var item in chestGui.player_panel.multi_inventory.all[0].data.inventory)
        {
            var found = StackSizeBackups.TryGetValue(item.id, out var value);
            if (!found) continue;

            item.definition.stack_count = value;
        }

        foreach (var item in chestGui.chest_panel.multi_inventory.all[0].data.inventory)
        {
            var found = StackSizeBackups.TryGetValue(item.id, out var value);
            if (!found) continue;

            item.definition.stack_count = value;
        }

        _usingShippingBox = false;
    }

    private static float GetBoxEarnings(WorldGameObject shippingBox)
    {
        return shippingBox.data.inventory.Sum(GetItemEarnings);
    }

    private static float GetItemEarnings(Item selectedItem)
    {
        var itemCache = PriceCache.Find(a =>
            string.Equals(a.GetItem().id, selectedItem.id) && a.GetQty() == selectedItem.value);

        if (itemCache != null)
        {
            if (itemCache.GetPrice() == 0)
            {
                var price = PityPrice * itemCache.GetQty();

                return UnlockedFullPrice() ? price * FullPriceModifier : price * PriceModifier;
            }


            return UnlockedFullPrice() ? itemCache.GetPrice() * FullPriceModifier : itemCache.GetPrice() * PriceModifier;
        }

        var totalSalePrice = 0f;
        _vendorSales.Clear();
        var totalCount = selectedItem.value;

        List<Vendor> vendorList = new();
        List<float> priceList = new();


        List<Vendor> vendors = null;
        while (vendors == null || vendors.Count == 0)
        {
            WorldMap.FillVendorsList();
            vendors = (List<Vendor>) AccessTools.Field(typeof(WorldMap), "_vendors").GetValue(null);
            // Debug.LogError($"Vendor count: {vendors.Count}");
        }

        foreach (var vendor in vendors)
        {
            var myVendor = WorldMap.GetNPCByObjID(vendor.id, true);

            if (!KnownVendors.Contains(myVendor)) continue;

            float num = 0;

            var myTrader = new Trading(myVendor);
            _myVendor = myVendor;
            if (selectedItem.definition.base_price <= 0)
            {
                if (selectedItem.id.EndsWith(":3"))
                {
                    num += 0.75f * totalCount;
                }
                else if (selectedItem.id.EndsWith(":2"))
                {
                    num += 0.60f * totalCount;
                }
                else if (selectedItem.id.EndsWith(":1"))
                {
                    num += 0.45f * totalCount;
                }
                else
                {
                    num += 0.25f * totalCount;
                }
            }
            else
            {
                for (var i = 0; i < totalCount; i++)
                {
                    var itemCost = Mathf.Round(myTrader.GetSingleItemCostInPlayerInventory(selectedItem, -i) * 100f) / 100f;
                    num += itemCost;
                }
            }

            vendorList.Add(vendor);
            priceList.Add(num);
            // Debug.LogError($"Vendor: {vendor.id}, Price: {num}");
        }

        var maxSaleIndex = priceList.IndexOf(priceList.Max());
        var newSale = new VendorSale(vendorList[maxSaleIndex]);
        newSale.AddSale(selectedItem, totalCount, priceList[maxSaleIndex]);
        _vendorSales.Add(newSale);
        _vendorSales = _vendorSales.OrderBy(a => a.GetVendor().id).ToList();
        totalSalePrice += priceList[maxSaleIndex];

        PriceCache.Add(new ItemPrice(selectedItem, totalCount, priceList[maxSaleIndex]));

        if (totalSalePrice <= 0)
        {
            var price = PityPrice * totalCount;


            return UnlockedFullPrice() ? price * FullPriceModifier : price * PriceModifier;
        }


        return UnlockedFullPrice() ? totalSalePrice * FullPriceModifier : totalSalePrice * PriceModifier;
    }

    private static void ShowIntroMessage()
    {
        GUIElements.me.dialog.OpenOK(strings.Message1, null, $"{strings.Message2}\n{strings.Message3}\n{strings.Message4}\n{strings.Message5}\n{strings.Message6}\n{strings.Message7}", true, strings.Message8);
    }

    private static void StartGerryRoutine(float num)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        var noSales = num <= 0;
        var money = Trading.FormatMoney(num, true);
        //var gerry = WorldMap.GetNPCByObjID("talking_skull");
        var gerry = WorldMap.SpawnWGO(_shippingBox.transform, "talking_skull", new Vector3(_shippingBox.pos3.x, _shippingBox.pos3.y + 43f, _shippingBox.pos3.z));
        Tools.NameSpawnedGerry(gerry);
        gerry.ReplaceWithObject("talking_skull", true);
        Tools.NameSpawnedGerry(gerry);

        GJTimer.AddTimer(2f, delegate
        {
            gerry.Say(noSales ? strings.Nothing : strings.WorkWork, delegate
            {
                GJTimer.AddTimer(1f, delegate
                {
                    gerry.ReplaceWithObject("talking_skull", true);
                    Tools.NameSpawnedGerry(gerry);
                    gerry.DestroyMe();
                });
            }, null, SpeechBubbleGUI.SpeechBubbleType.Talk, SmartSpeechEngine.VoiceID.Skull);
        });

        if (noSales) return;
        GJTimer.AddTimer(8f, delegate
        {
            var gerry2 = WorldMap.SpawnWGO(_shippingBox.transform, "talking_skull", new Vector3(_shippingBox.pos3.x, _shippingBox.pos3.y + 43f, _shippingBox.pos3.z));
            Tools.NameSpawnedGerry(gerry2);
            gerry2.ReplaceWithObject("talking_skull", true);
            Tools.NameSpawnedGerry(gerry2);

            GJTimer.AddTimer(2f, delegate
            {
                gerry2.Say($"{money}", delegate
                    {
                        _shippingBox.data.inventory.Clear();
                        if (_cfg.showSoldMessagesOnPlayer)
                        {
                            Sounds.PlaySound("coins_sound", MainGame.me.player_pos, true);
                            var pos = MainGame.me.player_pos;
                            pos.y += 125f;
                            EffectBubblesManager.ShowImmediately(pos, $"{money}",
                                num > 0 ? EffectBubblesManager.BubbleColor.Green : EffectBubblesManager.BubbleColor.Red,
                                true, 4f);
                        }
                        else
                        {
                            Sounds.PlaySound("coins_sound", gerry2.pos3, true);
                        }

                        GJTimer.AddTimer(2f, delegate
                        {
                            gerry2.Say(strings.Bye, delegate
                            {
                                GJTimer.AddTimer(1f, delegate
                                {
                                    gerry2.ReplaceWithObject("talking_skull", true);
                                    Tools.NameSpawnedGerry(gerry2);
                                    gerry2.DestroyMe();

                                    GJTimer.AddTimer(1f, delegate { ShowSummary(money); });
                                });
                            }, null, SpeechBubbleGUI.SpeechBubbleType.Talk, SmartSpeechEngine.VoiceID.Skull);
                        });
                    }, null, SpeechBubbleGUI.SpeechBubbleType.Talk,
                    SmartSpeechEngine.VoiceID.Skull);
            });
        });
    }

    private static void TryAdd<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key)) return;
        dictionary.Add(key, value);
    }

    private static bool UnlockedFullPrice()
    {
        return UnlockedShippingBoxExpansion() && MainGame.me.save.unlocked_techs.Exists(a => a.ToLowerInvariant().Equals("Best friend".ToLowerInvariant()));
    }

    private static bool UnlockedShippingBox()
    {
        return MainGame.me.save.unlocked_techs.Exists(a =>
            a.ToLowerInvariant().Equals("Wood processing".ToLowerInvariant()));
    }

    private static bool UnlockedShippingBoxExpansion()
    {
        return UnlockedShippingBox() && MainGame.me.save.unlocked_techs.Exists(a => a.ToLowerInvariant().Equals("Engineer".ToLowerInvariant()));
    }

    private static void UpdateInternalConfig()
    {
        InternalConfig.WriteOptions();
        _internalCfg = InternalConfig.GetOptions();
    }

    private static readonly ItemDefinition.ItemType[] ExcludeItems =
    {
        ItemDefinition.ItemType.Axe, ItemDefinition.ItemType.Shovel, ItemDefinition.ItemType.Hammer,
        ItemDefinition.ItemType.Pickaxe, ItemDefinition.ItemType.FishingRod, ItemDefinition.ItemType.BodyArmor,
        ItemDefinition.ItemType.HeadArmor, ItemDefinition.ItemType.Sword, ItemDefinition.ItemType.Preach,
        ItemDefinition.ItemType.GraveStone, ItemDefinition.ItemType.GraveFence, ItemDefinition.ItemType.GraveCover,
        ItemDefinition.ItemType.GraveStoneReq, ItemDefinition.ItemType.GraveFenceReq, ItemDefinition.ItemType.GraveCoverReq,
    };

    private static void UpdateItemStates(ref ChestGUI __instance)
    {
        foreach (var inventory in __instance.player_panel.multi_inventory.all.Where(i => i.data.inventory.Count > 0))
        {
            //reset status
            foreach (var item in inventory.data.inventory)
            {
                var itemCellGuiForItem = __instance.player_panel.GetItemCellGuiForItem(item);
                itemCellGuiForItem.SetInactiveState(false);
            }

            //disable quest item selling
            foreach (var item in inventory.data.inventory.Where(item => item.definition.player_cant_throw_out && !ExcludeItems.Contains(item.definition.type)))
            {
                var itemCellGuiForItem = __instance.player_panel.GetItemCellGuiForItem(item);
                itemCellGuiForItem.SetInactiveState();
            }
        }

        //disable items in the chest inventory
        foreach (var inventory in __instance.chest_panel.multi_inventory.all.Where(i => i.data.inventory.Count > 0))
        {
            inventory.is_locked = true;
            foreach (var item in inventory.data.inventory)
            {
                var itemCellGuiForItem = __instance.chest_panel.GetItemCellGuiForItem(item);
                if (itemCellGuiForItem != null)
                {
                    itemCellGuiForItem.SetInactiveState();
                }
            }
        }
    }

    private static int GetTrunkTier()
    {
        if (UnlockedFullPrice()) return 3;
        return UnlockedShippingBoxExpansion() ? 2 : 1;
    }


    private static void CheckShippingBox()
    {
        if (UnlockedShippingBox())
        {
            MainGame.me.save.UnlockCraft(ShippingBoxId);
            Log($"Tech requirements met, unlocking shipping box craft!");
        }
        else
        {
            MainGame.me.save.LockCraft(ShippingBoxId);
            Log($"Tech requirements not met, locking shipping box craft!");
        }
    }



}