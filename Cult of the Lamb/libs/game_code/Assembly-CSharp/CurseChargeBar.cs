// Decompiled with JetBrains decompiler
// Type: CurseChargeBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CurseChargeBar : MonoBehaviour
{
  public GameObject AimingRecticule;
  public SpriteRenderer ProjectileArrow;
  public SpriteRenderer ProjectileShadow;
  public SpriteRenderer ProjectileChargeTarget;
  public SpriteRenderer AreaOfDamage;
  public bool requiresCharging;
  public bool Hiding;

  public void ShowProjectileCharge(bool requiresCharging, float size = 1.5f)
  {
    if (this.gameObject.activeSelf || this.Hiding)
      return;
    this.gameObject.SetActive(true);
    this.requiresCharging = requiresCharging;
    this.ProjectileArrow.color = (Color) new Vector4(1f, 1f, 1f, 0.0f);
    this.ProjectileShadow.color = (Color) new Vector4(1f, 1f, 1f, 0.0f);
    this.ProjectileChargeTarget.color = (Color) new Vector4(1f, 1f, 1f, 0.0f);
    this.UpdateProjectileChargeBar(0.0f);
    this.SetAimingRecticuleScaleAndRotation(Vector3.zero, Vector3.zero);
    this.ProjectileArrow.DOKill();
    this.ProjectileShadow.DOKill();
    this.ProjectileChargeTarget.DOKill();
    this.ProjectileArrow.DOFade(1f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.ProjectileShadow.DOFade(1f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    if (requiresCharging)
      this.ProjectileChargeTarget.DOFade(1f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    else
      this.UpdateProjectileChargeBar(1f);
    this.ProjectileChargeTarget.gameObject.SetActive(true);
    this.ProjectileChargeTarget.size = new Vector2(0.34f, 2.2f);
    if (!((Object) this.AreaOfDamage != (Object) null))
      return;
    this.AreaOfDamage.DOKill();
    this.AreaOfDamage.color = (Color) new Vector4(1f, 1f, 1f, 0.0f);
    this.AreaOfDamage.DOFade(1f, 0.05f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.AreaOfDamage.transform.DOKill();
    this.AreaOfDamage.transform.localScale = Vector3.one * size * 2f;
    this.AreaOfDamage.transform.DOScale(Vector3.one * size, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.AreaOfDamage.transform.localEulerAngles = Vector3.zero;
    this.AreaOfDamage.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 180f), 0.5f).SetLoops<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(-1).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.Linear);
  }

  public void HideProjectileCharge()
  {
    if (!this.gameObject.activeSelf || this.Hiding)
      return;
    this.Hiding = true;
    this.ProjectileArrow.DOKill();
    this.ProjectileShadow.DOKill();
    this.ProjectileChargeTarget.DOKill();
    this.ProjectileChargeTarget.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.ProjectileArrow.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.ProjectileShadow.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      this.Hiding = false;
      this.gameObject.SetActive(false);
    }));
    if (!((Object) this.AreaOfDamage != (Object) null))
      return;
    this.AreaOfDamage.transform.DOKill();
    this.AreaOfDamage.DOKill();
    this.AreaOfDamage.transform.DOScale(Vector3.one * 3f, 0.1f);
    this.AreaOfDamage.DOFade(0.0f, 0.1f);
  }

  public void SetAimingRecticuleScaleAndRotation(Vector3 scale, Vector3 euler)
  {
    if (!this.gameObject.activeSelf)
      return;
    this.AimingRecticule.transform.localScale = scale;
    this.AimingRecticule.transform.eulerAngles = euler;
  }

  public void UpdateProjectileChargeBar(float fillAmount)
  {
    if (!this.gameObject.activeSelf)
      return;
    if (!this.requiresCharging)
      fillAmount = 1f;
    this.ProjectileArrow.material.SetFloat("_ColorRampOffset", Mathf.Lerp(0.5f, -0.5f, fillAmount));
    this.ProjectileChargeTarget.color = this.CorrectProjectileChargeRelease() ? new Color(0.0f, 1f, 0.0f, this.ProjectileChargeTarget.color.a) : new Color(1f, 1f, 1f, this.ProjectileChargeTarget.color.a);
  }

  public bool CorrectProjectileChargeRelease()
  {
    if (!this.gameObject.activeSelf || !this.requiresCharging)
      return false;
    float num = this.ProjectileArrow.material.GetFloat("_ColorRampOffset");
    return (double) num < -0.059999998658895493 && (double) num > -0.15999999642372131;
  }

  [CompilerGenerated]
  public void \u003CHideProjectileCharge\u003Eb__8_0()
  {
    this.Hiding = false;
    this.gameObject.SetActive(false);
  }
}
