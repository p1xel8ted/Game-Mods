// Decompiled with JetBrains decompiler
// Type: UIFollowerLocationSelectIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIFollowerLocationSelectIcon : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public FollowerBrain Brain;
  public Image SelectedIcon;
  public RectTransform Container;
  public TextMeshProUGUI Name;

  public void Play(FollowerBrain Brain)
  {
    this.Brain = Brain;
    this.Name.text = Brain.Info.Name;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.SelectedIcon.enabled = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Selected(this.Container.localScale.x, 1.3f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.SelectedIcon.enabled = false;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public IEnumerator Selected(float Starting, float Target)
  {
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(Starting, Target, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * Target;
  }

  public IEnumerator DeSelected()
  {
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = this.Container.localScale.x;
    float TargetScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(StartingScale, TargetScale, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * TargetScale;
  }
}
