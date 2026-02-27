// Decompiled with JetBrains decompiler
// Type: Demon_Collector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Demon_Collector : Demon
{
  private float AttackProgress;
  private float AttackAngle;
  private float AttackDelay;
  private float AttackDurationCount = 60f;
  public SimpleSpineAnimator simpleSpineAnimator;
  private GameObject _Master;
  private Health MasterHealth;
  private StateMachine MasterState;
  private StateMachine state;
  private float TargetAngle;
  private Vector3 MoveVector;
  private float Speed;
  private float vx;
  private float vy;
  private float Bobbing;
  private float SpineVZ;
  private float SpineVY;
  public SkeletonAnimation spine;
  public LayerMask layerToCheck;
  public GameObject Container;
  private float collectTimer;
  private const float collectMin = 20f;
  private const float collectMax = 30f;
  private Vector3 collectPosition;
  private InventoryItem.ITEM_TYPE itemToDrop;
  private float DetectEnemyRange = 5f;
  private Health CurrentTarget;
  private Vector3 pointToCheck;
  private Vector3 Seperator;
  public float SeperationRadius = 0.5f;

  private GameObject Master
  {
    get
    {
      if ((UnityEngine.Object) this._Master == (UnityEngine.Object) null)
      {
        this._Master = GameObject.FindGameObjectWithTag("Player");
        if ((UnityEngine.Object) this._Master != (UnityEngine.Object) null)
        {
          this.MasterState = this._Master.GetComponent<StateMachine>();
          this.MasterHealth = this._Master.GetComponent<Health>();
        }
      }
      return this._Master;
    }
    set => this._Master = value;
  }

  public bool CanCollect { get; set; } = true;

  private void OnEnable() => Demon_Arrows.Demons.Add(this.gameObject);

  private void OnDisable() => Demon_Arrows.Demons.Remove(this.gameObject);

  private void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.SpineVZ = -1.5f;
    this.SpineVY = 0.5f;
    this.spine.transform.localPosition = new Vector3(0.0f, this.SpineVY, this.SpineVZ + 0.1f * Mathf.Cos(this.Bobbing += 5f * Time.deltaTime));
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    HealthPlayer.OnPlayerDied += new HealthPlayer.HPUpdated(this.Health_OnDie);
    this.StartCoroutine((IEnumerator) this.SetSkin());
  }

  private IEnumerator SetSkin()
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

  private void Health_OnDie(HealthPlayer player) => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    HealthPlayer.OnPlayerDied -= new HealthPlayer.HPUpdated(this.Health_OnDie);
  }

  private void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.SignPostAttack)
      this.transform.position = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 50f;
    else
      this.transform.position = this.Master.transform.position + Vector3.right;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null || (double) GameManager.DeltaTime == 0.0 || MMConversation.isPlaying)
      return;
    if ((this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving) && (double) (this.AttackDelay += Time.deltaTime) > (double) this.AttackDurationCount && this.CanCollect)
    {
      this.AttackDurationCount = (float) UnityEngine.Random.Range(60, 120);
      this.AttackDelay = 0.0f;
      this.collectPosition = this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 50f;
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
          this.transform.position = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 50f;
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
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) < 1.5 && (UnityEngine.Object) PlayerFarming.Instance.gameObject != (UnityEngine.Object) null && !PlayerFarming.Instance.GoToAndStopping)
        {
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
    this.transform.position = this.transform.position + new Vector3(this.vx, this.vy);
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

  private void SeperateDemons()
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
