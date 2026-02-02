// Decompiled with JetBrains decompiler
// Type: UISlideIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public int _price;

  public void setPrice(int price)
  {
    this._price = price;
    this.priceTxt.text = price.ToString();
    this.checkUnlocked();
  }

  public void checkUnlocked()
  {
    if (DataManager.Instance.Followers.Count < this._price)
      this.locked();
    else
      this.unlocked();
  }

  public void locked()
  {
    this.priceTxt.color = this.cantAffordColor;
    this.Locked.SetActive(true);
    this.Unlocked.SetActive(false);
  }

  public void unlocked()
  {
    this.priceTxt.color = this.cantAffordColor;
    this.Locked.SetActive(false);
    this.Unlocked.SetActive(true);
  }
}
