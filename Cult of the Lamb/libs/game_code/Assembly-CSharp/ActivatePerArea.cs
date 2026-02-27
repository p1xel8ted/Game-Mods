// Decompiled with JetBrains decompiler
// Type: ActivatePerArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
