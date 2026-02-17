// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIChoiceInfoCard`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMTools;
using src.UINavigator;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIChoiceInfoCard<T> : 
  MonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public const float kHoldDuration = 3f;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _cardContainer;
  [SerializeField]
  public GameObject _holdActionContainer;
  [SerializeField]
  public RadialProgress _radialProgress;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Image _selectedSymbol;
  [SerializeField]
  public Sprite _selectedSymbolSprite;
  [SerializeField]
  public Sprite _unSelectedSymbolSprite;
  [SerializeField]
  public Image _unselectedOverlay;
  [SerializeField]
  public Image _redOutline;
  [SerializeField]
  public Image _whiteFlash;
  public Vector2 _defaultAnchoredPosition;
  public Vector2 _offscreenPosition;
  public T _info;

  public MMButton Button => this._button;

  public RectTransform CardContainer => this._cardContainer;

  public RectTransform RectTransform => this._rectTransform;

  public Image WhiteFlash => this._whiteFlash;

  public T Info => this._info;

  public void Configure(T info, Vector2 defaultAnchoredPosition, Vector2 offscreenPosition)
  {
    this._info = info;
    this._redOutline.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    this._defaultAnchoredPosition = defaultAnchoredPosition;
    this._offscreenPosition = offscreenPosition;
    this._rectTransform.anchoredPosition = offscreenPosition;
    this._rectTransform.localScale = Vector3.one;
    this.DoDeselect();
    this._button.enabled = false;
    this._button.interactable = false;
    this.ConfigureImpl(info);
  }

  public abstract void ConfigureImpl(T info);

  public void OnEnable()
  {
    this._holdActionContainer.SetActive(false);
    this._radialProgress.Progress = 0.0f;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._redOutline.rectTransform.DOKill();
    this._redOutline.rectTransform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._unselectedOverlay.DOKill();
    this._unselectedOverlay.color = (Color) new Vector4(1f, 1f, 1f, 1f);
    DOTweenModuleUI.DOFade(this._unselectedOverlay, 0.0f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCirc).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._selectedSymbol.sprite = this._selectedSymbolSprite;
    this._selectedSymbol.color = StaticColors.RedColor;
  }

  public void OnDeselect(BaseEventData eventData) => this.DoDeselect();

  public void DoDeselect()
  {
    this.ResetTweens();
    this._unselectedOverlay.DOKill();
    this._unselectedOverlay.color = (Color) new Vector4(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(this._unselectedOverlay, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCirc).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._selectedSymbol.sprite = this._unSelectedSymbolSprite;
    this._selectedSymbol.color = StaticColors.OffWhiteColor;
  }

  public void ResetTweens()
  {
    this._redOutline.rectTransform.DOKill();
    this._redOutline.rectTransform.DOScale(0.8f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._rectTransform.DOKill();
    this._rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void SendOffscreen()
  {
    this._rectTransform.DOAnchorPos(this._offscreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
  }

  public void BringOnscreen()
  {
    this._rectTransform.DOAnchorPos(this._defaultAnchoredPosition, 0.6f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
  }

  public IEnumerator PerformHoldAction(System.Action onCancel)
  {
    this._holdActionContainer.SetActive(true);
    this._holdActionContainer.transform.localScale = Vector3.one * 1.2f;
    this._holdActionContainer.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (SettingsManager.Settings.Accessibility.HoldActions)
    {
      EventInstance? loopingSound = new EventInstance?();
      float progress = 0.0f;
      while ((double) progress < 3.0)
      {
        float num1 = progress / 3f;
        if (InputManager.UI.GetAcceptButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || InputManager.Gameplay.GetInteractButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        {
          if (!loopingSound.HasValue)
            loopingSound = new EventInstance?(AudioManager.Instance.CreateLoop("event:/hearts_of_the_faithful/draw_power_loop", true));
          progress = Mathf.Clamp(progress + Time.unscaledDeltaTime, 0.0f, 3f);
        }
        else
          progress = Mathf.Clamp(progress - Time.unscaledDeltaTime * 5f, 0.0f, 3f);
        ref EventInstance? local = ref loopingSound;
        if (local.HasValue)
        {
          int num2 = (int) local.GetValueOrDefault().setParameterByName("power", num1);
        }
        this._redOutline.rectTransform.localScale = Vector3.Lerp(new Vector3(0.97f, 0.97f, 0.97f), new Vector3(1.2f, 1.2f, 1.2f), progress / 6f);
        this._rectTransform.localScale = Vector3.one * 1.1f * (float) (1.0 + (double) progress / 40.0);
        this._cardContainer.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * 2f);
        this._radialProgress.Progress = num1;
        if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1 && InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        {
          onCancel();
          this._holdActionContainer.SetActive(false);
          this._radialProgress.Progress = 0.0f;
          UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
          if (!loopingSound.HasValue)
            yield break;
          AudioManager.Instance.StopLoop(loopingSound.Value);
          yield break;
        }
        yield return (object) null;
      }
      if (loopingSound.HasValue)
        AudioManager.Instance.StopLoop(loopingSound.Value);
      loopingSound = new EventInstance?();
    }
    else
    {
      while (!InputManager.UI.GetAcceptButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !InputManager.Gameplay.GetInteractButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      {
        if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1 && InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        {
          onCancel();
          this._holdActionContainer.SetActive(false);
          UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
          yield break;
        }
        yield return (object) null;
      }
    }
    UIManager.PlayAudio("event:/hearts_of_the_faithful/draw_power_end");
    this._holdActionContainer.SetActive(false);
  }

  public void ShowSelection(bool state)
  {
    this._selectedSymbol.enabled = state;
    this._unselectedOverlay.enabled = state;
  }
}
