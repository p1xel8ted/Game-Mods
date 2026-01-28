// Decompiled with JetBrains decompiler
// Type: Interaction_BackToWork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Runtime.CompilerServices;

#nullable disable
public class Interaction_BackToWork : Interaction
{
  public Follower follower;
  public FollowerTask currentTask;
  public bool weatherEventTask;
  public new string label;

  public void Init(Follower follower, bool weatherEventTask = false)
  {
    this.follower = follower;
    this.weatherEventTask = weatherEventTask;
    follower.Interaction_FollowerInteraction.Interactable = false;
    this.currentTask = follower.Brain.CurrentTask;
    this.PriorityWeight = 2f;
    if (!((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null))
      return;
    this.OutlineTarget = follower.Interaction_FollowerInteraction.OutlineTarget;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.follower.Interaction_FollowerInteraction.Interactable = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.label = ScriptLocalization.FollowerInteractions.BackToWork;
  }

  public override void Update()
  {
    base.Update();
    if (this.follower.Brain.CurrentTask != this.currentTask || this.follower.Interaction_FollowerInteraction.LightningIncoming)
    {
      this.follower.Interaction_FollowerInteraction.Interactable = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
    else
      this.follower.Interaction_FollowerInteraction.Interactable = false;
  }

  public override void GetLabel()
  {
    if (this.label == null)
      this.UpdateLocalisation();
    this.Label = this.label;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    CultFaithManager.AddThought(Thought.Cult_InterruptedFollower, this.follower.Brain.Info.ID);
    if (this.weatherEventTask)
      this.follower.Brain._directInfoAccess.WeatherEventIDBackToWork = SeasonsManager.WeatherEventID;
    if (!(this.follower.Brain.CurrentTask is FollowerTask_ManualControl))
    {
      this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      this.follower.TimedAnimation("Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString(), 2f, (System.Action) (() =>
      {
        this.follower.Brain.CurrentTask?.Abort();
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      }));
      this.follower.AddBodyAnimation("idle", true, 0.0f);
    }
    this.Interactable = false;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__9_0()
  {
    this.follower.Brain.CurrentTask?.Abort();
    UnityEngine.Object.Destroy((UnityEngine.Object) this);
  }
}
