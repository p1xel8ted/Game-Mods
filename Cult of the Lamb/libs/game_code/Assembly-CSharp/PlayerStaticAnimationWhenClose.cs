// Decompiled with JetBrains decompiler
// Type: PlayerStaticAnimationWhenClose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerStaticAnimationWhenClose : MonoBehaviour
{
  public CircleCollider2D collider;
  public bool disabled;
  public bool active = true;

  public bool Active
  {
    get => !this.disabled && this.active;
    set
    {
      if (!this.active & value)
      {
        float radius = this.collider.radius;
        this.collider.radius = 0.0f;
        DOTween.To((DOGetter<float>) (() => this.collider.radius), (DOSetter<float>) (x => this.collider.radius = x), radius, 0.2f);
      }
      this.active = value;
    }
  }

  public void Awake() => this.collider = this.GetComponent<CircleCollider2D>();

  public void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerFarming component = collision.GetComponent<PlayerFarming>();
    if (!((Object) component != (Object) null) || !this.Active)
      return;
    component.Spine.AnimationState.SetAnimation(2, "static", true);
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    PlayerFarming component = collision.GetComponent<PlayerFarming>();
    if (!((Object) component != (Object) null))
      return;
    component.Spine.AnimationState.ClearTrack(2);
  }

  public void Disable()
  {
    this.disabled = true;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.AnimationState.ClearTrack(2);
  }

  [CompilerGenerated]
  public float \u003Cset_Active\u003Eb__5_0() => this.collider.radius;

  [CompilerGenerated]
  public void \u003Cset_Active\u003Eb__5_1(float x) => this.collider.radius = x;
}
