// Decompiled with JetBrains decompiler
// Type: ActivatePerArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
