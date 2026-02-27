// Decompiled with JetBrains decompiler
// Type: CoroutineQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CoroutineQueue
{
  private BaseMonoBehaviour m_Owner;
  private Coroutine m_InternalCoroutine;
  private Queue<IEnumerator> actions = new Queue<IEnumerator>();

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

  private IEnumerator Process()
  {
    while (true)
    {
      while (this.actions.Count <= 0)
        yield return (object) null;
      yield return (object) this.m_Owner.StartCoroutine((IEnumerator) this.actions.Dequeue());
    }
  }
}
