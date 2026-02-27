// Decompiled with JetBrains decompiler
// Type: DecorationDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DecorationDisplay : MonoBehaviour
{
  private UIDecorationDisplay decorationDisplay;
  private Interaction_BuyItem buyItem;

  private void Awake() => this.buyItem = this.GetComponent<Interaction_BuyItem>();

  private void Update()
  {
    if ((Object) PlayerFarming.Instance != (Object) null && (Object) Interactor.CurrentInteraction == (Object) this.buyItem && !PlayerFarming.Instance.GoToAndStopping && !LetterBox.IsPlaying && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
    {
      if (!((Object) this.decorationDisplay == (Object) null))
        return;
      AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
      this.decorationDisplay = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Decoration Pickup"), GameObject.FindWithTag("Canvas").transform).GetComponent<UIDecorationDisplay>();
      this.decorationDisplay.Play(this.buyItem.itemForSale.decorationToBuy, this.gameObject);
    }
    else
    {
      if (!((Object) this.decorationDisplay != (Object) null))
        return;
      Object.Destroy((Object) this.decorationDisplay.gameObject);
      this.decorationDisplay = (UIDecorationDisplay) null;
    }
  }

  private void OnDestroy()
  {
    if (!((Object) this.decorationDisplay != (Object) null))
      return;
    Object.Destroy((Object) this.decorationDisplay.gameObject);
    this.decorationDisplay = (UIDecorationDisplay) null;
  }
}
