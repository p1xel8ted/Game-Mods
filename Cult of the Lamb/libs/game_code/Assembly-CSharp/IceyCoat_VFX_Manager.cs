// Decompiled with JetBrains decompiler
// Type: IceyCoat_VFX_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class IceyCoat_VFX_Manager : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float revealValue;
  [SerializeField]
  public float revealDuration = 1f;
  [SerializeField]
  public float hideDuration = 0.1f;
  [SerializeField]
  public SpriteRenderer IceyCoat_VFX;
  [SerializeField]
  public SpriteRenderer IceyCoat_VFX_Secondary;
  [Space]
  public UnityEvent OnActivated;
  public UnityEvent OnDeacitvated;
  public UnityEvent OnShown;
  public UnityEvent OnHidden;

  public void Awake() => this.SetIceyCoatReveal(0.0f);

  public void OnDisable() => this.SetIceyCoatReveal(0.0f);

  public void SetActive(bool activate)
  {
    if (activate)
      this.Show();
    else
      this.Hide();
  }

  public void Show()
  {
    this.OnActivated?.Invoke();
    DOTween.To((DOGetter<float>) (() => this.revealValue), (DOSetter<float>) (x => this.revealValue = x), 1f, this.revealDuration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.SetIceyCoatReveal(this.revealValue))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.OnShown?.Invoke();
      this.SetIceyCoatReveal(this.revealValue);
    }));
  }

  public void Hide()
  {
    this.OnDeacitvated?.Invoke();
    DOTween.To((DOGetter<float>) (() => this.revealValue), (DOSetter<float>) (x => this.revealValue = x), 0.0f, this.hideDuration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.SetIceyCoatReveal(this.revealValue))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.SetIceyCoatReveal(this.revealValue);
      this.OnHidden?.Invoke();
    }));
  }

  public void SetIceyCoatReveal(float value)
  {
    if ((Object) this.IceyCoat_VFX != (Object) null && (Object) this.IceyCoat_VFX.material != (Object) null)
      this.IceyCoat_VFX.material.SetFloat("_IceyCoat_Reveal", value);
    else
      Debug.LogWarning((object) "IceyCoat_VFX is not assigned or its material is missing.");
    if ((Object) this.IceyCoat_VFX_Secondary != (Object) null && (Object) this.IceyCoat_VFX_Secondary.material != (Object) null)
      this.IceyCoat_VFX_Secondary.material.SetFloat("_IceyCoat_Reveal", value);
    else
      Debug.LogWarning((object) "IceyCoat_VFX_Secondary is not assigned or its material is missing.");
  }

  [CompilerGenerated]
  public float \u003CShow\u003Eb__12_0() => this.revealValue;

  [CompilerGenerated]
  public void \u003CShow\u003Eb__12_1(float x) => this.revealValue = x;

  [CompilerGenerated]
  public void \u003CShow\u003Eb__12_2() => this.SetIceyCoatReveal(this.revealValue);

  [CompilerGenerated]
  public void \u003CShow\u003Eb__12_3()
  {
    this.OnShown?.Invoke();
    this.SetIceyCoatReveal(this.revealValue);
  }

  [CompilerGenerated]
  public float \u003CHide\u003Eb__13_0() => this.revealValue;

  [CompilerGenerated]
  public void \u003CHide\u003Eb__13_1(float x) => this.revealValue = x;

  [CompilerGenerated]
  public void \u003CHide\u003Eb__13_2() => this.SetIceyCoatReveal(this.revealValue);

  [CompilerGenerated]
  public void \u003CHide\u003Eb__13_3()
  {
    this.SetIceyCoatReveal(this.revealValue);
    this.OnHidden?.Invoke();
  }
}
