// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetGarryWashTalkPos
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
[Category("Game Functions")]
[Color("eed9a7")]
[Name("Get Garry Wash Talks Pos", 0)]
public class Flow_GetGarryWashTalkPos : PureFunctionNode<Transform>
{
  public override Transform Invoke()
  {
    return MainGame.me.player.GetComponent<PlayerComponent>().garry_wash_talk_pos.transform;
  }
}
