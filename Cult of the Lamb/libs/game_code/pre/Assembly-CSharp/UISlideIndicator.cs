// Decompiled with JetBrains decompiler
// Type: UISlideIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class UISlideIndicator : BaseMonoBehaviour
{
  public TextMeshProUGUI priceTxt;
  public GameObject Locked;
  public GameObject Unlocked;
  public Color canAffordColor;
  public Color cantAffordColor;
  private int _price;

  public void setPrice(int price)
  {
    this._price = price;
    this.priceTxt.text = price.ToString();
    this.checkUnlocked();
  }

  private void checkUnlocked()
  {
    if (DataManager.Instance.Followers.Count < this._price)
      this.locked();
    else
      this.unlocked();
  }

  private void locked()
  {
    this.priceTxt.color = this.cantAffordColor;
    this.Locked.SetActive(true);
    this.Unlocked.SetActive(false);
  }

  private void unlocked()
  {
    this.priceTxt.color = this.cantAffordColor;
    this.Locked.SetActive(false);
    this.Unlocked.SetActive(true);
  }
}
