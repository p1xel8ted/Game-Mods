// Decompiled with JetBrains decompiler
// Type: Interaction_PalworldEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_PalworldEgg : Interaction
{
  [SerializeField]
  public GameObject cantAffordBark;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = $"{string.Format(ScriptLocalization.Interactions.Buy, (object) "")}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.BLACK_GOLD, 50 * (DataManager.Instance.PalworldEggsCollected + 1))}";
  }

  public override void OnInteract(StateMachine state)
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) >= 50 * (DataManager.Instance.PalworldEggsCollected + 1))
    {
      base.OnInteract(state);
    }
    else
    {
      this.cantAffordBark.gameObject.SetActive(true);
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
    }
  }
}
