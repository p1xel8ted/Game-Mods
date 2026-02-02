// Decompiled with JetBrains decompiler
// Type: BarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
  public float CurrentSize = 0.5f;
  public const float DoubleSizeIncreaseIconBorder = 0.1f;
  public const float DoubleSizeDecreaseIconBorder = -0.1f;
  public DG.Tweening.Sequence sequence;
  public List<AnimatedSizeChange> QueuedChanges = new List<AnimatedSizeChange>();
  public List<NotificationData> QueuedNotifications = new List<NotificationData>();
  [SerializeField]
  public float sequenceInterval = 0.5f;
  [SerializeField]
  public float sequenceDuration = 1f;
  [SerializeField]
  public float faithIconFadeDuration = 0.5f;
  [SerializeField]
  public float faithIconFadeDelay = 1f;
  [SerializeField]
  public TextMeshProUGUI faithIcon;
  [SerializeField]
  public Image faithImage;
  [SerializeField]
  public Sprite faithDoubleUp;
  [SerializeField]
  public Sprite faithUp;
  [SerializeField]
  public Sprite faithDown;
  [SerializeField]
  public Sprite faithDoubleDown;
  [SerializeField]
  public Image warningImage;
  [SerializeField]
  public bool forceVisualArrows;
  public bool SetColor = true;
  public bool checkSizeDifference;
  public bool useFaithDown = true;
  public float _cacheBarSize;
  public bool playingSizeDiff;
  public bool IsPlaying;
  public bool UseQueuing = true;
  public float MinimumDelta;

  public void OnDisable()
  {
    if (this.sequence != null && this.sequence.active)
      this.sequence.Kill();
    if ((UnityEngine.Object) this.WhiteBar != (UnityEngine.Object) null)
    {
      this.WhiteBar.DOKill();
      this.WhiteBar.transform.DOKill();
    }
    if ((UnityEngine.Object) this.RedBar != (UnityEngine.Object) null)
    {
      this.RedBar.transform.DOKill();
      this.RedBar.DOKill();
    }
    this.transform.DOKill();
    if ((UnityEngine.Object) this.transform.GetChild(0) != (UnityEngine.Object) null)
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
    if (!((UnityEngine.Object) this.LockImage != (UnityEngine.Object) null))
      return;
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
      if (NotificationData == null || !(NotificationData.Notification != "") || !(NotificationData.Notification != " "))
        return;
      NotificationCentre.Instance.PlayFaithNotification(NotificationData.Notification, NotificationData.DeltaDisplay, NotificationData.Flair, NotificationData.FollowerID, NotificationData.ExtraText);
    }
    else
    {
      if (Animate)
      {
        float sizeFromLastChange = this.GetSizeFromLastChange();
        AnimatedSizeChange animatedSizeChange = new AnimatedSizeChange(Size, sizeFromLastChange);
        if ((double) Mathf.Abs(animatedSizeChange.Difference) <= 0.0)
          return;
        this.QueuedChanges.Add(animatedSizeChange);
        if (this.forceVisualArrows && NotificationData == null)
          NotificationData = new NotificationData(" ", sizeFromLastChange, -1, NotificationBase.Flair.None, Array.Empty<string>());
        this.QueuedNotifications.Add(NotificationData);
      }
      if (!((UnityEngine.Object) this.warningImage != (UnityEngine.Object) null))
        return;
      DOTweenModuleUI.DOFade(this.warningImage, 0.75f - Size, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
  }

  public void Update()
  {
    if (this.QueuedChanges.Count <= 0 || this.UseQueuing && (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden || this.IsPlaying || (double) Time.timeScale <= 0.0 || LetterBox.IsPlaying))
      return;
    NotificationData queuedNotification = this.QueuedNotifications[0];
    this.Play(this.QueuedChanges[0], queuedNotification != null ? this.QueuedNotifications[0].Notification : "");
    this.QueuedChanges.RemoveAt(0);
    if (queuedNotification != null && queuedNotification.Notification != "" && queuedNotification.Notification != " ")
      NotificationCentre.Instance.PlayFaithNotification(queuedNotification.Notification, queuedNotification.DeltaDisplay, queuedNotification.Flair, queuedNotification.FollowerID, queuedNotification.ExtraText);
    this.QueuedNotifications.RemoveAt(0);
  }

  public void OnEnable()
  {
    if (!this.SetColor)
      return;
    if ((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null)
      this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
    if (!((UnityEngine.Object) this.warningImage != (UnityEngine.Object) null))
      return;
    this.warningImage.color = new Color(1f, 0.0f, 0.0f, 0.0f);
  }

  public void Play(AnimatedSizeChange animatedSizeChange, string Notification = "")
  {
    this.IsPlaying = true;
    if (this.sequence != null && this.sequence.active)
      this.sequence.Kill();
    this.WhiteBar.transform.DOKill();
    this.RedBar.transform.DOKill();
    this.transform.DOKill();
    this.RedBar.DOKill();
    if ((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null && this.SetColor)
      this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
    if ((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null && Notification != "")
    {
      if ((double) animatedSizeChange.Difference > 0.004999999888241291 || (double) animatedSizeChange.Difference < -0.004999999888241291)
      {
        this.transform.GetChild(0).DOKill();
        this.transform.GetChild(0).transform.localPosition = Vector3.zero;
        this.transform.GetChild(0).DOShakePosition(1f, this.Shake).SetUpdate<Tweener>(true);
      }
      this.faithImage.sprite = (Sprite) null;
      if ((double) animatedSizeChange.Difference <= -0.10000000149011612)
        this.faithImage.sprite = this.faithDoubleDown;
      else if ((double) animatedSizeChange.Difference >= 0.10000000149011612)
        this.faithImage.sprite = this.faithDoubleUp;
      else if ((double) animatedSizeChange.Difference < -(double) this.MinimumDelta || (double) animatedSizeChange.Difference == (double) this.MinimumDelta && (double) animatedSizeChange.Size == 0.0)
        this.faithImage.sprite = this.faithDown;
      else if ((double) animatedSizeChange.Difference > (double) this.MinimumDelta || (double) animatedSizeChange.Difference == (double) this.MinimumDelta && (double) animatedSizeChange.Size == 1.0)
        this.faithImage.sprite = this.faithUp;
      if (this.SetColor)
      {
        this.faithIcon.color = Color.white;
        this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
      }
      this.faithIcon.DOKill();
      this.faithImage.DOKill();
      if ((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null)
      {
        ShortcutExtensionsTMPText.DOFade(this.faithIcon, 0.0f, this.faithIconFadeDuration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        DOTweenModuleUI.DOFade(this.faithImage, 1f, this.faithIconFadeDuration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
    }
    if ((double) animatedSizeChange.Size < (double) this.CurrentSize)
    {
      this.RedBar.fillAmount = animatedSizeChange.Size;
      this.sequence = DOTween.Sequence();
      this.sequence.AppendInterval(this.sequenceInterval);
      this.sequence.SetUpdate<DG.Tweening.Sequence>(true);
      this.sequence.Append((Tween) this.WhiteBar.DOFillAmount(animatedSizeChange.Size, this.sequenceDuration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        this.IsPlaying = false;
        if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null))
          return;
        if ((UnityEngine.Object) this.faithIcon != (UnityEngine.Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay);
        if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null))
          return;
        DOTweenModuleUI.DOFade(this.faithImage, 0.0f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay);
      }));
    }
    else if ((double) animatedSizeChange.Size > (double) this.CurrentSize)
    {
      this.WhiteBar.fillAmount = animatedSizeChange.Size;
      this.sequence = DOTween.Sequence();
      if (this.WhiteBar.gameObject.activeSelf)
        this.sequence.AppendInterval(this.sequenceInterval);
      this.sequence.Append((Tween) this.RedBar.DOFillAmount(animatedSizeChange.Size, this.sequenceDuration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        this.IsPlaying = false;
        if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null))
          return;
        if ((UnityEngine.Object) this.faithIcon != (UnityEngine.Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null))
          return;
        DOTweenModuleUI.DOFade(this.faithImage, 0.0f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }));
    }
    else
    {
      this.IsPlaying = false;
      this.sequence = DOTween.Sequence();
      this.sequence.AppendInterval(this.sequenceInterval);
      this.sequence.Append((Tween) this.RedBar.DOFillAmount(animatedSizeChange.Size, this.sequenceDuration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null))
          return;
        if ((UnityEngine.Object) this.faithIcon != (UnityEngine.Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null))
          return;
        DOTweenModuleUI.DOFade(this.faithImage, 0.0f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }));
    }
    if (this.SetColor)
      this.RedBar.color = StaticColors.ColorForThreshold(animatedSizeChange.Size);
    this.CurrentSize = animatedSizeChange.Size;
  }

  public void ShrinkBarToEmpty(float duration)
  {
    this.WhiteBar.fillAmount = 0.0f;
    this.RedBar.DOFillAmount(0.0f, duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.enabled = true));
  }

  public float GetSizeFromLastChange()
  {
    return this.QueuedChanges != null && this.QueuedChanges.Count > 0 ? this.QueuedChanges[this.QueuedChanges.Count - 1].Size : this.CurrentSize;
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__37_0()
  {
    this.IsPlaying = false;
    if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.faithIcon != (UnityEngine.Object) null)
      ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay);
    if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null))
      return;
    DOTweenModuleUI.DOFade(this.faithImage, 0.0f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__37_1()
  {
    this.IsPlaying = false;
    if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.faithIcon != (UnityEngine.Object) null)
      ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null))
      return;
    DOTweenModuleUI.DOFade(this.faithImage, 0.0f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__37_2()
  {
    if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null) || !((UnityEngine.Object) this.faithImage.sprite != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.faithIcon != (UnityEngine.Object) null)
      ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    if (!((UnityEngine.Object) this.faithImage != (UnityEngine.Object) null))
      return;
    DOTweenModuleUI.DOFade(this.faithImage, 0.0f, this.faithIconFadeDuration).SetDelay<TweenerCore<Color, Color, ColorOptions>>(this.faithIconFadeDelay).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  [CompilerGenerated]
  public void \u003CShrinkBarToEmpty\u003Eb__38_0() => this.enabled = true;
}
