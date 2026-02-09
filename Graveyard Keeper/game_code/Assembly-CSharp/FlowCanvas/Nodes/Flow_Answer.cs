// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_Answer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Answer", 0)]
[Category("Game Functions")]
[Color("00ff00")]
public class Flow_Answer : PureFunctionNode<AnswerData, SmartRes, SmartRes, SmartRes>
{
  public override AnswerData Invoke(SmartRes price, SmartRes @lock, SmartRes reward)
  {
    return new AnswerData()
    {
      d_lock = @lock,
      d_price = price,
      d_reward = reward
    };
  }
}
