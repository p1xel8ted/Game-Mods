using System.Collections.Generic;
using System.Globalization;
using FlowCanvas;
using HarmonyLib;

namespace Helper
{
    public static class CrossModFields
    {
        public static bool _talkingToNpc { get; private set; }
        public static bool ConfigReloadShown { get; set; }
        public static string ModGerryTag => "mod_gerry";
        public static float ConfigReloadTime { get; set; }
        public static float ConfigReloadTimeLength => 5f;
        public static float TimeOfDayFloat { get; internal set; }
        public static string Lang { get; internal set; }
        public static CultureInfo Culture { get; internal set; }
        public static bool CraftAnywhere { get; set; }
        public static WorldGameObject CurrentWgoInteraction { get; internal set; }
        public static WorldGameObject PreviousWgoInteraction { get; set; }
        public static bool IsVendor { get; internal set; }
        public static bool IsCraft { get; internal set; }

        public static ValueInput<WorldGameObject> FlowNodeWgo { get; internal set; }

        public static ValueInput<string> FlowNodeText { get; internal set; }
        public static bool IsMoneyLender { get; internal set; }
        public static bool IsChurchPulpit { get; internal set; }
        public static bool IsChest { get; internal set; }
        public static bool IsBarman { get; internal set; }
        public static bool IsTavernCellarRack { get; internal set; }
        public static bool IsRefugee { get; internal set; }
        public static bool IsWritersTable { get; internal set; }
        public static bool IsSoulBox { get; internal set; }
        public static bool IsInDungeon { get; internal set; }

        public static bool PlayerIsDead { get; internal set; }
        public static bool PlayerIsControlled { get; internal set; }
        public static bool PlayerFollowingTarget { get; internal set; }

        public static List<WorldGameObject> WorldObjects => WorldMap._objs;
        
        public static List<WorldGameObject> WorldNpcs => WorldMap._npcs;
        
        public static List<Vendor> WorldVendors => WorldMap._vendors;

        public static void TalkingToNpc(string caller, bool setTalkingToNpc)
        {
           // Tools.Log("[QModHelper]",$"[SettingTalkingToNPC: {caller}]: Setting NPC to: {setTalkingToNpc}");
            _talkingToNpc = setTalkingToNpc;
        }

    }
}