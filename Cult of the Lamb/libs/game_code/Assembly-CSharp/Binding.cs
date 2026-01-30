// Decompiled with JetBrains decompiler
// Type: Binding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct Binding
{
  public KeyCode KeyCode;
  public ControllerType ControllerType;
  public Pole AxisContribution;
  public AxisRange AxisRange;
  public AxisType AxisType;
  public int Category;
  public int Action;
  public int ElementIdentifierID;

  public ElementAssignment ToElementAssigment()
  {
    if (this.ControllerType == ControllerType.Keyboard)
      return new ElementAssignment()
      {
        keyboardKey = this.KeyCode,
        actionId = this.Action,
        axisContribution = this.AxisContribution,
        axisRange = this.AxisRange
      };
    if (this.ControllerType != ControllerType.Mouse && this.ControllerType != ControllerType.Joystick)
      return new ElementAssignment();
    return new ElementAssignment()
    {
      elementIdentifierId = this.ElementIdentifierID,
      actionId = this.Action,
      axisContribution = this.AxisContribution,
      axisRange = this.AxisRange,
      type = this.AxisType.ToElementAssignmentType()
    };
  }

  public ElementAssignment ToElementAssignment(ActionElementMap actionElementMap)
  {
    ElementAssignment elementAssigment = this.ToElementAssigment();
    if (actionElementMap != null)
      elementAssigment.elementMapId = actionElementMap.id;
    return elementAssigment;
  }
}
