// Decompiled with JetBrains decompiler
// Type: DecorationDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DecorationDisplay : MonoBehaviour
{
  public UIDecorationDisplay decorationDisplay;
  public Interaction_BuyItem buyItem;
  [SerializeField]
  public Vector3 _offset = new Vector3(0.0f, 300f, 0.0f);

  public void Awake() => this.buyItem = this.GetComponent<Interaction_BuyItem>();

  public void Update()
  {
    if ((Object) PlayerFarming.Instance != (Object) null && (Object) PlayerFarming.Instance.interactor.CurrentInteraction == (Object) this.buyItem && !PlayerFarming.Instance.GoToAndStopping && !LetterBox.IsPlaying && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
    {
      if (!((Object) this.decorationDisplay == (Object) null))
        return;
      AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
      this.decorationDisplay = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Decoration Pickup"), GameObject.FindWithTag("Canvas").transform).GetComponent<UIDecorationDisplay>();
      this.decorationDisplay.offset = this._offset;
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

  public void OnDestroy()
  {
    if (!((Object) this.decorationDisplay != (Object) null))
      return;
    Object.Destroy((Object) this.decorationDisplay.gameObject);
    this.decorationDisplay = (UIDecorationDisplay) null;
  }
}
