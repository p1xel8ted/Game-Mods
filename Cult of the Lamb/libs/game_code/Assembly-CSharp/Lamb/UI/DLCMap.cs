// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DLCMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class DLCMap : MonoBehaviour
{
  public static void ClearRot()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) DLCMap.ClearRotRoutine());
  }

  public static IEnumerator ClearRotRoutine()
  {
    DataManager.Instance.RevealedDLCMapDoor = true;
    yield return (object) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets(), (System.Action) null);
    UIDLCMapMenuController map = MonoSingleton<UIManager>.Instance.DLCWorldMapTemplate.Instantiate<UIDLCMapMenuController>();
    map.LockMenu();
    map.AutoSelectNextNode = false;
    map.Parallax.parallaxToRelativeMousePosition = false;
    map.Show();
    map.SetDungeonSide(UIDLCMapMenuController.DLCDungeonSide.OutsideMountain);
    yield return (object) map.ClearRot().YieldUntilCompleted();
    map.Hide();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public static void RevealDoorway()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) DLCMap.RevealDoorwayRoutine());
  }

  public static IEnumerator RevealDoorwayRoutine()
  {
    DataManager.Instance.RevealedDLCMapDoor = true;
    DataManager.Instance.RevealedWolfNode = true;
    yield return (object) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets(), (System.Action) null);
    yield return (object) DLCMap.RevealNodeSequenceAsync(new DataManager.Variables[2]
    {
      DataManager.Variables.RevealedDLCMapDoor,
      DataManager.Variables.RevealedWolfNode
    }, UIDLCMapMenuController.DLCDungeonSide.OutsideMountain, new bool[2]
    {
      true,
      false
    }, new bool[2]{ true, true }, new bool[2]{ true, true }).YieldUntilCompleted();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public static void RevealHeart()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) DLCMap.RevealHeartRoutine());
  }

  public static IEnumerator RevealHeartRoutine()
  {
    DataManager.Instance.RevealedDLCMapHeart = true;
    yield return (object) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets(), (System.Action) null);
    yield return (object) DLCMap.RevealNodeSequenceAsync(DataManager.Variables.RevealedDLCMapHeart, UIDLCMapMenuController.DLCDungeonSide.InsideMountain, false, zoomReveal: false).YieldUntilCompleted();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public static void EnableHeart()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) DLCMap.EnableHeartRoutine());
  }

  public static IEnumerator EnableHeartRoutine()
  {
    DataManager.Instance.EnabledDLCMapHeart = true;
    yield return (object) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets(), (System.Action) null);
    yield return (object) DLCMap.RevealNodeSequenceAsync(DataManager.Variables.RevealedDLCMapHeart, UIDLCMapMenuController.DLCDungeonSide.InsideMountain, revealNode: false, zoomReveal: false).YieldUntilCompleted();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/YngyaFight", Objectives.CustomQuestTypes.ProceedToYngya), true, true);
  }

  public static async System.Threading.Tasks.Task RevealNodeSequenceAsync(
    DataManager.Variables variable,
    UIDLCMapMenuController.DLCDungeonSide side,
    bool revealConnection = true,
    bool revealNode = true,
    bool zoomReveal = true)
  {
    await DLCMap.RevealNodeSequenceAsync(new DataManager.Variables[1]
    {
      variable
    }, side, revealConnection, revealNode, zoomReveal);
  }

  public static async System.Threading.Tasks.Task RevealNodeSequenceAsync(
    DataManager.Variables[] variables,
    UIDLCMapMenuController.DLCDungeonSide side,
    bool revealConnection = true,
    bool revealNode = true,
    bool zoomReveal = true)
  {
    bool[] revealConnections = new bool[variables.Length];
    bool[] revealNodes = new bool[variables.Length];
    bool[] zoomReveals = new bool[variables.Length];
    for (int index = 0; index < variables.Length; ++index)
    {
      revealConnections[index] = revealConnection;
      revealNodes[index] = revealNode;
      zoomReveals[index] = zoomReveal;
    }
    await DLCMap.RevealNodeSequenceAsync(variables, side, revealConnections, revealNodes, zoomReveals);
  }

  public static async System.Threading.Tasks.Task RevealNodeSequenceAsync(
    DataManager.Variables[] variables,
    UIDLCMapMenuController.DLCDungeonSide side,
    bool[] revealConnections,
    bool[] revealNodes,
    bool[] zoomReveals)
  {
    UIDLCMapMenuController map = MonoSingleton<UIManager>.Instance.ShowDLCMapMenu(PlayerFarming.Instance);
    List<DungeonWorldMapIcon> locations = map.Locations.ToList<DungeonWorldMapIcon>();
    map.LockMenu();
    map.AutoSelectNextNode = false;
    map.Parallax.parallaxToRelativeMousePosition = false;
    map.SetDungeonSide(side);
    for (int index = 0; index < variables.Length; ++index)
    {
      DataManager.Variables variable = variables[index];
      DungeonWorldMapIcon dungeonWorldMapIcon = locations.Find((Predicate<DungeonWorldMapIcon>) (l => l.ShowIfVariable == variable));
      if (!((UnityEngine.Object) dungeonWorldMapIcon == (UnityEngine.Object) null) && (revealNodes == null || index >= revealNodes.Length ? 1 : (revealNodes[index] ? 1 : 0)) != 0)
      {
        dungeonWorldMapIcon.Content.transform.localScale = Vector3.zero;
        dungeonWorldMapIcon._canvasGroup.alpha = 0.0f;
      }
    }
    for (int i = 0; i < variables.Length; ++i)
    {
      DataManager.Variables variable = variables[i];
      DungeonWorldMapIcon location = locations.Find((Predicate<DungeonWorldMapIcon>) (l => l.ShowIfVariable == variable));
      if (!((UnityEngine.Object) location == (UnityEngine.Object) null))
      {
        bool revealConnection = revealConnections == null || i >= revealConnections.Length || revealConnections[i];
        bool revealNode = revealNodes == null || i >= revealNodes.Length || revealNodes[i];
        bool zoomReveal = zoomReveals == null || i >= zoomReveals.Length || zoomReveals[i];
        await map.RevealLocation(location, revealConnection, revealNode, zoomReveal);
        if (i < variables.Length - 1)
          await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0));
      }
    }
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3.0));
    map.Hide();
    map = (UIDLCMapMenuController) null;
    locations = (List<DungeonWorldMapIcon>) null;
  }

  public static async System.Threading.Tasks.Task RevealNodeSequenceAsync(
    DataManager.Variables[] variables,
    DungeonWorldMapIcon.NodeType[] nodeTypes,
    UIDLCMapMenuController.DLCDungeonSide side,
    bool revealConnection = true,
    bool revealNode = true,
    bool zoomReveal = true)
  {
    UIDLCMapMenuController map = MonoSingleton<UIManager>.Instance.ShowDLCMapMenu(PlayerFarming.Instance);
    List<DungeonWorldMapIcon> locations = map.Locations.ToList<DungeonWorldMapIcon>();
    map.LockMenu();
    map.AutoSelectNextNode = false;
    map.Parallax.parallaxToRelativeMousePosition = false;
    map.SetDungeonSide(side);
    int index;
    if (variables != null)
    {
      DataManager.Variables[] variablesArray = variables;
      for (index = 0; index < variablesArray.Length; ++index)
      {
        DataManager.Variables variable = variablesArray[index];
        DungeonWorldMapIcon location = locations.Find((Predicate<DungeonWorldMapIcon>) (l => l.ShowIfVariable == variable));
        if (!((UnityEngine.Object) location == (UnityEngine.Object) null))
        {
          if (revealNode)
            location.Content.transform.localScale = Vector3.zero;
          await map.RevealLocation(location, revealConnection, revealNode, zoomReveal);
          await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3.0));
        }
      }
      variablesArray = (DataManager.Variables[]) null;
    }
    if (nodeTypes != null)
    {
      DungeonWorldMapIcon.NodeType[] nodeTypeArray = nodeTypes;
      for (index = 0; index < nodeTypeArray.Length; ++index)
      {
        DungeonWorldMapIcon.NodeType nodeType = nodeTypeArray[index];
        DungeonWorldMapIcon location = locations.Find((Predicate<DungeonWorldMapIcon>) (l => l.Type == nodeType));
        if (!((UnityEngine.Object) location == (UnityEngine.Object) null))
        {
          if (revealNode)
            location.Content.transform.localScale = Vector3.zero;
          await map.RevealLocation(location, revealConnection, revealNode, zoomReveal);
          await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3.0));
        }
      }
      nodeTypeArray = (DungeonWorldMapIcon.NodeType[]) null;
    }
    map.Hide();
    map = (UIDLCMapMenuController) null;
    locations = (List<DungeonWorldMapIcon>) null;
  }

  public static void BreakDungeonLock(int lockCount, System.Action callback)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) DLCMap.BreakLockRoutine(lockCount, callback));
  }

  public static IEnumerator BreakLockRoutine(int lockCount, System.Action callback)
  {
    yield return (object) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadDLCWorldMapAssets(), (System.Action) null);
    yield return (object) DLCMap.BreakLockSequenceAsync(lockCount >= 3 ? UIDLCMapMenuController.DLCDungeonSide.InsideMountain : UIDLCMapMenuController.DLCDungeonSide.OutsideMountain, lockCount, false).YieldUntilCompleted();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static async System.Threading.Tasks.Task BreakLockSequenceAsync(
    UIDLCMapMenuController.DLCDungeonSide side,
    int lockCount,
    bool revealConnection = true,
    bool revealNode = true)
  {
    UIDLCMapMenuController map = MonoSingleton<UIManager>.Instance.DLCWorldMapTemplate.Instantiate<UIDLCMapMenuController>();
    map.Locations.ToList<DungeonWorldMapIcon>();
    map.LockMenu();
    ++map.Canvas.sortingOrder;
    map.AutoSelectNextNode = false;
    map.Parallax.parallaxToRelativeMousePosition = false;
    map.Show();
    map.SetDungeonSide(side);
    int index = 0;
    int num = 0;
    foreach (DungeonWorldMapIcon location in map._locations)
    {
      if (location.IsLock)
      {
        if (num == lockCount)
        {
          index = location.ID;
          break;
        }
        ++num;
      }
    }
    await map.UnlockLock(map._locations[index]);
    map.Hide();
    map = (UIDLCMapMenuController) null;
  }

  public static void UnlockAllLocks()
  {
    UIDLCMapMenuController worldMapTemplate = MonoSingleton<UIManager>.Instance.DLCWorldMapTemplate;
    List<DungeonWorldMapIcon> list = worldMapTemplate.Locations.ToList<DungeonWorldMapIcon>();
    foreach (DungeonWorldMapIcon location in worldMapTemplate._locations)
    {
      if (location.IsLock)
        DataManager.Instance.DLCDungeonNodesCompleted.Add(list.IndexOf(location));
    }
  }
}
