// Decompiled with JetBrains decompiler
// Type: TrapGrabber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapGrabber : BaseMonoBehaviour
{
  public GameObject Teeth;
  public Transform SortingGroup;
  public Transform CachePlayerParent;
  public int HP = 6;
  public TrapGrabber.State _CurrentState;
  public BoxCollider2D boxCollider2D;
  public List<Collider2D> colliders;
  public ContactFilter2D contactFilter2D;
  public SkeletonAnimation Spine;
  public PlayerFarming playerFarming;
  public float Angle;
  public float Speed;
  public Vector3 Position;
  public Coroutine ReturnToCenter;
  public Health EnemyHealth;

  public TrapGrabber.State CurrentState
  {
    get => this._CurrentState;
    set
    {
      if (this._CurrentState != value)
      {
        switch (this._CurrentState)
        {
          case TrapGrabber.State.Idle:
            this.ReturnToCenter = this.StartCoroutine((IEnumerator) this.DoReturnToCenter());
            this.Spine.AnimationState.SetAnimation(0, "hidden", true);
            break;
          case TrapGrabber.State.Grabbing:
            if (this.ReturnToCenter != null)
              this.StopCoroutine((IEnumerator) this.DoReturnToCenter());
            this.Spine.AnimationState.SetAnimation(0, "grab", false);
            this.Spine.AnimationState.AddAnimation(0, "grabbed", true, 0.0f);
            this.playerFarming.simpleSpineAnimator.Animate("grabber-grab", 0, false);
            this.playerFarming.simpleSpineAnimator.AddAnimate("grabber-grabbed", 0, false, 0.0f);
            break;
          case TrapGrabber.State.Hit:
            this.Spine.AnimationState.SetAnimation(0, "hit", false);
            this.Spine.AnimationState.AddAnimation(0, "grabbed", true, 0.0f);
            this.playerFarming.simpleSpineAnimator.Animate("grabber-hit", 0, false);
            this.playerFarming.simpleSpineAnimator.AddAnimate("grabber-grabbed", 0, false, 0.0f);
            break;
          case TrapGrabber.State.Die:
            this.Spine.AnimationState.SetAnimation(0, "kill", false);
            this.playerFarming.simpleSpineAnimator.Animate("grabber-kill", 0, false);
            break;
        }
      }
      this._CurrentState = value;
    }
  }

  public void Start()
  {
    this.boxCollider2D = this.GetComponent<BoxCollider2D>();
    this.contactFilter2D = new ContactFilter2D();
    this.contactFilter2D.NoFilter();
    this.CurrentState = TrapGrabber.State.Idle;
  }

  public IEnumerator DoReturnToCenter()
  {
    yield return (object) new WaitForSeconds(1f);
    float Distance = Vector3.Distance(this.Teeth.transform.localPosition, Vector3.zero);
    float Speed = 5f;
    Vector3 Start = this.Teeth.transform.localPosition;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < (double) Distance / (double) Speed)
    {
      this.Teeth.transform.localPosition = Vector3.Lerp(Start, Vector3.zero, Timer / (Distance / Speed));
      yield return (object) null;
    }
  }

  public IEnumerator DoAttack()
  {
    TrapGrabber trapGrabber = this;
    trapGrabber.CurrentState = TrapGrabber.State.Warning;
    GameObject player = PlayerFarming.FindClosestPlayerGameObject(trapGrabber.transform.position);
    trapGrabber.Speed = 0.0f;
    while ((double) Vector3.Distance(trapGrabber.Teeth.transform.position, player.transform.position) > (double) trapGrabber.Speed * (double) Time.deltaTime)
    {
      if ((double) trapGrabber.Speed < 5.0)
        ++trapGrabber.Speed;
      trapGrabber.Angle = Utils.GetAngle(trapGrabber.Teeth.transform.position, player.transform.position) * ((float) Math.PI / 180f);
      trapGrabber.Position = new Vector3(trapGrabber.Speed * Time.deltaTime * Mathf.Cos(trapGrabber.Angle), trapGrabber.Speed * Time.deltaTime * Mathf.Sin(trapGrabber.Angle));
      trapGrabber.Teeth.transform.position += trapGrabber.Position;
      if ((double) Vector3.Distance(trapGrabber.transform.position, player.transform.position) > 4.0)
      {
        trapGrabber.CurrentState = TrapGrabber.State.Idle;
        yield break;
      }
      yield return (object) null;
    }
    trapGrabber.EnemyHealth = player.GetComponent<Health>();
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(trapGrabber.transform.position, trapGrabber.EnemyHealth.transform.position));
    if (trapGrabber.EnemyHealth.isPlayer)
      GameManager.GetInstance().HitStop();
    trapGrabber.Spine.transform.position = trapGrabber.EnemyHealth.transform.position;
    trapGrabber.CachePlayerParent = trapGrabber.EnemyHealth.transform.parent;
    trapGrabber.EnemyHealth.transform.parent = trapGrabber.SortingGroup;
    trapGrabber.playerFarming = trapGrabber.EnemyHealth.GetComponent<PlayerFarming>();
    trapGrabber.playerFarming.state.CURRENT_STATE = StateMachine.State.Grabbed;
    trapGrabber.StartCoroutine((IEnumerator) trapGrabber.DoGrabbing());
    trapGrabber.StartCoroutine((IEnumerator) trapGrabber.DamagePlayer(trapGrabber.EnemyHealth));
    trapGrabber.Teeth.SetActive(false);
  }

  public IEnumerator DamagePlayer(Health h)
  {
    while (this.CurrentState != TrapGrabber.State.Die)
    {
      float Timer = 0.0f;
      while ((double) (Timer += Time.deltaTime) < 1.0)
        yield return (object) null;
      if (this.CurrentState != TrapGrabber.State.Die)
        h.DealDamage(1f, this.Spine.gameObject, this.Spine.transform.position);
      if ((double) h.HP <= 0.0)
        this.Clear();
      yield return (object) null;
    }
  }

  public IEnumerator DoGrabbing()
  {
    TrapGrabber trapGrabber = this;
    trapGrabber.CurrentState = TrapGrabber.State.Grabbing;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      trapGrabber.playerFarming.transform.position = Vector3.Lerp(trapGrabber.playerFarming.transform.position, trapGrabber.Spine.transform.position, Timer / 0.5f);
      yield return (object) null;
    }
    trapGrabber.StartCoroutine((IEnumerator) trapGrabber.DoGrabbed());
  }

  public IEnumerator DoGrabbed()
  {
    TrapGrabber trapGrabber1 = this;
    trapGrabber1.CurrentState = TrapGrabber.State.Grabbed;
    trapGrabber1.playerFarming.transform.position = trapGrabber1.Spine.transform.position;
    while (!InputManager.Gameplay.GetAttackButtonDown())
      yield return (object) null;
    TrapGrabber trapGrabber2 = trapGrabber1;
    int num1 = trapGrabber1.HP - 1;
    int num2 = num1;
    trapGrabber2.HP = num2;
    if (num1 > 0)
      trapGrabber1.StartCoroutine((IEnumerator) trapGrabber1.DoHit());
    else
      trapGrabber1.StartCoroutine((IEnumerator) trapGrabber1.DoKill());
  }

  public IEnumerator DoHit()
  {
    TrapGrabber trapGrabber = this;
    trapGrabber.CurrentState = TrapGrabber.State.Hit;
    trapGrabber.FlashRed();
    CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.20000000298023224)
      yield return (object) null;
    trapGrabber.StartCoroutine((IEnumerator) trapGrabber.DoGrabbed());
  }

  public IEnumerator DoKill()
  {
    TrapGrabber trapGrabber = this;
    trapGrabber.CurrentState = TrapGrabber.State.Die;
    GameManager.GetInstance().HitStop();
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 1.0)
    {
      if ((double) Timer < 0.30000001192092896)
        CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) null;
    }
    trapGrabber.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) trapGrabber.gameObject);
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    this.EnemyHealth = collision.gameObject.GetComponent<Health>();
    if (this.CurrentState != TrapGrabber.State.Idle || !((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || !this.EnemyHealth.isPlayer)
      return;
    this.StartCoroutine((IEnumerator) this.DoAttack());
  }

  public void FlashRed()
  {
    this.StopCoroutine((IEnumerator) this.DoFlashRed());
    this.StartCoroutine((IEnumerator) this.DoFlashRed());
  }

  public IEnumerator DoFlashRed()
  {
    MeshRenderer meshRenderer = this.Spine.gameObject.GetComponent<MeshRenderer>();
    MaterialPropertyBlock block = new MaterialPropertyBlock();
    meshRenderer.SetPropertyBlock(block);
    int fillAlpha = Shader.PropertyToID("_FillAlpha");
    int id = Shader.PropertyToID("_FillColor");
    block.SetFloat(fillAlpha, 1f);
    block.SetColor(id, Color.red);
    meshRenderer.SetPropertyBlock(block);
    float Progress = 0.0f;
    while ((double) (Progress += 0.05f) <= 1.0)
    {
      block.SetFloat(fillAlpha, 1f - Progress);
      meshRenderer.SetPropertyBlock(block);
      yield return (object) null;
    }
  }

  public void OnDestroy() => this.Clear();

  public void Clear()
  {
    this.StopAllCoroutines();
    this.CurrentState = TrapGrabber.State.Idle;
    if (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null))
      return;
    this.playerFarming.transform.parent = this.CachePlayerParent;
    this.playerFarming.HealingParticles.Stop();
  }

  public enum State
  {
    Idle,
    Warning,
    Grabbing,
    Grabbed,
    Hit,
    Die,
  }
}
