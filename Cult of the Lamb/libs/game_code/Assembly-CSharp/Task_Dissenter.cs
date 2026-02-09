// Decompiled with JetBrains decompiler
// Type: Task_Dissenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Task_Dissenter : Task
{
  public Worshipper w;
  public float BubbleTimer;
  public float SpeechDuration;
  public List<Worshipper> ListeningWorshippers;
  public bool Spoken;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Type = Task_Type.DISSENTER;
    this.w = t.GetComponent<Worshipper>();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override void TaskUpdate()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if (!this.Spoken && (double) Random.Range(0.0f, 1f) < 0.0099999997764825821)
        {
          this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
          this.w.SetAnimation("Dissenters/dissenter", true);
          this.SpeechDuration = (float) (8 + Random.Range(0, 4));
          this.BubbleTimer = 0.3f;
          this.ListeningWorshippers = new List<Worshipper>();
          this.Spoken = true;
          break;
        }
        if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.Timer = Random.Range(4f, 6f);
        this.t.givePath(TownCentre.Instance.RandomPositionInTownCentre());
        this.Spoken = false;
        break;
      case StateMachine.State.CustomAction0:
        foreach (Worshipper worshipper in Worshipper.worshippers)
        {
          if ((double) Vector3.Distance(this.t.transform.position, worshipper.transform.position) < 4.0 && !this.ListeningWorshippers.Contains(worshipper) && worshipper.CurrentTask != null && worshipper.CurrentTask.Type == Task_Type.NONE && !worshipper.InConversation)
          {
            worshipper.CurrentTask = (Task) new Task_DessenterListener();
            worshipper.CurrentTask.StartTask((TaskDoer) worshipper, this.t.gameObject);
            this.ListeningWorshippers.Add(worshipper);
          }
        }
        if ((double) (this.BubbleTimer -= Time.deltaTime) < 0.0)
        {
          WorshipperBubble.SPEECH_TYPE Type = (WorshipperBubble.SPEECH_TYPE) (6 + Random.Range(0, 3));
          this.w.bubble.Play(Type);
          this.BubbleTimer = (float) (4 + Random.Range(0, 2));
          foreach (Worshipper listeningWorshipper in this.ListeningWorshippers)
            listeningWorshipper.bubble.Play(Type);
        }
        if ((double) (this.SpeechDuration -= Time.deltaTime) >= 0.0)
          break;
        foreach (Worshipper listeningWorshipper in this.ListeningWorshippers)
        {
          listeningWorshipper.ClearTask();
          listeningWorshipper.CurrentTask = (Task) null;
        }
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
    }
  }
}
