// Decompiled with JetBrains decompiler
// Type: Swipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Swipe : BaseMonoBehaviour
{
  private SpriteRenderer spriterenderer;
  private float alpha = 100f;
  private Color color;
  private float timer;
  private bool disabled;
  public Health Origin;
  public Health.Team team;
  public Action<Health, Health.AttackTypes> CallBack;
  private float Damage;
  private float CritChance;
  public float radius = 1f;
  private Health.AttackTypes AttackType;
  private Health.AttackFlags AttackFlags;
  public Collider2D damageCollider;
  public float Duration = 0.1f;
  private float frameTime;
  private List<Health> objsHitThisFrame = new List<Health>();

  public void Init(
    Vector3 Position,
    float Angle,
    Health.Team team,
    Health Origin,
    Action<Health, Health.AttackTypes> CallBack,
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
    CircleCollider2D damageCollider = this.damageCollider as CircleCollider2D;
    if ((UnityEngine.Object) damageCollider != (UnityEngine.Object) null)
      damageCollider.radius = Radius;
    this.Damage = Damage;
    this.CritChance = Mathf.Clamp01(CritChance);
    this.AttackType = AttackType;
    this.AttackFlags = AttackFlags;
    this.Invoke("DestroySelf", this.Duration);
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    Health component = collider.gameObject.GetComponent<Health>();
    if (this.objsHitThisFrame.Count == 0)
      this.frameTime = GameManager.GetInstance().CurrentTime;
    if ((double) GameManager.GetInstance().CurrentTime - (double) this.frameTime < 0.10000000149011612)
    {
      if (this.objsHitThisFrame.Contains(component))
        return;
      this.objsHitThisFrame.Add(component);
    }
    else
      this.objsHitThisFrame.Clear();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.enabled || !((UnityEngine.Object) component != (UnityEngine.Object) this.Origin) || !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) || this.team == component.team && !((UnityEngine.Object) this.Origin == (UnityEngine.Object) PlayerFarming.Instance.health))
      return;
    Health.AttackFlags attackFlags = this.AttackFlags;
    float damage = this.Damage;
    if (PlayerFarming.Instance.playerWeapon.CriticalHitCharged || (double) UnityEngine.Random.Range(0.0f, 1f) < (double) this.CritChance)
    {
      damage *= 3f;
      attackFlags |= Health.AttackFlags.Crit;
    }
    component.DealDamage(damage, this.Origin.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.8f), AttackType: this.AttackType, AttackFlags: attackFlags);
    Action<Health, Health.AttackTypes> callBack = this.CallBack;
    if (callBack == null)
      return;
    callBack(component, this.AttackType);
  }

  private void DestroySelf() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void OnDestroy()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !PlayerFarming.Instance.playerWeapon.CriticalHitCharged)
      return;
    PlayerWeapon.CriticalHitTimer = 0.0f;
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.radius, Color.red);
  }
}
