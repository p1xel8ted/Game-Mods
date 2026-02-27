// Decompiled with JetBrains decompiler
// Type: NotificationBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public abstract class NotificationBase : MonoBehaviour
{
  [SerializeField]
  protected RectTransform _rectTransform;
  [SerializeField]
  protected RectTransform _contentRectTransform;
  [SerializeField]
  protected TextMeshProUGUI _description;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private GameObject _positiveFlair;
  [SerializeField]
  private GameObject _negativeFlair;
  protected bool Shown;
  protected Vector2 _onScreenPosition = new Vector2(0.0f, 0.0f);
  protected Vector2 _offScreenPosition = new Vector2(-800f, 0.0f);

  protected abstract float _onScreenDuration { get; }

  protected abstract float _showHideDuration { get; }

  protected virtual void Configure(NotificationBase.Flair flair)
  {
    this._contentRectTransform.anchoredPosition = this._offScreenPosition;
    this.Localize();
    this.Show();
    this._positiveFlair.SetActive(flair == NotificationBase.Flair.Positive);
    this._negativeFlair.SetActive(flair == NotificationBase.Flair.Negative);
  }

  protected virtual void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  protected void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  protected abstract void Localize();

  protected void Show(bool instant = false, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    if (instant)
    {
      this._contentRectTransform.anchoredPosition = this._onScreenPosition;
      this._canvasGroup.alpha = 1f;
      this.Shown = true;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow(andThen));
  }

  protected virtual IEnumerator DoShow(System.Action andThen = null)
  {
    this._contentRectTransform.DOKill();
    this._canvasGroup.DOKill();
    this._contentRectTransform.DOAnchorPosX(this._onScreenPosition.x, this._showHideDuration).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack);
    this._canvasGroup.DOFade(1f, this._showHideDuration * 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    System.Action action = andThen;
    if (action != null)
      action();
    this.Shown = true;
    yield return (object) this.HoldOnScreen();
    this.Hide();
  }

  protected virtual IEnumerator HoldOnScreen()
  {
    float timer = this._onScreenDuration;
    while ((double) timer > 0.0)
    {
      if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null && !HUD_Manager.Instance.Hidden && !LetterBox.IsPlaying)
        timer -= Time.deltaTime;
      yield return (object) null;
    }
  }

  protected void Hide(bool instant = false, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    if (instant)
    {
      this._contentRectTransform.anchoredPosition = this._offScreenPosition;
      this._canvasGroup.alpha = 0.0f;
      this.Shown = false;
      if (andThen != null)
        andThen();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  protected virtual IEnumerator DoHide(System.Action andThen = null)
  {
    NotificationBase notificationBase = this;
    notificationBase.Shown = false;
    notificationBase._contentRectTransform.DOKill();
    notificationBase._canvasGroup.DOKill();
    notificationBase._contentRectTransform.DOAnchorPosX(notificationBase._offScreenPosition.x, notificationBase._showHideDuration).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack);
    notificationBase._canvasGroup.DOFade(0.0f, notificationBase._showHideDuration * 0.5f).SetDelay<TweenerCore<float, float, FloatOptions>>(notificationBase._showHideDuration * 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    System.Action action = andThen;
    if (action != null)
      action();
    UnityEngine.Object.Destroy((UnityEngine.Object) notificationBase.gameObject);
  }

  protected virtual void OnDestroy() => NotificationCentre.Notifications.Remove(this);

  public enum Flair
  {
    None,
    Positive,
    Negative,
  }
}
