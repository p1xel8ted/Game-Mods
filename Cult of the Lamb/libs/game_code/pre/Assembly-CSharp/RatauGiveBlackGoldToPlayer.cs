// Decompiled with JetBrains decompiler
// Type: RatauGiveBlackGoldToPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
    Debug.Log((object) ("i Cost: " + (object) cost));
    while (--cost >= 0)
    {
      Debug.Log((object) ("i " + (object) cost));
      if (cost == 0)
        ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(this.GiveGoldAndCallBack));
      else
        ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, this.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(this.GiveGold));
    }
  }

  private void GiveGoldAndCallBack()
  {
    Debug.Log((object) "A");
    this.GiveGold();
    this.Callback?.Invoke();
  }

  private void GiveGold()
  {
    Debug.Log((object) "B");
    Inventory.AddItem(20, 1);
  }
}
