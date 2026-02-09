// Decompiled with JetBrains decompiler
// Type: CustomNetworkAnimatorSync
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CustomNetworkAnimatorSync : MonoBehaviour
{
  public Animator _animator;
  public bool _animator_inited;
  [NonSerialized]
  public SerializableAnimatorParameters stored_state = new SerializableAnimatorParameters();
  public const string DEBUG_ANIM_NAME = "worker_zombie";
  public Dictionary<string, bool> _states = new Dictionary<string, bool>();
  public const bool isLocalPlayer = true;
  public const bool isServer = true;

  public static CustomNetworkAnimatorSync InitAnimator(GameObject go)
  {
    CustomNetworkAnimatorSync networkAnimatorSync = go.GetComponentInChildren<CustomNetworkAnimatorSync>() ?? go.AddComponent<CustomNetworkAnimatorSync>();
    networkAnimatorSync.Init(go);
    return networkAnimatorSync;
  }

  public void Init(GameObject go)
  {
    this._animator = go.GetComponentInChildren<Animator>();
    if ((UnityEngine.Object) this._animator == (UnityEngine.Object) null)
    {
      this._animator = go.AddComponent<Animator>();
      this._animator.enabled = false;
      Debug.LogError((object) ("Animator not found at game object: " + go.name), (UnityEngine.Object) go);
    }
    this._animator_inited = true;
  }

  public void SetInteger(string param, int v)
  {
    this.DoSetInteger(param, v);
    if (!CustomNetworkManager.is_running)
      return;
    this.RpcSetInteger(param, v);
  }

  public void SetBool(string param, bool v)
  {
    this.DoSetBool(param, v);
    if (!CustomNetworkManager.is_running)
      return;
    this.RpcSetBool(param, v);
  }

  public void SetFloat(string param, float v)
  {
    this.DoSetFloat(param, v);
    if (!CustomNetworkManager.is_running)
      return;
    this.RpcSetFloat(param, v);
  }

  public void SetTrigger(string param)
  {
    this.DoSetTrigger(param);
    if (!CustomNetworkManager.is_running)
      return;
    this.RpcSetTrigger(param);
  }

  public void ResetTrigger(string param)
  {
    this.DoResetTrigger(param);
    if (!CustomNetworkManager.is_running)
      return;
    this.RpcResetTrigger(param);
  }

  public int GetInteger(string param) => this._animator.GetInteger(param);

  public bool GetBool(string param) => this._animator.GetBool(param);

  public AnimatorControllerParameter[] parameters => this._animator.parameters;

  public bool ParamExists(string param_name)
  {
    if ((UnityEngine.Object) this._animator == (UnityEngine.Object) null || !this._animator.gameObject.activeInHierarchy)
      return false;
    if (this._states.ContainsKey(param_name))
      return this._states[param_name];
    if (!this._animator.gameObject.activeSelf)
      return false;
    bool flag = false;
    foreach (AnimatorControllerParameter parameter in this._animator.parameters)
    {
      if (parameter.name == param_name)
      {
        flag = true;
        break;
      }
    }
    this._states.Add(param_name, flag);
    return flag;
  }

  public void CmdSetInteger(string param, int v) => this.DoSetInteger(param, v);

  public void CmdSetBool(string param, bool v) => this.DoSetBool(param, v);

  public void CmdSetFloat(string param, float v) => this.DoSetFloat(param, v);

  public void CmdSetTrigger(string trigger) => this.DoSetTrigger(trigger);

  public void CmdResetTrigger(string trigger) => this.DoResetTrigger(trigger);

  public void RpcSetInteger(string param, int v)
  {
  }

  public void RpcSetBool(string param, bool v)
  {
  }

  public void RpcSetFloat(string param, float v)
  {
  }

  public void RpcSetTrigger(string trigger)
  {
  }

  public void RpcResetTrigger(string trigger)
  {
  }

  public void DoSetInteger(string param, int v)
  {
    this.stored_state.Set<int>(param, v);
    if (!this.ParamExists(param))
      return;
    this._animator.SetInteger(param, v);
  }

  public void DoSetBool(string param, bool v)
  {
    this.stored_state.Set<bool>(param, v);
    if (!this.ParamExists(param))
      return;
    this._animator.SetBool(param, v);
  }

  public void DoSetFloat(string param, float v)
  {
    this.stored_state.Set<float>(param, v);
    try
    {
      if (!this.ParamExists(param))
        return;
      this._animator.SetFloat(param, v);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Animator exception at object: " + this.gameObject.name), (UnityEngine.Object) this.gameObject);
      throw;
    }
  }

  public void DoSetTrigger(string trigger)
  {
    if (!this.ParamExists(trigger))
      return;
    this._animator.SetTrigger(trigger);
  }

  public void DoResetTrigger(string trigger)
  {
    if (!this.ParamExists(trigger))
      return;
    this._animator.ResetTrigger(trigger);
  }

  public void DeserializeFromSavedState(string json)
  {
    if (string.IsNullOrEmpty(json))
    {
      this.stored_state = new SerializableAnimatorParameters();
    }
    else
    {
      this.stored_state = JsonUtility.FromJson<SerializableAnimatorParameters>(json);
      if (!this.gameObject.activeInHierarchy)
        return;
      this.OnEnable();
    }
  }

  public void DeserializeState(SerializableAnimatorParameters state)
  {
    foreach (SerializableAnimatorParameters.SerializableAnimatorParameter par in state.pars)
    {
      switch ((AnimatorControllerParameterType) par.param_type)
      {
        case AnimatorControllerParameterType.Float:
          this._animator.SetFloat(par.param_name, par.float_v);
          continue;
        case AnimatorControllerParameterType.Int:
          this._animator.SetInteger(par.param_name, par.int_v);
          continue;
        case AnimatorControllerParameterType.Bool:
          this._animator.SetBool(par.param_name, par.bool_v);
          continue;
        case AnimatorControllerParameterType.Trigger:
          if (par.trigger_v)
          {
            this._animator.SetTrigger(par.param_name);
            par.trigger_v = false;
            continue;
          }
          continue;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public void OnEnable()
  {
    if (this.stored_state == null)
      return;
    if (!this._animator_inited || (UnityEngine.Object) this._animator == (UnityEngine.Object) null)
    {
      WorldGameObject componentInParent1 = this.GetComponentInParent<WorldGameObject>();
      GameObject go = (GameObject) null;
      if ((UnityEngine.Object) componentInParent1 != (UnityEngine.Object) null)
      {
        go = componentInParent1.gameObject;
      }
      else
      {
        WorldSimpleObject componentInParent2 = this.GetComponentInParent<WorldSimpleObject>();
        if ((UnityEngine.Object) componentInParent2 != (UnityEngine.Object) null)
          go = componentInParent2.gameObject;
      }
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Couldn't init animator, " + this.name), (UnityEngine.Object) this);
        return;
      }
      this.Init(go);
    }
    this.DeserializeState(this.stored_state);
  }
}
