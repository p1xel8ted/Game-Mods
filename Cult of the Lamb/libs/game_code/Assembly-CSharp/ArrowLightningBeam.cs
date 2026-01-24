// Decompiled with JetBrains decompiler
// Type: ArrowLightningBeam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ArrowLightningBeam : MonoBehaviour, ISpellOwning
{
  public static List<ArrowLightningBeam> Beams = new List<ArrowLightningBeam>();
  [SerializeField]
  public LineRenderer lineRenderer;
  [SerializeField]
  public PolygonCollider2D polygonCollider;
  public static GameObject loadedBeamLightning;
  public static GameObject loadedBeamFire;
  public Health.Team team = Health.Team.Team2;
  public Health parentHealth;
  public float duration;
  public float width;
  public float signpostDuration;
  public bool startWidthAtZero;
  public bool disableCollision;
  public float signpostWidth = 0.1f;
  public Health.AttackFlags attackFlags;
  public Health Origin;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public static void ClearBeams()
  {
    for (int index = ArrowLightningBeam.Beams.Count - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) ArrowLightningBeam.Beams[index].gameObject);
    ArrowLightningBeam.Beams.Clear();
  }

  public static ArrowLightningBeam CreateBeamFire(
    Vector3[] positions,
    bool simplifiedPolygon,
    float width,
    float duration,
    Health.Team team,
    Transform parent,
    bool disableCollision = false,
    bool startWidthAtZero = false,
    Action<ArrowLightningBeam> result = null,
    float signpostDuration = 0.5f,
    float signpostWidth = 0.1f,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0)
  {
    return ArrowLightningBeam.CreateBeam("Assets/Prefabs/Enemies/Weapons/ArrowFireBeam.prefab", simplifiedPolygon, positions, width, duration, team, parent, disableCollision, startWidthAtZero, result, signpostDuration, signpostWidth, attackFlags: attackFlags);
  }

  public static ArrowLightningBeam CreateBeam(
    Vector3[] positions,
    bool simplifiedPolygon,
    float width,
    float duration,
    Health.Team team,
    Transform parent,
    bool disableCollision = false,
    bool startWidthAtZero = false,
    Action<ArrowLightningBeam> result = null,
    float signpostDuration = 0.5f,
    float signpostWidth = 0.1f,
    Health health = null,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0)
  {
    return ArrowLightningBeam.CreateBeam("Assets/Prefabs/Enemies/Weapons/ArrowLightningBeam.prefab", simplifiedPolygon, positions, width, duration, team, parent, disableCollision, startWidthAtZero, result, signpostDuration, signpostWidth, health, attackFlags);
  }

  public static ArrowLightningBeam CreateBeam(
    string path,
    bool simplifiedPolygon,
    Vector3[] positions,
    float width,
    float duration,
    Health.Team team,
    Transform parent,
    bool disableCollision = false,
    bool startWidthAtZero = false,
    Action<ArrowLightningBeam> result = null,
    float signpostDuration = 0.5f,
    float signpostWidth = 0.1f,
    Health health = null,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0)
  {
    GameObject prefab = !path.Contains("Fire") ? ArrowLightningBeam.loadedBeamLightning : ArrowLightningBeam.loadedBeamFire;
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
    {
      Addressables.LoadAssetAsync<GameObject>((object) path).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        ArrowLightningBeam.loadedAddressableAssets.Add(obj);
        if (path.Contains("Fire"))
          ArrowLightningBeam.loadedBeamFire = obj.Result;
        else
          ArrowLightningBeam.loadedBeamLightning = obj.Result;
        ArrowLightningBeam.CreateBeam(path, simplifiedPolygon, positions, width, duration, team, parent, disableCollision, startWidthAtZero, result, signpostDuration, signpostWidth, health);
      });
      return (ArrowLightningBeam) null;
    }
    ArrowLightningBeam component = ObjectPool.Spawn(prefab).GetComponent<ArrowLightningBeam>();
    component.duration = duration;
    component.lineRenderer.positionCount = positions.Length;
    component.lineRenderer.SetPositions(positions);
    component.lineRenderer.widthMultiplier = signpostWidth;
    component.width = width;
    component.signpostDuration = signpostDuration;
    component.startWidthAtZero = startWidthAtZero;
    component.disableCollision = disableCollision;
    component.signpostWidth = signpostWidth;
    component.attackFlags = attackFlags;
    Vector2[] vector2Array1 = ArrowLightningBeam.ConvertPositions(positions, width);
    component.polygonCollider.points = vector2Array1;
    component.polygonCollider.enabled = false;
    if (simplifiedPolygon)
    {
      Vector2[] vector2Array2 = ArrowLightningBeam.ConvertPositions(new Vector3[2]
      {
        positions[0],
        positions[positions.Length - 1]
      }, width);
      component.polygonCollider.points = vector2Array2;
    }
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      component.parentHealth = health;
    component.Configure();
    Action<ArrowLightningBeam> action = result;
    if (action != null)
      action(component);
    return component;
  }

  public static Vector2[] ConvertPositions(Vector3[] positions, float width)
  {
    Vector2[] vector2Array = new Vector2[positions.Length * 2 + 1];
    int index1 = 0;
    for (int index2 = 0; index2 < positions.Length; ++index2)
    {
      Vector3 vector3_1;
      Vector3 normalized;
      if (index2 >= positions.Length - 1)
      {
        vector3_1 = positions[index2] - positions[index2 - 1];
        normalized = vector3_1.normalized;
      }
      else
      {
        vector3_1 = positions[index2 + 1] - positions[index2];
        normalized = vector3_1.normalized;
      }
      Vector3 vector3_2 = normalized;
      Vector3 vector3_3 = Quaternion.AngleAxis(-90f, -Vector3.forward) * vector3_2 * (width / 2f);
      Vector3 vector3_4 = Quaternion.AngleAxis(90f, -Vector3.forward) * vector3_2 * (width / 2f);
      if (index2 % 2 != 0)
      {
        vector3_3 *= -1f;
        vector3_4 *= -1f;
      }
      vector2Array[index1] = (Vector2) (positions[index2] + vector3_3);
      vector2Array[index1 + 1] = (Vector2) (positions[index2] + vector3_4);
      index1 += 2;
    }
    vector2Array[vector2Array.Length - 1] = vector2Array[0];
    return vector2Array;
  }

  public void Configure()
  {
    if (!ArrowLightningBeam.Beams.Contains(this))
      ArrowLightningBeam.Beams.Add(this);
    this.polygonCollider.enabled = false;
    this.lineRenderer.startWidth = this.signpostWidth;
    this.lineRenderer.endWidth = this.signpostWidth;
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(this.signpostDuration, (System.Action) (() =>
    {
      this.polygonCollider.enabled = !this.disableCollision;
      this.lineRenderer.startWidth = this.startWidthAtZero ? 0.1f : this.width;
      this.lineRenderer.endWidth = this.width;
      this.StartCoroutine((IEnumerator) this.WaitForSeconds(this.duration, (System.Action) (() =>
      {
        this.polygonCollider.enabled = false;
        float t = 0.0f;
        DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 0.5f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          this.lineRenderer.startWidth = this.startWidthAtZero ? 0.1f : Mathf.Lerp(this.width, 0.0f, t);
          this.lineRenderer.endWidth = Mathf.Lerp(this.width, 0.0f, t);
        })).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
            return;
          ObjectPool.Recycle(this.gameObject);
        }));
      })));
    })));
  }

  public IEnumerator WaitForSeconds(float time, System.Action callback)
  {
    float t = 0.0f;
    while (true)
    {
      if (!PlayerRelic.TimeFrozen)
        t += Time.deltaTime;
      if ((double) t < (double) time)
        yield return (object) null;
      else
        break;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void UpdatePositions(Vector3[] positions)
  {
    Vector2[] vector2Array = ArrowLightningBeam.ConvertPositions(positions, this.width);
    this.lineRenderer.SetPositions(positions);
    this.polygonCollider.points = vector2Array;
  }

  public void ForceStop(float duration = 0.5f)
  {
    this.StopAllCoroutines();
    this.polygonCollider.enabled = false;
    float t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, duration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.lineRenderer.startWidth = this.startWidthAtZero ? 0.1f : Mathf.Lerp(this.width, 0.0f, t);
      this.lineRenderer.endWidth = Mathf.Lerp(this.width, 0.0f, t);
    })).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
        return;
      ObjectPool.Recycle(this.gameObject);
    }));
  }

  public void OnTriggerStay2D(Collider2D collision)
  {
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collision.gameObject);
    if (this.attackFlags.HasFlag((Enum) Health.AttackFlags.Trap) && (bool) (UnityEngine.Object) farmingComponent && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent))
      return;
    Health component = collision.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.team || this.IsOwner(component))
      return;
    float Damage = this.team == Health.Team.PlayerTeam ? PlayerWeapon.GetDamage(3f, farmingComponent.currentWeaponLevel, farmingComponent) : 1f;
    component.DealDamage(Damage, this.gameObject, this.gameObject.transform.position);
  }

  public bool IsOwner(Health health)
  {
    return (UnityEngine.Object) this.parentHealth != (UnityEngine.Object) null && (UnityEngine.Object) this.parentHealth == (UnityEngine.Object) health;
  }

  public void OnDestroy()
  {
    if (ArrowLightningBeam.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in ArrowLightningBeam.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      ArrowLightningBeam.loadedAddressableAssets.Clear();
    }
    ArrowLightningBeam.Beams.Remove(this);
  }

  public GameObject GetOwner()
  {
    return !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__21_0()
  {
    this.polygonCollider.enabled = !this.disableCollision;
    this.lineRenderer.startWidth = this.startWidthAtZero ? 0.1f : this.width;
    this.lineRenderer.endWidth = this.width;
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(this.duration, (System.Action) (() =>
    {
      this.polygonCollider.enabled = false;
      float t = 0.0f;
      DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 0.5f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.lineRenderer.startWidth = this.startWidthAtZero ? 0.1f : Mathf.Lerp(this.width, 0.0f, t);
        this.lineRenderer.endWidth = Mathf.Lerp(this.width, 0.0f, t);
      })).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
          return;
        ObjectPool.Recycle(this.gameObject);
      }));
    })));
  }

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__21_1()
  {
    this.polygonCollider.enabled = false;
    float t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 0.5f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.lineRenderer.startWidth = this.startWidthAtZero ? 0.1f : Mathf.Lerp(this.width, 0.0f, t);
      this.lineRenderer.endWidth = Mathf.Lerp(this.width, 0.0f, t);
    })).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
        return;
      ObjectPool.Recycle(this.gameObject);
    }));
  }

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__21_5()
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
      return;
    ObjectPool.Recycle(this.gameObject);
  }
}
