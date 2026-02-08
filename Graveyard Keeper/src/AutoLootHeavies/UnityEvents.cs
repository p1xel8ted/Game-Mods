namespace AutoLootHeavies;

public partial class Plugin
{
    private static bool InitialFullUpdate { get; set; }

    private void Update()
    {
        if (!MainGame.game_started) return;

        if (!InitialFullUpdate)
        {
            InitialFullUpdate = true;
            MainGame.me.StartCoroutine(RunFullUpdate());
        }

        CheckKeybinds();
    }

    private static void CheckKeybinds()
    {
        if (SetTimberLocationKeybind.Value.IsUp())
        {
            DesignatedTimberLocation.Value = MainGame.me.player_pos;
            ShowMessage(strings.DumpTimber, DesignatedTimberLocation.Value);
        }

        if (SetOreLocationKeybind.Value.IsUp())
        {
            DesignatedOreLocation.Value = MainGame.me.player_pos;
            ShowMessage(strings.DumpOre, DesignatedOreLocation.Value);
        }

        if (SetStoneLocationKeybind.Value.IsUp())
        {
            DesignatedStoneLocation.Value = MainGame.me.player_pos;
            ShowMessage(strings.DumpStone, DesignatedStoneLocation.Value);
        }
    }
}