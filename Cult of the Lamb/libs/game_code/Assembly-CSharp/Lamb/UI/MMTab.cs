// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public abstract class MMTab : MonoBehaviour
{
  public const string kInactiveLayer = "Inactive";
  public const string kActiveLayer = "Active";
  public System.Action OnTabPressed;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public UISubmenuBase _menu;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public Animator _animator;
  public float _inactiveWeight = 1f;
  public float _activeWeight;

  public MMButton Button => this._button;

  public UISubmenuBase Menu => this._menu;

  public RectTransform RectTransform => this._rectTransform;

  public abstract void Configure();

  public void Awake()
  {
    if ((UnityEngine.Object) this._menu != (UnityEngine.Object) null)
    {
      UISubmenuBase menu1 = this._menu;
      menu1.OnShow = menu1.OnShow + new System.Action(this.SetActive);
      UISubmenuBase menu2 = this._menu;
      menu2.OnHide = menu2.OnHide + new System.Action(this.SetInactive);
    }
    this._button.onClick.AddListener((UnityAction) (() =>
    {
      System.Action onTabPressed = this.OnTabPressed;
      if (onTabPressed == null)
        return;
      onTabPressed();
    }));
    this._animator.keepAnimatorStateOnDisable = true;
  }

  public virtual void SetActive()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Transition(1f, 0.0f));
  }

  public virtual void SetInactive()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Transition(0.0f, 1f));
  }

  public IEnumerator Transition(float targetActiveWeight, float targetInactiveWeight)
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

  [CompilerGenerated]
  public void \u003CAwake\u003Eb__16_0()
  {
    System.Action onTabPressed = this.OnTabPressed;
    if (onTabPressed == null)
      return;
    onTabPressed();
  }
}
