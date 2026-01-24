// Decompiled with JetBrains decompiler
// Type: TarotCardDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TarotCardDisplay : MonoBehaviour
{
  public UITarotDisplay tarotDisplay;
  public Interaction_BuyItem buyItem;
  [SerializeField]
  public Vector3 _offset = new Vector3(0.0f, 250f, 0.0f);
  [SerializeField]
  public SimpleBark _bark;

  public void Awake() => this.buyItem = this.GetComponent<Interaction_BuyItem>();

  public void Update()
  {
    if ((Object) this._bark != (Object) null && this._bark.isActiveAndEnabled)
    {
      if (!((Object) this.tarotDisplay != (Object) null))
        return;
      Object.Destroy((Object) this.tarotDisplay.gameObject);
      this.tarotDisplay = (UITarotDisplay) null;
    }
    else if ((Object) PlayerFarming.Instance != (Object) null && (Object) PlayerFarming.Instance.interactor.CurrentInteraction == (Object) this.buyItem && !PlayerFarming.Instance.GoToAndStopping && !LetterBox.IsPlaying && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
    {
      if (!((Object) this.tarotDisplay == (Object) null))
        return;
      AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
      this.tarotDisplay = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Tarot Pickup"), GameObject.FindWithTag("Canvas").transform).GetComponent<UITarotDisplay>();
      this.tarotDisplay.offset = this._offset;
      this.tarotDisplay.Play(this.buyItem.itemForSale.Card, this.gameObject, PlayerFarming.Instance);
    }
    else
    {
      if (!((Object) this.tarotDisplay != (Object) null))
        return;
      Object.Destroy((Object) this.tarotDisplay.gameObject);
      this.tarotDisplay = (UITarotDisplay) null;
    }
  }

  public void OnDestroy()
  {
    if (!((Object) this.tarotDisplay != (Object) null))
      return;
    Object.Destroy((Object) this.tarotDisplay.gameObject);
    this.tarotDisplay = (UITarotDisplay) null;
  }
}
