// Decompiled with JetBrains decompiler
// Type: DLCIntroManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

#nullable disable
public class DLCIntroManager : BaseMonoBehaviour
{
  public static DLCIntroManager Instance;
  public DungeonLocationManager DungeonLocationManager;
  public BiomeGenerator BiomeGenerator;
  [SerializeField]
  public GameObject gamePrefab;
  public GameObject distortionObject;
  public int WeatherPhase;
  [SerializeField]
  public Image whiteOverlay;
  [SerializeField]
  public GameObject YngyaTease;
  [SerializeField]
  public GameObject LogoSplash;
  [SerializeField]
  public Camera camA;
  [SerializeField]
  public Camera camB;
  [SerializeField]
  public GameObject quoteScreenObj;
  [SerializeField]
  public QuoteScreenController quoteScreenController;
  [SerializeField]
  public CanvasGroup _quoteCanvasGroup;
  [SerializeField]
  public Image _fadeOut;
  [SerializeField]
  public PostProcessVolume postProcessVolume;
  [SerializeField]
  public DepthOfField depthOfField;
  [SerializeField]
  public float depthOfFieldValue;
  [SerializeField]
  public Palette winterLUT;
  [SerializeField]
  public Palette quoteLUT;
  public static System.Action QuoteCallback;
  public EventInstance bassloop;
  public string musicLoopSFX = "event:/dlc/music/cutscene_01/02_loop";
  public string musicQuoteOneSFX = "event:/dlc/music/cutscene_01/01_sting_yngya_quote_01";
  public string logoAppearsStingSFX = "event:/dlc/music/cutscene_01/11_sting_woolhaven_logo";
  public string openingAmbienceLoopSFX = "event:/dlc/env/cutscene/01/blizzard_amb_loop";
  public string stingerLambWalksSFX = "event:/dlc/music/cutscene_01/03_sting_lamb_walks";
  public string mountainTopStingSFX = "event:/dlc/music/cutscene_01/10_sting_mountaintop";
  public string distanceToYngyaParam = "distanceToYngya";
  public bool hasLogoAppeared;
  public bool hasLambStartedMoving;
  public bool biomeGenerated;
  public EventInstance blizzardAtmos;
  public Vector3 playerStartPos;
  public Vector3 mountainTopTriggerPos;

