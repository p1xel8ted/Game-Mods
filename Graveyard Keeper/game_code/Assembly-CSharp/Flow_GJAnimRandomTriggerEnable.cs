// Decompiled with JetBrains decompiler
// Type: Flow_GJAnimRandomTriggerEnable
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
[Name("GJ Trigger Animation Roll Enable", 0)]
[Category("Game Actions")]
public class Flow_GJAnimRandomTriggerEnable : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<GameObject> game_object_in;
  public ValueInput<bool> enable_flag_in;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.SetActive));
    this.@out = this.AddFlowOutput("Out");
    this.game_object_in = this.AddValueInput<GameObject>("Game Object");
    this.enable_flag_in = this.AddValueInput<bool>("Is Active");
  }

  public void SetActive(Flow flow)
  {
    if ((Object) this.game_object_in.value != (Object) null)
    {
      GJAnimRandomTrigger componentInChildren = this.game_object_in.value.GetComponentInChildren<GJAnimRandomTrigger>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.SetRollActive(this.enable_flag_in.value);
      else
        Debug.LogError((object) ("GJAnimRandomTrigger component not found on object" + this.game_object_in.value?.ToString()));
    }
    else
      Debug.LogError((object) "GameObject is null");
    this.@out.Call(flow);
  }

  public override string name
  {
    get => "GJ Trigger Animation Roll " + (this.enable_flag_in.value ? "Enable" : "Disable");
  }
}
