// Decompiled with JetBrains decompiler
// Type: WorkPlace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorkPlace : BaseMonoBehaviour
{
  public static List<WorkPlace> workplaces = new List<WorkPlace>();
  public string ID;
  public List<WorkPlaceSlot> Positions = new List<WorkPlaceSlot>();
  private List<TaskDoer> Workers = new List<TaskDoer>();
  public List<Worshipper> Worshippers = new List<Worshipper>();
  private bool init;
  public static string NO_JOB = "-1";
  public StructureBrain.TYPES Type;
  public Task_Type JobType;
  public Structure structure;
  public WorkPlace.JobDelegate OnJobBegin;
  public WorkPlace.JobDelegate OnArrivedAtJob;
  public WorkPlace.JobEnded OnJobEnded;
  private float PowerTimer;

  private void Start()
  {
    this.structure = this.GetComponent<Structure>();
    for (int index = 0; index < this.Positions.Count; ++index)
      this.Workers.Add((TaskDoer) null);
  }

  public void BeginJob(TaskDoer Worker, int Position)
  {
    this.Workers[Position] = Worker;
    this.Worshippers.Add(Worker as Worshipper);
    WorkPlace.JobDelegate onJobBegin = this.OnJobBegin;
    if (onJobBegin == null)
      return;
    onJobBegin();
  }

  public void ArrivedAtJob()
  {
    WorkPlace.JobDelegate onArrivedAtJob = this.OnArrivedAtJob;
    if (onArrivedAtJob == null)
      return;
    onArrivedAtJob();
  }

  public void ClearAllWorkers()
  {
    int index1 = -1;
    while (++index1 < this.Worshippers.Count)
      Worshipper.ClearJob(this.Worshippers[index1]);
    this.Worshippers.Clear();
    int index2 = -1;
    while (++index2 < this.Workers.Count)
      this.Workers[index2] = (TaskDoer) null;
  }

  public void EndJob(TaskDoer Worker, int Position)
  {
    this.Workers[Position] = (TaskDoer) null;
    this.Worshippers.Remove(Worker as Worshipper);
    int WorkerCount = 0;
    foreach (Object worker in this.Workers)
    {
      if (worker != (Object) null)
        ++WorkerCount;
    }
    if (this.OnJobEnded == null)
      return;
    this.OnJobEnded(WorkerCount);
  }

  public bool HasPower() => true;

  private void Update()
  {
  }

  private void OnEnable() => WorkPlace.workplaces.Add(this);

  private void OnDisable() => WorkPlace.workplaces.Remove(this);

  public void SetID(string ID) => this.ID = ID;

  public static WorkPlace GetWorkPlaceByID(string ID)
  {
    foreach (WorkPlace workplace in WorkPlace.workplaces)
    {
      if (workplace.ID == ID)
        return workplace;
    }
    return (WorkPlace) null;
  }

  public delegate void JobDelegate();

  public delegate void JobEnded(int WorkerCount);
}
