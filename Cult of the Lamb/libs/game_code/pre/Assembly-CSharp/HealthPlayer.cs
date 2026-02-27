// Decompiled with JetBrains decompiler
// Type: HealthPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HealthPlayer : Health
{
  public static bool ResetHealthData = true;

  public static event HealthPlayer.HPUpdated OnHPUpdated;

  public static event HealthPlayer.HPUpdated OnHeal;

  public static event HealthPlayer.HPUpdated OnDamaged;

  public static event HealthPlayer.HPUpdated OnPlayerDied;

  public static event HealthPlayer.HPUpdated OnTotalHPUpdated;

  public float PoisonNormalisedTime => this.poisonTimer / this.poisonTickDuration;

  public new bool IsPoisoned => (double) this.poisonTimer > 0.0;

  public override float poisonTickDuration => 2f;

  protected override float playerPoisonDamage => 1f;

  public override void OnEnable()
  {
    base.OnEnable();
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    this.OnDie += new Health.DieAction(this.HealthPlayer_OnDie);
  }

  public override void InitHP()
  {
    if (HealthPlayer.ResetHealthData || PlayerFarming.Location == FollowerLocation.Base)
      DataManager.Instance.PLAYER_TOTAL_HEALTH = (float) (DataManager.Instance.PLAYER_STARTING_HEALTH_CACHED + DataManager.Instance.PLAYER_HEALTH_MODIFIED + DataManager.Instance.PLAYER_HEARTS_LEVEL - DataManager.Instance.RedHeartsTemporarilyRemoved);
    HealthPlayer.ResetHealthData = false;
    if (PlayerFarming.Location == FollowerLocation.Base)
      DataManager.Instance.PLAYER_HEALTH = DataManager.Instance.PLAYER_TOTAL_HEALTH;
    this.HP = DataManager.Instance.PLAYER_HEALTH;
    this.totalHP = DataManager.Instance.PLAYER_TOTAL_HEALTH;
    this.BlueHearts = DataManager.Instance.PLAYER_BLUE_HEARTS;
    this.TotalSpiritHearts = DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS;
    this.SpiritHearts = DataManager.Instance.PLAYER_SPIRIT_HEARTS;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    this.OnDie -= new Health.DieAction(this.HealthPlayer_OnDie);
  }

  private void HealthPlayer_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    HealthPlayer.HPUpdated onPlayerDied = HealthPlayer.OnPlayerDied;
    if (onPlayerDied == null)
      return;
    onPlayerDied(this);
  }

  private void OnTrinketsChanged(TarotCards.Card trinket)
  {
    this.TotalSpiritHearts = DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS;
  }

  public override float HP
  {
    get => DataManager.Instance.PLAYER_HEALTH;
    set
    {
      Debug.Log((object) $"HP CHANGE: {(object) DataManager.Instance.PLAYER_HEALTH}  {(object) value}");
      DataManager.Instance.PLAYER_HEALTH = value;
      if ((double) DataManager.Instance.PLAYER_HEALTH > (double) DataManager.Instance.PLAYER_TOTAL_HEALTH)
        DataManager.Instance.PLAYER_HEALTH = DataManager.Instance.PLAYER_TOTAL_HEALTH;
      if (HealthPlayer.OnHPUpdated == null)
        return;
      HealthPlayer.OnHPUpdated(this);
    }
  }

  public override float totalHP
  {
    get => DataManager.Instance.PLAYER_TOTAL_HEALTH;
    set
    {
      Debug.Log((object) $"PLAYER_TOTAL_HEALTH CHANGE: {(object) DataManager.Instance.PLAYER_TOTAL_HEALTH}  {(object) value}");
      DataManager.Instance.PLAYER_TOTAL_HEALTH = value;
      if (HealthPlayer.OnTotalHPUpdated == null)
        return;
      HealthPlayer.OnTotalHPUpdated(this);
    }
  }

  public override float BlueHearts
  {
    get => DataManager.Instance.PLAYER_BLUE_HEARTS;
    set
    {
      if ((double) value > (double) DataManager.Instance.PLAYER_BLUE_HEARTS)
      {
        DataManager.Instance.PLAYER_BLUE_HEARTS = value;
        if (HealthPlayer.OnTotalHPUpdated == null)
          return;
        HealthPlayer.OnTotalHPUpdated(this);
      }
      else
      {
        DataManager.Instance.PLAYER_BLUE_HEARTS = value;
        if (HealthPlayer.OnHPUpdated == null)
          return;
        HealthPlayer.OnHPUpdated(this);
      }
    }
  }

  public override float BlackHearts
  {
    get => DataManager.Instance.PLAYER_BLACK_HEARTS;
    set
    {
      if ((double) value > (double) DataManager.Instance.PLAYER_BLACK_HEARTS)
      {
        DataManager.Instance.PLAYER_BLACK_HEARTS = value;
        if (HealthPlayer.OnTotalHPUpdated == null)
          return;
        HealthPlayer.OnTotalHPUpdated(this);
      }
      else
      {
        DataManager.Instance.PLAYER_BLACK_HEARTS = value;
        if (HealthPlayer.OnHPUpdated == null)
          return;
        HealthPlayer.OnHPUpdated(this);
      }
    }
  }

  public override float TotalSpiritHearts
  {
    get => this._TotalSpiritHearts;
    set
    {
      Debug.Log((object) $" PLAYER_SPIRIT_TOTAL_HEARTS: {(object) DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS}  {(object) value}");
      float totalSpiritHearts = this.TotalSpiritHearts;
      DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS = value;
      this._TotalSpiritHearts = value;
      HealthPlayer.HPUpdated onTotalHpUpdated = HealthPlayer.OnTotalHPUpdated;
      if (onTotalHpUpdated != null)
        onTotalHpUpdated(this);
      if ((double) this.TotalSpiritHearts == (double) totalSpiritHearts)
        return;
      if ((double) this.TotalSpiritHearts > (double) totalSpiritHearts)
      {
        this.SpiritHearts += this.TotalSpiritHearts - totalSpiritHearts;
      }
      else
      {
        if ((double) this.SpiritHearts <= (double) this.TotalSpiritHearts)
          return;
        this.SpiritHearts = this.TotalSpiritHearts;
      }
    }
  }

  public override float SpiritHearts
  {
    get => DataManager.Instance.PLAYER_SPIRIT_HEARTS;
    set
    {
      Debug.Log((object) $"PLAYER_SPIRIT_HEARTS : {(object) DataManager.Instance.PLAYER_SPIRIT_HEARTS}  {(object) value}");
      float num = Mathf.Clamp(value, 0.0f, this.TotalSpiritHearts);
      if ((double) num == (double) DataManager.Instance.PLAYER_SPIRIT_HEARTS)
        return;
      DataManager.Instance.PLAYER_SPIRIT_HEARTS = num;
      HealthPlayer.HPUpdated onHpUpdated = HealthPlayer.OnHPUpdated;
      if (onHpUpdated == null)
        return;
      onHpUpdated(this);
    }
  }

  public override bool DealDamage(
    float Damage,
    GameObject Attacker,
    Vector3 AttackLocation,
    bool BreakBlocking = false,
    Health.AttackTypes AttackType = Health.AttackTypes.Melee,
    bool dealDamageImmediately = false,
    Health.AttackFlags AttackFlags = (Health.AttackFlags) 0)
  {
    int num = base.DealDamage(Damage, Attacker, AttackLocation, BreakBlocking, AttackType, dealDamageImmediately, AttackFlags) ? 1 : 0;
    if (num == 0)
      return num != 0;
    HealthPlayer.HPUpdated onDamaged = HealthPlayer.OnDamaged;
    if (onDamaged == null)
      return num != 0;
    onDamaged(this);
    return num != 0;
  }

  public override void Heal(float healing)
  {
    base.Heal(healing);
    HealthPlayer.HPUpdated onHeal = HealthPlayer.OnHeal;
    if (onHeal == null)
      return;
    onHeal(this);
  }

  public static int GainRandomHeart()
  {
    Health health = PlayerFarming.Instance.health;
    int num = Random.Range(0, 3);
    switch (num)
    {
      case 0:
        health.BlackHearts += 2f;
        break;
      case 1:
        health.BlueHearts += 2f;
        break;
      case 2:
        health.TotalSpiritHearts += 2f;
        break;
    }
    return num;
  }

  public static void LoseAllSpecialHearts()
  {
    Health health = PlayerFarming.Instance.health;
    health.BlueHearts = 0.0f;
    health.BlackHearts = 0.0f;
    health.TotalSpiritHearts = 0.0f;
  }

  public delegate void HPUpdated(HealthPlayer Target);

  public delegate void TotalHPUpdated(HealthPlayer Target);
}
