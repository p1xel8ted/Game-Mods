// Decompiled with JetBrains decompiler
// Type: FollowerTask_MissionaryTerrified
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_MissionaryTerrified : FollowerTask
{
  public Follower _follower;
  public LayerMask _islandLayerMask;
  public LayerMask _obstacleslayerMask;
  public float minSecondsOfDreadAniamtions = 15f;
  public float maxSecondsOfDreadAniamtions = 25f;

  public override FollowerTaskType Type => FollowerTaskType.ExistentialDread;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => false;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this._follower = follower;
    this._islandLayerMask = (LayerMask) ((int) this._islandLayerMask | 1 << LayerMask.NameToLayer("Island"));
    this._obstacleslayerMask = (LayerMask) ((int) this._obstacleslayerMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Existential Dread/dread-idle");
  }

  public override void OnStart()
  {
    this.SetState(FollowerTaskState.GoingTo);
    this.RecalculateDestination();
    if ((bool) (UnityEngine.Object) this._follower)
      this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Existential Dread/dread-walk");
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override int GetSubTaskCode() => 0;

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.0f, -1f));
    float distance = UnityEngine.Random.Range(1f, 5f);
    Vector3 vector3 = !((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, direction, distance, (int) this._islandLayerMask).collider == (UnityEngine.Object) null) ? follower.transform.position + (Vector3) direction * -1f * distance : follower.transform.position + (Vector3) direction * distance;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) vector3, 0.5f, Vector2.zero, 0.0f, (int) this._obstacleslayerMask);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      vector3 = (Vector3) raycastHit2D.collider.ClosestPoint((Vector2) vector3);
    return vector3;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    if ((double) UnityEngine.Random.value < 0.20000000298023224)
      this.AddRandomDreadThought();
    follower.SpeedMultiplier = 1f;
  }

  public override void OnArrive()
  {
    base.OnArrive();
    if (!(bool) (UnityEngine.Object) this._follower)
      return;
    System.Action onComplete = (System.Action) (() => this.RecalculateDestination());
    if ((double) UnityEngine.Random.value < 0.30000001192092896)
      this._follower.TimedAnimation("Existential Dread/dread-trauma", UnityEngine.Random.Range(this.minSecondsOfDreadAniamtions, this.maxSecondsOfDreadAniamtions), onComplete);
    else if ((double) UnityEngine.Random.value < 0.5)
      this._follower.TimedAnimation("Existential Dread/dread-idle-flashbacks", UnityEngine.Random.Range(this.minSecondsOfDreadAniamtions, this.maxSecondsOfDreadAniamtions), onComplete);
    else
      this._follower.TimedAnimation("Existential Dread/dread-idle", UnityEngine.Random.Range(this.minSecondsOfDreadAniamtions, this.maxSecondsOfDreadAniamtions), onComplete);
  }

  public void AddRandomDreadThought()
  {
    ThoughtData data = FollowerThoughts.GetData((Thought) UnityEngine.Random.Range(410, 415));
    data.Init();
    this.Brain._directInfoAccess.Thoughts.Add(data);
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  [CompilerGenerated]
  public void \u003COnArrive\u003Eb__20_0() => this.RecalculateDestination();
}
