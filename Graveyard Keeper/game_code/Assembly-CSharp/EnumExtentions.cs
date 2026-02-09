// Decompiled with JetBrains decompiler
// Type: EnumExtentions
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
public static class EnumExtentions
{
  public static bool Is<T>(this T value, T flags) where T : struct
  {
    if (!typeof (T).IsEnum)
      throw new ArgumentException("The type parameter T must be an enum type.");
    return ((int) (ValueType) value & (int) (ValueType) flags) != 0;
  }

  public static bool IsNot<T>(this T value, T flags) where T : struct => !value.Is<T>(flags);
}
