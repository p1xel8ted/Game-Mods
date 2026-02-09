// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetPlayerParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (float)})]
[Icon("HumanArrow", false, "")]
[Category("Game Functions")]
[Name("Get Player Param", 0)]
[Color("00ff00")]
public class Flow_GetPlayerParam : PureFunctionNode<float, string>
{
  public override float Invoke(string param) => MainGame.me.player.GetParam(param);
}
