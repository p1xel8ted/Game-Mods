// Decompiled with JetBrains decompiler
// Type: SwitchImageForPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SwitchImageForPlatform : MonoBehaviour
{
  public Sprite PC;
  public Sprite SWITCH;
  public Sprite XBOX;
  public Sprite PLAYSTATION5;
  public Sprite PLAYSTATION4;

  public void OnEnable()
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.PS4:
        this.GetComponent<Image>().sprite = this.PLAYSTATION4;
        break;
      case UnifyManager.Platform.Switch:
        this.GetComponent<Image>().sprite = this.SWITCH;
        break;
      case UnifyManager.Platform.GameCore:
      case UnifyManager.Platform.GameCoreConsole:
        this.GetComponent<Image>().sprite = this.XBOX;
        break;
      case UnifyManager.Platform.PS5:
        this.GetComponent<Image>().sprite = this.PLAYSTATION5;
        break;
      default:
        this.GetComponent<Image>().sprite = this.PC;
        break;
    }
  }
}
