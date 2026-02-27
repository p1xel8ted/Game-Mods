// Decompiled with JetBrains decompiler
// Type: Interaction_BackToWork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class Interaction_BackToWork : Interaction
{
  private Follower follower;
  private FollowerTask currentTask;
  private string label;

  public void Init(Follower follower)
  {
    this.follower = follower;
    follower.Interaction_FollowerInteraction.Interactable = false;
    this.currentTask = follower.Brain.CurrentTask;
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

  protected override void Update()
  {
    base.Update();
    if (this.follower.Brain.CurrentTask != this.currentTask)
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
}
