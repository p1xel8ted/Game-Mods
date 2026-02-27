// Decompiled with JetBrains decompiler
// Type: BuildingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BuildingShrine : Interaction
{
  public static List<BuildingShrine> Shrines = new List<BuildingShrine>();
  public DevotionCounterOverlay devotionCounterOverlay;
  public GameObject ReceiveSoulPosition;
  public Structure Structure;
  private string sString;
  private string sResearch;
  public GameObject ShrineCanLevelUp;
  public GameObject ShrineCantLevelUp;
  public Animator shrineLevelUpAnimator;
  public Animator twitchLevelUpAnimator;
  public SpriteXPBar XPBar;
  [SerializeField]
  private Interaction_AddFuel addFuel;
  [SerializeField]
  private GameObject[] flameLevels;
  [Space]
  [SerializeField]
  private GameObject[] spawnPositions;
  public SpriteRenderer ShrineGlow;
  public ParticleSystem psDevotion;
  private bool hasUnlockAvailable;
  private ParticleSystem.EmissionModule emissionModule;
  private Coroutine cFadeGlowMaterial;
  private bool wasFull;
  private GameObject Player;
  private bool Activating;
  private float Delay;
  public float DistanceToTriggerDeposits = 5f;
  private float ReduceDelay = 0.1f;

  public Structures_Shrine StructureBrain => this.Structure.Brain as Structures_Shrine;

  public GameObject[] SpawnPositions => this.spawnPositions;

  private void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_FlameIII))
      this.addFuel.MaxFuel /= 2;
    TwitchAuthentication.OnAuthenticated += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchTotem.TotemUpdated += new TwitchTotem.TotemResponse(this.TwitchTotem_TotemUpdated);
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.OnUpgradeUnlocked);
    UpgradeSystem.OnAbilityPointDelta += new System.Action(this.CheckXP);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.addFuel.OnFuelModified += new Interaction_AddFuel.FuelEvent(this.OnFuelModified);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    TwitchAuthentication.OnAuthenticated -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchTotem.TotemUpdated -= new TwitchTotem.TotemResponse(this.TwitchTotem_TotemUpdated);
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.OnUpgradeUnlocked);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    if (this.StructureBrain != null)
      this.StructureBrain.OnSoulsGained -= new System.Action<int>(this.OnSoulsGained);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    UpgradeSystem.OnAbilityPointDelta -= new System.Action(this.CheckXP);
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.OnUpgradeUnlocked);
    if (!((UnityEngine.Object) this.addFuel != (UnityEngine.Object) null))
      return;
    this.addFuel.OnFuelModified -= new Interaction_AddFuel.FuelEvent(this.OnFuelModified);
  }

  private void TwitchAuthentication_OnAuthenticated() => this.CheckXP();

  private void TwitchTotem_TotemUpdated(int contributions) => this.CheckXP();

  public override void OnEnableInteraction()
  {
    DataManager.Instance.ShrineLevel = 1;
    base.OnEnableInteraction();
    BuildingShrine.Shrines.Add(this);
    this.CheckXP();
  }

  private void OnUpgradeUnlocked(UpgradeSystem.Type upgradeType)
  {
    this.addFuel.gameObject.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_Flame));
    int num = 0;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_FlameII))
      num = 2;
    else if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_FlameIII))
      num = 1;
    for (int index = 0; index < this.flameLevels.Length; ++index)
      this.flameLevels[index].SetActive(index == num);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
    if (!DataManager.Instance.XPEnabled && this.StructureBrain.Data.Type == global::StructureBrain.TYPES.SHRINE)
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.RepairTheShrine);
      DataManager.Instance.XPEnabled = true;
      GameManager.RecalculatePaths(true);
    }
    if (this.Structure.Type == global::StructureBrain.TYPES.SHRINE)
      DataManager.Instance.HasBuiltShrine1 = true;
    if (this.Structure.Type == global::StructureBrain.TYPES.SHRINE_II)
      DataManager.Instance.HasBuiltShrine2 = true;
    if (this.Structure.Type == global::StructureBrain.TYPES.SHRINE_III)
      DataManager.Instance.HasBuiltShrine3 = true;
    if (this.Structure.Type == global::StructureBrain.TYPES.SHRINE_IV)
    {
      DataManager.Instance.HasBuiltShrine4 = true;
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FULLY_UPGRADED_SHRINE"));
    }
    this.OnUpgradeUnlocked(UpgradeSystem.Type.Ability_Eat);
    this.emissionModule = this.psDevotion.emission;
    this.CheckXP();
  }

  private IEnumerator NewFollowerAndTutorial()
  {
    BuildingShrine buildingShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(buildingShrine.gameObject, 8f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    FollowerManager.CreateNewRecruit(FollowerLocation.Base, BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 10f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
  }

  private void OnStructuresPlaced()
  {
    this.UpdateBar();
    DataManager.Instance.ShrineLevel = 1;
  }

  private void OnFuelModified(float fuel)
  {
    if (this.Structure.Structure_Info.FullyFueled)
      this.addFuel.Interactable = false;
    else
      this.addFuel.Interactable = true;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    BuildingShrine.Shrines.Remove(this);
  }

  public void CheckXP()
  {
    if ((!TwitchAuthentication.IsAuthenticated ? 0 : (TwitchTotem.TotemUnlockAvailable ? 1 : 0)) != 0)
    {
      AnimatorStateInfo animatorStateInfo = this.twitchLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
      if (!animatorStateInfo.IsName("Show"))
      {
        animatorStateInfo = this.twitchLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Shown"))
          this.twitchLevelUpAnimator.Play("Show");
      }
      animatorStateInfo = this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
      if (animatorStateInfo.IsName("Shown"))
      {
        this.shrineLevelUpAnimator.Play("Hide");
      }
      else
      {
        animatorStateInfo = this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Hidden"))
          this.shrineLevelUpAnimator.Play("Hidden");
      }
      this.ShrineCanLevelUp.SetActive(false);
      this.ShrineCantLevelUp.SetActive(true);
    }
    else if (UpgradeSystem.AbilityPoints > 0 && GameManager.HasUnlockAvailable())
    {
      if ((bool) (UnityEngine.Object) this.ShrineCanLevelUp && !this.ShrineCanLevelUp.activeSelf)
        AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_ready", this.gameObject);
      AnimatorStateInfo animatorStateInfo = this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
      if (!animatorStateInfo.IsName("Show"))
      {
        animatorStateInfo = this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Shown"))
          this.shrineLevelUpAnimator.Play("Show");
      }
      if ((bool) (UnityEngine.Object) this.ShrineCanLevelUp)
        this.ShrineCanLevelUp?.SetActive(true);
      if ((bool) (UnityEngine.Object) this.ShrineCantLevelUp)
        this.ShrineCantLevelUp?.SetActive(false);
      int id = Shader.PropertyToID("_Cutoff");
      this.ShrineGlow.material.DOKill();
      this.ShrineGlow.material.DOFloat(0.9f, id, 1f);
      if (this.StructureBrain != null)
        this.psDevotion.emission.rateOverTime = (ParticleSystem.MinMaxCurve) Mathf.Lerp(1f, 3f, Mathf.Cos(Time.deltaTime));
      animatorStateInfo = this.twitchLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
      if (animatorStateInfo.IsName("Shown"))
        this.twitchLevelUpAnimator.Play("Hide");
      else
        this.twitchLevelUpAnimator.Play("Hidden");
    }
    else
    {
      AnimatorStateInfo animatorStateInfo = this.shrineLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
      if (animatorStateInfo.IsName("Shown"))
        this.shrineLevelUpAnimator.Play("Hide");
      else
        this.shrineLevelUpAnimator.Play("Hidden");
      animatorStateInfo = this.twitchLevelUpAnimator.GetCurrentAnimatorStateInfo(0);
      if (animatorStateInfo.IsName("Shown"))
        this.twitchLevelUpAnimator.Play("Hide");
      else
        this.twitchLevelUpAnimator.Play("Hidden");
      if ((bool) (UnityEngine.Object) this.ShrineCanLevelUp)
        this.ShrineCanLevelUp.SetActive(false);
      if (!(bool) (UnityEngine.Object) this.ShrineCantLevelUp)
        return;
      this.ShrineCantLevelUp.SetActive(true);
    }
  }

  private IEnumerator FadeGlowMaterial()
  {
    float Progress = 0.0f;
    float Duration = 1f;
    int CutOffID = Shader.PropertyToID("_Cutoff");
    float Target = this.ShrineGlow.sharedMaterial.GetFloat(CutOffID);
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.ShrineGlow.material.SetFloat(CutOffID, Mathf.Lerp(0.0f, Target, Progress / Duration));
      yield return (object) null;
    }
    this.ShrineGlow.material.SetFloat(CutOffID, Target);
  }

  public override void IndicateHighlighted() => base.IndicateHighlighted();

  public override void EndIndicateHighlighted() => base.EndIndicateHighlighted();

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
      this.Label = "";
    else if ((!TwitchAuthentication.IsAuthenticated || !TwitchTotem.TotemUnlockAvailable ? 0 : (!TwitchTotem.Deactivated ? 1 : 0)) != 0)
    {
      this.Interactable = true;
      this.HasSecondaryInteraction = false;
      this.Label = LocalizationManager.GetTranslation("UI/Twitch/Totem/CollectReward").Colour(StaticColors.TwitchPurple);
    }
    else if (UpgradeSystem.AbilityPoints > 0 && GameManager.HasUnlockAvailable())
    {
      this.Interactable = true;
      this.HasSecondaryInteraction = false;
      this.Label = ScriptLocalization.Interactions.CollectNewAbility;
    }
    else
    {
      this.Interactable = this.StructureBrain.SoulCount > 0;
      this.HasSecondaryInteraction = true;
      this.hasUnlockAvailable = GameManager.HasUnlockAvailable();
      this.Label = $"{this.sString} {(this.hasUnlockAvailable ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">")} {(object) this.StructureBrain.SoulCount}{StaticColors.GreyColorHex} / {(object) this.StructureBrain.SoulMax}";
    }
  }

  public override void GetSecondaryLabel()
  {
    bool flag = TwitchAuthentication.IsAuthenticated && TwitchTotem.TotemUnlockAvailable && !TwitchTotem.Deactivated;
    if (((UpgradeSystem.AbilityPoints <= 0 ? 0 : (GameManager.HasUnlockAvailable() ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      this.SecondaryLabel = "";
    else
      this.SecondaryLabel = ScriptLocalization.Interactions.AbilityScreen;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.IndicateHighlighted();
    if ((!TwitchAuthentication.IsAuthenticated || !TwitchTotem.TotemUnlockAvailable ? 0 : (!TwitchTotem.Deactivated ? 1 : 0)) != 0)
      this.GiveTwitchTotemReward();
    else if (UpgradeSystem.AbilityPoints > 0 && GameManager.HasUnlockAvailable())
    {
      AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_statue_open", this.gameObject);
      MonoSingleton<UIManager>.Instance.ShowUpgradeTree(new System.Action(this.UpdateBar));
    }
    else
      this.Activating = true;
    if (this.StructureBrain.SoulCount >= this.StructureBrain.SoulMax)
      this.wasFull = true;
    this.hasUnlockAvailable = GameManager.HasUnlockAvailable();
  }

  private void GiveTwitchTotemReward()
  {
    List<UITwitchTotemWheel.Segment> ts = new List<UITwitchTotemWheel.Segment>();
    if (DataManager.TwitchTotemRewardAvailable())
    {
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.2f,
        reward = InventoryItem.ITEM_TYPE.LOG
      });
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.2f,
        reward = InventoryItem.ITEM_TYPE.MEAT
      });
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.2f,
        reward = InventoryItem.ITEM_TYPE.FOLLOWERS
      });
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.2f,
        reward = InventoryItem.ITEM_TYPE.STONE
      });
      ts.Shuffle<UITwitchTotemWheel.Segment>();
      ts.Insert(0, new UITwitchTotemWheel.Segment()
      {
        probability = 0.05f,
        reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
      });
      ts.Insert(2, new UITwitchTotemWheel.Segment()
      {
        probability = 0.05f,
        reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
      });
      ts.Insert(4, new UITwitchTotemWheel.Segment()
      {
        probability = 0.05f,
        reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
      });
      ts.Insert(6, new UITwitchTotemWheel.Segment()
      {
        probability = 0.05f,
        reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION
      });
    }
    else
    {
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.25f,
        reward = InventoryItem.ITEM_TYPE.LOG
      });
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.25f,
        reward = InventoryItem.ITEM_TYPE.MEAT
      });
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.25f,
        reward = InventoryItem.ITEM_TYPE.FOLLOWERS
      });
      ts.Add(new UITwitchTotemWheel.Segment()
      {
        probability = 0.25f,
        reward = InventoryItem.ITEM_TYPE.STONE
      });
      ts.Shuffle<UITwitchTotemWheel.Segment>();
    }
    GameManager.GetInstance().OnConversationNew();
    UITwitchTotemWheel wheel = MonoSingleton<UIManager>.Instance.TwitchTotemWheelController.Instantiate<UITwitchTotemWheel>();
    wheel.Show(ts.ToArray());
    UITwitchTotemWheel twitchTotemWheel = wheel;
    twitchTotemWheel.OnHidden = twitchTotemWheel.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      switch (wheel.ChosenSegment.reward)
      {
        case InventoryItem.ITEM_TYPE.LOG:
          List<InventoryItem.ITEM_TYPE> itemTypeList1 = new List<InventoryItem.ITEM_TYPE>();
          int num1 = DataManager.Instance.BossesCompleted.Count + 1;
          int num2 = Mathf.Clamp(UnityEngine.Random.Range(15, 25) * num1, 1, 50);
          for (int index = 0; index < num2; ++index)
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LOG, 1, this.transform.position + Vector3.down).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
          break;
        case InventoryItem.ITEM_TYPE.STONE:
          List<InventoryItem.ITEM_TYPE> itemTypeList2 = new List<InventoryItem.ITEM_TYPE>();
          int num3 = DataManager.Instance.BossesCompleted.Count + 1;
          int num4 = Mathf.Clamp(UnityEngine.Random.Range(5, 15) * num3, 1, 50);
          for (int index = 0; index < num4; ++index)
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.STONE, 1, this.transform.position + Vector3.down).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
          break;
        case InventoryItem.ITEM_TYPE.MEAT:
          List<InventoryItem.ITEM_TYPE> itemTypeList3 = new List<InventoryItem.ITEM_TYPE>();
          int num5 = DataManager.Instance.BossesCompleted.Count + 1;
          int num6 = Mathf.Clamp(UnityEngine.Random.Range(15, 20) * num5, 1, 50);
          for (int index = 0; index < num6; ++index)
          {
            int num7 = UnityEngine.Random.Range(0, 100);
            if (num7 < 33)
            {
              if (UnityEngine.Random.Range(0, 100) < 33)
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.MEAT);
              else
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.MEAT_MORSEL);
            }
            else if (num7 < 66)
            {
              int num8 = UnityEngine.Random.Range(0, 100);
              if (num8 < 45)
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.FISH_SMALL);
              else if (num8 < 75)
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.FISH);
              else
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.FISH_BIG);
            }
            else
            {
              int num9 = UnityEngine.Random.Range(0, 100);
              if (DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4) && num9 < 25)
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.BEETROOT);
              else if (DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3) && num9 < 50)
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.CAULIFLOWER);
              else if (DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2) && num9 < 75)
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.PUMPKIN);
              else
                itemTypeList3.Add(InventoryItem.ITEM_TYPE.BERRY);
            }
          }
          using (List<InventoryItem.ITEM_TYPE>.Enumerator enumerator = itemTypeList3.GetEnumerator())
          {
            while (enumerator.MoveNext())
              InventoryItem.Spawn(enumerator.Current, 1, this.transform.position + Vector3.down).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
            break;
          }
        case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
          List<global::StructureBrain.TYPES> totemDecorations = DataManager.GetAvailableTwitchTotemDecorations();
          List<string> twitchTotemSkins = DataManager.GetAvailableTwitchTotemSkins();
          global::StructureBrain.TYPES types = global::StructureBrain.TYPES.NONE;
          string skinName = "";
          if (UnityEngine.Random.Range(0, totemDecorations.Count + twitchTotemSkins.Count) < twitchTotemSkins.Count)
            skinName = twitchTotemSkins[UnityEngine.Random.Range(0, twitchTotemSkins.Count)];
          else
            types = totemDecorations[UnityEngine.Random.Range(0, totemDecorations.Count)];
          UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
          if (types != global::StructureBrain.TYPES.NONE)
          {
            StructuresData.CompleteResearch(types);
            StructuresData.SetRevealed(types);
            overlayController.pickedBuilding = types;
            overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
            break;
          }
          if (string.IsNullOrEmpty(skinName))
            break;
          DataManager.SetFollowerSkinUnlocked(skinName);
          overlayController.pickedBuilding = types;
          overlayController.Show(UINewItemOverlayController.TypeOfCard.FollowerSkin, this.transform.position, skinName);
          break;
        case InventoryItem.ITEM_TYPE.FOLLOWERS:
          FollowerInfo f = FollowerInfo.NewCharacter(FollowerLocation.Base);
          DataManager.SetFollowerSkinUnlocked(f.SkinName);
          if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count > 0)
          {
            DataManager.Instance.Followers_Recruit.Add(f);
            break;
          }
          this.StartCoroutine((IEnumerator) this.GiveFollowerIE(f));
          break;
      }
    });
    TwitchTotem.TotemRewardClaimed();
    this.CheckXP();
  }

  private IEnumerator GiveFollowerIE(FollowerInfo f)
  {
    BiomeBaseManager.Instance.SpawnExistingRecruits = false;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew(true, true, true);
    GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    DataManager.Instance.Followers_Recruit.Add(f);
    FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
    UnityEngine.Object.FindObjectOfType<FollowerRecruit>().ManualTriggerAnimateIn();
    BiomeBaseManager.Instance.SpawnExistingRecruits = true;
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationNext(BiomeBaseManager.Instance.RecruitSpawnLocation, 8f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_statue_open", this.gameObject);
    MonoSingleton<UIManager>.Instance.ShowUpgradeTree();
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    if (!TwitchAuthentication.IsAuthenticated || TwitchTotem.TotemUnlockAvailable || TwitchTotem.Deactivated)
      return;
    MonoSingleton<Indicator>.Instance.ShowTopInfo($"{LocalizationManager.GetTranslation("UI/Twitch/Totem/Contributions")}: {Mathf.Clamp(TwitchTotem.Contributions, 0, 10)} / {10}".Colour(StaticColors.TwitchPurple));
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    MonoSingleton<Indicator>.Instance.HideTopInfo();
  }

  private IEnumerator EndUpgradeRoutine()
  {
    BuildingShrine buildingShrine = this;
    yield return (object) new WaitForSeconds(1.5f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    GameManager.GetInstance().OnConversationNext(buildingShrine.gameObject, 8f);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  private void OnSoulsGained(int count) => this.UpdateBar();

  private void UpdateBar()
  {
    float t = Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f);
    this.XPBar.UpdateBar(t);
    if (this.StructureBrain == null || UpgradeSystem.AbilityPoints != 0 && GameManager.HasUnlockAvailable())
      return;
    int id = Shader.PropertyToID("_Cutoff");
    double num1 = (double) this.ShrineGlow.sharedMaterial.GetFloat(id);
    this.ShrineGlow.material.DOKill();
    this.ShrineGlow.material.DOFloat(t - 0.1f, id, 1f);
    ParticleSystem.EmissionModule emission = this.psDevotion.emission;
    float num2 = Mathf.SmoothStep(0.0f, 0.5f, t);
    emission.rateOverTime = new ParticleSystem.MinMaxCurve(num2, num2);
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    if (this.Activating && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits))
    {
      this.Activating = false;
      this.ReduceDelay = 0.1f;
      if (this.wasFull && this.StructureBrain.SoulCount <= 0)
      {
        this.wasFull = false;
        foreach (Follower follower in Follower.Followers)
        {
          if (follower.Brain.Info.FollowerRole == FollowerRole.Worshipper && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep && follower.Brain.CurrentTaskType != FollowerTaskType.SleepBedRest)
            follower.Brain.CompleteCurrentTask();
        }
      }
    }
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    if (this.hasUnlockAvailable)
    {
      SoulCustomTarget.Create(this.state.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      double num = (double) Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f);
    }
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.StructureBrain.SoulCount;
    this.Delay = Mathf.Max(this.ReduceDelay -= Time.deltaTime * 0.05f, 0.005f);
    this.UpdateBar();
  }

  private void GivePlayerSoul() => PlayerFarming.Instance?.GetSoul(1);
}
