// Decompiled with JetBrains decompiler
// Type: MiniBossManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using I2.Loc;
using Map;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class MiniBossManager : BaseMonoBehaviour
{
  public static int CurrentIndex;
  public List<MiniBossController> BossEncounters;
  public MiniBossController CurrentMiniBoss;
  public Interaction_Chest Chest;
  public List<SingleChoiceRewardOption> SingleChoiceRewardOptions = new List<SingleChoiceRewardOption>();
  public List<Interaction_WeaponSelectionPodium> WeaponPodiums = new List<Interaction_WeaponSelectionPodium>();
  public GameObject FollowerToSpawn;
  public int ForcedIndex = -1;
  public bool revealed;
  public bool introPlayed;
  public InventoryItem.ITEM_TYPE ForceReward;
  public GameObject Follower;
  public bool forceEnemiesPos = true;
  public int DeathCount;
  [CompilerGenerated]
  public bool \u003CCompleted\u003Ek__BackingField;
  public GameObject Boss;

  public void Start()
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>(true));
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
    {
      if (DataManager.Instance.BeatenYngya)
      {
        this.ForcedIndex = UnityEngine.Random.Range(0, this.BossEncounters.Count - 1);
        if (!DataManager.Instance.CheckKilledBosses(this.BossEncounters[this.BossEncounters.Count - 1].name))
          this.ForcedIndex = this.BossEncounters.Count - 1;
      }
      else
        this.ForcedIndex = GameManager.CurrentDungeonLayer != 3 || !GameManager.DungeonUseAllLayers ? GameManager.CurrentDungeonLayer - 1 : GameManager.CurrentDungeonLayer;
      if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Witness || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Witness)
        this.ForcedIndex = this.BossEncounters.Count - 1;
    }
    if (CheatConsole.ForcedMiniBossIndex != -1)
      this.ForcedIndex = CheatConsole.ForcedMiniBossIndex;
    if (this.ForcedIndex != -1)
    {
      this.CurrentMiniBoss = this.BossEncounters[this.ForcedIndex];
    }
    else
    {
      this.CurrentMiniBoss = this.BossEncounters[GameManager.CurrentDungeonLayer - 1];
      if (DataManager.Instance.DungeonCompleted(PlayerFarming.Location) && !GameManager.Layer2 || DungeonSandboxManager.Active)
      {
        if (!DataManager.Instance.CheckKilledBosses(this.BossEncounters[this.BossEncounters.Count - 1].name) && !DungeonSandboxManager.Active)
        {
          this.CurrentMiniBoss = this.BossEncounters[this.BossEncounters.Count - 1];
        }
        else
        {
          do
          {
            this.CurrentMiniBoss = this.BossEncounters[UnityEngine.Random.Range(0, this.BossEncounters.Count)];
          }
          while (this.CurrentMiniBoss.name == DataManager.Instance.PreviousMiniBoss);
        }
      }
      else if (GameManager.Layer2)
      {
        if (DataManager.Instance.DungeonCompleted(PlayerFarming.Location, true) && !DataManager.Instance.CheckKilledBosses(this.BossEncounters[this.BossEncounters.Count - 1].name + "_P2") && !DungeonSandboxManager.Active)
        {
          this.CurrentMiniBoss = this.BossEncounters[this.BossEncounters.Count - 1];
          this.ForceReward = InventoryItem.ITEM_TYPE.GOD_TEAR;
        }
        else
        {
          this.CurrentMiniBoss = (MiniBossController) null;
          if (!DataManager.Instance.CheckKilledBosses(this.BossEncounters[this.BossEncounters.Count - 1].name + "_P2"))
          {
            this.BossEncounters[this.BossEncounters.Count - 1].gameObject.SetActive(false);
            this.BossEncounters.RemoveAt(this.BossEncounters.Count - 1);
          }
          this.BossEncounters.Shuffle<MiniBossController>();
          foreach (MiniBossController bossEncounter in this.BossEncounters)
          {
            if (!DataManager.Instance.CheckKilledBosses(bossEncounter.name + "_P2") && !DungeonSandboxManager.Active)
            {
              this.CurrentMiniBoss = bossEncounter;
              this.ForceReward = InventoryItem.ITEM_TYPE.GOD_TEAR;
              break;
            }
          }
          if ((UnityEngine.Object) this.CurrentMiniBoss == (UnityEngine.Object) null)
          {
            do
            {
              this.CurrentMiniBoss = this.BossEncounters[UnityEngine.Random.Range(0, this.BossEncounters.Count)];
            }
            while (this.CurrentMiniBoss.name + "_P2" == DataManager.Instance.PreviousMiniBoss);
          }
        }
      }
    }
    DataManager.Instance.PreviousMiniBoss = this.CurrentMiniBoss.name + (GameManager.Layer2 ? "_P2" : "");
    if (DungeonSandboxManager.Active)
    {
      if (MapManager.Instance.CurrentMap.GetFinalBossNode() == MapManager.Instance.CurrentNode)
        this.ForceReward = InventoryItem.ITEM_TYPE.GOD_TEAR;
      int num = 0;
      while (num < 100)
      {
        ++num;
        this.CurrentMiniBoss = this.BossEncounters[UnityEngine.Random.Range(0, 4)];
        if (!DungeonSandboxManager.Instance.EncounteredMiniBosses.Contains(this.CurrentMiniBoss.name))
          break;
      }
      if (!DungeonSandboxManager.Instance.EncounteredMiniBosses.Contains(this.CurrentMiniBoss.name))
        DungeonSandboxManager.Instance.EncounteredMiniBosses.Add(this.CurrentMiniBoss.name);
    }
    foreach (MiniBossController bossEncounter in this.BossEncounters)
    {
      bossEncounter.gameObject.GetComponentInChildren<UnitObject>().CanHaveModifier = false;
      bossEncounter.gameObject.SetActive((UnityEngine.Object) bossEncounter == (UnityEngine.Object) this.CurrentMiniBoss);
      bossEncounter.gameObject.transform.position = Vector3.zero;
      if (this.ForceReward == InventoryItem.ITEM_TYPE.GOD_TEAR)
        bossEncounter.GetComponentInChildren<Health>().SlowMoOnkill = true;
    }
  }

  public void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.FrameDelayCallback((System.Action) (() =>
    {
      if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) && !((UnityEngine.Object) Interaction_DivineCrystal.Instance != (UnityEngine.Object) null))
        return;
      this.StartCoroutine((IEnumerator) this.WaitForFollowerToBeRecruited());
    })));
  }

  public void FixedUpdate() => this.ForceBossIntoCorrectPosition();

  public void ForceBossIntoCorrectPosition()
  {
    if ((UnityEngine.Object) this.CurrentMiniBoss == (UnityEngine.Object) null || !this.CurrentMiniBoss.forceMinibossIntoCorrectPosition)
      return;
    if (!this.introPlayed && !this.CurrentMiniBoss.multipleBoss)
      this.CurrentMiniBoss.BossIntro.transform.position = Vector3.zero;
    if (!this.CurrentMiniBoss.multipleBoss || !this.forceEnemiesPos)
      return;
    for (int index = 0; index < this.CurrentMiniBoss.EnemiesToTrack.Count; ++index)
      this.CurrentMiniBoss.EnemiesToTrack[index].transform.position = Vector3.zero - Vector3.right * 3f + Vector3.right * 6f * (float) index;
  }

  public void GetAndNameBosses(int DungeonNumber)
  {
    this.BossEncounters = new List<MiniBossController>((IEnumerable<MiniBossController>) this.GetComponentsInChildren<MiniBossController>());
    int index = -1;
    while (++index < this.BossEncounters.Count)
      this.BossEncounters[index].DisplayName = $"NAMES/MiniBoss/Dungeon{DungeonNumber.ToString()}/MiniBoss{(index + 1).ToString()}";
  }

  public void Play()
  {
    DOTween.Kill((object) this);
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + 1.5f, 1f).SetEase<Tweener>(Ease.OutQuart);
    foreach (Health health in this.CurrentMiniBoss.EnemiesToTrack)
    {
      if (this.CurrentMiniBoss.forceMinibossIntoCorrectPosition)
        health.transform.position = Vector3.zero;
      if (this.CurrentMiniBoss.hasCustomPostDeathLogic)
        health.OnDie += new Health.DieAction(this.CurrentMiniBoss.OnDeathLogicCustom);
      else
        health.OnDie += new Health.DieAction(this.H_OnDie);
    }
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  public IEnumerator IntroRoutine()
  {
    MiniBossManager miniBossManager = this;
    miniBossManager.introPlayed = true;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.CustomAnimation);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) miniBossManager.StartCoroutine((IEnumerator) miniBossManager.CurrentMiniBoss.IntroRoutine());
    miniBossManager.forceEnemiesPos = false;
  }

  public void IncrementDeathCount() => ++this.DeathCount;

  public bool Completed
  {
    get => this.\u003CCompleted\u003Ek__BackingField;
    set => this.\u003CCompleted\u003Ek__BackingField = value;
  }

  public void H_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    ++this.DeathCount;
    Victim.OnDie -= new Health.DieAction(this.H_OnDie);
    if (this.CurrentMiniBoss.multipleBoss && this.DeathCount < this.CurrentMiniBoss.bossesNumber)
      return;
    TrapPoison.RemoveAllPoison();
    Projectile.ClearProjectiles();
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_5:
        ++DataManager.Instance.DLCDungeon5MiniBossIndex;
        break;
      case FollowerLocation.Dungeon1_6:
        ++DataManager.Instance.DLCDungeon6MiniBossIndex;
        break;
    }
    if (this.ForceReward == InventoryItem.ITEM_TYPE.GOD_TEAR)
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, Vector3.ClampMagnitude(Victim.transform.position + Vector3.back, 4f), 0.0f);
      pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      pickUp.MagnetToPlayer = false;
      this.ForceReward = InventoryItem.ITEM_TYPE.NONE;
      PlayerReturnToBase.Disabled = true;
    }
    if (!this.Completed && (Victim.gameObject.CompareTag("Boss") || Health.team2.Count <= 1))
    {
      DOTween.Kill((object) this);
      DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
      this.Completed = true;
      if (!DataManager.Instance.CheckKilledBosses(this.CurrentMiniBoss.name) && !DungeonSandboxManager.Active)
      {
        PlayerReturnToBase.Disabled = true;
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_FIRST_BOSS"));
        AudioManager.Instance.PlayOneShot("event:/Stings/boss_kill", PlayerFarming.Instance.gameObject);
        this.Boss = Victim.gameObject;
        Vector3 position = this.Boss.transform.position;
        if ((bool) (UnityEngine.Object) Victim.GetComponent<EnemyDogMiniboss>())
          position.z = !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) ? 0.0f : PlayerFarming.Instance.transform.position.z;
        this.Follower = UnityEngine.Object.Instantiate<GameObject>(this.FollowerToSpawn, position, Quaternion.identity, this.Boss.transform.parent);
        if (!this.CurrentMiniBoss.multipleBoss)
        {
          this.Follower.GetComponent<Interaction_FollowerSpawn>().Play(this.CurrentMiniBoss.name, LocalizationManager.GetTranslation(this.CurrentMiniBoss.DisplayName));
        }
        else
        {
          string Term = (UnityEngine.Object) Victim == (UnityEngine.Object) this.CurrentMiniBoss.EnemiesToTrack[0] ? (this.CurrentMiniBoss as DoubleMinibossController).bossName1 : (this.CurrentMiniBoss as DoubleMinibossController).bossName2;
          this.Follower.GetComponent<Interaction_FollowerSpawn>().Play(this.CurrentMiniBoss.name, LocalizationManager.GetTranslation(Term));
        }
        DataManager.SetFollowerSkinUnlocked(this.CurrentMiniBoss.name);
      }
      string name = this.CurrentMiniBoss.name;
      if (GameManager.Layer2)
        name += "_P2";
      if (!DataManager.Instance.CheckKilledBosses(name) && !DungeonSandboxManager.Active)
      {
        switch (this.CurrentMiniBoss.name)
        {
          case "Boss Beholder 1":
          case "Boss Beholder 2":
          case "Boss Beholder 3":
          case "Boss Beholder 4":
            this.ForceReward = InventoryItem.ITEM_TYPE.BEHOLDER_EYE;
            break;
          case "Boss Beholder 5":
          case "Boss Beholder 6":
            this.ForceReward = InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT;
            break;
        }
      }
    }
    if (GameManager.CurrentDungeonLayer >= 4)
      return;
    DataManager.Instance.SetDungeonLayer(BiomeGenerator.Instance.DungeonLocation, GameManager.CurrentDungeonLayer + 1);
  }

  public InventoryItem.ITEM_TYPE GetForceReward() => this.ForceReward;

  public void Update()
  {
    if (this.introPlayed && !this.revealed && this.DeathCount >= this.CurrentMiniBoss.EnemiesToTrack.Count)
    {
      this.revealed = true;
      this.StartCoroutine((IEnumerator) this.RevealRoutine());
    }
    this.ForceBossIntoCorrectPosition();
    if (!((UnityEngine.Object) this.CurrentMiniBoss != (UnityEngine.Object) null) || !this.CurrentMiniBoss.multipleBoss || !this.forceEnemiesPos || !this.CurrentMiniBoss.forceMinibossIntoCorrectPosition)
      return;
    for (int index = 0; index < this.CurrentMiniBoss.EnemiesToTrack.Count; ++index)
      this.CurrentMiniBoss.EnemiesToTrack[index].transform.position = Vector3.zero - Vector3.right * 3f + Vector3.right * 6f * (float) index;
  }

  public IEnumerator RevealRoutine()
  {
    MiniBossManager miniBossManager = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    yield return (object) miniBossManager.StartCoroutine((IEnumerator) miniBossManager.CurrentMiniBoss.OutroRoutine());
    yield return (object) new WaitForSecondsRealtime(1.25f);
    if (!DataManager.Instance.CheckKilledBosses(miniBossManager.CurrentMiniBoss.name) && !DungeonSandboxManager.Active)
      yield return (object) new WaitForSeconds(2.5f);
    miniBossManager.StartCoroutine((IEnumerator) miniBossManager.WaitForFollowerToBeRecruited());
  }

  public void SetFollowerToWaitFor(GameObject follower) => this.Follower = follower;

  public IEnumerator WaitForFollowerToBeRecruited()
  {
    MiniBossManager miniBossManager1 = this;
    while ((UnityEngine.Object) miniBossManager1.Follower != (UnityEngine.Object) null)
      yield return (object) null;
    while ((UnityEngine.Object) Interaction_DivineCrystal.Instance != (UnityEngine.Object) null)
      yield return (object) null;
    if (DungeonSandboxManager.Active)
    {
      if (MapManager.Instance.CurrentMap.GetFinalBossNode() != MapManager.Instance.CurrentNode)
      {
        foreach (Interaction_WeaponSelectionPodium weaponPodium1 in miniBossManager1.WeaponPodiums)
        {
          MiniBossManager miniBossManager = miniBossManager1;
          Interaction_WeaponSelectionPodium s = weaponPodium1;
          s.Reveal();
          s.OnInteraction += (Interaction.InteractionEvent) (state =>
          {
            foreach (Interaction_WeaponSelectionPodium weaponPodium2 in miniBossManager.WeaponPodiums)
            {
              if ((UnityEngine.Object) weaponPodium2 != (UnityEngine.Object) s)
              {
                weaponPodium2.Interactable = false;
                weaponPodium2.Lighting.SetActive(false);
                weaponPodium2.IconSpriteRenderer.enabled = false;
                weaponPodium2.weaponBetterIcon.enabled = false;
                weaponPodium2.podiumOn.SetActive(false);
                weaponPodium2.podiumOff.SetActive(true);
                weaponPodium2.particleEffect.Stop();
                weaponPodium2.AvailableGoop.gameObject.SetActive(false);
                weaponPodium2.enabled = false;
              }
            }
            miniBossManager.StartCoroutine((IEnumerator) miniBossManager.WaitForUIToFinish(miniBossManager.ForceReward, 1f));
          });
        }
      }
      else
        miniBossManager1.StartCoroutine((IEnumerator) miniBossManager1.WaitForUIToFinish(miniBossManager1.ForceReward));
    }
    else if (miniBossManager1.SingleChoiceRewardOptions.Count > 0)
    {
      if ((PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6) && DataManager.Instance.GetDungeonLayer(PlayerFarming.Location) <= 1)
      {
        miniBossManager1.SingleChoiceRewardOptions[0].transform.position = Vector3.zero;
        miniBossManager1.SingleChoiceRewardOptions[0].Reveal();
        miniBossManager1.SingleChoiceRewardOptions[0].Callback.AddListener(new UnityAction(miniBossManager1.\u003CWaitForFollowerToBeRecruited\u003Eb__32_0));
        miniBossManager1.SingleChoiceRewardOptions[1].gameObject.SetActive(false);
      }
      else
      {
        foreach (SingleChoiceRewardOption choiceRewardOption in miniBossManager1.SingleChoiceRewardOptions)
        {
          choiceRewardOption.Reveal();
          choiceRewardOption.Callback.AddListener(new UnityAction(miniBossManager1.\u003CWaitForFollowerToBeRecruited\u003Eb__32_3));
        }
      }
    }
    else
      miniBossManager1.Chest.RevealBossReward(miniBossManager1.ForceReward, new System.Action(miniBossManager1.\u003CWaitForFollowerToBeRecruited\u003Eb__32_1));
    PlayerReturnToBase.Disabled = false;
  }

  public IEnumerator WaitForUIToFinish(InventoryItem.ITEM_TYPE ForceReward, float delay = 0.0f)
  {
    MiniBossManager miniBossManager = this;
    yield return (object) null;
    while (FoundItemPickUp.FoundItemPickUps.Count > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(delay);
    miniBossManager.Chest.RevealBossReward(ForceReward, new System.Action(miniBossManager.\u003CWaitForUIToFinish\u003Eb__33_0));
  }

  public IEnumerator FrameDelayCallback(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__13_0()
  {
    if (!((UnityEngine.Object) this.Follower != (UnityEngine.Object) null) && !((UnityEngine.Object) Interaction_DivineCrystal.Instance != (UnityEngine.Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.WaitForFollowerToBeRecruited());
  }

  [CompilerGenerated]
  public void \u003CWaitForFollowerToBeRecruited\u003Eb__32_0()
  {
    this.StartCoroutine((IEnumerator) this.WaitForUIToFinish(this.ForceReward));
  }

  [CompilerGenerated]
  public void \u003CWaitForFollowerToBeRecruited\u003Eb__32_3()
  {
    this.StartCoroutine((IEnumerator) this.WaitForUIToFinish(this.ForceReward));
  }

  [CompilerGenerated]
  public void \u003CWaitForFollowerToBeRecruited\u003Eb__32_1()
  {
    if (!DataManager.Instance.CheckKilledBosses(this.CurrentMiniBoss.name) && !DungeonSandboxManager.Active)
    {
      DataManager.Instance.AddKilledBoss(this.CurrentMiniBoss.name);
    }
    else
    {
      if (!GameManager.Layer2 || DungeonSandboxManager.Active)
        return;
      DataManager.Instance.AddKilledBoss(this.CurrentMiniBoss.name + "_P2");
    }
  }

  [CompilerGenerated]
  public void \u003CWaitForUIToFinish\u003Eb__33_0()
  {
    if (!DataManager.Instance.CheckKilledBosses(this.CurrentMiniBoss.name) && !DungeonSandboxManager.Active)
    {
      DataManager.Instance.AddKilledBoss(this.CurrentMiniBoss.name);
    }
    else
    {
      if (!GameManager.Layer2 || DungeonSandboxManager.Active)
        return;
      DataManager.Instance.AddKilledBoss(this.CurrentMiniBoss.name + "_P2");
    }
  }
}
