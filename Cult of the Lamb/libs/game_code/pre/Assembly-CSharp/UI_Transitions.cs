// Decompiled with JetBrains decompiler
// Type: UI_Transitions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UI_Transitions : BaseMonoBehaviour
{
  public Transform Container;
  public Vector3 StartPos;
  public Vector3 MovePos;
  public bool Hidden;
  public bool Revealed;

  private void Start()
  {
  }

  private void OnEnable()
  {
  }

  private void GetStartingPos() => this.StartPos = this.Container.localPosition;

  public void MoveBackOutFunction()
  {
    this.StopAllCoroutines();
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.MoveBarOut());
  }

  public void MoveBackInFunction()
  {
    this.StopAllCoroutines();
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.MoveBarIn());
  }

  public IEnumerator MoveBarOut()
  {
    this.Hidden = true;
    Vector3 currentPos = this.Container.localPosition;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localPosition = Vector3.Lerp(currentPos, this.MovePos, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.Container.localPosition = this.MovePos;
    this.Revealed = false;
  }

  public void hideBar()
  {
    this.Hidden = true;
    this.Revealed = false;
    if (!((Object) this.Container != (Object) null))
      return;
    this.Container.localPosition = this.MovePos;
  }

  public IEnumerator MoveBarIn()
  {
    Vector3 currentPos = this.Container.localPosition;
    this.Hidden = false;
    float Progress = 0.0f;
    float Duration = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localPosition = Vector3.Lerp(currentPos, this.StartPos, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.Container.localPosition = this.StartPos;
    this.Revealed = true;
  }

  private void OnDrawGizmos()
  {
    Gizmos.matrix = this.transform.parent.transform.localToWorldMatrix;
    Gizmos.DrawLine(this.StartPos, this.MovePos);
    Gizmos.DrawSphere(this.StartPos, 5f);
    Gizmos.DrawSphere(this.MovePos, 5f);
  }

  private void Update()
  {
  }
}
