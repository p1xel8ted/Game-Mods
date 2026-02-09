// Decompiled with JetBrains decompiler
// Type: Demon_Collector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Demon_Collector : Demon
{
  public float AttackProgress;
  public float AttackAngle;
  public float AttackDelay;
  public float AttackDurationCount = 60f;
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
  public const float collectMin = 20f;
  public const float collectMax = 30f;
  public Vector3 collectPosition;
  public InventoryItem.ITEM_TYPE itemToDrop;
  [CompilerGenerated]
  public bool \u003CCanCollect\u003Ek__BackingField = true;
  public bool droppedClothing;
  public float DetectEnemyRange = 5f;
  public Health CurrentTarget;
  public Vector3 pointToCheck;
  public Vector3 Seperator;
  public float SeperationRadius = 0.5f;

  public bool CanCollect
  {
    get => this.\u003CCanCollect\u003Ek__BackingField;
    set => this.\u003CCanCollect\u003Ek__BackingField = value;
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
    this.StartCoroutine((IEnumerator) this.SetSkin());
  }

  public override IEnumerator SetSkin()
  {
    Demon_Collector demonCollector = this;
    while (demonCollector.spine.AnimationState == null)
      yield return (object) null;
    if (demonCollector.Level > 1)
    {
      demonCollector.spine.skeleton.SetSkin("Heart+");
      demonCollector.spine.skeleton.SetSlotsToSetupPose();
      demonCollector.spine.AnimationState.Apply(demonCollector.spine.skeleton);
    }
  }

  public override void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.SignPostAttack)
      this.transform.position = this.GetRandomOnCircle(PlayerFarming.Instance.transform.position);
    else
      this.transform.position = this.Master.transform.position + Vector3.right;
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null || (double) GameManager.DeltaTime == 0.0 || MMConversation.isPlaying || LetterBox.IsPlaying)
      return;
    if ((this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving) && (double) (this.AttackDelay += Time.deltaTime) > (double) this.AttackDurationCount && this.CanCollect)
    {
      this.AttackDurationCount = (float) UnityEngine.Random.Range(60, 120);
      this.AttackDelay = 0.0f;
      this.collectPosition = this.GetRandomOnCircle(this.transform.position);
      this.collectTimer = UnityEngine.Random.Range(20f, 30f);
      this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    }
    if (!this.CanCollect && (this.state.CURRENT_STATE == StateMachine.State.SignPostAttack || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack))
    {
      this.transform.position = this.Master.transform.position + Vector3.right;
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
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
      case StateMachine.State.SignPostAttack:
        this.Speed += (float) ((6.0 - (double) this.Speed) / (15.0 * (double) GameManager.DeltaTime));
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.collectPosition);
        if (Health.team2.Count > 0 && (double) (this.state.Timer += Time.deltaTime) > (double) this.collectTimer)
        {
          this.transform.position = this.GetRandomOnCircle(PlayerFarming.Instance.transform.position);
          this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
          float num = 0.5f + 0.05f * (float) this.Level;
          this.itemToDrop = this.Level <= 1 ? ((double) UnityEngine.Random.value > 0.25 ? InventoryItem.ITEM_TYPE.HALF_HEART : InventoryItem.ITEM_TYPE.RED_HEART) : ((double) UnityEngine.Random.value >= (double) num ? ((double) UnityEngine.Random.value > 0.25 ? InventoryItem.ITEM_TYPE.HALF_HEART : InventoryItem.ITEM_TYPE.RED_HEART) : ((double) UnityEngine.Random.value > 0.25 ? InventoryItem.ITEM_TYPE.HALF_BLUE_HEART : InventoryItem.ITEM_TYPE.BLUE_HEART));
        }
        if ((double) this.state.Timer > 10.0)
        {
          this.Container.SetActive(false);
          break;
        }
        break;
      case StateMachine.State.RecoverFromAttack:
        this.Container.SetActive(true);
        this.TargetAngle = Utils.GetAngle(this.transform.position, this.Master.transform.position);
        this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
        this.Speed += (float) ((6.0 - (double) this.Speed) / (15.0 * (double) GameManager.DeltaTime));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) < 1.5 && (UnityEngine.Object) PlayerFarming.Instance.gameObject != (UnityEngine.Object) null && !PlayerFarming.Instance.GoToAndStopping && PlayerFarming.Location != FollowerLocation.Boss_5)
        {
          if (DataManager.Instance.TailorEnabled && !DungeonSandboxManager.Active && !DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_2) && !this.droppedClothing)
          {
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_2;
            this.droppedClothing = true;
          }
          else
            InventoryItem.Spawn(this.itemToDrop, 1, this.transform.position);
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

  public Vector3 GetRandomOnCircle(Vector3 center)
  {
    double num = 50.0;
    float f = UnityEngine.Random.Range(0.0f, 6.28318548f);
    float x = (float) num * Mathf.Cos(f);
    float y = (float) num * Mathf.Sin(f);
    return center + new Vector3(x, y, 0.0f);
  }
}
