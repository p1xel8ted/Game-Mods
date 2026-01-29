// Decompiled with JetBrains decompiler
// Type: NotificationCentreScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationCentreScreen : BaseMonoBehaviour
{
  public RectTransform rectTransform;
  public CanvasGroup canvasGroup;
  public TextMeshProUGUI Text;
  public CanvasGroup textCanvasGroup;
  public GameObject container;
  public Image background;
  [SerializeField]
  public CanvasGroup _backgroundCanvasGroup;
  public static NotificationCentreScreen Instance;
  public Vector3 _containerStartingPos;

  public void OnEnable()
  {
    NotificationCentreScreen.Instance = this;
    this.Text.text = "";
  }

  public void Start()
  {
    this.Text.text = "";
    this.container.SetActive(false);
  }

  public void OnDisable()
  {
    if (!((Object) NotificationCentreScreen.Instance == (Object) this))
      return;
    NotificationCentreScreen.Instance = (NotificationCentreScreen) null;
  }

  public static void Play(NotificationCentre.NotificationType Notification)
  {
    if ((Object) NotificationCentreScreen.Instance == (Object) null)
    {
      NotificationCentre.Instance.PlayGenericNotification(Notification);
    }
    else
    {
      NotificationCentreScreen.Instance.container.gameObject.SetActive(true);
      NotificationCentreScreen.Instance.Text.text = LocalizationManager.GetTranslation("Notifications/" + Notification.ToString());
      NotificationCentreScreen.Instance.StartCoroutine((IEnumerator) NotificationCentreScreen.Instance.PlayRoutine());
      NotificationCentreScreen.Instance.transform.SetAsLastSibling();
    }
  }

  public static void Play(string Notification)
  {
    if ((Object) NotificationCentreScreen.Instance == (Object) null)
    {
      NotificationCentre.Instance.PlayGenericNotification(Notification);
    }
    else
    {
      NotificationCentreScreen.Instance.container.gameObject.SetActive(true);
      NotificationCentreScreen.Instance.Text.text = Notification;
      NotificationCentreScreen.Instance.StartCoroutine((IEnumerator) NotificationCentreScreen.Instance.PlayRoutine());
      NotificationCentreScreen.Instance.transform.SetAsLastSibling();
    }
  }

  public IEnumerator PlayRoutine()
  {
    NotificationCentreScreen notificationCentreScreen = this;
    notificationCentreScreen.textCanvasGroup.alpha = 0.0f;
    notificationCentreScreen._backgroundCanvasGroup.DOKill();
    notificationCentreScreen._backgroundCanvasGroup.alpha = 0.0f;
    notificationCentreScreen._backgroundCanvasGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad);
    yield return (object) new WaitForSecondsRealtime(1f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", (Object) PlayerFarming.Instance != (Object) null ? PlayerFarming.Instance.gameObject : notificationCentreScreen.gameObject);
    notificationCentreScreen.textCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad);
    yield return (object) new WaitForSeconds(2.5f);
    notificationCentreScreen.textCanvasGroup.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuad);
    yield return (object) new WaitForSeconds(0.25f);
    notificationCentreScreen._backgroundCanvasGroup.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuad);
    yield return (object) new WaitForSeconds(1f);
    notificationCentreScreen.Stop();
  }

  public void FadeAndStop()
  {
    this.StopAllCoroutines();
    this.textCanvasGroup.DOKill();
    this.textCanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuad);
    this._backgroundCanvasGroup.DOKill();
    this._backgroundCanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuad).OnComplete<TweenerCore<float, float, FloatOptions>>(new TweenCallback(this.Stop));
  }

  public void Stop()
  {
    this.StopAllCoroutines();
    this.Text.text = "";
    this.container.SetActive(false);
  }
}
