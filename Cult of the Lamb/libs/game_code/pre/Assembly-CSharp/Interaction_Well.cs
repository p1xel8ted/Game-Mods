// Decompiled with JetBrains decompiler
// Type: Interaction_Well
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Well : Interaction
{
  public override void GetLabel()
  {
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (1 <= Inventory.GetItemQuantity(20))
    {
      this.StartCoroutine((IEnumerator) this.GiveGold());
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      MonoSingleton<Indicator>.Instance.PlayShake();
    }
  }

  private IEnumerator GiveGold()
  {
    Interaction_Well interactionWell = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionWell.state.gameObject, 5f);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
    ResourceCustomTarget.Create(interactionWell.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
    Inventory.ChangeItemQuantity(20, -1);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.25f, 0.5f);
    interactionWell.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
  }
}
