// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Variable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
[Serializable]
public abstract class Variable
{
  [SerializeField]
  public string _name;
  [SerializeField]
  public string _id;
  [SerializeField]
  public bool _protected;

  public event Action<string> onNameChanged;

  public event Action<string, object> onValueChanged;

  public string name
  {
    get => this._name;
    set
    {
      if (!(this._name != value))
        return;
      this._name = value;
      if (this.onNameChanged == null)
        return;
      this.onNameChanged(value);
    }
  }

  public string ID
  {
    get
    {
      if (string.IsNullOrEmpty(this._id))
        this._id = Guid.NewGuid().ToString();
      return this._id;
    }
  }

  public object value
  {
    get => this.objectValue;
    set => this.objectValue = value;
  }

  public bool isProtected
  {
    get => this._protected;
    set => this._protected = value;
  }

  public bool HasValueChangeEvent() => this.onValueChanged != null;

  public void OnValueChanged(string name, object value) => this.onValueChanged(name, value);

  public abstract object objectValue { get; set; }

  public abstract System.Type varType { get; }

  public abstract bool hasBinding { get; }

  public abstract string propertyPath { get; set; }

  public abstract void BindProperty(MemberInfo prop, GameObject target = null);

  public abstract void UnBindProperty();

  public abstract void InitializePropertyBinding(GameObject go, bool callSetter = false);

  public bool CanConvertTo(System.Type toType) => this.GetGetConverter(toType) != null;

  public Func<object> GetGetConverter(System.Type toType)
  {
    if (toType.RTIsAssignableFrom(this.varType))
      return (Func<object>) (() => this.value);
    if (typeof (IConvertible).RTIsAssignableFrom(toType) && typeof (IConvertible).RTIsAssignableFrom(this.varType))
      return (Func<object>) (() =>
      {
        try
        {
          return Convert.ChangeType(this.value, toType);
        }
        catch
        {
          return !toType.RTIsAbstract() ? Activator.CreateInstance(toType) : (object) null;
        }
      });
    if (System.Type.op_Equality(toType, typeof (Transform)) && System.Type.op_Equality(this.varType, typeof (GameObject)))
      return (Func<object>) (() => this.value == null ? (object) null : (object) (this.value as GameObject).transform);
    if (System.Type.op_Equality(toType, typeof (GameObject)) && typeof (Component).RTIsAssignableFrom(this.varType))
      return (Func<object>) (() => this.value == null ? (object) null : (object) (this.value as Component).gameObject);
    if (System.Type.op_Equality(toType, typeof (Vector3)) && System.Type.op_Equality(this.varType, typeof (GameObject)))
      return (Func<object>) (() => (object) (this.value != null ? (this.value as GameObject).transform.position : Vector3.zero));
    if (System.Type.op_Equality(toType, typeof (Vector3)) && System.Type.op_Equality(this.varType, typeof (Transform)))
      return (Func<object>) (() => (object) (this.value != null ? (this.value as Transform).position : Vector3.zero));
    if (System.Type.op_Equality(toType, typeof (Vector3)) && System.Type.op_Equality(this.varType, typeof (Quaternion)))
      return (Func<object>) (() => (object) ((Quaternion) this.value).eulerAngles);
    if (System.Type.op_Equality(toType, typeof (Quaternion)) && System.Type.op_Equality(this.varType, typeof (Vector3)))
      return (Func<object>) (() => (object) Quaternion.Euler((Vector3) this.value));
    if (System.Type.op_Equality(toType, typeof (Vector3)) && System.Type.op_Equality(this.varType, typeof (Vector2)))
      return (Func<object>) (() => (object) (Vector3) (Vector2) this.value);
    return System.Type.op_Equality(toType, typeof (Vector2)) && System.Type.op_Equality(this.varType, typeof (Vector3)) ? (Func<object>) (() => (object) (Vector2) (Vector3) this.value) : (Func<object>) null;
  }

