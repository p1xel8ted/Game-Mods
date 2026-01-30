// Decompiled with JetBrains decompiler
// Type: ShieldAmmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ShieldAmmo : PlayerAmmo
{
  public float LastLostAmmo;
  public HPBar hpBar;
  public float shieldAmmo = 3f;
  public int shieldAmmoTotal = 3;

  public new virtual void OnEnable() => this.Init();

  public void Init()
  {
    if (this.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(true);
    this.hpBar.gameObject.SetActive(true);
    this.hpBar.defence.SetActive(true);
    this.shieldAmmoTotal = 3;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Shield_Health))
      this.shieldAmmoTotal *= 2;
    this.shieldAmmo = (float) this.shieldAmmoTotal;
    this.AmmoChanged();
  }

  public float getShieldAmmo() => this.shieldAmmo;

  public bool UseAmmo()
  {
    this.hpBar.defence.SetActive(true);
    Debug.Log((object) $"Using shield ammo {this.shieldAmmo.ToString()}/{this.shieldAmmoTotal.ToString()}");
    if ((double) this.shieldAmmo <= 1.0)
    {
      this.AmmoChanged();
      this.shieldAmmo = 0.0f;
      return false;
    }
    if ((double) Time.time > (double) this.LastLostAmmo + 0.75)
    {
      this.SetShieldAmmo(this.shieldAmmo - 1f);
      this.hpBar.barInstant.transform.DOPunchScale(new Vector3(0.25f, 1f, 1f), 0.25f);
      this.LastLostAmmo = Time.time;
    }
    this.AmmoChanged();
    return true;
  }

  public bool AddAmmo()
  {
    if ((double) this.shieldAmmo >= (double) this.shieldAmmoTotal)
    {
      this.shieldAmmo = (float) this.shieldAmmoTotal;
      return false;
    }
    this.SetShieldAmmo(this.shieldAmmo + 1f);
    return true;
  }

  public void FillAllAmmo()
  {
    Debug.Log((object) ("Shield ammo pre-fill is " + this.shieldAmmo.ToString()));
    this.shieldAmmo = (float) this.shieldAmmoTotal;
    Debug.Log((object) ("Shield ammo fill to " + this.shieldAmmo.ToString()));
    this.AmmoChanged();
    this.LastLostAmmo = Time.time;
  }

  public void SetShieldAmmo(float value)
  {
    Debug.Log((object) ("Shield ammo pre-set is " + this.shieldAmmo.ToString()));
    this.shieldAmmo = Mathf.Clamp(value, 0.0f, (float) this.shieldAmmoTotal);
    Debug.Log((object) ("Shield ammo set to " + this.shieldAmmo.ToString()));
    this.AmmoChanged();
  }

  public override void CantAfford()
  {
    this.transform.DOShakePosition(0.25f, new Vector3(0.25f, 0.25f), randomness: 0.0f);
    this.AmmoChanged();
  }

  public new virtual void AmmoChanged()
  {
    this.gameObject.SetActive(true);
    Transform transform1 = this.hpBar.barInstant.transform;
    Transform transform2 = this.hpBar.barTween.transform;
    Vector3 vector3_1 = new Vector3(this.shieldAmmo / (float) this.shieldAmmoTotal, 1f);
    Vector3 vector3_2 = vector3_1;
    transform2.localScale = vector3_2;
    Vector3 vector3_3 = vector3_1;
    transform1.localScale = vector3_3;
  }

  public void ShowShieldHPBar()
  {
    this.hpBar.gameObject.SetActive(true);
    this.hpBar.DOKill();
    this.hpBar.transform.DOScale(1f, 0.33f);
    this.hpBar.defence.SetActive(true);
  }

  public void HideShieldHPBar()
  {
    this.hpBar.DOKill();
    this.hpBar.transform.DOScale(0.0f, 0.33f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.hpBar.gameObject.SetActive(false);
      this.hpBar.defence.SetActive(true);
      this.gameObject.SetActive(false);
    }));
  }

  [CompilerGenerated]
  public void \u003CHideShieldHPBar\u003Eb__14_0()
  {
    this.hpBar.gameObject.SetActive(false);
    this.hpBar.defence.SetActive(true);
    this.gameObject.SetActive(false);
  }
}
