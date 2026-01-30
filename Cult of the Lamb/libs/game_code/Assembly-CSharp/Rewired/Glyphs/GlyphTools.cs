// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Rewired.Glyphs;

public static class GlyphTools
{
  public static bool TryGetActionElementMaps(
    int playerId,
    int actionId,
    AxisRange actionRange,
    ControllerElementGlyphSelectorOptions options,
    List<ActionElementMap> workingActionElementMaps,
    out ActionElementMap aemResult1,
    out ActionElementMap aemResult2)
  {
    aemResult1 = (ActionElementMap) null;
    aemResult2 = (ActionElementMap) null;
    if (!ReInput.isReady || options == null || workingActionElementMaps == null)
      return false;
    InputAction action = ReInput.mapping.GetAction(actionId);
    if (action == null)
      return false;
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return false;
    Controller controller1 = player.controllers.GetLastActiveController();
    workingActionElementMaps.Clear();
    if (options.useLastActiveController && controller1 != null)
    {
      Controller controller2 = (Controller) null;
      if (controller1.type == ControllerType.Keyboard || controller1.type == ControllerType.Mouse)
      {
        if (GlyphTools.IsMousePrioritizedOverKeyboard(options))
        {
          if (ReInput.controllers.Mouse.enabled && player.controllers.hasMouse)
          {
            controller1 = (Controller) ReInput.controllers.Mouse;
            controller2 = (Controller) ReInput.controllers.Keyboard;
          }
        }
        else if (ReInput.controllers.Keyboard.enabled && player.controllers.hasKeyboard)
        {
          controller1 = (Controller) ReInput.controllers.Keyboard;
          controller2 = (Controller) ReInput.controllers.Mouse;
        }
      }
      if (GlyphTools.GetElementMapsWithAction(player, controller1.type, controller1.id, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2) || controller2 != null && GlyphTools.GetElementMapsWithAction(player, controller2.type, controller2.id, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2) || GlyphTools.GetElementMapsWithAction(player, controller1.type, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2))
        return true;
    }
    ControllerType controllerType;
    for (int index = 0; options.TryGetControllerTypeOrder(index, out controllerType); ++index)
    {
      if (GlyphTools.GetElementMapsWithAction(player, controllerType, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2))
        return true;
    }
    return GlyphTools.GetElementMapsWithAction(player, actionId, true, workingActionElementMaps) > 0 && GlyphTools.TryGetActionElementMaps(action, actionRange, workingActionElementMaps, out aemResult1, out aemResult2);
  }

  public static bool TryGetActionElementMaps(
    InputAction action,
    AxisRange actionRange,
    List<ActionElementMap> tempAems,
    out ActionElementMap aemResult1,
    out ActionElementMap aemResult2)
  {
    aemResult1 = (ActionElementMap) null;
    aemResult2 = (ActionElementMap) null;
    bool flag = action.type == InputActionType.Axis;
    int count = tempAems.Count;
    for (int index = 0; index < count; ++index)
    {
      if (flag)
      {
        if (actionRange == AxisRange.Full)
        {
          ActionElementMap negativeAem = GlyphTools.FindFirstFullAxisBinding(tempAems);
          if (negativeAem != null)
          {
            aemResult1 = negativeAem;
            return true;
          }
          ActionElementMap positiveAem;
          if (GlyphTools.FindFirstSplitAxisBindingPair(tempAems, out negativeAem, out positiveAem))
          {
            aemResult1 = negativeAem;
            aemResult2 = positiveAem;
            return true;
          }
        }
        else
        {
          ActionElementMap firstBinding = GlyphTools.FindFirstBinding(tempAems, actionRange);
          if (firstBinding != null)
          {
            aemResult1 = firstBinding;
            return true;
          }
        }
      }
      else
      {
        ActionElementMap firstBinding = GlyphTools.FindFirstBinding(tempAems, actionRange);
        if (firstBinding != null)
        {
          aemResult1 = firstBinding;
          return true;
        }
      }
    }
    return false;
  }

  public static ActionElementMap FindFirstFullAxisBinding(List<ActionElementMap> actionElementMaps)
  {
    int count = actionElementMaps.Count;
    for (int index = 0; index < count; ++index)
    {
      ActionElementMap actionElementMap = actionElementMaps[index];
      if (actionElementMap.elementType == ControllerElementType.Axis && actionElementMap.axisType == AxisType.Normal)
        return actionElementMap;
    }
    return (ActionElementMap) null;
  }

