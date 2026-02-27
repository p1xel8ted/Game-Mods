// Decompiled with JetBrains decompiler
// Type: HUD_GameTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.Init);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhase);
    TimeManager.OnScheduleChanged += new System.Action(this.OnScheduleChanged);
    if (!SaveAndLoad.Loaded)
      return;
    this.Init();
  }

  private void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.Init);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhase);
  }

  private void Init()
  {
    this.OnNewDay();
    this.OnNewPhase();
  }

  private void Update() => this.PhaseProgress.fillAmount = TimeManager.CurrentPhaseProgress;

  private void OnNewDay()
  {
    this.PhaseLabel.text = $"{TimeManager.CurrentPhase} " + $"Day {TimeManager.CurrentDay}";
  }

  private void OnNewPhase()
  {
    this.PhaseLabel.text = $"{TimeManager.CurrentPhase} " + $"Day {TimeManager.CurrentDay}";
    this.SetScheduleLabel();
  }

  private void OnScheduleChanged() => this.SetScheduleLabel();

  private void SetScheduleLabel()
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
