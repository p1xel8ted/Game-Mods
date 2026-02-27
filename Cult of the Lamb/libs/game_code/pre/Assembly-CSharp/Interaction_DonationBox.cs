// Decompiled with JetBrains decompiler
// Type: Interaction_DonationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DonationBox : Interaction
{
  public static Interaction_DonationBox Instance;
  public GameObject Level1;
  public GameObject Level1Full;
  public GameObject Level2;
  public GameObject Level2Full;
  private string sCollectCoins;
  private Vector3 PunchScale = new Vector3(0.1f, 0.1f, 0.1f);

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.UpdateGameObjects();
    Interaction_DonationBox.Instance = this;
    this.UpdateLocalisation();
  }

  private void UpdateGameObjects()
  {
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_IV))
    {
      if (DataManager.Instance.TempleDevotionBoxCoinCount > 0)
      {
        this.Level2.SetActive(false);
        this.Level2Full.SetActive(true);
      }
      else
      {
        this.Level2.SetActive(true);
        this.Level2Full.SetActive(false);
      }
      this.Level1.SetActive(false);
      this.Level1Full.SetActive(false);
    }
    else if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_III))
    {
      if (DataManager.Instance.TempleDevotionBoxCoinCount > 0)
      {
        this.Level1.SetActive(false);
        this.Level1Full.SetActive(true);
      }
      else
      {
        this.Level1.SetActive(true);
        this.Level1Full.SetActive(false);
      }
      this.Level2.SetActive(false);
      this.Level2Full.SetActive(false);
    }
    else
    {
      this.Level1.SetActive(false);
      this.Level1Full.SetActive(false);
      this.Level2.SetActive(false);
      this.Level2Full.SetActive(false);
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sCollectCoins = ScriptLocalization.Interactions.TakeItem;
  }

  public override void GetLabel()
  {
    if (!this.Level1.activeSelf && !this.Level2.activeSelf && !this.Level1Full.activeSelf && !this.Level2Full.activeSelf)
      this.Label = "";
    else if (DataManager.Instance.TempleDevotionBoxCoinCount > 0)
      this.Label = $"{this.sCollectCoins} <sprite name=\"icon_blackgold\"> x{(object) DataManager.Instance.TempleDevotionBoxCoinCount}";
    else
      this.Label = "";
  }

  public void DepositCoin()
  {
    ++DataManager.Instance.TempleDevotionBoxCoinCount;
    this.gameObject.transform.localScale = Vector3.one;
    this.gameObject.transform.DOKill();
    this.gameObject.transform.DOPunchScale(this.PunchScale, 1f, 5, 0.5f);
    this.HasChanged = true;
    this.UpdateGameObjects();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.GiveResourcesRoutine());
  }

  private IEnumerator GiveResourcesRoutine()
  {
    Interaction_DonationBox interactionDonationBox = this;
    interactionDonationBox.gameObject.transform.localScale = Vector3.one;
    interactionDonationBox.gameObject.transform.DOKill();
    interactionDonationBox.gameObject.transform.DOPunchScale(interactionDonationBox.PunchScale, 1f, 5, 0.5f);
    int max = Mathf.Min(DataManager.Instance.TempleDevotionBoxCoinCount, 5);
    int i = -1;
    while (++i < max)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionDonationBox.transform.position);
      ResourceCustomTarget.Create(interactionDonationBox.state.gameObject, interactionDonationBox.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSecondsRealtime(0.1f);
    }
    Inventory.AddItem(20, DataManager.Instance.TempleDevotionBoxCoinCount);
    interactionDonationBox.gameObject.transform.localScale = Vector3.one;
    interactionDonationBox.gameObject.transform.DOKill();
    interactionDonationBox.gameObject.transform.DOPunchScale(interactionDonationBox.PunchScale, 1f, 5, 0.5f);
    DataManager.Instance.TempleDevotionBoxCoinCount = 0;
    interactionDonationBox.UpdateGameObjects();
    interactionDonationBox.gameObject.AddComponent<SpriteRenderer>();
  }
}
