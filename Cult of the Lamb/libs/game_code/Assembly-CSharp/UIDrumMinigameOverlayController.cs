// Decompiled with JetBrains decompiler
// Type: UIDrumMinigameOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMTools;
using Rewired;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDrumMinigameOverlayController : MonoBehaviour
{
  [CompilerGenerated]
  public static bool \u003CIsPlaying\u003Ek__BackingField;
  [SerializeField]
  public RectTransform container;
  [SerializeField]
  public UIDrumMinigameOverlayController.Lane[] lanes;
  [SerializeField]
  public UIDrumMinigameTarget target;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public GameObject sinBarContainer;
  [SerializeField]
  public Image sinProgressBar;
  [SerializeField]
  public Image sinProgressBarFlash;
  [Space]
  [SerializeField]
  public List<UIDrumMinigameOverlayController.Melody> Melodies;
  public Interaction_Drum drum;
  public int happinessLevel;
  public float targetSinNorm;
  public int sinProgress;
  public float fallDuration = 1.9f;
  public float pressedBuffer = 0.25f;
  public UIDrumMinigameOverlayController.Melody currentMelody;
  public List<UIDrumMinigameTarget> targets = new List<UIDrumMinigameTarget>();
  public Coroutine routine;
  public PlayerFarming playerFarming;
  public List<int> ActiveKeyboardButtons = new List<int>()
  {
    98,
    99,
    100
  };
  public List<int> ActiveControllerButtons = new List<int>()
  {
    98,
    99,
    100
  };
  public int Offset = 40;
  public int previousLane;
  public int drumCount;

  public static bool IsPlaying
  {
    get => UIDrumMinigameOverlayController.\u003CIsPlaying\u003Ek__BackingField;
    set => UIDrumMinigameOverlayController.\u003CIsPlaying\u003Ek__BackingField = value;
  }

  public int HappinessLevel => this.happinessLevel;

  public int TARGET_AMOUNT => this.currentMelody.TargetAmount;

  public float SinProgress => (float) this.sinProgress / (float) this.TARGET_AMOUNT;

  public event UIDrumMinigameOverlayController.NormalEvent OnFailedPress;

  public event UIDrumMinigameOverlayController.NormalEvent OnSuccessfulPress;

  public void OnEnable() => UIDrumMinigameOverlayController.IsPlaying = true;

  public void OnDisable() => UIDrumMinigameOverlayController.IsPlaying = false;

  public void Initialise(Interaction_Drum drum, int song)
  {
    this.drum = drum;
    this.playerFarming = drum.playerFarming;
    this.transform.position = PlayerFarming.Instance.transform.position + Vector3.back;
    this.target.gameObject.SetActive(false);
    this.currentMelody = this.Melodies[song];
    this.SetPrompts();
    this.StartCoroutine((IEnumerator) this.PlayMelody());
    this.ForceSinAmount(0.0f);
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.UpdateController);
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
  }

  public void Close()
  {
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.UpdateController);
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => this.canvasGroup.DOFade(0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)))));
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  public void OnDestroy() => MonoSingleton<UIManager>.Instance.UnloadDrumMinigameAssets();

  public void Update()
  {
    this.pressedBuffer -= Time.deltaTime;
    if (this.targets.Count <= 0 || (double) this.pressedBuffer > 0.0)
      return;
    bool flag = this.ButtonPressedWasActiveButton();
    if (flag || InputManager.General.GetAnyButton(this.playerFarming))
    {
      if (flag)
      {
        UIDrumMinigameOverlayController.Lane lane = this.lanes[this.GetDrumIndexFromActiveButton()];
        lane.EndPos.DOKill();
        lane.EndPos.transform.localScale = Vector3.one * 1.3f;
        lane.EndPos.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      }
      if (InputManager.Gameplay.GetButtonDown(this.targets[0].Button))
      {
        if (!this.targets[0].Ready)
          return;
        this.PlayPlayerAnim(this.targets[0].Lane);
        this.IncreaseHappiness();
        this.RemovePrompt(this.targets[0].Ready);
      }
      else
      {
        if (!this.ButtonPressedWasActiveButton())
          return;
        AudioManager.Instance.PlayOneShot("event:/building/drum_circle/neg_note");
        this.DecreaseHappiness();
        this.RemovePrompt(false);
      }
    }
    else
    {
      if (!this.targets[0].Failed)
        return;
      this.DecreaseHappiness();
      this.RemovePrompt(false);
      AudioManager.Instance.PlayOneShot("event:/building/drum_circle/miss_note");
    }
  }

  public bool ButtonPressedWasActiveButton()
  {
    foreach (int button in InputManager.General.InputIsController(this.playerFarming) ? this.ActiveControllerButtons : this.ActiveKeyboardButtons)
    {
      if (InputManager.Gameplay.GetButtonDown(button, this.playerFarming))
        return true;
    }
    return false;
  }

  public int GetDrumIndexFromActiveButton()
  {
    List<int> intList = InputManager.General.InputIsController(this.playerFarming) ? this.ActiveControllerButtons : this.ActiveKeyboardButtons;
    int index = -1;
    while (++index < intList.Count)
    {
      if (InputManager.Gameplay.GetButtonDown(intList[index], this.playerFarming))
        return index;
    }
    return -1;
  }

  public void IncreaseHappiness()
  {
    ++this.sinProgress;
    this.SinUpdated(this.SinProgress);
    this.happinessLevel = Mathf.Clamp(this.happinessLevel + 1, -6, 6);
    this.pressedBuffer = 0.25f;
    this.UpdateEmotion();
    UIDrumMinigameOverlayController.NormalEvent onSuccessfulPress = this.OnSuccessfulPress;
    if (onSuccessfulPress != null)
      onSuccessfulPress();
    MMVibrate.Rumble(0.25f, 0.3f, 0.05f, (MonoBehaviour) this);
    DeviceLightingManager.FlashColor(Color.green);
  }

  public void DecreaseHappiness()
  {
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Drumming/drum-messup", false);
    this.AddPlayerIdleAnim();
    this.happinessLevel = Mathf.Clamp(this.happinessLevel - 2, -6, 6);
    this.pressedBuffer = 0.25f;
    this.drum.FollowerQuit();
    this.UpdateEmotion();
    MMVibrate.Haptic(MMVibrate.HapticTypes.Failure, coroutineSupport: (MonoBehaviour) this);
    DeviceLightingManager.FlashColor(Color.red);
    UIDrumMinigameOverlayController.NormalEvent onFailedPress = this.OnFailedPress;
    if (onFailedPress == null)
      return;
    onFailedPress();
  }

  public void UpdateEmotion()
  {
    foreach (Follower follower in this.drum.Followers)
    {
      if (this.happinessLevel > -2 && this.happinessLevel < 2)
        follower.SetFaceAnimation("Emotions/emotion-normal", true);
      else if (this.happinessLevel > 5)
        follower.SetFaceAnimation("Emotions/emotion-enlightened", true);
      else if (this.happinessLevel >= 2)
        follower.SetFaceAnimation("Emotions/emotion-happy", true);
      else if (this.happinessLevel < -5)
        follower.SetFaceAnimation("Emotions/emotion-angry", true);
      else if (this.happinessLevel <= -2)
        follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
    }
  }

  public IEnumerator PlayMelody()
  {
    foreach (int note in this.currentMelody.Notes)
    {
      if (note - 1 >= 0)
        this.SpawnPrompt(note - 1);
      yield return (object) new WaitForSeconds(0.42857f);
    }
    yield return (object) new WaitForSeconds(1.5f);
    this.Close();
  }

  public void UpdateController(Controller con) => this.SetPrompts();

  public void SetPrompts()
  {
    List<int> intList = InputManager.General.InputIsController(this.playerFarming) ? this.ActiveControllerButtons : this.ActiveKeyboardButtons;
    int num1 = 0;
    int num2 = Mathf.Min(this.lanes.Length, intList.Count);
    for (int index = 0; index < num2; ++index)
    {
      this.lanes[index].ControlPrompt.Action = intList[index];
      this.lanes[index].ControlPrompt.Category = num1;
    }
    foreach (UIDrumMinigameOverlayController.Lane lane in this.lanes)
    {
      lane.ControlPrompt.playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
      lane.ControlPrompt.ForceUpdate();
    }
  }

  public void SpawnPrompt(int l)
  {
    UIDrumMinigameOverlayController.Lane lane = this.lanes[l];
    UIDrumMinigameTarget drumMinigameTarget = UnityEngine.Object.Instantiate<UIDrumMinigameTarget>(this.target, lane.Container);
    drumMinigameTarget.gameObject.SetActive(true);
    drumMinigameTarget.Configure(lane.ControlPrompt.Action, l, lane.StartPos.localPosition, lane.EndPos.transform.localPosition, Vector3.down * (float) this.Offset, this.fallDuration);
    this.targets.Add(drumMinigameTarget);
    drumMinigameTarget.transform.localScale = Vector3.zero;
    drumMinigameTarget.transform.DOScale(0.83f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public void RemovePrompt(bool success)
  {
    UIDrumMinigameTarget target = this.targets[0];
    this.targets.RemoveAt(0);
    if (success)
    {
      target.Success();
    }
    else
    {
      target.Fail();
      if (this.targets.Count <= 0)
        return;
      this.RemovePrompt(false);
    }
  }

  public void PlayPlayerAnim(int lane)
  {
    switch (lane)
    {
      case 0:
      case 2:
        PlayerFarming.Instance.transform.localScale = lane == 0 ? new Vector3(-1f, 1f, 1f) : Vector3.one;
        if (this.happinessLevel > -3 && this.happinessLevel < 3)
        {
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Drumming/drum-hit-side", false);
          break;
        }
        if (this.happinessLevel >= 3)
        {
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Drumming/drum-hit-side-good", false);
          break;
        }
        if (this.happinessLevel <= -3)
        {
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Drumming/drum-hit-side-bad", false);
          break;
        }
        break;
      case 1:
        PlayerFarming.Instance.transform.localScale = Vector3.one;
        if (this.happinessLevel > -3 && this.happinessLevel < 3)
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, this.drumCount == 0 ? "Drumming/drum-hit-center" : "Drumming/drum-hit-center-chained", false);
        else if (this.happinessLevel >= 3)
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, this.drumCount == 0 ? "Drumming/drum-hit-center-good" : "Drumming/drum-hit-center-chained-good", false);
        else if (this.happinessLevel <= -3)
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, this.drumCount == 0 ? "Drumming/drum-hit-center-bad" : "Drumming/drum-hit-center-chained-bad", false);
        this.drumCount = (int) Utils.Repeat((float) (this.drumCount + 1), 2f);
        break;
    }
    this.AddPlayerIdleAnim();
    this.previousLane = lane;
  }

  public void AddPlayerIdleAnim()
  {
    if (this.happinessLevel > -3 && this.happinessLevel < 3)
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "Drumming/drum-idle", true, 0.0f);
    else if (this.happinessLevel >= 3)
    {
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "Drumming/drum-idle-good", true, 0.0f);
    }
    else
    {
      if (this.happinessLevel > -3)
        return;
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "Drumming/drum-idle-bad", true, 0.0f);
    }
  }

  public void SinUpdated(float sinNorm)
  {
    if (!this.gameObject.activeInHierarchy || (double) sinNorm == (double) this.targetSinNorm)
      return;
    if (this.routine != null)
    {
      this.StopCoroutine(this.routine);
      this.ForceSinAmount(this.targetSinNorm);
    }
    this.targetSinNorm = sinNorm;
    this.routine = this.StartCoroutine((IEnumerator) this.SinBarUpdated(this.targetSinNorm));
  }

  public void ForceSinAmount(float normSinAmount)
  {
    this.sinProgressBar.fillAmount = normSinAmount;
    this.sinProgressBarFlash.fillAmount = normSinAmount;
    this.targetSinNorm = normSinAmount;
  }

  public IEnumerator SinBarUpdated(float normAmount)
  {
    this.sinBarContainer.transform.DOKill();
    this.sinBarContainer.transform.localScale = Vector3.one;
    this.sinBarContainer.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    this.sinProgressBarFlash.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    float fromAmount = this.sinProgressBar.fillAmount;
    float t = 0.0f;
    while ((double) t < 0.25)
    {
      float t1 = t / 0.25f;
      this.sinProgressBar.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.sinProgressBar.fillAmount = normAmount;
    this.routine = (Coroutine) null;
  }

  [CompilerGenerated]
  public void \u003CClose\u003Eb__42_0()
  {
    this.canvasGroup.DOFade(0.0f, 0.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  [CompilerGenerated]
  public void \u003CClose\u003Eb__42_1() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [Serializable]
  public struct Melody
  {
    public List<int> Notes;
    public int TargetAmount;
  }

  [Serializable]
  public struct Lane
  {
    public Transform Container;
    public Transform StartPos;
    public Transform EndPos;
    public MMControlPrompt ControlPrompt;
  }

  public delegate void NormalEvent();
}
