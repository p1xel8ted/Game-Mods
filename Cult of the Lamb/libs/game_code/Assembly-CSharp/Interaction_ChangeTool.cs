// Decompiled with JetBrains decompiler
// Type: Interaction_ChangeTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Interaction_ChangeTool : Interaction
{
  public InventoryWeapon.ITEM_TYPE Tool;
  public InventoryWeapon ToolItem;

  public void Start() => this.ToolItem = new InventoryWeapon(this.Tool, 1);

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
