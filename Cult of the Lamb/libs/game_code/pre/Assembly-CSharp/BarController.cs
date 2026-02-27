// Decompiled with JetBrains decompiler
// Type: BarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BarController : MonoBehaviour
{
  public Image WhiteBar;
  public Image RedBar;
  public Image Minimum;
  public Image Maximum;
  public GameObject LockImage;
  public Vector3 Shake = new Vector3(0.0f, 10f);
  private float CurrentSize = 0.5f;
  private DG.Tweening.Sequence sequence;
  public List<float> QueuedChanges = new List<float>();
  public List<NotificationData> QueuedNotifications = new List<NotificationData>();
  [SerializeField]
  private TextMeshProUGUI faithIcon;
  [SerializeField]
  private Image faithImage;
  [SerializeField]
  private Sprite faithDoubleUp;
  [SerializeField]
  private Sprite faithUp;
  [SerializeField]
  private Sprite faithDown;
  [SerializeField]
  private Sprite faithDoubleDown;
  [SerializeField]
  private Image warningImage;
  public bool SetColor = true;
  public bool checkSizeDifference;
  public bool useFaithDown = true;
  private float _cacheBarSize;
  private bool playingSizeDiff;
  public bool IsPlaying;
  public bool UseQueuing = true;
  public float MinimumDelta;

  private void OnDisable()
  {
    if (this.sequence != null)
      this.sequence.Kill();
    if ((Object) this.WhiteBar != (Object) null)
    {
      this.WhiteBar.DOKill();
      this.WhiteBar.transform.DOKill();
    }
    if ((Object) this.RedBar != (Object) null)
    {
      this.RedBar.transform.DOKill();
      this.RedBar.DOKill();
    }
    this.transform.DOKill();
    if ((Object) this.transform.GetChild(0) != (Object) null)
      this.transform.GetChild(0).DOKill();
    this.sequence = (DG.Tweening.Sequence) null;
  }

  public void SetBarSizeForInfo(float WhiteSize, float RedBarSize, bool ShowLock)
  {
    this.RedBar.fillAmount = RedBarSize;
    if (this.SetColor)
      this.RedBar.color = StaticColors.ColorForThreshold(RedBarSize);
    this.WhiteBar.DOKill();
    this.WhiteBar.DOFillAmount(WhiteSize, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.LockImage.SetActive(ShowLock);
  }

  public void SetBarSize(float Size, bool Animate, bool Force = false, NotificationData NotificationData = null)
  {
    Size = Mathf.Clamp01(Size);
    if (!Animate)
    {
      this.RedBar.fillAmount = Size;
      this.WhiteBar.fillAmount = Size;
      if (this.SetColor)
        this.RedBar.color = StaticColors.ColorForThreshold(Size);
      this.CurrentSize = Size;
    }
    if ((double) this.CurrentSize == (double) Size && (double) this.CurrentSize != 1.0 && (double) this.CurrentSize != 0.0 && !Force)
    {
      this.WhiteBar.fillAmount = Size;
      this.RedBar.fillAmount = Size;
      if (this.SetColor)
        this.RedBar.color = StaticColors.ColorForThreshold(Size);
      if (NotificationData == null || !(NotificationData.Notification != ""))
        return;
      NotificationCentre.Instance.PlayFaithNotification(NotificationData.Notification, NotificationData.DeltaDisplay, NotificationData.Flair, NotificationData.FollowerID, NotificationData.ExtraText);
    }
    else
    {
      if (Animate)
      {
        this.QueuedChanges.Add(Size);
        this.QueuedNotifications.Add(NotificationData);
      }
      if (!((Object) this.warningImage != (Object) null))
        return;
      DOTweenModuleUI.DOFade(this.warningImage, 0.75f - Size, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
  }

  public void Update()
  {
    if (this.QueuedChanges.Count <= 0 || this.UseQueuing && (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden || this.IsPlaying || (double) Time.timeScale <= 0.0 || LetterBox.IsPlaying))
      return;
    if (this.gameObject.name == "Faith Bar")
      Debug.Log((object) $"{this.gameObject.name}{"  PLAY QUEUED!".Colour(Color.magenta)}  {(object) this.QueuedChanges[0]} count: {(object) this.QueuedChanges.Count}  {(this.QueuedNotifications[0] != null ? (object) this.QueuedNotifications[0].Notification : (object) "null")} count:{(object) this.QueuedNotifications.Count}");
    NotificationData queuedNotification = this.QueuedNotifications[0];
    this.Play(this.QueuedChanges[0], queuedNotification != null ? this.QueuedNotifications[0].Notification : "");
    this.QueuedChanges.RemoveAt(0);
    if (queuedNotification != null && queuedNotification.Notification != "")
      NotificationCentre.Instance.PlayFaithNotification(queuedNotification.Notification, queuedNotification.DeltaDisplay, queuedNotification.Flair, queuedNotification.FollowerID, queuedNotification.ExtraText);
    this.QueuedNotifications.RemoveAt(0);
  }

  private void OnEnable()
  {
    if (!this.SetColor)
      return;
    if ((Object) this.faithImage != (Object) null)
      this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
    if (!((Object) this.warningImage != (Object) null))
      return;
    this.warningImage.color = new Color(1f, 0.0f, 0.0f, 0.0f);
  }

  public void Play(float Size, string Notification = "")
  {
    this.IsPlaying = true;
    this.sequence.Kill();
    this.WhiteBar.transform.DOKill();
    this.RedBar.transform.DOKill();
    this.transform.DOKill();
    this.RedBar.DOKill();
    if ((Object) this.faithImage != (Object) null && this.SetColor)
      this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
    if ((Object) this.faithImage != (Object) null && Notification != "")
    {
      float num = Size - this.CurrentSize;
      Debug.Log((object) ("diff = " + (object) num));
      if ((double) num > 0.004999999888241291 || (double) num < -0.004999999888241291)
      {
        Debug.Log((object) "Play Shake");
        this.transform.GetChild(0).DOKill();
        this.transform.GetChild(0).transform.localPosition = Vector3.zero;
        this.transform.GetChild(0).DOShakePosition(1f, this.Shake).SetUpdate<Tweener>(true);
      }
      this.faithImage.sprite = (Sprite) null;
      if ((double) num <= -10.0)
        this.faithImage.sprite = this.faithDoubleDown;
      else if ((double) num >= 10.0)
        this.faithImage.sprite = this.faithDoubleUp;
      else if ((double) num < -(double) this.MinimumDelta || (double) num == (double) this.MinimumDelta && (double) Size == 0.0)
        this.faithImage.sprite = this.faithDown;
      else if ((double) num > (double) this.MinimumDelta || (double) num == (double) this.MinimumDelta && (double) Size == 1.0)
        this.faithImage.sprite = this.faithUp;
      if (this.SetColor)
      {
        this.faithIcon.color = Color.white;
        this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
      }
      this.faithIcon.DOKill();
      this.faithImage.DOKill();
      if ((Object) this.faithImage.sprite != (Object) null)
      {
        ShortcutExtensionsTMPText.DOFade(this.faithIcon, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        DOTweenModuleUI.DOFade(this.faithImage, 1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
    }
    if ((double) Size < (double) this.CurrentSize)
    {
      this.RedBar.fillAmount = Size;
      this.sequence = DOTween.Sequence();
      this.sequence.AppendInterval(0.5f);
      this.sequence.SetUpdate<DG.Tweening.Sequence>(true);
      this.sequence.Append((Tween) this.WhiteBar.DOFillAmount(Size, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        this.IsPlaying = false;
        if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
          return;
        if ((Object) this.faithIcon != (Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
        if (!((Object) this.faithImage != (Object) null))
          return;
        DOTweenModuleUI.DOFade(this.faithImage, 0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
      }));
    }
    else if ((double) Size > (double) this.CurrentSize)
    {
      this.WhiteBar.fillAmount = Size;
      this.sequence = DOTween.Sequence();
      if (this.WhiteBar.gameObject.activeSelf)
        this.sequence.AppendInterval(0.5f);
      this.sequence.Append((Tween) this.RedBar.DOFillAmount(Size, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        this.IsPlaying = false;
        if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
          return;
        if ((Object) this.faithIcon != (Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        if (!((Object) this.faithImage != (Object) null))
          return;
        DOTweenModuleUI.DOFade(this.faithImage, 0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }));
    }
    else
    {
      this.IsPlaying = false;
      this.sequence = DOTween.Sequence();
      this.sequence.AppendInterval(0.5f);
      this.sequence.Append((Tween) this.RedBar.DOFillAmount(Size, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
          return;
        if ((Object) this.faithIcon != (Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        if (!((Object) this.faithImage != (Object) null))
          return;
        DOTweenModuleUI.DOFade(this.faithImage, 0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }));
    }
    if (this.SetColor)
      this.RedBar.color = StaticColors.ColorForThreshold(Size);
    this.CurrentSize = Size;
  }

  public void ShrinkBarToEmpty(float duration)
  {
    this.WhiteBar.fillAmount = 0.0f;
    this.RedBar.DOFillAmount(0.0f, duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.enabled = true));
  }
}
