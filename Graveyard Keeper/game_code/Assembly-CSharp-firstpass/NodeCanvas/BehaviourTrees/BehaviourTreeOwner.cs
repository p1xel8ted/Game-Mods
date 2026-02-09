// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.BehaviourTreeOwner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[AddComponentMenu("NodeCanvas/Behaviour Tree Owner")]
public class BehaviourTreeOwner : GraphOwner<BehaviourTree>
{
  public bool repeat
  {
    get => !((Object) this.behaviour != (Object) null) || this.behaviour.repeat;
    set
    {
      if (!((Object) this.behaviour != (Object) null))
        return;
      this.behaviour.repeat = value;
    }
  }

  public float updateInterval
  {
    get => !((Object) this.behaviour != (Object) null) ? 0.0f : this.behaviour.updateInterval;
    set
    {
      if (!((Object) this.behaviour != (Object) null))
        return;
      this.behaviour.updateInterval = value;
    }
  }

  public NodeCanvas.Status rootStatus
  {
    get => !((Object) this.behaviour != (Object) null) ? NodeCanvas.Status.Resting : this.behaviour.rootStatus;
  }

  public NodeCanvas.Status Tick()
  {
    if (!((Object) this.behaviour == (Object) null))
      return this.behaviour.Tick((Component) this, this.blackboard);
    Debug.LogWarning((object) "There is no Behaviour Tree assigned", (Object) this.gameObject);
    return NodeCanvas.Status.Resting;
  }
}
