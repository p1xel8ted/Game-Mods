// Decompiled with JetBrains decompiler
// Type: Scheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (WorshipperInfoManager))]
public class Scheduler : BaseMonoBehaviour
{
  public Scheduler.Activities currentActivity;
  public WorshipperInfoManager wim;

  public void Start() => this.wim = this.GetComponent<WorshipperInfoManager>();

  public Scheduler.Activities CurrentActivity
  {
    get
    {
      if ((Object) this.wim == (Object) null || this.wim.v_i == null)
        this.currentActivity = Scheduler.Activities.Free;
      else
        this.GetCurrentSchedule();
      return this.currentActivity;
    }
    set => this.currentActivity = value;
  }

  public void GetCurrentSchedule()
  {
  }

  public enum Activities
  {
    None,
    Free,
    TravelToWork,
    Working,
    TravelToDwelling,
    Sleeping,
    TravelToWorship,
    Worship,
  }
}
