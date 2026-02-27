// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHoldInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIHoldInteraction : MonoBehaviour
{
  [SerializeField]
  private RectTransform _controlPrompt;
  [SerializeField]
  private RadialProgress _radialProgress;
  [SerializeField]
  private float _holdTime = 3f;
  public string LoopSound = "event:/unlock_building/unlock_hold";

  public float HoldTime => this._holdTime;

  private void Start() => this.Reset();

  public void Reset()
  {
    this._radialProgress.Progress = 0.0f;
    this._radialProgress.gameObject.SetActive(SettingsManager.Settings.Accessibility.HoldActions);
  }

  public IEnumerator DoHoldInteraction(Action<float> onUpdate, System.Action onCancel)
  {
    EventInstance? loopingSoundInstance = new EventInstance?();
    bool cancelled = false;
    float progress = 0.0f;
    while (SettingsManager.Settings.Accessibility.HoldActions)
    {
      if (InputManager.UI.GetAcceptButtonDown() || InputManager.Gameplay.GetInteractButtonDown())
      {
        this._controlPrompt.localScale = Vector3.one;
        this._controlPrompt.DOKill();
        this._controlPrompt.DOPunchScale(new Vector3(0.2f, 0.2f), 0.2f).SetUpdate<Tweener>(true);
      }
      else if (InputManager.UI.GetAcceptButtonHeld() || InputManager.Gameplay.GetInteractButtonHeld())
      {
        if (!loopingSoundInstance.HasValue)
          loopingSoundInstance = new EventInstance?(AudioManager.Instance.CreateLoop(this.LoopSound, true));
        progress += Time.unscaledDeltaTime;
        if ((double) progress >= (double) this._holdTime)
          break;
      }
      else
      {
        if (loopingSoundInstance.HasValue)
        {
          AudioManager.Instance.StopLoop(loopingSoundInstance.Value);
          loopingSoundInstance = new EventInstance?();
        }
        if ((double) progress > 0.0)
        {
          progress -= Time.unscaledDeltaTime * 5f;
          progress = Mathf.Max(progress, 0.0f);
        }
      }
      if (InputManager.UI.GetCancelButtonDown())
      {
        cancelled = true;
        break;
      }
      Action<float> action = onUpdate;
      if (action != null)
        action(progress / this._holdTime);
      this._radialProgress.Progress = progress / this._holdTime;
      yield return (object) null;
    }
    while (!SettingsManager.Settings.Accessibility.HoldActions && !InputManager.UI.GetAcceptButtonDown() && !InputManager.Gameplay.GetInteractButtonDown())
    {
      if (InputManager.UI.GetCancelButtonDown())
      {
        cancelled = true;
        break;
      }
      yield return (object) null;
    }
    if (loopingSoundInstance.HasValue)
    {
      AudioManager.Instance.StopLoop(loopingSoundInstance.Value);
      loopingSoundInstance = new EventInstance?();
    }
    if (cancelled)
    {
      System.Action action = onCancel;
      if (action != null)
        action();
    }
  }
}
