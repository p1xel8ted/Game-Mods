// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.BBObjectParameter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[Serializable]
public class BBObjectParameter : BBParameter<object>
{
  [SerializeField]
  public System.Type _type;

  public BBObjectParameter() => this.SetType(typeof (object));

  public BBObjectParameter(System.Type t) => this.SetType(t);

  public override System.Type varType => this._type;

  public void SetType(System.Type t)
  {
    if (System.Type.op_Equality(t, (System.Type) null))
      t = typeof (object);
    if (System.Type.op_Inequality(t, this._type))
      this._value = t.RTIsValueType() ? Activator.CreateInstance(t) : (object) null;
    this._type = t;
  }
}
