// Decompiled with JetBrains decompiler
// Type: InventoryEconomyItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class InventoryEconomyItem : BaseMonoBehaviour
{
  public TextMeshProUGUI AmountOfResource;
  public TextMeshProUGUI ResourceIcon;
  public GameObject ProgressBar;
  public Image ProgressBar_Progress;
  private InventoryItem.ITEM_TYPE Type;
  private Coroutine cLerpBarRoutine;

  private void Start()
  {
  }

  public void Init(InventoryItem.ITEM_TYPE type)
  {
    this.Type = type;
    this.ResourceIcon.text = FontImageNames.GetIconWhiteByType(type);
    this.UpdateResources();
    if ((double) Inventory.GetResourceCapacity(type) != -1.0)
      return;
    this.ProgressBar.SetActive(false);
  }

  public void UpdateResources()
  {
    this.AmountOfResource.text = "";
    int num = 4 - Inventory.GetItemQuantity((int) this.Type).ToString().Length;
    string str = "<color=#6e6d69>0</color>";
    for (int index = 0; index < num; ++index)
      this.AmountOfResource.text = (this.AmountOfResource.text += str);
    this.AmountOfResource.text += Inventory.GetItemQuantity((int) this.Type).ToString();
    if (this.Type == InventoryItem.ITEM_TYPE.INGREDIENTS)
      this.AmountOfResource.text += "<size=15>Kj</size>";
    if (!this.ProgressBar.activeSelf)
      return;
    if ((double) this.ProgressBar_Progress.fillAmount != (double) Inventory.GetResourceCapacity(this.Type))
    {
      if (this.cLerpBarRoutine != null)
        this.StopCoroutine(this.cLerpBarRoutine);
      this.cLerpBarRoutine = this.StartCoroutine((IEnumerator) this.LerpBarRoutine());
    }
    if (!Inventory.CheckCapacityFull(this.Type))
      this.ProgressBar_Progress.color = StaticColors.RedColor;
    else
      this.ProgressBar_Progress.color = StaticColors.OffWhiteColor;
  }

  private IEnumerator LerpBarRoutine()
  {
    yield return (object) new WaitForSeconds(0.2f);
    float StartPosition = this.ProgressBar_Progress.fillAmount;
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.ProgressBar_Progress.fillAmount = Mathf.Lerp(StartPosition, Inventory.GetResourceCapacity(this.Type), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.ProgressBar_Progress.fillAmount = Inventory.GetResourceCapacity(this.Type);
  }
}
