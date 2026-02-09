// Decompiled with JetBrains decompiler
// Type: GrassAnimation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GrassAnimation : MonoBehaviour, ICustomUpdateMonoBehaviour
{
  public List<GrassDeformSprite> sprs;
  public bool _animated;
  public Vector3 _shake_target = Vector3.zero;
  public float duration = 2.5f;
  public float strength = 0.3f;
  public int vibrato = 5;
  public float rnd = 10f;

  public void OnTriggerEnter2D(Collider2D collision) => this.StartAnimation();

  public void StartAnimation()
  {
    if (this._animated)
      return;
    this._shake_target = Vector3.zero;
    this._animated = true;
    CustomUpdateManager.updates.Add((ICustomUpdateMonoBehaviour) this);
    DOTween.Shake((DOGetter<Vector3>) (() => this._shake_target), (DOSetter<Vector3>) (x => this._shake_target = x), this.duration, new Vector3(this.strength, this.strength), this.vibrato, this.rnd).OnComplete<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>((TweenCallback) (() =>
    {
      this._animated = false;
      CustomUpdateManager.updates.Remove((ICustomUpdateMonoBehaviour) this);
    }));
  }

  public void CustomUpdate()
  {
    if (!this._animated || !this.gameObject.activeInHierarchy)
      return;
    foreach (GrassDeformSprite spr in this.sprs)
    {
      if (spr.gameObject.activeInHierarchy)
      {
        spr.skew = this._shake_target.x;
        spr.UpdateRenderer();
      }
    }
  }

  public void OnDestroy() => CustomUpdateManager.updates.Remove((ICustomUpdateMonoBehaviour) this);

  [CompilerGenerated]
  public Vector3 \u003CStartAnimation\u003Eb__8_0() => this._shake_target;

  [CompilerGenerated]
  public void \u003CStartAnimation\u003Eb__8_1(Vector3 x) => this._shake_target = x;

  [CompilerGenerated]
  public void \u003CStartAnimation\u003Eb__8_2()
  {
    this._animated = false;
    CustomUpdateManager.updates.Remove((ICustomUpdateMonoBehaviour) this);
  }
}
