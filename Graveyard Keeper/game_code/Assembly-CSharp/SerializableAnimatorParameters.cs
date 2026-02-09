// Decompiled with JetBrains decompiler
// Type: SerializableAnimatorParameters
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SerializableAnimatorParameters
{
  public List<SerializableAnimatorParameters.SerializableAnimatorParameter> pars = new List<SerializableAnimatorParameters.SerializableAnimatorParameter>();

  public SerializableAnimatorParameters()
  {
    this.Set<int>("global_state", 0);
    this.Set<float>("direction_angle", -90f);
  }

  public void Set<T>(string name, T value) where T : struct
  {
    AnimatorControllerParameterType animType = this.GetAnimType(typeof (T));
    foreach (SerializableAnimatorParameters.SerializableAnimatorParameter par in this.pars)
    {
      if (par.param_name == name && (AnimatorControllerParameterType) par.param_type == animType)
      {
        par.SetValue<T>(animType, value);
        return;
      }
    }
    SerializableAnimatorParameters.SerializableAnimatorParameter animatorParameter = new SerializableAnimatorParameters.SerializableAnimatorParameter()
    {
      param_name = name
    };
    animatorParameter.SetValue<T>(animType, value);
    this.pars.Add(animatorParameter);
  }

  public bool HasParameter(string name)
  {
    foreach (SerializableAnimatorParameters.SerializableAnimatorParameter par in this.pars)
    {
      if (par.param_name == name)
        return true;
    }
    return false;
  }

  public float GetParameterFloat(string name)
  {
    foreach (SerializableAnimatorParameters.SerializableAnimatorParameter par in this.pars)
    {
      if (par.param_name == name)
        return par.float_v;
    }
    return 0.0f;
  }

  public AnimatorControllerParameterType GetAnimType(System.Type t)
  {
    if (System.Type.op_Equality(t, typeof (int)))
      return AnimatorControllerParameterType.Int;
    if (System.Type.op_Equality(t, typeof (bool)))
      return AnimatorControllerParameterType.Bool;
    if (System.Type.op_Equality(t, typeof (float)))
      return AnimatorControllerParameterType.Float;
    if (System.Type.op_Equality(t, typeof (SerializableAnimatorParameters.Trigger)))
      return AnimatorControllerParameterType.Trigger;
    throw new Exception("Unknown type: " + t?.ToString());
  }

  [Serializable]
  public class SerializableAnimatorParameter
  {
    public int int_v;
    public bool bool_v;
    public float float_v;
    public bool trigger_v;
    public int param_type;
    public string param_name;

    public void SetValue<T>(AnimatorControllerParameterType at, T value) where T : struct
    {
      switch (at)
      {
        case AnimatorControllerParameterType.Float:
          this.float_v = Convert.ToSingle((object) value);
          break;
        case AnimatorControllerParameterType.Int:
          this.int_v = Convert.ToInt32((object) value);
          break;
        case AnimatorControllerParameterType.Bool:
          this.bool_v = Convert.ToBoolean((object) value);
          break;
        case AnimatorControllerParameterType.Trigger:
          this.trigger_v = Convert.ToBoolean((object) value);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.param_type = (int) at;
    }
  }

  [Serializable]
  public struct Trigger
  {
    public bool v;
  }
}
