// Decompiled with JetBrains decompiler
// Type: FollowerTask_Mating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Mating : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.Mating;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.HideAllFollowerIcons();
    follower.Spine.gameObject.SetActive(false);
    follower.Interaction_FollowerInteraction.Interactable = false;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.ShowAllFollowerIcons();
    follower.Spine.gameObject.SetActive(true);
    follower.Interaction_FollowerInteraction.Interactable = true;
  }

  public override void SimCleanup(SimFollower simFollower) => base.SimCleanup(simFollower);

  public override Vector3 UpdateDestination(Follower follower) => Vector3.zero;
}
