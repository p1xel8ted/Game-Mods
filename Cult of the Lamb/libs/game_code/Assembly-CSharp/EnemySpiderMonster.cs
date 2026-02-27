// Decompiled with JetBrains decompiler
// Type: EnemySpiderMonster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class EnemySpiderMonster : UnitObject
{
  public Interaction_MonsterHeart interaction_MonsterHeart;
  public bool Mutated;
  public Vector3 StartPosition;
  public SimpleSpineEventListener simpleSpineEventListener;
  public MeshRenderer SpineMonster;
  public SpriteRenderer Shadow;
  public ColliderEvents damageColliderEvents;
  public Coroutine damageColliderCoroutine;
  public CircleCollider2D Collider;
  public GameObject CameraTarget;
  public float SeperationRadius = 0.5f;
  public GameObject TargetObject;
  public Vector2 Range = new Vector2(6f, 3f);
  public float KnockbackSpeed = 0.1f;
  public SimpleSpineAnimator simpleSpineAnimator;
  public SimpleSpineFlash simpleSpineFlash;
  public GameObject poisonBomb;
  public Transform BombPoint;
  public GameObject SpawnWeb;
  public GameObject SpawnSlime;
  [SerializeField]
  public GameObject bulletPrefab;
  [SerializeField]
  public float numberOfShotsToFire = 45f;
  [SerializeField]
  public Vector2 gravSpeed;
  [SerializeField]
  public float arc;
  [SerializeField]
  public Vector2 randomArcOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  public Vector2 shootDistanceRange = new Vector2(2f, 3f);
  [SerializeField]
  public GameObject ShootBone;
  [Space]
  public ProjectilePattern LandProjectilePattern;
  [SerializeField]
  public GameObject followerToSpawn;
  public List<SpiderNest> spiderNests = new List<SpiderNest>();
  public bool active;
  public bool juicedForm;
  public Health EnemyHealth;
  public float FireWebsDelay;
  public EnemySpiderMonster.BombType CurrentBombType;
  public int Ammo;
  public float ChargeDelay;
  public bool JumpAttacking;
  public bool JumpAroundAttacking;
  public float ZipAwayDelay;
  public Vector3 ShadowSize;
  public Coroutine cShrinkShadow;
  public float CloseRangeAttackDelay;
  public float AttackDashSpeed;
  public float AttackSpeedValue = 0.6f;
  public bool DontPlayHurtAnimation = true;
  public bool Roared;

  public override void Awake()
  {
    base.Awake();
    this.spiderNests = ((IEnumerable<SpiderNest>) UnityEngine.Object.FindObjectsOfType<SpiderNest>()).ToList<SpiderNest>();
    foreach (SpiderNest spiderNest in this.spiderNests)
      spiderNest.Droppable = false;
    this.juicedForm = GameManager.Layer2;
    if (this.juicedForm)
    {
      this.health.totalHP *= 1.5f;
      this.health.HP = this.health.totalHP;
      this.numberOfShotsToFire *= 1.5f;
      for (int index = 0; index < this.LandProjectilePattern.Waves.Length; ++index)
      {
        this.LandProjectilePattern.Waves[index].Bullets = (int) ((double) this.LandProjectilePattern.Waves[index].Bullets * 1.5);
        this.LandProjectilePattern.Waves[index].AngleBetweenBullets = 360f / (float) this.LandProjectilePattern.Waves[index].Bullets;
        this.LandProjectilePattern.Waves[index].Speed *= 1.5f;
        this.LandProjectilePattern.Waves[index].FinishDelay /= 1.5f;
      }
    }
    this.health.SlowMoOnkill = false;
    this.InitializeMortarStrikes();
    this.InitializeGrenadeBullets();
  }

  public void Start()
  {
    SimulationManager.Pause();
    if (!((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null))
      return;
    SkeletonAnimationLODGlobalManager.Instance.DisableCulling(this.transform, this.simpleSpineAnimator.anim);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!GameManager.RoomActive || !this.active)
      return;
    this.health.enabled = true;
    this.StartCoroutine(this.Roar());
    this.StartCoroutine(this.DelayAddCamera());
  }

  public IEnumerator DelayAddCamera()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySpiderMonster enemySpiderMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().AddToCamera(enemySpiderMonster.gameObject, 0.25f);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void InitializeMortarStrikes()
  {
    List<MortarBomb> mortarBombList = new List<MortarBomb>();
    int num = 10;
    for (int index = 0; index < num; ++index)
    {
      MortarBomb component = ObjectPool.Spawn(this.poisonBomb, this.transform.parent).GetComponent<MortarBomb>();
      component.destroyOnFinish = false;
      mortarBombList.Add(component);
    }
    for (int index = 0; index < mortarBombList.Count; ++index)
      mortarBombList[index].gameObject.Recycle();
  }

  public void InitializeGrenadeBullets()
  {
    ObjectPool.CreatePool(this.bulletPrefab, (int) this.numberOfShotsToFire * 6);
  }

  public void OnSpineEvent(string EventName)
  {
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(EventName))
    {
      case 502195891:
        if (!(EventName == "roar shake"))
          break;
        CameraManager.instance.ShakeCameraForDuration(0.1f, 0.3f, 1f);
        CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        break;
      case 545011306:
        if (!(EventName == "turn on colliders"))
          break;
        this.SeperateObject = true;
        foreach (PlayerFarming player in PlayerFarming.players)
          Physics2D.IgnoreCollision((Collider2D) this.Collider, (Collider2D) player.circleCollider2D, false);
        this.health.invincible = false;
        break;
      case 1017970633:
        if (!(EventName == "battlecry"))
          break;
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_acidslime", this.gameObject);
        break;
      case 1207211535:
        if (!(EventName == "show name"))
          break;
        HUD_DisplayName.Play("Kumo");
        break;
      case 1274151103:
        if (!(EventName == "jump attack"))
          break;
        this.JumpAttacking = false;
        this.speed = 0.0f;
        this.DoDamageAttack(0.0f);
        AudioManager.Instance.PlayOneShot("event:/boss/spider/jump_attack_land", this.gameObject);
        if (!this.simpleSpineAnimator.IsVisible)
          break;
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
        break;
      case 1454297646:
        if (!(EventName == "grow shadow"))
          break;
        this.StartCoroutine(this.GrowShadow());
        break;
      case 1785338614:
        if (!(EventName == "turn off colliders"))
          break;
        this.SeperateObject = false;
        foreach (PlayerFarming player in PlayerFarming.players)
          Physics2D.IgnoreCollision((Collider2D) this.Collider, (Collider2D) player.circleCollider2D, true);
        this.health.invincible = true;
        break;
      case 1844065948:
        if (!(EventName == "land") || !this.JumpAroundAttacking)
          break;
        this.StartCoroutine(this.LandProjectilePattern.ShootIE(0.0f, (GameObject) null, (Transform) null, false));
        this.JumpAroundAttacking = false;
        break;
      case 2151091521:
        int num1 = EventName == "start flash" ? 1 : 0;
        break;
      case 2163513076:
        if (!(EventName == "shrink shadow"))
          break;
        this.cShrinkShadow = this.StartCoroutine(this.ShrinkShadow());
        break;
      case 2704129201:
        if (!(EventName == "webshot"))
          break;
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_acidslime", this.gameObject);
        break;
      case 3049036174:
        if (!(EventName == "mutate zoom"))
          break;
        GameManager.GetInstance().OnConversationNext(this.CameraTarget, 6f);
        break;
      case 3155283432:
        if (!(EventName == "deal damage"))
          break;
        this.DoDamageAttack(0.0f);
        break;
      case 3399030161:
        if (!(EventName == "bomb"))
          break;
        Debug.Log((object) ("BOMB!   " + this.CurrentBombType.ToString()));
        AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", this.gameObject);
        switch (this.CurrentBombType)
        {
          case EnemySpiderMonster.BombType.Web:
            float num2 = (float) (120 * this.Ammo) * ((float) Math.PI / 180f);
            MortarBomb component1 = ObjectPool.Spawn(this.poisonBomb, this.transform.parent, (Vector3) (UnityEngine.Random.insideUnitCircle * 8f), Quaternion.identity).GetComponent<MortarBomb>();
            component1.destroyOnFinish = false;
            component1.Play(this.BombPoint.position, this.juicedForm ? 1.5f : 2f, Health.Team.Team2);
            ++this.Ammo;
            break;
          case EnemySpiderMonster.BombType.Egg:
            for (int index = 0; (double) index < (double) this.numberOfShotsToFire; ++index)
            {
              float angle = Utils.GetAngle(this.transform.position, this.TargetEnemy.transform.position);
              GrenadeBullet component2 = ObjectPool.Spawn(this.bulletPrefab, this.ShootBone.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
              component2.SetOwner(this.gameObject);
              component2.Play(-6f, angle + UnityEngine.Random.Range(this.randomArcOffset.x, this.randomArcOffset.y), UnityEngine.Random.Range(this.shootDistanceRange.x, this.shootDistanceRange.y), UnityEngine.Random.Range(this.gravSpeed.x, this.gravSpeed.y), this.health.team);
            }
            break;
        }
        CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        break;
      case 4013347183:
        int num3 = EventName == "stop flash" ? 1 : 0;
        break;
      case 4049887943:
        if (!(EventName == "dash attack"))
          break;
        CameraManager.shakeCamera(0.4f, this.state.facingAngle);
        this.AttackDashSpeed = this.AttackSpeedValue;
        break;
      case 4077446761:
        if (!(EventName == "die-explode"))
          break;
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.gameObject);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.gameObject);
        break;
    }
  }

  public void DoDamageAttack(float startDelay)
  {
    if (this.damageColliderCoroutine != null)
      this.StopCoroutine(this.damageColliderCoroutine);
    this.damageColliderCoroutine = this.StartCoroutine(this.DoDamageAttackTimed(startDelay, 0.1f));
  }

  public IEnumerator DoDamageAttackTimed(float startDelay, float dur)
  {
    if (!((UnityEngine.Object) this.damageColliderEvents == (UnityEngine.Object) null))
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.simpleSpineAnimator.anim.timeScale) < (double) startDelay)
        yield return (object) null;
      this.damageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.simpleSpineAnimator.anim.timeScale) < (double) dur)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
      this.damageColliderCoroutine = (Coroutine) null;
    }
  }

  public IEnumerator WaitForTarget()
  {
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
      yield return (object) null;
    while ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      this.TargetObject = PlayerFarming.Instance.gameObject;
      yield return (object) null;
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    if (!GameManager.GetInstance()._CamFollowTarget.Contains(enemySpiderMonster.gameObject))
    {
      GameManager.GetInstance().AddToCamera(enemySpiderMonster.gameObject, 0.25f);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    }
    enemySpiderMonster.TargetEnemy = enemySpiderMonster.ReconsiderPlayerTarget();
    enemySpiderMonster.TargetObject = (UnityEngine.Object) enemySpiderMonster.TargetEnemy != (UnityEngine.Object) null ? enemySpiderMonster.TargetEnemy.gameObject : enemySpiderMonster.TargetObject;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) enemySpiderMonster.damageColliderEvents != (UnityEngine.Object) null)
      enemySpiderMonster.damageColliderEvents.SetActive(false);
    float ActionDelay = enemySpiderMonster.Roared ? UnityEngine.Random.Range(0.3f, 0.5f) : UnityEngine.Random.Range(0.5f, 1f);
    enemySpiderMonster.givePath(enemySpiderMonster.TargetObject.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 2f)));
    enemySpiderMonster.speed = 0.0f;
    float RepathTimer = 0.0f;
    while (true)
    {
      if ((double) enemySpiderMonster.speed < (double) enemySpiderMonster.maxSpeed)
        enemySpiderMonster.speed += 0.01f * Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
      enemySpiderMonster.state.facingAngle = Utils.SmoothAngle(enemySpiderMonster.state.facingAngle, Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position), 10f);
      if ((double) (RepathTimer += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) > 0.25)
      {
        enemySpiderMonster.givePath(enemySpiderMonster.TargetObject.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 2f)));
        RepathTimer = 0.0f;
        AudioManager.Instance.PlayOneShot("event:/boss/spider/footstep", enemySpiderMonster.gameObject);
      }
      enemySpiderMonster.CloseRangeAttackDelay -= Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
      enemySpiderMonster.ZipAwayDelay -= Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
      enemySpiderMonster.ChargeDelay -= Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
      enemySpiderMonster.FireWebsDelay -= Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
      float num = Vector3.Distance(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position);
      if ((double) (ActionDelay -= Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) <= 0.0)
      {
        if ((double) num >= 5.0 || (double) enemySpiderMonster.CloseRangeAttackDelay > 0.0)
        {
          if ((double) enemySpiderMonster.FireWebsDelay > 0.0)
          {
            if ((double) enemySpiderMonster.ZipAwayDelay > 0.0)
            {
              if ((double) enemySpiderMonster.ChargeDelay <= 0.0)
                goto label_17;
            }
            else
              goto label_15;
          }
          else
            goto label_13;
        }
        else
          break;
      }
      yield return (object) null;
    }
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.CloseRangeAttack());
    yield break;
