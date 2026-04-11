namespace AutoLootHeavies;

public partial class Plugin
{
    private static bool InitialFullUpdate { get; set; }

    private static void CheckKeybinds()
    {
        if (SetTimberLocationKeybind.Value.IsUp())
        {
            DesignatedTimberLocation.Value = MainGame.me.player_pos;
            ShowMessage(Lang.Get("DumpTimber"), DesignatedTimberLocation.Value);
        }

        if (SetOreLocationKeybind.Value.IsUp())
        {
            DesignatedOreLocation.Value = MainGame.me.player_pos;
            ShowMessage(Lang.Get("DumpOre"), DesignatedOreLocation.Value);
        }

        if (SetStoneLocationKeybind.Value.IsUp())
        {
            DesignatedStoneLocation.Value = MainGame.me.player_pos;
            ShowMessage(Lang.Get("DumpStone"), DesignatedStoneLocation.Value);
        }

        if (TeleportToggleKeybind.Value.IsUp())
        {
            TeleportToDumpSiteWhenAllStockPilesFull.Value = !TeleportToDumpSiteWhenAllStockPilesFull.Value;
            var state = TeleportToDumpSiteWhenAllStockPilesFull.Value ? "enabled" : "disabled";
            ShowMessage($"Teleport to dump site: {state}", MainGame.me.player.pos3);
        }
    }
}