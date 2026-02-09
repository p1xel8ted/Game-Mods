// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetPlayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Player", 0)]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
[Category("Game Functions")]
[Color("eed9a7")]
[Icon("Human", false, "")]
public class Flow_GetPlayer : PureFunctionNode<WorldGameObject>
{
  public override WorldGameObject Invoke() => MainGame.me.player;
}