  public bool CanConvertFrom(System.Type fromType) => this.GetSetConverter(fromType) != null;

  public Action<object> GetSetConverter(System.Type fromType)
  {
    if (this.varType.RTIsAssignableFrom(fromType))
      return (Action<object>) (o => this.value = o);
    if (typeof (IConvertible).RTIsAssignableFrom(this.varType) && typeof (IConvertible).RTIsAssignableFrom(fromType))
      return (Action<object>) (o =>
      {
        try
        {
          this.value = Convert.ChangeType(o, this.varType);
        }
        catch
        {
          this.value = !this.varType.RTIsAbstract() ? Activator.CreateInstance(this.varType) : (object) null;
        }
      });
    if (System.Type.op_Equality(this.varType, typeof (Transform)) && System.Type.op_Equality(fromType, typeof (GameObject)))
      return (Action<object>) (o => this.value = o != null ? (object) (o as GameObject).transform : (object) (Transform) null);
    if (System.Type.op_Equality(this.varType, typeof (GameObject)) && typeof (Component).RTIsAssignableFrom(fromType))
      return (Action<object>) (o => this.value = o != null ? (object) (o as Component).gameObject : (object) (GameObject) null);
    if (System.Type.op_Equality(this.varType, typeof (GameObject)) && System.Type.op_Equality(fromType, typeof (Vector3)))
      return (Action<object>) (o =>
      {
        if (this.value == null)
          return;
        (this.value as GameObject).transform.position = (Vector3) o;
      });
    if (System.Type.op_Equality(this.varType, typeof (Transform)) && System.Type.op_Equality(fromType, typeof (Vector3)))
      return (Action<object>) (o =>
      {
        if (this.value == null)
          return;
        (this.value as Transform).position = (Vector3) o;
      });
    if (System.Type.op_Equality(this.varType, typeof (Vector3)) && System.Type.op_Equality(fromType, typeof (Quaternion)))
      return (Action<object>) (o => this.value = (object) ((Quaternion) o).eulerAngles);
    if (System.Type.op_Equality(this.varType, typeof (Quaternion)) && System.Type.op_Equality(fromType, typeof (Vector3)))
      return (Action<object>) (o => this.value = (object) Quaternion.Euler((Vector3) o));
    if (System.Type.op_Equality(fromType, typeof (Vector3)) && System.Type.op_Equality(this.varType, typeof (Vector2)))
      return (Action<object>) (o => this.value = (object) (Vector2) (Vector3) o);
    return System.Type.op_Equality(fromType, typeof (Vector2)) && System.Type.op_Equality(this.varType, typeof (Vector3)) ? (Action<object>) (o => this.value = (object) (Vector3) (Vector2) o) : (Action<object>) null;
  }

  public override string ToString() => this.name;

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_0(object o) => this.value = o;

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_1(object o)
  {
    try
    {
      this.value = Convert.ChangeType(o, this.varType);
    }
    catch
    {
      this.value = !this.varType.RTIsAbstract() ? Activator.CreateInstance(this.varType) : (object) null;
    }
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_2(object o)
  {
    this.value = o != null ? (object) (o as GameObject).transform : (object) (Transform) null;
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_3(object o)
  {
    this.value = o != null ? (object) (o as Component).gameObject : (object) (GameObject) null;
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_4(object o)
  {
    if (this.value == null)
      return;
    (this.value as GameObject).transform.position = (Vector3) o;
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_5(object o)
  {
    if (this.value == null)
      return;
    (this.value as Transform).position = (Vector3) o;
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_6(object o)
  {
    this.value = (object) ((Quaternion) o).eulerAngles;
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_7(object o)
  {
    this.value = (object) Quaternion.Euler((Vector3) o);
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_8(object o)
  {
    this.value = (object) (Vector2) (Vector3) o;
  }

  [CompilerGenerated]
  public void \u003CGetSetConverter\u003Eb__39_9(object o)
  {
    this.value = (object) (Vector3) (Vector2) o;
  }
}
