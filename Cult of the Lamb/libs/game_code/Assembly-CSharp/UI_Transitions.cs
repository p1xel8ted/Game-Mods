// Decompiled with JetBrains decompiler
// Type: UI_Transitions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UI_Transitions : BaseMonoBehaviour
{
  public Transform Container;
  public bool UseAnchoredPosition;
  public Vector3 StartPos;
  public Vector3 MovePos;
  [Tooltip("Note: using coop positions needs to be triggered manually in code via SetCoopPositions()")]
  public bool CoopPositions;
  public Vector3 StartPosCoop;
  public Vector3 MovePosCoop;
  public bool Hidden;
  public bool Revealed;
  public Vector3 currentStartPos;
  public Vector3 currentMovePos;
  public RectTransform _rectTransform;

  public void Awake()
  {
    this.InitializeRectTransform();
    this.currentStartPos = this.StartPos;
    this.currentMovePos = this.MovePos;
  }

  public void Start()
  {
  }

  public void OnEnable()
  {
  }

  public void InitializeRectTransform()
  {
    if ((bool) (Object) this._rectTransform)
      return;
    this._rectTransform = this.GetComponent<RectTransform>();
  }

  public bool IsRectTransform() => this.transform is RectTransform;

  public void GetStartingPos()
  {
    this.InitializeRectTransform();
    if (this.UseAnchoredPosition)
      this.StartPos = (Vector3) this._rectTransform.anchoredPosition;
    else
      this.StartPos = this.Container.localPosition;
  }

  public void MoveBackOutFunction()
  {
    this.InitializeRectTransform();
    this.StopAllCoroutines();
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine(this.MoveBarOut());
  }

  public void MoveBackInFunction()
  {
    this.InitializeRectTransform();
    this.StopAllCoroutines();
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine(this.MoveBarIn());
  }

  public void SetCoopPositions(bool state)
  {
    this.currentStartPos = state ? this.StartPosCoop : this.StartPos;
    this.currentMovePos = state ? this.MovePosCoop : this.MovePos;
  }

  public IEnumerator MoveBarOut()
  {
    this.Hidden = true;
    Vector3 currentPos = !this.UseAnchoredPosition ? this.Container.localPosition : (Vector3) this._rectTransform.anchoredPosition;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      Vector3 vector3 = Vector3.Lerp(currentPos, this.currentMovePos, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      if (this.UseAnchoredPosition)
        this._rectTransform.anchoredPosition = (Vector2) vector3;
      else
        this.Container.localPosition = vector3;
      yield return (object) null;
    }
    if (this.UseAnchoredPosition)
      this._rectTransform.anchoredPosition = (Vector2) this.currentMovePos;
    else
      this.Container.localPosition = this.currentMovePos;
    this.Revealed = false;
  }

  public void hideBar()
  {
    this.InitializeRectTransform();
    this.StopAllCoroutines();
    this.Hidden = true;
    this.Revealed = false;
    if (!((Object) this.Container != (Object) null))
      return;
    if (this.UseAnchoredPosition)
      this._rectTransform.anchoredPosition = (Vector2) this.currentMovePos;
    else
      this.Container.localPosition = this.currentMovePos;
  }

  public IEnumerator MoveBarIn()
  {
    Vector3 currentPos = !this.UseAnchoredPosition ? this.Container.localPosition : (Vector3) this._rectTransform.anchoredPosition;
    this.Hidden = false;
    float Progress = 0.0f;
    float Duration = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      Vector3 vector3 = Vector3.Lerp(currentPos, this.currentStartPos, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      if (this.UseAnchoredPosition)
        this._rectTransform.anchoredPosition = (Vector2) vector3;
      else
        this.Container.localPosition = vector3;
      yield return (object) null;
    }
    if (this.UseAnchoredPosition)
      this._rectTransform.anchoredPosition = (Vector2) this.currentStartPos;
    else
      this.Container.localPosition = this.currentStartPos;
    this.Revealed = true;
  }

  public void OnDrawGizmos()
  {
    Gizmos.matrix = this.transform.parent.transform.localToWorldMatrix;
    Gizmos.DrawLine(this.StartPos, this.MovePos);
    Gizmos.DrawSphere(this.StartPos, 5f);
    Gizmos.DrawSphere(this.MovePos, 5f);
  }

  public void Update()
  {
  }
}
