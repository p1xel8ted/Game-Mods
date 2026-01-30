// Decompiled with JetBrains decompiler
// Type: Task_EatMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_EatMeal : Task
{
  public Meal Meal;
  public Worshipper Worshipper;
  public bool Arrived;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Type = Task_Type.EAT;
    this.Worshipper = t.GetComponent<Worshipper>();
    this.Worshipper.GoToAndStop(this.Meal.gameObject, new System.Action(this.EatMeal), this.Meal.gameObject, false);
  }

  public override void TaskUpdate()
  {
    if (this.Arrived || !this.Meal.TakenByPlayer && !((UnityEngine.Object) this.Meal == (UnityEngine.Object) null))
      return;
    this.ClearTask();
  }

  public void EatMeal() => this.t.StartCoroutine((IEnumerator) this.EatMealRoutine());

  public IEnumerator EatMealRoutine()
  {
    Task_EatMeal taskEatMeal = this;
    taskEatMeal.Arrived = true;
    if ((UnityEngine.Object) taskEatMeal.Meal == (UnityEngine.Object) null)
    {
      taskEatMeal.ClearTask();
    }
    else
    {
      bool MealIsRotten = taskEatMeal.Meal.StructureInfo.Rotten;
      taskEatMeal.Worshipper.SetAnimation("Food/food_eat", true);
      yield return (object) new WaitForSeconds(5f);
      taskEatMeal.Worshipper.SetAnimation("Food/food-finish", true);
      yield return (object) new WaitForSeconds(1.8f);
      taskEatMeal.Worshipper.Hunger = 100f;
      taskEatMeal.Worshipper.wim.v_i.Starve = 0.0f;
      taskEatMeal.Worshipper.wim.v_i.Complaint_Food = false;
      taskEatMeal.Worshipper.BeenFed = true;
      if (MealIsRotten)
        taskEatMeal.Worshipper.wim.v_i.Illness = Villager_Info.IllnessThreshold;
      taskEatMeal.ClearTask();
    }
  }

  public override void ClearTask()
  {
    this.Worshipper.GoToAndStopping = false;
    base.ClearTask();
    this.t.ClearPaths();
    this.t.StopAllCoroutines();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }
}
