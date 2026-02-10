// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.Preconditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Unity.Cloud;

public static class Preconditions
{
  public static void ArgumentIsLessThanOrEqualToLength(
    object value,
    int length,
    string argumentName)
  {
    if (value is string str && str.Length > length)
      throw new ArgumentException(argumentName);
  }

  public static void ArgumentNotNullOrWhitespace(object value, string argumentName)
  {
    if (value == null)
      throw new ArgumentNullException(argumentName);
    if (value is string str && str.Trim() == string.Empty)
      throw new ArgumentNullException(argumentName);
  }
}
