// Decompiled with JetBrains decompiler
// Type: FollowerTask_ChangeLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_ChangeLocation : FollowerTask
{
  public FollowerLocation _destLocation;
  public FollowerTaskType _parentType;
  public Follower _follower;

  public override FollowerTaskType Type => FollowerTaskType.ChangeLocation;

  public override FollowerLocation Location => this._brain.Location;

  public override bool ShouldSaveDestination => false;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSermon => false;

  public override bool BlockSocial => true;

  public FollowerTaskType ParentType => this._parentType;

  public FollowerLocation TargetLocation => this._destLocation;

  public FollowerTask_ChangeLocation(
    FollowerLocation destLocation,
    FollowerTaskType parentType,
    bool AnimateOutFromLocation)
  {
    this._destLocation = destLocation;
    this._parentType = parentType;
    this.AnimateOutFromLocation = AnimateOutFromLocation;
  }

  public override int GetSubTaskCode() => (int) this._destLocation;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive()
  {
    Follower follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null && this.AnimateOutFromLocation && this._brain.Location == PlayerFarming.Location && this.TargetLocation != FollowerLocation.Church && this.TargetLocation != FollowerLocation.Demon && this.TargetLocation != FollowerLocation.Base && this.Brain.Location == FollowerLocation.Base)
      follower.TimedAnimation("Conversations/greet-nice", 1.93333328f, (System.Action) (() =>
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
          follower.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
          {
            if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
              follower.TimedAnimation("spawn-out", 0.8333333f, (System.Action) (() => this.ChangedLocation()));
            else
              this.ChangedLocation();
          })));
        else
          this.ChangedLocation();
      }));
    else
      this.ChangedLocation();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    followerById.SetOutfit(FollowerOutfitType.Follower, false);
    followerById.Interaction_FollowerInteraction.Interactable = true;
  }

  public void ChangedLocation()
  {
    if ((double) this.Brain._directInfoAccess.Social <= 0.0)
      this.Brain._directInfoAccess.Social += 50f;
    this._brain.DesiredLocation = this._destLocation;
    this.SetState(FollowerTaskState.Done);
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return LocationManager.LocationManagers[this.Location].GetExitPosition(this._destLocation);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this._follower = follower;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    this._follower = (Follower) null;
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  public void OnChangeRoom()
  {
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
      return;
    if ((UnityEngine.Object) this._follower != (UnityEngine.Object) null)
      this._follower.ClearPath();
    this.Arrive();
  }

  public override string ToDebugString()
  {
    return $"{base.ToDebugString()} ({this._destLocation}, {this._parentType})";
  }
}
