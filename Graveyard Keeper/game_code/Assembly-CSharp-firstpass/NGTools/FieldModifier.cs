// Decompiled with JetBrains decompiler
// Type: NGTools.FieldModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace NGTools;

public class FieldModifier : IFieldModifier, IValueGetter
{
  public FieldInfo fieldInfo;

  public Type Type => this.fieldInfo.FieldType;

  public string Name => this.fieldInfo.Name;

  public bool IsPublic => this.fieldInfo.IsPublic;

  public FieldModifier()
  {
  }

  public FieldModifier(FieldInfo fieldInfo) => this.fieldInfo = fieldInfo;

  public void SetValue(object instance, object value) => this.fieldInfo.SetValue(instance, value);

  public object GetValue(object instance) => this.fieldInfo.GetValue(instance);

  public T GetValue<T>(object instance) => (T) this.fieldInfo.GetValue(instance);

  public bool IsDefined(Type type, bool inherit) => this.fieldInfo.IsDefined(type, inherit);

  public object[] GetCustomAttributes(Type type, bool inherit)
  {
    return this.fieldInfo.GetCustomAttributes(type, inherit);
  }
}
