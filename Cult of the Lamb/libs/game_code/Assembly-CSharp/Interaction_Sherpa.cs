// Decompiled with JetBrains decompiler
// Type: Interaction_Sherpa
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Sherpa : Interaction_Follower
{
  public bool PaidSherpa;
  public int SherpaCost = 2;
  public SkeletonAnimation Spine;

  public override void Start()
  {
    base.Start();
    if (!DataManager.Instance.SherpaFirstConvo)
      this.AutomaticallyInteract = true;
    string ForceSkin = "";
    if (DataManager.Instance.Followers.Count > 0)
      ForceSkin = DataManager.Instance.Followers[UnityEngine.Random.Range(0, DataManager.Instance.Followers.Count)].SkinName;
    Villager_Info v_i = Villager_Info.NewCharacter(ForceSkin);
    v_i.Outfit = WorshipperInfoManager.Outfit.Sherpa;
    this.wim.SetV_I(v_i);
  }

  public override void Update()
  {
    base.Update();
    this.Interactable = Inventory.itemsDungeon.Count > 0;
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (!DataManager.Instance.SherpaFirstConvo)
    {
      this.GetComponent<Interaction_SimpleConversation>().Play();
      this.AutomaticallyInteract = false;
      DataManager.Instance.SherpaFirstConvo = true;
    }
    else
    {
      if (this.Activated)
        return;
      if (Inventory.GetItemQuantity(20) >= this.SherpaCost)
      {
        this.StartCoroutine((IEnumerator) this.GiveLootIE());
        this.Activated = true;
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.playerFarming.gameObject);
        this.playerFarming.indicator.PlayShake();
      }
    }
  }

  public string GetAffordColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) > this.SherpaCost ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    if (Inventory.GetItemQuantity(20) < this.SherpaCost)
      return;
    this.Label = $"{this.GetAffordColor()}<sprite name=\"icon_blackgold\"> {LocalizeIntegration.FormatCurrentMax(Inventory.GetItemQuantity(20).ToString(), this.SherpaCost.ToString())}</color> - {ScriptLocalization.Interactions.TakeLoot}";
  }

  public IEnumerator GiveLootIE()
  {
    Interaction_Sherpa interactionSherpa = this;
    interactionSherpa.Interactable = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionSherpa.gameObject, 6f);
    for (int index = 0; index < interactionSherpa.SherpaCost; ++index)
    {
      Inventory.GetItemByType(20);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionSherpa.playerFarming.transform.position);
      ResourceCustomTarget.Create(interactionSherpa.gameObject, interactionSherpa.playerFarming.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      Inventory.ChangeItemQuantity(20, -1);
    }
    yield return (object) new WaitForSeconds(2f);
    foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionSherpa.playerFarming.transform.position);
      ResourceCustomTarget.Create(interactionSherpa.gameObject, interactionSherpa.playerFarming.transform.position, (InventoryItem.ITEM_TYPE) inventoryItem.type, (System.Action) null);
      Inventory.AddItem(inventoryItem.type, inventoryItem.quantity, true);
      yield return (object) new WaitForSeconds(0.2f);
    }
    Inventory.ClearDungeonItems();
    yield return (object) interactionSherpa.Spine.YieldForAnimation("Reactions/react-bow");
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionSherpa.playerFarming.transform.position);
    interactionSherpa.Spine.AnimationState.SetAnimation(0, "spawn-out", false);
    yield return (object) new WaitForSeconds(0.9f);
    CameraManager.shakeCamera(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionSherpa.gameObject);
  }
}
