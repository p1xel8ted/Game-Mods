// Decompiled with JetBrains decompiler
// Type: Snail_Interaction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Snail_Interaction : Interaction
{
  public SpriteRenderer Snail;
  public Sprite SnailUnlit;
  public Sprite SnailLit;
  public GameObject Lighting;
  public int ShrineNumber = -1;

  private void Start() => this.CheckSprites();

  private void CheckSprites()
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

  private string GetAffordColor()
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SHELL) >= 1)
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
      this.Label = "";
      this.Interactable = false;
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
      this.Label = $"{ScriptLocalization.Interactions.MakeOffering} | {this.GetAffordColor()}<sprite name=\"icon_Shell\">{(object) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SHELL)} / {(object) 1}";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.PrayRoutine());
  }

  private IEnumerator PrayRoutine()
  {
    Snail_Interaction snailInteraction = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 12f);
    PlayerFarming.Instance.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(snailInteraction.Snail.gameObject, 10f);
    // ISSUE: reference to a compiler-generated method
    ResourceCustomTarget.Create(snailInteraction.Snail.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.SHELL, new System.Action(snailInteraction.\u003CPrayRoutine\u003Eb__10_0));
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
      FollowerSkinCustomTarget.Create(snailInteraction.Snail.transform.position, PlayerFarming.Instance.transform.position, 1f, "Snail", new System.Action(snailInteraction.PickedUp));
    }
    else
      snailInteraction.PickedUp();
  }

  private void PickedUp()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.enabled = false;
  }
}