  public void Start()
  {
    this.distortionObject.gameObject.SetActive(false);
    GameManager.NewRun("", false);
    this.postProcessVolume.profile.TryGetSettings<DepthOfField>(out this.depthOfField);
    DOTweenModuleUI.DOFade(this.whiteOverlay, 0.0f, 0.0f);
    this.YngyaTease.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) CoopManager.Instance)
      CoopManager.Instance.UnlockAddRemovePlayer();
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    AudioManager.Instance.StopLoop(this.blizzardAtmos);
  }

  public void OnEnable()
  {
    DLCIntroManager.Instance = this;
    BiomeGenerator.OnBiomeGenerated += new BiomeGenerator.BiomeAction(this.OnGenerated);
    this.StartCoroutine((IEnumerator) this.WakeUpInBaseRoutine());
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) DLCIntroManager.Instance == (UnityEngine.Object) this)
      DLCIntroManager.Instance = (DLCIntroManager) null;
    BiomeGenerator.OnBiomeGenerated -= new BiomeGenerator.BiomeAction(this.OnGenerated);
  }

  public void OnGenerated()
  {
    this.BiomeGenerator.DoFirstArrivalRoutine = true;
    DataManager.Instance.dungeonRunDuration = Time.time;
    WeatherSystemController.Instance.HideWeather();
    BiomeGenerator.OnBiomeGenerated -= new BiomeGenerator.BiomeAction(this.OnGenerated);
    GameManager.setDefaultGlobalShaders();
    HUD_Manager.Instance.Hide(true, 0);
    this.StartCoroutine((IEnumerator) this.StopMusic());
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme);
    Shader.SetGlobalFloat("_Snow_Intensity", 1f);
    CameraTrackedFloorRT_Manager.Instance.EnableEffect();
    RoomLockController.RoomCompleted();
    this.biomeGenerated = true;
  }

  public IEnumerator WakeUpInBaseRoutine()
  {
    DLCIntroManager dlcIntroManager = this;
    while ((UnityEngine.Object) MonoSingleton<UIManager>.Instance == (UnityEngine.Object) null && (UnityEngine.Object) AudioManager.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    while (!dlcIntroManager.biomeGenerated)
      yield return (object) null;
    dlcIntroManager.blizzardAtmos = AudioManager.Instance.CreateLoop(dlcIntroManager.openingAmbienceLoopSFX, true);
    GameObject gameObject1 = GameObject.Find("Player Start");
    GameObject gameObject2 = GameObject.Find("MountainTopTrigger");
    dlcIntroManager.playerStartPos = gameObject1.transform.position;
    dlcIntroManager.mountainTopTriggerPos = gameObject2.transform.position;
    float distanceToYngyaNorm = dlcIntroManager.CalculateDistanceToYngyaNorm();
    AudioManager.Instance.SetEventInstanceParameter(dlcIntroManager.blizzardAtmos, dlcIntroManager.distanceToYngyaParam, distanceToYngyaNorm);
    LetterBox.Show(true);
    CameraManager.instance.CameraRef.GetComponent<Stylizer>().enabled = true;
    dlcIntroManager.camA.gameObject.SetActive(false);
    dlcIntroManager.camB.gameObject.SetActive(true);
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
    CoopManager.HideCoopPlayerTemporarily();
    if ((bool) (UnityEngine.Object) CoopManager.Instance)
      CoopManager.Instance.LockAddRemovePlayer();
    DOTweenModuleUI.DOFade(dlcIntroManager._fadeOut, 0.0f, 1f);
    yield return (object) new WaitForSeconds(0.5f);
    List<QuoteScreenController.QuoteTypes> QuoteType = new List<QuoteScreenController.QuoteTypes>();
    QuoteType.Add(QuoteScreenController.QuoteTypes.DLC_Intro);
    QuoteScreenController.CURRENT_QUOTE_INDEX = 0;
    QuoteScreenController.SFX = " ";
    AudioManager.Instance.PlayOneShot(dlcIntroManager.musicQuoteOneSFX);
    QuoteScreenController.Init(QuoteType, (System.Action) null, (System.Action) null);
    dlcIntroManager.quoteScreenController.enabled = false;
    yield return (object) null;
    dlcIntroManager.quoteScreenController.enabled = true;
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 5.0)
    {
      dlcIntroManager.depthOfField.aperture.value = Mathf.SmoothStep(0.0f, 25f, Progress / 5f);
      yield return (object) null;
    }
    dlcIntroManager.StartCoroutine((IEnumerator) dlcIntroManager.StartIntro());
  }

  public IEnumerator StartIntro()
  {
    this.YngyaTease.SetActive(true);
    this.YngyaTease.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
    yield return (object) new WaitForSeconds(1f);
    this.quoteScreenObj.SetActive(false);
    this.bassloop = AudioManager.Instance.CreateLoop("event:/music/intro/intro_bass", true);
    AudioManager.Instance.PlayMusic(this.musicLoopSFX);
    AudioManager.Instance.SetMusicParam(this.distanceToYngyaParam, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/relics/glitch");
    AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/01/whoosh_to_yngya");
    this.YngyaTease.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
    Vector3 position1 = this.camA.transform.position;
    this.camA.transform.position = new Vector3(position1.x, position1.y, -15f);
    this.camA.gameObject.SetActive(true);
    this.camB.gameObject.SetActive(false);
    Stylizer component = CameraManager.instance.CameraRef.GetComponent<Stylizer>();
    component.enabled = true;
    component.UseSecondPalette = true;
    component.Palette = this.quoteLUT;
    component.Palette2 = this.winterLUT;
    component.LerpPalette = 1f;
    GameManager.GetInstance().CameraSetZoom(15f);
    yield return (object) new WaitForSeconds(2f);
    WeatherSystemController.Instance.ShowWeather(false);
    this.YngyaTease.gameObject.GetComponent<CanvasGroup>().DOFade(0.0f, 0.5f);
    int num = (int) this.bassloop.stop(STOP_MODE.ALLOWFADEOUT);
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme);
    CameraTrackedFloorRT_Manager.Instance.EnableEffect();
    foreach (PlayerFarming player1 in PlayerFarming.players)
    {
      PlayerFarming player = player1;
      player.CustomAnimation("blizzard/run-up-called-center", true);
      player.Spine.timeScale = 1f;
      Vector3 position2 = player.transform.position;
      DOVirtual.Float(1f, 0.5f, 10f, (TweenCallback<float>) (x => player.Spine.timeScale = x)).SetUpdate<Tweener>(true);
    }
    CameraFollowTarget.Instance.targets = (List<CameraFollowTarget.Target>) null;
    DOVirtual.Float(15f, 9f, 5f, (TweenCallback<float>) (x => GameManager.GetInstance().CameraSetZoom(x))).SetUpdate<Tweener>(true).SetEase<Tweener>(Ease.InQuart);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 10f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.timeScale = 1f;
      player.CustomAnimation("dlc_intro/kneel-sequence", false);
      player.state.facingAngle = 270f;
    }
    if (CoopManager.CoopActive)
    {
      CoopManager.RemovePlayerFromMenu();
      CoopManager.Instance.LockAddRemovePlayer();
      yield return (object) new WaitForSeconds(2.5f);
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerController.WinterSpeedModifier = 0.82f;
      player.AllowDodging = false;
    }
    yield return (object) new WaitForSeconds(4.5f);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerController.SetSpecialMovingAnimations("blizzard/idle-called-center", "blizzard/run-up-called-center", "blizzard/run-down-called-center", "blizzard/run-called-center", "blizzard/run-up-diagonal-called-center", "blizzard/run-horizontal-called-center", StateMachine.State.Idle_Winter);
      player.playerController.OverrideBlizzardAnims = true;
      player.state.CURRENT_STATE = StateMachine.State.Idle_Winter;
    }
    GameManager.GetInstance().OnConversationEnd(false);
    DeviceLightingManager.StopAll();
    DeviceLightingManager.UpdateLocation();
  }

  public void DoMountainTopTrigger()
  {
    AudioManager.Instance.PlayOneShot(this.mountainTopStingSFX);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    foreach (PlayerFarming player1 in PlayerFarming.players)
    {
      PlayerFarming player = player1;
      player.CustomAnimation("blizzard/run-up", true);
      player.Spine.timeScale = 1f;
      Vector3 vector3 = player.transform.position + new Vector3(0.0f, 3f, 0.0f);
      player.transform.DOMoveY(vector3.y, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        player.CustomAnimation("Yngya_Calls/kneeled-up", false);
        player.Spine.timeScale = 1f;
      }));
    }
  }

  public void ShowLogo()
  {
    this.hasLogoAppeared = true;
    this.LogoSplash.gameObject.SetActive(true);
    this.quoteScreenObj.SetActive(false);
    this.YngyaTease.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/cutscene/01/woolhaven_logo");
    AudioManager.Instance.PlayMusic(this.logoAppearsStingSFX);
    CameraManager.instance.CameraRef.DOFieldOfView(30f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 2f);
    int num = (int) this.bassloop.start();
    this.UpdateDistanceToYngyaEventInstances(1f);
    PlayerFarming.Instance.CustomAnimation("final-boss/die2", false);
    GameManager.GetInstance().WaitForSeconds(5f, (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 1f, "", (System.Action) (() => SaveAndLoad.Save()), (System.Action) (() => AudioManager.Instance.StopOneShotInstanceEarly(this.bassloop, STOP_MODE.ALLOWFADEOUT)))));
  }

  public IEnumerator StopMusic()
  {
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
  }

  public void IncreaseWeather()
  {
    switch (++this.WeatherPhase)
    {
      case 2:
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
        break;
      case 3:
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
        break;
      case 4:
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
        break;
      case 5:
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
        break;
    }
  }

  public void Update()
  {
    Shader.SetGlobalFloat("_GlobalTimeUnscaled", Time.unscaledTime);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null && !HUD_Manager.Instance.Hidden)
      HUD_Manager.Instance.Hide(true);
    CoopManager.HideCoopPlayerTemporarily();
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || this.hasLogoAppeared)
      return;
    this.UpdateDistanceToYngyaEventInstances(this.CalculateDistanceToYngyaNorm());
    if (this.hasLambStartedMoving || (double) PlayerFarming.Instance.playerController.speed <= 0.0)
      return;
    this.hasLambStartedMoving = true;
    AudioManager.Instance.PlayOneShot(this.stingerLambWalksSFX);
  }

  public void UpdateDistanceToYngyaEventInstances(float value)
  {
    AudioManager.Instance.SetEventInstanceParameter(this.blizzardAtmos, this.distanceToYngyaParam, value);
    AudioManager.Instance.SetMusicParam(this.distanceToYngyaParam, value);
  }

  public float CalculateDistanceToYngyaNorm()
  {
    float num1 = 0.0f;
    float a = 0.0f;
    float b = Vector3.Distance(this.playerStartPos, this.mountainTopTriggerPos);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      float num2 = Vector3.Distance(PlayerFarming.Instance.transform.position, this.mountainTopTriggerPos);
      num1 = 1f - Mathf.InverseLerp(a, b, num2);
    }
    return Mathf.Clamp01(num1);
  }

  public void SetBlizzard()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerController.SetSpecialMovingAnimations("blizzard/idle-called-center", "blizzard/run-up-called-center", "blizzard/run-down-called-center", "blizzard/run-called-center", "blizzard/run-up-diagonal-called-center", "blizzard/run-horizontal-called-center", StateMachine.State.Idle_Winter);
      player.playerController.OverrideBlizzardAnims = true;
      player.state.CURRENT_STATE = StateMachine.State.Idle_Winter;
    }
  }

  public void PulseDisplacementObject()
  {
    if (this.distortionObject.gameObject.activeSelf)
    {
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DORestart();
      this.distortionObject.transform.DOPlay();
    }
    else
    {
      this.distortionObject.SetActive(true);
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.SetActive(false)));
    }
  }

  [CompilerGenerated]
  public void \u003CShowLogo\u003Eb__43_0()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 1f, "", (System.Action) (() => SaveAndLoad.Save()), (System.Action) (() => AudioManager.Instance.StopOneShotInstanceEarly(this.bassloop, STOP_MODE.ALLOWFADEOUT)));
  }

  [CompilerGenerated]
  public void \u003CShowLogo\u003Eb__43_2()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.bassloop, STOP_MODE.ALLOWFADEOUT);
  }

  [CompilerGenerated]
  public void \u003CPulseDisplacementObject\u003Eb__50_0()
  {
    this.distortionObject.SetActive(false);
  }
}
