// Decompiled with JetBrains decompiler
// Type: HUD_GameTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Text;
using TMPro;
using UnityEngine.UI;

#nullable disable
public class HUD_GameTime : BaseMonoBehaviour
{
  public TextMeshProUGUI DayLabel;
  public TextMeshProUGUI TimeLabel;
  public TextMeshProUGUI PhaseLabel;
  public TextMeshProUGUI ScheduledLabel;
  public Image PhaseProgress;

  public void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.Init);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhase);
    TimeManager.OnScheduleChanged += new System.Action(this.OnScheduleChanged);
    if (!SaveAndLoad.Loaded)
      return;
    this.Init();
  }

  public void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.Init);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhase);
  }

  public void Init()
  {
    this.OnNewDay();
    this.OnNewPhase();
  }

  public void Update() => this.PhaseProgress.fillAmount = TimeManager.CurrentPhaseProgress;

  public void OnNewDay()
  {
    this.PhaseLabel.text = $"{TimeManager.CurrentPhase} " + $"Day {TimeManager.CurrentDay}";
  }

  public void OnNewPhase()
  {
    this.PhaseLabel.text = $"{TimeManager.CurrentPhase} " + $"Day {TimeManager.CurrentDay}";
    this.SetScheduleLabel();
  }

  public void OnScheduleChanged() => this.SetScheduleLabel();

  public void SetScheduleLabel()
  {
    if (DataManager.Instance.Followers.Count > 0)
    {
      ScheduledActivity scheduledActivity1 = TimeManager.GetScheduledActivity(TimeManager.CurrentPhase);
      ScheduledActivity scheduledActivity2 = TimeManager.GetScheduledActivity(FollowerLocation.Base);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append((object) scheduledActivity1);
      if (scheduledActivity1 != scheduledActivity2)
        stringBuilder.Append($" ({scheduledActivity2})");
      this.ScheduledLabel.text = scheduledActivity2.ToString();
    }
    else
      this.ScheduledLabel.text = "";
  }
}
