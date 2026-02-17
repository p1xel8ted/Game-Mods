// Decompiled with JetBrains decompiler
// Type: PauseIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
