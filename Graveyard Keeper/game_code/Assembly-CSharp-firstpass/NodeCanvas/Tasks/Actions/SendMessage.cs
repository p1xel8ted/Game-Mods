// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendMessage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Script Control/Common")]
[Description("SendMessage to the agent, optionaly with an argument")]
public class SendMessage : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<string> methodName;

  public override string info => $"Message {this.methodName}()";

  public override void OnExecute()
  {
    this.agent.SendMessage(this.methodName.value);
    this.EndAction();
  }
}
