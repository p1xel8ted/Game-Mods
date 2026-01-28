// Decompiled with JetBrains decompiler
// Type: UISchedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class UISchedule : BaseMonoBehaviour
{
  public Animator Animator;
  public UINavigator uiNavigator;
  public TextMeshProUGUI DawnLabel;
  public TextMeshProUGUI MorningLabel;
  public TextMeshProUGUI AfternoonLabel;
  public TextMeshProUGUI DuskLabel;
  public TextMeshProUGUI NightLabel;
  public TextMeshProUGUI MealTimeLabel;
  public System.Action CallbackClose;
  public System.Action _callback;
  public Dictionary<DayPhase, ScheduledActivity> _workingSchedule;
  public Dictionary<InstantActivity, int> _workingInstantActivities;

  public void Start()
  {
    this.uiNavigator.OnClose += new UINavigator.Close(this.Close);
    this._workingSchedule = new Dictionary<DayPhase, ScheduledActivity>();
    for (int index = 0; index < 5; ++index)
      this._workingSchedule[(DayPhase) index] = TimeManager.GetScheduledActivity((DayPhase) index);
    this._workingInstantActivities = new Dictionary<InstantActivity, int>();
    for (int index = 0; index < 1; ++index)
      this._workingInstantActivities[(InstantActivity) index] = TimeManager.GetInstantActivityHour((InstantActivity) index);
    this.UpdateLabels();
  }

  public void Update()
  {
    if (DataManager.Instance.SchedulingEnabled)
    {
      if (this.uiNavigator.CurrentSelection < 5)
      {
        DayPhase currentSelection = (DayPhase) this.uiNavigator.CurrentSelection;
        if (InputManager.UI.GetPageNavigateLeftDown())
          this.PrevActivity(currentSelection);
        if (InputManager.UI.GetPageNavigateRightDown())
          this.NextActivity(currentSelection);
      }
      else
      {
        InstantActivity activity = (InstantActivity) (this.uiNavigator.CurrentSelection - 5);
        if (InputManager.UI.GetPageNavigateLeftDown())
          this.PrevHour(activity);
        if (InputManager.UI.GetPageNavigateRightDown())
          this.NextHour(activity);
      }
    }
    this.DawnLabel.color = TimeManager.CurrentPhase == DayPhase.Dawn ? Color.yellow : Color.white;
    this.MorningLabel.color = TimeManager.CurrentPhase == DayPhase.Morning ? Color.yellow : Color.white;
    this.AfternoonLabel.color = TimeManager.CurrentPhase == DayPhase.Afternoon ? Color.yellow : Color.white;
    this.DuskLabel.color = TimeManager.CurrentPhase == DayPhase.Dusk ? Color.yellow : Color.white;
    this.NightLabel.color = TimeManager.CurrentPhase == DayPhase.Night ? Color.yellow : Color.white;
  }

  public void OnDisable() => this.uiNavigator.OnClose -= new UINavigator.Close(this.Close);

  public void SelectDawn() => this.NextActivity(DayPhase.Dawn);

  public void SelectMorning() => this.NextActivity(DayPhase.Morning);

  public void SelectAfternoon() => this.NextActivity(DayPhase.Afternoon);

  public void SelectDusk() => this.NextActivity(DayPhase.Dusk);

  public void SelectNight() => this.NextActivity(DayPhase.Night);

  public void SelectMealTime() => this.NextHour(InstantActivity.MealTime);

  public void NextActivity(DayPhase phase)
  {
    if (!DataManager.Instance.SchedulingEnabled)
      return;
    ScheduledActivity scheduledActivity = (ScheduledActivity) ((int) (this._workingSchedule[phase] + 1) % 5);
    this._workingSchedule[phase] = scheduledActivity;
    this.UpdateLabels();
  }

  public void PrevActivity(DayPhase phase)
  {
    if (!DataManager.Instance.SchedulingEnabled)
      return;
    ScheduledActivity scheduledActivity1 = this._workingSchedule[phase];
    ScheduledActivity scheduledActivity2 = (ScheduledActivity) ((scheduledActivity1 == ScheduledActivity.Work ? 5 : (int) scheduledActivity1) - 1);
    this._workingSchedule[phase] = scheduledActivity2;
    this.UpdateLabels();
  }

  public void NextHour(InstantActivity activity)
  {
    if (!DataManager.Instance.SchedulingEnabled)
      return;
    int num = (this._workingInstantActivities[activity] + 1) % 20;
    this._workingInstantActivities[activity] = num;
    this.UpdateLabels();
  }

  public void PrevHour(InstantActivity activity)
  {
    if (!DataManager.Instance.SchedulingEnabled)
      return;
    int workingInstantActivity = this._workingInstantActivities[activity];
    int num = (workingInstantActivity == 0 ? 20 : workingInstantActivity) - 1;
    this._workingInstantActivities[activity] = num;
    this.UpdateLabels();
  }

  public void UpdateLabels()
  {
    this.DawnLabel.text = $"Dawn: {this._workingSchedule[DayPhase.Dawn]}";
    this.MorningLabel.text = $"Morning: {this._workingSchedule[DayPhase.Morning]}";
    this.AfternoonLabel.text = $"Afternoon: {this._workingSchedule[DayPhase.Afternoon]}";
    this.DuskLabel.text = $"Dusk: {this._workingSchedule[DayPhase.Dusk]}";
    this.NightLabel.text = $"Night: {this._workingSchedule[DayPhase.Night]}";
    this.MealTimeLabel.text = $"MealTime: {this._workingInstantActivities[InstantActivity.MealTime]:D2}:00";
  }

  public void Close()
  {
    this._callback = this.CallbackClose;
    this.StartCoroutine((IEnumerator) this.CloseRoutine());
    if (!DataManager.Instance.SchedulingEnabled)
      return;
    bool flag = this._workingSchedule[TimeManager.CurrentPhase] != TimeManager.GetScheduledActivity(TimeManager.CurrentPhase);
    foreach (KeyValuePair<DayPhase, ScheduledActivity> keyValuePair in this._workingSchedule)
      TimeManager.SetScheduledActivity(keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<InstantActivity, int> workingInstantActivity in this._workingInstantActivities)
      TimeManager.SetInstantActivity(workingInstantActivity.Key, workingInstantActivity.Value);
    if (flag)
    {
      TimeManager.SetOverrideScheduledActivity(ScheduledActivity.None);
    }
    else
    {
      System.Action onScheduleChanged = TimeManager.OnScheduleChanged;
      if (onScheduleChanged == null)
        return;
      onScheduleChanged();
    }
  }

  public IEnumerator CloseRoutine()
  {
    UISchedule uiSchedule = this;
    uiSchedule.Animator.Play("Base Layer.Out");
    yield return (object) new WaitForSeconds(0.5f);
    System.Action callback = uiSchedule._callback;
    if (callback != null)
      callback();
    UnityEngine.Object.Destroy((UnityEngine.Object) uiSchedule.gameObject);
  }
}
