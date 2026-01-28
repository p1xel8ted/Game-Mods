// Decompiled with JetBrains decompiler
// Type: EnableOnLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnableOnLocation : MonoBehaviour
{
  [SerializeField]
  public FollowerLocation location;
  public FollowerLocation debugLocation;
  [SerializeField]
  public bool disableInstead;

  public void OnEnable()
  {
    this.debugLocation = PlayerFarming.Location;
    if (!this.disableInstead)
    {
      if (PlayerFarming.Location == this.location)
        this.gameObject.SetActive(true);
      else
        this.gameObject.SetActive(false);
    }
    else if (PlayerFarming.Location != this.location)
      this.gameObject.SetActive(true);
    else
      this.gameObject.SetActive(false);
  }
}
