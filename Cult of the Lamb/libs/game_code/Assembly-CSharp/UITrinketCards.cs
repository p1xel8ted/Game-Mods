// Decompiled with JetBrains decompiler
// Type: UITrinketCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools.UIInventory;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UITrinketCards : 
  UIInventoryController,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public UIWeaponCard TrinketCard;
  public GameObject List;
  public Image background;
  public bool CameFromPauseScreen;
  [SerializeField]
  public Image selectedGameobject;
  public CanvasGroup canvasGroup;
  [CompilerGenerated]
  public bool \u003CDestroyAfter\u003Ek__BackingField = true;
  public TarotCards.TarotCard _drawnCard;
  public bool _requiresCallbacks = true;
  public System.Action OnDeselected;
  public System.Action OnSelected;

  public bool DestroyAfter
  {
    get => this.\u003CDestroyAfter\u003Ek__BackingField;
    set => this.\u003CDestroyAfter\u003Ek__BackingField = value;
  }

  public static GameObject Play(
    TarotCards.TarotCard drawnCard,
    System.Action CallBack,
    float pauseTimeSpeed = 1f)
  {
    UITrinketCards uiTrinketCards = UnityEngine.Object.Instantiate<UITrinketCards>(UnityEngine.Resources.Load<UITrinketCards>("MMUIInventory/UI Trinket Cards"), GlobalCanvasReference.Instance);
    uiTrinketCards.Callback = CallBack;
    uiTrinketCards.PauseTimeSpeed = pauseTimeSpeed;
    uiTrinketCards._drawnCard = drawnCard;
    uiTrinketCards.TrinketCard.CameFromPauseMenu = false;
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_reveal", PlayerFarming.Instance.transform.position);
    return uiTrinketCards.gameObject;
  }

  public static void PlayFromPause(TarotCards.TarotCard drawnCard, System.Action CallBack)
  {
    UITrinketCards uiTrinketCards = UnityEngine.Object.Instantiate<UITrinketCards>(UnityEngine.Resources.Load<UITrinketCards>("MMUIInventory/UI Trinket Cards"), GlobalCanvasReference.Instance);
    uiTrinketCards.Callback = CallBack;
    uiTrinketCards.PauseTimeSpeed = 0.0f;
    uiTrinketCards._drawnCard = drawnCard;
    uiTrinketCards.CameFromPauseScreen = true;
    uiTrinketCards.TrinketCard.CameFromPauseMenu = true;
  }

  public static void PlayFromPause(
    TarotCards.TarotCard drawnCard,
    System.Action CallBack,
    GameObject parent)
  {
    UITrinketCards uiTrinketCards = UnityEngine.Object.Instantiate<UITrinketCards>(UnityEngine.Resources.Load<UITrinketCards>("MMUIInventory/UI Trinket Cards"), parent.transform);
    uiTrinketCards.Callback = CallBack;
    uiTrinketCards.PauseTimeSpeed = 0.0f;
    uiTrinketCards._drawnCard = drawnCard;
    uiTrinketCards.CameFromPauseScreen = true;
    uiTrinketCards.TrinketCard.CameFromPauseMenu = true;
  }

  public void Play(TarotCards.TarotCard drawnCard)
  {
    this._drawnCard = drawnCard;
    this.PauseTimeSpeed = 1f;
    this.DestroyAfter = false;
    this.CameFromPauseScreen = false;
    this._requiresCallbacks = false;
    this.TrinketCard.CameFromPauseMenu = false;
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.transform.DOKill();
    this.canvasGroup.DOKill();
    this.selectedGameobject.transform.DOKill();
    this.transform.DOScale(0.9f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.canvasGroup.DOFade(0.9f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    DOTweenModuleUI.DOFade(this.selectedGameobject, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    System.Action onDeselected = this.OnDeselected;
    if (onDeselected == null)
      return;
    onDeselected();
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.transform.DOKill();
    this.canvasGroup.DOKill();
    this.selectedGameobject.transform.DOKill();
    this.transform.DOScale(1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.canvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    this.selectedGameobject.transform.DOKill();
    DOTweenModuleUI.DOFade(this.selectedGameobject, 1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    System.Action onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected();
  }

  public override void StartUIInventoryController()
  {
    this.StartCoroutine((IEnumerator) this.DealCards());
  }

  public void OnEnable()
  {
    if ((UnityEngine.Object) this.canvasGroup == (UnityEngine.Object) null)
      this.gameObject.GetComponent<CanvasGroup>();
    this.transform.DOScale(0.9f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.canvasGroup.DOFade(0.9f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
  }

  public IEnumerator DealCards()
  {
    UITrinketCards uiTrinketCards = this;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) uiTrinketCards.StartCoroutine((IEnumerator) uiTrinketCards.TrinketCard.Play(uiTrinketCards._drawnCard, Vector3.zero));
    if (!uiTrinketCards.CameFromPauseScreen && (double) Time.timeScale != 0.0)
      yield return (object) new WaitForSeconds(1.5f);
    if (uiTrinketCards._requiresCallbacks)
    {
      while (!InputManager.UI.GetAcceptButtonDown() && !InputManager.UI.GetCancelButtonDown())
        yield return (object) null;
      if (!uiTrinketCards.CameFromPauseScreen)
      {
        uiTrinketCards.Close();
      }
      else
      {
        System.Action callback = uiTrinketCards.Callback;
        if (callback != null)
          callback();
      }
      if (uiTrinketCards.DestroyAfter)
        UnityEngine.Object.Destroy((UnityEngine.Object) uiTrinketCards.gameObject);
    }
  }
}
