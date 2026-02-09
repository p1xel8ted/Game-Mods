// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Variable`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[Serializable]
public class Variable<T> : Variable
{
  [SerializeField]
  public T _value;
  [SerializeField]
  public string _propertyPath;
  public Func<T> getter;
  public Action<T> setter;

  public override string propertyPath
  {
    get => this._propertyPath;
    set => this._propertyPath = value;
  }

  public override bool hasBinding => !string.IsNullOrEmpty(this._propertyPath);

  public override object objectValue
  {
    get => (object) this.value;
    set => this.value = (T) value;
  }

  public override System.Type varType => typeof (T);

  public T value
  {
    get => this.getter == null ? this._value : this.getter();
    set
    {
      if (this.HasValueChangeEvent())
      {
        if (object.Equals((object) this._value, (object) value))
          return;
        this._value = value;
        if (this.setter != null)
          this.setter(value);
        this.OnValueChanged(this.name, (object) value);
      }
      else if (this.setter != null)
        this.setter(value);
      else
        this._value = value;
    }
  }

  public T GetValue() => this.value;

  public void SetValue(T newValue) => this.value = newValue;

  public override void BindProperty(MemberInfo prop, GameObject target = null)
  {
    switch (prop)
    {
      case PropertyInfo _:
      case FieldInfo _:
        this._propertyPath = $"{prop.RTReflectedType().FullName}.{prop.Name}";
        if (!((UnityEngine.Object) target != (UnityEngine.Object) null))
          break;
        this.InitializePropertyBinding(target, false);
        break;
    }
  }

  public override void UnBindProperty()
  {
    this._propertyPath = (string) null;
    this.getter = (Func<T>) null;
    this.setter = (Action<T>) null;
  }

  public override void InitializePropertyBinding(GameObject go, bool callSetter = false)
  {
    if (!this.hasBinding || !Application.isPlaying)
      return;
    this.getter = (Func<T>) null;
    this.setter = (Action<T>) null;
    int length = this._propertyPath.LastIndexOf('.');
    string typeFullName = this._propertyPath.Substring(0, length);
    string name = this._propertyPath.Substring(length + 1);
    System.Type type = ReflectionTools.GetType(typeFullName, true);
    if (System.Type.op_Equality(type, (System.Type) null))
    {
      Debug.LogError((object) $"Type '{typeFullName}' not found for Blackboard Variable '{this.name}' Binding", (UnityEngine.Object) go);
    }
    else
    {
      PropertyInfo property = type.RTGetProperty(name);
      if (PropertyInfo.op_Inequality(property, (PropertyInfo) null))
      {
        MethodInfo getMethod = property.RTGetGetMethod();
        MethodInfo setMethod = property.RTGetSetMethod();
        bool flag = MethodInfo.op_Inequality(getMethod, (MethodInfo) null) && getMethod.IsStatic || MethodInfo.op_Inequality(setMethod, (MethodInfo) null) && setMethod.IsStatic;
        Component instance = flag ? (Component) null : go.GetComponent(type);
        if ((UnityEngine.Object) instance == (UnityEngine.Object) null && !flag)
        {
          Debug.LogError((object) $"A Blackboard Variable '{this.name}' is due to bind to a Component type that is missing '{typeFullName}'. Binding ignored");
        }
        else
        {
          if (property.CanRead)
          {
            try
            {
              this.getter = getMethod.RTCreateDelegate<Func<T>>((object) instance);
            }
            catch
            {
              this.getter = (Func<T>) (() => (T) getMethod.Invoke((object) instance, (object[]) null));
            }
          }
          else
            this.getter = (Func<T>) (() =>
            {
              Debug.LogError((object) $"You tried to Get a Property Bound Variable '{this.name}', but the Bound Property '{this._propertyPath}' is Write Only!");
              return default (T);
            });
          if (property.CanWrite)
          {
            try
            {
              this.setter = setMethod.RTCreateDelegate<Action<T>>((object) instance);
            }
            catch
            {
              this.setter = (Action<T>) (o => setMethod.Invoke((object) instance, new object[1]
              {
                (object) o
              }));
            }
            if (!callSetter)
              return;
            this.setter(this._value);
          }
          else
            this.setter = (Action<T>) (o => Debug.LogError((object) $"You tried to Set a Property Bound Variable '{this.name}', but the Bound Property '{this._propertyPath}' is Read Only!"));
        }
      }
      else
      {
        FieldInfo field = type.RTGetField(name);
        if (FieldInfo.op_Inequality(field, (FieldInfo) null))
        {
          Component instance = field.IsStatic ? (Component) null : go.GetComponent(type);
          if ((UnityEngine.Object) instance == (UnityEngine.Object) null && !field.IsStatic)
            Debug.LogError((object) $"A Blackboard Variable '{this.name}' is due to bind to a Component type that is missing '{typeFullName}'. Binding ignored");
          else if (field.IsReadOnly())
          {
            T value = (T) field.GetValue((object) instance);
            this.getter = (Func<T>) (() => value);
          }
          else
          {
            this.getter = (Func<T>) (() => (T) field.GetValue((object) instance));
            this.setter = (Action<T>) (o => field.SetValue((object) instance, (object) o));
          }
        }
        else
          Debug.LogError((object) $"A Blackboard Variable '{this.name}' is due to bind to a property/field named '{name}' that does not exist on type '{type.FullName}'. Binding ignored");
      }
    }
  }
}
