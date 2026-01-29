// Decompiled with JetBrains decompiler
// Type: MysticShopKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using Lamb.UI;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class MysticShopKeeperManager : BaseMonoBehaviour
{
  public static MysticShopKeeperManager Instance;
  public GenerateRoom generateRoom;
  public GameObject Player;
  public GameObject PlayerPrefab;
  public Transform PlayerPosition;
  public BiomeGenerator biomeGenerator;
  public SimpleSetCamera SimpleSetCamera;
  public SkeletonAnimation Spine;
  [SerializeField]
  public GoopFade goop;
  [SerializeField]
  public Animator animator;
  public SkeletonAnimation skeletonAnimation;
  public Stylizer cameraStylizer;
  public ConversationObject ConversationObject;
  public List<ConversationEntry> ConversationEntries;
  public bool Translate;

  public void OnEnable()
  {
    MysticShopKeeperManager.Instance = this;
    this.generateRoom = this.GetComponent<GenerateRoom>();
    this.cameraStylizer = Camera.main.gameObject.GetComponent<Stylizer>();
    if ((UnityEngine.Object) this.cameraStylizer == (UnityEngine.Object) null)
      Debug.Log((object) "Camera null");
    this.cameraStylizer.enabled = true;
    this.goop.gameObject.SetActive(false);
    AudioManager.Instance.PlayMusic("event:/music/death_cat_battle/death_cat_battle");
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardRoom);
  }

  public void OnDisable()
  {
    WeatherSystemController.Instance.ExitedBuilding();
    if ((UnityEngine.Object) this.cameraStylizer != (UnityEngine.Object) null)
      this.cameraStylizer.enabled = false;
    if (!((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null))
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void Init(BiomeGenerator biomeGenerator) => this.biomeGenerator = biomeGenerator;

  public static void Play()
  {
    if ((UnityEngine.Object) MysticShopKeeperManager.Instance == (UnityEngine.Object) null)
      MysticShopKeeperManager.Instance = UnityEngine.Object.FindObjectOfType<MysticShopKeeperManager>();
    Debug.Log((object) "PLAY!");
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      Time.timeScale = 1f;
      MysticShopKeeperManager.Instance.gameObject.SetActive(true);
      MysticShopKeeperManager.Instance.StartCoroutine((IEnumerator) MysticShopKeeperManager.Instance.PlayRoutine());
    }));
  }

  public IEnumerator ConversationCompleted()
  {
    MysticShopKeeperManager shopKeeperManager = this;
    MysticShopKeeperManager.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNext(shopKeeperManager.PlayerPosition.gameObject, 8f);
    yield return (object) new WaitForSeconds(0.5f);
    shopKeeperManager.Player.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", shopKeeperManager.gameObject);
    shopKeeperManager.animator.Play("WarpOut");
    PlayerFarming.Instance.simpleSpineAnimator.Animate("warp-out-down", 0, false);
    shopKeeperManager.goop.gameObject.SetActive(true);
    shopKeeperManager.goop.FadeIn(1f, 1.4f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
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
    AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", PlayerFarming.Instance.gameObject);
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
    MysticShopKeeperManager shopKeeperManager = this;
    WeatherSystemController.Instance.EnteredBuilding();
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", shopKeeperManager.gameObject);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", shopKeeperManager.gameObject);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    MysticShopKeeperManager.Instance.generateRoom.SetColliderAndUpdatePathfinding();
    MysticShopKeeperManager.Instance.biomeGenerator.gameObject.SetActive(false);
    MysticShopKeeperManager.Instance.biomeGenerator.Player.SetActive(false);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.gameObject.SetActive(false);
      player.hudHearts.gameObject.SetActive(false);
    }
    yield return (object) null;
    Camera.main.backgroundColor = Color.white;
    shopKeeperManager.Player = UnityEngine.Object.Instantiate<GameObject>(shopKeeperManager.PlayerPrefab, shopKeeperManager.PlayerPosition.position, Quaternion.identity, shopKeeperManager.transform);
    PlayerFarming playerFarming = shopKeeperManager.Player.GetComponent<PlayerFarming>();
    PlayerFarming.Instance = playerFarming;
    GameManager.GetInstance().CameraSnapToPosition(shopKeeperManager.Player.transform.position);
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(playerFarming.CameraBone, 4f);
    StateMachine component = shopKeeperManager.Player.GetComponent<StateMachine>();
    component.facingAngle = 85f;
    component.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    if ((UnityEngine.Object) shopKeeperManager.skeletonAnimation == (UnityEngine.Object) null)
      shopKeeperManager.skeletonAnimation = playerFarming.Spine;
    if ((UnityEngine.Object) shopKeeperManager.skeletonAnimation != (UnityEngine.Object) null)
      shopKeeperManager.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(shopKeeperManager.HandleAnimationStateEvent);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_on", shopKeeperManager.gameObject);
    shopKeeperManager.animator.SetTrigger("warpIn");
    playerFarming.simpleSpineAnimator.Animate("warp-in-up", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle-up", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(3f);
    if ((UnityEngine.Object) shopKeeperManager.skeletonAnimation != (UnityEngine.Object) null)
      shopKeeperManager.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(shopKeeperManager.HandleAnimationStateEvent);
    MysticShopKeeperManager.Instance.SimpleSetCamera.Play();
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.ForeshadowedMysticShop = true;
    shopKeeperManager.Translate = true;
    shopKeeperManager.ConversationObject = new ConversationObject(shopKeeperManager.ConversationEntries, (List<MMTools.Response>) null, new System.Action(shopKeeperManager.\u003CPlayRoutine\u003Eb__22_0));
    MMConversation.Play(shopKeeperManager.ConversationObject, false, Translate: shopKeeperManager.Translate);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "warp-in-burst_start")
    {
      PlayerFarming.Instance.simpleSpineAnimator.SetColor(Color.black);
      PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().enabled = true;
    }
    if (!(e.Data.Name == "warp-in-burst_end"))
      return;
    PlayerFarming.Instance.simpleSpineAnimator.SetColor(Color.white);
  }

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__22_0()
  {
    this.StartCoroutine((IEnumerator) this.ConversationCompleted());
  }
}
