// Decompiled with JetBrains decompiler
// Type: DropResGameObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DLCRefugees;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DropResGameObject : MonoBehaviour
{
  public const float K = 96f;
  public const float DROP_OFFSET = 28.8000011f;
  public const float START_COL_RADIUS = 0.001f;
  public const float COL_RADIUS_STEP = 0.005f;
  public const float NOT_MERGING_DROPS_TIME = 3f;
  public const float MERGE_RADIUS = 1f;
  public const int POSSIBLE_MASK = 513;
  public const int WALLS_MASK = 1;
  [Range(0.0f, 1f)]
  public float position_randomization;
  public Item res = new Item();
  public SpriteRenderer sprite;
  public SpriteRenderer sprite_x2_item;
  public SpriteRenderer sprite_res_item;
  [Header("Logic setup")]
  [Range(0.0f, 5f)]
  public float speed;
  [Range(0.0f, 1f)]
  public float disable_body_dist;
  [Range(0.0f, 1f)]
  public float collect_delay;
  [Header("Bounce setup")]
  [Range(0.0f, 2f)]
  public float bounce_time;
  [Range(0.0f, 2f)]
  public float bounce_height;
  public DropResCurve[] anim_curves;
  [Header("Shadow setup")]
  public Transform shdw_cont_x2;
  public SpriteRenderer shdw_spr_x1;
  public SpriteRenderer shdw_spr_x2;
  [Range(0.0f, 2f)]
  public float shadow_size_k;
  [Range(0.0f, 2f)]
  public float shadow_alfa_k;
  [Space]
  public bool is_collected;
  public CircleCollider2D _collider;
  public Rigidbody2D _body;
  public WorldGameObject _target_obj;
  public Transform _target_char_tf;
  public Transform _tf;
  public Transform _sprite_tf;
  public Transform _shadow_tf;
  public SpriteRenderer _shadow_renderer;
  public DropResCurve _chosen_curve;
  public float _last_dist_to_target;
  public float _drop_time;
  public float _disable_body_dist_sqr;
  public float _col_radius;
  public float _try_do_merge_after;
  public Color _shadow_color = Color.white;
  public bool _in_player_radius;
  public bool _just_spawned;
  public bool _do_merge_done;
  public KickComponent _kick_component;
  public float dist_sqr_to_player;
  public static DropResGameObject currently_higlighted_obj;
  public GameObject go_small_drop;
  public GameObject go_x2_item;
  public Color c_hilighted = Color.yellow;
  public Color c_normal = Color.magenta;
  public bool small_collider_at_start = true;
  public DropResHint _linked_hint;
  public bool _performing_drop_and_fly;
  public Vector2 _dest_global_pos = Vector2.zero;
  public bool _flying_without_collider;
  [NonSerialized]
  public Transform object_transform;
  public bool _is_moving;
  public Vector2 _move_dir = Vector2.zero;
  [NonSerialized]
  public float dist_to_player = 9999f;
  public SpriteText stack_label;
  public string zone_id = "";
  [NonSerialized]
  public float dist = 999999f;

  public Vector3 pos => this._tf.position;

  public bool has_target => (UnityEngine.Object) this._target_obj != (UnityEngine.Object) null;

  public CircleCollider2D col
  {
    get => this._collider ?? (this._collider = this.GetComponent<CircleCollider2D>());
  }

  public float bounce_curve_factor => this._chosen_curve.duration_factor;

  public float collider_radius => this._col_radius;

  public KickComponent kick_component
  {
    get
    {
      if (this._kick_component == null)
      {
        this._kick_component = new KickComponent();
        this._kick_component.Init((WorldGameObject) null);
        this._kick_component.SetDropResGameObject(this);
      }
      return this._kick_component;
    }
  }

  public static void Drop(Vector3 pos, List<Item> res, Transform tf, Direction direction = Direction.None)
  {
    foreach (Item re in res)
      DropResGameObject.Drop(pos, re, tf, direction);
  }

  public static DropResGameObject DropAndFly(
    Vector3 pos,
    Item item,
    Transform parent,
    Vector2 dest_global_pos,
    bool fly_without_collider = false)
  {
    if (item == null || item.IsEmpty())
      return (DropResGameObject) null;
    if (item.value > 1)
    {
      for (int index = 0; index < item.value; ++index)
      {
        Vector3 pos1 = pos;
        Item obj = new Item(item);
        obj.value = 1;
        Transform parent1 = parent;
        Vector2 dest_global_pos1 = dest_global_pos;
        int num = fly_without_collider ? 1 : 0;
        DropResGameObject.DropAndFly(pos1, obj, parent1, dest_global_pos1, num != 0);
      }
      return (DropResGameObject) null;
    }
    if (item.is_tech_point)
    {
      Debug.LogError((object) "Can't DropAndFly tech points");
      return (DropResGameObject) null;
    }
    DropResGameObject drop = Prefabs.me.drop_res_game_object.Copy<DropResGameObject>(parent, name: $"Drop item {item.id}, v={item.value.ToString()}");
    drop.transform.position = pos;
    DropsList.me.Add(drop);
    WorldMap.OnNewDropItem(item);
    drop.DoDrop(item, do_bounce: false);
    Vector2 vector2 = (Vector2) (MainGame.me.world_root.worldToLocalMatrix * (Vector4) dest_global_pos);
    drop.SetFlyDestination(dest_global_pos, fly_without_collider);
    return drop;
  }

  public static DropResGameObject Drop(
    Vector3 pos,
    Item item,
    Transform parent,
    Direction direction = Direction.None,
    float force_factor = 1f,
    int selected_curve = -1,
    bool check_walls = true,
    bool force_stacked_drop = false)
  {
    if (item == null || item.IsEmpty())
      return (DropResGameObject) null;
    if (item.is_tech_point)
    {
      TechPointsDrop.Drop(pos, item);
      return (DropResGameObject) null;
    }
    if (item.definition != null && item.definition.item_replace != null && MainGame.me.player.GetParamInt(item.definition.item_replace.player_flag) > 0)
      item = new Item(item.definition.item_replace.replace_id, item.value);
    if (item.worker_unique_id > 0L)
    {
      Worker worker = item.worker;
      if (worker == null)
        Debug.LogError((object) "Wroker is null!");
      else
        item = worker.GetOnGroundItem();
    }
    if (item.definition != null)
    {
      string runScriptAfterDrop = item.definition.run_script_after_drop;
      if (!string.IsNullOrEmpty(runScriptAfterDrop))
        GS.RunFlowScript(runScriptAfterDrop);
      if (item.definition.destroy_after_drop)
        return (DropResGameObject) null;
    }
    if (item.value <= 1)
      return DropResGameObject.DoDrop(pos, item, parent, direction, force_factor, selected_curve, check_walls);
    if (((item.definition == null || item.value <= 5 ? 0 : (item.definition.item_size == 1 ? 1 : 0)) | (force_stacked_drop ? 1 : 0)) != 0)
    {
      int num1 = item.value;
      DropResGameObject dropResGameObject = (DropResGameObject) null;
      while (num1 > 0)
      {
        int stackCount = item.value;
        if (item.definition.stack_count > 0 && stackCount > item.definition.stack_count)
          stackCount = item.definition.stack_count;
        num1 -= stackCount;
        Vector3 pos1 = pos;
        Item obj = new Item(item);
        obj.value = stackCount;
        Transform parent1 = parent;
        int num2 = (int) direction;
        dropResGameObject = DropResGameObject.DoDrop(pos1, obj, parent1, (Direction) num2);
      }
      return dropResGameObject;
    }
    for (int index = 0; index < item.value; ++index)
    {
      Vector3 pos2 = pos;
      Item obj = new Item(item);
      obj.value = 1;
      Transform parent2 = parent;
      int num = (int) direction;
      DropResGameObject.DoDrop(pos2, obj, parent2, (Direction) num);
    }
    return (DropResGameObject) null;
  }

  public static DropResGameObject DoDrop(
    Vector3 pos,
    Item item,
    Transform parent,
    Direction direction = Direction.None,
    float force_factor = 1f,
    int selected_curve = -1,
    bool check_walls = true)
  {
    DropResGameObject dropResGameObject = Prefabs.me.drop_res_game_object.Copy<DropResGameObject>(parent, name: $"Drop item {item.id}, v={item.value.ToString()}");
    dropResGameObject.transform.position = pos;
    dropResGameObject.DoDrop(item, selected_curve, direction != Direction.IgnoreDirection);
    DropsList.me.Add(dropResGameObject);
    WorldMap.OnNewDropItem(item);
    if (item.definition != null && item.definition.type == ItemDefinition.ItemType.Body)
    {
      dropResGameObject._linked_hint = DropResHint.Show(dropResGameObject, true);
      Sounds.PlaySound("item_2h_drop", new Vector2?((Vector2) pos));
    }
    WorldZone zoneOfPoint = WorldZone.GetZoneOfPoint((Vector2) pos);
    dropResGameObject.zone_id = (UnityEngine.Object) zoneOfPoint == (UnityEngine.Object) null ? string.Empty : zoneOfPoint.id;
    if (dropResGameObject.res != null)
      dropResGameObject.res.drop_zone_id = dropResGameObject.zone_id;
    KickComponent kickComponent = dropResGameObject.kick_component;
    if (kickComponent == null)
      return dropResGameObject;
    if (item.definition != null && item.definition.is_big)
      kickComponent.active = false;
    if (direction == Direction.None)
    {
      Vector3 direction1 = dropResGameObject.RandPos();
      bool flag = false;
      if (check_walls)
      {
        flag = DropResGameObject.IsOverlapingSomething(pos + direction1, Vector3.zero, 0.01f);
        if (flag)
        {
          for (int index = 5; flag && index > 0; flag = DropResGameObject.IsOverlapingSomething(pos + direction1, Vector3.zero, 0.01f))
          {
            --index;
            direction1 = dropResGameObject.RandPos();
          }
        }
      }
      if (!flag)
        dropResGameObject.transform.position = pos + direction1;
      kickComponent.Kick((Vector2) direction1, dropResGameObject.bounce_curve_factor, new GJCommons.VoidDelegate(dropResGameObject.OnKickedDropStoped));
      return dropResGameObject;
    }
    Vector3 vector3 = direction.ToVec3() * 28.8000011f;
    if (check_walls)
    {
      bool flag = DropResGameObject.IsOverlapingSomething(pos, vector3, dropResGameObject.collider_radius);
      if (flag)
      {
        for (int index = 3; flag && index > 0; --index)
        {
          direction = direction.ClockwiseDir();
          vector3 = direction.ToVec3() * 28.8000011f;
          flag = DropResGameObject.IsOverlapingSomething(pos, vector3, dropResGameObject.collider_radius);
        }
        if (flag)
        {
          direction = direction.ClockwiseDir().Opposite();
          vector3 = direction.ToVec3() * 28.8000011f;
        }
      }
    }
    dropResGameObject.transform.position += vector3;
    if (direction == Direction.IgnoreDirection)
      dropResGameObject._chosen_curve = (DropResCurve) null;
    else
      kickComponent.Kick((Vector2) vector3, force_factor * dropResGameObject.bounce_curve_factor, new GJCommons.VoidDelegate(dropResGameObject.OnKickedDropStoped));
    return dropResGameObject;
  }

  public void OnKickedDropStoped()
  {
    if (this.has_target || !this.col.isTrigger)
      return;
    this.col.isTrigger = false;
  }

  public Vector3 RandPos()
  {
    return new Vector3(UnityEngine.Random.Range(-96f, 96f), UnityEngine.Random.Range(-96f, 96f)) * this.position_randomization;
  }

  public static bool IsOverlapingSomething(Vector3 pos, Vector3 dir, float radius)
  {
    return Physics2D.OverlapCircleAll((Vector2) (pos + dir / 2f), radius, 1).Length != 0;
  }

  public void DoDrop(Item drop_item, int selected_curve = -1, bool do_bounce = true)
  {
    this.res = drop_item;
    ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(drop_item.id);
    bool flag = false;
    if (dataOrNull != null)
      flag = dataOrNull.item_size >= 2;
    this.sprite.gameObject.SetActive(true);
    this.go_small_drop.SetActive(!flag);
    this.go_x2_item.SetActive(flag);
    this.GetComponent<CircleCollider2D>().enabled = !flag;
    this.GetComponent<CapsuleCollider2D>().enabled = flag;
    if ((UnityEngine.Object) this.stack_label != (UnityEngine.Object) null)
      this.stack_label.SetActive(false);
    if (flag)
    {
      string sprite_name = drop_item.GetIcon();
      if (sprite_name.Contains("body"))
        sprite_name = "i_body";
      this.sprite_x2_item.sprite = EasySpritesCollection.GetSprite(sprite_name);
      this.object_transform = this.sprite_x2_item.transform;
    }
    else
    {
      this.sprite.sprite = EasySpritesCollection.GetSprite("d_" + drop_item.id, true);
      if ((UnityEngine.Object) this.sprite_res_item != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.sprite.sprite == (UnityEngine.Object) null)
        {
          this.sprite_res_item.gameObject.SetActive(true);
          this.sprite.gameObject.SetActive(false);
          this.sprite_res_item.sprite = EasySpritesCollection.GetSprite(dataOrNull == null ? "i_" + drop_item.id : dataOrNull.GetIcon());
        }
        else
          this.sprite_res_item.gameObject.SetActive(false);
      }
      this.object_transform = this.sprite_res_item.transform;
      if (drop_item.value > 1 && (UnityEngine.Object) this.stack_label != (UnityEngine.Object) null)
      {
        this.stack_label.SetActive(true);
        this.stack_label.SetText(drop_item.value.ToString() ?? "");
      }
    }
    this.is_collected = false;
    this._tf = this.transform;
    this._sprite_tf = flag ? this.sprite_x2_item.transform : this.sprite.transform;
    this._shadow_tf = flag ? this.shdw_cont_x2 : this.shdw_spr_x1.transform;
    this._shadow_renderer = flag ? this.shdw_spr_x2 : this.shdw_spr_x1;
    this._disable_body_dist_sqr = this.disable_body_dist * this.disable_body_dist;
    this._chosen_curve = selected_curve < 0 ? (this.anim_curves.Length == 0 ? (DropResCurve) null : this.anim_curves[UnityEngine.Random.Range(0, this.anim_curves.Length)]) : this.anim_curves[selected_curve];
    if (this._chosen_curve == null)
      return;
    this.col.isTrigger = true;
    this._col_radius = this.col.radius;
    if (this.small_collider_at_start)
      this.col.radius = 1f / 1000f;
    this._just_spawned = true;
    this._body = this.GetComponent<Rigidbody2D>();
    this.ChangeKickableState(true);
    if (do_bounce)
    {
      EasyTimer.Add(this.collect_delay, new EasyTimer.VoidDelegate(this.StopBounce));
      this.PlayBounce();
      this._drop_time = 0.0f;
    }
    this._do_merge_done = false;
    this._try_do_merge_after = 3f;
  }

  public void RedrawStackCounter()
  {
    if ((UnityEngine.Object) this.stack_label == (UnityEngine.Object) null)
      return;
    this.stack_label.SetActive(this.res.value > 1);
    this.stack_label.SetText(this.res.value.ToString() ?? "");
  }

  public void UpdateMe()
  {
    this.collect_delay -= Time.deltaTime;
    if (this._is_moving && !this.is_collected)
      this._tf.position += (Vector3) this._move_dir * Time.deltaTime;
    if (!this._do_merge_done)
    {
      this._try_do_merge_after -= Time.deltaTime;
      if ((double) this._try_do_merge_after < 0.0)
        this.DoTryMerging();
    }
    if (!this._performing_drop_and_fly && (!this.has_target || this.is_collected || (double) this.collect_delay > 0.0))
      this.PlayBounce();
    else if (this.is_collected)
    {
      this._target_obj = (WorldGameObject) null;
      this._tf.DOComplete();
      this._sprite_tf.DOComplete();
    }
    else
      this.PerformDropMovementLogics();
  }

  public void PerformDropMovementLogics()
  {
    Vector3 vector3 = ((this._performing_drop_and_fly ? (Vector3) this._dest_global_pos : this._target_char_tf.position) - this._tf.position) with
    {
      z = 0.0f
    };
    this.dist = vector3.magnitude;
    if (!this._performing_drop_and_fly & (this.res != null && this.res.definition != null && this.res.definition.item_size >= 2))
      return;
    if ((double) this.dist < 270.0 || this._performing_drop_and_fly)
    {
      float num = 1f;
      if ((UnityEngine.Object) this._target_obj != (UnityEngine.Object) null && this._target_obj.is_player)
        num = (float) (((double) this.dist + 0.20000000298023224) * 0.0099999997764825821 * 1.2999999523162842);
      this._tf.position += vector3.normalized * this.speed * Time.deltaTime * 96f * num;
    }
    if (this._performing_drop_and_fly)
    {
      this._last_dist_to_target = this._tf.position.DistSqrTo((Vector3) this._dest_global_pos, 96f);
      if ((double) this._last_dist_to_target >= 0.10000000149011612)
        return;
      this._performing_drop_and_fly = false;
      if (this._flying_without_collider)
      {
        this.col.enabled = true;
        this._flying_without_collider = false;
      }
      this.ChangeKickableState(true);
      this.SetDropCollectingState(false);
    }
    else
    {
      this._last_dist_to_target = this._target_char_tf.position.DistSqrTo(this._tf.position, 96f);
      this.dist_to_player = this._last_dist_to_target;
    }
  }

  public void FixedUpdateMe(float delta_time)
  {
    if (this.is_collected)
      return;
    if ((double) this.col.radius < (double) this._col_radius)
    {
      if (this.col.radius.EqualsTo(this._col_radius, 0.005f))
        this.col.radius = this._col_radius;
      else
        this.col.radius += 0.005f;
    }
    this.kick_component.FixedUpdateComponent(delta_time);
    if (!this.col.isTrigger || !this._just_spawned || (UnityEngine.Object) Physics2D.OverlapCircle((Vector2) this._tf.position, this.col.radius, 513) != (UnityEngine.Object) null || this.has_target)
      return;
    this.col.isTrigger = false;
    this._just_spawned = false;
  }

  public void ProcessDropCollectorRangeCheck(WorldGameObject collector_wgo, Vector3 char_global_pos)
  {
    if (this._performing_drop_and_fly)
      return;
    float num = char_global_pos.DistSqrTo(this._tf.position, 96f);
    if (collector_wgo.is_player)
      this.dist_sqr_to_player = num;
    if ((double) num > 3.2399997711181641 || collector_wgo.CanCollectDrop(this) <= 0 || this.has_target || (double) this.collect_delay > 0.0)
      return;
    this.ChangeKickableState(false);
    this._target_obj = collector_wgo;
    this._target_char_tf = collector_wgo.transform;
    this._last_dist_to_target = num;
    this._just_spawned = false;
    this.SetDropCollectingState(true);
  }

  public void UnsuccessfullPickup(WorldGameObject obj)
  {
    if (!this.has_target || (UnityEngine.Object) this._target_obj != (UnityEngine.Object) obj)
      return;
    this._target_obj = (WorldGameObject) null;
    this._target_char_tf = (Transform) null;
    this.SetDropCollectingState(false);
    this.ChangeKickableState(true);
  }

  public void SetFlyDestination(Vector2 dest_global_pos, bool fly_without_collider = false)
  {
    this.ChangeKickableState(false);
    this._target_obj = (WorldGameObject) null;
    this._just_spawned = false;
    this._performing_drop_and_fly = true;
    this._dest_global_pos = dest_global_pos;
    this.SetDropCollectingState(true);
    this._flying_without_collider = fly_without_collider;
    if (!fly_without_collider)
      return;
    this.col.enabled = false;
  }

  public void ChangeKickableState(bool now_kickable)
  {
    if (now_kickable)
    {
      this.kick_component.enabled = true;
      this.kick_component.StartComponent();
    }
    else
      this.kick_component.enabled = false;
    this.kick_component.OnEnableStateChanged();
  }

  public void SetDropCollectingState(bool for_collect)
  {
    this._body.bodyType = for_collect ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    this.col.isTrigger = for_collect;
  }

  public void PlayBounce()
  {
    this._drop_time += Time.deltaTime;
    if ((double) this._drop_time > (double) this.bounce_time || this._chosen_curve == null)
      return;
    float num = this._chosen_curve.curve.Evaluate(this._drop_time / this.bounce_time);
    this._sprite_tf.localPosition = Vector3.up * num * this.bounce_height;
    this._shadow_tf.localScale = (float) (1.0 + (double) num * (double) this.shadow_size_k) * Vector3.one;
    this._shadow_color.a = (float) (1.0 - (double) num * (double) this.shadow_alfa_k * 2.0);
    this._shadow_renderer.color = this._shadow_color;
  }

  public void StopBounce()
  {
    if (this._chosen_curve == null || (UnityEngine.Object) this.sprite == (UnityEngine.Object) null)
      return;
    this._chosen_curve = (DropResCurve) null;
    if (this._sprite_tf.localPosition.magnitude.EqualsTo(0.0f, 0.0001f))
      return;
    this._sprite_tf.DOLocalMove(Vector3.zero, 0.2f);
  }

  public void SetInteractionHilight(bool interaction)
  {
    if ((bool) (UnityEngine.Object) this.sprite_x2_item)
      this.sprite_x2_item.color = interaction ? this.c_hilighted : this.c_normal;
    if (interaction)
    {
      DropResGameObject.currently_higlighted_obj = this;
    }
    else
    {
      if (!((UnityEngine.Object) DropResGameObject.currently_higlighted_obj == (UnityEngine.Object) this))
        return;
      DropResGameObject.currently_higlighted_obj = (DropResGameObject) null;
    }
  }

  public bool CanPickupWithInteraction(BaseCharacterComponent inventory_owner)
  {
    if (this.is_collected)
      return false;
    return this.res.definition.item_size == 2 || inventory_owner.wgo.data.CanAddItem(this.res);
  }

  public bool TryPickupWithInteraction(BaseCharacterComponent inventory_owner)
  {
    if (!inventory_owner.wgo.data.CanAddItem(this.res))
      return false;
    this.CollectDrop(inventory_owner.wgo);
    return true;
  }

  public void CollectDrop(WorldGameObject player)
  {
    Debug.Log((object) ("<color=yellow>Collect drop</color> " + this.res?.ToString()));
    this.is_collected = true;
    if (this.res.is_tech_point)
    {
      DarkTonic.MasterAudio.MasterAudio.PlaySound("pickup", variationName: "pickup1");
      MainGame.me.player.AddToParams(this.res.id, (float) this.res.value);
    }
    else
    {
      WorldMap.OnDropItemRemoved(this.res);
      if (this.res.id != "refugee_happiness_item")
        player.AddToInventory(this.res);
      if (this.res.definition.item_size == 1)
      {
        string variationName = "pickup1";
        switch (this.res.definition.id)
        {
          case "coins":
            variationName = "pickup_coin";
            break;
        }
        DarkTonic.MasterAudio.MasterAudio.PlaySound("pickup", variationName: variationName);
        player.TryEquipPickupedDrop(this.res);
      }
    }
    this.DestroyLinkedHint();
    if (this.res.id == "refugee_happiness_item")
    {
      WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_progress_obj")?.AddToInventory(this.res);
      RefugeesCampEngine.instance.UpdateRefugeeCampValues(0.0f, RefugeesCampEngine.UpdateHappinessItemsMode.ItemUpdatesGameRes);
    }
    DropCollectGUI.OnDropCollected(this.res);
  }

  public void DestroyLinkedHint()
  {
    if (!((UnityEngine.Object) this._linked_hint != (UnityEngine.Object) null))
      return;
    this._linked_hint.DestroyMe();
  }

  public void MakeObjectMove(Vector2 dir)
  {
    this._is_moving = true;
    this._move_dir = dir;
  }

  public void DoTryMerging()
  {
    if (this.is_collected || this._is_moving)
      return;
    this._do_merge_done = true;
    if (this.res?.definition == null || this.res.definition.stack_count == 1 || this.res.definition.is_big || this.res.value >= this.res.definition.stack_count)
      return;
    Collider2D[] collider2DArray = Physics2D.OverlapCircleAll((Vector2) this._tf.position, 96f, 16384 /*0x4000*/);
    for (int index = 0; index < collider2DArray.Length; ++index)
    {
      if (!((UnityEngine.Object) collider2DArray[index] == (UnityEngine.Object) this.col))
      {
        DropResGameObject component = collider2DArray[index].GetComponent<DropResGameObject>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && !((UnityEngine.Object) component == (UnityEngine.Object) this) && !component.is_collected && !component._is_moving && component.res != null && !component.res.IsEmpty() && !(component.res.id != this.res.id))
        {
          if (this.res.value > component.res.value)
          {
            int num = this.res.definition.stack_count - this.res.value;
            if (component.res.value > num)
            {
              this.res.value += num;
              component.res.value -= num;
            }
            else
            {
              this.res.value += component.res.value;
              WorldMap.OnDropItemRemoved(component.res);
              component.is_collected = true;
              component.DestroyLinkedHint();
            }
          }
          else
          {
            int num = component.res.definition.stack_count - component.res.value;
            if (this.res.value > num)
            {
              component.res.value += num;
              this.res.value -= num;
            }
            else
            {
              component.res.value += this.res.value;
              component.RedrawStackCounter();
              WorldMap.OnDropItemRemoved(this.res);
              this.is_collected = true;
              this.DestroyLinkedHint();
              break;
            }
          }
          this.RedrawStackCounter();
          component.RedrawStackCounter();
        }
      }
    }
  }
}
