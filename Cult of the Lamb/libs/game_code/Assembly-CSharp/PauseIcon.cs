// Decompiled with JetBrains decompiler
// Type: PauseIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
