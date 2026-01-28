// Decompiled with JetBrains decompiler
// Type: MMTools.MMTransition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using Map;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
namespace MMTools;

public class MMTransition : MonoBehaviour
{
  public static MMTransition Instance;
  public static bool ForceShowIcon = false;
  public List<MMTransition.ImageAndAlpha> Images = new List<MMTransition.ImageAndAlpha>();
  public static bool IsPlaying = false;
  public static bool CanResume = true;
  public static MMTransition mmTransition;
  public string SceneToLoad;
  public float Duration;
  public System.Action CallBack;
  public System.Action OnSceneLoadedCallBack;
  public bool WaitForCallback;
  public LoadingIcon loadingIcon;
  public TextMeshProUGUI LoadingText;
  public GoopFade goopFade;
  public Image pentagramImage;
  public List<Sprite> pentagramImages = new List<Sprite>();
  public List<Sprite> pentagramImagesSOTF = new List<Sprite>();
  public List<Sprite> pentagramImagesDLC1 = new List<Sprite>();
  [SerializeField]
  public Material pentagramMaterialNormal;
  [SerializeField]
  public Material pentagramMaterialCoOp;
  [SerializeField]
  public Material pentagramMaterialDLC1;
  [SerializeField]
  public Image _crownImage;
  [SerializeField]
  public Image _crownEyeFillImage;
  [SerializeField]
  public Material _crownNormalMaterial;
  [SerializeField]
  public Material _crownCoOpMaterial;
  [SerializeField]
  public Material _crownDLC1Material;
  [SerializeField]
  public Image _loadingBarBG;
  [SerializeField]
  public Material _loadingBarBGNormalMaterial;
  [SerializeField]
  public Material _loadingBarBGCoOpMaterial;
  [SerializeField]
  public Material _loadingBarBGDLC1Material;
  public IEnumerator currentTransition;
  [SerializeField]
  public List<MMTransition.TransitionEffects> Effects = new List<MMTransition.TransitionEffects>();
  public EventInstance audioMuteSnapshotInstance;
  public string muteOnLoadingScreenSnapshot = "snapshot:/mute_on_loading_screen";
  public HashSet<string> WinterLoadingColorScenes = new HashSet<string>()
  {
    "Dungeon5"
  };
  public HashSet<string> RotLoadingColorScenes = new HashSet<string>()
  {
    "Dungeon6"
  };
  public static float Speed;
  public static string NO_SCENE = "-1";
  public string Title;
  public static System.Action OnBeginTransition;
  public MMTransition.Effect CurrentEffect;
  public AsyncOperationHandle<SceneInstance> asyncLoad;
  public Coroutine cFadeOut;
  public AsyncOperationHandle<SceneInstance> BufferAsyncLoad;
  public Dictionary<string, int> lastMenuObjects;
  public static System.Action OnTransitionCompelte;
  public static string EventSent;

  public void SetLoadingScreenColors()
  {
    if (this.RotLoadingColorScenes.Contains(this.SceneToLoad))
    {
      this.pentagramImage.material = this.pentagramMaterialNormal;
      this._crownEyeFillImage.color = StaticColors.RedColor;
      this.LoadingText.color = StaticColors.RedColor;
      this._crownImage.material = this._crownNormalMaterial;
      this._loadingBarBG.material = this._loadingBarBGNormalMaterial;
    }
    else if (this.WinterLoadingColorScenes.Contains(this.SceneToLoad) || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      this.LoadingText.color = StaticColors.DLC1Blue;
      this._crownEyeFillImage.color = StaticColors.DLC1Blue;
      this._crownImage.material = this._crownDLC1Material;
      this._loadingBarBG.material = this._loadingBarBGDLC1Material;
      this.pentagramImage.material = this.pentagramMaterialDLC1;
    }
    else if (PlayerFarming.players.Count > 1)
    {
      this.LoadingText.color = StaticColors.GoatPurple;
      this._crownEyeFillImage.color = StaticColors.GoatPurple;
      this._crownImage.material = this._crownCoOpMaterial;
      this._loadingBarBG.material = this._loadingBarBGCoOpMaterial;
      this.pentagramImage.material = this.pentagramMaterialCoOp;
    }
    else
    {
      this.pentagramImage.material = this.pentagramMaterialNormal;
      this._crownEyeFillImage.color = StaticColors.RedColor;
      this.LoadingText.color = StaticColors.RedColor;
      this._crownImage.material = this._crownNormalMaterial;
      this._loadingBarBG.material = this._loadingBarBGNormalMaterial;
    }
  }

