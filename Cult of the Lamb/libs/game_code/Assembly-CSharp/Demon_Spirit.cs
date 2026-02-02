// Decompiled with JetBrains decompiler
// Type: Demon_Spirit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Demon_Spirit : Demon
{
  public SimpleSpineAnimator simpleSpineAnimator;
  public StateMachine state;
  public float TargetAngle;
  public Vector3 MoveVector;
  public float Speed;
  public float vx;
  public float vy;
  public float Bobbing;
  public float SpineVZ;
  public float SpineVY;
  public SkeletonAnimation spine;
  public LayerMask layerToCheck;
  public GameObject Container;
  public float collectTimer;
  public const float collectMin = 30f;
  public const float collectMax = 30f;
  public Vector3 collectPosition;
  public InventoryItem.ITEM_TYPE itemToDrop;
  public bool setSkin = true;
  [CompilerGenerated]
  public bool \u003CForceMove\u003Ek__BackingField;
  public float DetectEnemyRange = 5f;
  public Health CurrentTarget;
  public Vector3 pointToCheck;
  public Vector3 Seperator;
  public float SeperationRadius = 0.5f;

  public bool ForceMove
  {
    get => this.\u003CForceMove\u003Ek__BackingField;
    set => this.\u003CForceMove\u003Ek__BackingField = value;
  }

  public void OnEnable() => Demon_Arrows.Demons.Add(this.gameObject);

  public void OnDisable() => Demon_Arrows.Demons.Remove(this.gameObject);

  public override void Start()
  {
    base.Start();
    this.state = this.GetComponent<StateMachine>();
    this.SpineVZ = -1.5f;
    this.SpineVY = 0.5f;
    this.spine.transform.localPosition = new Vector3(0.0f, this.SpineVY, this.SpineVZ + 0.1f * Mathf.Cos(this.Bobbing += 5f * Time.deltaTime));
    if (!this.setSkin)
      return;
    this.StartCoroutine((IEnumerator) this.SetSkin());
  }

  public new IEnumerator SetSkin()
  {
    Demon_Spirit demonSpirit = this;
    while (demonSpirit.spine.AnimationState == null)
      yield return (object) null;
    if (demonSpirit.Level > 1)
    {
      demonSpirit.spine.skeleton.SetSkin("Spirit+");
      demonSpirit.spine.skeleton.SetSlotsToSetupPose();
      demonSpirit.spine.AnimationState.Apply(demonSpirit.spine.skeleton);
    }
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null || !this.ForceMove && ((double) GameManager.DeltaTime == 0.0 || MMConversation.isPlaying || LetterBox.IsPlaying))
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Speed += (float) ((0.0 - (double) this.Speed) / (7.0 * (double) GameManager.DeltaTime));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) > 2.0)
        {
          this.TargetAngle = Utils.GetAngle(this.transform.position, this.Master.transform.position);
          this.state.facingAngle = this.TargetAngle;
          this.state.CURRENT_STATE = StateMachine.State.Moving;
          break;
        }
        break;
      case StateMachine.State.Moving:
        this.TargetAngle = Utils.GetAngle(this.transform.position, this.Master.transform.position);
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        this.Speed += (float) ((6.0 - (double) this.Speed) / (15.0 * (double) GameManager.DeltaTime));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) < 1.5)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        break;
      case StateMachine.State.SpawnIn:
        if ((double) (this.state.Timer += Time.deltaTime) > 0.60000002384185791)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        break;
    }
    this.vx = this.Speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    this.vy = this.Speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    this.MoveVector = this.transform.position + new Vector3(this.vx, this.vy);
    this.MoveVector.z = this.Master.transform.position.z;
    this.transform.position = this.MoveVector;
    this.spine.skeleton.ScaleX = (double) this.Master.transform.position.x > (double) this.transform.position.x ? -1f : 1f;
    this.spine.transform.eulerAngles = new Vector3(-60f, 0.0f, this.vx * -5f / Time.deltaTime);
    this.SpineVZ = Mathf.Lerp(this.SpineVZ, -1f, 5f * Time.deltaTime);
    this.SpineVY = Mathf.Lerp(this.SpineVY, 0.5f, 5f * Time.deltaTime);
    this.spine.transform.localPosition = new Vector3(0.0f, 0.0f, this.SpineVZ + 0.1f * Mathf.Cos(this.Bobbing += 5f * Time.deltaTime));
    this.SeperateDemons();
  }

  public void GetNewTarget()
  {
    this.CurrentTarget = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.MasterHealth.team && allUnit.team != Health.Team.Neutral && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.CheckLineOfSight(allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
      {
        float num2 = Vector3.Distance(this.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          this.CurrentTarget = allUnit;
          num1 = num2;
        }
      }
    }
  }

  public bool CheckLineOfSight(Vector3 pointToCheck, float distance)
  {
    this.pointToCheck = pointToCheck;
    return !((UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null);
  }

  public void SeperateDemons()
  {
    this.Seperator = Vector3.zero;
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((UnityEngine.Object) demon != (UnityEngine.Object) this.gameObject && (UnityEngine.Object) demon != (UnityEngine.Object) null && this.state.CURRENT_STATE != StateMachine.State.SignPostAttack && this.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack)
      {
        float num = Vector2.Distance((Vector2) demon.gameObject.transform.position, (Vector2) this.transform.position);
        float angle = Utils.GetAngle(demon.gameObject.transform.position, this.transform.position);
        if ((double) num < (double) this.SeperationRadius)
        {
          this.Seperator.x += (float) (((double) this.SeperationRadius - (double) num) / 2.0) * Mathf.Cos(angle * ((float) Math.PI / 180f)) * GameManager.DeltaTime;
          this.Seperator.y += (float) (((double) this.SeperationRadius - (double) num) / 2.0) * Mathf.Sin(angle * ((float) Math.PI / 180f)) * GameManager.DeltaTime;
        }
      }
    }
    this.transform.position = this.transform.position + this.Seperator;
  }
}
