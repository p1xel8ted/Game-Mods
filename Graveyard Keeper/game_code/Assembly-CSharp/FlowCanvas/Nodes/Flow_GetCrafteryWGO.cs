// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetCrafteryWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Craftery WGO", 0)]
[Category("Game Functions")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
public class Flow_GetCrafteryWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => GUIElements.me.craft.GetCrafteryWGO()));
  }
}
