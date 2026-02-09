// Decompiled with JetBrains decompiler
// Type: NewBodyArrivedGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class NewBodyArrivedGUI : MonoBehaviour
{
  [SerializeField]
  public float _appear_time = 0.4f;
  [SerializeField]
  public float _display_time = 1f;
  [SerializeField]
  public float _hide_time = 0.2f;
  [SerializeField]
  [Space]
  public float _visible_point_y = 30f;
  [SerializeField]
  public float _default_point_y = 40f;
  [SerializeField]
  public GameObject _parent_object;

  public void Display()
  {
    if (this.gameObject.activeSelf)
      return;
    if (!this._parent_object.activeSelf)
      this._parent_object.gameObject.SetActive(true);
    this.gameObject.SetActive(true);
    this.StartCoroutine(this.RunAppearCoroutine());
  }

  public IEnumerator RunAppearCoroutine()
  {
    NewBodyArrivedGUI newBodyArrivedGui = this;
    bool is_done = false;
    newBodyArrivedGui.transform.DOLocalMoveY((float) Screen.height / 4f - newBodyArrivedGui._visible_point_y, newBodyArrivedGui._appear_time).OnComplete<Tweener>((TweenCallback) (() => is_done = true));
    yield return (object) new WaitUntil((Func<bool>) (() => is_done));
    yield return (object) new WaitForSeconds(newBodyArrivedGui._display_time);
    newBodyArrivedGui.transform.DOLocalMoveY((float) Screen.height / 4f + newBodyArrivedGui._default_point_y, newBodyArrivedGui._hide_time).OnComplete<Tweener>(new TweenCallback(newBodyArrivedGui.OnAnimationComplete));
  }

  public void OnAnimationComplete() => this.gameObject.SetActive(false);
}
