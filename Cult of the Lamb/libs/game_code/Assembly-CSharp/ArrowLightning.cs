// Decompiled with JetBrains decompiler
// Type: ArrowLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Ara;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ArrowLightning : MonoBehaviour
{
  [CompilerGenerated]
  public Health \u003CTarget\u003Ek__BackingField;
  [SerializeField]
  public AraTrail trail;
  [SerializeField]
  public TrailRenderer lowQualityTrail;
  [SerializeField]
  public Material altMaterial;

  public Health Target
  {
    get => this.\u003CTarget\u003Ek__BackingField;
    set => this.\u003CTarget\u003Ek__BackingField = value;
  }

  public static void CreateProjectiles(
    float damage,
    float duration,
    Health owner,
    Health target,
    Vector3 localPosition,
    System.Action callback = null,
    bool useAltMaterial = false)
  {
    ArrowLightning.CreateProjectiles(damage, duration, owner, target.transform.position, target.transform, localPosition, (System.Action) (() =>
    {
      if ((UnityEngine.Object) target != (UnityEngine.Object) null)
        target.DealDamage(damage, owner.gameObject, target.transform.position);
      System.Action action = callback;
      if (action == null)
        return;
      action();
    }), useAltMaterial);
  }

  public static void CreateProjectiles(
    float damage,
    float duration,
    Health owner,
    Vector3 target,
    Transform parent,
    Vector3 localPosition,
    System.Action callback = null,
    bool useAltMaterial = false)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/moving_spike_trap/moving_spike_trap_start", target);
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/ArrowLightning.prefab", target, Quaternion.identity, owner.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      if ((UnityEngine.Object) obj.Result == (UnityEngine.Object) null)
        return;
      ArrowLightning arrow = obj.Result.GetComponent<ArrowLightning>();
      arrow.transform.parent = parent;
      arrow.transform.localPosition = localPosition;
      arrow.trail.enabled = true;
      arrow.lowQualityTrail.enabled = false;
      arrow.trail.Clear();
      if (useAltMaterial)
        arrow.trail.materials[0] = arrow.altMaterial;
      foreach (Material material in arrow.trail.materials)
        material.DOFade(1f, 0.0f);
      CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, duration);
      arrow.transform.DOLocalMove(Vector3.zero, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if ((UnityEngine.Object) arrow != (UnityEngine.Object) null)
        {
          CameraManager.instance.ShakeCameraForDuration(1f, 1.25f, 0.15f);
          BiomeConstants.Instance.EmitSmokeExplosionVFX(arrow.transform.position);
          BiomeConstants.Instance.EmitGroundSmashVFXParticles(arrow.transform.position, 3f);
          Explosion.CreateExplosion(arrow.transform.position, Health.Team.PlayerTeam, owner, 2f, Team2Damage: damage);
          if ((UnityEngine.Object) arrow.trail != (UnityEngine.Object) null)
            arrow.StartCoroutine((IEnumerator) ArrowLightning.\u003CCreateProjectiles\u003Eg__Delay\u007C8_2(1f, (System.Action) (() =>
            {
              if (!((UnityEngine.Object) arrow != (UnityEngine.Object) null))
                return;
              arrow.trail.emit = false;
              foreach (Material material in arrow.trail.materials)
                material.DOFade(0.0f, 0.5f);
            })));
          arrow.transform.parent = (Transform) null;
          UnityEngine.Object.Destroy((UnityEngine.Object) arrow.gameObject, 2f);
        }
        System.Action action = callback;
        if (action == null)
          return;
        action();
      }));
    }));
  }

  [CompilerGenerated]
  public static IEnumerator \u003CCreateProjectiles\u003Eg__Delay\u007C8_2(float delay, System.Action c)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = c;
    if (action != null)
      action();
  }
}
