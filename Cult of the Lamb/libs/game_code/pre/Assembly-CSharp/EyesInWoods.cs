// Decompiled with JetBrains decompiler
// Type: EyesInWoods
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EyesInWoods : BaseMonoBehaviour
{
  public Animator animator;
  public bool toggle;
  public float waitTime;

  private void Start()
  {
    this.waitTime = Random.Range(4f, 14f);
    this.StartCoroutine((IEnumerator) this.WaitToChangeState());
  }

  private void TurnOn()
  {
    Debug.Log((object) "on");
    this.waitTime = Random.Range(4f, 14f);
    this.toggle = true;
    this.animator.Play("Eyes-In");
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.WaitToChangeState());
  }

  private void TurnOff()
  {
    Debug.Log((object) "off");
    this.waitTime = Random.Range(4f, 14f);
    this.toggle = false;
    this.animator.Play("Eyes-Out");
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.WaitToChangeState());
  }

  private IEnumerator WaitToChangeState()
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
