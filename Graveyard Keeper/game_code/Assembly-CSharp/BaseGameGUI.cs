// Decompiled with JetBrains decompiler
// Type: BaseGameGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BaseGameGUI : BaseGUI
{
  public virtual void OpenFromGameGUI() => this.Open();

  public virtual void CloseFromGameGUI() => this.OnClosePressed();
}
