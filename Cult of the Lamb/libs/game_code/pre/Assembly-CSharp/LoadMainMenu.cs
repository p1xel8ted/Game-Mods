// Decompiled with JetBrains decompiler
// Type: LoadMainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#nullable disable
public class LoadMainMenu : MonoBehaviour
{
  private AsyncOperationHandle<SceneInstance> s;
  private Scene scene;
  public SkeletonGraphic spine;
  public Material spineMaterial;
  public GameObject Loading;
  public LoadingIcon icon;
  public GameObject ControllerRecommended;

  private void Awake()
  {
  }

  private void Start()
  {
    AudioManager.Instance.enabled = true;
    this.StartCoroutine((IEnumerator) this.RunSplashScreens());
    this.spineMaterial = this.spine.material;
    this.spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.spine.gameObject.SetActive(false);
    KeyboardLightingManager.PlayVideo("Rainbow");
  }

  private IEnumerator RunSplashScreens()
  {
    LoadMainMenu loadMainMenu = this;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(2f);
    loadMainMenu.spine.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/music/mm_pre_roll/mm_pre_roll");
    yield return (object) loadMainMenu.spine.YieldForAnimation("animation");
    KeyboardLightingManager.StopVideo();
    KeyboardLightingManager.PlayVideo();
    loadMainMenu.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(loadMainMenu.HandleEvent);
    bool PlayingDevolverSplash = true;
    MMVideoPlayer.Play("Devolver_Animated_Logo_v001_4k_Audio-_HQ_HBR_MP4", (System.Action) (() => PlayingDevolverSplash = false), MMVideoPlayer.Options.DISABLE, MMVideoPlayer.Options.DISABLE);
    AudioManager.Instance.PlayOneShot("event:/music/devolver_pre_roll/DevolverSplash");
    while (PlayingDevolverSplash)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    KeyboardLightingManager.StopVideo();
    if ((UnityEngine.Object) loadMainMenu.ControllerRecommended != (UnityEngine.Object) null)
      loadMainMenu.ControllerRecommended.SetActive(true);
    yield return (object) new WaitForSeconds(3f);
    yield return (object) new WaitForEndOfFrame();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) null);
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    Debug.Log((object) "Handle Event");
    this.spineMaterial.DOFloat(1f, "_RainbowLerp", 0.0f);
    this.spineMaterial.DOFloat(0.0f, "_RainbowLerp", 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuart);
  }

  private IEnumerator EnableMainMenu()
  {
    while (true)
    {
      if (this.s.IsDone)
      {
        SceneManager.UnloadSceneAsync(this.scene);
        SceneManager.SetActiveScene(this.s.Result.Scene);
      }
      yield return (object) null;
    }
  }
}
