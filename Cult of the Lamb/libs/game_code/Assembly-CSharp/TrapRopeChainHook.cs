// Decompiled with JetBrains decompiler
// Type: TrapRopeChainHook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class TrapRopeChainHook : UnitObject
{
  [SerializeField]
  public ChainHook chainHook;
  [SerializeField]
  public WeaponData weaponData;
  [SerializeField]
  public EquipmentType equipmentType;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public Health handHealth;
  [SerializeField]
  public Health hookHealth;
  [EventRef]
  public string AttackSpinningTrapLoopSFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_spinningtrap_spin_loop";
  [EventRef]
  public string GetHitSFX = "event:/dlc/dungeon06/trap/flesh_ball/gethit";
  public EventInstance chainLoopInstanceSFX;

  public void Start()
  {
    if ((UnityEngine.Object) this.hookHealth != (UnityEngine.Object) null)
      this.hookHealth.OnHit += new Health.HitAction(this.OnHookHit);
    if ((UnityEngine.Object) this.handHealth != (UnityEngine.Object) null)
      this.handHealth.OnDie += new Health.DieAction(this.OnDie);
    PlayerWeapon.WeaponCombos combo1 = this.weaponData.Combos[0];
    float hookRadius = combo1.RangeRadius;
    this.chainHook.SetVisuals(this.equipmentType);
    Vector3 right = this.transform.right;
    Vector3 facingVectorRight = new Vector3(-right.y, right.x) * Mathf.Sign(this.transform.localScale.x);
    float offsetMultiplier = combo1.OffsetMultiplier;
    this.spine.AnimationState.SetAnimation(0, "appear", false);
    this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    EllipseMovement ellipseMovement = new EllipseMovement(Vector3.up, Vector3.right, -facingVectorRight * offsetMultiplier, combo1.EllipseRadiusY, combo1.EllipseRadiusX, combo1.StartAngle, combo1.AngleToMove, combo1.Duration / 1f, combo1.RadiusMultiplierOverTime);
    this.chainHook.SetHideOnComplete(false);
    this.chainHook.Init(ellipseMovement, 1f, combo1.RangeRadius, (Health.AttackFlags) 0, false, combo1.ScaleMultiplierOverTime, false, onComplete: (System.Action<Vector3>) (hitPosition =>
    {
      PlayerWeapon.WeaponCombos combo2 = this.weaponData.Combos[1];
      hookRadius = combo2.RangeRadius;
      this.chainHook.SetVisuals(this.equipmentType);
      ellipseMovement = new EllipseMovement(Vector3.up, Vector3.right, -facingVectorRight * offsetMultiplier, combo2.EllipseRadiusY, combo2.EllipseRadiusX, combo2.StartAngle, combo2.AngleToMove, combo2.Duration / 1f, combo2.RadiusMultiplierOverTime);
      this.chainHook.Init(ellipseMovement, 1f, combo2.RangeRadius, (Health.AttackFlags) 0, false, combo2.ScaleMultiplierOverTime, false, true, ownerSpine: this.spine);
    }), ownerSpine: this.spine);
    if (string.IsNullOrEmpty(this.AttackSpinningTrapLoopSFX))
      return;
    this.chainLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.AttackSpinningTrapLoopSFX, this.gameObject, true);
  }

  public float GetHealth() => (UnityEngine.Object) this.handHealth != (UnityEngine.Object) null ? this.handHealth.HP : -1f;

  public void DestroyEarly()
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    this.handHealth.DealDamage(9999f, this.gameObject, this.transform.position);
  }

  public new void OnDestroy() => AudioManager.Instance.StopLoop(this.chainLoopInstanceSFX);

  public new void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!((UnityEngine.Object) this.hookHealth != (UnityEngine.Object) null))
      return;
    this.hookHealth.DealDamage(9999f, Attacker, this.hookHealth.transform.position);
  }

  public void OnHookHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.chainHook.InvertMovmenet();
    if (string.IsNullOrEmpty(this.GetHitSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.GetHitSFX, this.hookHealth.transform.position);
  }
}
