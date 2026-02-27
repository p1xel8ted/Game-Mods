// Decompiled with JetBrains decompiler
// Type: DropLootOnDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DropLootOnDestroy : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE LootToDrop;
  public int NumToDrop = 1;
  public bool DropSoul = true;
  public bool chanceOfDrop;
  [Range(0.0f, 100f)]
  public float chanceToDrop = 100f;
  private Health _heath;

  private void OnEnable()
  {
    this._heath = this.gameObject.GetComponent<Health>();
    if (!((Object) this._heath != (Object) null))
      return;
    this._heath.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDisable()
  {
    if (!((Object) this._heath != (Object) null))
      return;
    this._heath.OnDie -= new Health.DieAction(this.OnDie);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    CameraManager.shakeCamera(0.25f, (float) Random.Range(0, 360));
    if (this.DropSoul && this.LootToDrop == InventoryItem.ITEM_TYPE.SOUL || this.LootToDrop == InventoryItem.ITEM_TYPE.NONE)
      return;
    if (this.chanceOfDrop)
    {
      if ((double) Random.Range(0.0f, 100f) > (double) this.chanceToDrop)
        return;
      int num = -1;
      while (++num < this.NumToDrop)
        InventoryItem.Spawn(this.LootToDrop, 1, this.transform.position);
    }
    else
    {
      int num = -1;
      while (++num < this.NumToDrop)
        InventoryItem.Spawn(this.LootToDrop, 1, this.transform.position);
    }
  }

  public void dropLoot()
  {
    CameraManager.shakeCamera(0.25f, (float) Random.Range(0, 360));
    if (this.DropSoul && this.LootToDrop == InventoryItem.ITEM_TYPE.SOUL)
      return;
    if (this.chanceOfDrop)
    {
      if ((double) Random.Range(0.0f, 100f) > (double) this.chanceToDrop)
        return;
      int num = -1;
      while (++num < this.NumToDrop)
        InventoryItem.Spawn(this.LootToDrop, 1, this.transform.position);
    }
    else
    {
      int num = -1;
      while (++num < this.NumToDrop)
        InventoryItem.Spawn(this.LootToDrop, 1, this.transform.position);
    }
  }
}
