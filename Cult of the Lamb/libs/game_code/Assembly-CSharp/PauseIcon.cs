// Decompiled with JetBrains decompiler
// Type: PauseIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LightShaft.Scripts;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PauseIcon : MonoBehaviour
{
  public YoutubePlayer p;
  public Image playImage;
  public Image pauseImage;

  public void FixedUpdate()
  {
    if (this.p.pauseCalled)
    {
      this.playImage.gameObject.SetActive(true);
      this.pauseImage.gameObject.SetActive(false);
    }
    else
    {
      this.pauseImage.gameObject.SetActive(true);
      this.playImage.gameObject.SetActive(false);
    }
  }
}
