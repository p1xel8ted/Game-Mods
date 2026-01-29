// Decompiled with JetBrains decompiler
// Type: ShowPlaceName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class ShowPlaceName : BaseMonoBehaviour
{
  [TermsPopup("")]
  public string PlaceName;

  public void Play() => HUD_DisplayName.Play(this.PlaceName);
}
