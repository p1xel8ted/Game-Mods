// Decompiled with JetBrains decompiler
// Type: BaseBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseBridge : BaseMonoBehaviour
{
  public GameObject BridgeFixed;
  public GameObject BridgeBroken;

  private void Start()
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
