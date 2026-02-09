// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.InputCustomAxisEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("You are free to define any Input Axis in this node.\nAxis can be set in 'Project Settings/Input'.\nCalls Out when either of the Axis defined is not zero")]
[Name("Input Axis (Custom)", 0)]
[Category("Events/Input")]
public class InputCustomAxisEvent : EventNode, IUpdatable
{
  public BBParameter<List<string>> axis = (BBParameter<List<string>>) new List<string>()
  {
    "Horizontal",
    "Vertical"
  };
  public float[] axisValues;
  public bool calledLastFrame;
  public FlowOutput o;

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Out");
    this.axisValues = new float[this.axis.value.Count + 1];
    for (int index = 0; index < this.axis.value.Count; ++index)
    {
      int i = index;
      if (!string.IsNullOrEmpty(this.axis.value[i]))
        this.AddValueOutput<float>(this.axis.value[i], (ValueHandler<float>) (() => this.axisValues[i]), i.ToString());
    }
  }

  public void Update()
  {
    List<string> stringList = this.axis.value;
    bool flag = false;
    for (int index = 0; index < stringList.Count; ++index)
    {
      if (!string.IsNullOrEmpty(stringList[index]))
      {
        float axis = Input.GetAxis(stringList[index]);
        this.axisValues[index] = axis;
        if ((double) axis != 0.0)
          flag = true;
      }
    }
    if (flag)
    {
      this.o.Call(new Flow());
      this.calledLastFrame = true;
    }
    if (flag || !this.calledLastFrame)
      return;
    this.o.Call(new Flow());
    this.calledLastFrame = false;
  }
}
