// Decompiled with JetBrains decompiler
// Type: ProjectileGhost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ProjectileGhost : BaseMonoBehaviour
{
  public static List<ProjectileGhost> Projectiles = new List<ProjectileGhost>();
  public float AttackInterval;
  public float AttackDelay;
  public SimpleSpineAnimator simpleSpineAnimator;
  public TrailRenderer TrailRenderer;
  public Collider2D DamageCollider;
  public Health health;
  public List<Collider2D> collider2DList;
  public Health CollisionHealth;
  public float DetectEnemyRange = 50f;
  public GameObject _Master;
  public Health MasterHealth;
  public StateMachine MasterState;
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
  public List<Health> DoubleHit;
  public float TurnSpeed;
  public EventInstance sfxLoop;
  public float damageMultiplier = 1f;
  public static AsyncOperationHandle<GameObject> ghostPrefabHandle;
  public float checkFrame;
  public Health cachedTarget;
  public Vector3 pointToCheck;
  public Vector3 Seperator;
  public float SeperationRadius = 0.5f;

  public GameObject Master
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

  public static void SpawnGhost(Vector3 position, float delay, float damageMultplier, float scale = 1f)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) ProjectileGhost.DelayCallback(delay, (System.Action) (() => ProjectileGhost.SpawnGhost(position, damageMultplier, scale))));
  }

  public static void Unload()
  {
    if (!ProjectileGhost.ghostPrefabHandle.IsValid())
      return;
    Addressables.Release<GameObject>(ProjectileGhost.ghostPrefabHandle);
  }

  public static IEnumerator DelayCallback(float delay, System.Action callback)
  {
    if (!ProjectileGhost.ghostPrefabHandle.IsValid())
    {
      ProjectileGhost.ghostPrefabHandle = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/ArrowGhost.prefab");
      yield return (object) new WaitUntil((Func<bool>) (() => ProjectileGhost.ghostPrefabHandle.IsDone));
    }
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void OnEnable()
  {
    if ((UnityEngine.Object) this.health == (UnityEngine.Object) null)
      this.health = this.GetComponent<Health>();
    ProjectileGhost.Projectiles.Add(this);
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.sfxLoop);
    ProjectileGhost.Projectiles.Remove(this);
  }

  public static void SpawnGhost(Vector3 position, float damageMultiplier, float scale = 1f)
  {
    if ((UnityEngine.Object) ProjectileGhost.ghostPrefabHandle.Result == (UnityEngine.Object) null)
      return;
    ProjectileGhost component = UnityEngine.Object.Instantiate<GameObject>(ProjectileGhost.ghostPrefabHandle.Result, position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform).GetComponent<ProjectileGhost>();
    component.transform.localScale *= scale;
    component.damageMultiplier = damageMultiplier;
    scale = component.transform.localScale.x;
    component.transform.localScale = Vector3.zero;
    component.transform.DOScale(scale, 0.1f);
  }

  public void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.SpineVZ = -1.5f;
    this.SpineVY = 0.5f;
    this.state.CURRENT_STATE = StateMachine.State.SpawnIn;
    AudioManager.Instance.PlayOneShot("event:/weapon/necromancer_ghost/ghost_spawn", this.gameObject);
    this.sfxLoop = AudioManager.Instance.CreateLoop("event:/weapon/necromancer_ghost/ghost_loop", this.gameObject, true);
    this.TurnSpeed = UnityEngine.Random.Range(1f, 7f);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.StopLoop(this.sfxLoop);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.sfxLoop);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    if (!(bool) (UnityEngine.Object) PlayerFarming.Instance)
      return;
    PlayerFarming.Instance.health.OnDie -= new Health.DieAction(this.Health_OnDie);
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    this.transform.position = this.Master.transform.position + Vector3.right;
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null || !this.Master.gameObject.activeSelf || !this.gameObject.activeSelf || (double) GameManager.DeltaTime == 0.0)
      return;
    if ((this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving) && (double) (this.AttackDelay += Time.deltaTime) > (double) this.AttackInterval)
    {
      this.AttackDelay = 0.0f;
      if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
      {
        AudioManager.Instance.PlayOneShot("event:/weapon/necromancer_ghost/ghost_attack", this.gameObject);
        this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      }
    }
    if (this.state.CURRENT_STATE != StateMachine.State.SignPostAttack && (double) this.state.Timer > 1.0)
    {
      if (this.state.CURRENT_STATE != StateMachine.State.SpawnOut)
        AudioManager.Instance.PlayOneShot("event:/weapon/necromancer_ghost/ghost_leave", this.transform.position);
      this.state.CURRENT_STATE = StateMachine.State.SpawnOut;
    }
    this.TrailRenderer.emitting = this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.SignPostAttack || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.state.Timer += Time.deltaTime;
        break;
      case StateMachine.State.Moving:
        this.state.Timer += Time.deltaTime;
        break;
      case StateMachine.State.SignPostAttack:
        this.Speed += (float) ((0.0 - (double) this.Speed) / (7.0 * (double) GameManager.DeltaTime));
        if (Time.frameCount % 5 == 0)
          this.simpleSpineAnimator.FlashWhite(!this.simpleSpineAnimator.isFillWhite);
        if ((double) (this.state.Timer += Time.deltaTime) > 0.20000000298023224)
        {
          this.simpleSpineAnimator.FlashWhite(false);
          if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
            this.TargetAngle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
          CameraManager.shakeCamera(0.5f, this.state.facingAngle);
          this.Speed = 30f;
          this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
          this.DoubleHit.Clear();
          break;
        }
        break;
      case StateMachine.State.RecoverFromAttack:
        if ((double) this.state.Timer < 2.0)
        {
          if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
            this.TargetAngle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
          this.collider2DList = new List<Collider2D>();
          this.DamageCollider.GetContacts((List<Collider2D>) this.collider2DList);
          foreach (Component component in this.collider2DList)
          {
            this.CollisionHealth = component.gameObject.GetComponent<Health>();
            if (!this.DoubleHit.Contains(this.CollisionHealth) && (UnityEngine.Object) this.CollisionHealth != (UnityEngine.Object) null && !this.CollisionHealth.invincible && !this.CollisionHealth.untouchable && this.CollisionHealth.team == Health.Team.Team2)
            {
              this.CollisionHealth.DealDamage(1f * this.damageMultiplier, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
              AudioManager.Instance.StopLoop(this.sfxLoop);
              UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
              return;
            }
          }
        }
        if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < 2.0)
        {
          this.GetClosestTarget().DealDamage(1f * this.damageMultiplier, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
          AudioManager.Instance.StopLoop(this.sfxLoop);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
          return;
        }
        if ((double) this.Speed > 0.0)
        {
          if (this.DoubleHit.Count > 0)
            this.Speed -= 2f * GameManager.DeltaTime;
          else
            this.Speed -= 1f * GameManager.DeltaTime;
          if ((double) this.Speed <= 0.0)
            this.Speed = 0.0f;
        }
        if ((double) (this.state.Timer += Time.deltaTime) > 2.0)
        {
          this.state.CURRENT_STATE = StateMachine.State.SpawnOut;
          AudioManager.Instance.PlayOneShot("event:/weapon/necromancer_ghost/ghost_leave", this.transform.position);
          break;
        }
        break;
      case StateMachine.State.SpawnIn:
        if ((double) (this.state.Timer += Time.deltaTime) > 0.5)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
        }
        break;
      case StateMachine.State.SpawnOut:
        if ((double) (this.state.Timer += Time.deltaTime) > 0.949999988079071)
        {
          AudioManager.Instance.StopLoop(this.sfxLoop);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
          break;
        }
        break;
    }
    this.vx = this.Speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    this.vy = this.Speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    this.spine.skeleton.ScaleX = (double) this.Master.transform.position.x > (double) this.transform.position.x ? -1f : 1f;
    this.spine.transform.eulerAngles = new Vector3(-60f, 0.0f, this.vx * -5f / Time.deltaTime);
    this.SpineVZ = Mathf.Lerp(this.SpineVZ, -1f, 5f * Time.deltaTime);
    this.SpineVY = Mathf.Lerp(this.SpineVY, 0.5f, 5f * Time.deltaTime);
    this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (double) this.TurnSpeed * (double) Time.deltaTime * 60.0);
    this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime, this.Speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime);
    this.SeperateDemons();
  }

  public Health GetClosestTarget()
  {
    if ((double) Time.time == (double) this.checkFrame)
      return this.cachedTarget;
    Health.Team team = Health.Team.Team2;
    List<Health> healthList = new List<Health>();
    foreach (Health health in Health.team2)
    {
      if (health.enabled && !health.invincible && !health.untouchable && !health.InanimateObject && (double) health.HP > 0.0 && (bool) (UnityEngine.Object) health && health.team == team)
        healthList.Add(health);
    }
    Health closestTarget = (Health) null;
    foreach (Health health in healthList)
    {
      float num = Vector3.Distance(health.transform.position, this.transform.position);
      if ((double) num <= (double) this.DetectEnemyRange && ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null || (double) num < (double) Vector3.Distance(closestTarget.transform.position, this.transform.position)))
        closestTarget = health;
    }
    this.checkFrame = Time.time;
    this.cachedTarget = closestTarget;
    return closestTarget;
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

  public static bool Contains(Health health)
  {
    foreach (ProjectileGhost projectile in ProjectileGhost.Projectiles)
    {
      if ((UnityEngine.Object) projectile != (UnityEngine.Object) null && (UnityEngine.Object) projectile == (UnityEngine.Object) health)
        return true;
    }
    return false;
  }
}
