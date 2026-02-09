// Decompiled with JetBrains decompiler
// Type: Snail_Interaction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Snail_Interaction : Interaction
{
  public SpriteRenderer Snail;
  public Sprite SnailUnlit;
  public Sprite SnailLit;
  public GameObject Lighting;
  public bool testDLCTrade;
  public InventoryItem.ITEM_TYPE DLCItemToTrade = InventoryItem.ITEM_TYPE.WOOL;
  public string DLCIconForItemToTrade = "<sprite name=\"icon_Wool\">";
  public InventoryItem.ITEM_TYPE DLCItemToReceive = InventoryItem.ITEM_TYPE.ANIMAL_SNAIL;
  public DataManager.Variables DLCTradeRequiresVariable = DataManager.Variables.OnboardedRanching;
  public int costOfDLCTrade = 25;
  public bool DLCTradeMode;
  public FollowerLocation DLCTradeLocation = FollowerLocation.HubShore;
  public int ShrineNumber = -1;

  public void Start() => this.CheckSprites();

  public void CheckSprites()
  {
    this.Snail.sprite = this.SnailUnlit;
    this.Lighting.SetActive(false);
    switch (this.ShrineNumber)
    {
      case 0:
        if (!DataManager.Instance.ShellsGifted_0)
          break;
        this.Snail.sprite = this.SnailLit;
        this.Lighting.SetActive(true);
        break;
      case 1:
        if (!DataManager.Instance.ShellsGifted_1)
          break;
        this.Snail.sprite = this.SnailLit;
        this.Lighting.SetActive(true);
        break;
      case 2:
        if (!DataManager.Instance.ShellsGifted_2)
          break;
        this.Snail.sprite = this.SnailLit;
        this.Lighting.SetActive(true);
        break;
      case 3:
        if (!DataManager.Instance.ShellsGifted_3)
          break;
        this.Snail.sprite = this.SnailLit;
        this.Lighting.SetActive(true);
        break;
      case 4:
        if (!DataManager.Instance.ShellsGifted_4)
          break;
        this.Snail.sprite = this.SnailLit;
        this.Lighting.SetActive(true);
        break;
    }
  }

  public string GetAffordColor()
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SHELL) >= 1)
    {
      this.Interactable = true;
      return "<color=#f4ecd3>";
    }
    this.Interactable = false;
    return "<color=red>";
  }

  public string GetAffordColorDLC()
  {
    if (Inventory.GetItemQuantity(this.DLCItemToTrade) >= this.costOfDLCTrade)
    {
      this.Interactable = true;
      return "<color=#f4ecd3>";
    }
    this.Interactable = false;
    return "<color=red>";
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.GetFollowerSkinUnlocked("Snail"))
    {
      if (PlayerFarming.Location == this.DLCTradeLocation && (this.DLCTradeRequiresVariable.Equals((object) true) || this.testDLCTrade))
      {
        this.Interactable = true;
        this.DLCTradeMode = true;
        this.Label = $"{ScriptLocalization.Interactions.MakeOffering} | {this.GetAffordColorDLC()}{this.DLCIconForItemToTrade}{LocalizeIntegration.FormatCurrentMax(Inventory.GetItemQuantity(this.DLCItemToTrade).ToString(), this.costOfDLCTrade.ToString() ?? "")}";
      }
      else
      {
        this.Label = "";
        this.Interactable = false;
      }
    }
    else
    {
      switch (this.ShrineNumber)
      {
        case 0:
          if (DataManager.Instance.ShellsGifted_0)
          {
            this.Interactable = false;
            this.Label = "";
            return;
          }
          break;
        case 1:
          if (DataManager.Instance.ShellsGifted_1)
          {
            this.Interactable = false;
            this.Label = "";
            return;
          }
          break;
        case 2:
          if (DataManager.Instance.ShellsGifted_2)
          {
            this.Interactable = false;
            this.Label = "";
            return;
          }
          break;
        case 3:
          if (DataManager.Instance.ShellsGifted_3)
          {
            this.Interactable = false;
            this.Label = "";
            return;
          }
          break;
        case 4:
          if (DataManager.Instance.ShellsGifted_4)
          {
            this.Interactable = false;
            this.Label = "";
            return;
          }
          break;
      }
      this.Label = $"{ScriptLocalization.Interactions.MakeOffering} | {this.GetAffordColor()}<sprite name=\"icon_Shell\">{LocalizeIntegration.FormatCurrentMax(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SHELL).ToString(), "1")}";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.PrayRoutine());
  }

  public IEnumerator PrayRoutine()
  {
    Snail_Interaction snailInteraction = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", snailInteraction.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(snailInteraction.playerFarming.gameObject, 12f);
    snailInteraction.playerFarming.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(snailInteraction.Snail.gameObject, 10f);
    if (snailInteraction.DLCTradeMode)
    {
      if (Inventory.GetItemQuantity(snailInteraction.DLCItemToTrade) >= snailInteraction.costOfDLCTrade)
      {
        ResourceCustomTarget.Create(snailInteraction.Snail.gameObject, snailInteraction.playerFarming.transform.position, snailInteraction.DLCItemToTrade, new System.Action(snailInteraction.\u003CPrayRoutine\u003Eb__19_0));
        yield return (object) new WaitForSeconds(1.5f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
        CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
        AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", snailInteraction.transform.position);
        yield return (object) new WaitForSeconds(1f);
        ResourceCustomTarget.Create(snailInteraction.playerFarming.gameObject, snailInteraction.Snail.gameObject.transform.position, snailInteraction.DLCItemToReceive, new System.Action(snailInteraction.\u003CPrayRoutine\u003Eb__19_1));
        yield return (object) new WaitForSeconds(1.5f);
      }
      else
      {
        MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
        CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.25f);
      }
      GameManager.GetInstance().OnConversationEnd();
    }
    else
    {
      ResourceCustomTarget.Create(snailInteraction.Snail.gameObject, snailInteraction.playerFarming.transform.position, InventoryItem.ITEM_TYPE.SHELL, new System.Action(snailInteraction.\u003CPrayRoutine\u003Eb__19_2));
      yield return (object) new WaitForSeconds(1f);
      switch (snailInteraction.ShrineNumber)
      {
        case 0:
          DataManager.Instance.ShellsGifted_0 = true;
          break;
        case 1:
          DataManager.Instance.ShellsGifted_1 = true;
          break;
        case 2:
          DataManager.Instance.ShellsGifted_2 = true;
          break;
        case 3:
          DataManager.Instance.ShellsGifted_3 = true;
          break;
        case 4:
          DataManager.Instance.ShellsGifted_4 = true;
          break;
      }
      if (DataManager.Instance.ShellsGifted_0 && DataManager.Instance.ShellsGifted_1 && DataManager.Instance.ShellsGifted_2 && DataManager.Instance.ShellsGifted_3 && DataManager.Instance.ShellsGifted_4)
      {
        yield return (object) new WaitForSeconds(0.5f);
        Inventory.SetItemQuantity(117, 0);
        MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
        CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
        AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", snailInteraction.transform.position);
        yield return (object) new WaitForSeconds(1f);
        FollowerSkinCustomTarget.Create(snailInteraction.Snail.transform.position, snailInteraction.playerFarming.transform.position, 1f, "Snail", new System.Action(snailInteraction.PickedUp));
      }
      else
        snailInteraction.PickedUp();
    }
  }

  public void PickedUp()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.enabled = false;
  }

  [CompilerGenerated]
  public void \u003CPrayRoutine\u003Eb__19_0()
  {
    AudioManager.Instance.PlayOneShot("event:/material/stone_impact", this.Snail.transform.position);
    Inventory.ChangeItemQuantity((int) this.DLCItemToTrade, -this.costOfDLCTrade);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    this.Snail.transform.DOShakePosition(1f, new Vector3(0.3f, 0.0f, 0.0f));
    this.Snail.sprite = this.SnailLit;
    this.Lighting.SetActive(true);
  }

  [CompilerGenerated]
  public void \u003CPrayRoutine\u003Eb__19_1()
  {
    AudioManager.Instance.PlayOneShot("event:/material/stone_impact", this.Snail.transform.position);
    Inventory.ChangeItemQuantity((int) this.DLCItemToReceive, 1);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    this.playerFarming.transform.DOShakePosition(1f, new Vector3(0.3f, 0.0f, 0.0f));
    this.Snail.sprite = this.SnailLit;
    this.Lighting.SetActive(true);
  }

  [CompilerGenerated]
  public void \u003CPrayRoutine\u003Eb__19_2()
  {
    AudioManager.Instance.PlayOneShot("event:/material/stone_impact", this.Snail.transform.position);
    Inventory.ChangeItemQuantity(117, -1);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    this.Snail.transform.DOShakePosition(1f, new Vector3(0.3f, 0.0f, 0.0f));
    this.Snail.sprite = this.SnailLit;
    this.Lighting.SetActive(true);
  }
}
