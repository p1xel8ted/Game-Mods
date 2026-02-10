// Decompiled with JetBrains decompiler
// Type: GoopBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
