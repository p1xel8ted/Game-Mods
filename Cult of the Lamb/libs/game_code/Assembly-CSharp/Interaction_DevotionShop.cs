// Decompiled with JetBrains decompiler
// Type: Interaction_DevotionShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_DevotionShop : Interaction
{
  public GameObject ReceiveSoulPosition;
  public float BasePrice = 1f;
  public string sLabel;

  public int LastDayUsed
  {
    get => DataManager.Instance.MidasDevotionLastUsed;
    set => DataManager.Instance.MidasDevotionLastUsed = value;
  }

  public float Cost
  {
    get => DataManager.Instance.MidasDevotionCost;
    set => DataManager.Instance.MidasDevotionCost = value;
  }

  public void Start()
  {
    if (!GameManager.HasUnlockAvailable() && !DataManager.Instance.DeathCatBeaten)
      this.gameObject.SetActive(false);
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.CoolDownPrice();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    TimeManager.OnNewDayStarted += new System.Action(this.CoolDownPrice);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    TimeManager.OnNewDayStarted -= new System.Action(this.CoolDownPrice);
  }

  public void CoolDownPrice()
  {
    if (TimeManager.CurrentDay > this.LastDayUsed)
    {
      this.Cost -= (float) (10 * (TimeManager.CurrentDay - this.LastDayUsed));
      if ((double) this.Cost < (double) this.BasePrice)
        this.Cost = this.BasePrice;
      this.HasChanged = true;
    }
    this.LastDayUsed = TimeManager.CurrentDay;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Buy;
  }

  public string GetAffordColor()
  {
    return (double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < (double) this.Cost ? "<color=red>" : "<color=#f4ecd3>";
  }

  public override void GetLabel()
  {
    this.Label = string.Format(ScriptLocalization.UI_ItemSelector_Context.Buy, (object) "10x<sprite name=\"icon_spirits\">", (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, (int) this.Cost));
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if ((double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < (double) this.Cost)
    {
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      base.OnInteract(state);
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
      {
        int num = 10;
        while (--num >= 0)
          SoulCustomTarget.Create(state.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      }
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      Inventory.ChangeItemQuantity(20, -(int) this.Cost);
      this.Cost = (float) Mathf.CeilToInt(this.Cost * 1.2f);
      this.HasChanged = true;
      this.LastDayUsed = TimeManager.CurrentDay;
    }
  }

  public void GivePlayerSoul() => PlayerFarming.Instance.GetSoul(1);
}
