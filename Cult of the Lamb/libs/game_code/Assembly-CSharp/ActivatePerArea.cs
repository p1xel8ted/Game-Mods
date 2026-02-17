// Decompiled with JetBrains decompiler
// Type: ActivatePerArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class ActivatePerArea : BaseMonoBehaviour
{
  public FollowerLocation ActiveLocation = FollowerLocation.None;
  public FollowerLocation currentLocation = FollowerLocation.None;

  public void OnEnable()
  {
    this.currentLocation = PlayerFarming.Location;
    if (this.currentLocation == this.ActiveLocation)
      this.gameObject.SetActive(true);
    else
      this.gameObject.SetActive(false);
  }
}
