// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.BBParameter`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[Serializable]
public class BBParameter<T> : BBParameter
{
  public Func<T> getter;
  public Action<T> setter;
  [SerializeField]
  public T _value;

  public BBParameter()
  {
  }

  public BBParameter(T value) => this._value = value;

  public T value
  {
    get
    {
      if (this.getter != null)
        return this.getter();
      if (!Application.isPlaying || this.bb == null || string.IsNullOrEmpty(this.name))
        return this._value;
      this.varRef = this.bb.GetVariable(this.name, typeof (T));
      return this.getter == null ? default (T) : this.getter();
    }
    set
    {
      if (this.setter != null)
      {
        this.setter(value);
      }
      else
      {
        if (this.isNone)
          return;
        if (this.bb != null && !string.IsNullOrEmpty(this.name))
        {
          this.varRef = this.PromoteToVariable(this.bb);
          if (this.setter == null)
            return;
          this.setter(value);
        }
        else
          this._value = value;
      }
    }
  }

  public override object objectValue
  {
    get => (object) this.value;
    set => this.value = (T) value;
  }

  public override System.Type varType => typeof (T);

  public override void Bind(Variable variable)
  {
    if (variable == null)
    {
      this.getter = (Func<T>) null;
      this.setter = (Action<T>) null;
      this._value = default (T);
    }
    else
    {
      this.BindGetter(variable);
      this.BindSetter(variable);
    }
  }

  public bool BindGetter(Variable variable)
  {
    if (variable is Variable<T>)
    {
      this.getter = new Func<T>((variable as Variable<T>).GetValue);
      return true;
    }
    Func<object> convertFunc = variable.GetGetConverter(this.varType);
    if (convertFunc == null)
      return false;
    this.getter = (Func<T>) (() => (T) convertFunc());
    return true;
  }

  public bool BindSetter(Variable variable)
  {
    if (variable is Variable<T>)
    {
      this.setter = new Action<T>((variable as Variable<T>).SetValue);
      return true;
    }
    Action<object> convertFunc = variable.GetSetConverter(this.varType);
    if (convertFunc == null)
      return false;
    this.setter = (Action<T>) (value => convertFunc((object) value));
    return true;
  }

  public static implicit operator BBParameter<T>(T value)
  {
    return new BBParameter<T>() { value = value };
  }
}
