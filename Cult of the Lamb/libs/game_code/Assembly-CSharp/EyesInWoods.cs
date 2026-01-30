// Decompiled with JetBrains decompiler
// Type: EyesInWoods
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EyesInWoods : BaseMonoBehaviour
{
  public Animator animator;
  public bool toggle;
  public float waitTime;

  public void Start()
  {
    this.waitTime = Random.Range(4f, 14f);
    this.StartCoroutine((IEnumerator) this.WaitToChangeState());
  }

  public void TurnOn()
  {
    Debug.Log((object) "on");
    this.waitTime = Random.Range(4f, 14f);
    this.toggle = true;
    this.animator.Play("Eyes-In");
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.WaitToChangeState());
  }

  public void TurnOff()
  {
    Debug.Log((object) "off");
    this.waitTime = Random.Range(4f, 14f);
    this.toggle = false;
    this.animator.Play("Eyes-Out");
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.WaitToChangeState());
  }

  public IEnumerator WaitToChangeState()
  {
    Debug.Log((object) "Started Coroutine");
    yield return (object) new WaitForSecondsRealtime(5f);
    Debug.Log((object) "WaitTimeOver");
    if (this.toggle)
      this.TurnOff();
    else
      this.TurnOn();
  }
}
