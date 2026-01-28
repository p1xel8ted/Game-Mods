// Decompiled with JetBrains decompiler
// Type: YngyaRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class YngyaRoomManager : BaseMonoBehaviour
{
  public static YngyaRoomManager Instance;
  public BiomeGenerator biomeGenerator;
  public GenerateRoom generateRoom;
  public GameObject Player;
  public GameObject PlayerPrefab;
  public Transform PlayerPosition;
  public CanvasGroup[] quotes1;
  public CanvasGroup[] widgets1;
  public CanvasGroup[] quotes2;
  public CanvasGroup[] widgets2;
  public GameObject RotScene;
  public GameObject HolyScene;
  public SimpleSetCamera SimpleSetCamera;
  public SkeletonAnimation Spine;
  [SerializeField]
  public SkeletonAnimation[] ghosts;
  [SerializeField]
  public string[] ghostNames;
  [SerializeField]
  public GoopFade goop;
  [SerializeField]
  public Animator animator;
  public SkeletonAnimation skeletonAnimation;
  public bool playing;
  public string[] quotes1SFX = new string[3]
  {
    "event:/dlc/env/cutscene/yngyaheart_a/yngya_quote_01",
    "event:/dlc/env/cutscene/yngyaheart_a/yngya_quote_02",
    "event:/dlc/env/cutscene/yngyaheart_a/yngya_quote_03"
  };
  public string[] quotes2SFX = new string[3]
  {
    "event:/dlc/env/cutscene/yngyaheart_b/lambspirit_quote_01",
    "event:/dlc/env/cutscene/yngyaheart_b/lambspirit_quote_02",
    "event:/dlc/env/cutscene/yngyaheart_b/lambspirit_quote_03"
  };
  public Stylizer cameraStylizer;
  public ConversationObject ConversationObject;
  public List<ConversationEntry> ConversationEntries;
  public bool Translate;
  public EventInstance loopingSfx;

  public void OnEnable()
  {
    YngyaRoomManager.Instance = this;
    this.generateRoom = this.GetComponent<GenerateRoom>();
    this.goop.gameObject.SetActive(false);
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

  public static void Play()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      if ((UnityEngine.Object) YngyaRoomManager.Instance == (UnityEngine.Object) null)
        YngyaRoomManager.Instance = UnityEngine.Object.FindObjectOfType<YngyaRoomManager>();
      Time.timeScale = 1f;
      YngyaRoomManager.Instance.gameObject.SetActive(true);
      GameManager.GetInstance().StartCoroutine((IEnumerator) YngyaRoomManager.Instance.PlayRoutine());
    }));
  }

  public void Init(BiomeGenerator biomeGenerator) => this.biomeGenerator = biomeGenerator;

  public IEnumerator ConversationCompleted()
  {
    this.goop.gameObject.SetActive(true);
    this.goop.FadeIn(1f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    yield return (object) null;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    GameManager.ToShip();
    BiomeConstants.Instance.VignetteTween(0.0f, 0.0f, 0.0f);
  }

  public IEnumerator PlayRoutine()
  {
    YngyaRoomManager yngyaRoomManager = this;
    yngyaRoomManager.playing = true;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    DataManager.Instance.RevealedDLCMapHeart = true;
    ++DataManager.Instance.YngyaHeartRoomEncounters;
    if (DataManager.Instance.YngyaHeartRoomEncounters >= 2)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/yngyaheart_b/opening");
      AudioManager.Instance.PlayAtmos("event:/dlc/music/cutscene_yngyaheart_b/opening");
    }
    else
      AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/yngyaheart_a/opening");
    if (DataManager.Instance.YngyaHeartRoomEncounters < 2)
    {
      BiomeConstants.Instance.VignetteTween(0.0f, 1f, 1f);
      yngyaRoomManager.cameraStylizer = Camera.main.gameObject.GetComponent<Stylizer>();
      if ((UnityEngine.Object) yngyaRoomManager.cameraStylizer == (UnityEngine.Object) null)
        Debug.Log((object) "Camera null");
      yngyaRoomManager.cameraStylizer.enabled = true;
    }
    yngyaRoomManager.RotScene.gameObject.SetActive(DataManager.Instance.YngyaHeartRoomEncounters != 2);
    yngyaRoomManager.HolyScene.gameObject.SetActive(DataManager.Instance.YngyaHeartRoomEncounters == 2);
    WeatherSystemController.Instance.EnteredBuilding();
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", yngyaRoomManager.gameObject);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(yngyaRoomManager.SimpleSetCamera.camera.transform.position + Vector3.down * 3f);
    YngyaRoomManager.Instance.generateRoom.SetColliderAndUpdatePathfinding();
    YngyaRoomManager.Instance.biomeGenerator.gameObject.SetActive(false);
    YngyaRoomManager.Instance.biomeGenerator.Player.SetActive(false);
    yield return (object) null;
    Camera.main.backgroundColor = Color.white;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.gameObject.SetActive(false);
      player.hudHearts.gameObject.SetActive(false);
    }
    YngyaRoomManager.Instance.SimpleSetCamera.Play();
    if (DataManager.Instance.YngyaHeartRoomEncounters == 1)
    {
      AudioManager.Instance.PlayMusic("event:/dlc/music/cutscene_yngyaheart_a/loop");
      yngyaRoomManager.loopingSfx = AudioManager.Instance.CreateLoop("event:/dlc/dungeon06/enemy/yngya/story_intro_amb_humming_loop", true);
    }
    else if (DataManager.Instance.YngyaHeartRoomEncounters == 2)
    {
      AudioManager.Instance.PlayMusic("event:/dlc/music/cutscene_yngyaheart_b/loop");
      yngyaRoomManager.loopingSfx = AudioManager.Instance.CreateLoop("event:/dlc/env/cutscene/yngyaheart_b/amb_loop", true);
    }
    yield return (object) new WaitForSeconds(2f);
    CanvasGroup[] quotes = yngyaRoomManager.quotes1;
    CanvasGroup[] widgets = yngyaRoomManager.widgets1;
    string[] quotesSFX = yngyaRoomManager.quotes1SFX;
    if (DataManager.Instance.YngyaHeartRoomEncounters == 2)
    {
      quotes = yngyaRoomManager.quotes2;
      widgets = yngyaRoomManager.widgets2;
      quotesSFX = yngyaRoomManager.quotes2SFX;
    }
    for (int i = 0; i < quotes.Length; ++i)
    {
      quotes[i].DOFade(1f, 1f);
      UIManager.PlayAudio(quotesSFX[i]);
      yield return (object) new WaitForSeconds(1f);
      widgets[i].DOFade(1f, 1f);
      while (!InputManager.Gameplay.GetAdvanceDialogueButtonDown())
        yield return (object) null;
      if (DataManager.Instance.YngyaHeartRoomEncounters == 2)
      {
        quotes[i].DOFade(0.0f, 0.0f);
        if (i < 2)
        {
          switch (i)
          {
            case 0:
              AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/yngyaheart_b/flashforward_01");
              AudioManager.Instance.PlayOneShot("event:/dlc/music/cutscene_yngyaheart_b/flashforward_01");
              break;
            case 1:
              AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/yngyaheart_b/flashforward_02");
              AudioManager.Instance.PlayOneShot("event:/dlc/music/cutscene_yngyaheart_b/flashforward_02");
              break;
          }
          yield return (object) yngyaRoomManager.StartCoroutine((IEnumerator) yngyaRoomManager.Flicker());
        }
        else
        {
          AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/yngyaheart_b/sacrifice_start");
          AudioManager.Instance.PlayOneShot("event:/dlc/music/cutscene_yngyaheart_b/sacrifice_start");
          foreach (Component ghost in yngyaRoomManager.ghosts)
            ghost.GetComponent<spineChangeAnimationSimple>().Play();
          DOTween.To(new DOGetter<Vector3>(yngyaRoomManager.\u003CPlayRoutine\u003Eb__32_0), new DOSetter<Vector3>(yngyaRoomManager.\u003CPlayRoutine\u003Eb__32_1), yngyaRoomManager.SimpleSetCamera.transform.position + yngyaRoomManager.SimpleSetCamera.transform.forward * -2f, 6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
          CameraManager.instance.ShakeCameraForDuration(0.25f, 0.35f, 10f);
          yield return (object) new WaitForSeconds(2.33333325f);
          BiomeConstants.Instance.ImpactFrameForDuration(0.3f);
          CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.3f);
          yield return (object) new WaitForSeconds(0.3f);
          yngyaRoomManager.RotScene.gameObject.SetActive(true);
          yngyaRoomManager.HolyScene.gameObject.SetActive(false);
          yngyaRoomManager.Spine.AnimationState.SetAnimation(0, "heart", true);
          yield return (object) new WaitForSeconds(1f);
        }
      }
      else
      {
        quotes[i].DOFade(0.0f, 1f);
        yield return (object) new WaitForSeconds(2f);
      }
    }
    if (DataManager.Instance.YngyaHeartRoomEncounters == 1)
      yield return (object) DLCMap.RevealHeartRoutine();
    AudioManager.Instance.StopLoop(yngyaRoomManager.loopingSfx);
    yngyaRoomManager.StartCoroutine((IEnumerator) yngyaRoomManager.ConversationCompleted());
  }

  public void Update()
  {
    if (!this.playing)
      return;
    for (int index = 0; index < Demon_Arrows.Demons.Count; ++index)
      Demon_Arrows.Demons[index].gameObject.SetActive(false);
    for (int index = 0; index < Familiar.Familiars.Count; ++index)
      Familiar.Familiars[index].gameObject.SetActive(false);
  }

  public void OnDestroy() => AudioManager.Instance.StopLoop(this.loopingSfx);

  public IEnumerator Flicker()
  {
    this.RotScene.gameObject.SetActive(true);
    this.HolyScene.gameObject.SetActive(false);
    BiomeConstants.Instance.ImpactFrameForDuration(0.1f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.3f);
    this.Spine.AnimationState.SetAnimation(0, "heart", true);
    yield return (object) new WaitForSeconds(0.6f);
    BiomeConstants.Instance.ImpactFrameForDuration(0.1f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.1f);
    this.RotScene.gameObject.SetActive(false);
    this.HolyScene.gameObject.SetActive(true);
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

  public static bool ShowHeartRoom()
  {
    return !DataManager.Instance.RevealedDLCMapHeart || DataManager.Instance.TotalShrineGhostJuice >= 50 && DataManager.Instance.YngyaHeartRoomEncounters == 1;
  }

  [CompilerGenerated]
  public Vector3 \u003CPlayRoutine\u003Eb__32_0() => this.SimpleSetCamera.transform.position;

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__32_1(Vector3 x)
  {
    this.SimpleSetCamera.transform.position = x;
  }
}
