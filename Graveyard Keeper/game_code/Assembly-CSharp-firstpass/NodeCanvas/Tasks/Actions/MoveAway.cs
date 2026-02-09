// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MoveAway
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Moves the agent away from target per frame without pathfinding")]
[Category("Movement/Direct")]
public class MoveAway : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public BBParameter<float> speed = (BBParameter<float>) 2f;
  public BBParameter<float> stopDistance = (BBParameter<float>) 3f;
  public bool waitActionFinish;

  public override void OnUpdate()
  {
    if ((double) (this.agent.position - this.target.value.transform.position).magnitude >= (double) this.stopDistance.value)
    {
      this.EndAction();
    }
    else
    {
      this.agent.position = Vector3.MoveTowards(this.agent.position, this.target.value.transform.position, -this.speed.value * Time.deltaTime);
      if (this.waitActionFinish)
        return;
      this.EndAction();
    }
  }
}
