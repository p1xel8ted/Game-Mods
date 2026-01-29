// Decompiled with JetBrains decompiler
// Type: FollowerFaithUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FollowerFaithUI : BaseMonoBehaviour
{
  public FollowerFaithUI.Stats Stat;
  public Image FillImage;
  public Follower Follower;
  public float TargetScale = 0.8f;
  public float Value;

  public void OnEnable()
  {
    this.transform.localScale = Vector3.one * this.TargetScale * 0.5f;
    this.transform.DOScale(Vector3.one * this.TargetScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    switch (this.Stat)
    {
      case FollowerFaithUI.Stats.Hunger:
        if ((double) this.Follower.Brain.Stats.Starvation > 0.0)
          break;
        this.gameObject.SetActive(false);
        break;
      case FollowerFaithUI.Stats.Illness:
        if ((double) this.Follower.Brain.Stats.Illness > 0.0)
          break;
        this.gameObject.SetActive(false);
        break;
      case FollowerFaithUI.Stats.Freezing:
        if ((double) this.Follower.Brain.Stats.Freezing > 0.0)
          break;
        this.gameObject.SetActive(false);
        break;
    }
  }

  public void Update()
  {
    switch (this.Stat)
    {
      case FollowerFaithUI.Stats.Faith:
        this.Value = this.Follower.Brain.Stats.Happiness / 100f;
        this.FillImage.color = this.ReturnColorBasedOnValue(this.Value);
        break;
      case FollowerFaithUI.Stats.Hunger:
        this.Value = (float) (((double) this.Follower.Brain.Stats.Satiation + (75.0 - (double) this.Follower.Brain.Stats.Starvation)) / 175.0);
        this.FillImage.color = this.ReturnColorBasedOnValueHunger(this.Value);
        break;
      case FollowerFaithUI.Stats.Illness:
        this.Value = (float) (1.0 - (double) this.Follower.Brain.Stats.Illness / 100.0);
        this.FillImage.color = this.ReturnColorBasedOnValue(this.Value);
        break;
      case FollowerFaithUI.Stats.Freezing:
        this.Value = (float) (1.0 - (double) this.Follower.Brain.Stats.Freezing / 100.0);
        this.FillImage.color = this.ReturnColorBasedOnValue(this.Value);
        break;
    }
    this.FillImage.fillAmount = this.Value;
  }

  public Color ReturnColorBasedOnValue(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.25)
      return StaticColors.RedColor;
    return (double) f >= 0.25 && (double) f < 0.5 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public Color ReturnColorBasedOnValueHunger(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.5)
      return StaticColors.RedColor;
    return (double) f >= 0.5 && (double) f < 0.75 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public enum Stats
  {
    Faith,
    Hunger,
    Illness,
    Freezing,
  }
}
