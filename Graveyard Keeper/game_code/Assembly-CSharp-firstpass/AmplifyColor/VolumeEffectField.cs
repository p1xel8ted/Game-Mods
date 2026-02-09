// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VolumeEffectField
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VolumeEffectField
{
  public string fieldName;
  public string fieldType;
  public float valueSingle;
  public Color valueColor;
  public bool valueBoolean;
  public Vector2 valueVector2;
  public Vector3 valueVector3;
  public Vector4 valueVector4;

  public VolumeEffectField(string fieldName, string fieldType)
  {
    this.fieldName = fieldName;
    this.fieldType = fieldType;
  }

  public VolumeEffectField(FieldInfo pi, Component c)
    : this(pi.Name, pi.FieldType.FullName)
  {
    this.UpdateValue(pi.GetValue((object) c));
  }

  public static bool IsValidType(string type)
  {
    switch (type)
    {
      case "System.Single":
      case "System.Boolean":
      case "UnityEngine.Color":
      case "UnityEngine.Vector2":
      case "UnityEngine.Vector3":
      case "UnityEngine.Vector4":
        return true;
      default:
        return false;
    }
  }

  public void UpdateValue(object val)
  {
    switch (this.fieldType)
    {
      case "System.Single":
        this.valueSingle = (float) val;
        break;
      case "System.Boolean":
        this.valueBoolean = (bool) val;
        break;
      case "UnityEngine.Color":
        this.valueColor = (Color) val;
        break;
      case "UnityEngine.Vector2":
        this.valueVector2 = (Vector2) val;
        break;
      case "UnityEngine.Vector3":
        this.valueVector3 = (Vector3) val;
        break;
      case "UnityEngine.Vector4":
        this.valueVector4 = (Vector4) val;
        break;
    }
  }
}
