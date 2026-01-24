// Decompiled with JetBrains decompiler
// Type: CritterBird
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CritterBird : UnitObject
{
  public const int AmountForRelic = 10;
  public float DangerDistance = 2f;
  public CircleCollider2D CircleCollider;
  public Animator animator;
  public float Timer;
  public float TargetAngle;
  public float vz;
  public GameObject Shadow;
  [SerializeField]
  public SkeletonAnimation bird;

  public void Start()
  {
  }

  public new void OnEnable()
  {
    this.state = this.GetComponent<StateMachine>();
    if ((UnityEngine.Object) this.CircleCollider == (UnityEngine.Object) null)
      this.CircleCollider = this.GetComponent<CircleCollider2D>();
    this.Timer = (float) UnityEngine.Random.Range(2, 4);
    if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, 1f, 1f);
    this.DangerDistance = 2.5f;
    this.GetComponent<Health>().OnDie += new Health.DieAction(((UnitObject) this).OnDie);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.GetComponent<Health>().OnDie -= new Health.DieAction(((UnitObject) this).OnDie);
  }

  public void SetFleeing()
  {
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
      this.state = this.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.Fleeing;
  }

  public override void Update()
  {
    if (PlayerRelic.TimeFrozen)
      return;
    base.Update();
    if ((UnityEngine.Object) this.bird == (UnityEngine.Object) null || this.bird.state == null)
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.LookForDanger();
        if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.Timer = UnityEngine.Random.Range(0.5f, 2f);
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
        this.bird.state.SetAnimation(0, "EAT", true);
        break;
      case StateMachine.State.Fleeing:
        if ((double) Time.deltaTime <= 0.0)
          break;
        this.vx = 3f * Mathf.Cos(this.TargetAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vy = 3f * Mathf.Sin(this.TargetAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
        this.vz -= 0.07f * Time.deltaTime;
        this.transform.position = this.transform.position + new Vector3(this.vx, this.vy, this.vz);
        if (!((UnityEngine.Object) this.Shadow != (UnityEngine.Object) null))
          break;
        this.Shadow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
        this.Shadow.transform.localScale -= new Vector3(1f / 500f, 1f / 500f, 0.0f);
        if ((double) this.Shadow.transform.localScale.x > 0.0)
          break;
        this.gameObject.Recycle();
        break;
      case StateMachine.State.CustomAction0:
        this.LookForDanger();
        if ((double) (this.Timer -= Time.deltaTime) >= 0.0)
          break;
        this.Timer = (float) UnityEngine.Random.Range(2, 4);
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.bird.state.SetAnimation(0, "IDLE", true);
        if ((double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5)
          break;
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, 1f, 1f);
        break;
    }
  }

  public void LookForDanger()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DangerDistance && allUnit.team != Health.Team.Neutral && !allUnit.untouchable)
      {
        this.TargetAngle = Utils.GetAngle(allUnit.transform.position, this.transform.position);
        this.bird.state.SetAnimation(0, "FLY", true);
        this.state.CURRENT_STATE = StateMachine.State.Fleeing;
        AudioManager.Instance.PlayOneShot("event:/birds/bird_fly_away", this.gameObject);
        this.transform.localScale = new Vector3((double) this.TargetAngle >= 90.0 || (double) this.TargetAngle <= -90.0 ? 1f : -1f, 1f, 1f);
        this.StartCoroutine((IEnumerator) this.DisableCollider());
      }
    }
  }

  public IEnumerator DisableCollider()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CircleCollider.enabled = false;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.DangerDistance, Color.white);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    ++DataManager.Instance.TotalBirdsCaught;
    if (DataManager.Instance.TotalBirdsCaught < 10 || !DataManager.Instance.OnboardedRelics || DataManager.Instance.PlayerFoundRelics.Contains(RelicType.RandomEnemyIntoCritter) || Health.team2.Count > 0 || RelicCustomTarget.IsRelicPickUpActive(RelicType.RandomEnemyIntoCritter))
      return;
    GameObject gameObject = RelicCustomTarget.Create(this.transform.position, this.transform.position - Vector3.forward, 1.5f, RelicType.RandomEnemyIntoCritter, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
    BiomeConstants.Instance.EmitSmokeInteractionVFX(gameObject.transform.position, (Vector3) Vector2.one);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(gameObject.gameObject, 6f);
  }
}
