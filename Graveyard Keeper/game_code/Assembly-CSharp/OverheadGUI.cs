// Decompiled with JetBrains decompiler
// Type: OverheadGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OverheadGUI : MonoBehaviour
{
  public UI2DSprite bar;
  public WorldGameObject _linked_obj;
  public long _linked_obj_id;
  public float _linked_obj_max_hp;
  public Transform _tf;
  public Camera _world_cam;
  public Camera _gui_cam;

  public long linked_obj_id => this._linked_obj_id;

  public void CustomUpdate()
  {
    if (!MainGame.game_started)
      return;
    this.bar.fillAmount = MainGame.me.save.GetHPPercentage();
    this.gameObject.SetActive((double) this.bar.fillAmount < 1.0);
    if ((double) this.bar.fillAmount >= 1.0)
      return;
    this.transform.SetGUIPosToWorldPos(MainGame.me.player.pos3, MainGame.me.world_cam, MainGame.me.gui_cam);
  }

  public void LinkToObj(WorldGameObject obj)
  {
    this._linked_obj = obj;
    this._linked_obj_id = this._linked_obj.unique_id;
    this._linked_obj_max_hp = this._linked_obj.obj_def.hp.EvaluateFloat(this._linked_obj);
    this._tf = this.transform;
    this._world_cam = MainGame.me.world_cam;
    this._gui_cam = MainGame.me.gui_cam;
  }

  public bool IsNotNeededAnymore()
  {
    return (Object) this._linked_obj == (Object) null || this._linked_obj.hp.EqualsTo(this._linked_obj_max_hp);
  }

  public void UpdateForLinkedObj()
  {
    this.bar.fillAmount = this._linked_obj.hp / this._linked_obj_max_hp;
    this._tf.SetGUIPosToWorldPos(this._linked_obj.pos3, this._world_cam, this._gui_cam);
  }

  public void SetActive(bool is_active) => this.gameObject.SetActive(is_active);
}
