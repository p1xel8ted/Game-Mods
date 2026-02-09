// Decompiled with JetBrains decompiler
// Type: SmartAnimationController2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SmartAnimationController2 : MonoBehaviour
{
  public List<SmartAnimationController2.SmartAnimationActivator> conditions = new List<SmartAnimationController2.SmartAnimationActivator>();
  public float _time_counter;
  public int _frame_counter;
  public Animator _anim;

  public void Start()
  {
    this._anim = this.GetComponent<Animator>();
    if (!((UnityEngine.Object) this._anim == (UnityEngine.Object) null))
      return;
    Debug.LogError((object) $"Animator not found on {this.GetType().Name}, obj = {this.name}", (UnityEngine.Object) this);
    this.enabled = false;
  }

  public void Update()
  {
    ++this._frame_counter;
    this._time_counter += Time.deltaTime;
    for (int index = 0; index < this.conditions.Count; ++index)
    {
      bool flag = false;
      SmartAnimationController2.SmartAnimationActivator condition = this.conditions[index];
      switch (condition.period)
      {
        case SmartAnimationController2.CheckPeriod.EveryFrame:
          flag = true;
          goto case SmartAnimationController2.CheckPeriod.Manual;
        case SmartAnimationController2.CheckPeriod.Every10Frames:
          if (this._frame_counter >= 10)
          {
            flag = true;
            this._frame_counter = 0;
            goto case SmartAnimationController2.CheckPeriod.Manual;
          }
          goto case SmartAnimationController2.CheckPeriod.Manual;
        case SmartAnimationController2.CheckPeriod.EverySec:
          if ((double) this._time_counter >= 1.0)
          {
            this._time_counter = 0.0f;
            flag = true;
            goto case SmartAnimationController2.CheckPeriod.Manual;
          }
          goto case SmartAnimationController2.CheckPeriod.Manual;
        case SmartAnimationController2.CheckPeriod.Every10Sec:
          if ((double) this._time_counter >= 10.0)
          {
            this._time_counter = 0.0f;
            flag = true;
            goto case SmartAnimationController2.CheckPeriod.Manual;
          }
          goto case SmartAnimationController2.CheckPeriod.Manual;
        case SmartAnimationController2.CheckPeriod.EveryMin:
          if ((double) this._time_counter >= 60.0)
          {
            this._time_counter = 0.0f;
            flag = true;
            goto case SmartAnimationController2.CheckPeriod.Manual;
          }
          goto case SmartAnimationController2.CheckPeriod.Manual;
        case SmartAnimationController2.CheckPeriod.Manual:
          if (flag && (UnityEngine.Object) this._anim != (UnityEngine.Object) null)
          {
            this._anim.SetBool(condition.boolean, condition.conditions.CheckCondition());
            continue;
          }
          continue;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  [Serializable]
  public class SmartAnimationActivator
  {
    public string boolean = "";
    public SmartAnimationController2.CheckPeriod period = SmartAnimationController2.CheckPeriod.EveryFrame;
    public SmartConditionsList conditions = new SmartConditionsList();
  }

  [Serializable]
  public enum CheckPeriod
  {
    EveryFrame = 1,
    Every10Frames = 2,
    EverySec = 3,
    Every10Sec = 4,
    EveryMin = 5,
    Manual = 200, // 0x000000C8
  }
}
