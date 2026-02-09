// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.ConditionTask
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

public abstract class ConditionTask : Task
{
  [SerializeField]
  public bool _invert;
  [NonSerialized]
  public int yieldReturn = -1;
  public int yields;

  public bool invert
  {
    get => this._invert;
    set => this._invert = value;
  }

  public void Enable(Component agent, IBlackboard bb)
  {
    if (!this.Set(agent, bb))
      return;
    this.OnEnable();
  }

  public void Disable()
  {
    this.isActive = false;
    this.OnDisable();
  }

  public bool CheckCondition(Component agent, IBlackboard blackboard)
  {
    if (!this.isActive || !this.Set(agent, blackboard))
      return false;
    if (this.yieldReturn != -1)
    {
      int num = this.invert ? (this.yieldReturn != 1 ? 1 : 0) : (this.yieldReturn == 1 ? 1 : 0);
      this.yieldReturn = -1;
      return num != 0;
    }
    return !this.invert ? this.OnCheck() : !this.OnCheck();
  }

  public void YieldReturn(bool value)
  {
    if (!this.isActive)
      return;
    this.yieldReturn = value ? 1 : 0;
    this.StartCoroutine(this.Flip());
  }

  public virtual void OnEnable()
  {
  }

  public virtual void OnDisable()
  {
  }

  public virtual bool OnCheck() => true;

  public IEnumerator Flip()
  {
    ++this.yields;
    yield return (object) null;
    --this.yields;
    if (this.yields == 0)
      this.yieldReturn = -1;
  }
}
