// Decompiled with JetBrains decompiler
// Type: FollowerExclamationMark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerExclamationMark : BaseMonoBehaviour
{
  public Follower follower;
  public GameObject Image;
  public GameObject FoodIcon;
  public SpriteRenderer FoodIconRing;
  public GameObject HappinessIcon;
  public SpriteRenderer HappinessIconRing;
  public GameObject SleepinessIcon;
  public SpriteRenderer SleepinesssIconRing;
  public GameObject IllnessIcon;
  public SpriteRenderer IllnessIconRing;
  public Gradient RadialGradient;

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.Init());

  private IEnumerator Init()
  {
    FollowerExclamationMark followerExclamationMark = this;
    followerExclamationMark.Image.SetActive(false);
    followerExclamationMark.FoodIcon.SetActive(false);
    followerExclamationMark.HappinessIcon.SetActive(false);
    followerExclamationMark.SleepinessIcon.SetActive(false);
    followerExclamationMark.IllnessIcon.SetActive(false);
    yield return (object) new WaitForEndOfFrame();
    followerExclamationMark.GetNotify();
    followerExclamationMark.OnSatiationStateChanged(followerExclamationMark.follower.Brain.Info.ID, (double) followerExclamationMark.follower.Brain.Stats.Satiation <= 30.0 ? FollowerStatState.On : FollowerStatState.Off, FollowerStatState.Off);
    followerExclamationMark.follower.Brain.Info.OnReadyToPromote += new System.Action(followerExclamationMark.GetNotify);
    followerExclamationMark.follower.Brain.Info.OnPromotion += new System.Action(followerExclamationMark.GetNotify);
    FollowerBrainStats.OnSatiationStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerExclamationMark.OnSatiationStateChanged);
    followerExclamationMark.FoodIconRing.material.SetFloat("_Angle", 90f);
    FollowerBrainStats.OnHappinessStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerExclamationMark.OnHappinessStateChanged);
    followerExclamationMark.HappinessIconRing.material.SetFloat("_Angle", 90f);
    FollowerBrainStats.OnRestStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerExclamationMark.OnRestStateChanged);
    followerExclamationMark.SleepinesssIconRing.material.SetFloat("_Angle", 90f);
    FollowerBrainStats.OnIllnessStateChanged += new FollowerBrainStats.StatStateChangedEvent(followerExclamationMark.OnIllnessStateChanged);
    followerExclamationMark.IllnessIconRing.material.SetFloat("_Angle", 90f);
    TimeManager.OnNewPhaseStarted += new System.Action(followerExclamationMark.OnNewPhaseStarted);
    TimeManager.OnScheduleChanged += new System.Action(followerExclamationMark.OnNewPhaseStarted);
    if ((double) followerExclamationMark.follower.Brain.Stats.Satiation <= 0.0)
      followerExclamationMark.FoodIcon.SetActive(true);
    if ((double) followerExclamationMark.follower.Brain.Stats.Happiness <= 25.0)
      followerExclamationMark.HappinessIcon.SetActive(true);
    if ((double) followerExclamationMark.follower.Brain.Stats.Rest <= 20.0)
      followerExclamationMark.SleepinessIcon.SetActive(true);
    if ((double) followerExclamationMark.follower.Brain.Stats.Illness > 0.0)
      followerExclamationMark.IllnessIcon.SetActive(true);
  }

  public void ShowIcon(FollowerExclamationMark.IconType Type)
  {
    switch (Type)
    {
      case FollowerExclamationMark.IconType.Sleep:
        this.SleepinessIcon.SetActive(true);
        break;
      case FollowerExclamationMark.IconType.Food:
        this.FoodIcon.SetActive(true);
        break;
      case FollowerExclamationMark.IconType.Happiness:
        this.HappinessIcon.SetActive(true);
        break;
      case FollowerExclamationMark.IconType.Illness:
        this.IllnessIcon.SetActive(true);
        break;
    }
  }

  public void HideIcon(FollowerExclamationMark.IconType Type)
  {
    switch (Type)
    {
      case FollowerExclamationMark.IconType.Sleep:
        if ((double) this.follower.Brain.Stats.Rest <= 20.0)
          break;
        this.SleepinessIcon.SetActive(false);
        break;
      case FollowerExclamationMark.IconType.Food:
        if ((double) this.follower.Brain.Stats.Satiation <= 30.0)
          break;
        this.FoodIcon.SetActive(false);
        break;
      case FollowerExclamationMark.IconType.Happiness:
        if ((double) this.follower.Brain.Stats.Happiness <= 25.0)
          break;
        this.HappinessIcon.SetActive(false);
        break;
    }
  }

  private void OnNewPhaseStarted()
  {
    int scheduledActivity = (int) TimeManager.GetScheduledActivity(this.follower.Brain.HomeLocation);
    if (scheduledActivity == 4)
      this.HappinessIcon.SetActive(true);
    else if ((double) this.follower.Brain.Stats.Happiness > 25.0)
      this.HappinessIcon.SetActive(false);
    if (scheduledActivity == 4)
    {
      this.HappinessIcon.SetActive(true);
    }
    else
    {
      if ((double) this.follower.Brain.Stats.Happiness <= 25.0)
        return;
      this.HappinessIcon.SetActive(false);
    }
  }

  private void OnHappinessStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.follower.Brain.Info.ID || TimeManager.GetScheduledActivity(this.follower.Brain.HomeLocation) == ScheduledActivity.Leisure)
      return;
    this.HappinessIcon.SetActive(newState != 0);
  }

  private void OnSatiationStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.follower.Brain.Info.ID)
      return;
    this.FoodIcon.SetActive(newState == FollowerStatState.On);
  }

  private void OnRestStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.follower.Brain.Info.ID)
      return;
    this.SleepinessIcon.SetActive(newState == FollowerStatState.On);
  }

  private void OnIllnessStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.follower.Brain.Info.ID)
      return;
    this.IllnessIcon.SetActive(newState == FollowerStatState.On);
  }

  private void Update()
  {
    this.FoodIconRing.material.SetFloat("_Arc1", (float) (360.0 - ((double) this.follower.Brain.Stats.Satiation + (75.0 - (double) this.follower.Brain.Stats.Starvation)) / 175.0 * 360.0));
    this.FoodIconRing.material.SetFloat("_Arc2", 0.0f);
    this.FoodIconRing.color = this.RadialGradient.Evaluate((float) (((double) this.follower.Brain.Stats.Satiation + (75.0 - (double) this.follower.Brain.Stats.Starvation)) / 175.0));
    this.HappinessIconRing.material.SetFloat("_Arc1", (float) (360.0 - (double) this.follower.Brain.Stats.Happiness / 100.0 * 360.0));
    this.HappinessIconRing.material.SetFloat("_Arc2", 0.0f);
    this.HappinessIconRing.color = this.RadialGradient.Evaluate(this.follower.Brain.Stats.Happiness / 100f);
    this.SleepinesssIconRing.material.SetFloat("_Arc1", (float) (360.0 - (double) this.follower.Brain.Stats.Rest / 100.0 * 360.0));
    this.SleepinesssIconRing.material.SetFloat("_Arc2", 0.0f);
    this.SleepinesssIconRing.color = this.RadialGradient.Evaluate(this.follower.Brain.Stats.Rest / 100f);
    this.IllnessIconRing.material.SetFloat("_Arc1", (float) (360.0 - (1.0 - (double) this.follower.Brain.Stats.Illness / 100.0) * 360.0));
    this.IllnessIconRing.material.SetFloat("_Arc2", 0.0f);
    this.IllnessIconRing.color = this.RadialGradient.Evaluate((float) (1.0 - (double) this.follower.Brain.Stats.Illness / 100.0));
  }

  private void OnDisable()
  {
    this.follower.Brain.Info.OnReadyToPromote -= new System.Action(this.GetNotify);
    this.follower.Brain.Info.OnPromotion -= new System.Action(this.GetNotify);
    FollowerBrainStats.OnSatiationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnSatiationStateChanged);
    FollowerBrainStats.OnHappinessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnHappinessStateChanged);
    FollowerBrainStats.OnRestStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnRestStateChanged);
    FollowerBrainStats.OnIllnessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    TimeManager.OnScheduleChanged -= new System.Action(this.OnNewPhaseStarted);
  }

  private void GetNotify(int FollowerID)
  {
    if (FollowerID != this.follower.Brain.Info.ID)
      return;
    this.GetNotify();
  }

  private void GetNotify() => this.Hide();

  private void Show()
  {
    if (this.Image.activeSelf)
      return;
    this.StartCoroutine((IEnumerator) this.ShowRoutine());
  }

  private IEnumerator ShowRoutine()
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.Image.transform.localScale = Vector3.one * Mathf.SmoothStep(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    this.Image.SetActive(true);
  }

  public void HideAll() => this.gameObject.SetActive(false);

  public void ShowAll() => this.gameObject.SetActive(true);

  private void Hide()
  {
    if (!this.Image.activeSelf)
      return;
    this.StartCoroutine((IEnumerator) this.HideRoutine());
  }

  private IEnumerator HideRoutine()
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.Image.transform.localScale = Vector3.one * Mathf.SmoothStep(1f, 0.0f, Progress / Duration);
      yield return (object) null;
    }
    this.Image.SetActive(false);
  }

  public enum IconType
  {
    Sleep,
    Food,
    Happiness,
    Illness,
  }
}
