// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TutorialInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Assets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class TutorialInfoCard : UIInfoCardBase<TutorialTopic>
{
  public System.Action OnLeftArrowClicked;
  public System.Action OnRightArrowClicked;
  [Header("Tutorial Info Card")]
  [SerializeField]
  public TutorialConfiguration _tutorialConfiguration;
  [Header("Content")]
  [SerializeField]
  public GameObject _topContainer;
  [SerializeField]
  public TextMeshProUGUI _header;
  [SerializeField]
  public Localize _headerLoc;
  [SerializeField]
  public TextMeshProUGUI _description;
  [SerializeField]
  public Localize _descriptionLoc;
  [SerializeField]
  public Image _image;
  [SerializeField]
  public TextMeshProUGUI _tutorialBody;
  [SerializeField]
  public Localize _tutorialBodyLoc;
  [Header("Pages")]
  [SerializeField]
  public TextMeshProUGUI _pageText;
  [SerializeField]
  public MMButton _leftArrow;
  [SerializeField]
  public MMButton _rightArrow;
  public int _page;
  public TutorialCategory _category;
  public Dictionary<AssetReferenceSprite, AsyncOperationHandle<Sprite>> handles = new Dictionary<AssetReferenceSprite, AsyncOperationHandle<Sprite>>();

  public int Page => this._page;

  public int NumPages => this._category.Entries.Length;

  public override void Awake()
  {
    base.Awake();
    this._leftArrow.onClick.AddListener((UnityAction) (() => this.OnLeftArrowClicked()));
    this._rightArrow.onClick.AddListener((UnityAction) (() => this.OnRightArrowClicked()));
  }

  public void OnDestroy() => this.UnloadAssets();

  public override void Configure(TutorialTopic config)
  {
    this._category = this._tutorialConfiguration.GetCategory(config);
    this._header.text = this._category.GetTitle();
    this._description.text = this._category.GetDescription();
    this._headerLoc.enabled = false;
    this._descriptionLoc.enabled = false;
    this._topContainer.SetActive(this._category.TopicImageRef.RuntimeKeyIsValid());
    this.StartCoroutine((IEnumerator) this.ConfigurePage(0));
  }

  public IEnumerator ConfigurePage(int page)
  {
    TutorialInfoCard tutorialInfoCard = this;
    tutorialInfoCard._page = page;
    if (page > 0 && page <= tutorialInfoCard._category.Entries.Length)
    {
      yield return (object) tutorialInfoCard.StartCoroutine((IEnumerator) tutorialInfoCard.LoadAsset(tutorialInfoCard._category.Entries[page - 1].ImageRef));
      tutorialInfoCard._image.sprite = tutorialInfoCard.handles[tutorialInfoCard._category.Entries[page - 1].ImageRef].Result;
      tutorialInfoCard._tutorialBody.text = tutorialInfoCard._category.Entries[page - 1].Description;
      tutorialInfoCard._tutorialBodyLoc.enabled = false;
    }
    else
    {
      yield return (object) tutorialInfoCard.StartCoroutine((IEnumerator) tutorialInfoCard.LoadAsset(tutorialInfoCard._category.TopicImageRef));
      tutorialInfoCard._image.sprite = tutorialInfoCard.handles[tutorialInfoCard._category.TopicImageRef].Result;
    }
    tutorialInfoCard._pageText.text = $"{page + 1}/{tutorialInfoCard._category.Entries.Length + 1}";
    tutorialInfoCard._pageText.isRightToLeftText = false;
    tutorialInfoCard._leftArrow.gameObject.SetActive(page != 0);
    tutorialInfoCard._rightArrow.gameObject.SetActive(page != tutorialInfoCard._category.Entries.Length);
    tutorialInfoCard._topContainer.SetActive(tutorialInfoCard._page == 0);
    tutorialInfoCard._tutorialBody.gameObject.SetActive(tutorialInfoCard._page > 0);
    yield return (object) null;
  }

  public IEnumerator LoadAsset(AssetReferenceSprite spriteRef)
  {
    AsyncOperationHandle<Sprite> asyncOperationHandle1;
    if (this.handles.TryGetValue(spriteRef, out asyncOperationHandle1))
    {
      if (!asyncOperationHandle1.IsValid())
      {
        asyncOperationHandle1 = Addressables.LoadAssetAsync<Sprite>((object) spriteRef);
        yield return (object) asyncOperationHandle1.WaitForCompletion();
      }
    }
    else
    {
      AsyncOperationHandle<Sprite> asyncOperationHandle2 = Addressables.LoadAssetAsync<Sprite>((object) spriteRef);
      this.handles.Add(spriteRef, asyncOperationHandle2);
      yield return (object) asyncOperationHandle2.WaitForCompletion();
    }
  }

  public void UnloadAssets()
  {
    foreach (AsyncOperationHandle<Sprite> handle in this.handles.Values)
    {
      if (handle.IsValid())
        Addressables.Release<Sprite>(handle);
    }
    this.handles.Clear();
  }

  public IEnumerator NextPage()
  {
    TutorialInfoCard tutorialInfoCard = this;
    if (tutorialInfoCard._page < tutorialInfoCard._category.Entries.Length)
    {
      UIManager.PlayAudio("event:/ui/arrow_change_selection");
      tutorialInfoCard.RectTransform.DOKill(true);
      tutorialInfoCard.CanvasGroup.DOKill(true);
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(-100f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(0.0f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
      yield return (object) tutorialInfoCard.StartCoroutine((IEnumerator) tutorialInfoCard.ConfigurePage(tutorialInfoCard._page + 1));
      tutorialInfoCard.RectTransform.DOKill();
      tutorialInfoCard.RectTransform.anchoredPosition = new Vector2(100f, 0.0f);
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(0.0f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(1f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
  }

  public IEnumerator PreviousPage()
  {
    TutorialInfoCard tutorialInfoCard = this;
    if (tutorialInfoCard._page > 0)
    {
      UIManager.PlayAudio("event:/ui/arrow_change_selection");
      tutorialInfoCard.RectTransform.DOKill(true);
      tutorialInfoCard.CanvasGroup.DOKill(true);
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(100f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(0.0f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
      yield return (object) tutorialInfoCard.StartCoroutine((IEnumerator) tutorialInfoCard.ConfigurePage(tutorialInfoCard._page - 1));
      tutorialInfoCard._topContainer.SetActive(tutorialInfoCard._page == 0);
      tutorialInfoCard._tutorialBody.gameObject.SetActive(tutorialInfoCard._page > 0);
      tutorialInfoCard.RectTransform.DOKill();
      tutorialInfoCard.RectTransform.anchoredPosition = new Vector2(-100f, 0.0f);
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(0.0f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(1f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
  }

  [CompilerGenerated]
  public void \u003CAwake\u003Eb__21_0() => this.OnLeftArrowClicked();

  [CompilerGenerated]
  public void \u003CAwake\u003Eb__21_1() => this.OnRightArrowClicked();
}
