// Decompiled with JetBrains decompiler
// Type: DoubleMinibossController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.DeathScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class DoubleMinibossController : MiniBossController
{
  [TermsPopup("")]
  public string bossName1;
  [TermsPopup("")]
  public string bossName2;
  public new EnemyModifier modifier;
  public new bool shown;
  public new bool bossDead;
  public string lastMinibossName = "";
  public int minibossDeathCount;
  public new bool PlayerHit;
  public new bool WaitingForAnimationToComplete;

  public new GameObject cameraTarget
  {
    get
    {
      return (Object) this.EnemiesToTrack[0] == (Object) null ? this.EnemiesToTrack[1].gameObject : this.EnemiesToTrack[0].gameObject;
    }
  }

  public new string Name
  {
    get
    {
      string translation = LocalizationManager.Sources[0].GetTranslation(this.DisplayName);
      if (this.isPostGameBoss)
        translation += " II";
      return translation;
    }
  }

  public string BossName1
  {
    get
    {
      string translation = LocalizationManager.Sources[0].GetTranslation(this.bossName1);
      if (this.isPostGameBoss)
        translation += " II";
      return translation;
    }
  }

  public string BossName2
  {
    get
    {
      string translation = LocalizationManager.Sources[0].GetTranslation(this.bossName2);
      if (this.isPostGameBoss)
        translation += " II";
      return translation;
    }
  }

  public override void Awake()
  {
    MiniBossController.MiniBossData miniBossData = DataManager.Instance.GetMiniBossData(this.BossIntro.GetComponent<UnitObject>().EnemyType);
    if (DataManager.Instance != null && miniBossData != null && this.cycleModifiers && !DungeonSandboxManager.Active)
    {
      EnemyModifier modifier = EnemyModifier.GetModifierExcluding(miniBossData.EncounteredModifiers);
      if ((Object) modifier == (Object) null)
      {
        miniBossData.EncounteredModifiers.Clear();
        modifier = EnemyModifier.GetModifier(1f);
      }
      this.modifier = modifier;
      this.EnemiesToTrack[0].GetComponent<UnitObject>().ForceSetModifier(modifier);
      this.EnemiesToTrack[1].GetComponent<UnitObject>().ForceSetModifier(modifier);
      miniBossData.EncounteredModifiers.Add(modifier.Modifier);
    }
    this.EnemiesToTrack[0].GetComponent<Health>().OnDie += new Health.DieAction(this.MiniBossController_OnDie);
    this.EnemiesToTrack[1].GetComponent<Health>().OnDie += new Health.DieAction(this.MiniBossController_OnDie);
    foreach (Health health in this.EnemiesToTrack)
    {
      health.totalHP *= DataManager.Instance.BossHealthMultiplier;
      health.HP *= DataManager.Instance.BossHealthMultiplier;
    }
    foreach (BaseMonoBehaviour baseMonoBehaviour in this.ComponentsToToggleEnabled)
    {
      if ((Object) baseMonoBehaviour != (Object) null)
        baseMonoBehaviour.enabled = false;
    }
  }

  public new void MiniBossController_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (++this.minibossDeathCount < 2)
      return;
    this.lastMinibossName = (Object) Victim == (Object) this.EnemiesToTrack[0] ? this.bossName1 : this.bossName2;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    DataManager.Instance.TakenBossDamage = this.PlayerHit;
    Debug.Log((object) ("Taken Boss Damage = " + this.PlayerHit.ToString()));
    GameManager.GetInstance().RemoveFromCamera(this.cameraTarget);
    DoubleUIBossHUD.Hide();
    this.bossDead = true;
    DataManager.Instance.BeatenFirstMiniBoss = true;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    if (DataManager.Instance.GetMiniBossData(Victim.GetComponent<UnitObject>().EnemyType) == null)
      DataManager.Instance.MiniBossData.Add(new MiniBossController.MiniBossData()
      {
        EnemyType = Victim.GetComponent<UnitObject>().EnemyType,
        EncounteredModifiers = new List<EnemyModifier.ModifierType>()
      });
    if (this.isBigBoss)
    {
      if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location) || GameManager.Layer2 && !this.BeatenPostGameLeader())
        DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.BeatenBoss;
    }
    else
      DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.BeatenMiniBoss;
    SimulationManager.UnPause();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.KillEnemiesDelay());
    int _cultLeader;
    switch (SceneManager.GetActiveScene().name)
    {
      case "Dungeon Boss 1":
        _cultLeader = 1;
        break;
      case "Dungeon Boss 2":
        _cultLeader = 2;
        break;
      case "Dungeon Boss 3":
        _cultLeader = 3;
        break;
      case "Dungeon Boss 4":
        _cultLeader = 4;
        break;
      default:
        _cultLeader = -1;
        break;
    }
    if (_cultLeader == -1 || !(bool) (Object) MonoSingleton<PlayerProgress_Analytics>.Instance)
      return;
    MonoSingleton<PlayerProgress_Analytics>.Instance.CultLeaderComplete(_cultLeader);
  }

  public new bool BeatenPostGameLeader()
  {
    return PlayerFarming.Location == FollowerLocation.Dungeon1_1 && DataManager.Instance.BeatenLeshyLayer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_2 && DataManager.Instance.BeatenHeketLayer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_3 && DataManager.Instance.BeatenKallamarLayer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_4 && DataManager.Instance.BeatenShamuraLayer2;
  }

  public new IEnumerator KillEnemiesDelay()
  {
    DoubleMinibossController minibossController = this;
    yield return (object) new WaitForSeconds(1f);
    Health component = (Object) minibossController.EnemiesToTrack[0] != (Object) null ? minibossController.EnemiesToTrack[0].GetComponent<Health>() : (Health) null;
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((Object) Health.team2[index] != (Object) null && (Object) Health.team2[index] != (Object) component && Health.team2[index].gameObject.activeInHierarchy)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, minibossController.gameObject, minibossController.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
  }

  public override void OnEnable()
  {
    if (this.shown && !this.bossDead)
    {
      DoubleUIBossHUD.Play(this.EnemiesToTrack[0], this.BossName1, this.EnemiesToTrack[1], this.BossName2);
      DoubleUIBossHUD.Instance.ForceHealthAmount1(this.EnemiesToTrack[0].HP / this.EnemiesToTrack[0].totalHP);
      DoubleUIBossHUD.Instance.ForceHealthAmount2(this.EnemiesToTrack[1].HP / this.EnemiesToTrack[1].totalHP);
      this.EnemiesToTrack[0].SlowMoOnkill = true;
      this.EnemiesToTrack[1].SlowMoOnkill = true;
      this.SetMusic();
    }
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public new void OnDisable()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public new void OnDestroy()
  {
    MiniBossController.Instance = (MiniBossController) null;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
  }

  public new void SetMusic()
  {
    switch (this.BossIntro.GetComponent<UnitObject>().EnemyType)
    {
      case Enemy.WormBoss:
        if (GameManager.Layer2)
        {
          AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossC);
          break;
        }
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossB);
        break;
      case Enemy.Beholder1:
      case Enemy.Beholder2:
      case Enemy.Beholder3:
      case Enemy.Beholder4:
      case Enemy.Beholder5:
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BeholderBattle);
        break;
      case Enemy.FrogBoss:
        if (GameManager.Layer2)
        {
          AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossC);
          break;
        }
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossB);
        break;
      case Enemy.JellyBoss:
        if (GameManager.Layer2)
        {
          AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossC);
          break;
        }
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossB);
        break;
      case Enemy.SpiderBoss:
        if (GameManager.Layer2)
        {
          AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossC);
          break;
        }
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossB);
        break;
      default:
        if (!this.isBigBoss)
        {
          AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossA);
          break;
        }
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossB);
        break;
    }
  }

  public new void TrackPlayerHealth()
  {
    this.PlayerHit = false;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.AddListener(new UnityAction(this.OnPlayerHit));
  }

  public new void OnPlayerHit() => this.PlayerHit = true;

  public new void Play(bool skipped = false)
  {
    this.StartCoroutine((IEnumerator) this.IntroRoutine(skipped));
  }

  public override IEnumerator IntroRoutine(bool skipped = false)
  {
    DoubleMinibossController minibossController = this;
    SimulationManager.Pause();
    MiniBossController.Instance = (MiniBossController) minibossController;
    minibossController.TrackPlayerHealth();
    if (minibossController.ShowName)
    {
      if (minibossController.BossIntro.GetComponent<UnitObject>().EnemyType == Enemy.FrogBoss)
        HUD_DisplayName.Play(minibossController.Name, 2, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.FrogBoss);
      else
        HUD_DisplayName.Play(minibossController.Name, 2, HUD_DisplayName.Positions.Centre);
    }
    foreach (Component component1 in minibossController.EnemiesToTrack)
    {
      ShowHPBar component2 = component1.GetComponent<ShowHPBar>();
      if ((bool) (Object) component2)
        component2.enabled = false;
    }
    yield return (object) minibossController.StartCoroutine((IEnumerator) minibossController.BossIntro.PlayRoutine(skipped));
    minibossController.SetMusic();
    foreach (Behaviour behaviour in minibossController.ComponentsToToggleEnabled)
      behaviour.enabled = true;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    DoubleUIBossHUD.Play(minibossController.EnemiesToTrack[0], minibossController.BossName1, minibossController.EnemiesToTrack[1], minibossController.BossName2);
    GameManager.GetInstance().AddToCamera(minibossController.cameraTarget);
    minibossController.shown = true;
    RoomLockController.CloseAll();
  }

  public new IEnumerator OutroRoutine()
  {
    yield return (object) null;
    foreach (Health health in Health.team2)
    {
      if ((bool) (Object) health)
        health.DestroyNextFrame();
    }
  }

  public new void OnLanguageChanged()
  {
    DoubleUIBossHUD.Instance?.UpdateName(this.BossName1, this.BossName2);
  }

  public class DoubleMiniBossData
  {
    public Enemy EnemyType;
    public List<EnemyModifier.ModifierType> EncounteredModifiers;
  }
}
