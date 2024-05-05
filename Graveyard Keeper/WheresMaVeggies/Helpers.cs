using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GYKHelper;


namespace WheresMaVeggies;

public static class Helpers
{
    internal const string ControllerUnlockTechTooltip = "Gardening";
    internal const string MouseUnlockTechTooltip = "Perk: Farmer";

    internal static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    internal static List<WorldGameObject> FindNearbyObjectsByVector(WorldGameObject obj)
    {
        var origX = Math.Abs(obj.pos3.x);
        var origY = Math.Abs(obj.pos3.y);
        var range = UnlockedStageOne() ? 96 : 0;
        var foundObjects = CrossModFields.WorldObjects
            .Where(a => a != obj && a.obj_id == obj.obj_id &&
                        Math.Abs(origX - Math.Abs(a.pos3.x)) <= range &&
                        Math.Abs(origY - Math.Abs(a.pos3.y)) <= range)
            .ToList();

        if (!Plugin.Debug.Value) return foundObjects;

        foreach (var obj2 in foundObjects)
        {
            Plugin.Log.LogInfo($"Found nearby object: {obj2.obj_id}");
        }

        return foundObjects;
    }


    internal static bool UnlockedStageOne()
    {
        return MainGame.me.save.unlocked_techs.Exists(a =>
            a.ToLowerInvariant().Equals(ControllerUnlockTechTooltip.ToLowerInvariant()));
    }
}