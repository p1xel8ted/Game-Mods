// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetPlayerParamInt
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("00ff00")]
[Category("Game Functions")]
[Name("Get Player Param Int", 0)]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (int)})]
[Icon("HumanArrow", false, "")]
public class Flow_GetPlayerParamInt : PureFunctionNode<int, string>
{
  public override int Invoke(string param) => MainGame.me.player.GetParamInt(param);
}
