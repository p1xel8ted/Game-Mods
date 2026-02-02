// Decompiled with JetBrains decompiler
// Type: BoolExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public static class BoolExtensions
{
  public static int ToInt(this bool b) => !b ? 0 : 1;
}
