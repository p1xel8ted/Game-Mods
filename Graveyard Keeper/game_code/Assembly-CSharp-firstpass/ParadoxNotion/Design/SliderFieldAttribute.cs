// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Design.SliderFieldAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Design;

[AttributeUsage(AttributeTargets.Field)]
public class SliderFieldAttribute : Attribute
{
  public float left;
  public float right;

  public SliderFieldAttribute(float left, float right)
  {
    this.left = left;
    this.right = right;
  }

  public SliderFieldAttribute(int left, int right)
  {
    this.left = (float) left;
    this.right = (float) right;
  }
}
