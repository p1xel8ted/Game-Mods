// Decompiled with JetBrains decompiler
// Type: Credits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public RectTransform _canvasRectTransform;
  [SerializeField]
  public float _skipSpeed = 4f;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public RectTransform _contentTransform;
  public EventInstance loopedSound;

  public void Awake()
  {
    this._canvasGroup.alpha = 0.0f;
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    bool wait = true;
    float endValue = this._contentTransform.rect.height - this._canvasRectTransform.rect.height;
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
    {
      MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
      MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
    }
    DG.Tweening.Sequence creditsSequence = DOTween.Sequence();
    creditsSequence.Append((Tween) this._canvasGroup.DOFade(1f, 1.5f));
    creditsSequence.Append((Tween) this._contentTransform.DOAnchorPosY(endValue, 41f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine));
    creditsSequence.Append((Tween) this._canvasGroup.DOFade(0.0f, 1.5f));
    creditsSequence.Play<DG.Tweening.Sequence>();
    DG.Tweening.Sequence sequence = creditsSequence;
    sequence.onComplete = sequence.onComplete + (TweenCallback) (() => wait = false);
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

  public void OnDisable() => AudioManager.Instance.StopLoop(this.loopedSound);

  public void LoadOverworld()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if (Credits.GoToMainMenu)
    {
      DeviceLightingManager.Reset();
      FollowerManager.Reset();
      SimulationManager.Pause();
      StructureManager.Reset();
      UIDynamicNotificationCenter.Reset();
      TwitchManager.Abort();
      MMTransition.ForceShowIcon = true;
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) null);
    }
    else
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 1f, "", (System.Action) null);
    Credits.GoToMainMenu = false;
  }
}
