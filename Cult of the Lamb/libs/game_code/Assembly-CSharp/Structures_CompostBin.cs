// Decompiled with JetBrains decompiler
// Type: Structures_CompostBin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_CompostBin : StructureBrain
{
  public System.Action UpdateCompostState;

  public int CompostCount => this.GetGrassCount();

  public int PoopCount => this.GetPoopCount();

  public virtual int CompostCost => 50;

  public virtual int PoopToCreate => 15;

  public virtual float COMPOST_DURATION => 400f;

  public void AddGrass()
  {
    this.AddGrass(1);
    this.Data.Progress = (double) this.Data.Progress == 0.0 ? TimeManager.TotalElapsedGameTime : this.Data.Progress;
    System.Action updateCompostState = this.UpdateCompostState;
    if (updateCompostState == null)
      return;
    updateCompostState();
  }

  public void AddPoop()
  {
    this.SetGrass(0);
    this.AddPoop(this.PoopToCreate);
    System.Action updateCompostState = this.UpdateCompostState;
    if (updateCompostState == null)
      return;
    updateCompostState();
  }

  public void CollectPoop()
  {
    this.SetPoop(0);
    System.Action updateCompostState = this.UpdateCompostState;
    if (updateCompostState == null)
      return;
    updateCompostState();
  }

  public int GetGrassCount()
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].type == 35)
        return this.Data.Inventory[index].quantity;
    }
    return 0;
  }

  public int GetPoopCount()
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].type == 39)
        return this.Data.Inventory[index].quantity;
    }
    return 0;
  }

  public void AddGrass(int amount)
  {
    bool flag = false;
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].type == 35)
      {
        this.Data.Inventory[index].quantity += amount;
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, amount));
  }

  public void SetGrass(int amount)
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].type == 35)
        this.Data.Inventory[index].quantity = amount;
    }
  }

  public void AddPoop(int amount)
  {
    bool flag = false;
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].type == 39)
      {
        this.Data.Inventory[index].quantity += amount;
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.POOP, amount));
  }

  public void SetPoop(int amount)
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].type == 39)
        this.Data.Inventory[index].quantity = amount;
    }
  }
}
