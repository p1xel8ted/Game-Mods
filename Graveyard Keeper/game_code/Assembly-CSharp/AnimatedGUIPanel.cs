// Decompiled with JetBrains decompiler
// Type: AnimatedGUIPanel
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

#nullable disable
public class AnimatedGUIPanel : MonoBehaviour
{
  public AnimatedGUIPanel.State _cur_state;
  public int _coord_hidden;
  public int _coord_shown;
  public float _timer;
  public Tweener _tweener;
  public float animation_time = 1f;
  public float stay_shown_time = 3f;
  public AnimatedGUIPanel.AnimationType animation_type;

  public void Init(int coord_hidden, int coord_shown, AnimatedGUIPanel.AnimationType anim_type = AnimatedGUIPanel.AnimationType.AnimateY)
  {
    this.gameObject.SetActive(false);
    this.animation_type = anim_type;
    this.coord = (float) coord_hidden;
    this._coord_hidden = coord_hidden;
    this._coord_shown = coord_shown;
  }

  public float coord
  {
    get
    {
      switch (this.animation_type)
      {
        case AnimatedGUIPanel.AnimationType.AnimateY:
          return this.transform.localPosition.y;
        case AnimatedGUIPanel.AnimationType.AnimateX:
          return this.transform.localPosition.x;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    set
    {
      Vector3 localPosition = this.transform.localPosition;
      switch (this.animation_type)
      {
        case AnimatedGUIPanel.AnimationType.AnimateY:
          localPosition.y = value;
          break;
        case AnimatedGUIPanel.AnimationType.AnimateX:
          localPosition.x = value;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.transform.localPosition = localPosition;
    }
  }

  public void SetVisible(bool vis)
  {
    if (vis && (this._cur_state == AnimatedGUIPanel.State.Shown || this._cur_state == AnimatedGUIPanel.State.Showing))
    {
      this._timer = 0.0f;
    }
    else
    {
      if (!vis && (this._cur_state == AnimatedGUIPanel.State.Hidden || this._cur_state == AnimatedGUIPanel.State.Hiding))
        return;
      this.gameObject.SetActive(true);
      this.Redraw();
      if (this._tweener != null)
        this._tweener.Kill();
      AnimatedGUIPanel.State dest = vis ? AnimatedGUIPanel.State.Shown : AnimatedGUIPanel.State.Hidden;
      this._tweener = (Tweener) DOTween.To((DOGetter<float>) (() => this.coord), (DOSetter<float>) (v => this.coord = v), vis ? (float) this._coord_shown : (float) this._coord_hidden, this.animation_time).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this._tweener = (Tweener) null;
        this._cur_state = dest;
        this._timer = 0.0f;
        if (this._cur_state != AnimatedGUIPanel.State.Hidden)
          return;
        this.gameObject.SetActive(false);
      }));
      this._cur_state = vis ? AnimatedGUIPanel.State.Showing : AnimatedGUIPanel.State.Hiding;
    }
  }

  public void Show() => this.SetVisible(true);

  public virtual void Redraw()
  {
  }

  public void Update()
  {
    if (this._cur_state != AnimatedGUIPanel.State.Shown)
      return;
    this._timer += Time.deltaTime;
    if ((double) this._timer <= (double) this.stay_shown_time)
      return;
    this.SetVisible(false);
  }

  public enum State
  {
    Hidden,
    Shown,
    Showing,
    Hiding,
  }

  public enum AnimationType
  {
    AnimateY,
    AnimateX,
  }
}
