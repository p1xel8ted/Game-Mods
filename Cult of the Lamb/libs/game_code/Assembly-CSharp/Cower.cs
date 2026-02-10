// Decompiled with JetBrains decompiler
// Type: Cower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Cower : BaseMonoBehaviour
{
  public bool CanCower = true;
  public StateMachine state;
  public Health health;
  public UnitObject AIScriptToDisable;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string KnockBackStart;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string KnockBackLoop;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string KnockBackEnd;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaggerStart;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaggerLoop;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaggerEnd;
  public string fallDownSFX = "";
  public string getUpSFX = "";
  public string staggerVO = "";
  public UnitObject unitObject;
  public bool ShieldDontCower;
  public bool CoweringActivated;
  public GameObject Player;
  public ShowHPBar ShowHPBar;
  public Rigidbody2D rb2d;
  public SpawnDeadBodyOnDeath SpawnDeadBodyOnDeath;
  public int HitsNeededToEndStagger = 1;
  public bool HeavyAttackEndsStagger;
  public int CurrentStaggerHits;
  public int StartingEnemies;
  public bool KnockBackOnMelee;
  public bool EnableAIScriptOnKnocbackDeath;
  public bool destroyOnDeath = true;
  public System.Action OnFinishDeath;
  public System.Action OnFinishKnockback;
  public bool preventStandardStagger;
  public Coroutine cStaggeredRoutine;
  public bool Staggered;
  public UnityEvent StaggerBegun;
  public UnityEvent StaggerEnded;
  public float Speed = 1500f;

  public void OnEnable()
  {
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
    this.health.OnHitEarly += new Health.HitAction(this.Health_OnHitEarly);
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
    this.ShowHPBar = this.GetComponent<ShowHPBar>();
    this.unitObject = this.GetComponent<UnitObject>();
    this.rb2d = this.GetComponent<Rigidbody2D>();
    this.SpawnDeadBodyOnDeath = this.GetComponent<SpawnDeadBodyOnDeath>();
    if ((UnityEngine.Object) this.SpawnDeadBodyOnDeath != (UnityEngine.Object) null)
    {
      this.SpawnDeadBodyOnDeath.deadBodySliding.CreatePool<DeadBodySliding>(3);
      this.SpawnDeadBodyOnDeath.enabled = false;
    }
    if (this.CoweringActivated)
      this.StartCoroutine((IEnumerator) this.CowerRoutine());
    System.Action onFinishDeath = this.OnFinishDeath;
    if (onFinishDeath != null)
      onFinishDeath();
    System.Action onFinishKnockback = this.OnFinishKnockback;
    if (onFinishKnockback == null)
      return;
    onFinishKnockback();
  }

  public void Health_OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!this.Staggered)
      return;
    ++this.CurrentStaggerHits;
    if (this.CurrentStaggerHits >= this.HitsNeededToEndStagger || this.HeavyAttackEndsStagger && AttackType == Health.AttackTypes.Heavy || (double) this.health.HP <= 0.0)
    {
      this.EndStaggered();
    }
    else
    {
      if (this.CurrentStaggerHits >= this.HitsNeededToEndStagger)
        return;
      CameraManager.shakeCamera(2f);
    }
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    switch (AttackType)
    {
      case Health.AttackTypes.Heavy:
        this.health.DestroyOnDeath = false;
        this.StartCoroutine((IEnumerator) this.KnockbackRoutine(Attacker, AttackLocation));
        break;
      case Health.AttackTypes.Projectile:
        this.SpawnDeadBodyOnDeath?.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
        break;
      default:
        if (!this.KnockBackOnMelee || AttackType != Health.AttackTypes.Melee)
          goto case Health.AttackTypes.Projectile;
        goto case Health.AttackTypes.Heavy;
    }
  }

  public void Start()
  {
    this.ShieldDontCower = this.health.HasShield;
    this.StartingEnemies = Health.team2.Count;
    this.destroyOnDeath = this.health.DestroyOnDeath;
  }

  public void OnDisable()
  {
    this.health.OnHitEarly -= new Health.HitAction(this.Health_OnHitEarly);
    this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
  }

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.preventStandardStagger)
      return;
    this.Player = Attacker;
    if (!this.health.WasJustParried)
    {
      switch (AttackType)
      {
        case Health.AttackTypes.Heavy:
          this.StartCoroutine((IEnumerator) this.KnockbackRoutine(Attacker, AttackLocation));
          return;
        case Health.AttackTypes.Projectile:
          break;
        default:
          if (!this.KnockBackOnMelee || AttackType != Health.AttackTypes.Melee)
            return;
          goto case Health.AttackTypes.Heavy;
      }
    }
    if (this.cStaggeredRoutine != null)
      this.StopCoroutine(this.cStaggeredRoutine);
    this.cStaggeredRoutine = this.StartCoroutine((IEnumerator) this.StaggerRoutine(Attacker));
  }

  public IEnumerator StaggerRoutine(GameObject Attacker)
  {
    Cower cower = this;
    cower.Staggered = true;
    cower.Speed = 1000f;
    cower.rb2d.angularDrag = 10f;
    cower.rb2d.drag = 10f;
    float f = Utils.GetAngle(Attacker.transform.position, cower.transform.position) * ((float) Math.PI / 180f);
    Vector2 force = new Vector2(cower.Speed * Mathf.Cos(f), cower.Speed * Mathf.Sin(f));
    cower.rb2d.AddForce(force);
    cower.Spine.AnimationState.SetAnimation(0, cower.StaggerStart, false);
    cower.Spine.AnimationState.AddAnimation(0, cower.StaggerLoop, true, 0.0f);
    cower.AIScriptToDisable.enabled = false;
    if (!string.IsNullOrEmpty(cower.fallDownSFX))
      AudioManager.Instance.PlayOneShot(cower.fallDownSFX, cower.transform.position);
    if (!string.IsNullOrEmpty(cower.staggerVO))
      AudioManager.Instance.PlayOneShot(cower.staggerVO, cower.transform.position);
    cower.StaggerBegun.Invoke();
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
      yield return (object) null;
    cower.Spine.AnimationState.SetAnimation(0, cower.StaggerEnd, false);
    if (!string.IsNullOrEmpty(cower.getUpSFX))
      AudioManager.Instance.PlayOneShot(cower.getUpSFX, cower.transform.position);
    yield return (object) new WaitForSeconds(0.6333333f);
    cower.EndStaggered();
  }

  public void EndStaggered()
  {
    this.CurrentStaggerHits = 0;
    if (this.cStaggeredRoutine != null)
      this.StopCoroutine(this.cStaggeredRoutine);
    this.StaggerEnded.Invoke();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Staggered = false;
    this.AIScriptToDisable.enabled = true;
  }

  public IEnumerator KnockbackRoutine(GameObject Attacker, Vector3 AttackLocation)
  {
    Cower cower = this;
    cower.OnFinishDeath = (System.Action) (() => this.FinishDeath(Attacker, AttackLocation));
    cower.OnFinishKnockback = (System.Action) (() => this.FinishKnockback());
    cower.AIScriptToDisable.enabled = false;
    cower.Spine.AnimationState.SetAnimation(0, cower.KnockBackStart, false);
    cower.Spine.AnimationState.AddAnimation(0, cower.KnockBackLoop, true, 0.0f);
    while (PlayerRelic.TimeFrozen)
      yield return (object) null;
    cower.Speed = 1500f;
    cower.rb2d.angularDrag = 10f;
    cower.rb2d.drag = 10f;
    float f = Utils.GetAngle(Attacker.transform.position, cower.transform.position) * ((float) Math.PI / 180f);
    Vector2 force = new Vector2(cower.Speed * Mathf.Cos(f), cower.Speed * Mathf.Sin(f));
    cower.rb2d.AddForce(force);
    float t = 0.0f;
    float knockBackOnDeathTime = 0.6f;
    while ((double) cower.rb2d.velocity.magnitude > 0.10000000149011612)
    {
      t += Time.deltaTime * cower.Spine.timeScale;
      if ((double) t < (double) knockBackOnDeathTime)
        yield return (object) null;
      else
        break;
    }
    if ((double) cower.health.HP <= 0.0)
    {
      yield return (object) new WaitForSeconds(0.5f);
      cower.OnFinishDeath();
    }
    else
    {
      yield return (object) new WaitForSeconds(1f);
      cower.Spine.AnimationState.SetAnimation(0, cower.KnockBackEnd, false);
      yield return (object) new WaitForSeconds(0.933333337f);
      System.Action onFinishKnockback = cower.OnFinishKnockback;
      if (onFinishKnockback != null)
        onFinishKnockback();
    }
  }

  public IEnumerator CowerRoutine()
  {
    Cower cower = this;
    if (Health.team2.Count <= 1 && cower.StartingEnemies > 1)
      cower.health.SlowMoOnkill = true;
    cower.health.HP = 1f;
    cower.AIScriptToDisable.enabled = false;
    cower.Spine.AnimationState.SetAnimation(0, "scared", false);
    cower.Spine.AnimationState.AddAnimation(0, "scared-loop", true, 0.0f);
    cower.health.invincible = false;
    while (true)
    {
      cower.state.facingAngle = Utils.GetAngle(cower.transform.position, cower.Player.transform.position);
      cower.Spine.skeleton.ScaleX = (double) cower.state.facingAngle >= 90.0 || (double) cower.state.facingAngle <= -90.0 ? 1f : -1f;
      yield return (object) null;
    }
  }

  public void FinishDeath(GameObject Attacker, Vector3 AttackLocation)
  {
    if ((double) this.health.HP <= 0.0)
    {
      this.AIScriptToDisable.enabled = this.EnableAIScriptOnKnocbackDeath;
      if ((UnityEngine.Object) this.SpawnDeadBodyOnDeath != (UnityEngine.Object) null)
        this.SpawnDeadBodyOnDeath.ReEnable(800f);
      this.health.enabled = true;
      this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
      this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
      this.health.DestroyOnDeath = this.destroyOnDeath;
      this.health.invincible = false;
      this.health.DealDamage(1f, Attacker, AttackLocation);
    }
    this.OnFinishDeath = (System.Action) null;
  }

  public void FinishKnockback()
  {
    if ((double) this.health.HP > 0.0)
    {
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.AIScriptToDisable.enabled = true;
      this.health.enabled = true;
    }
    else
      this.AIScriptToDisable.enabled = this.EnableAIScriptOnKnocbackDeath;
    this.OnFinishKnockback = (System.Action) null;
  }
}
