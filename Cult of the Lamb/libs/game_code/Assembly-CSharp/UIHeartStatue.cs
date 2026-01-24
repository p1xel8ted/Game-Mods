// Decompiled with JetBrains decompiler
// Type: UIHeartStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIHeartStatue : BaseMonoBehaviour
{
  [HideInInspector]
  public bool Upgrading;
  public RectTransform Bar;
  public List<GameObject> DiplayIcons = new List<GameObject>();
  public List<GameObject> LockIcons = new List<GameObject>();
  public static UIHeartStatue Instance;
  public GameObject canAffordButton;
  public GameObject cantAffordButton;
  public bool usesSlider;
  public UnityEngine.UI.Slider rangeSlider;
  public UISlideIndicator[] sliderIndicators;
  public int[] followerPrices;
  public Material normalMaterial;
  public Material BlackAndWhiteMaterial;
  public TextMeshProUGUI FollowerCount;
  public int lastPrice;

  public void OnEnable()
  {
    UIHeartStatue.Instance = this;
    this.UpdateFollowerCount();
    FollowerManager.OnFollowerAdded += new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
    FollowerManager.OnFollowerRemoved += new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
  }

  public void OnDisable()
  {
    UIHeartStatue.Instance = (UIHeartStatue) null;
    FollowerManager.OnFollowerAdded -= new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
    FollowerManager.OnFollowerRemoved -= new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
  }

  public void OnFollowerUpdated(int followerID) => UIHeartStatue.UpdateSpiritHearts();

  public void Start()
  {
    this.SetBarScale();
    this.UpdateDisplayIcons();
    this.UpdateFollowerCount();
    if (DataManager.Instance.ShrineHeart < 4)
      return;
    this.HideButton();
  }

  public virtual bool CanUpgrade() => DataManager.Instance.ShrineHeart < 4;

  public virtual void Repair() => DataManager.Instance.ShrineHeart = 1;

  public void UpdateFollowerCount()
  {
    if (!((Object) this.FollowerCount != (Object) null))
      return;
    if ((Object) this.rangeSlider != (Object) null)
    {
      this.lastPrice = this.followerPrices.Length - 1;
      this.rangeSlider.minValue = 0.0f;
      this.rangeSlider.maxValue = 100f;
      for (int index = 0; index <= this.lastPrice; ++index)
        this.sliderIndicators[index].setPrice(this.followerPrices[index]);
    }
    string translation = LocalizationManager.Sources[0].GetTranslation("UI/Generic/Followers");
    this.FollowerCount.text = $"<sprite name=\"icon_Followers\">{DataManager.Instance.Followers.Count.ToString()} {translation}";
  }

  public void SetBarScale()
  {
    if (this.usesSlider)
    {
      if ((Object) this.rangeSlider == (Object) null)
        return;
      float num1 = 0.0f;
      int index = -1;
      float num2 = 0.0f;
      while (++index < this.followerPrices.Length)
      {
        if (DataManager.Instance.Followers.Count < this.followerPrices[index])
        {
          float num3 = (float) DataManager.Instance.Followers.Count - num2;
          float num4 = (float) this.followerPrices[index] - num2;
          if ((double) num3 > 0.0)
            num1 += num3 / num4 * (float) (100 / (this.followerPrices.Length - 1));
        }
        else if (this.followerPrices[index] > 0)
          num1 += (float) (100 / (this.followerPrices.Length - 1));
        num2 = (float) this.followerPrices[index];
      }
      this.rangeSlider.value = num1;
    }
    else
      this.Bar.localScale = new Vector3(Mathf.Clamp((float) (((double) DataManager.Instance.Followers.Count - 1.0) / 6.0), 0.0f, 1f), 1f, 1f);
  }

  public virtual void Upgrade()
  {
    if (DataManager.Instance.ShrineHeart >= 4 || this.Upgrading)
      return;
    Debug.Log((object) "Upgrade!");
    this.StartCoroutine((IEnumerator) this.DoUpgrade());
  }

  public virtual IEnumerator DoUpgrade()
  {
    this.Upgrading = true;
    this.HideButton();
    DataManager.Instance.ShrineHeart = Mathf.Min(++DataManager.Instance.ShrineHeart, 4);
    Debug.Log((object) ("DataManager.Instance.ShrineHeart  " + DataManager.Instance.ShrineHeart.ToString()));
    UIHeartStatue.UpdateSpiritHearts();
    yield return (object) new WaitForSeconds(0.3f);
    if (DataManager.Instance.ShrineHeart < 4)
      this.ShowButton();
    this.Upgrading = false;
  }

  public static void UpdateSpiritHearts()
  {
    HealthPlayer objectOfType = Object.FindObjectOfType<HealthPlayer>();
    if ((Object) objectOfType == (Object) null)
      return;
    int count = DataManager.Instance.Followers.Count;
    int num = 0;
    switch (DataManager.Instance.ShrineHeart)
    {
      case 0:
        num = 0;
        break;
      case 1:
        if (count > 1)
        {
          num = 2;
          break;
        }
        if (count <= 1)
        {
          num = 0;
          break;
        }
        break;
      case 2:
        if (count > 1 && count <= 4)
        {
          num = 2;
          break;
        }
        if (count > 4)
        {
          num = 4;
          break;
        }
        if (count <= 1)
        {
          num = 0;
          break;
        }
        break;
      case 3:
        if (count > 1 && count <= 4)
        {
          num = 2;
          break;
        }
        if (count > 4 && count <= 9)
        {
          num = 4;
          break;
        }
        if (count > 9)
        {
          num = 6;
          break;
        }
        if (count <= 1)
        {
          num = 0;
          break;
        }
        break;
      case 4:
        if (count > 1 && count <= 4)
        {
          num = 2;
          break;
        }
        if (count > 4 && count <= 9)
        {
          num = 4;
          break;
        }
        if (count > 9 && count <= 14)
        {
          num = 6;
          break;
        }
        if (count > 14)
        {
          num = 8;
          break;
        }
        if (count <= 1)
        {
          num = 0;
          break;
        }
        break;
    }
    objectOfType.TotalSpiritHearts = (float) num;
    objectOfType.SpiritHearts = objectOfType.TotalSpiritHearts;
    UIHeartStatue.Instance?.UpdateDisplayIcons();
  }

  public virtual void UpdateDisplayIcons()
  {
    if (!((Object) UIHeartStatue.Instance != (Object) null))
      return;
    HealthPlayer objectOfType = Object.FindObjectOfType<HealthPlayer>();
    if ((Object) objectOfType == (Object) null)
      return;
    int index1 = -1;
    while (++index1 < UIHeartStatue.Instance.DiplayIcons.Count)
    {
      if ((double) index1 < (double) objectOfType.PLAYER_SPIRIT_TOTAL_HEARTS / 2.0)
        UIHeartStatue.Instance.DiplayIcons[index1].GetComponent<Image>().material = this.normalMaterial;
      else
        UIHeartStatue.Instance.DiplayIcons[index1].GetComponent<Image>().material = this.BlackAndWhiteMaterial;
    }
    if (UIHeartStatue.Instance.LockIcons == null)
      return;
    int index2 = -1;
    while (++index2 < UIHeartStatue.Instance.LockIcons.Count)
    {
      if (index2 < DataManager.Instance.ShrineHeart)
        this.LockIcons[index2].SetActive(false);
      else
        this.LockIcons[index2].SetActive(true);
    }
  }

  public void ShowButton()
  {
    this.canAffordButton.SetActive(true);
    this.cantAffordButton.SetActive(false);
  }

  public void HideButton()
  {
    this.canAffordButton.SetActive(false);
    this.cantAffordButton.SetActive(true);
  }
}
