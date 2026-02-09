// Decompiled with JetBrains decompiler
// Type: LiveStreamingGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class LiveStreamingGUI : BaseMenuGUI
{
  public bool _mixer;
  public SimpleUITable table;
  public MenuItemGUI btn_mixer;
  public MenuItemGUI btn_twitch;

  public override void Open()
  {
    base.Open();
    this.Redraw();
  }

  public void Redraw() => this.btn_mixer.additional_go.SetActive(this._mixer);

  public void OnBackBtnClicked() => this.OnClosePressed();

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    base.OnClosePressed();
    GUIElements.me.main_menu.Open(false);
  }

  public void OnPressedMixer()
  {
    if (!MixerLightIntegration.IsAvailable())
      return;
    this._mixer = !this._mixer;
    MixerLightIntegration.ApplyEnableMode(this._mixer ? 1 : 0);
    this.Redraw();
  }
}
