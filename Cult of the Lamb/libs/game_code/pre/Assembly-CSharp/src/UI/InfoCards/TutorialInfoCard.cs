// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TutorialInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.Assets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class TutorialInfoCard : UIInfoCardBase<TutorialTopic>
{
  [Header("Tutorial Info Card")]
  [SerializeField]
  private TutorialConfiguration _tutorialConfiguration;
  [Header("Content")]
  [SerializeField]
  private GameObject _topContainer;
  [SerializeField]
  private TextMeshProUGUI _header;
  [SerializeField]
  private TextMeshProUGUI _description;
  [SerializeField]
  private Image _image;
  [SerializeField]
  private TextMeshProUGUI _tutorialBody;
  [Header("Pages")]
  [SerializeField]
  private TextMeshProUGUI _pageText;
  [SerializeField]
  private GameObject _leftArrow;
  [SerializeField]
  private GameObject _rightArrow;
  private int _page;
  private TutorialCategory _category;

  public int Page => this._page;

  public int NumPages => this._category.Entries.Length;

  public override void Configure(TutorialTopic config)
  {
    this._category = this._tutorialConfiguration.GetCategory(config);
    this._header.text = this._category.GetTitle();
    this._description.text = this._category.GetDescription();
    this._topContainer.SetActive((Object) this._category.TopicImage != (Object) null);
    this.ConfigurePage(0);
  }

  private void ConfigurePage(int page)
  {
    this._page = page;
    if (page > 0 && page <= this._category.Entries.Length)
    {
      this._image.sprite = this._category.Entries[page - 1].Image;
      this._tutorialBody.text = this._category.Entries[page - 1].Description;
    }
    else
      this._image.sprite = this._category.TopicImage;
    this._pageText.text = $"{page + 1}/{this._category.Entries.Length + 1}";
    this._leftArrow.SetActive(page != 0);
    this._rightArrow.SetActive(page != this._category.Entries.Length);
    this._topContainer.SetActive(this._page == 0);
    this._tutorialBody.gameObject.SetActive(this._page > 0);
  }

  public IEnumerator NextPage()
  {
    TutorialInfoCard tutorialInfoCard = this;
    if (tutorialInfoCard._page < tutorialInfoCard._category.Entries.Length)
    {
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(-100f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(0.0f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
      tutorialInfoCard.ConfigurePage(tutorialInfoCard._page + 1);
      tutorialInfoCard.RectTransform.DOKill();
      tutorialInfoCard.RectTransform.anchoredPosition = new Vector2(100f, 0.0f);
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(0.0f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(1f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
    }
  }

  public IEnumerator PreviousPage()
  {
    TutorialInfoCard tutorialInfoCard = this;
    if (tutorialInfoCard._page > 0)
    {
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(100f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(0.0f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
      tutorialInfoCard.ConfigurePage(tutorialInfoCard._page - 1);
      tutorialInfoCard._topContainer.SetActive(tutorialInfoCard._page == 0);
      tutorialInfoCard._tutorialBody.gameObject.SetActive(tutorialInfoCard._page > 0);
      tutorialInfoCard.RectTransform.DOKill();
      tutorialInfoCard.RectTransform.anchoredPosition = new Vector2(-100f, 0.0f);
      tutorialInfoCard.RectTransform.DOAnchorPos(new Vector2(0.0f, 0.0f), 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      tutorialInfoCard.CanvasGroup.DOFade(1f, 0.15f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
    }
  }
}
