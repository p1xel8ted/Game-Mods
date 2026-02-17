// Decompiled with JetBrains decompiler
// Type: ShieldRepel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ShieldRepel : MonoBehaviour
{
  public GameObject ShieldTargetHeavy;
  public ShieldAmmo shieldAmmo;
  public GameObject shieldDestroyedEffect;
  public GameObject shieldDamagedEffect;
  public PlayerFarming playerFarming;
  public float facingAngle;
  public float hideAimTimer;
  public float lastKnockback;
  [CompilerGenerated]
  public Action<Health, Health.AttackTypes> \u003ChitEnemyCallback\u003Ek__BackingField;

  public void Start()
  {
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (!this.gameObject.activeSelf)
      return;
    UnitObject component = collider.gameObject.GetComponent<UnitObject>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.repelUnit(component);
  }

  public void SetAngle(float targetAngle, float heavyDist = -1f)
  {
    this.gameObject.SetActive(true);
    this.hideAimTimer = Time.fixedTime + 0.25f;
    this.facingAngle = targetAngle;
    this.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.facingAngle);
    if ((double) heavyDist != -1.0)
    {
      this.ShieldTargetHeavy.SetActive(true);
      this.ShieldTargetHeavy.transform.SetLocalPositionAndRotation(new Vector3(0.4f * heavyDist * 5.5f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, -this.facingAngle));
    }
    else
      this.ShieldTargetHeavy.SetActive(false);
  }

  public void FixedUpdate()
  {
    if ((double) Time.fixedTime <= (double) this.hideAimTimer)
      return;
    this.gameObject.SetActive(false);
  }

  public Action<Health, Health.AttackTypes> hitEnemyCallback
  {
    get => this.\u003ChitEnemyCallback\u003Ek__BackingField;
    set => this.\u003ChitEnemyCallback\u003Ek__BackingField = value;
  }

  public void repelUnit(UnitObject unit)
  {
    if (!((UnityEngine.Object) unit != (UnityEngine.Object) null) || unit.health.team != Health.Team.Team2)
      return;
    float num = this.lastKnockback + 0.5f;
    if ((double) Time.fixedTime > (double) num)
    {
      this.playerFarming.unitObject.DoKnockBack((float) (((double) this.facingAngle + 160.0 + (double) UnityEngine.Random.value * 40.0) % 360.0 * (Math.PI / 180.0)), 0.15f, 0.15f);
      this.lastKnockback = Time.fixedTime;
    }
    AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
    if ((double) Time.fixedTime > (double) num && this.shieldAmmo.UseAmmo())
    {
      float angle = Utils.GetAngle(this.transform.position, unit.transform.position);
      unit.DoKnockBack(angle, 1.5f, 0.33f);
      CameraManager.shakeCamera(2f, Utils.GetAngle(this.transform.position, unit.transform.position));
    }
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HA_Blunderbuss))
      return;
    int currentWeaponLevel = this.playerFarming.currentWeaponLevel;
    unit.health.DealDamage((float) currentWeaponLevel, this.gameObject, Vector3.Lerp(this.playerFarming.transform.position, unit.transform.position, 0.8f));
  }

  public void DamageShieldEffect()
  {
    Debug.Log((object) ("Damaging SHIELD " + ((object) this.shieldDestroyedEffect)?.ToString()));
    AudioManager.Instance.PlayOneShot("event:/weapon/metal_medium", this.gameObject);
    CameraManager.shakeCamera(2f);
    if (!((UnityEngine.Object) this.shieldDamagedEffect != (UnityEngine.Object) null))
      return;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.shieldDamagedEffect);
    gameObject.name = "SHIELD DESTROY EFFECT";
    gameObject.SetActive(true);
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.position = this.transform.position;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 3f);
  }

  public void DestroyShieldEffect()
  {
    AudioManager.Instance.PlayOneShot("event:/weapon/metal_heavy", this.gameObject);
    CameraManager.shakeCamera(2f);
    Debug.Log((object) ("DESTROYING SHIELD " + ((object) this.shieldDestroyedEffect)?.ToString()));
    if (!((UnityEngine.Object) this.shieldDestroyedEffect != (UnityEngine.Object) null))
      return;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.shieldDestroyedEffect);
    gameObject.name = "SHIELD DESTROY EFFECT";
    gameObject.SetActive(true);
    gameObject.transform.localScale = Vector3.one * 5f;
    gameObject.transform.position = this.transform.position;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 3f);
  }
}
