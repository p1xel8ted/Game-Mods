// Decompiled with JetBrains decompiler
// Type: Inventory_Icon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Inventory_Icon : BaseMonoBehaviour
{
  private float scale;
  private float scaleSpeed;
  public float Angle;
  private float Distance;
  private float DistanceSpeed;
  public float TargetDistance;
  public float Delay;
  private float TargetScale = 1f;
  public RectTransform rectTransform;
  public Vector3 TargetLocation;
  public Text QuantityText;
  public InventoryItemDisplay ItemDisplay;
  private InventoryItem item;

  private void Start()
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

  private void Update()
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
