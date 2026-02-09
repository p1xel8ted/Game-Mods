// Decompiled with JetBrains decompiler
// Type: AnimatedBehaviour
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class AnimatedBehaviour : WorldGameObjectComponent
{
  public event GJCommons.VoidDelegate on_loop;

  public event GJCommons.VoidDelegate on_first_loop;

  public event GJCommons.VoidDelegate on_enter;

  public event GJCommons.VoidDelegate on_exit;

  public event AnimatedBehaviour.OnItemAnimationEvent on_item_loop;

  public event AnimatedBehaviour.OnItemAnimationEvent on_item_first_loop;

  public event AnimatedBehaviour.OnAnimUpdate on_update;

  public void SetCallbacks(AnimatedBehaviour.OnAnimUpdate update, GJCommons.VoidDelegate loop)
  {
    this.on_update += update;
    this.on_loop += loop;
  }

  public void RemoveCallbacks(AnimatedBehaviour.OnAnimUpdate update, GJCommons.VoidDelegate loop)
  {
    this.on_update -= update;
    this.on_loop -= loop;
  }

  public void OnItemStart(ItemDefinition.ItemType item_type)
  {
  }

  public void OnItemStop(ItemDefinition.ItemType item_type)
  {
  }

  public void OnItemLoop(ItemDefinition.ItemType item_type, bool flag)
  {
    if (this.on_item_loop == null)
      return;
    this.on_item_loop(item_type, flag);
  }

  public void OnItemFirstLoop(ItemDefinition.ItemType item_type, bool flag)
  {
    if (this.on_item_first_loop == null)
      return;
    this.on_item_first_loop(item_type, flag);
  }

  public void OnLoop() => this.on_loop.TryInvoke();

  public void OnEnter()
  {
  }

  public void OnExit() => this.on_exit.TryInvoke();

  public void OnFirstLoop() => this.on_first_loop.TryInvoke();

  public void OnUpdate(float normalized_time)
  {
    if (this.on_update == null)
      return;
    this.on_update(normalized_time);
  }

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = obj_type == ObjectDefinition.ObjType.Mob || obj_type == ObjectDefinition.ObjType.NPC || this.wgo.is_player;
  }

  public delegate void OnItemAnimationEvent(ItemDefinition.ItemType item_type, bool flag);

  public delegate void OnAnimUpdate(float normalized_time);
}
