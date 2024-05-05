using HarmonyLib;
using Rewired;
using System;
using System.Linq;
using System.Reflection;
using MaxButton;
using Tools = Helper.Tools;

namespace MaxButtonControllerSupport
{
    public class MainPatcher
    {
        private static bool _craftGuiOpen;
        private static bool _itemCountGuiOpen;
        private static CraftItemGUI _craftItemGui;
        private static WorldGameObject _crafteryWgo;
        private static SmartSlider _slider;

        private static bool _unsafeInteraction;

        private static readonly string[] UnSafeCraftObjects =
        {
            "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
            "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2", "zombie_garden_desk_3",
            "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk", "blockage_H_low", "blockage_V_low",
            "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1", "refugee_camp_garden_bed_2",
            "refugee_camp_garden_bed_3"
        };

        private static readonly string[] UnSafeCraftZones =
        {
            "church"
        };

        private static readonly string[] UnSafePartials =
        {
            "blockage", "obstacle", "builddesk", "fix", "broken"
        };

        public static void Patch()
        {
            try
            {
                if (Harmony.HasAnyPatches("com.graveyardkeeper.urbanvibes.maxbutton"))
                {
                    //Log($"MaxButton found, continuing with patch process.");
                    var harmony = new Harmony("p1xel8ted.GraveyardKeeper.MaxButtonControllerSupport");
                    harmony.PatchAll(Assembly.GetExecutingAssembly());
                }
                else
                {
                    Log($"MaxButton not found, aborting patch process.");
                }
            }
            catch (Exception ex)
            {
                Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
            }
        }

        private static void Log(string message, bool error = false)
        {
            Tools.Log("MaxButtonControllerSupport", $"{message}", error);
        }

        [HarmonyPatch(typeof(CraftGUI))]
        public static class CraftGuiPatchMbcs
        {
            [HarmonyPatch(nameof(CraftGUI.Open))]
            [HarmonyPostfix]
            public static void OpenPostfix()
            {
                _craftGuiOpen = true;
            }

            [HarmonyPatch(nameof(CraftGUI.OnClosePressed))]
            [HarmonyPostfix]
            public static void ClosePostfix()
            {
                _craftGuiOpen = false;
            }
        }

        [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.OnOver))]
        public static class CraftItemGuiOnOverPatchMbcs
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                _craftItemGui = CraftItemGUI.current_overed;
                _crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
            }
        }

        [HarmonyPatch(typeof(ItemCountGUI))]
        public static class ItemCountGuiMbcs
        {
            [HarmonyPostfix]
            [HarmonyPatch("Open")]
            public static void OpenPostfix(ref ItemCountGUI __instance)
            {
                //if (!MainGame.game_started || MainGame.me.player.is_dead || MainGame.me.player.IsDisabled()) return;
                _itemCountGuiOpen = true;
                _slider = __instance.transform.Find("window/Container/smart slider").GetComponent<SmartSlider>();
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnPressedBack")]
            public static void HidePostfix()
            {
                _itemCountGuiOpen = false;
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnConfirm")]
            public static void OnClosePressedPostfix()
            {
                _itemCountGuiOpen = false;
            }
        }

        [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
        public static class WorldGameObjectInteractPatch
        {
            [HarmonyPrefix]
            public static void Prefix(ref WorldGameObject __instance)
            {
                if (UnSafeCraftZones.Contains(__instance.GetMyWorldZoneId()) || UnSafePartials.Any(__instance.obj_id.Contains) || UnSafeCraftObjects.Contains(__instance.obj_id))
                {
                    _unsafeInteraction = true;
                }
                else
                {
                    _unsafeInteraction = false;
                }
            }
        }

        [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
        public static class TimeOfDayUpdatePatchMbcs
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                if (!MainGame.game_started || MainGame.me.player.is_dead || MainGame.me.player.IsDisabled()) return;
                if (FloatingWorldGameObject.cur_floating != null) return;

                //RT = 19
                if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(19) && _itemCountGuiOpen)
                {
                    typeof(MaxButtonVendor).GetMethod("SetMaxPrice", AccessTools.all)
                        ?.Invoke(typeof(MaxButtonVendor), new object[]
                        {
                            _slider
                        });
                }

                //LT = 20
                if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(20) && _itemCountGuiOpen)
                {
                    typeof(MaxButtonVendor).GetMethod("SetSliderValue", AccessTools.all)
                        ?.Invoke(typeof(MaxButtonVendor), new object[]
                        {
                            _slider,
                            1
                        });
                }

                //RT = 19
                if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(19) && _craftGuiOpen && !_unsafeInteraction)
                {
                    if (_craftItemGui.current_craft.needs.Any(need => need.is_multiquality)) return;
                    if (_craftItemGui.current_craft.one_time_craft) return;
                    typeof(MaxButtonCrafting).GetMethod("SetMaximumAmount", AccessTools.all)
                        ?.Invoke(typeof(MaxButtonCrafting), new object[]
                        {
                            _craftItemGui,
                            _crafteryWgo
                        });
                }

                //LT = 20
                if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(20) && _craftGuiOpen && !_unsafeInteraction)
                {
                    if (_craftItemGui.current_craft.needs.Any(need => need.is_multiquality)) return;
                    if (_craftItemGui.current_craft.one_time_craft) return;
                    typeof(MaxButtonCrafting).GetMethod("SetMinimumAmount", AccessTools.all)
                        ?.Invoke(typeof(MaxButtonCrafting), new object[]
                        {
                            _craftItemGui
                        });
                }
            }

            [HarmonyFinalizer]
            public static Exception Finalizer()
            {
                return null;
            }
        }
    }
}