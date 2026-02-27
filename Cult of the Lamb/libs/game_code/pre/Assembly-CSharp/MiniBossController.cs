// Decompiled with JetBrains decompiler
// Type: MiniBossController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.DeathScreen;
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
  [Space]
  [SerializeField]
  private bool cycleModifiers = true;
  private EnemyModifier modifier;
  private bool shown;
  private bool bossDead;
  [SerializeField]
  private bool isBigBoss;
  private bool PlayerHit;
  private bool WaitingForAnimationToComplete;

  private void Awake()
  {
    MiniBossController.MiniBossData miniBossData = DataManager.Instance.GetMiniBossData(this.BossIntro.GetComponent<UnitObject>().EnemyType);
    if (DataManager.Instance != null && miniBossData != null && this.cycleModifiers)
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
    this.EnemiesToTrack[0].GetComponent<Health>().OnDie += new Health.DieAction(this.MiniBossController_OnDie);
    foreach (BaseMonoBehaviour baseMonoBehaviour in this.ComponentsToToggleEnabled)
    {
      if ((Object) baseMonoBehaviour != (Object) null)
        baseMonoBehaviour.enabled = false;
    }
  }

  private void MiniBossController_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    PlayerFarming.Instance.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    DataManager.Instance.TakenBossDamage = this.PlayerHit;
    Debug.Log((object) ("Taken Boss Damage = " + this.PlayerHit.ToString()));
    GameManager.GetInstance().RemoveFromCamera(this.EnemiesToTrack[0].gameObject);
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
      if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
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
    if (_cultLeader == -1)
      return;
    MonoSingleton<PlayerProgress_Analytics>.Instance.CultLeaderComplete(_cultLeader);
  }

  private IEnumerator KillEnemiesDelay()
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

  private void OnEnable()
  {
    if (!this.shown || this.bossDead)
      return;
    UIBossHUD.Play(this.EnemiesToTrack[0], LocalizationManager.Sources[0].GetTranslation(this.DisplayName));
    UIBossHUD.Instance.ForceHealthAmount(this.EnemiesToTrack[0].HP / this.EnemiesToTrack[0].totalHP);
    this.EnemiesToTrack[0].SlowMoOnkill = true;
    this.SetMusic();
  }

  private void OnDisable()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    PlayerFarming.Instance.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
  }

  private void OnDestroy()
  {
    MiniBossController.Instance = (MiniBossController) null;
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    PlayerFarming.Instance.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
  }

  private void SetMusic()
  {
    switch (this.BossIntro.GetComponent<UnitObject>().EnemyType)
    {
      case Enemy.Beholder1:
      case Enemy.Beholder2:
      case Enemy.Beholder3:
      case Enemy.Beholder4:
      case Enemy.Beholder5:
        AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BeholderBattle);
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

  private void TrackPlayerHealth()
  {
    this.PlayerHit = false;
    PlayerFarming.Instance.health.OnHitCallback.AddListener(new UnityAction(this.OnPlayerHit));
  }

  private void OnPlayerHit() => this.PlayerHit = true;

  public void Play(bool skipped = false)
  {
    this.StartCoroutine((IEnumerator) this.IntroRoutine(skipped));
  }

  public IEnumerator IntroRoutine(bool skipped = false)
  {
    MiniBossController miniBossController = this;
    SimulationManager.Pause();
    MiniBossController.Instance = miniBossController;
    miniBossController.TrackPlayerHealth();
    if (miniBossController.ShowName)
    {
      if (miniBossController.BossIntro.GetComponent<UnitObject>().EnemyType == Enemy.FrogBoss)
        HUD_DisplayName.Play(miniBossController.DisplayName, 2, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.FrogBoss);
      else
        HUD_DisplayName.Play(miniBossController.DisplayName, 2, HUD_DisplayName.Positions.Centre);
    }
    foreach (Component component1 in miniBossController.EnemiesToTrack)
    {
      ShowHPBar component2 = component1.GetComponent<ShowHPBar>();
      if ((bool) (Object) component2)
        component2.enabled = false;
    }
    yield return (object) miniBossController.StartCoroutine((IEnumerator) miniBossController.BossIntro.PlayRoutine(skipped));
    miniBossController.SetMusic();
    foreach (Behaviour behaviour in miniBossController.ComponentsToToggleEnabled)
      behaviour.enabled = true;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    UIBossHUD.Play(miniBossController.EnemiesToTrack[0], LocalizationManager.Sources[0].GetTranslation(miniBossController.DisplayName));
    GameManager.GetInstance().AddToCamera(miniBossController.EnemiesToTrack[0].gameObject);
    miniBossController.shown = true;
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

  public class MiniBossData
  {
    public Enemy EnemyType;
    public List<EnemyModifier.ModifierType> EncounteredModifiers;
  }
}
