// Decompiled with JetBrains decompiler
// Type: UI_Navigator_MoveButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UI_Navigator_MoveButton : BaseMonoBehaviour
{
  public UI_NavigatorSimple UINavigator;
  public GameObject ButtonToMove;
  public CanvasGroup canvasGroup;
  public Vector3 velocity = Vector3.zero;
  public Selectable oldSelectable;
  public bool canvasOff;

  public void Start()
  {
    this.oldSelectable = this.UINavigator.selectable;
    DOTween.Init((bool?) new bool?(), (bool?) new bool?(), (LogBehaviour?) new LogBehaviour?());
  }

  public void Update()
  {
    if (!this.canvasGroup.interactable || (double) this.canvasGroup.alpha == 0.0)
    {
      this.canvasOff = true;
    }
    else
    {
      if (this.canvasOff)
        this.MoveButton();
      if (!((Object) this.UINavigator.selectable != (Object) this.oldSelectable))
        return;
      this.MoveButton();
    }
  }

  public void MoveButton()
  {
    this.canvasOff = false;
    this.oldSelectable = this.UINavigator.selectable;
    this.ButtonToMove.transform.localScale = Vector3.one;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.MoveButtonRoutine());
  }

  public IEnumerator MoveButtonRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    this.ButtonToMove.transform.localScale = Vector3.one;
    this.ButtonToMove.transform.DOShakeScale(0.1f, new Vector3(-0.1f, 0.1f, 1f), 5, fadeOut: false);
    Vector3 targetLocalPosition = this.ButtonToMove.transform.parent.InverseTransformPoint(this.UINavigator.selectable.transform.position);
    Vector3 currentLocalPosition = this.ButtonToMove.transform.localPosition;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime * 5f) <= 1.0)
    {
      this.ButtonToMove.transform.localPosition = Vector3.SmoothDamp(targetLocalPosition, currentLocalPosition, ref this.velocity, Progress);
      yield return (object) null;
    }
    this.ButtonToMove.transform.localScale = Vector3.one;
    yield return (object) null;
  }
}
