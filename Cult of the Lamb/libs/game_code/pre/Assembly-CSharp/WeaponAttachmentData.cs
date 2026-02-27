// Decompiled with JetBrains decompiler
// Type: WeaponAttachmentData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "COTL/Weapon Attachment Data")]
public class WeaponAttachmentData : ScriptableObject
{
  public AttachmentEffect Effect;
  public AttachmentState State;
  public string DescriptionKey;
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

  private bool hasExplodeEffect => this.Effect == AttachmentEffect.Explode;

  private bool hasDashEffect => this.Effect == AttachmentEffect.Dash;

  private bool hasDamageEffect => this.Effect == AttachmentEffect.Damage;

  private bool hasExtraSlotsEffect => this.Effect == AttachmentEffect.ExtraSlots;

  private bool hasCriticalEffect => this.Effect == AttachmentEffect.Critical;

  private bool hasRangeEffect => this.Effect == AttachmentEffect.Range;

  private bool hasAttackRateEffect => this.Effect == AttachmentEffect.AttackRate;

  private bool hasMovementSpeedEffect => this.Effect == AttachmentEffect.MovementSpeed;

  private bool hasIncreaseXPEffect => this.Effect == AttachmentEffect.IncreasedXPDrop;

  private bool hasHealChanceEffect => this.Effect == AttachmentEffect.HealChance;

  private bool hasNegateDamageChanceEffect => this.Effect == AttachmentEffect.NegateDamageChance;

  private bool hasPoisonEffect => this.Effect == AttachmentEffect.Poison;

  private bool hasNecromanyEffect => this.Effect == AttachmentEffect.Necromancy;

  public bool IsAttachmentActive() => true;
}
