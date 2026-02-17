// Decompiled with JetBrains decompiler
// Type: IcegoreController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class IcegoreController : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject handParent;
  [SpineAnimation("", "", true, false, dataField = "_skeleton")]
  [SerializeField]
  public string walkAnimation;
  [SpineAnimation("", "", true, false, dataField = "_skeleton")]
  [SerializeField]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "_skeleton")]
  [SerializeField]
  public string pickupAnimation;
  [SpineAnimation("", "", true, false, dataField = "_skeleton")]
  [SerializeField]
  public string walkingWithPickupAnimation;
  [SpineAnimation("", "", true, false, dataField = "_skeleton")]
  [SerializeField]
  public string roarAnimation;
  [SerializeField]
  public SkeletonAnimation skeleton;
  public bool hasPickedUpObject;

  public void Show()
  {
    this.skeleton.transform.localScale = Vector3.one;
    this.skeleton.gameObject.SetActive(true);
  }

  public void Hide()
  {
    this.StopAllCoroutines();
    this.skeleton.gameObject.SetActive(false);
  }

  public IEnumerator Pickup(GameObject target, System.Action onPickup = null)
  {
    TrackEntry pickup = this.skeleton.AnimationState.SetAnimation(0, this.pickupAnimation, false);
    yield return (object) new WaitForSeconds(0.9f);
    System.Action action = onPickup;
    if (action != null)
      action();
    this.hasPickedUpObject = true;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
    {
      target.transform.SetParent(this.handParent.transform, false);
      target.transform.localPosition = new Vector3(0.0f, 0.0f, -0.1f);
      target.transform.localRotation = Quaternion.identity;
    }
    yield return (object) new WaitUntil((Func<bool>) (() => pickup.IsComplete));
    this.Idle();
  }

  public IEnumerator Roar()
  {
    TrackEntry roar = this.skeleton.AnimationState.SetAnimation(0, this.roarAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/icegore/roar_vo");
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 1f);
    yield return (object) new WaitUntil((Func<bool>) (() => roar.IsComplete));
    this.Idle();
  }

  public IEnumerator WalkTo(Vector3 position, float speed)
  {
    IcegoreController icegoreController = this;
    bool flag = (double) (position - icegoreController.transform.position).normalized.x > 0.0;
    icegoreController.skeleton.gameObject.transform.localScale = new Vector3(flag ? -1f : 1f, 1f, 1f);
    string animationName = icegoreController.hasPickedUpObject ? icegoreController.walkingWithPickupAnimation : icegoreController.walkAnimation;
    icegoreController.skeleton.AnimationState.SetAnimation(0, animationName, true);
    Coroutine stomping = icegoreController.StartCoroutine((IEnumerator) icegoreController.StompEvery(1f));
    yield return (object) icegoreController.transform.DOMove(position, speed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().WaitForCompletion();
    if (stomping != null)
      icegoreController.StopCoroutine(stomping);
    icegoreController.Idle();
  }

  public IEnumerator StompFor(float duration)
  {
    IcegoreController icegoreController = this;
    Coroutine stomping = icegoreController.StartCoroutine((IEnumerator) icegoreController.StompEvery(1f));
    yield return (object) new WaitForSeconds(duration);
    if (stomping != null)
      icegoreController.StopCoroutine(stomping);
  }

  public IEnumerator StompEvery(float time)
  {
    while (true)
    {
      CameraManager.instance.shakeCamera1(2f, (float) UnityEngine.Random.Range(0, 360));
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/miniboss_kingoflambs/mv_stomp_single");
      yield return (object) new WaitForSeconds(time);
    }
  }

  public void Idle() => this.skeleton.AnimationState.SetAnimation(0, this.idleAnimation, true);
}
