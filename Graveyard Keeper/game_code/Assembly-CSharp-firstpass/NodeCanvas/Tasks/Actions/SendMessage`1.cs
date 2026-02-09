// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendMessage`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("SendMessage to the agent, optionaly with an argument")]
[Category("✫ Script Control/Common")]
public class SendMessage<T> : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<string> methodName;
  public BBParameter<T> argument;

  public override string info => $"Message {this.methodName}({this.argument.ToString()})";

  public override void OnExecute()
  {
    if (this.argument.isNull)
      this.agent.SendMessage(this.methodName.value);
    else
      this.agent.SendMessage(this.methodName.value, (object) this.argument.value);
    this.EndAction();
  }
}
