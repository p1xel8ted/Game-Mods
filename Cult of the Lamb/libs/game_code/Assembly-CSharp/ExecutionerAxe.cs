// Decompiled with JetBrains decompiler
// Type: ExecutionerAxe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ExecutionerAxe : UnitObject, ISpellOwning
{
  [SerializeField]
  public Transform container;
  [SerializeField]
  public float rotationSpeed;
  [SerializeField]
  public SpriteRenderer renderer;
  [SerializeField]
  public ColliderEvents colliderEvent;
  [SerializeField]
  public float acceleration;
  [SerializeField]
  public float turningSpeed;
  [SerializeField]
  public Vector2 idleWaitRange = new Vector2(1f, 3f);
  [SerializeField]
  public float slashDistance;
  [SerializeField]
  public float slashAnticipation;
  [SerializeField]
  public float slashDuration;
  [SerializeField]
  public float slashForce;
  public string AxeThrowWhooshLoopSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_axe_throw_whoosh_loop";
  public EventInstance axeWhooshLoopInstanceSFX;
  public float idleWait;
  public bool attacking;
  public bool spawned;
  public float lifetime;
  public float angle;
  [CompilerGenerated]
  public bool \u003CDestroying\u003Ek__BackingField;
  public EnemyBruteBoss boss;
  public Health Origin;
  public static ExecutionerAxe AttackingAxe;
  public bool smackedBack;

  public bool Destroying
  {
    get => this.\u003CDestroying\u003Ek__BackingField;
    set => this.\u003CDestroying\u003Ek__BackingField = value;
  }

  public static void SpawnThrowingAxe(
    EnemyBruteBoss boss,
    Health target,
    float force,
    float duration,
    System.Action<ExecutionerAxe> callback)
  {
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Executioner Axe.prefab", boss.transform.position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      ExecutionerAxe component = obj.Result.GetComponent<ExecutionerAxe>();
      component.maxSpeed *= force;
      component.boss = boss;
      component.spawned = true;
      component.DoKnockBack(Utils.GetAngle(boss.transform.position, target.transform.position) * ((float) Math.PI / 180f), force, duration);
      System.Action<ExecutionerAxe> action = callback;
      if (action != null)
        action(component);
      component.StartLoopSFX();
    }));
  }

  public void Start()
  {
    this.colliderEvent.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.HitPlayer);
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      if (this.spawned)
        return;
      this.DoKnockBack(Utils.GetAngle(this.boss.transform.position, this.transform.position) * ((float) Math.PI / 180f), this.slashForce, this.slashDuration);
    }));
  }

  public void StartLoopSFX()
  {
    if (string.IsNullOrEmpty(this.AxeThrowWhooshLoopSFX))
      return;
    this.axeWhooshLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.AxeThrowWhooshLoopSFX, this.gameObject, true);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.attacking = false;
  }

  public override void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.axeWhooshLoopInstanceSFX);
    base.OnDisable();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.colliderEvent.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.HitPlayer);
  }

  public override void Update()
  {
    base.Update();
    this.container.transform.eulerAngles += new Vector3(0.0f, 0.0f, this.rotationSpeed * Time.deltaTime * this.boss.Spine.timeScale);
    this.lifetime += Time.deltaTime * this.boss.Spine.timeScale;
    if (this.attacking)
      return;
    if (this.state.CURRENT_STATE == StateMachine.State.Idle && (double) (this.idleWait -= Time.deltaTime * this.boss.Spine.timeScale) <= 0.0 && !this.DisableForces)
      this.GetNewTargetPosition();
    if (this.spawned && this.state.CURRENT_STATE == StateMachine.State.Moving && (double) (this.idleWait -= Time.deltaTime * this.boss.Spine.timeScale) <= 0.0 && !this.DisableForces)
      this.GetNewTargetPosition();
    if (!this.spawned && (double) Vector3.Distance(this.boss.transform.position, this.transform.position) > 5.0 && (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) > 5.0 && (double) this.idleWait <= 0.0 && !this.DisableForces)
      this.GetNewTargetPosition();
    else if (this.spawned && (double) Vector3.Distance(this.boss.transform.position, this.transform.position) < 1.5 && (double) this.lifetime > 1.0 && !this.Destroying)
    {
      if (this.smackedBack)
      {
        this.boss.health.DealDamage(this.boss.health.totalHP * 0.1f, this.boss.gameObject, this.boss.transform.position);
        this.boss.KnockedOut();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else
      {
        this.Destroying = true;
        this.transform.DOScale(0.0f, 0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
      }
    }
    if (this.UsePathing)
    {
      if (this.pathToFollow == null)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.boss.Spine.timeScale;
        this.move();
        return;
      }
      if (this.currentWaypoint >= this.pathToFollow.Count)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.boss.Spine.timeScale;
        this.move();
        return;
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / (7.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.boss.Spine.timeScale;
      if (this.UsePathing)
      {
        this.state.facingAngle = Mathf.LerpAngle(this.state.facingAngle, Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]), Time.deltaTime * this.turningSpeed * this.boss.Spine.timeScale);
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance)
        {
          ++this.currentWaypoint;
          if (this.currentWaypoint == this.pathToFollow.Count)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            System.Action endOfPath = this.EndOfPath;
            if (endOfPath != null)
              endOfPath();
            this.pathToFollow = (List<Vector3>) null;
          }
        }
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.boss.Spine.timeScale;
    if (!this.spawned && (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) < (double) this.slashDistance && (UnityEngine.Object) ExecutionerAxe.AttackingAxe == (UnityEngine.Object) null)
      this.Slash();
    this.move();
  }

  public void GetNewTargetPosition()
  {
    if (this.spawned)
    {
      this.givePath(this.boss.transform.position);
    }
    else
    {
      if ((double) Vector3.Distance(this.boss.transform.position, this.transform.position) > 5.0 && (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) > 5.0)
        this.givePath(PlayerFarming.Instance.transform.position);
      else
        this.givePath(BiomeGenerator.GetRandomPositionInIsland());
      this.idleWait = UnityEngine.Random.Range(this.idleWaitRange.x, this.idleWaitRange.y);
    }
  }

  public void HitPlayer(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team != Health.Team.PlayerTeam && component.team != Health.Team.Neutral || this.smackedBack || this.boss.health.TimeFrozen)
      return;
    component.DealDamage(1f, this.gameObject, this.transform.position);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!((UnityEngine.Object) Attacker == (UnityEngine.Object) PlayerFarming.Instance.gameObject) || (double) this.lifetime <= 1.5 || (double) Vector3.Distance(this.transform.position, this.boss.transform.position) <= 3.0 && (double) this.lifetime >= 2.0)
      return;
    this.StopAllCoroutines();
    this.rb.velocity = Vector2.zero;
    GameManager.GetInstance().HitStop(0.2f);
    this.TargetEnemy = this.boss.health;
    this.attacking = false;
    this.maxSpeed *= 2f;
    this.acceleration *= 2f;
    this.DisableForces = false;
    this.speed = this.maxSpeed;
    this.smackedBack = true;
    this.lifetime = float.MaxValue;
    if (!((UnityEngine.Object) ExecutionerAxe.AttackingAxe == (UnityEngine.Object) this))
      return;
    ExecutionerAxe.AttackingAxe = (ExecutionerAxe) null;
  }

  public void Slash() => this.StartCoroutine((IEnumerator) this.SlashIE());

  public IEnumerator SlashIE()
  {
    ExecutionerAxe executionerAxe = this;
    ExecutionerAxe.AttackingAxe = executionerAxe;
    executionerAxe.attacking = true;
    yield return (object) executionerAxe.StartCoroutine((IEnumerator) executionerAxe.FadeWhite(executionerAxe.slashAnticipation));
    executionerAxe.rb.velocity = Vector2.zero;
    executionerAxe.DoKnockBack(Utils.GetAngle(executionerAxe.transform.position, PlayerFarming.Instance.transform.position) * ((float) Math.PI / 180f), executionerAxe.slashForce, executionerAxe.slashDuration);
    yield return (object) new WaitForSeconds(executionerAxe.slashDuration * executionerAxe.boss.Spine.timeScale);
    ExecutionerAxe.AttackingAxe = (ExecutionerAxe) null;
    executionerAxe.attacking = false;
  }

  public IEnumerator FadeWhite(float duration)
  {
    float time = 0.0f;
    while ((double) time < (double) duration)
    {
      time += Time.deltaTime;
      this.renderer.material.SetFloat("_FillAlpha", Mathf.Lerp(0.0f, 1f, time / duration));
      yield return (object) null;
    }
    this.renderer.material.SetFloat("_FillAlpha", 0.0f);
  }

  public GameObject GetOwner()
  {
    return !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();

  [CompilerGenerated]
  public void \u003CStart\u003Eb__28_0()
  {
    if (this.spawned)
      return;
    this.DoKnockBack(Utils.GetAngle(this.boss.transform.position, this.transform.position) * ((float) Math.PI / 180f), this.slashForce, this.slashDuration);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__33_0() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [Serializable]
  public class Move
  {
    public AnimationCurve verticalCurve;
    public float verticalStrength;
    public AnimationCurve horizontalCurve;
    public float horizontalStrength;
  }
}
