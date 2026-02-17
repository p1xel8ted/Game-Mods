// Decompiled with JetBrains decompiler
// Type: Bouncer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMRoomGeneration;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Bouncer : MonoBehaviour
{
  public AnimationCurve bounceCurve;
  public UnityEvent OnBounceCallback;
  [SerializeField]
  public bool isShrinkingOnRoomComplete = true;
  [SerializeField]
  public float inflictsPoisonAmmount;
  public float lastPoisonTime;
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string bounceAnimation;
  public Health health;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    if ((Object) this.health != (Object) null)
    {
      this.health.OnHit += new Health.HitAction(this.OnHit);
      this.health.OnDie += new Health.DieAction(this.OnDie);
    }
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    this.GetComponent<CircleCollider2D>().enabled = false;
    if (!this.isShrinkingOnRoomComplete)
      return;
    this.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.transform.parent.gameObject.SetActive(false)));
  }

  public void OnDestroy()
  {
    if ((Object) this.health != (Object) null)
    {
      this.health.OnHit -= new Health.HitAction(this.OnHit);
      this.health.OnDie -= new Health.DieAction(this.OnDie);
    }
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void bounceUnit(UnitObject unit, Vector3 dir, float power = 20f, float duration = 0.25f)
  {
    AudioManager.Instance.PlayOneShot("event:/material/mushroom_impact", this.gameObject);
    this.StartCoroutine((IEnumerator) this.UpdateBounceUnit(unit, dir, power, duration));
  }

  public IEnumerator UpdateBounceUnit(UnitObject unit, Vector3 dir, float power, float duration)
  {
    float elapsedTime = 0.0f;
    dir.Normalize();
    Debug.Log((object) ("Spine on dropper " + ((object) this.Spine)?.ToString()));
    if ((Object) this.Spine != (Object) null)
    {
      this.Spine.AnimationState.SetAnimation(0, this.bounceAnimation, false);
      this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    }
    while ((double) elapsedTime < (double) duration)
    {
      float num = power * Time.deltaTime * this.bounceCurve.Evaluate(1f / duration * elapsedTime);
      unit.moveVX = dir.x * num;
      unit.moveVY = dir.y * num;
      elapsedTime += Time.deltaTime;
      yield return (object) null;
    }
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    UnitObject component1 = collision.GetComponent<UnitObject>();
    if (!(bool) (Object) component1)
      return;
    this.bounceUnit(component1, component1.transform.position - this.transform.position);
    if ((double) this.inflictsPoisonAmmount > 0.0)
    {
      Health component2 = component1.GetComponent<Health>();
      if ((Object) component2 != (Object) null && component2.isPlayer && (double) Time.realtimeSinceStartup - (double) this.lastPoisonTime > 5.0)
      {
        this.lastPoisonTime = Time.realtimeSinceStartup;
        TrapPoison.CreatePoison(component1.transform.position, 3, 1.5f, GenerateRoom.Instance.transform);
      }
    }
    this.OnBounceCallback.Invoke();
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((double) this.inflictsPoisonAmmount <= 0.0 || (double) Time.realtimeSinceStartup - (double) this.lastPoisonTime <= 2.0)
      return;
    this.lastPoisonTime = Time.realtimeSinceStartup;
    TrapPoison.CreatePoison(this.transform.position, 1, 1.5f, GenerateRoom.Instance.transform);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((double) this.inflictsPoisonAmmount <= 0.0 || (double) Time.realtimeSinceStartup - (double) this.lastPoisonTime <= 2.0)
      return;
    this.lastPoisonTime = Time.realtimeSinceStartup;
    Vector3 normalized = (Attacker.transform.position - this.transform.position).normalized;
    TrapPoison.CreatePoison(this.transform.position, 1, 1.5f, GenerateRoom.Instance.transform);
  }

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__10_0()
  {
    this.transform.parent.gameObject.SetActive(false);
  }
}
