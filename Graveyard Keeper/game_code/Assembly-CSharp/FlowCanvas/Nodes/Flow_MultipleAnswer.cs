// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_MultipleAnswer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Multiple Answer", 0)]
[Category("Game Functions")]
[Color("00ff00")]
public class Flow_MultipleAnswer : PureFunctionNode<MultipleAnswerData, List<AnswerData>, SmartRes>
{
  public override MultipleAnswerData Invoke(List<AnswerData> datas, SmartRes reward)
  {
    return new MultipleAnswerData(reward, datas);
  }
}
