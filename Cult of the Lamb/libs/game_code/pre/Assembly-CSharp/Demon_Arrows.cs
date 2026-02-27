// Decompiled with JetBrains decompiler
// Type: Demon_Arrows
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Demon_Arrows : Demon
{
  public static List<GameObject> Demons = new List<GameObject>();
  public SimpleSpineAnimator simpleSpineAnimator;
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
  private GameObject _Master;
  private float AttackDelay;
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
    Demon_Arrows demonArrows = this;
    while (demonArrows.spine.AnimationState == null)
      yield return (object) null;
    if (demonArrows.Level > 1)
    {
      demonArrows.spine.skeleton.SetSkin("Arrows+");
      demonArrows.spine.skeleton.SetSlotsToSetupPose();
      demonArrows.spine.AnimationState.Apply(demonArrows.spine.skeleton);
    }
  }

  private void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    HealthPlayer.OnPlayerDied -= new HealthPlayer.HPUpdated(this.Health_OnDie);
  }

  private void Health_OnDie(HealthPlayer player) => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void BiomeGenerator_OnBiomeChangeRoom()
  {
    this.transform.position = this.Master.transform.position + Vector3.right;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.Master == (UnityEngine.Object) null || (double) GameManager.DeltaTime == 0.0 || MMConversation.isPlaying)
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Speed += (float) ((0.0 - (double) this.Speed) / (7.0 * (double) GameManager.DeltaTime));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Master.transform.position) > 1.5)
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
