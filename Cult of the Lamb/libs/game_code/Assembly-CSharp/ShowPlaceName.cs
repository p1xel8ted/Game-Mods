// Decompiled with JetBrains decompiler
// Type: ShowPlaceName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class ShowPlaceName : BaseMonoBehaviour
{
  [TermsPopup("")]
  public string PlaceName;

  public void Play() => HUD_DisplayName.Play(this.PlaceName);
}
