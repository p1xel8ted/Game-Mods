// Decompiled with JetBrains decompiler
// Type: WeaponAttachmentData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "COTL/Weapon Attachment Data")]
public class WeaponAttachmentData : ScriptableObject
{
  public AttachmentEffect Effect;
  public AttachmentState State;
  public float ExplosionRadius;
  public float ExplosionDamage;
  public float ExplosionOffset;
  public float DashSpeed;
  public float DamageMultiplierIncrement;
  public int ExtraSlotsAmount;
  public float CriticalMultiplierIncrement;
  public float RangeIncrement;
  public float AttackRateIncrement;
  public float MovementSpeedIncrement;
  public float xpDropIncrement;
  [Range(0.0f, 1f)]
  public float healChanceIncrement;
  public float healAmount;
  [Range(0.0f, 100f)]
  public float negateDamageChanceIncrement;
  [Range(0.0f, 1f)]
  public float poisonChance = 0.3f;
  [Range(0.0f, 1f)]
  public float necromancyChance = 0.3f;
  [Range(0.0f, 100f)]
  public float scale = 1f;
  [Range(0.0f, 1f)]
  public float burnChance = 0.3f;
  [Range(0.0f, 1f)]
  public float fervourOnHitChance = 1f;
  public float electricRingSpreadSpeed = 1f;
  public float electricRingMaxRadius = 3f;

  public bool hasExplodeEffect => this.Effect == AttachmentEffect.Explode;

  public bool hasDashEffect => this.Effect == AttachmentEffect.Dash;

  public bool hasDamageEffect => this.Effect == AttachmentEffect.Damage;

  public bool hasExtraSlotsEffect => this.Effect == AttachmentEffect.ExtraSlots;

  public bool hasCriticalEffect => this.Effect == AttachmentEffect.Critical;

  public bool hasRangeEffect => this.Effect == AttachmentEffect.Range;

  public bool hasAttackRateEffect => this.Effect == AttachmentEffect.AttackRate;

  public bool hasMovementSpeedEffect => this.Effect == AttachmentEffect.MovementSpeed;

  public bool hasIncreaseXPEffect => this.Effect == AttachmentEffect.IncreasedXPDrop;

  public bool hasHealChanceEffect => this.Effect == AttachmentEffect.HealChance;

  public bool hasNegateDamageChanceEffect => this.Effect == AttachmentEffect.NegateDamageChance;

  public bool hasPoisonEffect => this.Effect == AttachmentEffect.Poison;

  public bool hasNecromanyEffect => this.Effect == AttachmentEffect.Necromancy;

  public bool hasScaleEffect => this.Effect == AttachmentEffect.Scale;

  public bool hasBurnEffect => this.Effect == AttachmentEffect.Burn;

  public bool hasFervourOnHit => this.Effect == AttachmentEffect.FervourOnHit;

  public bool hasElectricRing => this.Effect == AttachmentEffect.FervourOnHit;

  public bool IsAttachmentActive() => true;
}
