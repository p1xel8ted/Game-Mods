// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHoldInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIHoldInteraction : MonoBehaviour
{
  [SerializeField]
  public RectTransform _controlPrompt;
  [SerializeField]
  public RadialProgress _radialProgress;
  [SerializeField]
  public float _holdTime = 3f;
  public string LoopSound = "event:/unlock_building/unlock_hold";

  public float HoldTime => this._holdTime;

  public void Start() => this.Reset();

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
      if (InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || InputManager.Gameplay.GetInteractButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        this._controlPrompt.localScale = Vector3.one;
        this._controlPrompt.DOKill();
        this._controlPrompt.DOPunchScale(new Vector3(0.2f, 0.2f), 0.2f).SetUpdate<Tweener>(true);
      }
      else if (InputManager.UI.GetAcceptButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || InputManager.Gameplay.GetInteractButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
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
      if (InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
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
    while (!SettingsManager.Settings.Accessibility.HoldActions && !InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !InputManager.Gameplay.GetInteractButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
    {
      if (InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
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
