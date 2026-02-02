// Decompiled with JetBrains decompiler
// Type: ActivatePerArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
