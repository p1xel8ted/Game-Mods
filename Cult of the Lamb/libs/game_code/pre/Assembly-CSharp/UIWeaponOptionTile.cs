// Decompiled with JetBrains decompiler
// Type: UIWeaponOptionTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIWeaponOptionTile : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public RectTransform Container;
  public Image Icon;
  public Image BG;
  public Image SelectedIcon;
  public WeaponUpgradeSystem.WeaponType WeaponType;
  private bool _Paused;

  public Selectable Selectable { get; private set; }

  public bool Paused
  {
    get => this._Paused;
    set
    {
      Debug.Log((object) ("CHANGE VALUE! " + value.ToString()));
      this._Paused = value;
    }
  }

  private void Awake() => this.Selectable = this.GetComponent<Selectable>();

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

  private IEnumerator Selected(float Starting, float Target)
  {
    while (this.Paused)
      yield return (object) null;
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(Starting, Target, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one * Target;
  }

  private IEnumerator DeSelected()
  {
    while (this.Paused)
      yield return (object) null;
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = this.Container.localScale.x;
    float TargetScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Container.localScale = Vector3.one * Mathf.SmoothStep(StartingScale, TargetScale, Progress / Duration);
      yield return (object) null;
    }
    this.Container.localScale = Vector3.one;
  }
}
