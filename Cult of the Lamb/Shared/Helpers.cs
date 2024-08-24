using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared;

public static class Helpers
{
    internal static FollowerTask_ClearRubble GetRubbleTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var rubble = StructureManager.GetAllStructuresOfType<Structures_Rubble>();
        foreach (var r in rubble)
        {
            r.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_ClearRubble;
    }


    internal static FollowerTask_Janitor GetJanitorTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var janitors = StructureManager.GetAllStructuresOfType<Structures_JanitorStation>();
        foreach (var janitor in janitors)
        {
            janitor.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Janitor;
    }

    internal static FollowerTask_Refinery GetRefineryTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var refineries = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
        foreach (var refinery in refineries)
        {
            refinery.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Refinery;
    }

    internal static FollowerTask_Undertaker GetMorgueTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var undertakers = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
        foreach (var undertaker in undertakers)
        {
            undertaker.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Undertaker;
    }

    internal static void StartTask(FollowerBrain brain, FollowerTask task)
    {
        brain.CompleteCurrentTask();
        brain.HardSwapToTask(task);
    }
    
    private static void StartTaskFromCommand(FollowerBrain brain, FollowerCommands command)
    {
        switch (command)
        {
            case FollowerCommands.CutTrees:
                StartTask(brain, new FollowerTask_ChopTrees());
                break;
            case FollowerCommands.ClearRubble:
                StartTask(brain, GetRubbleTask());
                break;
            case FollowerCommands.WorshipAtShrine:
                StartTask(brain, GetPrayTask());
                break;
            case FollowerCommands.Farmer_2:
                StartTask(brain, GetFarmTask());
                break;
            case FollowerCommands.Build:
                StartTask(brain, GetBuildTask());
                break;
            case FollowerCommands.Cook_2:
                StartTask(brain, GetKitchenTask());
                break;
            case FollowerCommands.Janitor_2:
                StartTask(brain, GetJanitorTask());
                break;
            case FollowerCommands.Refiner_2:
                StartTask(brain, GetRefineryTask());
                break;
            case FollowerCommands.Undertaker:
                StartTask(brain, GetMorgueTask());
                break;
            case FollowerCommands.Brew:
                StartTask(brain, GetBrewTask());
                break;
            default:
                StartTask(brain, GetRandomTask(brain));
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
            
        }
    }

    internal static FollowerTask_Cook GetKitchenTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var kitchens = StructureManager.GetAllStructuresOfType<Structures_Kitchen>();
        foreach (var kitchen in kitchens)
        {
            kitchen.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Cook;
    }

    internal static FollowerTask GetRandomTask(FollowerBrain brain)
    {
        var task = FollowerBrain.GetDesiredTask_Work(brain.Location);
        return task.FirstOrDefault();
    }

    internal static FollowerTask_Brew GetBrewTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var breweries = StructureManager.GetAllStructuresOfType<Structures_Pub>();
        foreach (var brewery in breweries)
        {
            brewery.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Brew;
    }

    internal static FollowerTask_Farm GetFarmTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var farms = StructureManager.GetAllStructuresOfType<Structures_FarmerStation>();
        foreach (var farm in farms)
        {
            farm.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Farm;
    }

    internal static FollowerTask_Pray GetPrayTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var temples = StructureManager.GetAllStructuresOfType<Structures_Shrine>();
        foreach (var temple in temples)
        {
            temple.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Pray;
    }

    internal static FollowerTask_Build GetBuildTask()
    {
        var tasks = new SortedList<float, FollowerTask>();
        var builders = StructureManager.GetAllStructuresOfType<Structures_BuildSite>();
        foreach (var builder in builders)
        {
            builder.GetAvailableTasks(ScheduledActivity.Work, tasks);
        }
        return tasks.Values.FirstOrDefault() as FollowerTask_Build;
    }

    internal static List<Follower> AllFollowers => FollowerManager.Followers.SelectMany(followerList => followerList.Value).ToList();

    public static IEnumerator FilterEnumerator(IEnumerator original, Type[] typesToRemove)
    {
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && !typesToRemove.Contains(current.GetType()))
            {
                yield return current;
            }
        }
    }
}