label_13:
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.FireWebs());
    yield break;
label_15:
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.ZipAway());
    yield break;
label_17:
    if (UnityEngine.Random.Range(0, 2) == 0)
      enemySpiderMonster.StartCoroutine(enemySpiderMonster.JumpAttack());
    else
      enemySpiderMonster.StartCoroutine(enemySpiderMonster.JumpAround());
  }

  public IEnumerator FireWebs()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.Ammo = 0;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (enemySpiderMonster.CurrentBombType == EnemySpiderMonster.BombType.Web)
      enemySpiderMonster.simpleSpineAnimator.Animate("bomb-even-more", 0, false);
    else
      enemySpiderMonster.simpleSpineAnimator.Animate(enemySpiderMonster.Roared ? "bomb-more" : "bombs", 0, false);
    enemySpiderMonster.simpleSpineAnimator.AddAnimate("idle-boss", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_start", enemySpiderMonster.gameObject);
    float time = 0.0f;
    if (enemySpiderMonster.CurrentBombType == EnemySpiderMonster.BombType.Web)
    {
      while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 4.3000001907348633)
        yield return (object) null;
    }
    else
    {
      while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < (enemySpiderMonster.Roared ? 3.0 : 2.2999999523162842))
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_end", enemySpiderMonster.gameObject);
    enemySpiderMonster.FireWebsDelay = 2f;
    enemySpiderMonster.CurrentBombType = (EnemySpiderMonster.BombType) UnityEngine.Random.Range(0, 2);
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.ChasePlayer());
  }

  public IEnumerator JumpAttack()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("angry intro", 0, true);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/angry", enemySpiderMonster.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 1.0)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/jump_attack_jump", enemySpiderMonster.gameObject);
    int Loop = UnityEngine.Random.Range(2, 5);
    while (Loop > 0)
    {
      float degree = Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position);
      int num = 0;
      while (num++ < 32 /*0x20*/ && (bool) Physics2D.Raycast((Vector2) enemySpiderMonster.transform.position, Utils.DegreeToVector2(degree), 5f, (int) enemySpiderMonster.layerToCheck))
        degree = Utils.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      enemySpiderMonster.state.facingAngle = degree;
      enemySpiderMonster.JumpAttacking = true;
      enemySpiderMonster.simpleSpineAnimator.Animate("jump-attack", 0, false);
      CameraManager.shakeCamera(0.3f, enemySpiderMonster.state.facingAngle);
      while (enemySpiderMonster.JumpAttacking)
      {
        enemySpiderMonster.speed = 15f * Time.deltaTime;
        yield return (object) null;
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 0.5)
        yield return (object) null;
      --Loop;
      yield return (object) null;
    }
    enemySpiderMonster.ChargeDelay = 1f;
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.ChasePlayer());
  }

  public IEnumerator JumpAround()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("angry intro", 0, true);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/angry", enemySpiderMonster.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 1.0)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/jump_attack_jump", enemySpiderMonster.gameObject);
    int Loop = UnityEngine.Random.Range(2, 5);
    while (Loop > 0)
    {
      float degree = Utils.GetAngle(enemySpiderMonster.transform.position, (Vector3) (UnityEngine.Random.insideUnitCircle * 5f));
      int num = 0;
      while (num++ < 32 /*0x20*/ && (bool) Physics2D.Raycast((Vector2) enemySpiderMonster.transform.position, Utils.DegreeToVector2(degree), 5f, (int) enemySpiderMonster.layerToCheck))
        degree = Utils.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      enemySpiderMonster.state.facingAngle = degree;
      enemySpiderMonster.JumpAroundAttacking = true;
      enemySpiderMonster.simpleSpineAnimator.Animate("jump", 0, false);
      CameraManager.shakeCamera(0.3f, enemySpiderMonster.state.facingAngle);
      while (enemySpiderMonster.JumpAroundAttacking)
      {
        enemySpiderMonster.speed = 15f * Time.deltaTime;
        yield return (object) null;
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 0.5)
        yield return (object) null;
      --Loop;
      yield return (object) null;
    }
    enemySpiderMonster.ChargeDelay = 1f;
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.ChasePlayer());
  }

  public IEnumerator ZipAway()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("swing-away", 0, false);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/swing_away", enemySpiderMonster.gameObject);
    string soundPath = "event:/dialogue/dun4_cult_leader_shamura/battle_cry_shamura";
    if (GameManager.Layer2)
      soundPath = "event:/dialogue/dun4_cult_leader_shamura/undead_battle_cry_shamura";
    AudioManager.Instance.PlayOneShot(soundPath, enemySpiderMonster.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 2.5)
      yield return (object) null;
    enemySpiderMonster.transform.position = enemySpiderMonster.TargetObject.transform.position;
    enemySpiderMonster.simpleSpineAnimator.Animate("swing-in", 0, false);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/swing_in", enemySpiderMonster.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 0.5)
      yield return (object) null;
    CameraManager.shakeCamera(0.7f, Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position));
    int num = 1;
    for (int index = 0; index < num && enemySpiderMonster.spiderNests.Count > 0; ++index)
    {
      SpiderNest spiderNest = enemySpiderMonster.spiderNests[UnityEngine.Random.Range(0, enemySpiderMonster.spiderNests.Count)];
      spiderNest.Droppable = true;
      enemySpiderMonster.spiderNests.Remove(spiderNest);
      spiderNest.DropEnemies();
    }
    foreach (SpiderNest spiderNest in enemySpiderMonster.spiderNests)
      spiderNest.Shake();
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 0.64999997615814209)
      yield return (object) null;
    enemySpiderMonster.ZipAwayDelay = 4f;
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.ChasePlayer());
  }

  public IEnumerator ShrinkShadow()
  {
    float Scale = 1f;
    while ((double) Scale > 0.0)
    {
      Scale -= Time.deltaTime * 5f * this.simpleSpineAnimator.anim.timeScale;
      this.Shadow.transform.localScale = this.ShadowSize * Scale;
      yield return (object) null;
    }
    this.Shadow.transform.localScale = Vector3.zero;
  }

  public IEnumerator GrowShadow()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    if (enemySpiderMonster.cShrinkShadow != null)
      enemySpiderMonster.StopCoroutine(enemySpiderMonster.cShrinkShadow);
    enemySpiderMonster.cShrinkShadow = (Coroutine) null;
    float Scale = 0.0f;
    while ((double) Scale < 1.0)
    {
      Scale += Time.deltaTime * 3f * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
      enemySpiderMonster.Shadow.transform.localScale = enemySpiderMonster.ShadowSize * Scale;
      yield return (object) null;
    }
    enemySpiderMonster.Shadow.transform.localScale = enemySpiderMonster.ShadowSize;
  }

  public IEnumerator CloseRangeAttack()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/attack", enemySpiderMonster.gameObject);
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderMonster.simpleSpineAnimator.Animate("attack", 0, false);
    enemySpiderMonster.simpleSpineAnimator.AddAnimate("idle-boss", 0, true, 0.0f);
    enemySpiderMonster.state.facingAngle = Utils.GetAngle(enemySpiderMonster.transform.position, enemySpiderMonster.TargetObject.transform.position);
    enemySpiderMonster.AttackDashSpeed = 0.0f;
    float Duration = 2f;
    enemySpiderMonster.DoDamageAttack(0.5f);
    while ((double) (Duration -= Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) > 0.0)
    {
      if ((double) enemySpiderMonster.AttackDashSpeed > 0.0)
      {
        enemySpiderMonster.AttackDashSpeed -= 3f * Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale;
        enemySpiderMonster.speed = enemySpiderMonster.AttackDashSpeed;
      }
      yield return (object) null;
    }
    enemySpiderMonster.CloseRangeAttackDelay = 1f;
    enemySpiderMonster.StartCoroutine(enemySpiderMonster.ChasePlayer());
  }

  public void Play()
  {
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.simpleSpineEventListener.skeletonAnimation.ForceVisible = true;
    this.StartCoroutine(this.WaitForTarget());
    this.SeperateObject = true;
    this.StartPosition = this.transform.position;
    this.ShadowSize = this.Shadow.transform.localScale;
    this.TargetObject = PlayerFarming.Instance.gameObject;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.active = true;
    this.StartCoroutine(this.ChasePlayer());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.simpleSpineFlash.FlashFillRed(0.25f);
    AudioManager.Instance.PlayOneShot("event:/boss/spider/gethit", this.gameObject);
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    BiomeConstants.Instance.EmitHitVFX(AttackLocation - Vector3.back * 0.5f, Quaternion.identity.z, "HitFX_Weak");
    if (!this.Roared && (double) this.health.HP <= (double) this.health.totalHP * 0.5)
    {
      AudioManager.Instance.PlayOneShot("event:/dialogue/dun4_cult_leader_shamura/wounded_shamura", this.gameObject);
      this.StopAllCoroutines();
      this.DisableForces = false;
      if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
        this.damageColliderEvents.SetActive(false);
      this.StartCoroutine(this.Roar());
    }
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!(bool) (UnityEngine.Object) DungeonSandboxManager.Instance)
      {
        player.health.invincible = true;
        player.health.untouchable = true;
      }
      player.playerWeapon.DoSlowMo(false);
    }
    RoomLockController.RoomCompleted(true, false);
    if (this.state.CURRENT_STATE == StateMachine.State.Dieing)
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_BOSS_4"));
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    GameObject gameObject = BiomeConstants.Instance.GroundSmash_Medium.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    this.damageColliderEvents.gameObject.SetActive(false);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.StartCoroutine(this.Die());
  }

  public IEnumerator Roar()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/roar", enemySpiderMonster.gameObject);
    int count1 = EnemySlime.Slimes.Count;
    while (--count1 > 0)
    {
      EnemySlime slime = EnemySlime.Slimes[count1];
      slime.ExplodeOnDeath = false;
      slime.health.DestroyNextFrame();
    }
    int count2 = EnemySpawner.EnemySpawners.Count;
    while (--count2 > 0)
      UnityEngine.Object.Destroy((UnityEngine.Object) EnemySpawner.EnemySpawners[count2]);
    enemySpiderMonster.ClearPaths();
    enemySpiderMonster.health.invincible = true;
    enemySpiderMonster.speed = 0.0f;
    enemySpiderMonster.simpleSpineAnimator.Animate("idle-boss", 0, false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 0.5)
      yield return (object) null;
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (enemySpiderMonster.juicedForm != GameManager.Layer2)
      enemySpiderMonster.simpleSpineAnimator.SetSkin("NoMask");
    enemySpiderMonster.simpleSpineAnimator.Animate("roar", 0, false);
    enemySpiderMonster.simpleSpineAnimator.AddAnimate("idle-boss", 0, true, 0.0f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 2f);
    enemySpiderMonster.DontPlayHurtAnimation = true;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 2.2999999523162842)
      yield return (object) null;
    enemySpiderMonster.Roared = true;
    enemySpiderMonster.DontPlayHurtAnimation = false;
    enemySpiderMonster.health.invincible = false;
    if (UnityEngine.Random.Range(0, 2) == 0)
      enemySpiderMonster.StartCoroutine(enemySpiderMonster.JumpAttack());
    else
      enemySpiderMonster.StartCoroutine(enemySpiderMonster.JumpAround());
    yield return (object) null;
  }

  public IEnumerator Die()
  {
    EnemySpiderMonster enemySpiderMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/death", enemySpiderMonster.gameObject);
    float time = 0.0f;
    enemySpiderMonster.ClearPaths();
    enemySpiderMonster.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
    GameManager.GetInstance().OnConversationNext(enemySpiderMonster.CameraTarget, 7f);
    enemySpiderMonster.simpleSpineAnimator.FlashWhite(false);
    enemySpiderMonster.state.CURRENT_STATE = StateMachine.State.Dieing;
    enemySpiderMonster.rb.velocity = (Vector2) Vector3.zero;
    enemySpiderMonster.rb.isKinematic = true;
    enemySpiderMonster.rb.simulated = false;
    enemySpiderMonster.rb.bodyType = RigidbodyType2D.Static;
    if ((double) enemySpiderMonster.transform.position.x > 11.0)
      enemySpiderMonster.transform.position = new Vector3(11f, enemySpiderMonster.transform.position.y, 0.0f);
    if ((double) enemySpiderMonster.transform.position.x < -11.0)
      enemySpiderMonster.transform.position = new Vector3(-11f, enemySpiderMonster.transform.position.y, 0.0f);
    if ((double) enemySpiderMonster.transform.position.y > 7.0)
      enemySpiderMonster.transform.position = new Vector3(enemySpiderMonster.transform.position.x, 7f, 0.0f);
    if ((double) enemySpiderMonster.transform.position.y < -7.0)
      enemySpiderMonster.transform.position = new Vector3(enemySpiderMonster.transform.position.x, -7f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    enemySpiderMonster.simpleSpineFlash.StopAllCoroutines();
    enemySpiderMonster.DisableForces = false;
    enemySpiderMonster.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    TrapPoison.RemoveAllPoison();
    enemySpiderMonster.KillAllSpawnedEnemies();
    yield return (object) new WaitForEndOfFrame();
    bool beatenLayer2 = DataManager.Instance.BeatenShamuraLayer2;
    bool isDeathWithHeart = !DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location) && !DungeonSandboxManager.Active && !DataManager.Instance.SurvivalModeActive;
    if (isDeathWithHeart)
    {
      enemySpiderMonster.simpleSpineAnimator.Animate("die", 0, false);
      enemySpiderMonster.simpleSpineAnimator.AddAnimate("dead", 0, true, 0.0f);
    }
    else
    {
      if (enemySpiderMonster.juicedForm && !DataManager.Instance.BeatenShamuraLayer2 && !DungeonSandboxManager.Active)
      {
        enemySpiderMonster.simpleSpineAnimator.Animate("die-follower", 0, false);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 1.7669999599456787)
          yield return (object) null;
        enemySpiderMonster.simpleSpineAnimator.gameObject.SetActive(false);
        PlayerReturnToBase.Disabled = true;
        GameObject Follower = UnityEngine.Object.Instantiate<GameObject>(enemySpiderMonster.followerToSpawn, enemySpiderMonster.transform.position, Quaternion.identity, enemySpiderMonster.transform.parent);
        Follower.GetComponent<Interaction_FollowerSpawn>().Play("CultLeader 4", ScriptLocalization.NAMES_CultLeaders.Dungeon4, cursedState: Thought.Dissenter);
        DataManager.SetFollowerSkinUnlocked("CultLeader 4");
        DataManager.Instance.BeatenShamuraLayer2 = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.NewGamePlus4);
        while ((UnityEngine.Object) Follower != (UnityEngine.Object) null)
          yield return (object) null;
        GameManager.GetInstance().OnConversationEnd();
        Interaction_Chest.Instance?.RevealBossReward(InventoryItem.ITEM_TYPE.GOD_TEAR);
        enemySpiderMonster.KillAllSpawnedEnemies();
        enemySpiderMonster.StopAllCoroutines();
        yield break;
      }
      enemySpiderMonster.simpleSpineAnimator.Animate("die-noheart", 0, false);
      enemySpiderMonster.simpleSpineAnimator.AddAnimate("dead-noheart", 0, true, 0.0f);
      if (!DungeonSandboxManager.Active)
        RoomLockController.RoomCompleted();
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 2.7000000476837158)
      yield return (object) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderMonster.simpleSpineAnimator.anim.timeScale) < 0.5)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().RemoveFromCamera(enemySpiderMonster.gameObject);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 11f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 13f;
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
      enemySpiderMonster.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith4;
    enemySpiderMonster.interaction_MonsterHeart.Play(beatenLayer2 || !GameManager.Layer2 ? InventoryItem.ITEM_TYPE.NONE : InventoryItem.ITEM_TYPE.GOD_TEAR);
    if (isDeathWithHeart)
      enemySpiderMonster.simpleSpineAnimator.Animate("dead", 0, false);
    else
      enemySpiderMonster.simpleSpineAnimator.Animate("dead-noheart", 0, true);
    enemySpiderMonster.KillAllSpawnedEnemies();
    enemySpiderMonster.enabled = false;
    enemySpiderMonster.StopAllCoroutines();
  }

  public void KillAllSpawnedEnemies()
  {
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && Health.team2[index].gameObject.activeInHierarchy)
      {
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        if ((UnityEngine.Object) Health.team2[index].GetComponent<SpawnEnemyOnDeath>() != (UnityEngine.Object) null)
          Health.team2[index].GetComponent<SpawnEnemyOnDeath>().SpawnEnemies = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy, dealDamageImmediately: true);
      }
    }
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public enum BombType
  {
    Web,
    Egg,
    Count,
  }
}
