// Decompiled with JetBrains decompiler
// Type: NGTools.PropertyModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace NGTools;

public class PropertyModifier : IFieldModifier, IValueGetter
{
  public PropertyInfo propertyInfo;

  public Type Type => this.propertyInfo.PropertyType;

  public string Name => this.propertyInfo.Name;

  public bool IsPublic => this.propertyInfo.CanRead;

  public PropertyModifier(PropertyInfo propertyInfo) => this.propertyInfo = propertyInfo;

  public void SetValue(object instance, object value)
  {
    this.propertyInfo.SetValue(instance, value, (object[]) null);
  }

  public object GetValue(object instance) => this.propertyInfo.GetValue(instance, (object[]) null);

  public T GetValue<T>(object instance)
  {
    return (T) this.propertyInfo.GetValue(instance, (object[]) null);
  }

  public bool IsDefined(Type type, bool inherit) => this.propertyInfo.IsDefined(type, inherit);

  public object[] GetCustomAttributes(Type type, bool inherit)
  {
    return this.propertyInfo.GetCustomAttributes(type, inherit);
  }
}