  public static ActionElementMap FindFirstBinding(
    List<ActionElementMap> actionElementMaps,
    AxisRange actionRange)
  {
    if (actionElementMaps.Count == 0)
      return (ActionElementMap) null;
    int count = actionElementMaps.Count;
    for (int index = 0; index < count; ++index)
    {
      ActionElementMap actionElementMap = actionElementMaps[index];
      switch (actionRange)
      {
        case AxisRange.Full:
          if (actionElementMap.axisRange == AxisRange.Full)
            return actionElementMap;
          break;
        case AxisRange.Positive:
          if ((actionElementMap.axisType == AxisType.Split || actionElementMap.axisType == AxisType.None) && actionElementMap.axisContribution == Pole.Positive)
            return actionElementMap;
          break;
        case AxisRange.Negative:
          if ((actionElementMap.axisType == AxisType.Split || actionElementMap.axisType == AxisType.None) && actionElementMap.axisContribution == Pole.Negative)
            return actionElementMap;
          break;
      }
    }
    if (actionRange == AxisRange.Full)
    {
      for (int index = 0; index < count; ++index)
      {
        ActionElementMap actionElementMap = actionElementMaps[index];
        if ((actionElementMap.axisType == AxisType.Split || actionElementMap.axisType == AxisType.None) && actionElementMap.axisContribution == Pole.Positive)
          return actionElementMap;
      }
    }
    return (ActionElementMap) null;
  }

  public static bool FindFirstSplitAxisBindingPair(
    List<ActionElementMap> actionElementMaps,
    out ActionElementMap negativeAem,
    out ActionElementMap positiveAem)
  {
    negativeAem = (ActionElementMap) null;
    positiveAem = (ActionElementMap) null;
    int count = actionElementMaps.Count;
    for (int index = 0; index < count; ++index)
    {
      ActionElementMap actionElementMap = actionElementMaps[index];
      if (actionElementMap.elementType == ControllerElementType.Axis)
      {
        if (actionElementMap.axisType == AxisType.Normal || actionElementMap.axisType == AxisType.None)
          continue;
      }
      else if (actionElementMap.elementType != ControllerElementType.Button)
        continue;
      if (actionElementMap.axisContribution == Pole.Positive)
      {
        if (positiveAem == null)
          positiveAem = actionElementMap;
      }
      else if (negativeAem == null)
        negativeAem = actionElementMap;
    }
    return negativeAem != null || positiveAem != null;
  }

  public static bool IsMousePrioritizedOverKeyboard(ControllerElementGlyphSelectorOptions options)
  {
    if (options == null)
      return false;
    ControllerType controllerType;
    for (int index = 0; options.TryGetControllerTypeOrder(index, out controllerType); ++index)
    {
      if (controllerType == ControllerType.Mouse)
        return true;
      if (controllerType == ControllerType.Keyboard)
        return false;
    }
    return false;
  }

  public static int GetElementMapsWithAction(
    Player player,
    ControllerType controllerType,
    int controllerId,
    int actionId,
    bool skipDisabledMaps,
    List<ActionElementMap> results)
  {
    int count = results.Count;
    player.controllers.maps.GetElementMapsWithAction(controllerType, controllerId, actionId, skipDisabledMaps, (List<ActionElementMap>) results);
    GlyphTools.RemoveInvalidElementMaps(player, results, count);
    return results.Count - count;
  }

  public static int GetElementMapsWithAction(
    Player player,
    ControllerType controllerType,
    int actionId,
    bool skipDisabledMaps,
    List<ActionElementMap> results)
  {
    int count = results.Count;
    player.controllers.maps.GetElementMapsWithAction(controllerType, actionId, skipDisabledMaps, (List<ActionElementMap>) results);
    GlyphTools.RemoveInvalidElementMaps(player, results, count);
    return results.Count - count;
  }

  public static int GetElementMapsWithAction(
    Player player,
    int actionId,
    bool skipDisabledMaps,
    List<ActionElementMap> results)
  {
    int count = results.Count;
    player.controllers.maps.GetElementMapsWithAction(actionId, skipDisabledMaps, (List<ActionElementMap>) results);
    GlyphTools.RemoveInvalidElementMaps(player, results, count);
    return results.Count - count;
  }

  public static int RemoveInvalidElementMaps(
    Player player,
    List<ActionElementMap> results,
    int startIndex)
  {
    int count = results.Count;
    for (int index = count - 1; index >= startIndex; --index)
    {
      if (!player.controllers.ContainsController(results[index].controllerMap.controller) || !results[index].controllerMap.controller.enabled)
        results.RemoveAt(index);
    }
    return count - results.Count;
  }
}
