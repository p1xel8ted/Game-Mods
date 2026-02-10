// Decompiled with JetBrains decompiler
// Type: CoroutineQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CoroutineQueue
{
  public BaseMonoBehaviour m_Owner;
  public Coroutine m_InternalCoroutine;
  public Queue<IEnumerator> actions = new Queue<IEnumerator>();

  public Queue<IEnumerator> Actions => this.actions;

  public CoroutineQueue(BaseMonoBehaviour aCoroutineOwner) => this.m_Owner = aCoroutineOwner;

  public void StartLoop()
  {
    this.m_InternalCoroutine = this.m_Owner.StartCoroutine((IEnumerator) this.Process());
  }

  public void StopLoop()
  {
    this.m_Owner.StopCoroutine(this.m_InternalCoroutine);
    this.m_InternalCoroutine = (Coroutine) null;
  }

  public void EnqueueAction(IEnumerator aAction) => this.actions.Enqueue(aAction);

  public IEnumerator Process()
  {
    while (true)
    {
      while (this.actions.Count <= 0)
        yield return (object) null;
      yield return (object) this.m_Owner.StartCoroutine((IEnumerator) this.actions.Dequeue());
    }
  }
}
