// Decompiled with JetBrains decompiler
// Type: FollowerTask_Freezing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Freezing : FollowerTask
{
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 20f;
  public float _gameTimeToNextStateUpdate;
  public StructureBrain targetStructure;

  public override FollowerTaskType Type => FollowerTaskType.Freezing;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this._state != FollowerTaskState.Idle && this._state != FollowerTaskState.Doing)
      return;
    this._gameTimeToNextStateUpdate -= deltaGameTime;
    if ((double) this._gameTimeToNextStateUpdate > 0.0)
      return;
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
    this._gameTimeToNextStateUpdate = Random.Range(10f, 20f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Freezing/idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Freezing/walk");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.DECORATION_TORCH_BIG, StructureBrain.TYPES.DECORATION_SPIDER_TORCH);
    structuresOfTypes.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_Furnace>());
    this.targetStructure = (StructureBrain) null;
    if (structuresOfTypes.Count > 0)
    {
      structuresOfTypes.Shuffle<StructureBrain>();
      for (int index = 0; index < structuresOfTypes.Count; ++index)
      {
        if (structuresOfTypes[index].Data.Fuel > 0)
        {
          this.targetStructure = structuresOfTypes[index];
          return this.targetStructure.Data.Position;
        }
      }
    }
    return TownCentre.RandomCircleFromTownCentre(16f);
  }

  public override float RestChange(float deltaGameTime) => 100f;
}
