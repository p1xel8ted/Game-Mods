// Decompiled with JetBrains decompiler
// Type: GoopBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
