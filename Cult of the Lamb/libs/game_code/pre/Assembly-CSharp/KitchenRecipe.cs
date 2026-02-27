// Decompiled with JetBrains decompiler
// Type: KitchenRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KitchenRecipe : BaseMonoBehaviour
{
  public TextMeshProUGUI RecipeText;
  private GameObject LockPosition;
  public Vector3 Offset;
  private Canvas canvas;
  private StructuresData kitchenData;
  public CanvasGroup canvasGroup;
  private RectTransform rectTransform;
  private Camera mainCamera;
  public List<Image> PieChartPiece = new List<Image>();
  public List<float> PieChartPieceValues = new List<float>();
  public List<InventoryItem> LocalInventory = new List<InventoryItem>();
  private bool Hiding = true;

  private void Awake() => this.mainCamera = Camera.main;

  public void Play(GameObject LockPosition, StructuresData kitchenData)
  {
    this.LockPosition = LockPosition;
    this.canvas = this.GetComponentInParent<Canvas>();
    this.rectTransform = this.GetComponent<RectTransform>();
    this.kitchenData = kitchenData;
    this.gameObject.SetActive(true);
    this.Hiding = false;
    int index = -1;
    while (++index < this.PieChartPiece.Count)
      this.PieChartPiece[index].fillAmount = 0.0f;
  }

  private InventoryItem GetItem(int type)
  {
    foreach (InventoryItem inventoryItem in this.LocalInventory)
    {
      if (inventoryItem.type == type)
        return inventoryItem;
    }
    return (InventoryItem) null;
  }

  private void GetRecipe()
  {
    float num1 = 0.0f;
    this.LocalInventory = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem1 in this.kitchenData.Inventory)
    {
      InventoryItem inventoryItem2 = this.GetItem(inventoryItem1.type);
      if (inventoryItem2 != null)
      {
        inventoryItem2.quantity += inventoryItem1.quantity;
      }
      else
      {
        InventoryItem inventoryItem3 = new InventoryItem();
        inventoryItem3.Init(inventoryItem1.type, inventoryItem1.quantity);
        this.LocalInventory.Add(inventoryItem3);
      }
      num1 += (float) inventoryItem1.quantity;
    }
    this.PieChartPieceValues = new List<float>();
    int index = -1;
    float num2 = 0.0f;
    while (++index < this.LocalInventory.Count)
    {
      num2 += (float) this.LocalInventory[index].quantity / num1;
      this.PieChartPieceValues.Add(num2);
    }
    if (this.LocalInventory.Count <= 0)
    {
      this.RecipeText.text = "Empty";
    }
    else
    {
      int num3 = int.MinValue;
      InventoryItem.ITEM_TYPE Type = InventoryItem.ITEM_TYPE.MEAT;
      foreach (InventoryItem inventoryItem in this.LocalInventory)
      {
        if (inventoryItem.quantity > num3)
        {
          num3 = inventoryItem.quantity;
          Type = (InventoryItem.ITEM_TYPE) inventoryItem.type;
        }
      }
      this.RecipeText.text = InventoryItem.Name(Type) + " Stew";
    }
  }

  private void Update()
  {
    if ((Object) this.LockPosition != (Object) null)
      this.rectTransform.position = this.mainCamera.WorldToScreenPoint(this.LockPosition.transform.position) + this.Offset * this.canvas.scaleFactor;
    int index = -1;
    while (++index < this.PieChartPieceValues.Count)
      this.PieChartPiece[index].fillAmount = Mathf.Lerp(this.PieChartPiece[index].fillAmount, this.PieChartPieceValues[index], Time.deltaTime * 10f);
    if (!this.Hiding)
    {
      this.GetRecipe();
      if ((double) this.canvasGroup.alpha >= 1.0)
        return;
      this.canvasGroup.alpha += Time.deltaTime * 5f;
    }
    else if ((double) this.canvasGroup.alpha > 0.0)
    {
      this.canvasGroup.alpha -= 5f * Time.deltaTime;
    }
    else
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
    }
  }

  public void Hide() => this.Hiding = true;
}
