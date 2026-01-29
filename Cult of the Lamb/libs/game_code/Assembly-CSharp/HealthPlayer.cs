// Decompiled with JetBrains decompiler
// Type: HealthPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class HealthPlayer : Health
{
  public float healTimer;
  public bool ableToHeal;
  public float PLAYER_BLACK_HEARTS;
  public float PLAYER_BLUE_HEARTS;
  public float PLAYER_FIRE_HEARTS;
  public float PLAYER_ICE_HEARTS;
  public float PLAYER_SPIRIT_HEARTS;
  public float PLAYER_HEALTH = 6f;
  public float PLAYER_TOTAL_HEALTH = 6f;
  public float PLAYER_STARTING_HEALTH_CACHED;
  public float PLAYER_SPIRIT_TOTAL_HEARTS;
  public static bool ResetHealthDataP1 = true;
  public static bool ResetHealthDataP2 = true;

  public event HealthPlayer.HPUpdated OnHPUpdated;

  public event HealthPlayer.HPUpdated OnHeal;

  public event HealthPlayer.HPUpdated OnDamaged;

  public event HealthPlayer.HPUpdated OnPlayerDied;

  public event HealthPlayer.HPUpdated OnTotalHPUpdated;

  public float PoisonNormalisedTime => this.poisonTimer / this.poisonTickDuration;

  public float BurnNormalisedTime => this.burnTimer / this.burnTickDuration;

  public new bool IsPoisoned => (double) this.poisonTimer > 0.0;

  public override float poisonTickDuration => 0.75f;

  public override float playerPoisonDamage => 1f;

  public int PLAYER_STARTING_HEALTH
  {
    get
    {
      if (!CoopManager.CoopActive)
      {
        switch (DifficultyManager.PrimaryDifficulty)
        {
          case DifficultyManager.Difficulty.Easy:
            return 8;
          case DifficultyManager.Difficulty.Hard:
            return 4;
          case DifficultyManager.Difficulty.ExtraHard:
            return 2;
          default:
            return 6;
        }
      }
      else
      {
        switch (DifficultyManager.PrimaryDifficulty)
        {
          case DifficultyManager.Difficulty.Easy:
            return 6;
          case DifficultyManager.Difficulty.Hard:
            return 4;
          case DifficultyManager.Difficulty.ExtraHard:
            return 2;
          default:
            return 4;
        }
      }
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    this.OnDie += new Health.DieAction(this.HealthPlayer_OnDie);
    this.OnHit += new Health.HitAction(this.HealthPlayer_OnHit);
  }

  public override void InitHP()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      this.playerFarming = this.GetComponent<PlayerFarming>();
    bool flag = this.playerFarming.isLamb ? HealthPlayer.ResetHealthDataP1 : HealthPlayer.ResetHealthDataP2;
    if (flag || PlayerFarming.Location == FollowerLocation.Base)
    {
      HealthPlayer.ResetHealthDataP1 = !this.playerFarming.isLamb && HealthPlayer.ResetHealthDataP1;
      HealthPlayer.ResetHealthDataP2 = this.playerFarming.isLamb && HealthPlayer.ResetHealthDataP2;
      flag = false;
      this.PLAYER_TOTAL_HEALTH = (float) (this.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEALTH_MODIFIED + DataManager.Instance.PLAYER_HEARTS_LEVEL - this.playerFarming.RedHeartsTemporarilyRemoved) * PlayerFleeceManager.GetHealthMultiplier();
      this.PLAYER_STARTING_HEALTH_CACHED = (float) this.PLAYER_STARTING_HEALTH;
      this.PLAYER_HEALTH = this.PLAYER_TOTAL_HEALTH;
      this.HP = this.PLAYER_TOTAL_HEALTH;
      this.totalHP = this.PLAYER_TOTAL_HEALTH;
      if (this.playerFarming.isLamb)
      {
        this.BlueHearts = DataManager.Instance.PLAYER_BLUE_HEARTS;
        this.BlackHearts = DataManager.Instance.PLAYER_BLACK_HEARTS;
        this.FireHearts = DataManager.Instance.PLAYER_FIRE_HEARTS;
        this.IceHearts = DataManager.Instance.PLAYER_ICE_HEARTS;
        this.TotalSpiritHearts = DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS;
        this.BlueHearts = DataManager.Instance.PLAYER_BLUE_HEARTS;
        this.HP -= DataManager.Instance.PLAYER_REMOVED_HEARTS;
      }
      else
      {
        this.BlueHearts = DataManager.Instance.COOP_PLAYER_BLUE_HEARTS;
        this.BlackHearts = DataManager.Instance.COOP_PLAYER_BLACK_HEARTS;
        this.FireHearts = DataManager.Instance.COOP_PLAYER_FIRE_HEARTS;
        this.IceHearts = DataManager.Instance.COOP_PLAYER_ICE_HEARTS;
        this.TotalSpiritHearts = DataManager.Instance.COOP_PLAYER_SPIRIT_TOTAL_HEARTS;
        this.BlueHearts = DataManager.Instance.COOP_PLAYER_BLUE_HEARTS;
        this.HP -= DataManager.Instance.COOP_PLAYER_REMOVED_HEARTS;
      }
    }
    if (DataManager.Instance.PlayerFleece == 7)
    {
      this.totalHP = PlayerFleeceManager.OneHitKillHP;
      this.HP = PlayerFleeceManager.OneHitKillHP;
    }
    else
      this.totalHP = this.PLAYER_TOTAL_HEALTH;
    if (!flag && !GameManager.IsDungeon(PlayerFarming.Location))
      return;
    DataManager.Instance.PLAYER_BLUE_HEARTS = this.playerFarming.isLamb ? 0.0f : DataManager.Instance.PLAYER_BLUE_HEARTS;
    DataManager.Instance.PLAYER_BLACK_HEARTS = this.playerFarming.isLamb ? 0.0f : DataManager.Instance.PLAYER_BLACK_HEARTS;
    DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS = this.playerFarming.isLamb ? 0.0f : DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS;
    DataManager.Instance.PLAYER_FIRE_HEARTS = this.playerFarming.isLamb ? 0.0f : DataManager.Instance.PLAYER_FIRE_HEARTS;
    DataManager.Instance.PLAYER_ICE_HEARTS = this.playerFarming.isLamb ? 0.0f : DataManager.Instance.PLAYER_ICE_HEARTS;
    DataManager.Instance.PLAYER_REMOVED_HEARTS = this.playerFarming.isLamb ? 0.0f : DataManager.Instance.PLAYER_REMOVED_HEARTS;
    DataManager.Instance.COOP_PLAYER_BLUE_HEARTS = !this.playerFarming.isLamb ? 0.0f : DataManager.Instance.COOP_PLAYER_BLUE_HEARTS;
    DataManager.Instance.COOP_PLAYER_BLACK_HEARTS = !this.playerFarming.isLamb ? 0.0f : DataManager.Instance.COOP_PLAYER_BLACK_HEARTS;
    DataManager.Instance.COOP_PLAYER_SPIRIT_TOTAL_HEARTS = !this.playerFarming.isLamb ? 0.0f : DataManager.Instance.COOP_PLAYER_SPIRIT_TOTAL_HEARTS;
    DataManager.Instance.COOP_PLAYER_FIRE_HEARTS = !this.playerFarming.isLamb ? 0.0f : DataManager.Instance.COOP_PLAYER_FIRE_HEARTS;
    DataManager.Instance.COOP_PLAYER_ICE_HEARTS = !this.playerFarming.isLamb ? 0.0f : DataManager.Instance.COOP_PLAYER_ICE_HEARTS;
    DataManager.Instance.COOP_PLAYER_REMOVED_HEARTS = !this.playerFarming.isLamb ? 0.0f : DataManager.Instance.COOP_PLAYER_REMOVED_HEARTS;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.OnTrinketsChanged);
    this.OnDie -= new Health.DieAction(this.HealthPlayer_OnDie);
    this.OnHit -= new Health.HitAction(this.HealthPlayer_OnHit);
  }

  public void HealthPlayer_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    string str1 = "Custom";
    string str2 = "?";
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.generateRoom.Pieces != null && BiomeGenerator.Instance.CurrentRoom.generateRoom.Pieces.Count > 0 && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom.Pieces[0] != (UnityEngine.Object) null)
    {
      GenerateRoom generateRoom = BiomeGenerator.Instance.CurrentRoom.generateRoom;
      str1 = generateRoom.Pieces[0].name;
      str2 = generateRoom.Pieces[0].CurrentEncounter;
    }
    AnalyticsLogger.LogEvent(AnalyticsLogger.EventType.Deaths, $"{PlayerFarming.Location.ToString()} {DataManager.Instance.CurrentDLCDungeonID.ToString()}", str1, str2, Attacker.name);
    HealthPlayer.HPUpdated onPlayerDied = this.OnPlayerDied;
    if (onPlayerDied == null)
      return;
    onPlayerDied(this);
  }

  public void OnTrinketsChanged(TarotCards.Card trinket, PlayerFarming playerFarming = null)
  {
    this.TotalSpiritHearts = this.PLAYER_SPIRIT_TOTAL_HEARTS;
  }

  public override float HP
  {
    get => this.PLAYER_HEALTH;
    set
    {
      this.PLAYER_HEALTH = value;
      if ((double) this.PLAYER_HEALTH > (double) this.PLAYER_TOTAL_HEALTH)
        this.PLAYER_HEALTH = this.PLAYER_TOTAL_HEALTH;
      this._HP = this.PLAYER_HEALTH;
      if (this.OnHPUpdated == null)
        return;
      this.OnHPUpdated(this);
    }
  }

  public override float totalHP
  {
    get => this.PLAYER_TOTAL_HEALTH;
    set
    {
      this.PLAYER_TOTAL_HEALTH = value;
      if ((double) this.PLAYER_HEALTH > (double) this.PLAYER_TOTAL_HEALTH)
        this.PLAYER_HEALTH = this.PLAYER_TOTAL_HEALTH;
      this._totalHP = value;
      if (this.OnTotalHPUpdated == null)
        return;
      this.OnTotalHPUpdated(this);
    }
  }

  public override float BlueHearts
  {
    get => this.PLAYER_BLUE_HEARTS;
    set
    {
      if ((double) value > (double) this.PLAYER_BLUE_HEARTS)
      {
        this.PLAYER_BLUE_HEARTS = value;
        if (this.OnTotalHPUpdated == null)
          return;
        this.OnTotalHPUpdated(this);
      }
      else
      {
        this.PLAYER_BLUE_HEARTS = value;
        if (this.OnHPUpdated == null)
          return;
        this.OnHPUpdated(this);
      }
    }
  }

  public override float BlackHearts
  {
    get => this.PLAYER_BLACK_HEARTS;
    set
    {
      if ((double) value > (double) this.PLAYER_BLACK_HEARTS)
      {
        this.PLAYER_BLACK_HEARTS = value;
        if (this.OnTotalHPUpdated == null)
          return;
        this.OnTotalHPUpdated(this);
      }
      else
      {
        this.PLAYER_BLACK_HEARTS = value;
        if (this.OnHPUpdated == null)
          return;
        this.OnHPUpdated(this);
      }
    }
  }

  public override float FireHearts
  {
    get => this.PLAYER_FIRE_HEARTS;
    set
    {
      if ((double) value > (double) this.PLAYER_FIRE_HEARTS)
      {
        this.PLAYER_FIRE_HEARTS = value;
        if (this.OnTotalHPUpdated == null)
          return;
        this.OnTotalHPUpdated(this);
      }
      else
      {
        this.PLAYER_FIRE_HEARTS = value;
        if (this.OnHPUpdated == null)
          return;
        this.OnHPUpdated(this);
      }
    }
  }

  public override float IceHearts
  {
    get => this.PLAYER_ICE_HEARTS;
    set
    {
      if ((double) value > (double) this.PLAYER_ICE_HEARTS)
      {
        this.PLAYER_ICE_HEARTS = value;
        if (this.OnTotalHPUpdated == null)
          return;
        this.OnTotalHPUpdated(this);
      }
      else
      {
        this.PLAYER_ICE_HEARTS = value;
        if (this.OnHPUpdated == null)
          return;
        this.OnHPUpdated(this);
      }
    }
  }

  public override float TotalSpiritHearts
  {
    get => this._TotalSpiritHearts;
    set
    {
      float spiritTotalHearts = this.PLAYER_SPIRIT_TOTAL_HEARTS;
      this.PLAYER_SPIRIT_TOTAL_HEARTS = value;
      this._TotalSpiritHearts = value;
      HealthPlayer.HPUpdated onTotalHpUpdated = this.OnTotalHPUpdated;
      if (onTotalHpUpdated != null)
        onTotalHpUpdated(this);
      if ((double) this.TotalSpiritHearts == (double) spiritTotalHearts)
        return;
      if ((double) this.TotalSpiritHearts > (double) spiritTotalHearts)
      {
        this.SpiritHearts += this.TotalSpiritHearts - spiritTotalHearts;
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
    get => this.PLAYER_SPIRIT_HEARTS;
    set
    {
      float num = Mathf.Clamp(value, 0.0f, this.TotalSpiritHearts);
      if ((double) num == (double) this.PLAYER_SPIRIT_HEARTS)
        return;
      this.PLAYER_SPIRIT_HEARTS = num;
      HealthPlayer.HPUpdated onHpUpdated = this.OnHPUpdated;
      if (onHpUpdated == null)
        return;
      onHpUpdated(this);
    }
  }

  public bool IsNoHeartSlotsLeft
  {
    get
    {
      return (double) this.totalHP + (double) this.TotalSpiritHearts <= 0.0 && (double) this.BlueHearts <= 0.0 && (double) this.BlackHearts <= 0.0 && (double) this.FireHearts <= 0.0 && (double) this.IceHearts <= 0.0;
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
    if ((UnityEngine.Object) Attacker == (UnityEngine.Object) null || Attacker.CompareTag("Player") && (UnityEngine.Object) Attacker != (UnityEngine.Object) this.gameObject)
      return false;
    if (SettingsManager.Settings.Accessibility.UnlimitedHP)
      Damage = 0.0f;
    int num = base.DealDamage(Damage, Attacker, AttackLocation, BreakBlocking, AttackType, dealDamageImmediately, AttackFlags) ? 1 : 0;
    if (num == 0)
      return num != 0;
    if (CoopManager.CoopActive)
      CoopIndicatorIcon.AnimateDamageOnIcons(this.playerFarming);
    HealthPlayer.HPUpdated onDamaged = this.OnDamaged;
    if (onDamaged == null)
      return num != 0;
    onDamaged(this);
    return num != 0;
  }

  public override void Heal(float healing)
  {
    base.Heal(healing);
    HealthPlayer.HPUpdated onHeal = this.OnHeal;
    if (onHeal != null)
      onHeal(this);
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() =>
    {
      if (!this.playerFarming.IsKnockedOut)
        return;
      CoopManager.WakeKnockedOutPlayer(this.playerFarming, 0.0f, pauseTime: true);
    }));
  }

  public static int GainRandomHeart()
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = UnityEngine.Random.Range(0, 5);
    switch (num6)
    {
      case 0:
        num1 += 2;
        break;
      case 1:
        num2 += 2;
        break;
      case 2:
        num3 += 2;
        break;
      case 3:
        num4 += 2;
        break;
      case 4:
        num5 += 2;
        break;
    }
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming.players[index].health.BlackHearts += (float) num1;
      PlayerFarming.players[index].health.BlueHearts += (float) num2;
      PlayerFarming.players[index].health.TotalSpiritHearts += (float) num3;
      PlayerFarming.players[index].health.FireHearts += (float) num4;
      PlayerFarming.players[index].health.IceHearts += (float) num5;
    }
    return num6;
  }

  public static void LoseAllSpecialHearts()
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming.players[index].health.BlackHearts = 0.0f;
      PlayerFarming.players[index].health.FireHearts = 0.0f;
      PlayerFarming.players[index].health.IceHearts = 0.0f;
      PlayerFarming.players[index].health.BlueHearts = 0.0f;
      PlayerFarming.players[index].health.TotalSpiritHearts = 0.0f;
    }
  }

  public new void FullHeal()
  {
    this.HP = this.PLAYER_TOTAL_HEALTH + this.PLAYER_SPIRIT_HEARTS;
    HealthPlayer.HPUpdated onHeal = this.OnHeal;
    if (onHeal == null)
      return;
    onHeal(this);
  }

  public void HealthPlayer_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (AttackType != Health.AttackTypes.Burn)
      return;
    this.ClearBurn();
  }

  [CompilerGenerated]
  public void \u003CHeal\u003Eb__74_0()
  {
    if (!this.playerFarming.IsKnockedOut)
      return;
    CoopManager.WakeKnockedOutPlayer(this.playerFarming, 0.0f, pauseTime: true);
  }

  public delegate void HPUpdated(HealthPlayer Target);

  public delegate void TotalHPUpdated(HealthPlayer Target);
}
