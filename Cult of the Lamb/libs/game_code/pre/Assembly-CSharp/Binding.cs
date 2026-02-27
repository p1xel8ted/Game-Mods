// Decompiled with JetBrains decompiler
// Type: Binding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
