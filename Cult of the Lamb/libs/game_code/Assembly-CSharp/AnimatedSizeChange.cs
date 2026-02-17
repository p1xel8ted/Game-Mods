// Decompiled with JetBrains decompiler
// Type: AnimatedSizeChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public struct AnimatedSizeChange(float Size, float CurrentSize)
{
  [CompilerGenerated]
  public float \u003CSize\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CDifference\u003Ek__BackingField;

  public float Size
  {
    readonly get => this.\u003CSize\u003Ek__BackingField;
    set => this.\u003CSize\u003Ek__BackingField = value;
  }

  public float Difference
  {
    readonly get => this.\u003CDifference\u003Ek__BackingField;
    set => this.\u003CDifference\u003Ek__BackingField = value;
  }
}
