// Decompiled with JetBrains decompiler
// Type: FollowerTask_EnforcingManualControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class FollowerTask_EnforcingManualControl : FollowerTask_ManualControl
{
  public FollowerBrain enforcer;
  public FollowerTaskType enorcingTask;

  public override FollowerTaskType Type => FollowerTaskType.EnforcerManualControl;

  public FollowerTask_EnforcingManualControl(FollowerBrain enforcer, FollowerTaskType enorcingTask)
  {
    this.enforcer = enforcer;
    this.enorcingTask = enorcingTask;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    FollowerTaskType? type1 = this.enforcer.CurrentTask?.Type;
    FollowerTaskType enorcingTask1 = this.enorcingTask;
    if (!(type1.GetValueOrDefault() == enorcingTask1 & type1.HasValue))
    {
      FollowerTaskType? type2 = this.enforcer.CurrentTask?.Type;
      FollowerTaskType enorcingTask2 = this.enorcingTask;
      if (!(type2.GetValueOrDefault() == enorcingTask2 & type2.HasValue))
        return;
    }
    this.enforcer.CurrentTask.Abort();
  }
}
