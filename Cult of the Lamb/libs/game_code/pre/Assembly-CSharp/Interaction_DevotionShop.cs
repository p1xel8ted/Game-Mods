// Decompiled with JetBrains decompiler
// Type: Interaction_DevotionShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_DevotionShop : Interaction
{
  public GameObject ReceiveSoulPosition;
  private float BasePrice = 1f;
  private string sLabel;

  private int LastDayUsed
  {
    get => DataManager.Instance.MidasDevotionLastUsed;
    set => DataManager.Instance.MidasDevotionLastUsed = value;
  }

  private float Cost
  {
    get => DataManager.Instance.MidasDevotionCost;
    set => DataManager.Instance.MidasDevotionCost = value;
  }

  private void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.CoolDownPrice();
    if (GameManager.HasUnlockAvailable())
      return;
    this.gameObject.SetActive(false);
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

  private void CoolDownPrice()
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

  private string GetAffordColor()
  {
    return (double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < (double) this.Cost ? "<color=red>" : "<color=#f4ecd3>";
  }

  public override void GetLabel()
  {
    this.Label = string.Format(ScriptLocalization.UI_ItemSelector_Context.Buy, (object) "10x<sprite name=\"icon_spirits\">", (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, (int) this.Cost));
  }

  public override void OnBecomeCurrent() => base.OnBecomeCurrent();

  public override void OnBecomeNotCurrent() => base.OnBecomeNotCurrent();

  public override void OnInteract(StateMachine state)
  {
    if ((double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < (double) this.Cost)
    {
      MonoSingleton<Indicator>.Instance.PlayShake();
    }
    else
    {
      base.OnInteract(state);
      if (GameManager.HasUnlockAvailable())
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

  private void GivePlayerSoul() => PlayerFarming.Instance.GetSoul(1);
}
