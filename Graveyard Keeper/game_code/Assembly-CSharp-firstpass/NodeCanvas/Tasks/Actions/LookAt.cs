// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.LookAt
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class LookAt : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> lookTarget;
  public bool repeat;

  public override string info => "LookAt " + this.lookTarget?.ToString();

  public override void OnExecute() => this.DoLook();

  public override void OnUpdate() => this.DoLook();

  public void DoLook()
  {
    this.agent.LookAt(this.lookTarget.value.transform.position with
    {
      y = this.agent.position.y
    });
    if (this.repeat)
      return;
    this.EndAction(true);
  }
}
