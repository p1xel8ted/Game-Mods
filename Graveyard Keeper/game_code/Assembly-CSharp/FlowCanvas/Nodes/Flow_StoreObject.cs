// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StoreObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Store Object", 0)]
public class Flow_StoreObject : MyFlowNode
{
  public object _obj;

  public override void RegisterPorts()
  {
    ValueInput<object> in_object = this.AddValueInput<object>("Object");
    this.AddValueOutput<object>("out Object", (ValueHandler<object>) (() => this._obj));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this._obj = in_object.value;
      flow_out.Call(f);
    }));
  }
}
