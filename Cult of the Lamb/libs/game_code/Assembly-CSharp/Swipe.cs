// Decompiled with JetBrains decompiler
// Type: Swipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Swipe : BaseMonoBehaviour
{
  public SpriteRenderer spriterenderer;
  public float alpha = 100f;
  public Color color;
  public float timer;
  public bool disabled;
  public Health Origin;
  public Health.Team team;
  public Action<Health, Health.AttackTypes, Health.AttackFlags, float> CallBack;
  public float Damage;
  public float CritChance;
  public float radius = 1f;
  public Health.AttackTypes AttackType;
  public Health.AttackFlags AttackFlags;
  public Collider2D damageCollider;
  public float Duration = 0.1f;
  public ParticleSystem[] particleSystems;
  public bool destroyAfterDuration = true;
  public float frameTime;
  public List<Health> objsHitThisFrame = new List<Health>();

  public void Init(
    Vector3 Position,
    float Angle,
    Health.Team team,
    Health Origin,
    Action<Health, Health.AttackTypes, Health.AttackFlags, float> CallBack,
    float Radius,
    float Damage = 1f,
    float CritChance = 0.0f,
    Health.AttackTypes AttackType = Health.AttackTypes.Melee,
    Health.AttackFlags AttackFlags = (Health.AttackFlags) 0)
  {
    this.transform.position = Position;
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Angle);
    if ((double) Angle > 90.0 || (double) Angle < -90.0)
      this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * -1f, this.transform.localScale.z);
    this.team = team;
    this.Origin = Origin;
    this.CallBack = CallBack;
    if ((UnityEngine.Object) this.damageCollider != (UnityEngine.Object) null)
    {
      if (((object) this.damageCollider).GetType() == typeof (CircleCollider2D))
        (this.damageCollider as CircleCollider2D).radius = Radius;
      else if (!(((object) this.damageCollider).GetType() == typeof (PolygonCollider2D)))
        ;
    }
    else
      Debug.LogWarning((object) "Damage collider for swipe is null, this should probably not be the case");
    this.Damage = Damage;
    this.CritChance = Mathf.Clamp01(CritChance);
    this.AttackType = AttackType;
    this.AttackFlags = AttackFlags;
    this.gameObject.SetActive(true);
    this.ReplayParticleSystems();
    if ((double) this.Duration == -1.0)
      return;
    this.Invoke("DestroySelf", this.Duration);
  }

  public void ReplayParticleSystems()
  {
    for (int index = 0; index < this.particleSystems.Length; ++index)
    {
      ParticleSystem particleSystem = this.particleSystems[index];
      if ((UnityEngine.Object) particleSystem != (UnityEngine.Object) null)
      {
        particleSystem.Clear();
        particleSystem.Play();
      }
    }
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (!this.gameObject.activeSelf)
      return;
    Health component1 = collider.gameObject.GetComponent<Health>();
    if (this.objsHitThisFrame.Count == 0)
      this.frameTime = GameManager.GetInstance().CurrentTime;
    if ((double) GameManager.GetInstance().CurrentTime - (double) this.frameTime < 0.10000000149011612)
    {
      if (this.objsHitThisFrame.Contains(component1))
        return;
      this.objsHitThisFrame.Add(component1);
    }
    else
      this.objsHitThisFrame.Clear();
    PlayerFarming component2 = (UnityEngine.Object) this.Origin != (UnityEngine.Object) null ? this.Origin.GetComponent<PlayerFarming>() : (PlayerFarming) null;
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !component1.enabled || !((UnityEngine.Object) component1 != (UnityEngine.Object) this.Origin) || !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) || this.team == component1.team && !((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    Health.AttackFlags attackFlags = this.AttackFlags;
    Vector3 AttackLocation = Vector3.Lerp(this.transform.position, component1.transform.position, 0.8f);
    float damage = this.Damage;
    if (component1.HasShield && component2.playerWeapon.DoingHeavyAttack)
      this.AttackType = Health.AttackTypes.Heavy;
    float num = 1f;
    if (PlayerFarming.Instance.playerWeapon.CriticalHitCharged || (double) UnityEngine.Random.Range(0.0f, 1f) < (double) this.CritChance)
    {
      if ((bool) (UnityEngine.Object) component2 && (bool) (UnityEngine.Object) component2.playerWeapon && (component2.playerWeapon.CriticalHitCharged || (double) UnityEngine.Random.Range(0.0f, 1f) < (double) this.CritChance))
      {
        damage *= 3f;
        attackFlags |= Health.AttackFlags.Crit;
        if (EquipmentManager.GetWeaponData(component2.currentWeapon).PrimaryEquipmentType == EquipmentType.Blunderbuss)
        {
          num *= 3f;
          PlayerWeapon.CriticalHitTimer = 0.0f;
        }
      }
      component1.DealDamage(damage, this.Origin.gameObject, AttackLocation, AttackType: this.AttackType, AttackFlags: attackFlags);
    }
    if ((double) this.Damage != 0.0 || component2.currentWeapon == EquipmentType.Sword_Ratau)
    {
      if (attackFlags.HasFlag((Enum) Health.AttackFlags.Crit))
        damage *= num;
      component1.DealDamage(damage, this.Origin.gameObject, AttackLocation, AttackType: this.AttackType, AttackFlags: attackFlags);
    }
    Action<Health, Health.AttackTypes, Health.AttackFlags, float> callBack = this.CallBack;
    if (callBack == null)
      return;
    callBack(component1, this.AttackType, attackFlags, num);
  }

  public void DestroySelf()
  {
    if (this.destroyAfterDuration)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject, this.Duration);
    else
      this.RemoveSwipe();
    this.gameObject.SetActive(false);
  }

  public void RemoveSwipe()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !PlayerFarming.Instance.playerWeapon.CriticalHitCharged)
      return;
    PlayerWeapon.CriticalHitTimer = 0.0f;
  }

  public void OnDestroy() => this.RemoveSwipe();

  public void OnDrawGizmos() => Utils.DrawCircleXY(this.transform.position, this.radius, Color.red);
}
