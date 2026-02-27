// Decompiled with JetBrains decompiler
// Type: Interaction_GhostVendor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_GhostVendor : Interaction
{
  [SerializeField]
  public SkeletonAnimation skeletonAnimation;
  [SerializeField]
  public GhostNPC ghost;
  [SerializeField]
  public CritterBee movement;
  [SerializeField]
  public Interaction_SimpleConversation purchaseConvo;
  [Space]
  [SerializeField]
  public int woolCost;
  [SerializeField]
  public bool rewardIsRelic;
  [SerializeField]
  public RelicType rewardRelic;
  [SerializeField]
  public bool rewardIsFleece;
  [SerializeField]
  public PlayerFleeceManager.FleeceType rewardFleece;
  [SerializeField]
  public bool rewardIsDeco;
  [SerializeField]
  public StructureBrain.TYPES rewardDeco;
  public bool interacting;

  public bool IsValid
  {
    get
    {
      return (!this.rewardIsRelic || !DataManager.Instance.PlayerFoundRelics.Contains(this.rewardRelic)) && (!this.rewardIsFleece || !DataManager.Instance.UnlockedFleeces.Contains((int) this.rewardFleece)) && (!this.rewardIsDeco || !DataManager.Instance.UnlockedStructures.Contains(this.rewardDeco));
    }
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.ghost.enabled = false;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.ghost.IsActive || this.ghost.IsRevealed)
      return;
    this.gameObject.SetActive(false);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = this.IsValid && this.ghost.IsHome;
    this.Label = this.Interactable ? $"{LocalizationManager.GetTranslation("Interactions/Give")} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.WOOL, this.woolCost)}" : "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) >= this.woolCost)
    {
      this.interacting = true;
      this.StartCoroutine(this.OnInteractIE());
    }
    else
      PlayerFarming.Instance.indicator.PlayShake();
    base.OnInteract(state);
  }

  public IEnumerator OnInteractIE()
  {
    Interaction_GhostVendor interactionGhostVendor = this;
    AudioManager.Instance.PlayOneShot("event:/shop/buy");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionGhostVendor.gameObject, 6f);
    interactionGhostVendor.playerFarming.GoToAndStop(interactionGhostVendor.transform.position + Vector3.right + Vector3.down, interactionGhostVendor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    interactionGhostVendor.playerFarming.EndGoToAndStop();
    for (int i = 0; i < 5; ++i)
    {
      ResourceCustomTarget.Create(interactionGhostVendor.gameObject, interactionGhostVendor._playerFarming.transform.position, InventoryItem.ITEM_TYPE.WOOL, (System.Action) null);
      yield return (object) new WaitForSeconds(0.2f);
    }
    interactionGhostVendor.purchaseConvo.Play();
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionGhostVendor.gameObject, 6f);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.WOOL, -interactionGhostVendor.woolCost);
    bool waiting = true;
    AudioManager.Instance.PlayOneShot("event:/player/new_item_reveal");
    if (interactionGhostVendor.rewardIsRelic)
      RelicCustomTarget.Create(interactionGhostVendor.skeletonAnimation.transform.position, interactionGhostVendor.playerFarming.transform.position, 1f, interactionGhostVendor.rewardRelic, (System.Action) (() => waiting = false));
    else if (interactionGhostVendor.rewardIsFleece)
    {
      DataManager.Instance.UnlockedFleeces.Add((int) interactionGhostVendor.rewardFleece);
      UIPlayerUpgradesMenuController upgradesMenuController1 = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
      upgradesMenuController1.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
      {
        interactionGhostVendor.rewardFleece
      }, true);
      UIPlayerUpgradesMenuController upgradesMenuController2 = upgradesMenuController1;
      upgradesMenuController2.OnHidden = upgradesMenuController2.OnHidden + (System.Action) (() => waiting = false);
    }
    else if (interactionGhostVendor.rewardIsDeco)
      DecorationCustomTarget.Create(interactionGhostVendor.skeletonAnimation.transform.position, interactionGhostVendor.playerFarming.transform.position, 1f, interactionGhostVendor.rewardDeco, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    DLCShrineRoomLocationManager.CheckWoolhavenCompleteAchievement();
    interactionGhostVendor.interacting = false;
    GameManager.GetInstance().OnConversationEnd();
    interactionGhostVendor.HasChanged = true;
    interactionGhostVendor.EndIndicateHighlighted(interactionGhostVendor.playerFarming);
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.IndicateHighlighted(playerFarming);
    if (!this.ghost.IsHome)
      return;
    this.ghost.enabled = true;
    this.movement.enabled = false;
    this.transform.localScale = new Vector3(-1f, 1f, 1f);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.EndIndicateHighlighted(playerFarming);
    if (!this.ghost.IsHome || this.interacting)
      return;
    this.ghost.enabled = false;
    this.movement.enabled = true;
    this.transform.localScale = Vector3.one;
    this.skeletonAnimation.transform.localScale = Vector3.one;
  }
}
