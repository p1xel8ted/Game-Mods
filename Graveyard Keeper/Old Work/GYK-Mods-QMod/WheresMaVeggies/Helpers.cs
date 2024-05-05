using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Helper;

namespace WheresMaVeggies;

public static partial class MainPatcher
{
    
    internal const string ControllerUnlockTechTooltip = "Gardening";
    internal const string MouseUnlockTechTooltip = "Perk: Farmer";
    
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    private static void Log(string message, bool error = false)
    {
        if (!_cfg.Debug) return;
        if (error)
            Tools.Log("WheresMaVeggies", $"{message}", true);
        else
            Tools.Log("WheresMaVeggies", $"{message}");
    }

    private static List<WorldGameObject> FindNearbyObjectsByVector(WorldGameObject obj)
    {
        var origX = Math.Abs(obj.pos3.x);
        var origY = Math.Abs(obj.pos3.y);
        var objects = CrossModFields.WorldObjects.Where(a => a != obj && a.obj_id == obj.obj_id).ToList();
        var foundObjects = new List<WorldGameObject>();
        foreach (var obj2 in objects)
        {
            var newX = Math.Abs(obj2.pos3.x);
            var newY = Math.Abs(obj2.pos3.y);
            var range = 0;
            if (UnlockedStageOne())
            {
                range = 96;
            }
            if (Math.Abs(origX - newX) <= range && Math.Abs(origY - newY) <= range)
            {
                foundObjects.Add(obj2);
                Log($"Found nearby object: {obj2.obj_id}");
            }
        }

        return foundObjects;
        //var origZ = obj.pos3.z;
        //Log("FindNearbyObjectsByVector");
    }


    private static bool UnlockedStageOne()
    {
        return MainGame.me.save.unlocked_techs.Exists(a =>
            a.ToLowerInvariant().Equals(ControllerUnlockTechTooltip.ToLowerInvariant()));
    }
}