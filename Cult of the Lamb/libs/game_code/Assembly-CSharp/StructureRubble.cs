// Decompiled with JetBrains decompiler
// Type: StructureRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StructureRubble : MonoBehaviour
{
  [SerializeField]
  public GameObject stone;
  [SerializeField]
  public GameObject wood;
  [SerializeField]
  public GameObject big;

  public void Configure(InventoryItem.ITEM_TYPE rubbleType)
  {
    this.stone.gameObject.SetActive(rubbleType == InventoryItem.ITEM_TYPE.STONE);
    this.wood.gameObject.SetActive(rubbleType == InventoryItem.ITEM_TYPE.LOG);
  }

  public void SetBig() => this.big.gameObject.SetActive(true);
}
