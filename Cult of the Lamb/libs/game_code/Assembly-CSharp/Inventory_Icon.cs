// Decompiled with JetBrains decompiler
// Type: Inventory_Icon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Inventory_Icon : BaseMonoBehaviour
{
  public float scale;
  public float scaleSpeed;
  public float Angle;
  public float Distance;
  public float DistanceSpeed;
  public float TargetDistance;
  public float Delay;
  public float TargetScale = 1f;
  public RectTransform rectTransform;
  public Vector3 TargetLocation;
  public Text QuantityText;
  public InventoryItemDisplay ItemDisplay;
  public InventoryItem item;

  public void Start()
  {
    this.transform.localScale = Vector3.zero;
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  public void SetImage(int i, int quantity)
  {
    this.ItemDisplay.SetImage((InventoryItem.ITEM_TYPE) i);
    this.QuantityText.text = quantity.ToString();
  }

  public void SetItem(InventoryItem item) => this.item = item;

  public void Update()
  {
    if ((double) (this.Delay -= Time.deltaTime) > 0.0)
      return;
    this.scaleSpeed += (float) (((double) this.TargetScale - (double) this.scale) * 0.30000001192092896);
    this.scale += (this.scaleSpeed *= 0.7f);
    this.transform.localScale = new Vector3(this.scale, this.scale, 1f);
    this.DistanceSpeed += (float) (((double) this.TargetDistance - (double) this.Distance) * 0.40000000596046448);
    this.Distance += (this.DistanceSpeed *= 0.5f);
    this.rectTransform.localPosition = new Vector3(this.Distance * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)), this.Distance * Mathf.Sin(this.Angle * ((float) Math.PI / 180f)), 0.0f);
    this.TargetScale = !((UnityEngine.Object) HUD_Inventory.CURRENT_SELECTION == (UnityEngine.Object) this) ? 1f : 1.2f;
    if (this.item == null)
      return;
    this.SetImage(this.item.type, this.item.quantity);
  }
}
