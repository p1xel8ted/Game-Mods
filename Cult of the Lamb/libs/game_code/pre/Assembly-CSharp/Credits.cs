// Decompiled with JetBrains decompiler
// Type: Credits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class Credits : BaseMonoBehaviour
{
  public static bool GoToMainMenu;
  [SerializeField]
  private RectTransform _canvasRectTransform;
  [SerializeField]
  private float _skipSpeed = 4f;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private RectTransform _contentTransform;
  private EventInstance loopedSound;

  private void Awake()
  {
    this._canvasGroup.alpha = 0.0f;
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
  }

  private IEnumerator Start()
  {
    yield return (object) null;
    bool wait = true;
    float endValue = this._contentTransform.rect.height - this._canvasRectTransform.rect.height;
    DG.Tweening.Sequence creditsSequence = DOTween.Sequence();
    creditsSequence.Append((Tween) this._canvasGroup.DOFade(1f, 1.5f));
    creditsSequence.Append((Tween) this._contentTransform.DOAnchorPosY(endValue, 41f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine));
    creditsSequence.Append((Tween) this._canvasGroup.DOFade(0.0f, 1.5f));
    creditsSequence.Play<DG.Tweening.Sequence>();
    DG.Tweening.Sequence sequence = creditsSequence;
    sequence.onComplete = sequence.onComplete + (TweenCallback) (() => wait = false);
    KeyboardLightingManager.PlayVideo("Rainbow");
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.StopCurrentMusic();
    this.loopedSound = AudioManager.Instance.CreateLoop("event:/music/credits/credits", true);
    while (wait)
    {
      yield return (object) null;
      if (InputManager.UI.GetAcceptButtonHeld())
        creditsSequence.timeScale = this._skipSpeed;
      else
        creditsSequence.timeScale = 1f;
      int num = (int) this.loopedSound.setPitch(creditsSequence.timeScale);
    }
    this.LoadOverworld();
  }

  private void OnDisable() => AudioManager.Instance.StopLoop(this.loopedSound);

  private void LoadOverworld()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if (Credits.GoToMainMenu)
    {
      KeyboardLightingManager.Reset();
      FollowerManager.Reset();
      SimulationManager.Pause();
      StructureManager.Reset();
      UIDynamicNotificationCenter.Reset();
      GameManager.GoG_Initialised = false;
      TwitchManager.Abort();
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) null);
    }
    else
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 1f, "", (System.Action) null);
    Credits.GoToMainMenu = false;
  }
}
