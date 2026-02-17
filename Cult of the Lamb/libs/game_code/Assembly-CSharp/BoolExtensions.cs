// Decompiled with JetBrains decompiler
// Type: BoolExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public static class BoolExtensions
{
  public static int ToInt(this bool b) => !b ? 0 : 1;
}
