// Decompiled with JetBrains decompiler
// Type: BuildModeLogics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class BuildModeLogics
{
  public const float INNER_BORDERS = 96f;
  public const float OUTER_BORDERS = 96f;
  public const float CAMS_STEP = 32f;
  public BuildModeLogics.Mode _mode;
  public Vector2 _mouse_pos = Vector2.zero;
  public ObjectCraftDefinition _cd;
  public Vector2 _mouse_dpos = Vector2.zero;
  public string _cur_build_zone_id;
  public WorldZone _cur_build_zone;
  public Bounds _cur_build_zone_bounds;
  public Bounds _outer_visible_bounds;
  public Vector3 _cam_half_size;
  public MultiInventory _multi_inventory;
  public Transform _zone_camera_tf;
  public static WorldGameObject last_build_desk;
  public string building_obj_bubble;
  public Dictionary<GameKey, Vector2> gamepad_directions = new Dictionary<GameKey, Vector2>()
  {
    {
      GameKey.Left,
      Vector2.left
    },
    {
      GameKey.Right,
      Vector2.right
    },
    {
      GameKey.Up,
      Vector2.up
    },
    {
      GameKey.Down,
      Vector2.down
    }
  };
  public List<WorldGameObject> _wgos_with_marks = new List<WorldGameObject>();
  public GameObject _cur_remove_group;
  public GameObject _remove_grey_spr;
  public Vector2 _last_obj_pos = Vector2.zero;
  public bool _break_point;

  public static event System.Action on_cancel_while_script_building;

  public static event System.Action on_apply_while_script_building;

  public static event System.Action on_rotate_left_while_script_building;

  public static event System.Action on_rotate_right_while_script_building;

  public WorldZone cur_build_zone => this._cur_build_zone;

  public string cur_build_zone_id => this._cur_build_zone_id;

  public MultiInventory multi_inventory => this._multi_inventory;

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.LeftControl))
      this._break_point = !this._break_point;
    this._mouse_dpos = this._mouse_pos - (Vector2) Input.mousePosition;
    this._mouse_pos = (Vector2) Input.mousePosition;
    switch (this._mode)
    {
      case BuildModeLogics.Mode.Placing:
        this.UpdateWhilePlacing();
        break;
      case BuildModeLogics.Mode.Removing:
        this.UpdateWhileRemoving();
        break;
      case BuildModeLogics.Mode.ScriptBuilding:
        this.UpdateWhileScriptBuilding();
        break;
    }
  }

  public void UpdateWhilePlacing()
  {
    if (LazyInput.GetKeyDown(GameKey.Back) || LazyInput.GetKeyDown(GameKey.RightClick))
    {
      this.CancelPlacing();
      LazyInput.ClearKeyDown(GameKey.Back);
    }
    else
    {
      this.ProcessMovement();
      Vector3 position = FloatingWorldGameObject.cur_floating.wobj.transform.position;
      if (!this._last_obj_pos.x.EqualsTo(position.x) || !this._last_obj_pos.y.EqualsTo(position.y))
      {
        BuildGrid.me.RedrawTotemRadius(FloatingWorldGameObject.cur_floating.wobj, FloatingWorldGameObject.cur_floating.center_offsest);
        this._last_obj_pos = (Vector2) position;
        FloatingWorldGameObject.cur_floating.wobj.RefreshPositionCache();
        this.cur_build_zone.RecalculateTotems();
        this.cur_build_zone.RedrawQualities(new bool?(true), true);
      }
      if (LazyInput.GetKeyDown(GameKey.RotateLeft))
      {
        FloatingWorldGameObject.RotateCurrentFloatingObject(false);
        LazyInput.ClearKeyDown(GameKey.RotateLeft);
      }
      else if (LazyInput.GetKeyDown(GameKey.RotateRight))
      {
        FloatingWorldGameObject.RotateCurrentFloatingObject();
        LazyInput.ClearKeyDown(GameKey.RotateRight);
      }
      if (LazyInput.gamepad_active && LazyInput.GetKeyDown(GameKey.Interaction) || !LazyInput.gamepad_active && LazyInput.GetKeyDown(GameKey.LeftClick))
        this.DoPlace();
      GUIElements.me.build_mode_gui.RedrawPlacing(FloatingWorldGameObject.can_be_built && this.CanBuild((CraftDefinition) this._cd), FloatingWorldGameObject.IsObjectRotatable());
    }
  }

  public void UpdateWhileRemoving()
  {
    if (LazyInput.GetKeyDown(GameKey.Back) || LazyInput.GetKeyDown(GameKey.RightClick))
    {
      this.CancelRemoving();
      LazyInput.ClearKeyDown(GameKey.Back);
    }
    else
    {
      this.ProcessMovement();
      if (LazyInput.gamepad_active && LazyInput.GetKeyDown(GameKey.Interaction) || !LazyInput.gamepad_active && LazyInput.GetKeyDown(GameKey.LeftClick))
        this.DoRemove();
      WorldGameObject underFloatingCursor = FloatingWorldGameObject.GetWGOUnderFloatingCursor();
      if ((UnityEngine.Object) underFloatingCursor == (UnityEngine.Object) null)
        GUIElements.me.build_mode_gui.RedrawRemoving(false, false);
      else
        GUIElements.me.build_mode_gui.RedrawRemoving(underFloatingCursor.is_removing, underFloatingCursor.has_removal_craft);
    }
  }

  public void UpdateWhileScriptBuilding()
  {
    if (LazyInput.GetKeyDown(GameKey.Back) || LazyInput.GetKeyDown(GameKey.RightClick))
    {
      MainGame.me.player.AddToInventory(this._cd.needs);
      MainGame.me.build_mode_logics.SetCurrentBuildZone(string.Empty);
      System.Action whileScriptBuilding = BuildModeLogics.on_cancel_while_script_building;
      if (whileScriptBuilding != null)
        whileScriptBuilding();
      GUIElements.me.build_mode_gui.Hide();
      MainGame.me.ExitBuildMode();
      GUIElements.me.craft.Hide(true);
      this._mode = BuildModeLogics.Mode.None;
      this.RemoveMarksFromAllWGOs();
      MainGame.me.ExitBuildMode();
      MainGame.me.OpenBuildObjectGUI(BuildModeLogics.last_build_desk);
      this.cur_build_zone.RedrawQualities(new bool?(false));
      LazyInput.ClearKeyDown(GameKey.Back);
    }
    else if (LazyInput.gamepad_active && LazyInput.GetKeyDown(GameKey.Interaction) || !LazyInput.gamepad_active && LazyInput.GetKeyDown(GameKey.LeftClick))
    {
      MainGame.me.build_mode_logics.SetCurrentBuildZone(string.Empty);
      System.Action whileScriptBuilding = BuildModeLogics.on_apply_while_script_building;
      if (whileScriptBuilding != null)
        whileScriptBuilding();
      GUIElements.me.build_mode_gui.Hide();
      this._mode = BuildModeLogics.Mode.None;
      GUIElements.me.craft.Hide(true);
      MainGame.me.ExitBuildMode();
    }
    else
    {
      if (LazyInput.GetKeyDown(GameKey.RotateLeft) && this._cd.has_variations)
      {
        System.Action whileScriptBuilding = BuildModeLogics.on_rotate_left_while_script_building;
        if (whileScriptBuilding != null)
          whileScriptBuilding();
        LazyInput.ClearKeyDown(GameKey.RotateLeft);
      }
      else if (LazyInput.GetKeyDown(GameKey.RotateRight) && this._cd.has_variations)
      {
        System.Action whileScriptBuilding = BuildModeLogics.on_rotate_right_while_script_building;
        if (whileScriptBuilding != null)
          whileScriptBuilding();
        LazyInput.ClearKeyDown(GameKey.RotateRight);
      }
      GUIElements.me.build_mode_gui.RedrawScriptMode(this._cd.has_variations);
    }
  }

  public void ProcessMovement()
  {
    if (LazyInput.gamepad_active)
    {
      foreach (GameKey key in this.gamepad_directions.Keys)
      {
        if (LazyInput.GetKeyDown(key))
          FloatingWorldGameObject.MoveCurrentByDir(this.gamepad_directions[key]);
      }
    }
    else
      this.MoveObjectToMouse();
    this.CheckCameraZone();
  }

  public void CheckCameraZone()
  {
    int num1 = this._break_point ? 1 : 0;
    Camera worldCam = MainGame.me.world_cam;
    float num2 = 1f;
    Bounds bounds1 = new Bounds()
    {
      min = worldCam.ScreenToWorldPoint(Vector3.zero),
      max = worldCam.ScreenToWorldPoint(new Vector3((float) Screen.width, (float) Screen.height) * num2)
    };
    Bounds bounds2 = bounds1;
    bounds2.min += Vector3.one * 96f;
    bounds2.max -= Vector3.one * 96f;
    Vector2 vector2 = (Vector2) (LazyInput.gamepad_active ? FloatingWorldGameObject.cur_floating_pos : MainGame.me.world_cam.ScreenToWorldPoint(Input.mousePosition));
    Vector3 zero = Vector3.zero;
    if ((double) vector2.x < (double) bounds2.min.x)
      zero += Vector3.left;
    else if ((double) vector2.x > (double) bounds2.max.x)
      zero += Vector3.right;
    if ((double) vector2.y < (double) bounds2.min.y)
      zero += Vector3.down;
    else if ((double) vector2.y > (double) bounds2.max.y)
      zero += Vector3.up;
    if (zero.magnitude.EqualsTo(0.0f))
      return;
    this._cam_half_size = bounds1.extents / 2f;
    this._zone_camera_tf.position = this.GetFitCameraPos(this._zone_camera_tf.position + zero * 32f);
  }

  public Vector3 GetFitCameraPos(Vector3 source_pos)
  {
    Bounds outerVisibleBounds = this._outer_visible_bounds;
    outerVisibleBounds.min += this._cam_half_size;
    outerVisibleBounds.max -= this._cam_half_size;
    if ((double) outerVisibleBounds.min.x > (double) outerVisibleBounds.max.x)
    {
      source_pos.x = outerVisibleBounds.center.x;
    }
    else
    {
      if ((double) source_pos.x < (double) outerVisibleBounds.min.x)
        source_pos.x = outerVisibleBounds.min.x;
      if ((double) source_pos.x > (double) outerVisibleBounds.max.x)
        source_pos.x = outerVisibleBounds.max.x;
    }
    if ((double) outerVisibleBounds.min.y > (double) outerVisibleBounds.max.y)
    {
      source_pos.y = outerVisibleBounds.center.y;
    }
    else
    {
      if ((double) source_pos.y < (double) outerVisibleBounds.min.y)
        source_pos.y = outerVisibleBounds.min.y;
      if ((double) source_pos.y > (double) outerVisibleBounds.max.y)
        source_pos.y = outerVisibleBounds.max.y;
    }
    return source_pos;
  }

  public void MoveObjectToMouse()
  {
    FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) (Camera.main.ScreenToWorldPoint((Vector3) this._mouse_pos) / 96f), false);
  }

  public void DoPlace()
  {
    Debug.Log((object) nameof (DoPlace));
    if (!FloatingWorldGameObject.can_be_built)
      Debug.Log((object) "can't build - place is busy");
    else if (!this.CanBuild((CraftDefinition) this._cd))
    {
      Debug.Log((object) "Not enough");
    }
    else
    {
      this._cur_build_zone.Recalculate();
      this._multi_inventory.RemoveItems(this._cd.needs);
      Stats.DesignEvent("Build:" + this._cd.out_obj);
      Vector3 curFloatingPos = FloatingWorldGameObject.cur_floating_pos;
      WorldGameObject wobj = FloatingWorldGameObject.cur_floating.wobj;
      FloatingWorldGameObject.StopCurrentFloating(true);
      string objId = wobj.obj_id;
      if (GameBalance.me.GetDataOrNull<ObjectDefinition>(objId + "_place") != null)
      {
        wobj.ReplaceWithObject(objId + "_place", true);
        wobj.ForceInitOptimizedColliders();
        foreach (Behaviour componentsInChild in wobj.GetComponentsInChildren<Collider2D>(true))
          componentsInChild.enabled = true;
      }
      if (!string.IsNullOrEmpty(this._cd.end_script))
      {
        if (this._cd.end_script.Contains(":"))
        {
          CustomFlowScript customFlowScript = GS.RunFlowScript(this._cd.end_script.Split(':')[0]);
          customFlowScript.StartBehaviour();
          if (this._cd.end_script.Split(':').Length > 2)
            customFlowScript.FireEvent(this._cd.end_script.Split(':')[1], this._cd.end_script.Split(':')[2]);
          else
            customFlowScript.FireEvent(this._cd.end_script.Split(':')[1]);
        }
        GS.RunFlowScript(this._cd.end_script);
      }
      if (this._cd.one_time_craft && !MainGame.me.save.completed_one_time_crafts.Contains(this._cd.id))
      {
        Debug.Log((object) this._cd.id);
        MainGame.me.save.completed_one_time_crafts.Add(this._cd.id);
      }
      wobj.just_built = true;
      wobj.Redraw();
      if (this.CanBuild((CraftDefinition) this._cd) && MainGame.me.player.GetParamInt("waiting_for_first_bureal") != 1 && MainGame.me.save.IsCraftVisible((CraftDefinition) this._cd))
      {
        this.CraftBuilding((CraftDefinition) this._cd);
        if (LazyInput.gamepad_active)
          FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) curFloatingPos);
        else
          this.MoveObjectToMouse();
      }
      else
      {
        Debug.Log((object) "Not enough res for the next building");
        this.CancelPlacing();
      }
      if (MainGame.me.player.GetParamInt("waiting_for_first_bureal") == 1)
      {
        GUIElements.me.craft.OnClosePressed();
      }
      else
      {
        if (!LazyInput.gamepad_active)
          this.MoveObjectToMouse();
        BuildGrid.ReshowBuildGrid();
      }
    }
  }

  public bool CanBuild(CraftDefinition cd)
  {
    return this._multi_inventory != null && cd != null && this._multi_inventory.IsEnoughItems(cd.needs);
  }

  public void OnBuildPressed()
  {
  }

  public bool IsNowActive() => this._mode != 0;

  public void EnterScriptBuilding() => this._mode = BuildModeLogics.Mode.ScriptBuilding;

  public void EnterRemoveMode()
  {
    this._mode = BuildModeLogics.Mode.Removing;
    FloatingWorldGameObject.CreateFloatingWorldObjectById("_cursor");
    this._last_obj_pos = new Vector2(-99999f, 99999f);
    if (LazyInput.gamepad_active)
      FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) MainGame.me.world_cam.transform.position);
    this.RemoveMarksFromAllWGOs();
    WorldZone zoneById = WorldZone.GetZoneByID(this.cur_build_zone_id);
    if ((UnityEngine.Object) zoneById == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "WorldZone is null");
    }
    else
    {
      if ((UnityEngine.Object) zoneById.can_be_removed_group == (UnityEngine.Object) null)
      {
        zoneById.can_be_removed_group = new GameObject("can be removed group");
        zoneById.can_be_removed_group.transform.SetParent(MainGame.me.world_root, false);
        zoneById.can_be_removed_group.AddComponent<SortingGroup>().sortingLayerName = "over everything";
      }
      this._cur_remove_group = zoneById.can_be_removed_group;
      this._cur_remove_group.GetComponent<SortingGroup>().enabled = true;
      foreach (WorldGameObject zoneWgO in zoneById.GetZoneWGOs())
      {
        if (zoneWgO.has_removal_craft)
        {
          zoneWgO.MarkObjectAsCanBeRemoved(zoneById.can_be_removed_group);
          this._wgos_with_marks.Add(zoneWgO);
        }
      }
      GUIElements.me.hud.Hide();
      GUIElements.me.build_mode_gui.Open();
      if ((UnityEngine.Object) this._remove_grey_spr == (UnityEngine.Object) null)
      {
        this._remove_grey_spr = Resources.Load<GameObject>("remove_grey_spr").Copy();
        this._remove_grey_spr.transform.SetParent(MainGame.me.world_root, false);
      }
      this._remove_grey_spr.transform.position = zoneById.transform.position;
      this._remove_grey_spr.SetActive(true);
      BuildGrid.ShowBuildGrid(true, true);
    }
  }

  public void RemoveMarksFromAllWGOs()
  {
    foreach (WorldGameObject wgosWithMark in this._wgos_with_marks)
    {
      if (!wgosWithMark.is_removing)
        wgosWithMark.RemoveMark();
    }
    this._wgos_with_marks.Clear();
  }

  public void OnPickupPressed()
  {
  }

  public void EnterBuildMode()
  {
    this._mode = BuildModeLogics.Mode.None;
    this._last_obj_pos = new Vector2(-99999f, 99999f);
  }

  public void CancelPlacing()
  {
    Debug.Log((object) ("CancelPlacing, cur mode = " + this._mode.ToString()));
    if (this._mode != BuildModeLogics.Mode.Placing)
      return;
    if (this._cd != null && !string.IsNullOrEmpty(this._cd.sub_zone_id))
      BuildGrid.ShowBuildGrid(true);
    this.CancelCurrentMode();
  }

  public void CancelRemoving()
  {
    Debug.Log((object) ("CancelRemoving, cur mode = " + this._mode.ToString()));
    if (this._mode != BuildModeLogics.Mode.Removing)
      return;
    this.CancelCurrentMode();
    BuildGrid.ShowBuildGrid(true);
    this._cur_remove_group.GetComponent<SortingGroup>().enabled = false;
    if ((UnityEngine.Object) this._remove_grey_spr != (UnityEngine.Object) null)
      this._remove_grey_spr.SetActive(false);
    foreach (WorldGameObject componentsInChild in this._cur_remove_group.GetComponentsInChildren<WorldGameObject>(true))
      componentsInChild.CancelCanBeRemoved();
  }

  public void CancelCurrentMode()
  {
    this._mode = BuildModeLogics.Mode.None;
    FloatingWorldGameObject.StopCurrentFloating();
    this.RemoveMarksFromAllWGOs();
    MainGame.me.ExitBuildMode();
    GUIElements.me.build_mode_gui.Hide();
    MainGame.me.OpenBuildObjectGUI(BuildModeLogics.last_build_desk);
    this.cur_build_zone.RedrawQualities(new bool?(false));
    BuildGrid.me.ClearPreviousTotemRadius(true);
  }

  public void OnRemoveObjectPressed()
  {
  }

  public void OnBuildCraftSelected(ObjectCraftDefinition cd, Vector3? spawn_pos = null)
  {
    MainGame.me.EnterBuildMode();
    MainGame.me.gui_elements.craft.Hide(true);
    Debug.Log((object) $"OnBuildCraftSelected {cd.id}, obj = {cd.out_obj}");
    MainGame.me.save.quests.CheckKeyQuests("build_" + cd.out_obj);
    this._mode = BuildModeLogics.Mode.Placing;
    this._cd = cd;
    string obj_id = cd.out_obj;
    if (obj_id.Contains("_place") && GameBalance.me.GetData<ObjectDefinition>(obj_id.Replace("_place", "")) != null)
      obj_id = obj_id.Replace("_place", "");
    if (!string.IsNullOrEmpty(cd.sub_zone_id))
      BuildGrid.ShowBuildGrid(true, true, cd.sub_zone_id);
    FloatingWorldGameObject.CreateFloatingWorldObjectById(obj_id);
    SmartDrawer component = FloatingWorldGameObject.cur_floating.wobj.wop.GetComponent<SmartDrawer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.Redraw(true);
    FloatingWorldGameObject.cur_floating.wobj.RedrawBubble();
    if (spawn_pos.HasValue && (double) spawn_pos.Value.magnitude > 0.0)
      FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) spawn_pos.Value);
    else if (LazyInput.gamepad_active)
      FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) MainGame.me.world_cam.transform.position);
    MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
  }

  public static ObjectCraftDefinition GetObjectRemoveCraftDefinition(string obj_id)
  {
    foreach (ObjectCraftDefinition removeCraftDefinition in GameBalance.me.craft_obj_data)
    {
      if (removeCraftDefinition.out_obj == obj_id && removeCraftDefinition.build_type == ObjectCraftDefinition.BuildType.Remove && !removeCraftDefinition.locked_builders_ids.Contains(BuildModeLogics.last_build_desk.obj_id))
        return removeCraftDefinition;
    }
    return (ObjectCraftDefinition) null;
  }

  public static ObjectCraftDefinition GetObjectPutCraftDefinition(string obj_id)
  {
    foreach (ObjectCraftDefinition putCraftDefinition in GameBalance.me.craft_obj_data)
    {
      if (putCraftDefinition.out_obj == obj_id && putCraftDefinition.build_type == ObjectCraftDefinition.BuildType.Put && ((UnityEngine.Object) BuildModeLogics.last_build_desk == (UnityEngine.Object) null || !putCraftDefinition.locked_builders_ids.Contains(BuildModeLogics.last_build_desk.obj_id)))
        return putCraftDefinition;
    }
    return (ObjectCraftDefinition) null;
  }

  public void ProcessRemovingCraft(WorldGameObject wgo, float delta_time)
  {
    CraftDefinition removeCraftDefinition = (CraftDefinition) BuildModeLogics.GetObjectRemoveCraftDefinition(wgo.obj_id);
    if (removeCraftDefinition == null)
    {
      Debug.Log((object) "no remove craft found");
    }
    else
    {
      CraftComponent craft = wgo.components.craft;
      if (craft == null)
      {
        Debug.LogError((object) "Can't remove object without craft component", (UnityEngine.Object) wgo);
      }
      else
      {
        if (!craft.is_crafting)
        {
          if (craft.crafts.IndexOf(removeCraftDefinition) == 0)
            craft.crafts.Add(removeCraftDefinition);
          craft.Craft(removeCraftDefinition);
        }
        craft.DoAction(MainGame.me.player, delta_time, false);
        if (craft.is_crafting)
          return;
        wgo.ProcessRemove();
      }
    }
  }

  public void DoRemove()
  {
    WorldGameObject underFloatingCursor = FloatingWorldGameObject.GetWGOUnderFloatingCursor();
    if ((UnityEngine.Object) underFloatingCursor == (UnityEngine.Object) null)
      Debug.Log((object) "No object under cursor");
    else
      underFloatingCursor.MarkForRemoval();
  }

  public bool MouseWasMoved() => (double) this._mouse_dpos.sqrMagnitude > 0.01;

  public void SetCurrentBuildZone(string zone_id, string custom_sub_zone = "")
  {
    Debug.Log((object) $"SetCurrentBuildZone {zone_id}, cur = {this._cur_build_zone_id}");
    if (this._cur_build_zone_id == zone_id)
      return;
    if (string.IsNullOrEmpty(zone_id))
    {
      WorldZone.MarkZoneAsDirty(this._cur_build_zone_id);
      WorldZone zoneById = WorldZone.GetZoneByID(this._cur_build_zone_id);
      if ((UnityEngine.Object) zoneById != (UnityEngine.Object) null)
        zoneById.Recalculate();
    }
    this._cur_build_zone_id = zone_id;
    this._multi_inventory = string.IsNullOrEmpty(this._cur_build_zone_id) ? (MultiInventory) null : MainGame.me.player.GetMultiInventory(force_world_zone: this.cur_build_zone_id);
    this.FocusCameraOnBuildZone(zone_id);
    BuildGrid.ShowBuildGrid(!string.IsNullOrEmpty(zone_id), custom_sub_zone: custom_sub_zone);
  }

  public void FocusCameraOnBuildZone(string zone_id)
  {
    if (string.IsNullOrEmpty(zone_id))
    {
      CameraTools.RestoreCameraTargets();
    }
    else
    {
      CameraTools.StoreCameraTargets();
      this._cur_build_zone = WorldZone.GetZoneByID(zone_id);
      if ((UnityEngine.Object) this._cur_build_zone == (UnityEngine.Object) null)
        return;
      this._cur_build_zone_bounds = this._cur_build_zone.GetBounds();
      float num = 1f;
      Bounds bounds1 = new Bounds()
      {
        min = MainGame.me.world_cam.ScreenToWorldPoint(Vector3.zero),
        max = MainGame.me.world_cam.ScreenToWorldPoint(new Vector3((float) Screen.width, (float) Screen.height) * num)
      };
      this._cam_half_size = bounds1.extents / 2f;
      this._outer_visible_bounds = this._cur_build_zone_bounds;
      this._outer_visible_bounds.min += bounds1.extents / 2f - Vector3.one * 96f;
      this._outer_visible_bounds.max += Vector3.one * 96f - bounds1.extents / 2f;
      string[] strArray = new string[6]
      {
        "_cur_build_zone_bounds: ",
        this._cur_build_zone_bounds.ToString(),
        ", cam_bounds: ",
        null,
        null,
        null
      };
      Bounds bounds2 = bounds1;
      strArray[3] = bounds2.ToString();
      strArray[4] = ", _outer_visible_bounds: ";
      bounds2 = this._outer_visible_bounds;
      strArray[5] = bounds2.ToString();
      Debug.Log((object) string.Concat(strArray));
      if ((UnityEngine.Object) this._zone_camera_tf == (UnityEngine.Object) null)
      {
        this._zone_camera_tf = new GameObject("~build zone camera target").transform;
        this._zone_camera_tf.SetParent(MainGame.me.world_root);
        this._zone_camera_tf.localScale = Vector3.one;
        this._zone_camera_tf.gameObject.SetActive(false);
      }
      this._zone_camera_tf.position = this.GetFitCameraPos(this._cur_build_zone.center_tf.position);
      CameraTools.AddToCameraTargets(this._zone_camera_tf);
      BuildGrid.me.MoveBuildGridTo((Vector2) this._cur_build_zone.center_tf.position);
    }
  }

  public bool IsBuilding() => !string.IsNullOrEmpty(this._cur_build_zone_id);

  public void CraftBuilding(CraftDefinition craft)
  {
    ObjectCraftDefinition objectCraftDefinition = craft as ObjectCraftDefinition;
    Debug.Log((object) $"CraftBuilding craft_id = {craft.id}, type = {objectCraftDefinition.build_type.ToString()}");
    if (objectCraftDefinition.build_type == ObjectCraftDefinition.BuildType.Put)
    {
      this.OnBuildCraftSelected(objectCraftDefinition);
      GUIElements.me.game_gui.OnClosePressed();
      GUIElements.me.hud.Hide();
      GUIElements.me.build_mode_gui.Open();
      MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
    }
    else if (objectCraftDefinition.wait_script_callback)
    {
      Debug.Log((object) $"OnBuildCraftSelected {objectCraftDefinition.id}, obj = {objectCraftDefinition.out_obj}");
      MainGame.me.save.quests.CheckKeyQuests("build_" + objectCraftDefinition.out_obj);
      this._mode = BuildModeLogics.Mode.ScriptBuilding;
      this._cd = objectCraftDefinition;
      MainGame.me.EnterScriptBuilding();
      MainGame.me.gui_elements.craft.Hide(true);
      GUIElements.me.game_gui.OnClosePressed();
      GUIElements.me.hud.Hide();
      GUIElements.me.build_mode_gui.Open();
      MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
      BuildModeLogics.last_build_desk.components.craft.crafts.Clear();
      BuildModeLogics.last_build_desk.components.craft.crafts.Add((CraftDefinition) objectCraftDefinition);
      BuildModeLogics.last_build_desk.components.craft.CraftAsPlayer((CraftDefinition) objectCraftDefinition);
    }
    else
    {
      GUIElements.me.build_mode_gui.Hide();
      GUIElements.me.craft.Hide(true);
      MainGame.me.ExitBuildMode();
      BuildModeLogics.last_build_desk.components.craft.crafts.Clear();
      BuildModeLogics.last_build_desk.components.craft.crafts.Add((CraftDefinition) objectCraftDefinition);
      BuildModeLogics.last_build_desk.components.craft.CraftAsPlayer((CraftDefinition) objectCraftDefinition);
      MainGame.me.build_mode_logics.SetCurrentBuildZone(string.Empty);
    }
  }

  public enum Mode
  {
    None,
    Placing,
    Removing,
    PickUping,
    ScriptBuilding,
  }
}
