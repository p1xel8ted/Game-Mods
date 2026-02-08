namespace BringOutYerDead;

public partial class Plugin
{
    internal static bool PrideDayLogged { get; set; }
    private static WorldGameObject Donkey { get; set; }
    private static bool StrikeDone { get; set; }
    
    private void Update()
    {
        if (!MainGame.game_started) return;
        if (MainGame.paused) return;

        if (!Helpers.TutorialDone() && !InternalTutMessageShown.Value)
        {
            InternalTutMessageShown.Value = true;
            Helpers.Log("Need to complete all 'tutorial' quests first, up to and including the repair the sword quest.");
            return;
        }
        
        if (Donkey == null)
        {
            Donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        }


        if (Donkey != null)
        {
            var dataGetParam = Donkey.data.GetParam("speed");
            var getParam = Donkey.GetParam("speed");

            StrikeDone = Donkey.GetParam("strike_completed") > 0f;

            if (dataGetParam < DonkeySpeed.Value || getParam < DonkeySpeed.Value)
            {
                Helpers.Log($"TDU: Donkey old speeds: DataGetParam: {dataGetParam}, GetParam: {getParam}");
                Donkey.components.character.SetSpeed(DonkeySpeed.Value);
                Helpers.Log($"TDU: Donkey new speeds: DataGetParam: {dataGetParam}, GetParam: {getParam}");
            }

            if (!StrikeDone)
            {
                Helpers.Log($"Must complete the donkey strike first! Pay him 10 carrots, grease his wheels etc.");
                return;
            }
        }
        else
        {
            Helpers.Log($"Donkey is null!?!?!");
            return;
        }


        if (MainGame.me.save.day_of_week == 1)
        {
            if (!PrideDayLogged)
            {
                Helpers.Log($"Pride day! Skipping donkey as he doesnt come anyway when asked if its Pride day!");
                PrideDayLogged = true;
            }

            return;
        }


        switch (TimeOfDay.me.time_of_day_enum)
        {
            case TimeOfDay.TimeOfDayEnum.Night:
                if (!NightDelivery.Value)
                {
                    Helpers.Log("Night delivery is disabled in config!");
                    break;
                }

                if (!InternalNightDelivery.Value)
                {
                    //Tools.ShowMessage("Night Delivery!", MainGame.me.player_pos);

                    if (Patches.ForceDonkey(Donkey))
                    {
                        Helpers.Log($"It's night! Beginning night time delivery!");
                        InternalNightDelivery.Value = true;
                    }
                    else
                    {
                        InternalNightDelivery.Value = false;
                        Helpers.Log($"It's night! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Morning:
                if (!MorningDelivery.Value)
                {
                    Helpers.Log("Morning delivery is disabled in config!");
                    break;
                }

                if (!InternalMorningDelivery.Value)
                {
                    //Tools.ShowMessage("Morning Delivery!", MainGame.me.player_pos);
                    Helpers.Log($"It's morning! Beginning morning delivery!");
                    if (Patches.ForceDonkey(Donkey))
                    {
                        InternalMorningDelivery.Value = true;
                    }
                    else
                    {
                        InternalMorningDelivery.Value = false;
                        Helpers.Log($"It's morning! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Day:
                if (!DayDelivery.Value)
                {
                    Helpers.Log("Day delivery is disabled in config!");
                    return;
                }

                if (!InternalDayDelivery.Value)
                {
                    // Tools.ShowMessage("Day Delivery!", MainGame.me.player_pos);
                    Helpers.Log($"It's Day! Beginning midday delivery!");
                    if (Patches.ForceDonkey(Donkey))
                    {
                        InternalDayDelivery.Value = true;
                    }
                    else
                    {
                        InternalDayDelivery.Value = false;
                        Helpers.Log($"It's midday! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            case TimeOfDay.TimeOfDayEnum.Evening:
                if (!EveningDelivery.Value)
                {
                    Helpers.Log("Evening delivery is disabled in config!");
                    return;
                }

                if (!InternalEveningDelivery.Value)
                {
                    if (Patches.ForceDonkey(Donkey))
                    {
                        Helpers.Log($"It's evening! Beginning evening delivery!");
                        InternalEveningDelivery.Value = true;
                    }
                    else
                    {
                        InternalEveningDelivery.Value = false;
                        Helpers.Log($"It's evening! But we failed to force the donkey to deliver!");
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}