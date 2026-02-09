// Decompiled with JetBrains decompiler
// Type: InteractionComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InteractionComponent : WorldGameObjectComponent
{
  public const int OBJS_MASK = 66305;
  public const int DROPS_MASK = 16384 /*0x4000*/;
  public const int OBJS_CHECK_FREQUENCY = 1;
  public const int DROPS_CHECK_FREQUENCY = 1;
  [HideInInspector]
  public DropResGameObject nearest_drop;
  public Vector3 rotation;
  public List<WorldGameObject> _collisions = new List<WorldGameObject>();
  public bool _is_initialized;
  public bool _nearest_has_action;
  public bool _nearest_has_interaction;
  public float _interaction_start_time;
  public Transform _collider_tf;
  public WorldGameObject _target_obj;
  public BoxCollider2D _collider;
  public DropsList _drops_list;
  public WorldGameObject _nearest;
  public static InteractionComponent last_interacted;

  public WorldGameObject nearest
  {
    get => this._nearest;
    set
    {
      this._nearest = value;
      this._nearest_has_action = (Object) this._nearest != (Object) null && this._nearest.obj_def.tool_actions.action_tools.Count > 0;
      this._nearest_has_interaction = (Object) this._nearest != (Object) null && this._nearest.obj_def.interaction_type != 0;
    }
  }

  public ObjectDefinition nearest_definition
  {
    get
    {
      return !((Object) this._nearest != (Object) null) ? (ObjectDefinition) null : this._nearest.obj_def;
    }
  }

  public bool nearest_has_action => this._nearest_has_action;

  public bool nearest_has_interaction => this._nearest_has_interaction;

  public bool has_no_target => (Object) this._target_obj == (Object) null;

  public override void StartComponent()
  {
    if (!Application.isPlaying)
      return;
    base.StartComponent();
    foreach (BoxCollider2D componentsInChild in this.wgo.GetComponentsInChildren<BoxCollider2D>())
    {
      if (componentsInChild.gameObject.layer == 8)
      {
        this._collider = componentsInChild;
        break;
      }
    }
    if ((Object) this._collider != (Object) null)
    {
      this._collider_tf = this._collider.transform;
      this._drops_list = DropsList.me;
      this._is_initialized = true;
    }
    this.nearest = (WorldGameObject) null;
    this._collisions.Clear();
  }

  public void StopInteraction() => this._interaction_start_time = 0.0f;

  public bool Interact(bool interaction_start)
  {
    InteractionComponent.last_interacted = this;
    if ((double) this._interaction_start_time <= 0.0)
      this._interaction_start_time = Time.time;
    this._target_obj = this.FindObjectForInteraction();
    if ((Object) this._target_obj == (Object) null)
      return false;
    Debug.Log((object) $"Interact on = {this.wgo.name}, found target = {this._target_obj.name}");
    MainGame.me.save.quests.CheckKeyQuests("interact_" + this._target_obj.obj_id);
    Stats.DesignEvent("Interact:" + this._target_obj.obj_id);
    this._target_obj.Interact(this.wgo, interaction_start, Time.time - this._interaction_start_time);
    return true;
  }

  public void FailAnimationEventAction()
  {
    if (!((Object) this._target_obj != (Object) null))
      return;
    this._target_obj.DoAnimAction();
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    if (!this._is_initialized)
      return;
    this.rotation.z = (float) ((int) this.components.character.anim_direction * 90);
    this._collider_tf.eulerAngles = this.rotation;
    this.FindNearestInteractionObj();
    this.FindNearestDrop();
    this._collisions.RemoveUnityNulls<WorldGameObject>();
    this._drops_list.SetHighlighted(this.nearest_drop);
    if (this._collisions.Count == 0)
    {
      if ((Object) this.nearest != (Object) null)
        this.nearest.UnprepareForInteraction();
      this.nearest = (WorldGameObject) null;
    }
    else
      this.UpdateInteractionNearest();
  }

  public void UpdateInteractionNearest()
  {
    WorldGameObject interactionNearest = this.FindCurrentInteractionNearest();
    if ((Object) interactionNearest == (Object) this.nearest || (Object) interactionNearest == (Object) null)
      return;
    this.nearest = interactionNearest;
    foreach (WorldGameObject collision in this._collisions)
    {
      if ((Object) collision != (Object) this.nearest)
        collision.UnprepareForInteraction();
    }
    if (!this.wgo.is_player)
      return;
    this.components.character.wgo_hilighted_for_work = (WorldGameObject) null;
    if ((double) this.wgo.components.character.average_step > 0.0099999997764825821 && (double) LazyInput.GetDirection().magnitude > 0.0)
      this.nearest = (WorldGameObject) null;
    else
      this.nearest.PrepareForInteraction(this.components.character);
  }

  public void UpdateNearestHint()
  {
    if ((Object) this.nearest == (Object) null || !this.wgo.is_player || !MainGame.game_started)
      return;
    this.nearest.UnprepareForInteraction();
    this.nearest.PrepareForInteraction(this.components.character);
    InteractionBubbleGUI bubble = InteractionBubbleGUI.GetBubble(this.nearest.unique_id);
    if (!((Object) bubble != (Object) null))
      return;
    bubble.widget.alpha = 1f;
    TweenAlpha component = bubble.GetComponent<TweenAlpha>();
    if (!((Object) component != (Object) null))
      return;
    component.DestroyComponent();
  }

  public WorldGameObject FindCurrentInteractionNearest()
  {
    if (this._collisions.Count > 1)
      return this.GetGameObject(this.tf.localPosition, this.components.character.anim_direction);
    return this._collisions.Count != 0 ? this._collisions[0] : (WorldGameObject) null;
  }

  public void OnObjectEnter(WorldGameObject collided_object)
  {
    if ((Object) collided_object == (Object) null || this._collisions.Contains(collided_object))
      return;
    this._collisions.Add(collided_object);
  }

  public void OnObjectExit(WorldGameObject collided_object)
  {
    if ((Object) collided_object == (Object) null)
    {
      this._collisions.RemoveUnityNulls<WorldGameObject>();
    }
    else
    {
      if (!this._collisions.Contains(collided_object))
        return;
      collided_object.UnprepareForInteraction();
      this._collisions.Remove(collided_object);
    }
  }

  public WorldGameObject GetGameObject(Vector3 position, Direction direction)
  {
    if (this._collisions.Count == 0)
      return (WorldGameObject) null;
    int index1 = 0;
    float current = direction.ToVec().Atan2();
    float num1 = float.MaxValue;
    for (int index2 = 0; index2 < this._collisions.Count; ++index2)
    {
      Vector2 v = (Vector2) this._collisions[index2].tf.position - this.wgo.pos;
      float target = v.Atan2();
      float num2 = Mathf.Abs(Mathf.DeltaAngle(current, target)) + (float) ((double) v.magnitude / 96.0 / 2.0);
      if (index2 == 0 || (double) num2 < (double) num1)
      {
        num1 = num2;
        index1 = index2;
      }
    }
    return this._collisions[index1];
  }

  public void FindNearestInteractionObj()
  {
    if (Time.frameCount % 1 != 0)
      return;
    List<WorldGameObject> worldGameObjectList = new List<WorldGameObject>();
    Bounds bounds = this._collider.bounds;
    Vector2 min = (Vector2) bounds.min;
    bounds = this._collider.bounds;
    Vector2 max = (Vector2) bounds.max;
    Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(min, max, 66305);
    bool flag = false;
    foreach (Collider2D collider2D in collider2DArray)
    {
      if (!((Object) collider2D.GetComponent<SkipInteraction>() != (Object) null))
      {
        WorldObjectPart component = collider2D.GetComponent<WorldObjectPart>();
        WorldGameObject wgo = (Object) component == (Object) null ? collider2D.GetComponentInParent<WorldGameObject>() : component.parent;
        if (!((Object) wgo == (Object) null))
        {
          ObjectDefinition objDef = wgo.obj_def;
          if (objDef != null && !objDef.IsNotInteractive(wgo))
          {
            if (collider2D.isTrigger && !flag)
            {
              if (worldGameObjectList.Count > 0)
                worldGameObjectList.Clear();
              flag = true;
            }
            if (!flag || collider2D.isTrigger)
              worldGameObjectList.Add(wgo);
          }
        }
      }
    }
    for (int index = 0; index < this._collisions.Count; ++index)
    {
      if (!worldGameObjectList.Contains(this._collisions[index]))
      {
        this.OnObjectExit(this._collisions[index]);
        --index;
      }
    }
    foreach (WorldGameObject collided_object in worldGameObjectList)
    {
      if (!this._collisions.Contains(collided_object))
        this.OnObjectEnter(collided_object);
    }
  }

  public void FindNearestDrop()
  {
    if (Time.frameCount % 1 != 0)
      return;
    Bounds bounds = this._collider.bounds;
    Vector2 min = (Vector2) bounds.min;
    bounds = this._collider.bounds;
    Vector2 max = (Vector2) bounds.max;
    Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(min, max, 16384 /*0x4000*/);
    this.nearest_drop = (DropResGameObject) null;
    if (collider2DArray.Length == 0)
      return;
    float num = float.MaxValue;
    foreach (Component component1 in collider2DArray)
    {
      DropResGameObject component2 = component1.GetComponent<DropResGameObject>();
      if (!((Object) component2 == (Object) null) && !component2.res.is_tech_point && component2.res.definition != null && !component2.res.definition.is_small && (double) component2.dist_sqr_to_player <= (double) num)
      {
        this.nearest_drop = component2;
        num = component2.dist_sqr_to_player;
      }
    }
  }

  public Item GetWorkToolTypeForNearest()
  {
    return !((Object) this.nearest == (Object) null) ? this.GetWorkToolTypeForObj(this.nearest) : (Item) null;
  }

  public Item GetWorkToolTypeForObj(WorldGameObject wobj)
  {
    ObjectDefinition objDef = wobj.obj_def;
    if (!MainGame.me.save.IsWorkAvailible(objDef))
      return (Item) null;
    List<Item> objList = new List<Item>();
    foreach (ItemDefinition.ItemType actionTool in objDef.tool_actions.action_tools)
    {
      Item equippedTool = this.wgo.GetEquippedTool(actionTool);
      if (equippedTool != null)
        objList.Add(equippedTool);
    }
    return objList.Count != 0 && MainGame.me.save.IsWorkAvailible(objDef) ? objList[0] : (Item) null;
  }

  public bool CanWorkWith(WorldGameObject wobj) => this.GetWorkToolTypeForObj(wobj) != null;

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = this.wgo.is_player;
  }
}
