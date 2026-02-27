// Decompiled with JetBrains decompiler
// Type: EnemyStasisTicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyStasisTicker : BaseMonoBehaviour
{
  private Health enemy;
  private bool active;
  private Vector2 offset;
  private float startFrame = -1f;

  public static void Instantiate(
    Health enemy,
    Vector2 offset,
    Health.AttackTypes attackType,
    Action<EnemyStasisTicker> result)
  {
    string key = "";
    switch (attackType)
    {
      case Health.AttackTypes.Poison:
        key = "Assets/Prefabs/Enemies/Misc/Enemy Poison Ticker.prefab";
        break;
      case Health.AttackTypes.Ice:
        key = "Assets/Prefabs/Enemies/Misc/Enemy Ice Ticker.prefab";
        break;
      case Health.AttackTypes.Charm:
        key = "Assets/Prefabs/Enemies/Misc/Enemy Charm Ticker.prefab";
        break;
    }
    Addressables.InstantiateAsync((object) key).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyStasisTicker component = obj.Result.GetComponent<EnemyStasisTicker>();
      component.enemy = enemy;
      component.offset = offset;
      component.startFrame = Time.time;
      Action<EnemyStasisTicker> action = result;
      if (action == null)
        return;
      action(component);
    });
  }

  private void OnEnable() => this.Show();

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

  private void LateUpdate()
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
}
