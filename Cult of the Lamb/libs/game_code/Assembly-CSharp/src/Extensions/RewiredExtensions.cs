// Decompiled with JetBrains decompiler
// Type: src.Extensions.RewiredExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;

#nullable disable
namespace src.Extensions;

public static class RewiredExtensions
{
  public static AxisRange ToAxisRange(this Pole pole)
  {
    return pole == Pole.Positive ? AxisRange.Positive : AxisRange.Negative;
  }

  public static ElementAssignmentType ToElementAssignmentType(this AxisType axisType)
  {
    return axisType == AxisType.Split ? ElementAssignmentType.SplitAxis : ElementAssignmentType.Button;
  }

  public static Binding ToBinding(this ActionElementMap actionElementMap)
  {
    return new Binding()
    {
      Action = actionElementMap.actionId,
      ControllerType = actionElementMap.controllerMap.controllerType,
      AxisContribution = actionElementMap.axisContribution,
      AxisRange = actionElementMap.axisRange,
      AxisType = actionElementMap.axisType,
      KeyCode = actionElementMap.keyCode,
      ElementIdentifierID = actionElementMap.elementIdentifierId,
      Category = actionElementMap.controllerMap.categoryId
    };
  }

  public static UnboundBinding ToUnboundBinding(this ActionElementMap actionElementMap)
  {
    return new UnboundBinding()
    {
      Action = actionElementMap.actionId,
      AxisContribution = actionElementMap.axisContribution,
      Category = actionElementMap.controllerMap.categoryId
    };
  }

  public static ActionElementMap GetActionElementMap(
    this ControllerMap controllerMap,
    int action,
    Pole axisContribution)
  {
    foreach (ActionElementMap actionElementMap in controllerMap.GetElementMapsWithAction(action))
    {
      if (actionElementMap.axisContribution == axisContribution)
        return actionElementMap;
    }
    return (ActionElementMap) null;
  }
}
