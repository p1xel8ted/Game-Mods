// Decompiled with JetBrains decompiler
// Type: ShowPlaceName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class ShowPlaceName : BaseMonoBehaviour
{
  [TermsPopup("")]
  public string PlaceName;

  public void Play() => HUD_DisplayName.Play(this.PlaceName);
}
