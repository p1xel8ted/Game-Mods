// Decompiled with JetBrains decompiler
// Type: EnableOnLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
