// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public abstract class MMTab : MonoBehaviour
{
  protected const string kInactiveLayer = "Inactive";
  protected const string kActiveLayer = "Active";
  public System.Action OnTabPressed;
  [SerializeField]
  protected MMButton _button;
  [SerializeField]
  protected UIMenuBase _menu;
  [SerializeField]
  protected RectTransform _rectTransform;
  [SerializeField]
  protected Animator _animator;
  private float _inactiveWeight = 1f;
  private float _activeWeight;

  public MMButton Button => this._button;

  public UIMenuBase Menu => this._menu;

  public RectTransform RectTransform => this._rectTransform;

  public abstract void Configure();

  public void Awake()
  {
    this._menu.OnShow += new System.Action(this.SetActive);
    this._menu.OnHide += new System.Action(this.SetInactive);
    this._button.onClick.AddListener((UnityAction) (() =>
    {
      System.Action onTabPressed = this.OnTabPressed;
      if (onTabPressed == null)
        return;
      onTabPressed();
    }));
    this._animator.keepAnimatorControllerStateOnDisable = true;
  }

  protected virtual void SetActive()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Transition(1f, 0.0f));
  }

  protected virtual void SetInactive()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Transition(0.0f, 1f));
  }

  private IEnumerator Transition(float targetActiveWeight, float targetInactiveWeight)
  {
    int inactiveLayer = this._animator.GetLayerIndex("Inactive");
    int activeLayer = this._animator.GetLayerIndex("Active");
    float activeWeight = this._activeWeight;
    float inactiveWeight = this._inactiveWeight;
    float t = 0.0f;
    float time = 0.2f;
    while ((double) t < (double) time)
    {
      t += Time.unscaledDeltaTime;
      this._activeWeight = Mathf.Lerp(activeWeight, targetActiveWeight, t / time);
      this._inactiveWeight = Mathf.Lerp(inactiveWeight, targetInactiveWeight, t / time);
      this._animator.SetLayerWeight(inactiveLayer, this._inactiveWeight);
      this._animator.SetLayerWeight(activeLayer, this._activeWeight);
      yield return (object) null;
    }
  }
}
