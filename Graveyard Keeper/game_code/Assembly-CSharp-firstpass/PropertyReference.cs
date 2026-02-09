// Decompiled with JetBrains decompiler
// Type: PropertyReference
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

#nullable disable
[Serializable]
public class PropertyReference
{
  [SerializeField]
  public Component mTarget;
  [SerializeField]
  public string mName;
  public FieldInfo mField;
  public PropertyInfo mProperty;
  public static int s_Hash = "PropertyBinding".GetHashCode();

  public Component target
  {
    get => this.mTarget;
    set
    {
      this.mTarget = value;
      this.mProperty = (PropertyInfo) null;
      this.mField = (FieldInfo) null;
    }
  }

  public string name
  {
    get => this.mName;
    set
    {
      this.mName = value;
      this.mProperty = (PropertyInfo) null;
      this.mField = (FieldInfo) null;
    }
  }

  public bool isValid
  {
    get => (UnityEngine.Object) this.mTarget != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.mName);
  }

  public bool isEnabled
  {
    get
    {
      if ((UnityEngine.Object) this.mTarget == (UnityEngine.Object) null)
        return false;
      MonoBehaviour mTarget = this.mTarget as MonoBehaviour;
      return (UnityEngine.Object) mTarget == (UnityEngine.Object) null || mTarget.enabled;
    }
  }

  public PropertyReference()
  {
  }

  public PropertyReference(Component target, string fieldName)
  {
    this.mTarget = target;
    this.mName = fieldName;
  }

  public System.Type GetPropertyType()
  {
    if (PropertyInfo.op_Equality(this.mProperty, (PropertyInfo) null) && FieldInfo.op_Equality(this.mField, (FieldInfo) null) && this.isValid)
      this.Cache();
    if (PropertyInfo.op_Inequality(this.mProperty, (PropertyInfo) null))
      return this.mProperty.PropertyType;
    return FieldInfo.op_Inequality(this.mField, (FieldInfo) null) ? this.mField.FieldType : typeof (void);
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return !this.isValid;
    if (!(obj is PropertyReference))
      return false;
    PropertyReference propertyReference = obj as PropertyReference;
    return (UnityEngine.Object) this.mTarget == (UnityEngine.Object) propertyReference.mTarget && string.Equals(this.mName, propertyReference.mName);
  }

  public override int GetHashCode() => PropertyReference.s_Hash;

  public void Set(Component target, string methodName)
  {
    this.mTarget = target;
    this.mName = methodName;
  }

  public void Clear()
  {
    this.mTarget = (Component) null;
    this.mName = (string) null;
  }

  public void Reset()
  {
    this.mField = (FieldInfo) null;
    this.mProperty = (PropertyInfo) null;
  }

  public override string ToString() => PropertyReference.ToString(this.mTarget, this.name);

  public static string ToString(Component comp, string property)
  {
    if (!((UnityEngine.Object) comp != (UnityEngine.Object) null))
      return (string) null;
    string str = comp.GetType().ToString();
    int num = str.LastIndexOf('.');
    if (num > 0)
      str = str.Substring(num + 1);
    return !string.IsNullOrEmpty(property) ? $"{str}.{property}" : str + ".[property]";
  }

  [DebuggerHidden]
  [DebuggerStepThrough]
  public object Get()
  {
    if (PropertyInfo.op_Equality(this.mProperty, (PropertyInfo) null) && FieldInfo.op_Equality(this.mField, (FieldInfo) null) && this.isValid)
      this.Cache();
    if (PropertyInfo.op_Inequality(this.mProperty, (PropertyInfo) null))
    {
      if (this.mProperty.CanRead)
        return this.mProperty.GetValue((object) this.mTarget, (object[]) null);
    }
    else if (FieldInfo.op_Inequality(this.mField, (FieldInfo) null))
      return this.mField.GetValue((object) this.mTarget);
    return (object) null;
  }

  [DebuggerHidden]
  [DebuggerStepThrough]
  public bool Set(object value)
  {
    if (PropertyInfo.op_Equality(this.mProperty, (PropertyInfo) null) && FieldInfo.op_Equality(this.mField, (FieldInfo) null) && this.isValid)
      this.Cache();
    if (PropertyInfo.op_Equality(this.mProperty, (PropertyInfo) null) && FieldInfo.op_Equality(this.mField, (FieldInfo) null))
      return false;
    if (value == null)
    {
      try
      {
        if (PropertyInfo.op_Inequality(this.mProperty, (PropertyInfo) null))
        {
          if (this.mProperty.CanWrite)
          {
            this.mProperty.SetValue((object) this.mTarget, (object) null, (object[]) null);
            return true;
          }
        }
        else
        {
          this.mField.SetValue((object) this.mTarget, (object) null);
          return true;
        }
      }
      catch (Exception ex)
      {
        return false;
      }
    }
    if (!this.Convert(ref value))
    {
      if (Application.isPlaying)
        UnityEngine.Debug.LogError((object) $"Unable to convert {value.GetType()?.ToString()} to {this.GetPropertyType()?.ToString()}");
    }
    else
    {
      if (FieldInfo.op_Inequality(this.mField, (FieldInfo) null))
      {
        this.mField.SetValue((object) this.mTarget, value);
        return true;
      }
      if (this.mProperty.CanWrite)
      {
        this.mProperty.SetValue((object) this.mTarget, value, (object[]) null);
        return true;
      }
    }
    return false;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  public bool Cache()
  {
    if ((UnityEngine.Object) this.mTarget != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.mName))
    {
      System.Type type = this.mTarget.GetType();
      this.mField = type.GetField(this.mName);
      this.mProperty = type.GetProperty(this.mName);
    }
    else
    {
      this.mField = (FieldInfo) null;
      this.mProperty = (PropertyInfo) null;
    }
    return FieldInfo.op_Inequality(this.mField, (FieldInfo) null) || PropertyInfo.op_Inequality(this.mProperty, (PropertyInfo) null);
  }

  public bool Convert(ref object value)
  {
    if ((UnityEngine.Object) this.mTarget == (UnityEngine.Object) null)
      return false;
    System.Type propertyType = this.GetPropertyType();
    System.Type from;
    if (value == null)
    {
      if (!propertyType.IsClass)
        return false;
      from = propertyType;
    }
    else
      from = value.GetType();
    return PropertyReference.Convert(ref value, from, propertyType);
  }

  public static bool Convert(System.Type from, System.Type to)
  {
    object obj = (object) null;
    return PropertyReference.Convert(ref obj, from, to);
  }

  public static bool Convert(object value, System.Type to)
  {
    if (value != null)
      return PropertyReference.Convert(ref value, value.GetType(), to);
    value = (object) null;
    return PropertyReference.Convert(ref value, to, to);
  }

  public static bool Convert(ref object value, System.Type from, System.Type to)
  {
    if (to.IsAssignableFrom(from))
      return true;
    if (System.Type.op_Equality(to, typeof (string)))
    {
      value = value != null ? (object) value.ToString() : (object) "null";
      return true;
    }
    if (value == null)
      return false;
    if (System.Type.op_Equality(to, typeof (int)))
    {
      if (System.Type.op_Equality(from, typeof (string)))
      {
        int result;
        if (int.TryParse((string) value, out result))
        {
          value = (object) result;
          return true;
        }
      }
      else
      {
        if (System.Type.op_Equality(from, typeof (float)))
        {
          value = (object) Mathf.RoundToInt((float) value);
          return true;
        }
        if (System.Type.op_Equality(from, typeof (double)))
          value = (object) (int) Math.Round((double) value);
      }
    }
    else if (System.Type.op_Equality(to, typeof (float)))
    {
      if (System.Type.op_Equality(from, typeof (string)))
      {
        float result;
        if (float.TryParse((string) value, out result))
        {
          value = (object) result;
          return true;
        }
      }
      else if (System.Type.op_Equality(from, typeof (double)))
        value = (object) (float) (double) value;
      else if (System.Type.op_Equality(from, typeof (int)))
        value = (object) (float) (int) value;
    }
    else if (System.Type.op_Equality(to, typeof (double)))
    {
      if (System.Type.op_Equality(from, typeof (string)))
      {
        double result;
        if (double.TryParse((string) value, out result))
        {
          value = (object) result;
          return true;
        }
      }
      else if (System.Type.op_Equality(from, typeof (float)))
        value = (object) (double) (float) value;
      else if (System.Type.op_Equality(from, typeof (int)))
        value = (object) (double) (int) value;
    }
    return false;
  }
}
