// Decompiled with JetBrains decompiler
// Type: GuardianPet_GroundSlam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GuardianPet_GroundSlam : GuardianPet
{
  public GameObject PetGameObject;
  public float acceleration = 2f;
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float timestamp;
  public float IdleSpeed = 0.03f;
  public float ChaseSpeed = 0.1f;
  public float WhirlWindSpeed = 0.1f;
  public float MaximumRange = 5f;
  public float AttackAnticipation = 0.5f;
  public float AttackDuration = 0.5f;
  public float vulnerableTime = 2f;
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/attack";
  public string GroundSlamStartSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_pet_slam_start";
  public string GroundSlamReturnSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_pet_slam_return";
  public EventInstance groundSlamStartInstanceSFX;
  [SerializeField]
  public bool useAcceleration;
  [SerializeField]
  public SpriteRenderer indicator;
  public float Angle;
  public bool ChasingPlayer;
  public bool avoidTarget;
  public float noticePlayerDistance = 5f;
  public bool NoticedPlayer;
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  public float AttackCoolDown;
  public Vector2 AttackCoolDownDuration = new Vector2(1f, 2f);
  public int RanDirection = 1;
  public int NumberOfAttacks = 3;
  public float time;
  public float flashTickTimer;
  public Color indicatorColor = Color.white;
  [SerializeField]
  public DamageCollider collider;
  public static float lastSlamTime;
  public int KnockBackForce = 750;
  public int AttackCycle;
  public List<GuardianPetWhirlwindChild> WhirlWindChildren = new List<GuardianPetWhirlwindChild>();
  public float WhirlWindDistance = 1f;
  [SerializeField]
  public GameObject ChildContainer;
  [SerializeField]
  public ProjectileCircle projectilePatternRings;
  [SerializeField]
  public float projectilePatternRingsSpeed = 2.5f;
  [SerializeField]
  public float projectilePatternRingsRadius = 1f;
  [SerializeField]
  public float projectilePatternRingsAcceleration = 7.5f;
  public int ShotCount;

  public override void Play()
  {
    base.Play();
    Debug.Log((object) "PLAY!".Colour(Color.yellow));
    this.timestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.PetGameObject = this.gameObject;
    this.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.RanDirection = (double) UnityEngine.Random.value < 0.5 ? -1 : 1;
    if ((UnityEngine.Object) this.health == (UnityEngine.Object) null)
      this.health = this.GetComponent<Health>();
    this.health.HP = this.health.totalHP;
    this.health.enabled = true;
    this.health.invincible = true;
    this.transform.DOMoveZ(-3f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.ShotCount = 0;
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    int index = -1;
    while (++index < this.WhirlWindChildren.Count)
      this.WhirlWindChildren[index].gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
    this.indicator.transform.parent = this.transform;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.indicator?.gameObject.SetActive(false);
    this.collider?.gameObject.SetActive(false);
  }

  public override void Update()
  {
    base.Update();
    if ((double) Time.timeScale == 0.0)
      return;
    if (this.indicator.gameObject.activeSelf && (double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicator.material.SetColor("_Color", this.indicatorColor);
      this.flashTickTimer = 0.0f;
    }
    this.flashTickTimer += Time.deltaTime;
  }

  public IEnumerator KnockBackRoutine(float angle, System.Action Callback)
  {
    GuardianPet_GroundSlam guardianPetGroundSlam = this;
    guardianPetGroundSlam.DisableForces = true;
    Vector3 force = (Vector3) new Vector2((float) guardianPetGroundSlam.KnockBackForce * Mathf.Cos(angle), (float) guardianPetGroundSlam.KnockBackForce * Mathf.Sin(angle));
    guardianPetGroundSlam.rb.AddForce((Vector2) force);
    yield return (object) new WaitForSeconds(0.5f);
    guardianPetGroundSlam.DisableForces = false;
    guardianPetGroundSlam.rb.velocity = Vector2.zero;
    guardianPetGroundSlam.AttackCoolDown = 0.0f;
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public void ReturnToController()
  {
    Debug.Log((object) "ReturnToController!".Colour(Color.yellow));
    this.Return();
    this.StopAllCoroutines();
  }

  public Health GetClosestTarget() => this.GetClosestTarget(true);

  public virtual IEnumerator ActiveRoutine()
  {
    GuardianPet_GroundSlam guardianPetGroundSlam = this;
    guardianPetGroundSlam.maxSpeed = guardianPetGroundSlam.ChaseSpeed;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      float turningSpeed = guardianPetGroundSlam.turningSpeed;
      if ((UnityEngine.Object) guardianPetGroundSlam.state != (UnityEngine.Object) null && (UnityEngine.Object) guardianPetGroundSlam.GetClosestTarget() != (UnityEngine.Object) null)
        guardianPetGroundSlam.state.LookAngle = Utils.GetAngle(guardianPetGroundSlam.transform.position, guardianPetGroundSlam.GetClosestTarget().transform.position);
      else
        guardianPetGroundSlam.state.LookAngle = Utils.GetAngle(guardianPetGroundSlam.transform.position, (Vector3) (UnityEngine.Random.insideUnitCircle * 5f));
      guardianPetGroundSlam.TargetPosition = guardianPetGroundSlam.GetClosestTarget().transform.position;
      guardianPetGroundSlam.TargetPosition = new Vector3(guardianPetGroundSlam.TargetPosition.x, guardianPetGroundSlam.TargetPosition.y, -3f);
      guardianPetGroundSlam.Angle = Mathf.LerpAngle(guardianPetGroundSlam.Angle, Utils.GetAngle(guardianPetGroundSlam.transform.position, guardianPetGroundSlam.TargetPosition), Time.deltaTime * turningSpeed);
      if ((double) Vector3.Distance(guardianPetGroundSlam.transform.position, guardianPetGroundSlam.TargetPosition) >= 1.0 || (double) Time.time <= (double) GuardianPet_GroundSlam.lastSlamTime || PlayerRelic.TimeFrozen)
      {
        if (!guardianPetGroundSlam.useAcceleration)
          guardianPetGroundSlam.speed = guardianPetGroundSlam.maxSpeed * guardianPetGroundSlam.SpeedMultiplier;
        guardianPetGroundSlam.state.facingAngle = guardianPetGroundSlam.Angle;
        yield return (object) null;
      }
      else
        break;
    }
    guardianPetGroundSlam.GroundSmash();
  }

  public virtual IEnumerator FleeRoutine()
  {
    yield return (object) null;
  }

  public virtual bool ShouldStartCharging() => false;

  public virtual IEnumerator ChargingRoutine()
  {
    yield return (object) null;
  }

  public virtual bool ShouldAttack() => false;

  public void WhirlWindAttack()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.WhirlWindAttackRoutine());
  }

  public IEnumerator WhirlWindAttackRoutine()
  {
    GuardianPet_GroundSlam Pet = this;
    Pet.state.CURRENT_STATE = StateMachine.State.Attacking;
    Pet.speed = 0.0f;
    yield return (object) null;
    int num1 = -1;
    while (++num1 < Pet.WhirlWindChildren.Count)
      Pet.WhirlWindChildren[num1].Play((GuardianPet) Pet, num1, Pet.WhirlWindChildren.Count);
    yield return (object) new WaitForSeconds(0.5f);
    Pet.state.CURRENT_STATE = StateMachine.State.Moving;
    float Duration = 0.0f;
    while ((double) (Duration += Time.deltaTime) < 4.0)
    {
      Pet.speed = Mathf.Lerp(0.0f, Pet.WhirlWindSpeed, Duration / 1f);
      Pet.state.LookAngle = Utils.GetAngle(Pet.transform.position, Pet.GetClosestTarget().transform.position);
      Pet.state.facingAngle = Utils.GetAngle(Pet.transform.position, Pet.GetClosestTarget().transform.position);
      yield return (object) null;
    }
    int index = -1;
    while (++index < Pet.WhirlWindChildren.Count)
      Pet.WhirlWindChildren[index].Close();
    DOTween.To(new DOGetter<float>(Pet.\u003CWhirlWindAttackRoutine\u003Eb__51_0), new DOSetter<float>(Pet.\u003CWhirlWindAttackRoutine\u003Eb__51_1), 0.0f, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    Pet.state.CURRENT_STATE = StateMachine.State.Idle;
    Pet.AttackCoolDown = UnityEngine.Random.Range(Pet.AttackCoolDownDuration.x, Pet.AttackCoolDownDuration.y);
    GuardianPet_GroundSlam guardianPetGroundSlam = Pet;
    int num2 = Pet.ShotCount + 1;
    int num3 = num2;
    guardianPetGroundSlam.ShotCount = num3;
    if (num2 >= Pet.NumberOfAttacks && !Pet.HostHasDied)
      Pet.ReturnToController();
    else
      Pet.StartCoroutine((IEnumerator) Pet.ActiveRoutine());
  }

  public void GroundSmash()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.GroundSmashRoutine());
  }

  public override void OnDestroy()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.groundSlamStartInstanceSFX, STOP_MODE.ALLOWFADEOUT);
    base.OnDestroy();
  }

  public IEnumerator GroundSmashRoutine()
  {
    GuardianPet_GroundSlam guardianPetGroundSlam = this;
    GuardianPet_GroundSlam.lastSlamTime = Time.time + 2f;
    guardianPetGroundSlam.state.CURRENT_STATE = StateMachine.State.Attacking;
    yield return (object) null;
    if (!string.IsNullOrEmpty(guardianPetGroundSlam.GroundSlamStartSFX))
      guardianPetGroundSlam.groundSlamStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(guardianPetGroundSlam.GroundSlamStartSFX, guardianPetGroundSlam.transform);
    Vector3 p = guardianPetGroundSlam.GetClosestTarget().transform.position with
    {
      z = -4f
    };
    guardianPetGroundSlam.transform.DOKill();
    guardianPetGroundSlam.transform.DOMove(p, guardianPetGroundSlam.AttackAnticipation).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    p.z = 0.0f;
    guardianPetGroundSlam.indicator.transform.parent = (Transform) null;
    guardianPetGroundSlam.indicator.transform.position = p;
    guardianPetGroundSlam.indicator.gameObject.SetActive(true);
    guardianPetGroundSlam.spine.AnimationState.SetAnimation(0, "attack-melee", false);
    guardianPetGroundSlam.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < (double) guardianPetGroundSlam.AttackAnticipation)
      yield return (object) null;
    guardianPetGroundSlam.transform.DOMove(p, guardianPetGroundSlam.AttackDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < (double) guardianPetGroundSlam.AttackDuration)
      yield return (object) null;
    guardianPetGroundSlam.collider.gameObject.SetActive(true);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    BiomeConstants.Instance.EmitHammerEffectsInstantiated(new Vector3(guardianPetGroundSlam.transform.position.x, guardianPetGroundSlam.transform.position.y, 0.0f));
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    guardianPetGroundSlam.health.invincible = false;
    guardianPetGroundSlam.indicator.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.1f);
    guardianPetGroundSlam.collider.gameObject.SetActive(false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < (double) guardianPetGroundSlam.vulnerableTime)
      yield return (object) null;
    guardianPetGroundSlam.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianPetGroundSlam.AttackCoolDown = UnityEngine.Random.Range(guardianPetGroundSlam.AttackCoolDownDuration.x, guardianPetGroundSlam.AttackCoolDownDuration.y);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < 0.20000000298023224)
      yield return (object) null;
    guardianPetGroundSlam.spine.AnimationState.SetAnimation(0, "pop", false);
    guardianPetGroundSlam.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    guardianPetGroundSlam.health.invincible = true;
    guardianPetGroundSlam.transform.DOMoveZ(-3f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    guardianPetGroundSlam.DisableForces = false;
    if (!string.IsNullOrEmpty(guardianPetGroundSlam.GroundSlamReturnSFX))
      AudioManager.Instance.PlayOneShot(guardianPetGroundSlam.GroundSlamReturnSFX, guardianPetGroundSlam.transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < 0.5)
      yield return (object) null;
    guardianPetGroundSlam.StartCoroutine((IEnumerator) guardianPetGroundSlam.ActiveRoutine());
  }

  public void ProjectileRings()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ProjectileRingsRoutine());
  }

  public IEnumerator ProjectileRingsRoutine()
  {
    GuardianPet_GroundSlam guardianPetGroundSlam1 = this;
    guardianPetGroundSlam1.state.CURRENT_STATE = StateMachine.State.Attacking;
    yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/attack", guardianPetGroundSlam1.transform.position);
    Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(guardianPetGroundSlam1.projectilePatternRings, guardianPetGroundSlam1.transform.parent).GetComponent<Projectile>();
    arrow.transform.position = new Vector3(guardianPetGroundSlam1.transform.position.x, guardianPetGroundSlam1.transform.position.y, -0.5f);
    arrow.health = guardianPetGroundSlam1.health;
    arrow.team = guardianPetGroundSlam1.health.team;
    arrow.Speed = guardianPetGroundSlam1.projectilePatternRingsSpeed;
    arrow.Acceleration = guardianPetGroundSlam1.projectilePatternRingsAcceleration;
    arrow.Owner = guardianPetGroundSlam1.health;
    arrow.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, guardianPetGroundSlam1.projectilePatternRingsRadius, 0.0f, (System.Action) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
      if ((UnityEngine.Object) this.PetGameObject != (UnityEngine.Object) null)
      {
        AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
        arrow.Angle = Mathf.Round(arrow.Angle / 45f) * 45f;
      }
      else
        arrow.DestroyProjectile();
    }));
    yield return (object) new WaitForSeconds(1f);
    guardianPetGroundSlam1.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianPetGroundSlam1.AttackCoolDown = UnityEngine.Random.Range(guardianPetGroundSlam1.AttackCoolDownDuration.x, guardianPetGroundSlam1.AttackCoolDownDuration.y);
    GuardianPet_GroundSlam guardianPetGroundSlam2 = guardianPetGroundSlam1;
    int num1 = guardianPetGroundSlam1.ShotCount + 1;
    int num2 = num1;
    guardianPetGroundSlam2.ShotCount = num2;
    if (num1 >= guardianPetGroundSlam1.NumberOfAttacks && !guardianPetGroundSlam1.HostHasDied)
      guardianPetGroundSlam1.ReturnToController();
    else
      guardianPetGroundSlam1.StartCoroutine((IEnumerator) guardianPetGroundSlam1.ActiveRoutine());
  }

  [CompilerGenerated]
  public float \u003CWhirlWindAttackRoutine\u003Eb__51_0() => this.speed;

  [CompilerGenerated]
  public void \u003CWhirlWindAttackRoutine\u003Eb__51_1(float x) => this.speed = x;
}
