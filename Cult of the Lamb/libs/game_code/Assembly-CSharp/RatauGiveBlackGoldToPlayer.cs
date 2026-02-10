// Decompiled with JetBrains decompiler
// Type: RatauGiveBlackGoldToPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class RatauGiveBlackGoldToPlayer : BaseMonoBehaviour
{
  public UnityEvent Callback;
  public Interaction_Follower interaction_Follower;

  public void Play()
  {
    int cost = this.interaction_Follower.Cost;
    Debug.Log((object) ("i Cost: " + cost.ToString()));
    while (--cost >= 0)
    {
      Debug.Log((object) ("i " + cost.ToString()));
      if (cost == 0)
        ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(this.GiveGoldAndCallBack));
      else
        ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(this.GiveGold));
    }
  }

  public void GiveGoldAndCallBack()
  {
    Debug.Log((object) "A");
    this.GiveGold();
    this.Callback?.Invoke();
  }

  public void GiveGold()
  {
    Debug.Log((object) "B");
    Inventory.AddItem(20, 1);
  }
}
