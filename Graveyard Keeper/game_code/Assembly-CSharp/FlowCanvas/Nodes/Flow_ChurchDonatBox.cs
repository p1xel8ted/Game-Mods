// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ChurchDonatBox
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
[Name("Get Church Donat Box", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[Icon("Human", false, "")]
public class Flow_ChurchDonatBox : PureFunctionNode<WorldGameObject>
{
  public override WorldGameObject Invoke()
  {
    return WorldMap.GetWorldGameObjectByCustomTag("donat_box_inside");
  }
}
