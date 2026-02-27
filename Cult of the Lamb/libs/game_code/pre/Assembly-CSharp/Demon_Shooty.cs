// Decompiled with JetBrains decompiler
// Type: Demon_Shooty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Demon_Shooty : Demon
{
  private float AttackProgress;
  private float AttackAngle;
  private float AttackDelay;
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
  public SpriteRenderer spriteRenderer;
  public SkeletonAnimation spine;
  public Transform ChainPoint;
  public LayerMask layerToCheck;
  private float attackDelay = 3f;
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
    Demon_Shooty demonShooty = this;
    while (demonShooty.spine.AnimationState == null)
      yield return (object) null;
    if (demonShooty.Level > 1)
    {
      demonShooty.spine.skeleton.SetSkin("Projectile+");
      demonShooty.spine.skeleton.SetSlotsToSetupPose();
      demonShooty.spine.AnimationState.Apply(demonShooty.spine.skeleton);
    }
  }

  public override void Init(int followerID)
  {
    base.Init(followerID);
    this.attackDelay = 3f;
    this.attackDelay -= 0.15f * (float) this.Level;
  }

  private void Health_OnDie(HealthPlayer player) => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    HealthPlayer.OnPlayerDied -= new HealthPlayer.HPUpdated(this.Health_OnDie);
  }

  private void BiomeGenerator_OnBiomeChangeRoom()
  {
    this.transform.position = this.Master.transform.position + Vector3.right;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null || (double) GameManager.DeltaTime == 0.0 || MMConversation.isPlaying)
      return;
    if ((this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving) && (double) (this.AttackDelay += Time.deltaTime) > (double) this.attackDelay)
    {
      this.AttackDelay = 0.0f;
      this.GetNewTarget();
      if ((UnityEngine.Object) this.CurrentTarget != (UnityEngine.Object) null)
        this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
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
        this.Speed += (float) ((0.0 - (double) this.Speed) / (7.0 * (double) GameManager.DeltaTime));
        if (Time.frameCount % 5 == 0)
          this.simpleSpineAnimator.FlashWhite(!this.simpleSpineAnimator.isFillWhite);
        if ((double) (this.state.Timer += Time.deltaTime) > 0.5)
        {
          this.simpleSpineAnimator.FlashWhite(false);
          if ((UnityEngine.Object) this.CurrentTarget != (UnityEngine.Object) null)
            this.state.facingAngle = Utils.GetAngle(this.transform.position, this.CurrentTarget.transform.position);
          CameraManager.shakeCamera(0.5f, this.state.facingAngle);
          Addressables.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/ArrowPlayer.prefab", this.spine.transform.position, Quaternion.identity, this.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
          {
            Projectile component = obj.Result.GetComponent<Projectile>();
            AudioManager.Instance.PlayOneShot("event:/player/Curses/fireball", component.gameObject);
            component.Angle = this.state.facingAngle;
            component.team = Health.Team.PlayerTeam;
            component.Explosive = false;
            component.CanKnockBack = false;
          });
          this.Speed = -5f;
          this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
          break;
        }
        break;
      case StateMachine.State.RecoverFromAttack:
        this.Speed += (float) ((0.0 - (double) this.Speed) / (20.0 * (double) GameManager.DeltaTime));
        if ((double) (this.state.Timer += Time.deltaTime) > 0.5)
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
      if (allUnit.team != this.MasterHealth.team && allUnit.team != Health.Team.Neutral && allUnit.enabled && !allUnit.invincible && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.CheckLineOfSight(allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
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
