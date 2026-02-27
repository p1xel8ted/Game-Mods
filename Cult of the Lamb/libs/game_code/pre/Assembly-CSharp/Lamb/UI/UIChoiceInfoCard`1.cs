// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIChoiceInfoCard`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMTools;
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
  private const float kHoldDuration = 3f;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _cardContainer;
  [SerializeField]
  private GameObject _holdActionContainer;
  [SerializeField]
  private RadialProgress _radialProgress;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Image _selectedSymbol;
  [SerializeField]
  private Sprite _selectedSymbolSprite;
  [SerializeField]
  private Sprite _unSelectedSymbolSprite;
  [SerializeField]
  private Image _unselectedOverlay;
  [SerializeField]
  private Image _redOutline;
  [SerializeField]
  private Image _whiteFlash;
  private Vector2 _defaultAnchoredPosition;
  private Vector2 _offscreenPosition;
  protected T _info;

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

  protected abstract void ConfigureImpl(T info);

  private void OnEnable()
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

  private void DoDeselect()
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
        if (InputManager.UI.GetAcceptButtonHeld() || InputManager.Gameplay.GetInteractButtonHeld())
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
        if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1 && InputManager.UI.GetCancelButtonDown())
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
      while (!InputManager.UI.GetAcceptButtonHeld() && !InputManager.Gameplay.GetInteractButtonHeld())
      {
        if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1 && InputManager.UI.GetCancelButtonDown())
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
