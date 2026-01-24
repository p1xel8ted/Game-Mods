// Decompiled with JetBrains decompiler
// Type: StructureRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
