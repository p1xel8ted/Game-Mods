﻿namespace EasyLiving;

public partial class Plugin
{
    private const string LoadScreen = "LoadScreen";

    private static void Notify()
    {
        if (SingletonBehaviour<NotificationStack>.Instance is not null)
        {
            SingletonBehaviour<NotificationStack>.Instance.SendNotification($"Movement Speed Multiplier: {MoveSpeedMultiplier.Value}");
        }
    }
    private void Update()
    {

        if (EnableSaveShortcut.Value && SaveShortcut.Value.IsUp() && Player.Instance is not null && GameSave.Instance is not null)
        {
            Utils.SaveGame(true);
        }

        if (MoveSpeedMultiplierIncrease.Value.IsUp())
        {
            MoveSpeedMultiplier.Value += 0.25f;
            Notify();
        }
        else if (MoveSpeedMultiplierDecrease.Value.IsUp())
        {
            MoveSpeedMultiplier.Value -= 0.25f;
            Notify();
        }

        if (Input.GetKey(SkipAutoLoadMostRecentSaveShortcut.Value.MainKey) && SceneManager.GetActiveScene().name.Equals(LoadScreen, StringComparison.InvariantCultureIgnoreCase))
        {
            Patches.SkipAutoLoad = true;
        }
    }
}