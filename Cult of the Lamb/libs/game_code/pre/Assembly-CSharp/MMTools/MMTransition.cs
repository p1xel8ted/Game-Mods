// Decompiled with JetBrains decompiler
// Type: MMTools.MMTransition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
  public static GameObject Instance;
  public static bool ForceShowIcon = false;
  public List<MMTransition.ImageAndAlpha> Images = new List<MMTransition.ImageAndAlpha>();
  public static bool IsPlaying = false;
  private static MMTransition mmTransition;
  private string SceneToLoad;
  private float Duration;
  private System.Action CallBack;
  private bool WaitForCallback;
  public LoadingIcon loadingIcon;
  public TextMeshProUGUI LoadingText;
  public GoopFade goopFade;
  public Image pentagramImage;
  public List<Sprite> pentagramImages = new List<Sprite>();
  private IEnumerator currentTransition;
  [SerializeField]
  public List<MMTransition.TransitionEffects> Effects = new List<MMTransition.TransitionEffects>();
  private static float Speed;
  public static string NO_SCENE = "-1";
  public string Title;
  private MMTransition.Effect CurrentEffect;
  public AsyncOperationHandle<SceneInstance> asyncLoad;
  private Coroutine cFadeOut;
  private AsyncOperationHandle<SceneInstance> BufferAsyncLoad;
  private Dictionary<string, int> lastMenuObjects;
  public static System.Action OnTransitionCompelte;

  private void Start() => UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);

  private void OnEnable()
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
    System.Action CallBack)
  {
    if ((UnityEngine.Object) MMTransition.Instance == (UnityEngine.Object) null)
    {
      MMTransition.Instance = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("MMTransition/Transition")) as GameObject;
      MMTransition.mmTransition = MMTransition.Instance.GetComponent<MMTransition>();
    }
    else
      MMTransition.Instance.SetActive(true);
    MMTransition.mmTransition.Show(transitionType, effect, SceneToLoad, Duration, Title, CallBack);
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
    System.Action CallBack)
  {
    if (MMTransition.IsPlaying)
      return;
    MMVibrate.StopRumble();
    this.SceneToLoad = SceneToLoad;
    this.Duration = Duration;
    this.CallBack = CallBack;
    this.CurrentEffect = effect;
    this.Title = Title;
    SimulationManager.Pause();
    MMTransition.IsPlaying = true;
    this.currentTransition = (IEnumerator) null;
    if (SceneToLoad == "MAIN MENU")
      Debug.Log((object) ("MAIN MENU: " + transitionType.ToString()));
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
    if (this.currentTransition == null)
      return;
    this.StartCoroutine((IEnumerator) this.currentTransition);
  }

  private IEnumerator ChangeSceneAutoResumeRoutine()
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
    UnityEngine.Resources.UnloadUnusedAssets();
  }

  private IEnumerator ChangeRoomWaitToResumeRoutine()
  {
    MMTransition mmTransition = this;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) PlayerFarming.Instance.gameObject);
    yield return (object) mmTransition.StartCoroutine((IEnumerator) mmTransition.FadeInRoutine());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    mmTransition.loadingIcon.gameObject.SetActive(true);
    mmTransition.pentagramImage.enabled = true;
    mmTransition.pentagramImage.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(mmTransition.pentagramImage, 1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    mmTransition.pentagramImage.sprite = mmTransition.pentagramImages[UnityEngine.Random.Range(0, mmTransition.pentagramImages.Count)];
    int num = UnityEngine.Random.Range(0, 2);
    mmTransition.pentagramImage.transform.localScale = num == 1 ? new Vector3(1f, -1f, 1f) : Vector3.one;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    System.Action callBack = mmTransition.CallBack;
    if (callBack != null)
      callBack();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if (mmTransition.SceneToLoad != MMTransition.NO_SCENE)
      yield return (object) mmTransition.StartCoroutine((IEnumerator) mmTransition.LoadScene());
  }

  private IEnumerator ChangeRoomRoutine()
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

  private IEnumerator FadeAndCallBackRoutine()
  {
    yield return (object) this.FadeInRoutine();
    System.Action callBack = this.CallBack;
    if (callBack != null)
      callBack();
  }

  private IEnumerator LoadAndFadeOutRoutine()
  {
    if (this.SceneToLoad != MMTransition.NO_SCENE)
    {
      this.loadingIcon.gameObject.SetActive(true);
      this.pentagramImage.enabled = true;
      this.pentagramImage.color = new Color(1f, 1f, 1f, 0.0f);
      DOTweenModuleUI.DOFade(this.pentagramImage, 1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.pentagramImage.sprite = this.pentagramImages[UnityEngine.Random.Range(0, this.pentagramImages.Count)];
      this.pentagramImage.transform.localScale = UnityEngine.Random.Range(0, 2) == 1 ? new Vector3(1f, -1f, 1f) : Vector3.one;
      yield return (object) this.LoadScene();
      yield return (object) this.FadeOutRoutine();
    }
  }

  private IEnumerator LoadScene()
  {
    MMTransition mmTransition = this;
    foreach (MMTransition.ImageAndAlpha image in mmTransition.Images)
    {
      image.Image.color = mmTransition.CurrentEffect == MMTransition.Effect.WhiteFade ? Color.white : Color.black;
      image.Alpha = 1f;
      yield return (object) null;
    }
    switch (SceneManager.GetActiveScene().name)
    {
      case "Base Biome 1":
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        break;
      case "Dungeon Final":
      case "Dungeon Sandbox":
      case "Dungeon1":
      case "Dungeon2":
      case "Dungeon3":
      case "Dungeon4":
      case "Game Biome Intro":
        MonoSingleton<UIManager>.Instance.UnloadDungeonAssets();
        break;
      case "Dungeon Ratau Home":
        MonoSingleton<UIManager>.Instance.UnloadKnucklebonesAssets();
        break;
      case "Hub-Shore":
        MonoSingleton<UIManager>.Instance.UnloadHubShoreAssets();
        break;
      case "Main Menu":
        MonoSingleton<UIManager>.Instance.UnloadMainMenuAssets();
        yield return (object) MonoSingleton<UIManager>.Instance.LoadPersistentGameAssets().YieldUntilCompleted();
        break;
    }
    mmTransition.BufferAsyncLoad = Addressables.LoadSceneAsync((object) "Assets/Scenes/BufferScene.unity");
    while (!mmTransition.BufferAsyncLoad.IsDone)
    {
      MMTransition.UpdateProgress(mmTransition.BufferAsyncLoad.PercentComplete);
      yield return (object) null;
    }
    yield return (object) null;
    MapGenerator.Clear();
    MapConfig.Clear();
    ObjectPool.DestroyAll();
    yield return (object) null;
    yield return (object) mmTransition.StartCoroutine((IEnumerator) ObjectPool.PoolReset());
    yield return (object) null;
    UnityEngine.Resources.UnloadUnusedAssets();
    yield return (object) null;
    switch (mmTransition.SceneToLoad)
    {
      case "Base Biome 1":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadBaseAssets().YieldUntilCompleted();
        break;
      case "Dungeon Final":
      case "Dungeon Sandbox":
      case "Dungeon1":
      case "Dungeon2":
      case "Dungeon3":
      case "Dungeon4":
      case "Game Biome Intro":
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        yield return (object) MonoSingleton<UIManager>.Instance.LoadDungeonAssets().YieldUntilCompleted();
        break;
      case "Dungeon Ratau Home":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadKnucklebonesAssets().YieldUntilCompleted();
        break;
      case "Hub-Shore":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadHubShoreAssets().YieldUntilCompleted();
        break;
      case "Main Menu":
        yield return (object) MonoSingleton<UIManager>.Instance.LoadMainMenuAssets().YieldUntilCompleted();
        MonoSingleton<UIManager>.Instance.UnloadPersistentGameAssets();
        break;
      default:
        MonoSingleton<UIManager>.Instance.UnloadBaseAssets();
        break;
    }
    mmTransition.asyncLoad = Addressables.LoadSceneAsync((object) $"Assets/Scenes/{mmTransition.SceneToLoad}.unity");
    MMTransition.UpdateProgress(mmTransition.asyncLoad.PercentComplete);
    while (!mmTransition.asyncLoad.IsDone)
    {
      MMTransition.UpdateProgress(mmTransition.asyncLoad.PercentComplete);
      yield return (object) null;
    }
  }

  public static void ResumePlay() => MMTransition.ResumePlay((System.Action) null);

  public static void ResumePlay(System.Action andThen)
  {
    if (MMTransition.ForceShowIcon)
      MMTransition.ForceShowIcon = false;
    if (!MMTransition.IsPlaying || MMTransition.mmTransition.cFadeOut != null)
      return;
    MMTransition.mmTransition.cFadeOut = MMTransition.mmTransition.StartCoroutine((IEnumerator) MMTransition.mmTransition.FadeOutRoutine(andThen));
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

  public IEnumerator FadeOutRoutine(System.Action andThen = null)
  {
    System.Action transitionCompelte = MMTransition.OnTransitionCompelte;
    if (transitionCompelte != null)
      transitionCompelte();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    MMTransition.mmTransition.loadingIcon.gameObject.SetActive(false);
    this.LoadingText.text = "";
    this.pentagramImage.enabled = false;
    float FadeProgress = 0.0f;
    bool ResetTimeScale = false;
    while ((double) FadeProgress / (double) this.Duration <= 0.5)
    {
      FadeProgress += Time.unscaledDeltaTime;
      foreach (MMTransition.ImageAndAlpha image in this.Images)
        image.Alpha = Mathf.SmoothStep(1f, 0.0f, FadeProgress / (this.Duration * 0.5f));
      if (!ResetTimeScale && (double) FadeProgress / (double) this.Duration > 0.20000000298023224)
      {
        ResetTimeScale = true;
        Time.timeScale = 1f;
      }
      yield return (object) null;
    }
    if (!ResetTimeScale)
      Time.timeScale = 1f;
    SimulationManager.UnPause();
    MMTransition.IsPlaying = false;
    MMTransition.Instance.SetActive(false);
    MMTransition.mmTransition.cFadeOut = (Coroutine) null;
    System.Action action = andThen;
    if (action != null)
      action();
  }

  public static void UpdateProgress(float Progress, string ProgressText = "")
  {
    if ((UnityEngine.Object) MMTransition.mmTransition == (UnityEngine.Object) null)
      return;
    MMTransition.mmTransition.loadingIcon.UpdateProgress(Progress);
    MMTransition.mmTransition.LoadingText.text = ProgressText;
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
    private Color color;

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
