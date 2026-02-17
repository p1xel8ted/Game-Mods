// Decompiled with JetBrains decompiler
// Type: Structures_IceSculpture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_IceSculpture : Structures_IceBlock
{
  public bool IsFinished => (double) this.Data.Progress >= 1.0;

  public float Duration => 600f;
}
