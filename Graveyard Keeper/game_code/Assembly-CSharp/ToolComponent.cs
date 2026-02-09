// Decompiled with JetBrains decompiler
// Type: ToolComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ToolComponent : WorldGameObjectComponent
{
  public const int ACTION_DELAY = 2;
  public WorldGameObject _target_obj;
  public int _action_delay;
  public bool _target_state_changed;
  public bool _is_driven_by_anim_event;
  public bool _playing_animation;
  public bool _is_using_tool;
  public bool _tried_to_stop;
  public bool _was_using_tool;
  public float _action_start_time = -1f;
  public ItemDefinition.ItemType _current_tool;

  public bool has_no_target => (Object) this._target_obj == (Object) null;

  public bool playing_animation => this._playing_animation;

  public override void StartComponent()
  {
    base.StartComponent();
    this.components.animated_behaviour.on_item_loop += new AnimatedBehaviour.OnItemAnimationEvent(this.OnItemLoop);
  }

  public void OnItemLoop(ItemDefinition.ItemType item_type, bool flag)
  {
    bool key = LazyInput.GetKey(GameKey.Work);
    Debug.Log((object) ("OnItemLoop, work_holded = " + key.ToString()));
    if (item_type == ItemDefinition.ItemType.Sword)
      return;
    this._playing_animation = false;
    this.StopUsingTool(key, false);
  }

  public bool UseTool(bool placed_on_dock_point)
  {
    if (this._action_delay > 0)
    {
      --this._action_delay;
      return this._is_using_tool;
    }
    if (!this._is_driven_by_anim_event || !this._target_state_changed)
      this.UseCurrentTool(placed_on_dock_point);
    if (this._is_using_tool && !this._playing_animation && this.components.character.anim_state == CharAnimState.Tool)
      this._playing_animation = true;
    return this._is_using_tool;
  }

  public void TryStop()
  {
    this._was_using_tool = false;
    if (!this._is_using_tool)
      return;
    this._tried_to_stop = true;
    Item equippedTool = this.wgo.GetEquippedTool();
    ItemDefinition.ItemType itemType = equippedTool != null ? equippedTool.definition.type : ItemDefinition.ItemType.None;
    if (equippedTool != null && itemType != ItemDefinition.ItemType.Hand)
      return;
    this.StopUsingTool();
    this.components.character.SetAnimationState(CharAnimState.Idle);
  }

  public void UseCurrentTool(bool placed_on_dock_point)
  {
    WorldGameObject objectForInteraction = this.FindObjectForInteraction();
    if ((Object) objectForInteraction != (Object) null)
    {
      if (objectForInteraction.CheckIfDisabledInTutorial())
        return;
      if (objectForInteraction.is_removing)
      {
        MainGame.me.build_mode_logics.ProcessRemovingCraft(objectForInteraction, Time.deltaTime);
        this._is_driven_by_anim_event = false;
        this.components.character.SetAnimationState(CharAnimState.Tool, ItemDefinition.ItemType.Hand);
        return;
      }
      if (objectForInteraction.playing_disappearing_anim)
        return;
      CraftComponent craft = objectForInteraction.components.craft;
      if (craft.enabled && !craft.IsCraftQueueEmpty() && !craft.is_crafting)
      {
        craft.TryStartCraftFromQueue(this.wgo.is_player);
        if (!craft.is_crafting && this.wgo.is_player)
          this.components.character.ShowCustomNeedBubble("not_enough_resources");
      }
      if (craft.enabled && (!craft.is_crafting || craft.current_craft.is_auto && !craft.current_craft.hidden))
        return;
    }
    this._current_tool = this.wgo.GetEquippedToolType();
    if ((Object) objectForInteraction != (Object) null && this._current_tool == ItemDefinition.ItemType.None)
    {
      this.components.character.ShowNeededToolBubble(!MainGame.me.save.IsWorkAvailible(objectForInteraction.obj_def));
      this.components.character.SetAnimationState(CharAnimState.Idle);
      this.StopUsingTool(true);
    }
    else if ((Object) this._target_obj != (Object) null && this._target_obj.CheckDisabledInteractions())
    {
      this._target_obj = (WorldGameObject) null;
    }
    else
    {
      if ((Object) this._target_obj == (Object) null || this._target_state_changed)
      {
        if (this._tried_to_stop)
        {
          this._target_state_changed = false;
          return;
        }
        this._target_obj = this.FindObjectForInteraction();
        if ((Object) this._target_obj == (Object) null)
        {
          if (!placed_on_dock_point && this._was_using_tool)
            return;
          this._is_using_tool = false;
          return;
        }
        if (this._target_obj.CheckDisabledInteractions())
        {
          this._target_obj = (WorldGameObject) null;
          return;
        }
        this._is_using_tool = true;
        this._target_state_changed = false;
        Debug.Log((object) ("Set action start time, prev = " + this._action_start_time.ToString()));
        if ((double) this._action_start_time <= 0.0)
          this._action_start_time = Time.time;
        ToolTypeDefinition toolTypeDefinition = ToolTypeDefinition.Get(this._current_tool);
        this._is_driven_by_anim_event = toolTypeDefinition != null && toolTypeDefinition.driven_by_anim_event;
        this.components.character.SetAnimationState(CharAnimState.Tool, this._current_tool);
      }
      PlayerComponent player = this.components.character.player;
      if (!player.IsEnoughEnergyToWork())
      {
        player.ShowNeedEnergyBubble();
        this.components.character.SetAnimationState(CharAnimState.Idle);
        this.StopUsingTool(true);
      }
      else
      {
        if ((Object) this._target_obj != (Object) null && !string.IsNullOrEmpty(objectForInteraction.obj_def.work_sfx))
          Sounds.PlaySound(objectForInteraction.obj_def.work_sfx);
        if (this._is_driven_by_anim_event || !((Object) this._target_obj != (Object) null))
          return;
        this._target_state_changed = this._target_obj.DoAction(this.wgo);
        bool flag = (Object) this._target_obj != (Object) null && this._target_obj.components != null && this._target_obj.components.craft != null && this._target_obj.components.craft.is_crafting;
        this._was_using_tool = true;
        if (!this._target_state_changed || flag)
          return;
        this._action_delay = 2;
        this._playing_animation = false;
        this.StopUsingTool(complete_work: true);
      }
    }
  }

  public void AnimationEventAction()
  {
    if (!this._is_driven_by_anim_event || (Object) this._target_obj == (Object) null)
      return;
    this._was_using_tool = true;
    this._target_state_changed = this._target_obj.DoAction(this.wgo, Time.time - this._action_start_time);
    Debug.Log((object) ("Set action start time #2, prev = " + this._action_start_time.ToString()));
    this._action_start_time = Time.time;
    if (!this._target_state_changed)
      return;
    this._action_delay = 2;
  }

  public void FailAnimationEventAction()
  {
    if (!((Object) this._target_obj != (Object) null))
      return;
    this._target_obj.DoAnimAction();
  }

  public void StopUsingTool(bool work_holded = false, bool stop_working_really = true, bool complete_work = false)
  {
    Debug.Log((object) $"StopUsingTool, work_holded = {work_holded.ToString()}, stop_working_really = {stop_working_really.ToString()}");
    if (stop_working_really)
      this.wgo.components.character.dont_work_anymore = true;
    this._is_using_tool = false;
    if ((Object) this._target_obj != (Object) null)
    {
      if (!string.IsNullOrEmpty(this._target_obj.obj_def.work_sfx))
      {
        DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(this._target_obj.obj_def.work_sfx);
        if (complete_work && !string.IsNullOrEmpty(this._target_obj.obj_def.work_end_sfx))
          Sounds.PlaySound(this._target_obj.obj_def.work_end_sfx);
      }
      this._target_obj.OnWorkFinished();
    }
    this._target_obj = (WorldGameObject) null;
    this._tried_to_stop = this._target_state_changed = this._is_driven_by_anim_event = this._playing_animation = false;
    if (!work_holded)
    {
      Debug.Log((object) ("Set action start time = -1, prev = " + this._action_start_time.ToString()));
      this._action_start_time = -1f;
    }
    this._current_tool = ItemDefinition.ItemType.None;
  }

  public override int GetExecutionOrder() => 4;

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = this.wgo.is_player;
  }

  public void ResetLastActionTime() => this._action_start_time = -1f;
}
