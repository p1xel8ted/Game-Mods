// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VolumeEffectFieldFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VolumeEffectFieldFlags
{
  public string fieldName;
  public string fieldType;
  public bool blendFlag;

  public VolumeEffectFieldFlags(FieldInfo pi)
  {
    this.fieldName = pi.Name;
    this.fieldType = pi.FieldType.FullName;
  }

  public VolumeEffectFieldFlags(VolumeEffectField field)
  {
    this.fieldName = field.fieldName;
    this.fieldType = field.fieldType;
    this.blendFlag = true;
  }
}
