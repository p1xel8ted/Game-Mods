// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VolumeEffectComponent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VolumeEffectComponent
{
  public string componentName;
  public List<VolumeEffectField> fields;

  public VolumeEffectComponent(string name)
  {
    this.componentName = name;
    this.fields = new List<VolumeEffectField>();
  }

  public VolumeEffectField AddField(FieldInfo pi, Component c) => this.AddField(pi, c, -1);

  public VolumeEffectField AddField(FieldInfo pi, Component c, int position)
  {
    VolumeEffectField volumeEffectField = VolumeEffectField.IsValidType(pi.FieldType.FullName) ? new VolumeEffectField(pi, c) : (VolumeEffectField) null;
    if (volumeEffectField != null)
    {
      if (position < 0 || position >= this.fields.Count)
        this.fields.Add(volumeEffectField);
      else
        this.fields.Insert(position, volumeEffectField);
    }
    return volumeEffectField;
  }

  public void RemoveEffectField(VolumeEffectField field) => this.fields.Remove(field);

  public VolumeEffectComponent(Component c, VolumeEffectComponentFlags compFlags)
    : this(compFlags.componentName)
  {
    foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields)
    {
      if (componentField.blendFlag)
      {
        FieldInfo field = c.GetType().GetField(componentField.fieldName);
        VolumeEffectField volumeEffectField = VolumeEffectField.IsValidType(field.FieldType.FullName) ? new VolumeEffectField(field, c) : (VolumeEffectField) null;
        if (volumeEffectField != null)
          this.fields.Add(volumeEffectField);
      }
    }
  }

  public void UpdateComponent(Component c, VolumeEffectComponentFlags compFlags)
  {
    foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields)
    {
      VolumeEffectFieldFlags fieldFlags = componentField;
      if (fieldFlags.blendFlag && !this.fields.Exists((Predicate<VolumeEffectField>) (s => s.fieldName == fieldFlags.fieldName)))
      {
        FieldInfo field = c.GetType().GetField(fieldFlags.fieldName);
        VolumeEffectField volumeEffectField = VolumeEffectField.IsValidType(field.FieldType.FullName) ? new VolumeEffectField(field, c) : (VolumeEffectField) null;
        if (volumeEffectField != null)
          this.fields.Add(volumeEffectField);
      }
    }
  }

  public VolumeEffectField FindEffectField(string fieldName)
  {
    for (int index = 0; index < this.fields.Count; ++index)
    {
      if (this.fields[index].fieldName == fieldName)
        return this.fields[index];
    }
    return (VolumeEffectField) null;
  }

  public static FieldInfo[] ListAcceptableFields(Component c)
  {
    return (UnityEngine.Object) c == (UnityEngine.Object) null ? new FieldInfo[0] : ((IEnumerable<FieldInfo>) c.GetType().GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (f => VolumeEffectField.IsValidType(f.FieldType.FullName))).ToArray<FieldInfo>();
  }

  public string[] GetFieldNames()
  {
    return this.fields.Select<VolumeEffectField, string>((Func<VolumeEffectField, string>) (r => r.fieldName)).ToArray<string>();
  }
}
