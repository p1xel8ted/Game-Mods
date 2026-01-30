// Decompiled with JetBrains decompiler
// Type: MiniBossController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class MiniBossController : BaseMonoBehaviour
{
  public static MiniBossController Instance;
  public BossIntro BossIntro;
  [TermsPopup("")]
  public string DisplayName;
  public bool ShowName = true;
  public List<Health> EnemiesToTrack = new List<Health>();
  public List<BaseMonoBehaviour> ComponentsToToggleEnabled = new List<BaseMonoBehaviour>();
  public bool multipleBoss;
  public int bossesNumber = 1;
  public bool forceMinibossIntoCorrectPosition = true;
  [Tooltip("If set to true, MiniBossManager will call the OnDieCustom() method from this class when a unit in EnemiesToTrack dies INSTEAD of MiniBossManager.H_OnDie().")]
  public bool hasCustomPostDeathLogic;
  [Space]
  [SerializeField]
  public bool cycleModifiers = true;
  public EnemyModifier modifier;
  public bool shown;
  public bool bossDead;
  [SerializeField]
  public bool isBigBoss;
  [SerializeField]
  public bool isPostGameBoss;
  public bool PlayerHit;
  public bool WaitingForAnimationToComplete;

  public GameObject cameraTarget => this.EnemiesToTrack[0].gameObject;

  public string Name
  {
    get
    {
      string translation = LocalizationManager.Sources[0].GetTranslation(this.DisplayName);
      if (this.isPostGameBoss)
        translation += " II";
      return translation;
    }
  }

  public virtual void Awake()
  {
    UnitObject unitObject = this.BossIntro.GetComponent<UnitObject>();
    if ((Object) unitObject == (Object) null)
      unitObject = this.BossIntro.GetComponentInChildren<UnitObject>();
    MiniBossController.MiniBossData miniBossData = DataManager.Instance.GetMiniBossData(unitObject.EnemyType);
    if (DataManager.Instance != null && miniBossData != null && this.cycleModifiers && !DungeonSandboxManager.Active)
    {
      bool flag = true;
      if ((PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6) && !DataManager.Instance.BeatenYngya)
        flag = false;
      if (flag)
      {
        EnemyModifier modifier = EnemyModifier.GetModifierExcluding(miniBossData.EncounteredModifiers);
        if ((Object) modifier == (Object) null)
        {
          miniBossData.EncounteredModifiers.Clear();
          modifier = EnemyModifier.GetModifier(1f);
        }
        this.modifier = modifier;
        this.EnemiesToTrack[0].GetComponent<UnitObject>().ForceSetModifier(modifier);
        miniBossData.EncounteredModifiers.Add(modifier.Modifier);
      }
    }
    this.EnemiesToTrack[0].GetComponent<Health>().OnDie += new Health.DieAction(this.MiniBossController_OnDie);
    foreach (Health health in this.EnemiesToTrack)
    {
      health.totalHP *= DataManager.Instance.BossHealthMultiplier;
      health.HP *= DataManager.Instance.BossHealthMultiplier;
    }
    foreach (BaseMonoBehaviour baseMonoBehaviour in this.ComponentsToToggleEnabled)
    {
      if ((Object) baseMonoBehaviour != (Object) null)
      {
        baseMonoBehaviour.enabled = false;
        baseMonoBehaviour.transform.position = Vector3.zero;
      }
    }
  }

  public void MiniBossController_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Vector3 position = Victim.transform.position with
    {
      z = 0.0f
    };
    Victim.transform.position = position;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    DataManager.Instance.TakenBossDamage = this.PlayerHit;
    Debug.Log((object) ("Taken Boss Damage = " + this.PlayerHit.ToString()));
    GameManager.GetInstance().RemoveFromCamera(this.cameraTarget);
    UIBossHUD.Hide();
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
        DataManager.Instance.LastRunResults = Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results.BeatenBoss;
    }
    else
      DataManager.Instance.LastRunResults = Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results.BeatenMiniBoss;
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

  public bool BeatenPostGameLeader()
  {
    return PlayerFarming.Location == FollowerLocation.Dungeon1_1 && DataManager.Instance.BeatenLeshyLayer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_2 && DataManager.Instance.BeatenHeketLayer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_3 && DataManager.Instance.BeatenKallamarLayer2 || PlayerFarming.Location == FollowerLocation.Dungeon1_4 && DataManager.Instance.BeatenShamuraLayer2;
  }

  public IEnumerator KillEnemiesDelay()
  {
    MiniBossController miniBossController = this;
    yield return (object) new WaitForSeconds(1f);
    Health component = (Object) miniBossController.EnemiesToTrack[0] != (Object) null ? miniBossController.EnemiesToTrack[0].GetComponent<Health>() : (Health) null;
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((Object) Health.team2[index] != (Object) null && (Object) Health.team2[index] != (Object) component && Health.team2[index].gameObject.activeInHierarchy)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, miniBossController.gameObject, miniBossController.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
  }

  public virtual void OnEnable()
  {
    if (this.shown && !this.bossDead)
    {
      UIBossHUD.Play(this.EnemiesToTrack[0], this.Name);
      UIBossHUD.Instance.ForceHealthAmount(this.EnemiesToTrack[0].HP / this.EnemiesToTrack[0].totalHP);
      this.SetMusic();
    }
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public void OnDisable()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public void OnDestroy()
  {
    MiniBossController.Instance = (MiniBossController) null;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
  }

  public void SetMusic()
  {
    switch (this.BossIntro.GetComponentInChildren<UnitObject>().EnemyType)
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

  public void TrackPlayerHealth()
  {
    this.PlayerHit = false;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.OnHitCallback.AddListener(new UnityAction(this.OnPlayerHit));
  }

  public void OnPlayerHit() => this.PlayerHit = true;

  public void Play(bool skipped = false)
  {
    this.StartCoroutine((IEnumerator) this.IntroRoutine(skipped));
  }

  public virtual IEnumerator IntroRoutine(bool skipped = false)
  {
    MiniBossController miniBossController = this;
    SimulationManager.Pause();
    MiniBossController.Instance = miniBossController;
    miniBossController.TrackPlayerHealth();
    if (miniBossController.ShowName)
    {
      UnitObject component = miniBossController.BossIntro.GetComponent<UnitObject>();
      if ((component != null ? (component.EnemyType == Enemy.FrogBoss ? 1 : 0) : 0) != 0)
        HUD_DisplayName.Play(miniBossController.Name, 2, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.FrogBoss);
      else
        HUD_DisplayName.Play(miniBossController.Name, 2, HUD_DisplayName.Positions.Centre);
    }
    foreach (Health health in miniBossController.EnemiesToTrack)
    {
      health.SlowMoOnkill = true;
      ShowHPBar component = health.GetComponent<ShowHPBar>();
      if ((bool) (Object) component)
        component.enabled = false;
    }
    GameManager.SetGlobalOcclusionActive(false);
    yield return (object) miniBossController.StartCoroutine((IEnumerator) miniBossController.BossIntro.PlayRoutine(skipped));
    GameManager.SetGlobalOcclusionActive(true);
    miniBossController.SetMusic();
    foreach (Behaviour behaviour in miniBossController.ComponentsToToggleEnabled)
      behaviour.enabled = true;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    UIBossHUD.Play(miniBossController.EnemiesToTrack[0], miniBossController.Name);
    GameManager.GetInstance().AddToCamera(miniBossController.cameraTarget);
    miniBossController.shown = true;
    RoomLockController.CloseAll();
  }

  public IEnumerator OutroRoutine()
  {
    yield return (object) null;
    foreach (Health health in Health.team2)
    {
      if ((bool) (Object) health)
        health.DestroyNextFrame();
    }
  }

  public void OnLanguageChanged() => UIBossHUD.Instance?.UpdateName(this.Name);

  public virtual void OnDeathLogicCustom(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
  }

  [MessagePackObject(false)]
  public class MiniBossData
  {
    [Key(0)]
    public Enemy EnemyType;
    [Key(1)]
    public List<EnemyModifier.ModifierType> EncounteredModifiers;
  }
}
