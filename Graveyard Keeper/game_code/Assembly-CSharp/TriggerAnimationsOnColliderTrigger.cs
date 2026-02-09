// Decompiled with JetBrains decompiler
// Type: TriggerAnimationsOnColliderTrigger
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TriggerAnimationsOnColliderTrigger : MonoBehaviour
{
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  [Space]
  public TriggerAnimationsOnColliderTrigger.TriggerAnimationDefinition _on_enter;
  [SerializeField]
  public TriggerAnimationsOnColliderTrigger.TriggerAnimationDefinition _on_exit;
  public static List<string> _trigger_names_array;
  public List<string> _state_names = new List<string>();
  public List<string> _trigger_names = new List<string>();
  public List<string> _param_names = new List<string>();
  public bool _is_player_inside;

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (!this.IsAnimatorSet())
      return;
    WorldGameObject componentInParent = other.gameObject.GetComponentInParent<WorldGameObject>();
    if ((UnityEngine.Object) componentInParent == (UnityEngine.Object) null || !componentInParent.is_player || this._is_player_inside)
      return;
    this._is_player_inside = true;
    this._on_enter.TrySetTrigger(this._animator);
    this._on_exit.TryResetTrigger(this._animator);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (!this.IsAnimatorSet())
      return;
    WorldGameObject componentInParent = other.gameObject.GetComponentInParent<WorldGameObject>();
    if ((UnityEngine.Object) componentInParent == (UnityEngine.Object) null || !componentInParent.is_player || !this._is_player_inside)
      return;
    this._is_player_inside = false;
    this._on_exit.TrySetTrigger(this._animator);
    this._on_enter.TryResetTrigger(this._animator);
  }

  public void OnCustomInspectorGUI()
  {
    GJEditorAnimatorHelper.ScanAnimator(this._animator.gameObject, ref this._state_names, ref this._trigger_names, ref this._param_names);
    TriggerAnimationsOnColliderTrigger._trigger_names_array = this._trigger_names;
  }

  public bool IsAnimatorSet()
  {
    if (!((UnityEngine.Object) this._animator == (UnityEngine.Object) null))
      return true;
    Debug.LogError((object) "Animator not set!", (UnityEngine.Object) this);
    return false;
  }

  [Serializable]
  public class TriggerAnimationDefinition
  {
    [SerializeField]
    public string trigger;

    public List<string> _trigger_names_array
    {
      get => TriggerAnimationsOnColliderTrigger._trigger_names_array;
    }

    public void TrySetTrigger(Animator animator)
    {
      if (string.IsNullOrEmpty(this.trigger))
        return;
      animator.SetTrigger(this.trigger);
    }

    public void TryResetTrigger(Animator animator)
    {
      if (string.IsNullOrEmpty(this.trigger))
        return;
      animator.ResetTrigger(this.trigger);
    }
  }
}
