// Decompiled with JetBrains decompiler
// Type: BarControllerNonUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class BarControllerNonUI : MonoBehaviour
{
  public SpriteRenderer WhiteBar;
  public SpriteRenderer RedBar;
  public SpriteRenderer Minimum;
  public SpriteRenderer Maximum;
  public GameObject LockImage;
  public Vector3 Shake = new Vector3(0.0f, 10f);
  public float CurrentSize = 0.5f;
  public const float DoubleSizeIncreaseIconBorder = 0.1f;
  public const float DoubleSizeDecreaseIconBorder = -0.1f;
  public DG.Tweening.Sequence sequence;
  public List<AnimatedSizeChange> QueuedChanges = new List<AnimatedSizeChange>();
  public List<NotificationData> QueuedNotifications = new List<NotificationData>();
  [SerializeField]
  public TextMeshPro faithIcon;
  [SerializeField]
  public SpriteRenderer faithImage;
  [SerializeField]
  public Sprite faithDoubleUp;
  [SerializeField]
  public Sprite faithUp;
  [SerializeField]
  public Sprite faithDown;
  [SerializeField]
  public Sprite faithDoubleDown;
  [SerializeField]
  public SpriteRenderer warningImage;
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
    this.RedBar.transform.localScale = new Vector3(RedBarSize, this.RedBar.transform.localScale.y, this.RedBar.transform.localScale.z);
    if (this.SetColor)
      this.RedBar.color = StaticColors.ColorForThreshold(RedBarSize);
    this.WhiteBar.DOKill();
    this.WhiteBar.transform.DOScaleX(WhiteSize, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.LockImage.SetActive(ShowLock);
  }

  public void SetBarSize(float Size, bool Animate, bool Force = false, NotificationData NotificationData = null)
  {
    Size = Mathf.Clamp01(Size);
    if (!Animate)
    {
      this.RedBar.transform.localScale = new Vector3(Size, this.RedBar.transform.localScale.y, this.RedBar.transform.localScale.z);
      this.WhiteBar.transform.localScale = new Vector3(Size, this.WhiteBar.transform.localScale.y, this.WhiteBar.transform.localScale.z);
      if (this.SetColor)
        this.RedBar.color = StaticColors.ColorForThreshold(Size);
      this.CurrentSize = Size;
    }
    if ((double) this.CurrentSize == (double) Size && (double) this.CurrentSize != 1.0 && (double) this.CurrentSize != 0.0 && !Force)
    {
      this.RedBar.transform.localScale = new Vector3(Size, this.RedBar.transform.localScale.y, this.RedBar.transform.localScale.z);
      this.WhiteBar.transform.localScale = new Vector3(Size, this.WhiteBar.transform.localScale.y, this.WhiteBar.transform.localScale.z);
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
        float sizeFromLastChange = this.GetSizeFromLastChange();
        this.QueuedChanges.Add(new AnimatedSizeChange(Size, sizeFromLastChange));
        this.QueuedNotifications.Add(NotificationData);
      }
      if (!((Object) this.warningImage != (Object) null))
        return;
      this.warningImage.DOFade(0.75f - Size, 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
  }

  public void Update()
  {
    if (this.QueuedChanges.Count <= 0 || this.UseQueuing && (HUD_Manager.IsTransitioning || HUD_Manager.Instance.Hidden || this.IsPlaying || (double) Time.timeScale <= 0.0 || LetterBox.IsPlaying))
      return;
    if (this.gameObject.name == "Faith Bar")
      Debug.Log((object) $"{this.gameObject.name}{"  PLAY QUEUED!".Colour(Color.magenta)}  {this.QueuedChanges[0].ToString()} count: {this.QueuedChanges.Count.ToString()}  {(this.QueuedNotifications[0] != null ? this.QueuedNotifications[0].Notification : "null")} count:{this.QueuedNotifications.Count.ToString()}");
    NotificationData queuedNotification = this.QueuedNotifications[0];
    this.Play(this.QueuedChanges[0], queuedNotification != null ? this.QueuedNotifications[0].Notification : "");
    this.QueuedChanges.RemoveAt(0);
    if (queuedNotification != null && queuedNotification.Notification != "")
      NotificationCentre.Instance.PlayFaithNotification(queuedNotification.Notification, queuedNotification.DeltaDisplay, queuedNotification.Flair, queuedNotification.FollowerID, queuedNotification.ExtraText);
    this.QueuedNotifications.RemoveAt(0);
  }

  public void OnEnable()
  {
    if (!this.SetColor)
      return;
    if ((Object) this.faithImage != (Object) null)
      this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
    if (!((Object) this.warningImage != (Object) null))
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
    if ((Object) this.faithImage != (Object) null && this.SetColor)
      this.faithImage.color = new Color(1f, 1f, 1f, 0.0f);
    if ((Object) this.faithImage != (Object) null && Notification != "")
    {
      if ((double) animatedSizeChange.Difference > 0.004999999888241291 || (double) animatedSizeChange.Difference < -0.004999999888241291)
      {
        Debug.Log((object) "Play Shake");
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
      if ((Object) this.faithImage.sprite != (Object) null)
      {
        ShortcutExtensionsTMPText.DOFade(this.faithIcon, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.faithImage.DOFade(1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
    }
    if ((double) animatedSizeChange.Size < (double) this.CurrentSize)
    {
      this.RedBar.transform.localScale = new Vector3(animatedSizeChange.Size, this.RedBar.transform.localScale.y, this.RedBar.transform.localScale.z);
      this.sequence = DOTween.Sequence();
      this.sequence.AppendInterval(0.5f);
      this.sequence.SetUpdate<DG.Tweening.Sequence>(true);
      this.sequence.Append((Tween) this.WhiteBar.transform.DOScaleX(animatedSizeChange.Size, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        this.IsPlaying = false;
        if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
          return;
        if ((Object) this.faithIcon != (Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
        if (!((Object) this.faithImage != (Object) null))
          return;
        this.faithImage.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
      }));
    }
    else if ((double) animatedSizeChange.Size > (double) this.CurrentSize)
    {
      this.WhiteBar.transform.localScale = new Vector3(animatedSizeChange.Size, this.WhiteBar.transform.localScale.y, this.WhiteBar.transform.localScale.z);
      this.sequence = DOTween.Sequence();
      if (this.WhiteBar.gameObject.activeSelf)
        this.sequence.AppendInterval(0.5f);
      this.sequence.Append((Tween) this.RedBar.transform.DOScaleX(animatedSizeChange.Size, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        this.IsPlaying = false;
        if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
          return;
        if ((Object) this.faithIcon != (Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        if (!((Object) this.faithImage != (Object) null))
          return;
        this.faithImage.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }));
    }
    else
    {
      this.IsPlaying = false;
      this.sequence = DOTween.Sequence();
      this.sequence.AppendInterval(0.5f);
      this.sequence.Append((Tween) this.RedBar.transform.DOScaleX(animatedSizeChange.Size, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
      this.sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
      {
        if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
          return;
        if ((Object) this.faithIcon != (Object) null)
          ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        if (!((Object) this.faithImage != (Object) null))
          return;
        this.faithImage.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }));
    }
    if (this.SetColor)
      this.RedBar.color = StaticColors.ColorForThreshold(animatedSizeChange.Size);
    this.CurrentSize = animatedSizeChange.Size;
  }

  public void ShrinkBarToEmpty(float duration)
  {
    this.gameObject.SetActive(true);
    this.WhiteBar.transform.localScale = new Vector3(0.0f, this.WhiteBar.transform.localScale.y, this.WhiteBar.transform.localScale.z);
    this.RedBar.transform.DOScaleX(0.0f, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.enabled = true));
  }

  public float GetSizeFromLastChange()
  {
    return this.QueuedChanges != null && this.QueuedChanges.Count > 0 ? this.QueuedChanges[this.QueuedChanges.Count - 1].Size : this.CurrentSize;
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__32_0()
  {
    this.IsPlaying = false;
    if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
      return;
    if ((Object) this.faithIcon != (Object) null)
      ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
    if (!((Object) this.faithImage != (Object) null))
      return;
    this.faithImage.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__32_1()
  {
    this.IsPlaying = false;
    if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
      return;
    if ((Object) this.faithIcon != (Object) null)
      ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    if (!((Object) this.faithImage != (Object) null))
      return;
    this.faithImage.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__32_2()
  {
    if (!((Object) this.faithImage != (Object) null) || !((Object) this.faithImage.sprite != (Object) null))
      return;
    if ((Object) this.faithIcon != (Object) null)
      ShortcutExtensionsTMPText.DOFade(this.faithIcon, 1f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    if (!((Object) this.faithImage != (Object) null))
      return;
    this.faithImage.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  [CompilerGenerated]
  public void \u003CShrinkBarToEmpty\u003Eb__33_0() => this.enabled = true;
}
