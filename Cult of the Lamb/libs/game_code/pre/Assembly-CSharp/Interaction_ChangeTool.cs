// Decompiled with JetBrains decompiler
// Type: Interaction_ChangeTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Interaction_ChangeTool : Interaction
{
  public InventoryWeapon.ITEM_TYPE Tool;
  private InventoryWeapon ToolItem;

  private void Start() => this.ToolItem = new InventoryWeapon(this.Tool, 1);

  public override void GetLabel()
  {
    int currentWeapon = Inventory.CURRENT_WEAPON;
    int tool = (int) this.Tool;
  }

  public override void OnInteract(StateMachine state)
  {
    if ((InventoryWeapon.ITEM_TYPE) Inventory.CURRENT_WEAPON != this.Tool)
      Inventory.CURRENT_WEAPON = (int) this.Tool;
    else
      Inventory.CURRENT_WEAPON = 0;
  }
}
