// Decompiled with JetBrains decompiler
// Type: Cower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Cower : BaseMonoBehaviour
{
  public bool CanCower = true;
  private StateMachine state;
  private Health health;
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
  private UnitObject unitObject;
  private bool ShieldDontCower;
  private bool CoweringActivated;
  private GameObject Player;
  private ShowHPBar ShowHPBar;
  private Rigidbody2D rb2d;
  private SpawnDeadBodyOnDeath SpawnDeadBodyOnDeath;
  private int StartingEnemies;
  public bool KnockBackOnMelee;
  private Coroutine cStaggeredRoutine;
  private bool Staggered;
  private float Speed = 1500f;

  private void OnEnable()
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
      this.SpawnDeadBodyOnDeath.enabled = false;
    if (!this.CoweringActivated)
      return;
    this.StartCoroutine((IEnumerator) this.CowerRoutine());
  }

  private void Health_OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!this.Staggered || (double) this.health.HP <= 0.0)
      return;
    this.EndStaggered();
  }

  private void Health_OnDie(
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

  private void Start()
  {
    this.ShieldDontCower = this.health.HasShield;
    this.StartingEnemies = Health.team2.Count;
  }

  private void OnDisable()
  {
    this.health.OnHitEarly -= new Health.HitAction(this.Health_OnHitEarly);
    this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
  }

  private void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
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

  private IEnumerator StaggerRoutine(GameObject Attacker)
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
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
      yield return (object) null;
    cower.Spine.AnimationState.SetAnimation(0, cower.StaggerEnd, false);
    yield return (object) new WaitForSeconds(0.6333333f);
    cower.EndStaggered();
  }

  private void EndStaggered()
  {
    if (this.cStaggeredRoutine != null)
      this.StopCoroutine(this.cStaggeredRoutine);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Staggered = false;
    this.AIScriptToDisable.enabled = true;
  }

  private IEnumerator KnockbackRoutine(GameObject Attacker, Vector3 AttackLocation)
  {
    Cower cower = this;
    cower.AIScriptToDisable.enabled = false;
    cower.Spine.AnimationState.SetAnimation(0, cower.KnockBackStart, false);
    cower.Spine.AnimationState.AddAnimation(0, cower.KnockBackLoop, true, 0.0f);
    cower.Speed = 1500f;
    cower.rb2d.angularDrag = 10f;
    cower.rb2d.drag = 10f;
    float f = Utils.GetAngle(Attacker.transform.position, cower.transform.position) * ((float) Math.PI / 180f);
    Vector2 force = new Vector2(cower.Speed * Mathf.Cos(f), cower.Speed * Mathf.Sin(f));
    cower.rb2d.AddForce(force);
    while ((double) cower.rb2d.velocity.magnitude > 0.10000000149011612)
      yield return (object) null;
    if ((double) cower.health.HP <= 0.0)
    {
      yield return (object) new WaitForSeconds(0.5f);
      if ((UnityEngine.Object) cower.SpawnDeadBodyOnDeath != (UnityEngine.Object) null)
        cower.SpawnDeadBodyOnDeath.ReEnable(800f);
      cower.health.enabled = true;
      cower.health.OnHit -= new Health.HitAction(cower.Health_OnHit);
      cower.health.OnDie -= new Health.DieAction(cower.Health_OnDie);
      cower.health.DestroyOnDeath = true;
      cower.health.DealDamage(1f, Attacker, AttackLocation);
    }
    else
    {
      yield return (object) new WaitForSeconds(1f);
      cower.Spine.AnimationState.SetAnimation(0, cower.KnockBackEnd, false);
      yield return (object) new WaitForSeconds(0.933333337f);
      if ((double) cower.health.HP > 0.0)
      {
        cower.state.CURRENT_STATE = StateMachine.State.Idle;
        cower.AIScriptToDisable.enabled = true;
        cower.health.enabled = true;
      }
    }
  }

  private IEnumerator CowerRoutine()
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
}
