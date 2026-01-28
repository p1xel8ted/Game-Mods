// Decompiled with JetBrains decompiler
// Type: FollowerTask_ChopTrees
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_ChopTrees : FollowerTask_AssistPlayerBase
{
  public const float REMOVAL_DURATION_GAME_MINUTES = 4f;
  public Structures_Tree _tree;
  public int _treeID;
  public FollowerLocation _location;
  public float _removalProgress;
  public float _gameTimeSinceLastProgress;
  public float WaitTimer;
  public List<Structures_Tree> cachedTrees = new List<Structures_Tree>();

  public override FollowerTaskType Type => FollowerTaskType.ChopTrees;

  public override FollowerLocation Location => this._location;

  public override float Priorty => this._tree != null ? (this.BigTreeVariant ? 0.0f : 3f) : 3f;

  public bool BigTreeVariant => this._tree != null && this._tree.Data.VariantIndex == 2;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    if (FollowerRole != FollowerRole.Lumberjack)
      return PriorityCategory.Low;
    return this._tree == null || this._tree.Data == null || !this.BigTreeVariant ? PriorityCategory.WorkPriority : PriorityCategory.Medium;
  }

  public FollowerTask_ChopTrees(int TreeID)
  {
    this._helpingPlayer = false;
    this._treeID = TreeID;
    this._tree = StructureManager.GetStructureByID<Structures_Tree>(TreeID);
    this._location = this._tree.Data.Location;
  }

  public FollowerTask_ChopTrees() => this._helpingPlayer = true;

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    this._tree = StructureManager.GetStructureByID<Structures_Tree>(this._treeID);
    if (this._tree == null)
      return;
    this._tree.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    this._tree = StructureManager.GetStructureByID<Structures_Tree>(this._treeID);
    if (this._tree == null)
      return;
    this._tree.ReservedForTask = false;
  }

  public override void OnStart()
  {
    this.ReleaseReservations();
    this.Loop(true);
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void AssistPlayerTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Wait)
    {
      if (PlayerFarming.Location == this._brain.Location)
      {
        this.Loop();
      }
      else
      {
        if ((double) (this.WaitTimer += deltaGameTime) <= 60.0)
          return;
        this.Loop();
      }
    }
    else
    {
      if (LocationManager.GetLocationState(this._location) == LocationState.Active)
      {
        TreeBase tree = this.FindTree();
        if ((Object) tree == (Object) null || tree.StructureBrain.TreeChopped)
        {
          this._tree = (Structures_Tree) null;
          this._treeID = -1;
          this.SetState(FollowerTaskState.Idle);
          this.Loop();
        }
      }
      else if (this._tree == null)
      {
        this.SetState(FollowerTaskState.Idle);
        this.Loop();
      }
      if (this.State != FollowerTaskState.Doing)
        return;
      this._gameTimeSinceLastProgress += deltaGameTime;
      if (this._brain.Location == PlayerFarming.Location || (double) this._gameTimeSinceLastProgress <= (double) this.ConvertAnimTimeToGameTime(4.06f) / 5.0)
        return;
      this.ProgressTask();
    }
  }

  public override void TaskTick(float deltaGameTime)
  {
    base.TaskTick(deltaGameTime);
    if (PlayerFarming.Location != FollowerLocation.Base || this.State != FollowerTaskState.Doing || (double) Vector3.Distance(this.Brain.LastPosition, this._tree.Data.Position) <= 2.0)
      return;
    this.SetState(FollowerTaskState.GoingTo);
  }

  public float ConvertAnimTimeToGameTime(float duration) => duration * 2f;

  public override void ProgressTask()
  {
    this._tree = StructureManager.GetStructureByID<Structures_Tree>(this._treeID);
    if (this._tree == null || this._tree.TreeChopped)
    {
      Debug.Log((object) "Tree is null so END");
      this.End();
    }
    else
    {
      float num = 0.075f;
      this._gameTimeSinceLastProgress = 0.0f;
      this._tree.TreeHit(num * this._brain.Info.ProductivityMultiplier, followerID: this._brain.Info.ID);
      this._brain.GetXP(0.1f);
      if (!this._tree.TreeChopped)
        return;
      if (this._brain.Location != PlayerFarming.Location)
      {
        List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(this._brain.Location);
        if (structuresOfType.Count > 0)
          structuresOfType[0].AddItem(InventoryItem.ITEM_TYPE.LOG, Mathf.RoundToInt((float) this._tree.Data.LootCountToDrop * this._brain.ResourceHarvestingMultiplier));
      }
      if (this._brain.Location != PlayerFarming.Location)
      {
        this.WaitTimer = 0.0f;
        this.SetState(FollowerTaskState.Wait);
      }
      else
        this.Loop();
    }
  }

  public void Loop(bool force = false)
  {
    if (!force && this._helpingPlayer && this.EndIfPlayerIsDistant())
      return;
    Structures_Tree nextTree = this.GetNextTree();
    if (nextTree == null)
    {
      this.End();
    }
    else
    {
      this.ReleaseReservations();
      this.ClearDestination();
      this._treeID = nextTree.Data.ID;
      this._tree = nextTree;
      this._tree.ReservedForTask = true;
      this.SetState(FollowerTaskState.GoingTo);
    }
  }

  public Structures_Tree GetNextTree()
  {
    Structures_Tree nextTree = (Structures_Tree) null;
    float num1 = float.MaxValue;
    float num2 = this._helpingPlayer ? this.AssistRange : float.MaxValue;
    PlayerFarming instance = PlayerFarming.Instance;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    StructureManager.TryGetAllAvailableTrees(ref this.cachedTrees, this.Location);
    foreach (Structures_Tree cachedTree in this.cachedTrees)
    {
      if ((Object) followerById == (Object) null)
      {
        nextTree = cachedTree;
        break;
      }
      float num3 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, cachedTree.Data.Position);
      if (!this.BigTreeVariant && (double) num3 < (double) num2 && !cachedTree.Data.IsSapling)
      {
        float num4 = num3 + (cachedTree.Data.Prioritised ? 0.0f : 1000f);
        if ((double) num4 < (double) num1)
        {
          nextTree = cachedTree;
          num1 = num4;
        }
      }
    }
    if (nextTree == null)
    {
      foreach (Structures_Tree cachedTree in this.cachedTrees)
      {
        if ((Object) followerById == (Object) null)
        {
          nextTree = cachedTree;
          break;
        }
        float num5 = Vector3.Distance(this._helpingPlayer ? instance.transform.position : followerById.transform.position, cachedTree.Data.Position);
        if (this.BigTreeVariant && (double) num5 < (double) num2 && !cachedTree.Data.IsSapling)
        {
          float num6 = num5 + (cachedTree.Data.Prioritised ? 0.0f : 1000f);
          if ((double) num6 < (double) num1)
          {
            nextTree = cachedTree;
            num1 = num6;
          }
        }
      }
    }
    this.cachedTrees.Clear();
    return nextTree;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this._tree != null ? this._tree.Data.Position + ((double) Random.value < 0.5 ? new Vector3(1f, 0.0f, -0.5f) : new Vector3(-1f, 0.0f, -0.5f)) : follower.transform.position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this._treeID == 0)
    {
      this.ProgressTask();
    }
    else
    {
      if ((Object) this.FindTree() != (Object) null)
        follower.FacePosition(this._tree.Data.Position);
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) follower.SetBodyAnimation("chop-wood", true);
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Chop"))
      return;
    this.ProgressTask();
  }

  public TreeBase FindTree()
  {
    TreeBase tree1 = (TreeBase) null;
    foreach (TreeBase tree2 in TreeBase.Trees)
    {
      if (tree2.StructureInfo.ID == this._treeID)
      {
        tree1 = tree2;
        break;
      }
    }
    return tree1;
  }
}
