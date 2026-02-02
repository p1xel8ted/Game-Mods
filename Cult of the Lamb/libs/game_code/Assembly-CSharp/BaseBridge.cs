// Decompiled with JetBrains decompiler
// Type: BaseBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseBridge : BaseMonoBehaviour
{
  public GameObject BridgeFixed;
  public GameObject BridgeBroken;

  public void Start()
  {
    if (DataManager.Instance.BridgeFixed)
    {
      this.BridgeFixed.SetActive(true);
      this.BridgeBroken.SetActive(false);
    }
    else
    {
      this.BridgeFixed.SetActive(false);
      this.BridgeBroken.SetActive(true);
    }
  }

  public void FixBridge()
  {
    DataManager.Instance.BridgeFixed = true;
    this.BridgeFixed.SetActive(true);
    this.BridgeBroken.SetActive(false);
  }
}
