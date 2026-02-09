// Decompiled with JetBrains decompiler
// Type: NotificationBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float _overrideScreenDuration = -1f;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _contentRectTransform;
  [SerializeField]
  public TextMeshProUGUI _description;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public GameObject _positiveFlair;
  [SerializeField]
  public GameObject _negativeFlair;
  [SerializeField]
  public GameObject _winterFlair;
  public bool Shown;
  public Vector2 _onScreenPosition = new Vector2(0.0f, 0.0f);
  public Vector2 _offScreenPosition = new Vector2(-800f, 0.0f);

  public abstract float _onScreenDuration { get; }

  public abstract float _showHideDuration { get; }

  public virtual void Configure(NotificationBase.Flair flair)
  {
    this._overrideScreenDuration = -1f;
    this._contentRectTransform.anchoredPosition = this._offScreenPosition;
    this.Localize();
    this.Show();
    this._positiveFlair.SetActive(flair == NotificationBase.Flair.Positive);
    this._negativeFlair.SetActive(flair == NotificationBase.Flair.Negative);
    if (!((UnityEngine.Object) this._winterFlair != (UnityEngine.Object) null))
      return;
    this._winterFlair.SetActive(flair == NotificationBase.Flair.Winter);
  }

  public virtual void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public abstract void Localize();

  public void Show(bool instant = false, System.Action andThen = null)
  {
    this.ResetScale();
    this.gameObject.SetActive(true);
    this.StopAllCoroutines();
    if (this._description.text.Contains("This is a Notification"))
      this.Hide(true);
    else if (instant)
    {
      this._contentRectTransform.anchoredPosition = this._onScreenPosition;
      this._canvasGroup.alpha = 1f;
      this.Shown = true;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow(andThen));
  }

  public virtual IEnumerator DoShow(System.Action andThen = null)
  {
    this.Shown = true;
    this._contentRectTransform.DOKill();
    this._canvasGroup.DOKill();
    this._contentRectTransform.DOAnchorPosX(this._onScreenPosition.x, this._showHideDuration).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack);
    this._canvasGroup.DOFade(1f, this._showHideDuration * 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    System.Action action = andThen;
    if (action != null)
      action();
    yield return (object) this.HoldOnScreen();
    this.Hide();
  }

  public virtual IEnumerator HoldOnScreen()
  {
    float timer = (double) this._overrideScreenDuration != -1.0 ? this._overrideScreenDuration : this._onScreenDuration;
    while ((double) timer > 0.0)
    {
      if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null && !HUD_Manager.Instance.Hidden && !LetterBox.IsPlaying)
        timer -= Time.deltaTime;
      yield return (object) null;
    }
  }

  public void Hide(bool instant = false, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    if (instant)
    {
      this._contentRectTransform.anchoredPosition = this._offScreenPosition;
      this._canvasGroup.alpha = 0.0f;
      this.Shown = false;
      if (andThen != null)
        andThen();
      this.OnHide();
      ObjectPool.Recycle(this.gameObject);
    }
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  public virtual IEnumerator DoHide(System.Action andThen = null)
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
    notificationBase.OnHide();
    ObjectPool.Recycle(notificationBase.gameObject);
  }

  public void ResetScale() => this._rectTransform.localScale = new Vector3(1f, 1f, 1f);

  public virtual void OnHide() => NotificationCentre.Notifications.Remove(this);

  public virtual void OnDestroy() => NotificationCentre.Notifications.Remove(this);

  public void SetOverrideShowDuration(float duration) => this._overrideScreenDuration = duration;

  public enum Flair
  {
    None,
    Positive,
    Negative,
    Winter,
  }
}
