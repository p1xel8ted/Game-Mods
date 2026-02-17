// Decompiled with JetBrains decompiler
// Type: DeathCatRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DeathCatRoomManager : BaseMonoBehaviour
{
  public static DeathCatRoomManager.ConversationTypes ConversationType;
  public static bool Testing;
  public static DeathCatRoomManager Instance;
  public BiomeGenerator biomeGenerator;
  public GenerateRoom generateRoom;
  public GameObject Player;
  public GameObject PlayerPrefab;
  public Transform PlayerPosition;
  public GameObject DeathCatObject;
  public SimpleSetCamera SimpleSetCamera;
  public SkeletonAnimation Spine;
  [TermsPopup("")]
  public string CharacterName = "-";
  [SerializeField]
  public GoopFade goop;
  [SerializeField]
  public Animator animator;
  public SkeletonAnimation skeletonAnimation;
  public List<string> Sounds = new List<string>()
  {
    "event:/dialogue/death_cat/standard_death"
  };
  public List<string> Animations = new List<string>()
  {
    "talk",
    "talk2",
    "talk3",
    "talk-laugh"
  };
  public Stylizer cameraStylizer;
  public static DeathCatRoomManager.ConversationTypes TestingResult;
  public List<string> AnimationList = new List<string>()
  {
    "talk",
    "talk2",
    "talk3",
    "talk-laugh"
  };
  public ConversationObject ConversationObject;
  public List<ConversationEntry> ConversationEntries;
  public bool Translate;

  public static DeathCatRoomManager.ConversationTypes GetConversationType()
  {
    if (DeathCatRoomManager.Testing)
    {
      DeathCatRoomManager.Story = 1;
      DeathCatRoomManager.ConversationType = DeathCatRoomManager.TestingResult;
      DeathCatRoomManager.Testing = false;
      return DeathCatRoomManager.ConversationType;
    }
    DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.None;
    if (DataManager.Instance.DeathCatBeaten)
      return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.None;
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && !DeathCatRoomManager.Boss1)
      return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.BeatBoss1;
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && !DeathCatRoomManager.Boss2)
      return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.BeatBoss2;
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && !DeathCatRoomManager.Boss3)
      return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.BeatBoss3;
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4) && !DeathCatRoomManager.Boss4)
      return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.BeatBoss4;
    if (DataManager.Instance.RatauKilled && !DeathCatRoomManager.RatauKilled)
      return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.RatauKilled;
    if (DataManager.Instance.DeathCatConversationLastRun < DataManager.Instance.dungeonRun + 3)
    {
      switch (UIDeathScreenOverlayController.Result)
      {
        case UIDeathScreenOverlayController.Results.Killed:
          Debug.Log((object) "Killed");
          return DeathCatRoomManager.ConversationType = DeathCatRoomManager.GetConversationCount(DeathCatRoomManager.ConversationTypes.GenericDied.ToString() + DeathCatRoomManager.Dead.ToString()) > 0 ? DeathCatRoomManager.ConversationTypes.GenericDied : DeathCatRoomManager.ConversationTypes.None;
        case UIDeathScreenOverlayController.Results.Completed:
          Debug.Log((object) "Completed");
          return DataManager.Instance.dungeonRun >= 2 && DeathCatRoomManager.GetConversationCount(DeathCatRoomManager.ConversationTypes.Story.ToString() + DeathCatRoomManager.Story.ToString()) > 0 && UIDeathScreenOverlayController.Result == UIDeathScreenOverlayController.Results.Completed && (DeathCatRoomManager.Story != 2 || DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Special_Bonfire)) ? (DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.Story) : (DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.None);
        case UIDeathScreenOverlayController.Results.Escaped:
          Debug.Log((object) "ESCAPED");
          return DeathCatRoomManager.ConversationType = DeathCatRoomManager.ConversationTypes.None;
      }
    }
    return DeathCatRoomManager.ConversationType;
  }

  public void OnEnable()
  {
    DeathCatRoomManager.Instance = this;
    this.generateRoom = this.GetComponent<GenerateRoom>();
    this.cameraStylizer = Camera.main.gameObject.GetComponent<Stylizer>();
    if ((UnityEngine.Object) this.cameraStylizer == (UnityEngine.Object) null)
      Debug.Log((object) "Camera null");
    this.cameraStylizer.enabled = true;
    this.goop.gameObject.SetActive(false);
  }

  public void OnDisable()
  {
    WeatherSystemController.Instance.ExitedBuilding();
    this.cameraStylizer.enabled = false;
    if (!((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null))
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void Test(DeathCatRoomManager.ConversationTypes Result = DeathCatRoomManager.ConversationTypes.Story)
  {
    DeathCatRoomManager.TestingResult = Result;
    DeathCatRoomManager.Testing = true;
    DeathCatRoomManager.Play();
  }

  public static void Play()
  {
    Debug.Log((object) "PLAY!");
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      if ((UnityEngine.Object) DeathCatRoomManager.Instance == (UnityEngine.Object) null)
        DeathCatRoomManager.Instance = UnityEngine.Object.FindObjectOfType<DeathCatRoomManager>();
      Time.timeScale = 1f;
      DeathCatRoomManager.Instance.gameObject.SetActive(true);
      DeathCatRoomManager.Instance.StartCoroutine((IEnumerator) DeathCatRoomManager.Instance.PlayRoutine());
    }));
  }

  public void Init(BiomeGenerator biomeGenerator) => this.biomeGenerator = biomeGenerator;

  public static int Story
  {
    get => DataManager.Instance.DeathCatStory;
    set => DataManager.Instance.DeathCatStory = value;
  }

  public static bool Boss1
  {
    get => DataManager.Instance.DeathCatBoss1;
    set => DataManager.Instance.DeathCatBoss1 = value;
  }

  public static bool Boss2
  {
    get => DataManager.Instance.DeathCatBoss2;
    set => DataManager.Instance.DeathCatBoss2 = value;
  }

  public static bool Boss3
  {
    get => DataManager.Instance.DeathCatBoss3;
    set => DataManager.Instance.DeathCatBoss3 = value;
  }

  public static bool Boss4
  {
    get => DataManager.Instance.DeathCatBoss4;
    set => DataManager.Instance.DeathCatBoss4 = value;
  }

  public static bool RatauKilled
  {
    get => DataManager.Instance.DeathCatRatauKilled;
    set => DataManager.Instance.DeathCatRatauKilled = value;
  }

  public static int Dead
  {
    get => DataManager.Instance.DeathCatDead;
    set => DataManager.Instance.DeathCatDead = value;
  }

  public static int Won
  {
    get => DataManager.Instance.DeathCatWon;
    set => DataManager.Instance.DeathCatWon = value;
  }

  public string GetConversation()
  {
    DeathCatRoomManager.ConversationTypes conversationType = DeathCatRoomManager.GetConversationType();
    switch (conversationType)
    {
      case DeathCatRoomManager.ConversationTypes.Story:
        return "Story" + DeathCatRoomManager.Story.ToString();
      case DeathCatRoomManager.ConversationTypes.GenericDied:
        return "GenericDied" + DeathCatRoomManager.Dead.ToString();
      case DeathCatRoomManager.ConversationTypes.GenericWon:
        return "GenericWon" + DeathCatRoomManager.Won.ToString();
      case DeathCatRoomManager.ConversationTypes.RatauKilled:
        return "Ratau";
      default:
        return conversationType.ToString();
    }
  }

  public static int GetConversationCount(string Entry)
  {
    int conversationCount = 0;
    int num = -1;
    string str = $"DeathCat/{Entry}/";
    Debug.Log((object) (str + (num + 1).ToString()));
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
      ++conversationCount;
    return conversationCount;
  }

  public List<ConversationEntry> GetConversationEntry(string Entry)
  {
    List<ConversationEntry> conversationEntry = new List<ConversationEntry>();
    string str = $"DeathCat/{Entry}/";
    int num = -1;
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
    {
      Debug.Log((object) (str + num.ToString()));
      conversationEntry.Add(new ConversationEntry(this.DeathCatObject, str + num.ToString(), this.Animations[UnityEngine.Random.Range(0, this.Animations.Count)], "event:/dialogue/death_cat/standard_death"));
      conversationEntry[conversationEntry.Count - 1].CharacterName = this.CharacterName;
      conversationEntry[conversationEntry.Count - 1].SkeletonData = this.Spine;
      conversationEntry[conversationEntry.Count - 1].Animation = this.AnimationList[UnityEngine.Random.Range(0, this.AnimationList.Count)];
    }
    return conversationEntry;
  }

  public IEnumerator ConversationCompleted()
  {
    DeathCatRoomManager deathCatRoomManager = this;
    DeathCatRoomManager.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNext(deathCatRoomManager.PlayerPosition.gameObject, 8f);
    float Delay = 0.5f;
    switch (DeathCatRoomManager.ConversationType)
    {
      case DeathCatRoomManager.ConversationTypes.Story:
        Debug.Log((object) ("STORY: " + DeathCatRoomManager.Story.ToString()));
        switch (DeathCatRoomManager.Story)
        {
          case 1:
            Delay = 0.0f;
            yield return (object) deathCatRoomManager.StartCoroutine((IEnumerator) deathCatRoomManager.GiveDoctrine(2));
            break;
          case 2:
            Delay = 0.0f;
            yield return (object) deathCatRoomManager.StartCoroutine((IEnumerator) deathCatRoomManager.GiveDoctrine(3));
            break;
          case 3:
            DataManager.Instance.OnboardedOfferingChest = true;
            break;
        }
        ++DeathCatRoomManager.Story;
        break;
      case DeathCatRoomManager.ConversationTypes.BeatBoss1:
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BishopsOfTheOldFaith", Objectives.CustomQuestTypes.FreeDeathCat));
        DeathCatRoomManager.Boss1 = true;
        break;
      case DeathCatRoomManager.ConversationTypes.BeatBoss2:
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BishopsOfTheOldFaith", Objectives.CustomQuestTypes.FreeDeathCat));
        DeathCatRoomManager.Boss2 = true;
        break;
      case DeathCatRoomManager.ConversationTypes.BeatBoss3:
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BishopsOfTheOldFaith", Objectives.CustomQuestTypes.FreeDeathCat));
        DeathCatRoomManager.Boss3 = true;
        break;
      case DeathCatRoomManager.ConversationTypes.BeatBoss4:
        if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BishopsOfTheOldFaith", Objectives.CustomQuestTypes.FreeDeathCat));
        DeathCatRoomManager.Boss4 = true;
        break;
      case DeathCatRoomManager.ConversationTypes.GenericDied:
        ++DeathCatRoomManager.Dead;
        break;
      case DeathCatRoomManager.ConversationTypes.GenericWon:
        ++DeathCatRoomManager.Won;
        break;
      case DeathCatRoomManager.ConversationTypes.RatauKilled:
        DeathCatRoomManager.RatauKilled = true;
        break;
    }
    DataManager.Instance.DeathCatConversationLastRun = DataManager.Instance.dungeonRun;
    yield return (object) null;
    yield return (object) new WaitForSeconds(Delay);
    deathCatRoomManager.Player.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", deathCatRoomManager.gameObject);
    deathCatRoomManager.animator.Play("WarpOut");
    if ((bool) (UnityEngine.Object) deathCatRoomManager.Player)
      deathCatRoomManager.Player.GetComponent<PlayerFarming>().simpleSpineAnimator.Animate("warp-out-down", 0, false);
    deathCatRoomManager.goop.gameObject.SetActive(true);
    deathCatRoomManager.goop.FadeIn(1f, 1.4f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    if (deathCatRoomManager.GoViaQuote())
    {
      QuoteScreenController.Init(new List<QuoteScreenController.QuoteTypes>()
      {
        deathCatRoomManager.GetQuoteType()
      }, (System.Action) (() => GameManager.ToShip()), (System.Action) (() => GameManager.ToShip()));
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "QuoteScreen", 5f, "", (System.Action) (() => Time.timeScale = 1f));
    }
    else
      GameManager.ToShip();
  }

  public IEnumerator GiveDoctrine(int Level)
  {
    yield return (object) null;
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, Level, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) this.Player)
      AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", this.Player);
    DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.GetSermonReward(SermonCategory.Special, Level, true));
    UITutorialOverlayController TutorialOverlay = (UITutorialOverlayController) null;
    switch (Level)
    {
      case 2:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.FollowerAction))
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.FollowerAction);
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("FollowerInteractions/Surveillance", Objectives.CustomQuestTypes.ReadMind), true);
        break;
      case 3:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Rituals))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Rituals);
          break;
        }
        break;
    }
    while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
      yield return (object) null;
  }

  public bool GoViaQuote()
  {
    switch (DeathCatRoomManager.ConversationType)
    {
      case DeathCatRoomManager.ConversationTypes.BeatBoss1:
      case DeathCatRoomManager.ConversationTypes.BeatBoss2:
      case DeathCatRoomManager.ConversationTypes.BeatBoss3:
      case DeathCatRoomManager.ConversationTypes.BeatBoss4:
        return true;
      default:
        return false;
    }
  }

  public QuoteScreenController.QuoteTypes GetQuoteType()
  {
    Debug.Log((object) ("GET QUOTE TYPE! " + PlayerFarming.Location.ToString()));
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return QuoteScreenController.QuoteTypes.QuoteBoss1;
      case FollowerLocation.Dungeon1_2:
        return QuoteScreenController.QuoteTypes.QuoteBoss2;
      case FollowerLocation.Dungeon1_3:
        return QuoteScreenController.QuoteTypes.QuoteBoss3;
      case FollowerLocation.Dungeon1_4:
        return QuoteScreenController.QuoteTypes.QuoteBoss4;
      case FollowerLocation.Dungeon1_5:
        return QuoteScreenController.QuoteTypes.QuoteBoss5;
      default:
        return QuoteScreenController.QuoteTypes.QuoteBoss5;
    }
  }

  public IEnumerator PlayRoutine()
  {
    DeathCatRoomManager deathCatRoomManager = this;
    WeatherSystemController.Instance.EnteredBuilding();
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", deathCatRoomManager.gameObject);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", deathCatRoomManager.gameObject);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    DeathCatRoomManager.Instance.generateRoom.SetColliderAndUpdatePathfinding();
    DeathCatRoomManager.Instance.biomeGenerator.gameObject.SetActive(false);
    DeathCatRoomManager.Instance.biomeGenerator.Player.SetActive(false);
    yield return (object) null;
    Camera.main.backgroundColor = Color.white;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.gameObject.SetActive(false);
      player.hudHearts.gameObject.SetActive(false);
    }
    deathCatRoomManager.Player = deathCatRoomManager.PlayerPrefab;
    PlayerFarming playerFarming = deathCatRoomManager.Player.GetComponent<PlayerFarming>();
    playerFarming.Init();
    deathCatRoomManager.Player.SetActive(true);
    deathCatRoomManager.Player.transform.position = deathCatRoomManager.PlayerPosition.position;
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(playerFarming.CameraBone, 4f);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(playerFarming.CameraBone);
    GameManager.GetInstance().CameraSnapToPosition(deathCatRoomManager.Player.transform.position);
    StateMachine component = deathCatRoomManager.Player.GetComponent<StateMachine>();
    component.facingAngle = 85f;
    component.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    if ((UnityEngine.Object) deathCatRoomManager.skeletonAnimation == (UnityEngine.Object) null)
      deathCatRoomManager.skeletonAnimation = playerFarming.Spine;
    if ((UnityEngine.Object) deathCatRoomManager.skeletonAnimation != (UnityEngine.Object) null)
      deathCatRoomManager.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(deathCatRoomManager.HandleAnimationStateEvent);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_on", deathCatRoomManager.gameObject);
    deathCatRoomManager.animator.SetTrigger("warpIn");
    playerFarming.simpleSpineAnimator.Animate("warp-in-up", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle-up", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(3f);
    if ((UnityEngine.Object) deathCatRoomManager.skeletonAnimation != (UnityEngine.Object) null)
      deathCatRoomManager.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(deathCatRoomManager.HandleAnimationStateEvent);
    DeathCatRoomManager.Instance.SimpleSetCamera.Play();
    yield return (object) new WaitForSeconds(1f);
    deathCatRoomManager.Translate = true;
    deathCatRoomManager.ConversationEntries = deathCatRoomManager.GetConversationEntry(deathCatRoomManager.GetConversation());
    deathCatRoomManager.AddFinalConversation();
    deathCatRoomManager.ConversationObject = new ConversationObject(deathCatRoomManager.ConversationEntries, (List<MMTools.Response>) null, new System.Action(deathCatRoomManager.\u003CPlayRoutine\u003Eb__61_0));
    MMConversation.Play(deathCatRoomManager.ConversationObject, false, Translate: deathCatRoomManager.Translate);
  }

  public void AddFinalConversation()
  {
    if (DeathCatRoomManager.ConversationType != DeathCatRoomManager.ConversationTypes.BeatBoss1 && DeathCatRoomManager.ConversationType != DeathCatRoomManager.ConversationTypes.BeatBoss2 && DeathCatRoomManager.ConversationType != DeathCatRoomManager.ConversationTypes.BeatBoss3 && DeathCatRoomManager.ConversationType != DeathCatRoomManager.ConversationTypes.BeatBoss4 || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
      return;
    foreach (ConversationEntry conversationEntry in this.GetConversationEntry("BossFinal"))
      this.ConversationEntries.Add(conversationEntry);
    this.Translate = false;
    ConversationEntry conversationEntry1 = (ConversationEntry) null;
    ConversationEntry conversationEntry2 = (ConversationEntry) null;
    ConversationEntry conversationEntry3 = (ConversationEntry) null;
    foreach (ConversationEntry conversationEntry4 in this.ConversationEntries)
    {
      if (conversationEntry4.TermToSpeak == "DeathCat/BossFinal/2")
      {
        conversationEntry1 = conversationEntry4;
        conversationEntry4.TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry4.TermToSpeak), (object) DataManager.Instance.STATS_FollowersStarvedToDeath);
      }
      else if (conversationEntry4.TermToSpeak == "DeathCat/BossFinal/3")
      {
        conversationEntry2 = conversationEntry4;
        conversationEntry4.TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry4.TermToSpeak), (object) DataManager.Instance.STATS_Sacrifices);
      }
      else if (conversationEntry4.TermToSpeak == "DeathCat/BossFinal/4")
      {
        conversationEntry3 = conversationEntry4;
        conversationEntry4.TermToSpeak = string.Format(LocalizationManager.GetTranslation(conversationEntry4.TermToSpeak), (object) DataManager.Instance.STATS_Murders);
      }
      else
        conversationEntry4.TermToSpeak = LocalizationManager.GetTranslation(conversationEntry4.TermToSpeak);
    }
    if (conversationEntry1 != null && DataManager.Instance.STATS_FollowersStarvedToDeath <= 0)
      this.ConversationEntries.Remove(conversationEntry1);
    if (conversationEntry2 != null && DataManager.Instance.STATS_Sacrifices <= 0)
      this.ConversationEntries.Remove(conversationEntry2);
    if (conversationEntry3 == null || DataManager.Instance.STATS_Murders > 0)
      return;
    this.ConversationEntries.Remove(conversationEntry3);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
      return;
    PlayerFarming component = this.Player.GetComponent<PlayerFarming>();
    if (e.Data.Name == "warp-in-burst_start")
    {
      component.simpleSpineAnimator.SetColor(Color.black);
      component.Spine.GetComponent<MeshRenderer>().enabled = true;
    }
    if (!(e.Data.Name == "warp-in-burst_end"))
      return;
    component.simpleSpineAnimator.SetColor(Color.white);
  }

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__61_0()
  {
    this.StartCoroutine((IEnumerator) this.ConversationCompleted());
  }

  public enum ConversationTypes
  {
    None,
    Story,
    BeatBoss1,
    BeatBoss2,
    BeatBoss3,
    BeatBoss4,
    GenericDied,
    GenericWon,
    RatauKilled,
  }
}
