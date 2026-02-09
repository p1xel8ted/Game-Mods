// Decompiled with JetBrains decompiler
// Type: Flow_WaitForFlow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
[Name("Wait For Flow", 0)]
[Description("This block is like DoOnce one, but waits flow as many as set")]
public class Flow_WaitForFlow : MyFlowNode
{
  [SerializeField]
  public int inputFlowsWaitCount = 2;
  public FlowInput reset;
  public FlowOutput waitEnded;
  public FlowOutput whileWaiting;
  public bool[] activated;

  public override void RegisterPorts()
  {
    this.activated = new bool[this.inputFlowsWaitCount];
    for (int index = 0; index < this.inputFlowsWaitCount; ++index)
    {
      int j = index;
      this.AddFlowInput(j.ToString(), (FlowHandler) (flow => this.Check(j, flow)));
    }
    this.reset = this.AddFlowInput("Reset", new FlowHandler(this.Reset));
    this.waitEnded = this.AddFlowOutput("Wait End");
    this.whileWaiting = this.AddFlowOutput("While Waiting");
  }

  public void Check(int index, Flow flow)
  {
    this.activated[index] = true;
    if (this.CheckFlowsActivated())
    {
      for (int index1 = 0; index1 < this.inputFlowsWaitCount; ++index1)
        this.activated[index1] = false;
      this.waitEnded.Call(flow);
    }
    else
      this.whileWaiting.Call(flow);
  }

  public bool CheckFlowsActivated()
  {
    for (int index = 0; index < this.inputFlowsWaitCount; ++index)
    {
      if (!this.activated[index])
        return false;
    }
    return true;
  }

  public void Reset(Flow flow)
  {
    for (int index = 0; index < this.inputFlowsWaitCount; ++index)
      this.activated[index] = false;
    this.waitEnded.Call(flow);
  }

  public override string name => $"Wait For {this.inputFlowsWaitCount} Flow";

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    if (!GUILayout.Button("Refresh"))
      return;
    this.GatherPorts();
  }
}
