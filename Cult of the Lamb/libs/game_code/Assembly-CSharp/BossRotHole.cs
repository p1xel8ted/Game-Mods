// Decompiled with JetBrains decompiler
// Type: BossRotHole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BossRotHole : UnitObject
{
  public GameObject HoleMouthSprite;
  public GameObject WIPWeakpointSprite;
  public GameObject WIPSpottedTarget;
  public GameObject Weakpoint;
  public ColliderEvents damageColliderEvents;
  public Collider2D solidCollider;
  public Animator animator;
  public GameObject[] shuffleGameObjects;
  public List<Vector3> shufflePositions;
  public bool SelfControlled;
  public Coroutine currentStateRoutine;
  public DG.Tweening.Sequence attackSequence;

  public void AddShufflePositions()
  {
    Debug.Log((object) "Custom button clicked!");
    this.shufflePositions.Clear();
    this.shufflePositions.Add(this.transform.position);
    for (int index = 0; index < this.shuffleGameObjects.Length; ++index)
      this.shufflePositions.Add(this.shuffleGameObjects[index].transform.position);
  }

  public override void Awake()
  {
    base.Awake();
    for (int index = 0; index < this.shuffleGameObjects.Length; ++index)
      this.shuffleGameObjects[index].SetActive(false);
    this.WIPSpottedTarget.SetActive(false);
    this.Weakpoint.SetActive(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.damageColliderEvents.SetActive(true);
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.SetDangerous(true);
    if (!this.SelfControlled)
      return;
    this.StartState(this.IdleState());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.damageColliderEvents.SetActive(false);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void StartState(IEnumerator newState)
  {
    if (this.currentStateRoutine != null)
      this.StopCoroutine(this.currentStateRoutine);
    this.currentStateRoutine = this.StartCoroutine((IEnumerator) newState);
  }

  public IEnumerator IdleState()
  {
    BossRotHole bossRotHole = this;
    bossRotHole.health.invincible = true;
    float visionDistance = 100f;
    bossRotHole.state.CURRENT_STATE = StateMachine.State.Idle;
    Health health;
    do
    {
      yield return (object) new WaitForSeconds(0.5f);
      health = (Health) PlayerFarming.GetClosestPlayer(bossRotHole.transform.position)?.health;
    }
    while (!((Object) health != (Object) null) || (double) Vector3.Distance(bossRotHole.transform.position, health.transform.position) > (double) visionDistance);
    bossRotHole.StartState(bossRotHole.ApproachPlayerState());
  }

  public IEnumerator ApproachPlayerState()
  {
    BossRotHole bossRotHole = this;
    yield return (object) new WaitForSeconds(0.1f);
    bossRotHole.maxSpeed = 0.1f;
    bossRotHole.state.CURRENT_STATE = StateMachine.State.Moving;
    bossRotHole.UsePathing = true;
    bossRotHole.health.invincible = true;
    float weakpointTime = Time.time + 15f;
    do
    {
      yield return (object) new WaitForSeconds(0.25f);
      GameObject gameObject = PlayerFarming.GetClosestPlayer(bossRotHole.transform.position)?.gameObject;
      if ((Object) gameObject != (Object) null)
        bossRotHole.givePath(gameObject.transform.position, gameObject.gameObject);
      else
        goto label_4;
    }
    while ((double) Time.time <= (double) weakpointTime);
    bossRotHole.StartState(bossRotHole.ShowWeakpointState());
    yield break;
label_4:
    bossRotHole.StartState(bossRotHole.IdleState());
  }

  public IEnumerator ShowWeakpointState()
  {
    BossRotHole bossRotHole = this;
    if ((Object) bossRotHole.Weakpoint == (Object) null)
    {
      bossRotHole.StartState(bossRotHole.IdleState());
    }
    else
    {
      bossRotHole.state.CURRENT_STATE = StateMachine.State.Idle;
      bossRotHole.Weakpoint.SetActive(true);
      bossRotHole.Weakpoint.transform.localScale = Vector3.zero;
      yield return (object) bossRotHole.Weakpoint.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).WaitForCompletion();
      bossRotHole.health.invincible = false;
      float weakTime = Time.time + 10f;
      while ((double) Time.time < (double) weakTime)
      {
        GameObject gameObject = PlayerFarming.GetClosestPlayer(bossRotHole.transform.position)?.gameObject;
        if ((Object) gameObject != (Object) null)
        {
          float x = (double) gameObject.transform.position.x < (double) bossRotHole.transform.position.x ? 1f : -1f;
          bossRotHole.Weakpoint.transform.localScale = new Vector3(x, 1f, 1f);
        }
        yield return (object) new WaitForSeconds(0.5f);
      }
      yield return (object) bossRotHole.Weakpoint.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).WaitForCompletion();
      bossRotHole.Weakpoint.SetActive(false);
      bossRotHole.health.invincible = true;
      bossRotHole.StartState(bossRotHole.IdleState());
    }
  }

  public void SetDangerous(bool dangerous)
  {
    this.damageColliderEvents.SetActive(dangerous);
    this.solidCollider.gameObject.SetActive(!dangerous);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.health.team)
      return;
    this.playBiteAnimation();
    component.DealDamage(1f, this.gameObject, this.transform.position);
  }

  public void playBiteAnimation()
  {
    if (this.attackSequence != null)
      this.attackSequence.Kill();
    this.HoleMouthSprite.transform.localScale = Vector3.one;
    this.attackSequence = DOTween.Sequence();
    this.attackSequence.Append((Tween) this.HoleMouthSprite.transform.DOScale(1.25f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad)).Append((Tween) this.HoleMouthSprite.transform.DOScale(0.5f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad)).Append((Tween) this.HoleMouthSprite.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad));
    this.attackSequence.Play<DG.Tweening.Sequence>();
  }
}
