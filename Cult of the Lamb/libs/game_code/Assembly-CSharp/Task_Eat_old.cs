// Decompiled with JetBrains decompiler
// Type: Task_Eat_old
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Eat_old : Task
{
  public Worshipper worshipper;
  public SimpleInventory Inventory;
  public Structure structure;
  public Restaurant restaurant;
  public WorshipperBubble bubble;
  public bool MoveForTantrum;

  public Task_Eat_old() => this.Type = Task_Type.EAT;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.worshipper = t.GetComponent<Worshipper>();
    this.Inventory = t.GetComponent<SimpleInventory>();
    this.structure = TargetObject.transform.parent.gameObject.GetComponent<Structure>();
    this.restaurant = TargetObject.transform.parent.gameObject.GetComponent<Restaurant>();
    this.bubble = t.GetComponentInChildren<WorshipperBubble>();
    this.ClearOnComplete = true;
  }

  public override void TaskUpdate()
  {
    if (this.Inventory.GetItemType() != InventoryItem.ITEM_TYPE.MEAT)
    {
      if (this.Inventory.GetItemType() != InventoryItem.ITEM_TYPE.NONE)
        this.Inventory.DropItem();
      this.GetFoodFromStation();
    }
    else
      this.SitAndEatFood();
    base.TaskUpdate();
  }

  public void GetFoodFromStation()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Timer = 0.0f;
        this.PathToPosition(this.structure.transform.position);
        TaskDoer t = this.t;
        t.EndOfPath = t.EndOfPath + new System.Action(this.CollectFood);
        break;
      case StateMachine.State.Moving:
        if (!this.MoveForTantrum)
        {
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.structure.transform.position);
          break;
        }
        if ((double) (this.Timer -= Time.deltaTime) > 0.0)
          break;
        this.bubble.Play(WorshipperBubble.SPEECH_TYPE.FOOD);
        this.t.UsePathing = true;
        this.worshipper.EATEN_DINNNER = true;
        this.ClearTask();
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
      case StateMachine.State.CustomAction0:
        if ((double) (this.Timer += Time.deltaTime) <= 0.5)
          break;
        if (this.structure.HasInventoryType(InventoryItem.ITEM_TYPE.MEAT))
        {
          this.structure.RemoveInventoryByType(InventoryItem.ITEM_TYPE.MEAT);
          this.Inventory.GiveItem(InventoryItem.ITEM_TYPE.MEAT);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        this.Timer = UnityEngine.Random.Range(0.8f, 1.3f);
        this.t.UsePathing = false;
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        this.t.state.facingAngle = (float) UnityEngine.Random.Range(180, 360);
        this.t.speed = this.t.maxSpeed;
        this.MoveForTantrum = true;
        this.restaurant.RemoveFromPositions(this.t.gameObject);
        break;
    }
  }

  public void CollectFood()
  {
    this.Timer = 0.0f;
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.CollectFood);
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  public void SitAndEatFood()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Timer = 0.0f;
        this.PathToPosition(this.TargetObject.transform.position);
        TaskDoer t = this.t;
        t.EndOfPath = t.EndOfPath + new System.Action(this.CollectFood);
        break;
      case StateMachine.State.Moving:
        if ((double) (this.Timer += Time.deltaTime) <= 1.0)
          break;
        this.Timer = 0.0f;
        this.PathToPosition(this.TargetObject.gameObject.transform.position);
        break;
      case StateMachine.State.CustomAction0:
        if ((double) (this.Timer += Time.deltaTime) <= 5.0)
          break;
        this.Inventory.RemoveItem();
        this.worshipper.EATEN_DINNNER = true;
        this.restaurant.RemoveFromPositions(this.t.gameObject);
        this.ClearTask();
        break;
    }
  }

  public void EndOfPath()
  {
    this.Timer = 0.0f;
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.EndOfPath);
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  public override void ClearTask()
  {
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    TaskDoer t1 = this.t;
    t1.EndOfPath = t1.EndOfPath - new System.Action(this.EndOfPath);
    TaskDoer t2 = this.t;
    t2.EndOfPath = t2.EndOfPath - new System.Action(this.CollectFood);
    base.ClearTask();
  }
}
