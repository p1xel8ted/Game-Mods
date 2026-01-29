// Decompiled with JetBrains decompiler
// Type: SinSnake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class SinSnake : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  public float progress;
  public float lifetime;
  public float speed;
  public bool hiding;

  public static void Spawn(
    Vector3 spawnPosition,
    float angle,
    Transform parent,
    float speed,
    float lifetime)
  {
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Sin Snake.prefab", spawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)), parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      SinSnake component = obj.Result.GetComponent<SinSnake>();
      component.lifetime = lifetime;
      component.speed = speed;
      int num = UnityEngine.Random.Range(0, 4);
      if (num <= 1)
        component.spine.Skeleton.SetSkin("black");
      else if (num == 1)
        component.spine.Skeleton.SetSkin("diamond");
      else
        component.spine.Skeleton.SetSkin("striped");
    });
  }

  public void Update()
  {
    this.transform.position += -this.transform.up * this.speed * Time.deltaTime;
    this.progress += Time.deltaTime;
    if ((double) this.progress <= (double) this.lifetime || this.hiding)
      return;
    this.hiding = true;
    float time = 0.0f;
    DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, 0.25f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.spine.Skeleton.A = 1f - time)).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__6_3() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
