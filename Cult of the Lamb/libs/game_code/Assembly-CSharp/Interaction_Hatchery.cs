// Decompiled with JetBrains decompiler
// Type: Interaction_Hatchery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Hatchery : Interaction
{
  public static List<Interaction_Hatchery> Hatcheries = new List<Interaction_Hatchery>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject followerToSpawn;
  [SerializeField]
  public GameObject followerSpawnPosition;
  [SerializeField]
  public Interaction_EggFollower egg;
  [SerializeField]
  public GameObject normal;
  [SerializeField]
  public GameObject collapsed;
  [SerializeField]
  public GameObject waterIcon;
  [SerializeField]
  public GameObject normalOutlineTarget;
  [SerializeField]
  public GameObject collapsedOutlineTarget;
  [SerializeField]
  public GameObject tendPosition;
  public Structures_Hatchery structureBrain;
  public bool wasCollapsed = true;
  public string sHatch;
  public List<Follower> availableFollowers = new List<Follower>();
  public EventInstance loopy;

  public Structure Structure => this.structure;

  public GameObject TendPosition => this.tendPosition;

  public bool hasEgg => this.structureBrain.Data.HasEgg;

  public bool eggReady => this.structureBrain.Data.EggReady;

  public bool eggRequiresWatering
  {
    get => this.hasEgg && !this.eggReady && !this.structureBrain.Data.Watered;
  }

  public void Awake()
  {
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    this.UpdateLocalisation();
    Interaction_Hatchery.Hatcheries.Add(this);
    TimeManager.OnNewPhaseStarted += new System.Action(this.UpdateEgg);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.loopy);
    Interaction_Hatchery.Hatcheries.Remove(this);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.UpdateEgg);
    if (this.structureBrain != null)
      this.structureBrain.OnEggReady -= new System.Action(this.OnEggReady);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    if (this.egg.Spine.AnimationState == null)
      return;
    this.egg.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structureBrain = this.structure.Brain as Structures_Hatchery;
    this.structureBrain.OnEggReady += new System.Action(this.OnEggReady);
    this.egg.gameObject.SetActive(this.hasEgg);
    this.wasCollapsed = this.eggRequiresWatering;
    this.UpdateEgg();
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Audio/egg_bounce"))
      return;
    AudioManager.Instance.PlayOneShot("event:/material/egg_bounce", this.egg.gameObject);
  }

  public void OnStructureRemoved(StructuresData structure)
  {
    if (this.structureBrain == null || structure.ID != this.structureBrain.Data.ID || this.structureBrain.Data.EggInfo == null)
      return;
    this.SpawnEgg(this.transform.position);
  }

  public void OnEggReady()
  {
    this.UpdateEgg();
    AudioManager.Instance.PlayOneShot("event:/material/egg_crack", this.egg.gameObject);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sHatch = LocalizationManager.GetTranslation("Interactions/HatchEgg");
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.hasEgg)
    {
      if (this.eggReady)
      {
        this.Interactable = true;
        this.Label = this.sHatch;
      }
      else if (this.eggRequiresWatering)
      {
        this.Interactable = true;
        this.Label = LocalizationManager.GetTranslation("Interactions/Tend");
      }
      else
      {
        this.Interactable = false;
        this.Label = "";
      }
    }
    else
    {
      this.Interactable = false;
      this.Label = "";
    }
  }

  public void UpdateEgg()
  {
    this.egg.gameObject.SetActive(this.hasEgg);
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      if (this.hasEgg)
        this.egg.UpdateEgg(this.eggReady, this.Structure.Brain.Data.Rotten, this.Structure.Brain.Data.EggInfo.Rotting, this.Structure.Brain.Data.EggInfo.Golden);
      this.normal.gameObject.SetActive(!this.eggRequiresWatering);
      this.collapsed.gameObject.SetActive(this.eggRequiresWatering);
      this.waterIcon.gameObject.SetActive(true);
      if (this.structureBrain != null && this.structureBrain.Data != null && this.structureBrain.Data.EggInfo != null && this.structureBrain.Data.EggInfo.Special != FollowerSpecialType.None)
        this.egg.Spine.Skeleton.SetSkin($"Eggs/{this.structureBrain.Data.EggInfo.Special}");
      this.OutlineTarget = this.normal.gameObject.activeSelf ? this.normalOutlineTarget : this.collapsedOutlineTarget;
      if (this.wasCollapsed || !this.eggRequiresWatering)
        return;
      AudioManager.Instance.PlayOneShot("event:/material/nest_collapsing", this.gameObject);
      this.wasCollapsed = true;
    }));
    if (this.egg.Spine.AnimationState == null)
      return;
    this.egg.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    this.egg.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.eggReady)
    {
      this.StartCoroutine((IEnumerator) this.HatchIE());
    }
    else
    {
      if (!this.eggRequiresWatering)
        return;
      this.StartCoroutine((IEnumerator) this.WaterIE());
    }
  }

  public IEnumerator WaterIE()
  {
    Interaction_Hatchery interactionHatchery = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionHatchery.egg.gameObject);
    interactionHatchery.playerFarming.GoToAndStop(interactionHatchery.tendPosition.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/player/body_wrap", interactionHatchery.gameObject);
    interactionHatchery.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionHatchery.playerFarming.state.facingAngle = interactionHatchery.playerFarming.state.LookAngle = Utils.GetAngle(interactionHatchery.playerFarming.transform.position, interactionHatchery.egg.transform.position);
    interactionHatchery.playerFarming.CustomAnimation("Egg/egg-tending", true);
    interactionHatchery.egg.Spine.AnimationState.SetAnimation(0, interactionHatchery.structureBrain.Data.EggInfo.Golden ? "Egg/egg-tend-gold" : "Egg/egg-tend", false);
    interactionHatchery.egg.Spine.AnimationState.AddAnimation(0, "Egg/egg-idle", true, 0.0f);
    if (interactionHatchery.structureBrain.Data.EggInfo.Special != FollowerSpecialType.None)
      interactionHatchery.egg.Spine.Skeleton.SetSkin($"Eggs/{interactionHatchery.structureBrain.Data.EggInfo.Special}");
    yield return (object) new WaitForSeconds(1.8f);
    GameManager.GetInstance().OnConversationEnd();
    interactionHatchery.structureBrain.Data.WateredCount = 0;
    interactionHatchery.structureBrain.Data.Watered = true;
    interactionHatchery.wasCollapsed = false;
    interactionHatchery.UpdateEgg();
    BiomeConstants.Instance.EmitHeartPickUpVFX(interactionHatchery.egg.transform.position - Vector3.forward / 2f, 0.0f, "red", "burst_big");
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionHatchery.gameObject);
  }

  public IEnumerator HatchIE()
  {
    Interaction_Hatchery interactionHatchery = this;
    FollowerInfo info = interactionHatchery.GetChildFollower();
    ++DataManager.Instance.eggsHatched;
    interactionHatchery.loopy = AudioManager.Instance.CreateLoop("event:/Stings/baby_born1_loop", true, false);
    if (FollowerManager.PalworldIDs.Contains(info.ID))
    {
      int num = (int) interactionHatchery.loopy.setParameterByName("palworld", 1f);
    }
    FollowerRecruit recruit = (FollowerRecruit) null;
    Vector3 vector3 = interactionHatchery.transform.position + Vector3.left * 1.5f;
    int mask = LayerMask.GetMask("Island");
    if ((UnityEngine.Object) Physics2D.OverlapCircle((Vector2) vector3, 0.3f, mask) != (UnityEngine.Object) null)
      vector3 = interactionHatchery.transform.position + Vector3.left * 1f;
    interactionHatchery.playerFarming.GoToAndStop(vector3, interactionHatchery.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionHatchery.egg.gameObject, 6f);
    yield return (object) new WaitForSeconds(1f);
    if (info.ID == 100000)
    {
      interactionHatchery.StartCoroutine((IEnumerator) interactionHatchery.BringFollowersTogether());
      yield return (object) new WaitForSeconds(3f);
    }
    info.Outfit = FollowerOutfitType.Follower;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(interactionHatchery.followerToSpawn, interactionHatchery.followerSpawnPosition.transform.position, Quaternion.identity, BaseLocationManager.Instance.UnitLayer);
    BiomeBaseManager.Instance.SpawnExistingRecruits = true;
    DataManager.Instance.Followers_Recruit.Add(info);
    FollowerBrain brain1 = FollowerBrain.GetOrCreateBrain(info);
    brain1._directInfoAccess.TraitsSet = info.TraitsSet;
    brain1._directInfoAccess.Traits = info.Traits;
    if (brain1._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated))
      brain1._directInfoAccess.DayBecameMutated = TimeManager.CurrentDay + 5;
    gameObject.GetComponent<Follower>().Init(brain1, new FollowerOutfit(info));
    recruit = gameObject.GetComponent<FollowerRecruit>();
    recruit.Follower.State.LookAngle = recruit.Follower.State.facingAngle = 180f;
    recruit.Follower.LockToGround = false;
    recruit.MovePlayer = false;
    recruit.PlaySpawnAnim = false;
    recruit.PlayConvo = false;
    if (info.ID == 100000)
    {
      interactionHatchery.StartCoroutine((IEnumerator) interactionHatchery.ChosenChildHatchIE(recruit.Follower));
      recruit.Follower.Spine.AnimationState.SetAnimation(0, "Baby/baby-hatch-chosen" + (interactionHatchery.structureBrain.Data.EggInfo.Golden ? "-gold" : ""), false);
      recruit.Follower.Spine.AnimationState.AddAnimation(0, "Baby/baby-idle-sit", true, 0.0f);
      yield return (object) new WaitForEndOfFrame();
    }
    else if (info.ID == 100006)
    {
      brain1.AddThought(Thought.Midas_JustHatched);
    }
    else
    {
      if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      {
        if (interactionHatchery.structureBrain.Data.EggInfo.Golden)
          recruit.Follower.Spine.AnimationState.SetAnimation(0, "Baby/Baby-zombie/baby-hatch-gold-zombie", false);
        else
          recruit.Follower.Spine.AnimationState.SetAnimation(0, "Baby/Baby-zombie/baby-hatch-zombie", false);
        recruit.Follower.Spine.AnimationState.AddAnimation(0, "Baby/Baby-zombie/baby-idle-sit-zombie", true, 0.0f);
      }
      else
      {
        recruit.Follower.Spine.AnimationState.SetAnimation(0, "Baby/baby-hatch" + (interactionHatchery.structureBrain.Data.EggInfo.Golden ? "-gold" : ""), false);
        recruit.Follower.Spine.AnimationState.AddAnimation(0, "Baby/baby-idle-sit", true, 0.0f);
      }
      yield return (object) new WaitForEndOfFrame();
    }
    recruit.Follower.Brain._directInfoAccess.StartingCursedState = Thought.Child;
    recruit.Follower.Brain.Info.Outfit = FollowerOutfitType.None;
    recruit.Follower.Spine.transform.localScale = Vector3.one * 0.65f;
    if (info.ID == 100006)
    {
      recruit.Follower.Spine.transform.localScale = Vector3.one;
      recruit.Follower.Brain.Info.Special = FollowerSpecialType.Midas_Arm;
      recruit.Follower.transform.position = interactionHatchery.egg.transform.position;
      recruit.Follower.transform.localPosition = new Vector3(recruit.Follower.transform.localPosition.x, recruit.Follower.transform.localPosition.y, -0.2f);
      recruit.Follower.Brain.RemoveCurseState(Thought.OldAge);
      recruit.Follower.Brain.MakeChild();
      FollowerBrain.SetFollowerCostume(recruit.Follower.Spine.Skeleton, recruit.Follower.Brain._directInfoAccess, forceUpdate: true);
      recruit.Follower.Spine.AnimationState.SetAnimation(0, "Midas/hatch", false);
      recruit.Follower.Spine.AnimationState.AddAnimation(0, "Baby/baby-idle-sit", true, 0.0f);
    }
    else
    {
      recruit.Follower.Brain.RemoveCurseState(Thought.OldAge);
      recruit.Follower.Brain.MakeChild();
      FollowerBrain.SetFollowerCostume(recruit.Follower.Spine.Skeleton, recruit.Follower.Brain._directInfoAccess, forceUpdate: true);
    }
    recruit.Follower.Brain.AddThought((Thought) UnityEngine.Random.Range(368, 374));
    FollowerInfo infoById1 = FollowerInfo.GetInfoByID(interactionHatchery.structureBrain.Data.EggInfo.Parent_1_ID, true);
    if (infoById1 != null)
    {
      FollowerBrain brain2 = FollowerBrain.GetOrCreateBrain(infoById1);
      if (brain2 != null)
        interactionHatchery.DoParentCalculations(brain2);
    }
    FollowerInfo infoById2 = FollowerInfo.GetInfoByID(interactionHatchery.structureBrain.Data.EggInfo.Parent_2_ID, true);
    if (infoById2 != null)
    {
      FollowerBrain brain3 = FollowerBrain.GetOrCreateBrain(infoById2);
      if (brain3 != null)
        interactionHatchery.DoParentCalculations(brain3);
    }
    interactionHatchery.RemoveEgg();
    if (DataManager.Instance.eggsHatched >= 5)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("MATING_5"));
    if (info.ID == 100006)
    {
      yield return (object) new WaitForSeconds(3.86666656f);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(recruit.Follower.transform.position);
      FollowerBrain.SetFollowerCostume(recruit.Follower.Spine.Skeleton, recruit.Follower.Brain._directInfoAccess, forceUpdate: true);
    }
    interactionHatchery.normal.gameObject.SetActive(false);
    interactionHatchery.waterIcon.gameObject.SetActive(false);
    interactionHatchery.collapsed.gameObject.SetActive(true);
    interactionHatchery.OutlineTarget = interactionHatchery.normal.gameObject.activeSelf ? interactionHatchery.normalOutlineTarget : interactionHatchery.collapsedOutlineTarget;
    interactionHatchery.egg.gameObject.SetActive(false);
    if (info.ID == 100006)
      AudioManager.Instance.PlayOneShot("event:/dialogue/midas/laugh_baby", interactionHatchery.egg.transform.position);
    AudioManager.Instance.PlayOneShot("event:/material/egg_break", interactionHatchery.egg.gameObject);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", interactionHatchery.egg.gameObject);
    yield return (object) new WaitForSeconds(2.5f);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Fertility))
    {
      interactionHatchery.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      AudioManager.Instance.PlayOneShot("event:/player/collect_sin_from_fol", interactionHatchery.playerFarming.gameObject);
      interactionHatchery.playerFarming.simpleSpineAnimator.Animate("Sin/collect", 0, false);
      interactionHatchery.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      DataManager.Instance.PreviousSinPointFollowers.Add(recruit.Follower.Brain.Info.ID);
      recruit.Follower.PleasureUI.BarController.SetBarSize(0.0f, false, true);
      ++DataManager.Instance.pleasurePointsRedeemed;
      GameObject godTear = UnityEngine.Object.Instantiate<GameObject>(recruit.Follower.rewardPrefab, recruit.Follower.Spine.transform.position + new Vector3(0.0f, -0.1f, -1f), Quaternion.identity, interactionHatchery.transform.parent);
      godTear.transform.localScale = Vector3.zero;
      godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
      GameManager.GetInstance().OnConversationNext(godTear, 6f);
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
      yield return (object) new WaitForSeconds(1.1f);
      AudioManager.Instance.PlayOneShot("event:/Stings/baby_sins_snake_sting");
      yield return (object) new WaitForSeconds(0.4f);
      PlayerSimpleInventory simpleInventory = interactionHatchery.playerFarming.simpleInventory;
      godTear.transform.DOMove(new Vector3(simpleInventory.ItemImage.transform.position.x, simpleInventory.ItemImage.transform.position.y, -1f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      yield return (object) new WaitForSeconds(0.25f);
      recruit.Follower.Brain.Info.Pleasure = 0;
      Inventory.AddItem(154, 1);
      godTear.transform.DOScale(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject)));
      yield return (object) new WaitForSeconds(1.25f);
    }
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Allegiance))
      recruit.Follower.Brain.Info.XPLevel += UnityEngine.Random.Range(1, 3);
    if ((UnityEngine.Object) recruit != (UnityEngine.Object) null)
      recruit.OnInteract(interactionHatchery.playerFarming.state);
    recruit.OnCharacterSetupCallback += (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopy);
      if (FollowerManager.PalworldIDs.Contains(info.ID))
        AudioManager.Instance.PlayOneShotAndSetParameterValue("event:/Stings/baby_born2", "palworld", 1f);
      else
        AudioManager.Instance.PlayOneShot("event:/Stings/baby_born2");
      if (DataManager.Instance.eggsHatched > 1 && (double) UnityEngine.Random.value >= 0.30000001192092896)
        return;
      AudioManager.Instance.SetMusicParam(SoundParams.BabyFollowerAdded, 1f);
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.baby_fol_first_born);
    });
    yield return (object) new WaitForSeconds(0.2f);
    while (LetterBox.IsPlaying)
      yield return (object) new WaitForSeconds(0.2f);
    if ((UnityEngine.Object) recruit != (UnityEngine.Object) null)
      recruit.Follower.LockToGround = true;
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.winter_random);
    else
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
  }

  public void DoParentCalculations(FollowerBrain brain)
  {
    if ((double) UnityEngine.Random.value < 0.20000000298023224)
    {
      if ((double) UnityEngine.Random.value < 0.5)
      {
        brain.AddTrait(FollowerTrait.TraitType.OverworkedParent, true);
        brain.AddRandomThoughtFromList(Thought.OverwhelmedParent_0, Thought.OverwhelmedParent_1, Thought.OverwhelmedParent_2, Thought.OverwhelmedParent_3, Thought.OverwhelmedParent_4);
      }
      else
      {
        brain.AddTrait(FollowerTrait.TraitType.ProudParent, true);
        brain.AddRandomThoughtFromList(Thought.HadChild_1, Thought.HadChild_2, Thought.HadChild_3);
      }
    }
    else
      brain.AddRandomThoughtFromList(Thought.HadChild_1, Thought.HadChild_2, Thought.HadChild_3);
  }

  public void AddEgg(StructuresData eggData)
  {
    this.structureBrain.EggAdded();
    this.egg.gameObject.SetActive(true);
    this.structureBrain.Data.EggInfo = eggData.EggInfo;
    this.structureBrain.Data.Watered = false;
    this.structureBrain.Data.HasEgg = true;
    this.structureBrain.Data.EggReady = false;
    this.UpdateEgg();
  }

  public void RemoveEgg()
  {
    this.egg.gameObject.SetActive(false);
    this.structureBrain.Data.EggInfo = (StructuresData.EggData) null;
    this.structureBrain.Data.Watered = false;
    this.structureBrain.Data.HasEgg = false;
    this.structureBrain.Data.EggReady = false;
  }

  public FollowerInfo GetChildFollower()
  {
    StructuresData.EggData eggInfo = this.structureBrain.Data.EggInfo;
    System.Random random = new System.Random(eggInfo.EggSeed);
    if (FollowerManager.PalworldIDs.Contains(eggInfo.Parent_1_ID) && !DataManager.Instance.FollowerSkinsUnlocked.Contains(eggInfo.Parent_1_SkinName) || eggInfo.Parent_1_ID == 100006 && !DataManager.Instance.CompletedMidasFollowerQuest || eggInfo.Special == FollowerSpecialType.Gold && eggInfo.Parent_1_ID != 0 && eggInfo.Parent_2_ID == 0)
    {
      string str = "PalworldOne";
      FollowerTrait.TraitType traitType = FollowerTrait.TraitType.Scared;
      switch (eggInfo.Parent_1_ID)
      {
        case 100001:
          traitType = FollowerTrait.TraitType.Insomniac;
          str = "PalworldTwo";
          break;
        case 100002:
          traitType = FollowerTrait.TraitType.Faithful;
          str = "PalworldFive";
          break;
        case 100003:
          traitType = FollowerTrait.TraitType.Lazy;
          str = "PalworldThree";
          break;
        case 100004:
          traitType = FollowerTrait.TraitType.Industrious;
          str = "PalworldFour";
          break;
        case 100006:
          traitType = FollowerTrait.TraitType.Bastard;
          str = "Midas";
          break;
        default:
          if (eggInfo.Special == FollowerSpecialType.Gold && eggInfo.Parent_1_ID != 0 && eggInfo.Parent_2_ID == 0)
          {
            eggInfo.Parent_1_ID = ++DataManager.Instance.FollowerID;
            while (FollowerManager.UniqueFollowerIDs.Contains(eggInfo.Parent_1_ID))
              eggInfo.Parent_1_ID = ++DataManager.Instance.FollowerID;
            str = "Dragon";
            Interaction_DragonEgg.DragonSkins.Shuffle<string>();
            for (int index = 0; index < Interaction_DragonEgg.DragonSkins.Count; ++index)
            {
              if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(Interaction_DragonEgg.DragonSkins[index]))
                str = Interaction_DragonEgg.DragonSkins[index];
            }
            traitType = FollowerTrait.GetRareTrait();
            if (traitType == FollowerTrait.TraitType.None)
              traitType = FollowerTrait.GetStartingTrait();
            eggInfo.Parent1Name = "";
            break;
          }
          break;
      }
      DataManager.SetFollowerSkinUnlocked(str);
      FollowerInfo childFollower = FollowerInfo.NewCharacter(FollowerLocation.Base, str);
      childFollower.Name = eggInfo.Parent1Name;
      childFollower.ID = eggInfo.Parent_1_ID;
      childFollower.Traits.Clear();
      childFollower.Traits.Add(traitType);
      childFollower.BornInCult = true;
      childFollower.TraitsSet = true;
      childFollower.Special = FollowerSpecialType.None;
      if (eggInfo.Parent_1_ID == 100006)
        childFollower.Traits.Add(FollowerTrait.TraitType.RoyalPooper);
      return childFollower;
    }
    double num1 = random.NextDouble();
    string str1 = "";
    string str2 = eggInfo.Parent_1_ID == 100006 ? "Starfish" : eggInfo.Parent_1_SkinName;
    string str3 = eggInfo.Parent_2_ID == 100006 ? "Starfish" : eggInfo.Parent_2_SkinName;
    if (!str2.Contains("Snowman") && str3.Contains("Snowman"))
      str3 = str2;
    else if (str2.Contains("Snowman") && !str3.Contains("Snowman"))
      str2 = str3;
    string str4 = num1 >= 0.5 ? str2 : str3;
    string skinName1 = num1 >= 0.5 ? eggInfo.Parent_2_SkinName : eggInfo.Parent_1_SkinName;
    int fromColorSlot = num1 >= 0.5 ? eggInfo.Parent_2_SkinColor : eggInfo.Parent_1_SkinColor;
    int toSkinCharacter = this.GetSkinIndex(str4);
    int skinIndex = this.GetSkinIndex(skinName1);
    int num2 = num1 >= 0.5 ? eggInfo.Parent_2_SkinVariant : eggInfo.Parent_1_SkinVariant;
    bool flag1 = FollowerManager.UniqueFollowerIDs.Contains(eggInfo.Parent_1_ID);
    if (flag1 && (FollowerManager.PilrgrimIDs.Contains(eggInfo.Parent_1_ID) || FollowerManager.PalworldIDs.Contains(eggInfo.Parent_1_ID) || eggInfo.Parent_1_ID == 100006) || eggInfo.Parent_1_ID == 10009)
      flag1 = false;
    bool flag2 = FollowerManager.UniqueFollowerIDs.Contains(eggInfo.Parent_2_ID);
    if (flag2 && (FollowerManager.PilrgrimIDs.Contains(eggInfo.Parent_2_ID) || FollowerManager.PalworldIDs.Contains(eggInfo.Parent_2_ID) || eggInfo.Parent_2_ID == 100006) || eggInfo.Parent_2_ID == 10009)
      flag2 = false;
    if (flag1 | flag2 || DataManager.Instance.ForceAbomination)
    {
      toSkinCharacter = WorshipperData.Instance.GetSkinIndexFromName("Abomination");
      num2 = UnityEngine.Random.Range(0, 2);
      str4 = "Abomination";
      DataManager.Instance.ForceAbomination = false;
    }
    int num3 = -1;
    if (eggInfo.Golden)
    {
      List<string> possibleGoldenEggSkins = Structures_MatingTent.GetPossibleGoldenEggSkins();
      if (possibleGoldenEggSkins.Count > 0)
      {
        string skinName2 = possibleGoldenEggSkins[0];
        DataManager.SetFollowerSkinUnlocked(skinName2);
        toSkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(skinName2);
        str4 = skinName2;
        num2 = UnityEngine.Random.Range(0, 2);
      }
    }
    else if (DataManager.Instance.PalworldSkins.Contains<string>(eggInfo.Parent_1_SkinName) && DataManager.Instance.PalworldSkins.Contains<string>(eggInfo.Parent_2_SkinName) && !DataManager.Instance.FollowerSkinsUnlocked.Contains("PalworldTwo") && eggInfo.Parent_1_SkinName != eggInfo.Parent_2_SkinName)
    {
      toSkinCharacter = WorshipperData.Instance.GetSkinIndexFromName("PalworldTwo");
      num2 = 0;
      str4 = "PalworldTwo";
      str1 = LocalizationManager.GetTranslation("NAMES/Daedream");
      num3 = 0;
      DataManager.SetFollowerSkinUnlocked(str4);
    }
    FollowerInfo childFollower1 = FollowerInfo.NewCharacter(FollowerLocation.Base, str4);
    if (eggInfo.Traits != null && eggInfo.Traits.Contains(FollowerTrait.TraitType.ChosenOne))
    {
      toSkinCharacter = WorshipperData.Instance.GetSkinIndexFromName("ChosenChild");
      str4 = "ChosenChild";
      childFollower1.ID = 100000;
    }
    int num4 = num3 == -1 ? WorshipperData.GetClosestColorSlotFromColor(skinIndex, fromColorSlot, toSkinCharacter) : num3;
    if (str4 == "ChosenChild")
      num4 = 0;
    childFollower1.SkinName = str4;
    childFollower1.SkinCharacter = toSkinCharacter;
    childFollower1.SkinVariation = Mathf.Clamp(num2, 0, WorshipperData.Instance.Characters[childFollower1.SkinCharacter].Skin.Count - 1);
    childFollower1.SkinName = WorshipperData.Instance.Characters[childFollower1.SkinCharacter].Skin[childFollower1.SkinVariation].Skin;
    childFollower1.SkinColour = num4;
    childFollower1.Traits = eggInfo.Traits == null ? new List<FollowerTrait.TraitType>() : eggInfo.Traits;
    childFollower1.TraitsSet = true;
    childFollower1.BornInCult = true;
    childFollower1.RottingUnique = eggInfo.RottingUnique;
    childFollower1.IsSnowman = str4.Contains("Snowman");
    childFollower1.Name = str1;
    if (childFollower1.IsSnowman)
    {
      childFollower1.FollowerRole = FollowerRole.Worshipper;
      childFollower1.Special = FollowerSpecialType.Snowman_Great;
    }
    string str5 = this.structure.Brain.Data.EggInfo.Parent1Name;
    string str6 = this.structure.Brain.Data.EggInfo.Parent2Name;
    if (str5 == null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this.Structure.Brain.Data.EggInfo.Parent_1_ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && followerById.Brain != null)
        str5 = followerById.Brain.Info.Name;
    }
    if (str6 == null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this.Structure.Brain.Data.EggInfo.Parent_2_ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && followerById.Brain != null)
        str6 = followerById.Brain.Info.Name;
    }
    childFollower1.Parent1Name = str5;
    childFollower1.Parent2Name = str6;
    childFollower1.Parents.Add(this.Structure.Brain.Data.EggInfo.Parent_1_ID);
    childFollower1.Parents.Add(this.Structure.Brain.Data.EggInfo.Parent_2_ID);
    return childFollower1;
  }

  public int GetSkinIndex(string skinName)
  {
    return !skinName.Contains("Boss") && !skinName.Contains("CultLeader") && !skinName.Contains("Snowman") ? WorshipperData.Instance.GetSkinIndexFromName(skinName.StripNumbers()) : WorshipperData.Instance.GetSkinIndexFromName(skinName);
  }

  public void SpawnEgg(Vector3 spawnPos)
  {
    StructuresData egg = StructuresData.GetInfoByType(StructureBrain.TYPES.EGG_FOLLOWER, 0);
    egg.EggInfo = this.structureBrain.Data.EggInfo;
    StructureManager.BuildStructure(FollowerLocation.Base, egg, spawnPos, Vector2Int.one, false, (System.Action<GameObject>) (obj => obj.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, egg.EggInfo.Rotting, egg.EggInfo.Golden, egg.EggInfo.Special)));
  }

  public IEnumerator ChosenChildHatchIE(Follower child)
  {
    Interaction_Hatchery interactionHatchery = this;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 1.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 10f;
    GameManager.GetInstance().OnConversationNext(interactionHatchery.transform.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.35f);
    for (int index = 0; index < interactionHatchery.availableFollowers.Count; ++index)
    {
      double num = (double) interactionHatchery.availableFollowers[index].SetBodyAnimation("devotion/devotion-collect-loopstart-whiteyes", true);
      interactionHatchery.availableFollowers[index].AddBodyAnimation("devotion/devotion-collect-loop-whiteyes", false, 0.0f);
    }
    yield return (object) new WaitForSeconds(2f);
    for (int index = 0; index < interactionHatchery.availableFollowers.Count; ++index)
      interactionHatchery.availableFollowers[index].Brain.CompleteCurrentTask();
  }

  public IEnumerator BringFollowersTogether()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    Interaction_Hatchery interactionHatchery1 = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    foreach (Follower follower in Follower.Followers)
    {
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain != null && follower.Brain.Stats != null && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && follower.Brain.Info.CursedState == Thought.None && (double) follower.Brain.Stats.Exhaustion <= 0.0 && (double) Vector3.Distance(interactionHatchery1.transform.position, follower.transform.position) < 10.0)
        interactionHatchery1.availableFollowers.Add(follower);
    }
    if (interactionHatchery1.availableFollowers.Count <= 0)
      return false;
    for (int index = 0; index < interactionHatchery1.availableFollowers.Count; ++index)
    {
      Interaction_Hatchery interactionHatchery = interactionHatchery1;
      Follower f = interactionHatchery1.availableFollowers[index];
      f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      f.GoTo(interactionHatchery1.GetCirclePosition(interactionHatchery1.availableFollowers, interactionHatchery1.availableFollowers[index]), (System.Action) (() =>
      {
        f.FacePosition(interactionHatchery.transform.position);
        f.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num2 = (double) f.SetBodyAnimation("pray", true);
      }));
      double num3 = (double) f.SetBodyAnimation("run", true);
      f.SpeedMultiplier = 1.5f;
    }
    return false;
  }

  public Vector3 GetCirclePosition(List<Follower> availableFollowers, Follower follower)
  {
    int num1 = availableFollowers.IndexOf(follower);
    if (availableFollowers.Count <= 12)
    {
      float num2 = 2f;
      float f = (float) ((double) num1 * (360.0 / (double) availableFollowers.Count) * (Math.PI / 180.0));
      return this.transform.position + new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
    }
    int b = 8;
    float num3;
    float f1;
    if (num1 < b)
    {
      num3 = 2f;
      f1 = (float) ((double) num1 * (360.0 / (double) Mathf.Min(availableFollowers.Count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num3 = 3f;
      f1 = (float) ((double) (num1 - b) * (360.0 / (double) (availableFollowers.Count - b)) * (Math.PI / 180.0));
    }
    return this.transform.position + new Vector3(num3 * Mathf.Cos(f1), num3 * Mathf.Sin(f1));
  }

  [CompilerGenerated]
  public void \u003CUpdateEgg\u003Eb__33_0()
  {
    if (this.hasEgg)
      this.egg.UpdateEgg(this.eggReady, this.Structure.Brain.Data.Rotten, this.Structure.Brain.Data.EggInfo.Rotting, this.Structure.Brain.Data.EggInfo.Golden);
    this.normal.gameObject.SetActive(!this.eggRequiresWatering);
    this.collapsed.gameObject.SetActive(this.eggRequiresWatering);
    this.waterIcon.gameObject.SetActive(true);
    if (this.structureBrain != null && this.structureBrain.Data != null && this.structureBrain.Data.EggInfo != null && this.structureBrain.Data.EggInfo.Special != FollowerSpecialType.None)
      this.egg.Spine.Skeleton.SetSkin($"Eggs/{this.structureBrain.Data.EggInfo.Special}");
    this.OutlineTarget = this.normal.gameObject.activeSelf ? this.normalOutlineTarget : this.collapsedOutlineTarget;
    if (this.wasCollapsed || !this.eggRequiresWatering)
      return;
    AudioManager.Instance.PlayOneShot("event:/material/nest_collapsing", this.gameObject);
    this.wasCollapsed = true;
  }
}
