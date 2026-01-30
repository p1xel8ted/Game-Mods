// Decompiled with JetBrains decompiler
// Type: EnemyStasisTicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyStasisTicker : BaseMonoBehaviour
{
  public Health enemy;
  public bool active;
  public Vector2 offset;
  public float startFrame = -1f;

  public static void Instantiate(
    Health enemy,
    Vector2 offset,
    Health.AttackTypes attackType,
    Action<EnemyStasisTicker> result)
  {
    string key = "";
    if (attackType == Health.AttackTypes.Poison)
      key = "Assets/Prefabs/Enemies/Misc/Enemy Poison Ticker.prefab";
    else if (attackType == Health.AttackTypes.Ice)
      key = "Assets/Prefabs/Enemies/Misc/Enemy Ice Ticker.prefab";
    else if (attackType == Health.AttackTypes.Burn)
      key = "Assets/Prefabs/Enemies/Misc/Enemy Fire Ticker.prefab";
    else if (attackType == Health.AttackTypes.Charm)
      key = "Assets/Prefabs/Enemies/Misc/Enemy Charm Ticker.prefab";
    else if (attackType == Health.AttackTypes.Poison)
      key = "Assets/Prefabs/Enemies/Misc/Enemy Poison Ticker.prefab";
    Addressables_wrapper.InstantiateAsync((object) key, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyStasisTicker component = obj.Result.GetComponent<EnemyStasisTicker>();
      component.enemy = enemy;
      component.offset = offset;
      component.startFrame = Time.time;
      if ((UnityEngine.Object) enemy != (UnityEngine.Object) null)
        component.transform.parent = enemy.transform.parent;
      Action<EnemyStasisTicker> action = result;
      if (action == null)
        return;
      action(component);
    }));
  }

  public void OnEnable() => this.Show();

  public void Show()
  {
    this.active = true;
    this.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f).OnComplete<Tweener>((TweenCallback) (() => this.transform.localScale = Vector3.one));
  }

  public void Hide(bool destroyAfter = true)
  {
    this.active = false;
    this.transform.DOScale(Vector3.zero, 0.1f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (!destroyAfter)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }));
  }

  public void LateUpdate()
  {
    if ((UnityEngine.Object) this.enemy != (UnityEngine.Object) null && (double) this.enemy.HP > 0.0 && !this.enemy.invincible && this.enemy.enabled)
    {
      if (!this.active)
        this.Show();
      this.transform.position = this.enemy.transform.position - new Vector3(this.offset.x, -0.5f, this.offset.y);
      this.startFrame = Time.time;
    }
    else if (this.active && (double) this.startFrame != -1.0 && (double) Time.time != (double) this.startFrame)
    {
      this.Hide((UnityEngine.Object) this.enemy == (UnityEngine.Object) null || (double) this.enemy.HP <= 0.0);
    }
    else
    {
      if (!((UnityEngine.Object) this.enemy == (UnityEngine.Object) null) || (double) this.startFrame == -1.0 || (double) Time.time == (double) this.startFrame)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__6_0() => this.transform.localScale = Vector3.one;
}
