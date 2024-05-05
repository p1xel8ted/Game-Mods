using HarmonyLib;
using System;
using UnityEngine;

namespace AlwaysShowExperienceBar
{
    public class AlwaysShowExperienceBar
    {
        [HarmonyPatch(typeof(BaseCharacterComponent), "UseItemFromToolbar", typeof(int))]
        public class BaseCharacterComponentUseItemFromToolbar
        {
            [HarmonyPostfix]
            public static void Postfix() => GUIElements.me.hud.tech_points_bar.Redraw();
        }

        [HarmonyPatch(typeof(InventoryGUI), "UseItem", new Type[] { })]
        public class InventoryGuiUseItem
        {
            [HarmonyPostfix]
            public static void Postfix() => GUIElements.me.hud.tech_points_bar.Redraw();
        }

        [HarmonyPatch(typeof(Item), "UseItem", typeof(WorldGameObject), typeof(Vector3?))]
        public class ItemUseItem
        {
            [HarmonyPostfix]
            public static void Postfix() => GUIElements.me.hud.tech_points_bar.Redraw();
        }

        [HarmonyPatch(typeof(PlayerComponent), "SpawnPlayer", null)]
        public class PlayerComponentSpawnPlayer
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (MainPatcher.FieldStayShownTime == null)
                    MainPatcher.FieldStayShownTime = typeof(AnimatedGUIPanel).GetField("stay_shown_time", AccessTools.all);
                if (!(MainPatcher.FieldStayShownTime != null))
                    return;
                GUIElements.me.hud.tech_points_bar.Init();
                MainPatcher.FieldStayShownTime.SetValue(GUIElements.me.hud.tech_points_bar, float.PositiveInfinity);
                GUIElements.me.hud.tech_points_bar.Show();
            }
        }

        [HarmonyPatch(typeof(TechConnector), "SetState", typeof(TechDefinition.TechState))]
        public class TechConnectorSetState
        {
            [HarmonyPostfix]
            public static void Postfix(TechDefinition.TechState state)
            {
                if (state != TechDefinition.TechState.Purchased)
                    return;
                GUIElements.me.hud.tech_points_bar.Redraw();
            }
        }

        [HarmonyPatch(typeof(WorldGameObject), "UseItemFromInventory", typeof(Item), typeof(Vector3?), typeof(Item))]
        public class WorldGameObjectUseItemFromInventory
        {
            [HarmonyPostfix]
            public static void Postfix() => GUIElements.me.hud.tech_points_bar.Redraw();
        }
    }
}