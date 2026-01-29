// Decompiled with JetBrains decompiler
// Type: FollowerRecruit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerRecruit : Interaction
{
  public GameObject Menu;
  public GameObject MenuRecruit;
  public Follower Follower;
  public GameObject CameraBone;
  public AudioClip SacrificeSting;
  public interaction_FollowerInteraction FollowerInteraction;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  public System.Action StatueCallback;
  public bool triggered;
  public const float triggerDistance = 10f;
  public static FollowerRecruit.FollowerEventDelegate OnFollowerRecruited;
  public static FollowerRecruit.FollowerEventDelegate OnRecruitFinalised;
  public SkeletonAnimationLODManager skeletonAnimationLODManager;
  public System.Action OnCharacterSetupCallback;
  [CompilerGenerated]
  public bool \u003CPlaySpawnAnim\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CPlayConvo\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CMovePlayer\u003Ek__BackingField = true;
  public bool spawnedEgg;
  public string dString;
  public bool Activating;
  public GameObject FollowerRoleMenu;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public List<MeshRenderer> FollowersTurnedOff = new List<MeshRenderer>();
  public static System.Action OnNewRecruit;
  public bool RecruitOnComplete = true;

  public bool PlaySpawnAnim
  {
    get => this.\u003CPlaySpawnAnim\u003Ek__BackingField;
    set => this.\u003CPlaySpawnAnim\u003Ek__BackingField = value;
  }

  public bool PlayConvo
  {
    get => this.\u003CPlayConvo\u003Ek__BackingField;
    set => this.\u003CPlayConvo\u003Ek__BackingField = value;
  }

  public bool MovePlayer
  {
    get => this.\u003CMovePlayer\u003Ek__BackingField;
    set => this.\u003CMovePlayer\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.IgnoreTutorial = true;
    this.Follower.State.CURRENT_STATE = StateMachine.State.Idle;
    this.ActivateDistance = 2f;
    this.Interactable = false;
    this.SecondaryInteractable = false;
    FollowerOutfitType followerOutfitType = FollowerOutfitType.Rags;
    if (followerOutfitType != FollowerOutfitType.Old && this.Follower.Brain.Info.Special != FollowerSpecialType.SozoOld)
    {
      FollowerOutfitType outfitFromCursedState = FollowerBrain.GetOutfitFromCursedState(this.Follower.Brain._directInfoAccess);
      if (outfitFromCursedState != FollowerOutfitType.None)
        followerOutfitType = outfitFromCursedState;
    }
    this.Follower.Brain.Info.Outfit = followerOutfitType;
    FollowerBrain.SetFollowerCostume(this.Follower.Spine.skeleton, this.Follower.Brain._directInfoAccess, forceUpdate: true);
    this.Follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.FollowerEvent);
    this.GetComponentInChildren<UIFollowerName>()?.Hide(false);
    if (!this.TryGetComponent<SkeletonAnimationLODManager>(out this.skeletonAnimationLODManager))
      return;
    this.skeletonAnimationLODManager.DisableLODManager(true);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    DataManager.Instance.followerRecruitWaiting = false;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.triggered && !this.Interactable)
      this.StartCoroutine((IEnumerator) this.DelayedInteractable());
    DataManager.Instance.followerRecruitWaiting = true;
  }

  public void SpawnAnim(bool pause = false)
  {
    if (!this.PlaySpawnAnim)
      return;
    this.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (FollowerManager.PalworldIDs.Contains(this.Follower.Brain.Info.ID) || this.Follower.Brain._directInfoAccess.FromDragonNPC || this.Follower.Brain.Info.ID == 100006 && !DataManager.Instance.CompletedMidasFollowerQuest)
    {
      this.Activating = true;
      double num = (double) this.Follower.SetBodyAnimation("Egg/spawn-in-base", false);
      this.Follower.AddBodyAnimation("Egg/egg-idle", true, 0.0f);
      this.Follower.Spine.Skeleton.SetSkin($"Eggs/{this.Follower.Brain.Info.Special}");
      Vector3 endValue = (Vector3) (UnityEngine.Random.insideUnitCircle.normalized * 2f);
      this.Follower.transform.position = BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position;
      this.Follower.Spine.transform.DOKill();
      this.Follower.Spine.transform.localPosition = Vector3.zero;
      this.Follower.Spine.transform.DOLocalMove(endValue, 1f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.75f);
      GameManager.GetInstance().WaitForSeconds(2.63333344f, (System.Action) (() =>
      {
        if (this.spawnedEgg)
          return;
        this.spawnedEgg = true;
        StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.EGG_FOLLOWER, 0);
        infoByType.EggInfo = new StructuresData.EggData()
        {
          Parent1Name = this.Follower.Brain.Info.Name,
          Parent_1_ID = this.Follower.Brain.Info.ID
        };
        infoByType.CanBecomeRotten = false;
        if (this.Follower.Brain._directInfoAccess.FromDragonNPC)
          infoByType.EggInfo.Parent_1_ID = -666;
        this.Follower.Brain._directInfoAccess.FromDragonNPC = false;
        StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.transform.position + this.Follower.Spine.transform.localPosition, Vector2Int.one, false, (System.Action<GameObject>) (obj =>
        {
          obj.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, false, false, this.Follower.Brain.Info.Special);
          PickUp component = obj.GetComponent<PickUp>();
          component.Bounce = false;
          component.ScaleIn = false;
          UnityEngine.Object.Destroy((UnityEngine.Object) this.Follower.gameObject);
          FollowerManager.RemoveFollower(this.Follower.Brain.Info.ID);
          FollowerManager.RemoveFollowerBrain(this.Follower.Brain.Info.ID);
          DataManager.Instance.Followers_Recruit.RemoveAt(0);
          BiomeBaseManager.Instance.SpawnExistingRecruits = false;
        }), emitParticles: false);
      }));
    }
    else
    {
      double num = (double) this.Follower.SetBodyAnimation("spawn-in-base", false);
      this.Follower.AddBodyAnimation("pray", true, 0.0f);
      if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.OldAge)
        this.Follower.SetOutfit(FollowerOutfitType.Old, false);
      else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.Ill)
        this.Follower.SetFaceAnimation("Emotions/emotion-sick", true);
      else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.BecomeStarving)
        this.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
      else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.Freezing)
        this.Follower.SetFaceAnimation("Emotions/emotion-freezing", true);
      else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.Dissenter)
      {
        this.Follower.SetFaceAnimation("Emotions/emotion-dissenter", true);
        this.Follower.SetFaceAnimation("Emotions/emotion-dissenter", true);
        this.Follower.SetOutfit(FollowerOutfitType.Rags, false, this.Follower.Brain._directInfoAccess.StartingCursedState);
      }
    }
    if (pause)
    {
      this.Follower.Spine.AnimationState.TimeScale = 0.0f;
    }
    else
    {
      this.portalSpine.gameObject.SetActive(true);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ShowPortal());
      AudioManager.Instance.PlayOneShot("event:/followers/teleport_to_base", this.Follower.gameObject);
      this.Follower.Spine.AnimationState.TimeScale = 1f;
    }
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator ShowPortal()
  {
    while (this.portalSpine.AnimationState == null)
      yield return (object) null;
    this.portalSpine.AnimationState.SetAnimation(0, "spawn-in-base", false);
  }

  public IEnumerator DelayedInteractable()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerRecruit followerRecruit = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      followerRecruit.Interactable = true;
      followerRecruit.SecondaryInteractable = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.dString = ScriptLocalization.Interactions.Indoctrinate;
  }

  public void ManualTriggerAnimateIn()
  {
    this.Follower.Spine.gameObject.SetActive(true);
    this.SpawnAnim();
    this.StartCoroutine((IEnumerator) this.DelayedInteractable());
    this.triggered = true;
  }

  public override void Update()
  {
    base.Update();
    if (this.triggered || !(bool) (UnityEngine.Object) PlayerFarming.Instance || (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) >= 10.0 || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive)
      return;
    this.Follower.Spine.gameObject.SetActive(true);
    this.SpawnAnim();
    this.StartCoroutine((IEnumerator) this.DelayedInteractable());
    this.triggered = true;
  }

  public override void GetLabel()
  {
    this.Label = !this.Interactable || this.Activating || !this.triggered ? "" : this.dString;
  }

  public override void GetSecondaryLabel() => this.SecondaryLabel = "";

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activating = true;
    PlayerFarming.Instance.unitObject.speed = 0.0f;
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      this.DoRecruit(state, true);
      BiomeConstants.Instance.DepthOfFieldTween(1.5f, 4.5f, 10f, 1f, 145f);
      SimulationManager.Pause();
    })));
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.Activating = true;
    PlayerFarming.Instance.GoToAndStop(this.transform.position + ((double) state.transform.position.x < (double) this.transform.position.x ? new Vector3(-1.5f, -0.5f) : new Vector3(1.5f, -0.5f)), this.gameObject, GoToCallback: (System.Action) (() => this.DoRecruit(state, false)));
  }

  public void CallbackClose() => this.state.CURRENT_STATE = StateMachine.State.Idle;

  public void DoRecruit(StateMachine state, bool customise, bool newRecruit = true)
  {
    this.state = state;
    GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIRST_FOLLOWER"));
    if (DataManager.Instance.Followers.Count >= 4)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("GAIN_FIVE_FOLLOWERS"));
    if (DataManager.Instance.Followers.Count >= 9)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("TEN_FOLLOWERS"));
    if (DataManager.Instance.Followers.Count >= 19)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("TWENTY_FOLLOWERS"));
    if (newRecruit)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != this.Follower.Brain && allBrain.Location == this.Follower.Brain.Location)
          allBrain.AddThought(Thought.CultHasNewRecruit);
      }
    }
    DataManager.Instance.GameOverEnabled = true;
    if (DataManager.Instance.InGameOver)
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GameOver);
      DataManager.Instance.InGameOver = false;
      DataManager.Instance.DisplayGameOverWarning = false;
      DataManager.Instance.GameOver = false;
    }
    FollowerRecruit.FollowerEventDelegate followerRecruited = FollowerRecruit.OnFollowerRecruited;
    if (followerRecruited != null)
      followerRecruited(this.Follower.Brain._directInfoAccess);
    this.CompleteCallBack(FollowerRole.Worker, customise);
  }

  public void CompleteCallBack(FollowerRole FollowerRole, bool customise)
  {
    if (this.MovePlayer)
      PlayerFarming.Instance.GoToAndStop(this.transform.position + new Vector3(-1.5f, -0.1f), this.gameObject, GoToCallback: (System.Action) (() => PlayerFarming.Instance.transform.position = this.transform.position + new Vector3(-1.5f, -0.1f)));
    this.Follower.Brain.Info.FollowerRole = FollowerRole;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    List<string> stringList = new List<string>()
    {
      "Conversation_NPC/FollowerSpawn/Line1",
      "Conversation_NPC/FollowerSpawn/Line2",
      "Conversation_NPC/FollowerSpawn/Line3",
      "Conversation_NPC/FollowerSpawn/Line4",
      "Conversation_NPC/FollowerSpawn/Line5"
    };
    if (this.Follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
    {
      stringList.Clear();
      stringList.Add("Conversation_NPC/RottenFollowerSpawn/Line1");
      stringList.Add("Conversation_NPC/RottenFollowerSpawn/Line2");
    }
    ConversationEntry conversationEntry1 = new ConversationEntry(this.gameObject, stringList[UnityEngine.Random.Range(0, stringList.Count)]);
    conversationEntry1.soundPath = "event:/dialogue/followers/general_talk";
    conversationEntry1.pitchValue = this.Follower.Brain._directInfoAccess.follower_pitch;
    conversationEntry1.vibratoValue = this.Follower.Brain._directInfoAccess.follower_vibrato;
    conversationEntry1.followerID = this.Follower.Brain.Info.ID;
    conversationEntry1.SetZoom = true;
    conversationEntry1.Zoom = 4f;
    conversationEntry1.Offset = new Vector3(0.0f, -0.5f, 0.0f);
    conversationEntry1.CharacterName = $"<color=yellow>{this.Follower.Brain.Info.Name}</color>";
    Entries.Add(conversationEntry1);
    if (this.Follower.Brain.Info.ID == 666 && !DataManager.Instance.LeaderFollowersRecruited.Contains(this.Follower.Brain.Info.ID))
      DataManager.Instance.LeaderFollowersRecruited.Add(this.Follower.Brain.Info.ID);
    bool flag1 = false;
    bool flag2 = false;
    if (this.Follower.Brain.Info.ID == 99990 || this.Follower.Brain.Info.ID == 99991 || this.Follower.Brain.Info.ID == 99992 || this.Follower.Brain.Info.ID == 99993 || this.Follower.Brain.Info.ID == 99998 || this.Follower.Brain.Info.ID == 99997 || this.Follower.Brain.Info.ID == 10009 || this.Follower.Brain.Info.ID == 99999 || this.Follower.Brain.Info.ID == 10014 || this.Follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) && !LoreSystem.LoreAvailable(16 /*0x10*/))
      flag1 = true;
    if (this.Follower.Brain.Info.ID == 99990 || this.Follower.Brain.Info.ID == 99991 || this.Follower.Brain.Info.ID == 99992 || this.Follower.Brain.Info.ID == 99993 || this.Follower.Brain.Info.ID == 10015 || this.Follower.Brain.Info.ID == 10016 || this.Follower.Brain.Info.ID == 10014)
      flag2 = true;
    if (flag1 | flag2)
    {
      if (FollowerManager.BishopIDs.Contains(this.Follower.Brain.Info.ID) && !DataManager.Instance.LeaderFollowersRecruited.Contains(this.Follower.Brain.Info.ID))
      {
        DataManager.Instance.LeaderFollowersRecruited.Add(this.Follower.Brain.Info.ID);
        if (this.Follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Poet))
          this.Follower.Brain.RemoveTrait(FollowerTrait.TraitType.Poet);
      }
      else if (!FollowerManager.BishopIDs.Contains(this.Follower.Brain.Info.ID) && !DataManager.Instance.UniqueFollowersRecruited.Contains(this.Follower.Brain.Info.ID))
        DataManager.Instance.UniqueFollowersRecruited.Add(this.Follower.Brain.Info.ID);
      Entries.Clear();
      string str1 = "";
      string str2 = "";
      if (this.Follower.Brain.Info.ID == 99990)
      {
        str1 = "Conversation_NPC/FollowerSpawn/Leshy/0";
        str2 = "event:/dialogue/followers/boss/fol_leshy";
      }
      else if (this.Follower.Brain.Info.ID == 99991)
      {
        str1 = "Conversation_NPC/FollowerSpawn/Heket/0";
        str2 = "event:/dialogue/followers/boss/fol_heket";
      }
      else if (this.Follower.Brain.Info.ID == 99992)
      {
        str1 = "Conversation_NPC/FollowerSpawn/Kallamar/0";
        str2 = "event:/dialogue/followers/boss/fol_kallamar";
      }
      else if (this.Follower.Brain.Info.ID == 99993)
      {
        str1 = "Conversation_NPC/FollowerSpawn/Shamura/0";
        str2 = "event:/dialogue/followers/boss/fol_shamura";
      }
      else if (this.Follower.Brain.Info.ID == DataManager.Instance.ExecutionerFollowerNoteGiverID)
        str1 = "Conversation_NPC/FollowerRecruited/Executioner/0";
      else if (this.Follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) && !LoreSystem.LoreAvailable(16 /*0x10*/))
        str1 = "Conversation_NPC/Follower/FirstRotten/0";
      else if (this.Follower.Brain.Info.ID == 10009)
        str1 = "Conversation_NPC/FollowerSpawn/Narayana/0";
      else if (this.Follower.Brain.Info.ID == 99998)
        str1 = "Conversation_NPC/FollowerSpawn/Yarlen/0";
      else if (this.Follower.Brain.Info.ID == 99999)
        str1 = "Conversation_NPC/FollowerSpawn/Rinor/0";
      else if (this.Follower.Brain.Info.ID == 99997)
        str1 = "Conversation_NPC/FollowerSpawn/Jalala/0";
      else if (this.Follower.Brain.Info.ID == 10014)
      {
        str1 = "Conversation_NPC/WarriorTrio/Indoctrinate/Mestor/0";
        str2 = "event:/dlc/dialogue/miniboss_wolf_guardian_trio/sad";
      }
      else if (this.Follower.Brain.Info.ID == 10015)
      {
        str1 = stringList[UnityEngine.Random.Range(0, stringList.Count)];
        str2 = "event:/dlc/dialogue/miniboss_wolf_guardian_trio/sad";
      }
      else if (this.Follower.Brain.Info.ID == 10016)
      {
        str1 = stringList[UnityEngine.Random.Range(0, stringList.Count)];
        str2 = "event:/dlc/dialogue/miniboss_wolf_guardian_trio/sad";
      }
      ConversationEntry.Clone(conversationEntry1);
      conversationEntry1.TermToSpeak = str1;
      conversationEntry1.followerID = this.Follower.Brain.Info.ID;
      if (flag2 && !string.IsNullOrEmpty(str2))
        conversationEntry1.soundPath = str2;
      Entries.Add(conversationEntry1);
      if (flag1)
      {
        int num = 0;
        while (LocalizationManager.GetTermData(ConversationEntry.Clone(conversationEntry1).TermToSpeak.Replace("0", (++num).ToString())) != null)
        {
          ConversationEntry conversationEntry2 = ConversationEntry.Clone(conversationEntry1);
          conversationEntry2.TermToSpeak = conversationEntry2.TermToSpeak.Replace("0", num.ToString());
          Entries.Add(conversationEntry2);
        }
      }
    }
    GameManager.GetInstance().CamFollowTarget.ZoomSpeedConversation = 1f;
    GameManager.GetInstance().CamFollowTarget.MaxZoomInConversation = 100f;
    if (this.PlayConvo)
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine(true)))), false);
    else
      this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine(true));
  }

  public IEnumerator SimpleNewRecruitRoutine(bool customise)
  {
    FollowerRecruit followerRecruit = this;
    if (!LoreSystem.LoreAvailable(16 /*0x10*/) && followerRecruit.Follower.Brain.Info.SkinName.Contains("Boss Rot"))
    {
      LoreStone component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LORE_STONE, 1, PlayerFarming.Instance.transform.position).GetComponent<LoreStone>();
      component.SetLore(16 /*0x10*/, true);
      component.OnInteract(PlayerFarming.Instance.state);
      BiomeBaseManager.Instance.UpdateSpiderShopPosition();
      BiomeBaseManager.Instance.SpiderShop.ShopKeeper.SetMutated();
      BiomeBaseManager.Instance.SpiderShop.ShopKeeper.free = true;
      BiomeBaseManager.Instance.SpiderShop.RotFollowerConvo.gameObject.SetActive(true);
      if ((UnityEngine.Object) BiomeBaseManager.Instance.SpiderShop.PostGameConvo != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) BiomeBaseManager.Instance.SpiderShop.PostGameConvo.gameObject);
      yield return (object) null;
      while (LetterBox.IsPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
    }
    GameManager.GetInstance().OnConversationNext(followerRecruit.CameraBone, 4f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(followerRecruit.transform.position, followerRecruit.Follower.transform.position);
    yield return (object) new WaitForSeconds(0.3f);
    if (customise)
    {
      followerRecruit.FollowersTurnedOff.Clear();
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      {
        SkeletonAnimation spine = locationFollower.Spine;
        if (spine.gameObject.activeSelf && (double) Vector3.Distance(spine.transform.position, followerRecruit.transform.position) < 1.0 && (double) spine.transform.position.y < (double) followerRecruit.transform.position.y)
        {
          Debug.Log((object) ("Turning off gameobject: " + spine.name));
          MeshRenderer component = spine.gameObject.GetComponent<MeshRenderer>();
          component.enabled = false;
          followerRecruit.FollowersTurnedOff.Add(component);
        }
      }
      followerRecruit.Follower.Brain.Info.SkinCharacter = followerRecruit.Follower.Brain.Info.SkinName.Contains("Boss") || followerRecruit.Follower.Brain.Info.SkinName.Contains("CultLeader") || followerRecruit.Follower.Brain.Info.SkinName.Contains("Snowman") ? WorshipperData.Instance.GetSkinIndexFromName(followerRecruit.Follower.Brain.Info.SkinName) : WorshipperData.Instance.GetSkinIndexFromName(followerRecruit.Follower.Brain.Info.SkinName.StripNumbers());
      GameManager.GetInstance().CameraSetOffset(new Vector3(-2f, 0.0f, 0.0f));
      UIFollowerIndoctrinationMenuController indoctrinationMenuInstance = MonoSingleton<UIManager>.Instance.ShowIndoctrinationMenu(followerRecruit.Follower, new OriginalFollowerLookData(followerRecruit.Follower.Brain._directInfoAccess));
      indoctrinationMenuInstance.OnIndoctrinationCompleted += new System.Action(followerRecruit.\u003CSimpleNewRecruitRoutine\u003Eb__51_0);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController1 = indoctrinationMenuInstance;
      indoctrinationMenuController1.OnShown = indoctrinationMenuController1.OnShown + new System.Action(followerRecruit.\u003CSimpleNewRecruitRoutine\u003Eb__51_1);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController2 = indoctrinationMenuInstance;
      indoctrinationMenuController2.OnHide = indoctrinationMenuController2.OnHide + new System.Action(followerRecruit.\u003CSimpleNewRecruitRoutine\u003Eb__51_2);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController3 = indoctrinationMenuInstance;
      indoctrinationMenuController3.OnHidden = indoctrinationMenuController3.OnHidden + (System.Action) (() => indoctrinationMenuInstance = (UIFollowerIndoctrinationMenuController) null);
    }
    else
      followerRecruit.StartCoroutine((IEnumerator) followerRecruit.CharacterSetupCallback());
  }

  public void FollowerEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "indoctrinated") || this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.OldAge)
      return;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.25f, 1f, 0.33f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    this.Follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    this.Follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.Follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.FollowerEvent);
  }

  public IEnumerator CharacterSetupCallback()
  {
    FollowerRecruit recruit = this;
    if (recruit.Follower.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
    {
      recruit.Follower.Brain.DesiredLocation = recruit.Follower.Brain._directInfoAccess.Location = FollowerLocation.Base;
      recruit.Follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    }
    System.Action characterSetupCallback = recruit.OnCharacterSetupCallback;
    if (characterSetupCallback != null)
      characterSetupCallback();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    recruit.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("recruit", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(recruit.transform.position, recruit.Follower.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", recruit.Follower.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", PlayerFarming.Instance.gameObject);
    double num1 = (double) recruit.Follower.SetBodyAnimation("Indoctrinate/indoctrinate-finish", false);
    yield return (object) new WaitForSeconds(4f);
    recruit.Follower.SimpleAnimator?.ResetAnimationsToDefaults();
    if (recruit.RecruitOnComplete)
      FollowerManager.RecruitFollower(recruit, false);
    yield return (object) new WaitForEndOfFrame();
    recruit.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) null;
    if (recruit.Follower.Brain.Stats.Thoughts.Count <= 0)
    {
      float num2 = UnityEngine.Random.value;
      if ((double) num2 <= 0.20000000298023224)
        recruit.Follower.Brain.AddThought(Thought.EnthusiasticNewRecruit);
      else if ((double) num2 > 0.20000000298023224 && (double) num2 < 0.800000011920929)
        recruit.Follower.Brain.AddThought(Thought.HappyNewRecruit);
      else if ((double) num2 >= 0.800000011920929)
        recruit.Follower.Brain.AddThought(Thought.UnenthusiasticNewRecruit);
    }
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      ThoughtData thought = allBrain.GetThought(Thought.Fasting);
      if (thought != null)
        recruit.Follower.Brain.AddThought(thought.Clone());
    }
    Debug.Log((object) ("RecruitOnComplete:  " + recruit.RecruitOnComplete.ToString()));
    if (recruit.RecruitOnComplete)
    {
      Debug.Log((object) "OnNewRecruit?.Invoke();");
      recruit.Follower.Spine.transform.localScale = new Vector3(1f, 1f, 1f);
      recruit.FollowerInteraction.enabled = true;
      recruit.FollowerInteraction.SelectTask(recruit.state, false, false);
      System.Action onNewRecruit = FollowerRecruit.OnNewRecruit;
      if (onNewRecruit != null)
        onNewRecruit();
      if (DataManager.Instance.FirstTimeInDungeon)
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GetNewFollowersFromDungeon);
      if (recruit.Follower.Brain.Info.CursedState != Thought.Child)
        FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
      CultFaithManager.AddThought(Thought.Cult_NewFolllower);
    }
    else
      recruit.Follower.Spine.transform.localScale = new Vector3(1f, 1f, 1f);
    if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.NaturallySkeptical))
      CultFaithManager.AddThought(Thought.Cult_NewRecruitSkeptical);
    if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.NaturallyObedient))
      CultFaithManager.AddThought(Thought.Cult_NewRecruitObedient);
    if (DataManager.Instance.LeaderFollowersRecruited.Count >= 5)
    {
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_LEADER_FOLLOWERS"));
      Debug.Log((object) "ACHIEVEMENT GOT : ALL_LEADER_FOLLOWERS");
    }
    if (recruit.Follower.Brain.Info.SkinName == "Mushroom")
      DataManager.Instance.SozoMushroomRecruitedDay = TimeManager.CurrentDay + 5;
    if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard))
      recruit.Follower.Brain.AddThought((Thought) UnityEngine.Random.Range(356, 359));
    else if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.Scared))
      recruit.Follower.Brain.AddThought((Thought) UnityEngine.Random.Range(359, 362));
    StructureAndTime.SetTime(recruit.Follower.Brain.Info.ID, recruit.Follower.Brain, StructureAndTime.IDTypes.Follower);
    recruit.OnRecruitFinished();
    DataManager.Instance.followerRecruitWaiting = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) recruit);
  }

  public IEnumerator GiveExecutionerObjective()
  {
    while (true)
    {
      if (!LetterBox.IsPlaying && !MMConversation.isPlaying)
        yield return (object) new WaitForSeconds(0.25f);
      if (LetterBox.IsPlaying || MMConversation.isPlaying)
        yield return (object) null;
      else
        break;
    }
    yield return (object) new WaitForSeconds(1f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Executioner", Objectives.CustomQuestTypes.ReceiveClueFromPlimbo), true, true);
  }

  public void ContinueRecruit()
  {
    Debug.Log((object) nameof (ContinueRecruit));
    this.StartCoroutine((IEnumerator) this.ContinueRecruitRoutine());
  }

  public IEnumerator ContinueRecruitRoutine()
  {
    FollowerRecruit recruit = this;
    Debug.Log((object) "ContinueRecruitRoutine ");
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(recruit.CameraBone, 4f);
    recruit.Follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    recruit.Follower.SetOutfit(FollowerOutfitType.Follower, false);
    recruit.Follower.SimpleAnimator.ResetAnimationsToDefaults();
    double num = (double) recruit.Follower.SetBodyAnimation("recruit-end", false);
    recruit.Follower.AddBodyAnimation("idle", true, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("recruit-end", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    if (recruit.RecruitOnComplete)
      FollowerManager.RecruitFollower(recruit, false);
    recruit.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action statueCallback = recruit.StatueCallback;
    if (statueCallback != null)
      statueCallback();
    recruit.OnRecruitFinished();
    UnityEngine.Object.Destroy((UnityEngine.Object) recruit);
  }

  public void InstantRecruit(bool followPlayer = false)
  {
    this.StartCoroutine((IEnumerator) this.InstantRecruitRoutine(followPlayer));
  }

  public IEnumerator InstantRecruitRoutine(bool followPlayer)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerRecruit recruit = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      recruit.OnRecruitFinished();
      UnityEngine.Object.Destroy((UnityEngine.Object) recruit);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    recruit.Follower.SimpleAnimator.ResetAnimationsToDefaults();
    recruit.Follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    recruit.Follower.SetOutfit(FollowerOutfitType.Follower, false);
    FollowerManager.RecruitFollower(recruit, followPlayer);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnRecruitFinished()
  {
    if ((UnityEngine.Object) this.skeletonAnimationLODManager != (UnityEngine.Object) null)
      this.skeletonAnimationLODManager.DisableLODManager(false);
    TwitchFollowers.SendFollowers();
    FollowerRecruit.FollowerEventDelegate recruitFinalised = FollowerRecruit.OnRecruitFinalised;
    if (recruitFinalised == null)
      return;
    recruitFinalised(this.Follower.Brain._directInfoAccess);
  }

  public void CallbackSacrifice()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CarryBone.gameObject, 8f);
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.AltarPosition.gameObject, this.gameObject, GoToCallback: new System.Action(this.ContinueSacrifice));
  }

  public void ContinueSacrifice()
  {
    this.StartCoroutine((IEnumerator) ChurchFollowerManager.Instance.DoSacrificeRoutine((Interaction) this, this.Follower.Brain.Info.ID, new System.Action(this.CompleteSacrifice)));
  }

  public void CompleteSacrifice()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.Follower.SimpleAnimator.ResetAnimationsToDefaults();
    FollowerManager.RemoveRecruit(this.Follower.Brain.Info.ID);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    ChurchFollowerManager.Instance.ExitAllFollowers();
  }

  [CompilerGenerated]
  public void \u003CSpawnAnim\u003Eb__31_0()
  {
    if (this.spawnedEgg)
      return;
    this.spawnedEgg = true;
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.EGG_FOLLOWER, 0);
    infoByType.EggInfo = new StructuresData.EggData()
    {
      Parent1Name = this.Follower.Brain.Info.Name,
      Parent_1_ID = this.Follower.Brain.Info.ID
    };
    infoByType.CanBecomeRotten = false;
    if (this.Follower.Brain._directInfoAccess.FromDragonNPC)
      infoByType.EggInfo.Parent_1_ID = -666;
    this.Follower.Brain._directInfoAccess.FromDragonNPC = false;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.transform.position + this.Follower.Spine.transform.localPosition, Vector2Int.one, false, (System.Action<GameObject>) (obj =>
    {
      obj.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, false, false, this.Follower.Brain.Info.Special);
      PickUp component = obj.GetComponent<PickUp>();
      component.Bounce = false;
      component.ScaleIn = false;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Follower.gameObject);
      FollowerManager.RemoveFollower(this.Follower.Brain.Info.ID);
      FollowerManager.RemoveFollowerBrain(this.Follower.Brain.Info.ID);
      DataManager.Instance.Followers_Recruit.RemoveAt(0);
      BiomeBaseManager.Instance.SpawnExistingRecruits = false;
    }), emitParticles: false);
  }

  [CompilerGenerated]
  public void \u003CSpawnAnim\u003Eb__31_1(GameObject obj)
  {
    obj.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, false, false, this.Follower.Brain.Info.Special);
    PickUp component = obj.GetComponent<PickUp>();
    component.Bounce = false;
    component.ScaleIn = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.Follower.gameObject);
    FollowerManager.RemoveFollower(this.Follower.Brain.Info.ID);
    FollowerManager.RemoveFollowerBrain(this.Follower.Brain.Info.ID);
    DataManager.Instance.Followers_Recruit.RemoveAt(0);
    BiomeBaseManager.Instance.SpawnExistingRecruits = false;
  }

  [CompilerGenerated]
  public void \u003CCompleteCallBack\u003Eb__47_0()
  {
    PlayerFarming.Instance.transform.position = this.transform.position + new Vector3(-1.5f, -0.1f);
  }

  [CompilerGenerated]
  public void \u003CCompleteCallBack\u003Eb__47_1()
  {
    this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine(true));
  }

  [CompilerGenerated]
  public void \u003CSimpleNewRecruitRoutine\u003Eb__51_0()
  {
    this.StartCoroutine((IEnumerator) this.CharacterSetupCallback());
  }

  [CompilerGenerated]
  public void \u003CSimpleNewRecruitRoutine\u003Eb__51_1()
  {
    LightingManager.Instance.inOverride = true;
    this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(true);
  }

  [CompilerGenerated]
  public void \u003CSimpleNewRecruitRoutine\u003Eb__51_2()
  {
    foreach (Renderer renderer in this.FollowersTurnedOff)
      renderer.enabled = true;
    this.FollowersTurnedOff.Clear();
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
  }

  public delegate void FollowerEventDelegate(FollowerInfo info);
}
