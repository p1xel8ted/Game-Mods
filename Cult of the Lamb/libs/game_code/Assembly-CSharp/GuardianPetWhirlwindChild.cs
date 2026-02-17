// Decompiled with JetBrains decompiler
// Type: GuardianPetWhirlwindChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GuardianPetWhirlwindChild : MonoBehaviour
{
  public float RotationSpeed;
  public float RotationMaxSpeed = 7f;
  public GuardianPet Pet;
  public int Index;
  public int Total;
  public ColliderEvents colliderEvents;
  public float Angle;
  [SerializeField]
  public float Distance = 1f;
  public bool Closing;

  public void Play(GuardianPet Pet, int Index, int Total)
  {
    this.Pet = Pet;
    this.Index = Index;
    this.Total = Total;
    this.Closing = false;
    this.gameObject.SetActive(true);
    this.RotationSpeed = 0.0f;
    this.Angle = (float) (Index * (360 / Total)) * ((float) Math.PI / 180f);
    this.transform.DOKill();
    this.transform.localPosition = Vector3.zero;
    this.transform.DOLocalMove(new Vector3(this.Distance * Mathf.Cos(this.Angle), this.Distance * Mathf.Sin(this.Angle)), 0.5f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.StartCoroutine((IEnumerator) this.LoopRoutine())));
  }

  public IEnumerator LoopRoutine()
  {
    GuardianPetWhirlwindChild petWhirlwindChild = this;
    DOTween.To(new DOGetter<float>(petWhirlwindChild.\u003CLoopRoutine\u003Eb__10_0), new DOSetter<float>(petWhirlwindChild.\u003CLoopRoutine\u003Eb__10_1), petWhirlwindChild.RotationMaxSpeed, 1f);
    petWhirlwindChild.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(petWhirlwindChild.OnDamageTriggerEnter);
    while (true)
    {
      petWhirlwindChild.Angle += petWhirlwindChild.RotationSpeed * Time.deltaTime;
      petWhirlwindChild.transform.localPosition = new Vector3(petWhirlwindChild.Distance * Mathf.Cos(petWhirlwindChild.Angle), petWhirlwindChild.Distance * Mathf.Sin(petWhirlwindChild.Angle));
      yield return (object) null;
    }
  }

  public void Close()
  {
    if (!this.gameObject.activeSelf || this.Closing)
      return;
    this.Closing = true;
    this.StopAllCoroutines();
    this.colliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.transform.DOKill();
    this.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false))).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
  }

  public void OnEnable() => this.colliderEvents = this.GetComponent<ColliderEvents>();

  public void OnDisable()
  {
    this.StopAllCoroutines();
    this.colliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == Health.Team.Team2 && !component.IsCharmedEnemy && component.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__9_0() => this.StartCoroutine((IEnumerator) this.LoopRoutine());

  [CompilerGenerated]
  public float \u003CLoopRoutine\u003Eb__10_0() => this.RotationSpeed;

  [CompilerGenerated]
  public void \u003CLoopRoutine\u003Eb__10_1(float x) => this.RotationSpeed = x;

  [CompilerGenerated]
  public void \u003CClose\u003Eb__11_0() => this.gameObject.SetActive(false);
}
