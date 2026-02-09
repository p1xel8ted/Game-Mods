// Decompiled with JetBrains decompiler
// Type: GoopBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
