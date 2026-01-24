// Decompiled with JetBrains decompiler
// Type: GoopBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class GoopBomb : PoisonBomb, ISpellOwning
{
  public override void BombLanded()
  {
    AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_impact", this.gameObject);
    base.BombLanded();
  }
}
