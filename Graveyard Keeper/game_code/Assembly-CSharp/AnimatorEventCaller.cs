// Decompiled with JetBrains decompiler
// Type: AnimatorEventCaller
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class AnimatorEventCaller : MonoBehaviour
{
  public List<AnimatorEventCaller.AnimatorEventCallerAtom> event_caller_atoms = new List<AnimatorEventCaller.AnimatorEventCallerAtom>();

  public void SetTrigger_1()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_1 error!");
    else
      this.CallTrigger(this.event_caller_atoms[0]);
  }

  public void SetTrigger_2()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_2 error!");
    else
      this.CallTrigger(this.event_caller_atoms[1]);
  }

  public void SetTrigger_3()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_3 error!");
    else
      this.CallTrigger(this.event_caller_atoms[2]);
  }

  public void SetTrigger_4()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_4 error!");
    else
      this.CallTrigger(this.event_caller_atoms[3]);
  }

  public void SetTrigger_5()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_5 error!");
    else
      this.CallTrigger(this.event_caller_atoms[4]);
  }

  public void SetTrigger_6()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_6 error!");
    else
      this.CallTrigger(this.event_caller_atoms[5]);
  }

  public void SetTrigger_7()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_7 error!");
    else
      this.CallTrigger(this.event_caller_atoms[6]);
  }

  public void SetTrigger_8()
  {
    if (this.event_caller_atoms == null || this.event_caller_atoms.Count < 1)
      Debug.LogError((object) "SetTrigger_8 error!");
    else
      this.CallTrigger(this.event_caller_atoms[7]);
  }

  public void CallTrigger(
    AnimatorEventCaller.AnimatorEventCallerAtom caller_atom)
  {
    if ((UnityEngine.Object) caller_atom.animator == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "CallTrigger error: animator is null!");
    }
    else
    {
      switch (caller_atom.anim_type)
      {
        case AnimatorEventCaller.AnimatorParamType.Integer:
          caller_atom.animator.SetInteger(caller_atom.anim_param_name, caller_atom.anim_int);
          break;
        case AnimatorEventCaller.AnimatorParamType.Trigger:
          caller_atom.animator.SetTrigger(caller_atom.anim_param_name);
          break;
        case AnimatorEventCaller.AnimatorParamType.Bool:
          caller_atom.animator.SetBool(caller_atom.anim_param_name, caller_atom.anim_bool);
          break;
        case AnimatorEventCaller.AnimatorParamType.Float:
          caller_atom.animator.SetFloat(caller_atom.anim_param_name, caller_atom.anim_float);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public enum AnimatorParamType
  {
    Integer,
    Trigger,
    Bool,
    Float,
  }

  [Serializable]
  public class AnimatorEventCallerAtom
  {
    public Animator animator;
    public AnimatorEventCaller.AnimatorParamType anim_type;
    public string anim_param_name;
    public int anim_int;
    public bool anim_bool;
    public float anim_float;
  }
}
