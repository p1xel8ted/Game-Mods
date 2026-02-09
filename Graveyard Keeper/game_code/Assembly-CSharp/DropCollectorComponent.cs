// Decompiled with JetBrains decompiler
// Type: DropCollectorComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DropCollectorComponent : WorldGameObjectComponent
{
  public override void StartComponent()
  {
    this._update_every_frame = 4;
    base.StartComponent();
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    if ((Object) DropsList.me == (Object) null || this.DelayedUpdate(delta_time))
      return;
    DropsList.me.CheckDrops(this.wgo);
  }

  public void OnTriggerStay2D(Collider2D col)
  {
    if (!Application.isPlaying)
      return;
    DropResGameObject component = col.GetComponent<DropResGameObject>();
    if (!(bool) (Object) component)
      return;
    this.CheckDrop(component);
  }

  public void OnTriggerEnter2D(Collider2D col)
  {
    if (!Application.isPlaying)
      return;
    DropResGameObject component = col.GetComponent<DropResGameObject>();
    if (!(bool) (Object) component)
      return;
    this.CheckDrop(component);
  }

  public void CheckDrop(DropResGameObject drop)
  {
    if (drop.is_collected)
      return;
    int item_value = this.wgo.CanCollectDrop(drop);
    if (item_value == 0)
      drop.UnsuccessfullPickup(this.wgo);
    else if (drop.res.value == item_value)
    {
      drop.CollectDrop(this.wgo);
    }
    else
    {
      drop.res.value -= item_value;
      drop.RedrawStackCounter();
      this.wgo.data.AddItem(drop.res.id, item_value);
    }
  }
}