  public void Start() => UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);

  public void OnEnable()
  {
    this.LoadingText.text = "";
    this.loadingIcon.gameObject.SetActive(false);
    this.pentagramImage.enabled = false;
  }

  public static void Play(
    MMTransition.TransitionType transitionType,
    MMTransition.Effect effect,
    string SceneToLoad,
    float Duration,
    string Title,
    System.Action CallBack,
    System.Action OnSceneLoadedCallBack = null)
  {
    System.Action onBeginTransition = MMTransition.OnBeginTransition;
    if (onBeginTransition != null)
      onBeginTransition();
    if ((UnityEngine.Object) MMTransition.Instance == (UnityEngine.Object) null)
    {
      MMTransition.Instance = UnityEngine.Object.Instantiate<MMTransition>(UnityEngine.Resources.Load<MMTransition>("MMTransition/Transition"));
      MMTransition.mmTransition = MMTransition.Instance;
    }
    else
      MMTransition.Instance.gameObject.SetActive(true);
    MMTransition.mmTransition.Show(transitionType, effect, SceneToLoad, Duration, Title, CallBack, OnSceneLoadedCallBack);
  }

  public static void StopCurrentTransition()
  {
    if ((UnityEngine.Object) MMTransition.mmTransition == (UnityEngine.Object) null)
      return;
    if (MMTransition.mmTransition.currentTransition != null)
    {
      MMTransition.mmTransition.StopCoroutine((IEnumerator) MMTransition.mmTransition.currentTransition);
      SimulationManager.UnPause();
      MMTransition.IsPlaying = false;
    }
    if (MMTransition.mmTransition.cFadeOut == null)
      return;
    MMTransition.mmTransition.StopCoroutine(MMTransition.mmTransition.cFadeOut);
    MMTransition.mmTransition.cFadeOut = (Coroutine) null;
  }

  public void Show(
    MMTransition.TransitionType transitionType,
    MMTransition.Effect effect,
    string SceneToLoad,
    float Duration,
    string Title,
    System.Action CallBack,
    System.Action OnSceneLoadedCallBack)
  {
    if (MMTransition.IsPlaying)
    {
      if (CallBack == null)
        return;
      CallBack();
    }
    else
    {
      MMVibrate.StopRumble();
      this.SceneToLoad = SceneToLoad;
      this.SetLoadingScreenColors();
      this.Duration = Duration;
      this.CallBack = CallBack;
      this.OnSceneLoadedCallBack = OnSceneLoadedCallBack;
      this.CurrentEffect = effect;
      this.Title = Title;
      SimulationManager.Pause();
      MMTransition.IsPlaying = true;
      this.currentTransition = (IEnumerator) null;
      this.audioMuteSnapshotInstance = AudioManager.Instance.PlayOneShotWithInstance(this.muteOnLoadingScreenSnapshot);
      if (SceneToLoad == "MAIN MENU")
        Debug.Log((object) ("MAIN MENU: " + transitionType.ToString()));
      Debug.Log((object) ("transitionType: ".Colour(Color.green) + transitionType.ToString()));
      switch (transitionType)
      {
        case MMTransition.TransitionType.ChangeSceneAutoResume:
          this.currentTransition = this.ChangeSceneAutoResumeRoutine();
          break;
        case MMTransition.TransitionType.ChangeRoomWaitToResume:
          this.currentTransition = this.ChangeRoomWaitToResumeRoutine();
          break;
        case MMTransition.TransitionType.ChangeRoom:
          this.currentTransition = this.ChangeRoomRoutine();
          break;
        case MMTransition.TransitionType.FadeAndCallBack:
          this.currentTransition = this.FadeAndCallBackRoutine();
          break;
        case MMTransition.TransitionType.LoadAndFadeOut:
          this.currentTransition = this.LoadAndFadeOutRoutine();
          break;
      }
      Debug.Log((object) "A".Colour(Color.green));
      if (this.currentTransition == null)
        return;
      this.StartCoroutine((IEnumerator) this.currentTransition);
    }
  }

  public IEnumerator ChangeSceneAutoResumeRoutine()
  {
    MMTransition mmTransition = this;
    if (MMTransition.ForceShowIcon)
      mmTransition.loadingIcon.gameObject.SetActive(true);
    yield return (object) mmTransition.StartCoroutine((IEnumerator) mmTransition.FadeInRoutine());
    Time.timeScale = 0.0f;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    System.Action callBack = mmTransition.CallBack;
    if (callBack != null)
      callBack();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if (mmTransition.SceneToLoad != MMTransition.NO_SCENE)
      yield return (object) mmTransition.StartCoroutine((IEnumerator) mmTransition.LoadScene());
    if (mmTransition.SceneToLoad == "MAIN MENU")
      Debug.Log((object) "MAIN MENU LOADED");
    MMTransition.ResumePlay();
    yield return (object) UnityEngine.Resources.UnloadUnusedAssets();
  }

  public IEnumerator ChangeRoomWaitToResumeRoutine()
  {
    MMTransition mmTransition = this;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      Debug.Log((object) ("Current Playerfarming location is Dungeon?: " + LocationManager.LocationIsDungeon(PlayerFarming.Location).ToString()));
      Debug.Log((object) ("Current Playerfarming location : " + PlayerFarming.Location.ToString()));
      if (!LocationManager.LocationIsDungeon(PlayerFarming.Location))
        Debug.Log((object) "Current Playerfarming Destroying lamb instance!");
    }
    else
      Debug.Log((object) "Current Playerfarming No player faming instance to destroy");
    yield return (object) mmTransition.StartCoroutine((IEnumerator) mmTransition.FadeInRoutine());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    mmTransition.loadingIcon.gameObject.SetActive(true);
    mmTransition.pentagramImage.enabled = true;
    mmTransition.pentagramImage.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(mmTransition.pentagramImage, 1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    List<Sprite> spriteList = new List<Sprite>((IEnumerable<Sprite>) mmTransition.pentagramImages);
    if (DataManager.Instance.PleasureEnabled)
      spriteList.AddRange((IEnumerable<Sprite>) mmTransition.pentagramImagesSOTF);
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      spriteList.AddRange((IEnumerable<Sprite>) mmTransition.pentagramImagesDLC1);
    mmTransition.pentagramImage.sprite = spriteList[UnityEngine.Random.Range(0, spriteList.Count)];
    mmTransition.pentagramImage.transform.localScale = Vector3.one;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    System.Action callBack = mmTransition.CallBack;
    if (callBack != null)
      callBack();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if (mmTransition.SceneToLoad != MMTransition.NO_SCENE)
      yield return (object) mmTransition.StartCoroutine((IEnumerator) mmTransition.LoadScene());
  }

  public IEnumerator ChangeRoomRoutine()
  {
    MMTransition mmTransition = this;
    mmTransition.StartCoroutine((IEnumerator) mmTransition.FadeInRoutine());
    yield return (object) new WaitForSecondsRealtime(0.3f * mmTransition.Duration);
    Time.timeScale = 0.0f;
    yield return (object) new WaitForSecondsRealtime(0.4f * mmTransition.Duration);
    System.Action callBack = mmTransition.CallBack;
    if (callBack != null)
      callBack();
  }

  public IEnumerator FadeAndCallBackRoutine()
  {
    yield return (object) this.FadeInRoutine();
    System.Action callBack = this.CallBack;
    if (callBack != null)
      callBack();
  }

  public IEnumerator LoadAndFadeOutRoutine()
  {
    if (this.SceneToLoad != MMTransition.NO_SCENE)
    {
      this.loadingIcon.gameObject.SetActive(true);
      this.pentagramImage.enabled = true;
      this.pentagramImage.color = new Color(1f, 1f, 1f, 0.0f);
      DOTweenModuleUI.DOFade(this.pentagramImage, 1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.pentagramImage.sprite = this.pentagramImages[UnityEngine.Random.Range(0, this.pentagramImages.Count)];
      this.pentagramImage.transform.localScale = Vector3.one;
      yield return (object) this.LoadScene();
      yield return (object) this.FadeOutRoutine();
    }
  }

  public IEnumerator LoadScene()
  {
    MMTransition mmTransition = this;
    Debug.Log((object) ("Fading Loading scene " + mmTransition.SceneToLoad));
    foreach (MMTransition.ImageAndAlpha image in mmTransition.Images)
    {
      image.Image.color = mmTransition.CurrentEffect == MMTransition.Effect.WhiteFade ? Color.white : Color.black;
      image.Alpha = 1f;
      yield return (object) null;
    }
    Debug.Log((object) "###WaitWhile  UIManager.Instance == null");
    yield return (object) new WaitWhile((Func<bool>) (() => (UnityEngine.Object) MonoSingleton<UIManager>.Instance == (UnityEngine.Object) null));
    Debug.Log((object) "###WaitWhile  UIManager.Instance == null Complete");
    Scene sceneToUnload = SceneManager.GetActiveScene();
    Debug.Log((object) ("###sceneToUnload " + sceneToUnload.name));
    switch (sceneToUnload.name)
    {
      case "Base Biome 1":
      case "Woolhaven Intro":
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        break;
      case "Dungeon Boss Wolf":
      case "Dungeon Boss Yngya":
      case "Dungeon Final":
      case "Dungeon Sandbox":
      case "Dungeon1":
      case "Dungeon2":
      case "Dungeon3":
      case "Dungeon4":
      case "Dungeon5":
      case "Dungeon6":
      case "Game Biome Intro":
        MonoSingleton<UIManager>.Instance.UnloadDungeonAssets();
        break;
      case "Dungeon Ratau Home":
        MonoSingleton<UIManager>.Instance.UnlockRatauShackAssets();
        break;
      case "Hub-Shore":
        MonoSingleton<UIManager>.Instance.UnloadHubShoreAssets();
        break;
      case "Main Menu":
        MonoSingleton<UIManager>.Instance.UnloadMainMenuAssets();
        yield return (object) MonoSingleton<UIManager>.Instance.LoadPersistentGameAssets().YieldUntilCompleted();
        break;
    }
    yield return (object) null;
    Debug.Log((object) ("###sceneToUnloaded " + sceneToUnload.name));
    Debug.Log((object) "###BufferAsyncLoad");
    mmTransition.BufferAsyncLoad = Addressables.LoadSceneAsync((object) "Assets/Scenes/BufferScene.unity");
    while (!mmTransition.BufferAsyncLoad.IsDone)
    {
      MMTransition.UpdateProgress(mmTransition.BufferAsyncLoad.PercentComplete);
      yield return (object) null;
    }
    yield return (object) null;
    Debug.Log((object) "###BufferAsyncLoaded");
    MapGenerator.Clear();
    MapConfig.Clear();
    ObjectPool.DestroyAll();
    BlackSoulUpdater.Instance.Clear();
    AudioManager.Instance.ReleaseInstances();
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.UnloadFlockadePiecesAssets();
    yield return (object) null;
    yield return (object) mmTransition.StartCoroutine((IEnumerator) ObjectPool.PoolReset());
    yield return (object) null;
    yield return (object) UnityEngine.Resources.UnloadUnusedAssets();
    yield return (object) null;
    Debug.Log((object) ("###SceneToLoad" + mmTransition.SceneToLoad));
    switch (mmTransition.SceneToLoad)
    {
      case "Base Biome 1":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadBaseAssets().YieldUntilCompleted();
        break;
      case "Dungeon Boss Wolf":
      case "Dungeon Boss Yngya":
      case "Dungeon Final":
      case "Dungeon Sandbox":
      case "Dungeon1":
      case "Dungeon2":
      case "Dungeon3":
      case "Dungeon4":
      case "Dungeon5":
      case "Dungeon6":
      case "Game Biome Intro":
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        yield return (object) null;
        yield return (object) MonoSingleton<UIManager>.Instance.LoadDungeonAssets().YieldUntilCompleted();
        break;
      case "Dungeon Ratau Home":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadRatauShackAssets().YieldUntilCompleted();
        break;
      case "Hub-Shore":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadHubShoreAssets().YieldUntilCompleted();
        break;
      case "Main Menu":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadMainMenuAssets().YieldUntilCompleted();
        MonoSingleton<UIManager>.Instance.UnloadPersistentGameAssets();
        break;
      case "Woolhaven Intro":
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        break;
      default:
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        break;
    }
    yield return (object) null;
    mmTransition.asyncLoad = Addressables.LoadSceneAsync((object) $"Assets/Scenes/{mmTransition.SceneToLoad}.unity");
    MMTransition.UpdateProgress(mmTransition.asyncLoad.PercentComplete);
    while (!mmTransition.asyncLoad.IsDone)
    {
      MMTransition.UpdateProgress(mmTransition.asyncLoad.PercentComplete);
      yield return (object) null;
    }
    System.Action sceneLoadedCallBack = mmTransition.OnSceneLoadedCallBack;
    if (sceneLoadedCallBack != null)
      sceneLoadedCallBack();
  }

  public static void ForceStopMMTransition()
  {
    MMTransition.mmTransition.StopAllCoroutines();
    foreach (MMTransition.ImageAndAlpha image in MMTransition.mmTransition.Images)
      image.Alpha = 0.0f;
    MMTransition.mmTransition.loadingIcon.gameObject.SetActive(false);
    MMTransition.mmTransition.LoadingText.text = "";
    MMTransition.mmTransition.pentagramImage.enabled = false;
    MMTransition.IsPlaying = false;
  }

  public static void ResumePlay() => MMTransition.ResumePlay((System.Action) null);

  public static void ResumePlay(System.Action andThen, bool resumeSimulation = true, bool resumeTimeScale = true)
  {
    if (MMTransition.ForceShowIcon)
      MMTransition.ForceShowIcon = false;
    if (!MMTransition.IsPlaying || !MMTransition.CanResume)
      return;
    if (MMTransition.mmTransition.cFadeOut == null)
      MMTransition.mmTransition.cFadeOut = MMTransition.mmTransition.StartCoroutine((IEnumerator) MMTransition.mmTransition.FadeOutRoutine(andThen, resumeSimulation, resumeTimeScale));
    CoopManager.HandleSignOutDuringTransistions();
  }

  public float FadeIn()
  {
    if ((UnityEngine.Object) MMTransition.Instance != (UnityEngine.Object) null)
    {
      MMTransition.Instance.gameObject.SetActive(true);
      this.StartCoroutine((IEnumerator) this.FadeInRoutine());
    }
    return this.Duration;
  }

  public void FadeOutInstant()
  {
    if (!((UnityEngine.Object) MMTransition.Instance != (UnityEngine.Object) null))
      return;
    MMTransition.Instance.gameObject.SetActive(false);
  }

  public IEnumerator FadeInRoutine()
  {
    foreach (MMTransition.ImageAndAlpha image in this.Images)
      image.Image.color = this.CurrentEffect == MMTransition.Effect.WhiteFade ? Color.white : Color.black;
    float progress = 0.0f;
    while ((double) (progress += Time.unscaledDeltaTime) / (double) this.Duration <= 0.5)
    {
      foreach (MMTransition.ImageAndAlpha image in this.Images)
        image.Alpha = Mathf.SmoothStep(0.0f, 1f, progress / (this.Duration * 0.5f));
      yield return (object) null;
    }
  }

  public IEnumerator FadeOutRoutine(System.Action andThen = null, bool resumeSimulation = true, bool resumeTimeScale = true)
  {
    System.Action transitionCompelte = MMTransition.OnTransitionCompelte;
    if (transitionCompelte != null)
      transitionCompelte();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    MMTransition.mmTransition.loadingIcon.gameObject.SetActive(false);
    this.LoadingText.text = "";
    this.pentagramImage.enabled = false;
    AudioManager.Instance.StopOneShotInstanceEarly(this.audioMuteSnapshotInstance, STOP_MODE.ALLOWFADEOUT);
    float FadeProgress = 0.0f;
    bool ResetTimeScale = false;
    while ((double) FadeProgress / (double) this.Duration <= 0.5)
    {
      FadeProgress += Time.unscaledDeltaTime;
      foreach (MMTransition.ImageAndAlpha image in this.Images)
        image.Alpha = Mathf.SmoothStep(1f, 0.0f, FadeProgress / (this.Duration * 0.5f));
      if (!ResetTimeScale & resumeTimeScale && (double) FadeProgress / (double) this.Duration > 0.20000000298023224)
      {
        ResetTimeScale = true;
        Time.timeScale = 1f;
      }
      yield return (object) null;
    }
    if (!ResetTimeScale & resumeTimeScale)
      Time.timeScale = 1f;
    if (resumeSimulation)
      SimulationManager.UnPause();
    MMTransition.IsPlaying = false;
    MMTransition.Instance.gameObject.SetActive(false);
    MMTransition.mmTransition.cFadeOut = (Coroutine) null;
    System.Action action = andThen;
    if (action != null)
      action();
    CoopManager.HandleSignOutDuringTransistions();
  }

  public static void UpdateProgress(float Progress, string ProgressText = "")
  {
    if ((UnityEngine.Object) MMTransition.mmTransition == (UnityEngine.Object) null)
      return;
    MMTransition.mmTransition.loadingIcon.UpdateProgress(Progress);
    MMTransition.mmTransition.LoadingText.text = ProgressText;
  }

  public static async System.Threading.Tasks.Task FadeIn(float duration)
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, duration, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      await System.Threading.Tasks.Task.Yield();
  }

  public static async System.Threading.Tasks.Task FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      await System.Threading.Tasks.Task.Yield();
  }

  public enum Effect
  {
    BlackFade,
    BlackWipe,
    BlackFadeInOnly,
    WhiteFade,
    GoopFade,
  }

  [Serializable]
  public class ImageAndAlpha
  {
    public Image Image;
    public Color color;

    public float Alpha
    {
      set
      {
        this.color = this.Image.color;
        this.color.a = value;
        this.Image.color = this.color;
      }
    }
  }

  [Serializable]
  public class TransitionEffects
  {
    public MMTransition.Effect type;
    public AnimationClip animation;
  }

  public enum TransitionType
  {
    ChangeSceneAutoResume,
    ChangeRoomWaitToResume,
    ChangeRoom,
    FadeAndCallBack,
    LoadAndFadeOut,
  }
}
