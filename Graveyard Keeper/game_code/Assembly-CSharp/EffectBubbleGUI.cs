// Decompiled with JetBrains decompiler
// Type: EffectBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EffectBubbleGUI : MonoBehaviour
{
  public UILabel label;
  [HideInInspector]
  public Transform tf;
  public Camera _world_cam;
  public Camera _gui_cam;
  public Vector3 _last_target_pos;
  public bool _inited;

  public void Init()
  {
    this.tf = this.transform;
    this._inited = true;
  }

  public void InitEffect(
    Vector3 pos,
    string text,
    Color color,
    bool ignore_timescale = true,
    float custom_time = -1f)
  {
    this.Init();
    this._last_target_pos = pos;
    this.label.color = color;
    this.label.text = text;
    this.HideAll();
    this.label.Activate<UILabel>();
    TweenAlpha componentInChildren1 = this.GetComponentInChildren<TweenAlpha>(true);
    TweenPosition componentInChildren2 = this.GetComponentInChildren<TweenPosition>(true);
    if ((double) custom_time > 0.0)
    {
      componentInChildren2.duration = componentInChildren1.duration = custom_time;
      componentInChildren2.to.y *= custom_time;
    }
    if (!ignore_timescale)
    {
      componentInChildren1.duration *= Time.timeScale;
      componentInChildren2.duration *= Time.timeScale;
    }
    componentInChildren1.Play(true);
    componentInChildren2.Play(true);
    componentInChildren1.SetOnFinished((EventDelegate.Callback) (() => EffectBubblesManager.RemoveBubble(this)));
    this.UpdateBubble();
  }

  public void UpdateBubble()
  {
    if (!this._inited)
      this.Init();
    this.tf.SetGUIPosToWorldPos(this._last_target_pos, MainGame.me.world_cam, MainGame.me.gui_cam);
  }

  public void HideAll() => this.label.Deactivate<UILabel>();

  [CompilerGenerated]
  public void \u003CInitEffect\u003Eb__7_0() => EffectBubblesManager.RemoveBubble(this);
}
