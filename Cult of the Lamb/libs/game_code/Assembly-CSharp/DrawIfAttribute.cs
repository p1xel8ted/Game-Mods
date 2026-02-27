// Decompiled with JetBrains decompiler
// Type: DrawIfAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DrawIfAttribute : PropertyAttribute
{
  [CompilerGenerated]
  public string \u003CcomparedPropertyName\u003Ek__BackingField;
  [CompilerGenerated]
  public object \u003CcomparedValue\u003Ek__BackingField;
  [CompilerGenerated]
  public DrawIfAttribute.DisablingType \u003CdisablingType\u003Ek__BackingField;

  public string comparedPropertyName
  {
    get => this.\u003CcomparedPropertyName\u003Ek__BackingField;
    set => this.\u003CcomparedPropertyName\u003Ek__BackingField = value;
  }

  public object comparedValue
  {
    get => this.\u003CcomparedValue\u003Ek__BackingField;
    set => this.\u003CcomparedValue\u003Ek__BackingField = value;
  }

  public DrawIfAttribute.DisablingType disablingType
  {
    get => this.\u003CdisablingType\u003Ek__BackingField;
    set => this.\u003CdisablingType\u003Ek__BackingField = value;
  }

  public DrawIfAttribute(
    string comparedPropertyName,
    object comparedValue,
    DrawIfAttribute.DisablingType disablingType = DrawIfAttribute.DisablingType.DontDraw)
  {
    this.comparedPropertyName = comparedPropertyName;
    this.comparedValue = comparedValue;
    this.disablingType = disablingType;
  }

  public enum DisablingType
  {
    ReadOnly = 2,
    DontDraw = 3,
  }
}
