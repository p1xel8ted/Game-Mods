// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetObjectActive
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Set Visibility", 0)]
[Category("GameObject")]
public class SetObjectActive : ActionTask<Transform>
{
  public SetObjectActive.SetActiveMode setTo = SetObjectActive.SetActiveMode.Toggle;

  public override string info => $"{this.setTo} {this.agentInfo}";

  public override void OnExecute()
  {
    this.agent.gameObject.SetActive(this.setTo != SetObjectActive.SetActiveMode.Toggle ? this.setTo == SetObjectActive.SetActiveMode.Activate : !this.agent.gameObject.activeSelf);
    this.EndAction();
  }

  public enum SetActiveMode
  {
    Deactivate,
    Activate,
    Toggle,
  }
}
