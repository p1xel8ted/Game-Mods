// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ShoutEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Description("Sends an event to all GraphOwners within range of the agent and over time like a shockwave.")]
public class ShoutEvent : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<string> eventName;
  public BBParameter<float> shoutRange = (BBParameter<float>) 10f;
  public BBParameter<float> completionTime = (BBParameter<float>) 1f;
  public GraphOwner[] owners;
  public List<GraphOwner> receivedOwners = new List<GraphOwner>();
  public float traveledDistance;

  public override string info => $"Shout Event [{this.eventName.ToString()}]";

  public override void OnExecute()
  {
    this.owners = Object.FindObjectsOfType<GraphOwner>();
    this.receivedOwners.Clear();
  }

  public override void OnUpdate()
  {
    this.traveledDistance = Mathf.Lerp(0.0f, this.shoutRange.value, this.elapsedTime / this.completionTime.value);
    foreach (GraphOwner owner in this.owners)
    {
      if ((double) (this.agent.position - owner.transform.position).magnitude <= (double) this.traveledDistance && !this.receivedOwners.Contains(owner))
      {
        owner.SendEvent(this.eventName.value);
        this.receivedOwners.Add(owner);
      }
    }
    if ((double) this.elapsedTime < (double) this.completionTime.value)
      return;
    this.EndAction();
  }

  public override void OnDrawGizmosSelected()
  {
    if (!((Object) this.agent != (Object) null))
      return;
    Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
    Gizmos.DrawWireSphere(this.agent.position, this.traveledDistance);
    Gizmos.DrawWireSphere(this.agent.position, this.shoutRange.value);
  }
}
