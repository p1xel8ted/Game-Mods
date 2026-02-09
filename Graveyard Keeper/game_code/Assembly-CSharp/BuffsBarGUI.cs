// Decompiled with JetBrains decompiler
// Type: BuffsBarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BuffsBarGUI : BaseGUI
{
  public UILabel label;

  public override void Init() => base.Init();

  public void Redraw()
  {
    string str = "";
    if ((double) MainGame.me.player.sanity <= 50.0)
    {
      str = "* You lose part of your tech points";
    }
    else
    {
      double sanity1 = (double) MainGame.me.player.sanity;
    }
    double sanity2 = (double) MainGame.me.player.sanity;
    this.label.text = str;
  }

  public new void Update()
  {
    if (!MainGame.game_started)
      return;
    this.Redraw();
  }
}
