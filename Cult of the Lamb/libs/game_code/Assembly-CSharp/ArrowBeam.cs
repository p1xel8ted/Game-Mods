// Decompiled with JetBrains decompiler
// Type: ArrowBeam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ArrowBeam : MonoBehaviour
{
  [SerializeField]
  public LineRenderer lineRenderer;
  [SerializeField]
  public PolygonCollider2D polygonCollider;
  public static GameObject loadedBeam;
  public Health.Team team = Health.Team.Team2;
  public float duration;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public static void CreateBeam(
    Vector3[] positions,
    float width,
    float duration,
    Health.Team team,
    Transform parent,
    Action<ArrowBeam> result)
  {
    if ((UnityEngine.Object) ArrowBeam.loadedBeam == (UnityEngine.Object) null)
    {
      Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/ArrowBeam.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        ArrowBeam.loadedAddressableAssets.Add(obj);
        ArrowBeam.loadedBeam = obj.Result;
        ArrowBeam.CreateBeam(positions, width, duration, team, parent, result);
      });
    }
    else
    {
      ArrowBeam component = ObjectPool.Spawn(ArrowBeam.loadedBeam).GetComponent<ArrowBeam>();
      component.duration = duration;
      component.lineRenderer.positionCount = positions.Length;
      component.lineRenderer.SetPositions(positions);
      component.lineRenderer.widthMultiplier = width;
      Vector2[] vector2Array = new Vector2[positions.Length * 2 + 1];
      int index1 = 0;
      for (int index2 = 0; index2 < positions.Length; ++index2)
      {
        Vector3 vector3_1 = index2 < positions.Length - 1 ? (positions[index2 + 1] - positions[index2]).normalized : (positions[index2] - positions[index2 - 1]).normalized;
        Vector3 vector3_2 = Quaternion.AngleAxis(-90f, -Vector3.forward) * vector3_1 * (width / 2f);
        Vector3 vector3_3 = Quaternion.AngleAxis(90f, -Vector3.forward) * vector3_1 * (width / 2f);
        if (index2 % 2 != 0)
        {
          vector3_2 *= -1f;
          vector3_3 *= -1f;
        }
        vector2Array[index1] = (Vector2) (positions[index2] + vector3_2);
        vector2Array[index1 + 1] = (Vector2) (positions[index2] + vector3_3);
        index1 += 2;
      }
      vector2Array[vector2Array.Length - 1] = vector2Array[0];
      component.polygonCollider.points = vector2Array;
      Action<ArrowBeam> action = result;
      if (action == null)
        return;
      action(component);
    }
  }

  public void Update()
  {
    this.duration -= Time.deltaTime;
    if ((double) this.duration >= 1.0 || !this.polygonCollider.enabled)
      return;
    this.polygonCollider.enabled = false;
    this.lineRenderer.material.DOFade(0.0f, 1f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  public void OnTriggerStay2D(Collider2D collision)
  {
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collision.gameObject);
    Health component = collision.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.team)
      return;
    float Damage = this.team == Health.Team.PlayerTeam ? PlayerWeapon.GetDamage(3f, farmingComponent.currentWeaponLevel, farmingComponent) : 1f;
    component.DealDamage(Damage, this.gameObject, this.gameObject.transform.position);
  }

  public void OnDestroy()
  {
    if (ArrowBeam.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in ArrowBeam.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    ArrowBeam.loadedAddressableAssets.Clear();
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__7_0() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
