// Decompiled with JetBrains decompiler
// Type: MovementComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MovementComponent : WorldGameObjectComponent
{
  public const bool DEBUG_PATHWALKING = false;
  public const bool SET_PLAYER_STATIC = false;
  public const float DASH_TIME = 0.1f;
  public const float DASH_DIST = 1.5f;
  public const float DASH_COST = 2f;
  public const int STEPS_COUNT = 5;
  public const float MIN_DELTA = 0.01f;
  public const float VERTICAL_K = 0.8f;
  public const float STEP_EPSILON = 0.001f;
  public const float MIN_GOTO_DEST_DIF = 0.1f;
  public const float FOLLOW_PATHFINDING_DELAY = 0.5f;
  public const float WALLS_CHECK_DELAY = 0.1f;
  public const float DEFAULT_MASS = 100f;
  public const float DEFAULT_DRAG = 50f;
  [RuntimeValue("Runtime values", false)]
  public float velocity;
  [RuntimeValue]
  public Vector2 movement_dir;
  [RuntimeValue]
  public Vector2 delta_vec;
  [RuntimeValue]
  public bool max_speed_reached;
  [NonSerialized]
  public int path_waypoint;
  [NonSerialized]
  public float dir_distance;
  [NonSerialized]
  public float follow_delay;
  [NonSerialized]
  public float min_follow_dis;
  [NonSerialized]
  public float follow_pathfinding_delay;
  [NonSerialized]
  public float goto_vector_dist;
  [NonSerialized]
  public float min_follow_dist;
  [NonSerialized]
  public float last_step;
  [NonSerialized]
  public float walked_dist;
  [NonSerialized]
  public float calcultated_average_step;
  [NonSerialized]
  public float walls_check_delay;
  [NonSerialized]
  public Vector2 goto_vector_dir;
  [NonSerialized]
  public Vector2 current_point_pos;
  [NonSerialized]
  public Vector2 dir_before_path_finding;
  [NonSerialized]
  public Vector3 current_pos;
  [NonSerialized]
  public bool astar_following;
  [NonSerialized]
  public bool no_walls_to_target;
  [NonSerialized]
  public bool stopped = true;
  [NonSerialized]
  public MovementComponent.MovementState state;
  [NonSerialized]
  public Transform target;
  [NonSerialized]
  public GJCommons.VoidDelegate on_complete;
  [NonSerialized]
  public GJCommons.VoidDelegate on_failed;
  [NonSerialized]
  public MovementComponent.OnMove on_move_dir;
  [NonSerialized]
  public List<float> last_steps = new List<float>();
  [NonSerialized]
  public string event_on_complete = "";
  [NonSerialized]
  public Vector2 _nearest_gd_point_pos = Vector2.zero;
  public AnimationCurve _current_curve;
  public Vector2 _curve_delta;
  public Vector2 _prev_curve_pos;
  public float _curve_normalized_time;
  public int _curve_mvmnt_start_frame;
  public bool _curve_based_on_anim_timing;
  [NonSerialized]
  public List<Vector3> cur_astar_path;
  public int _stuck_counter;
  public Vector2 _cur_move_dv = Vector2.zero;
  public AStarSearcher _astar;
  public bool _using_gd_graph;
  [NonSerialized]
  public bool player_controlled_by_script;
  public float _stored_speed;
  public bool _in_stored_speed_mode;
  public string _target_gd_point_tag = "";
  [NonSerialized]
  public float dash_remaining_time = -3f;
  [NonSerialized]
  public float last_pressed_dash_time;
  [NonSerialized]
  public Vector2 dash_direction = Vector2.down;
  public MovementComponent.Modifier _mod_accel;
  public MovementComponent.Modifier _mod_friction;
  public MovementComponent.Modifier _mod_accel_always;
  public MovementComponent.Modifier _mod_speed;
  public int idle_animation;
  public Vector2 _prev_pos = Vector2.zero;

  public AStarSearcher astar => this._astar ?? (this._astar = new AStarSearcher(this));

  public Transform following_target => this.target;

  public bool is_following_target => (UnityEngine.Object) this.target != (UnityEngine.Object) null;

  public float step => this.wgo.data.GetParam("speed") / 30f;

  public bool IsStopped => this.stopped;

  public MovementComponent.MovementState movement_state => this.state;

  public float average_step => this.calcultated_average_step;

  public override void StartComponent()
  {
    if (this.started)
      return;
    base.StartComponent();
    this.current_pos = this.tf.position;
    this.CheckMovementParams();
    this.UpdateBodyPhysics();
  }

  public void CheckMovementParams()
  {
    if ((UnityEngine.Object) this.wgo == (UnityEngine.Object) null)
      Debug.LogError((object) "WGO is null");
    else if (this.wgo.obj_def == null)
    {
      Debug.LogError((object) ("Obj def is null for WGO " + this.wgo.name), (UnityEngine.Object) this.wgo);
    }
    else
    {
      if (!this.wgo.obj_def.res.Has("acceleration"))
        this.wgo.obj_def.res.Set("acceleration", 1f);
      if (this.wgo.obj_def.res.Has("friction"))
        return;
      this.wgo.obj_def.res.Set("friction", 0.0f);
    }
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    this.last_step = this.current_pos.DistTo(this.tf.position) / 96f;
    this.current_pos = this.tf.position;
    this.last_steps.Add(this.last_step);
    if (this.last_steps.Count > 5)
      this.last_steps.RemoveAt(0);
    this.walked_dist = 0.0f;
    foreach (float lastStep in this.last_steps)
      this.walked_dist += lastStep;
    this.calcultated_average_step = (double) this.walked_dist > 0.0 ? this.walked_dist / (float) this.last_steps.Count : 0.0f;
    if (this.state != MovementComponent.MovementState.None && MainGame.game_started && this.last_steps.Count == 5 && (this.walked_dist.EqualsTo(0.0f, 1f / 1000f) || this.calcultated_average_step.EqualsTo(0.0f, 1f / 1000f)) && this.state == MovementComponent.MovementState.GoTo)
    {
      if (this.cur_astar_path == null)
        return;
      if (++this._stuck_counter > 5)
      {
        Debug.LogWarning((object) (this.wgo.name + " stucked, #path## fail"), (UnityEngine.Object) this.wgo);
        this.OnPathFailed();
      }
    }
    switch (this.state)
    {
      case MovementComponent.MovementState.Following:
        this.UpdateFollowing(delta_time);
        break;
      case MovementComponent.MovementState.GoToVector:
        this.UpdateGoToVector();
        break;
    }
  }

  public void ChangeMovementState(
    MovementComponent.MovementState new_state,
    bool reset_callbacks = true,
    bool force_state_change = false)
  {
    if (this.wgo.is_player)
      Debug.Log((object) $"ChangeMovementState from = {this.state.ToString()} ==> {new_state.ToString()}");
    if (this.wgo.is_player && new_state == this.state && new_state == MovementComponent.MovementState.AnimCurve && this._curve_based_on_anim_timing)
      this.components.animated_behaviour.on_loop -= new GJCommons.VoidDelegate(this.OnCurveLoop);
    if (new_state == this.state && !force_state_change)
      return;
    if (!this.started)
      this.StartComponent();
    this.last_steps.Clear();
    this.walked_dist = 0.0f;
    this._stuck_counter = 0;
    if (new_state == MovementComponent.MovementState.None)
    {
      if ((double) this.movement_dir.magnitude > 0.0)
        this.SetMovementDir(Vector2.zero);
      this.stopped = true;
      int num = this.wgo.is_player ? 1 : 0;
    }
    switch (this.state)
    {
      case MovementComponent.MovementState.None:
        this.cur_astar_path = (List<Vector3>) null;
        break;
      case MovementComponent.MovementState.Following:
        this.idle_animation = 0;
        this.UpdateMovement(Vector2.zero, 0.0f);
        this.target = (Transform) null;
        this.astar_following = false;
        break;
      case MovementComponent.MovementState.GoTo:
        this.idle_animation = 0;
        this.astar.Clear();
        break;
      case MovementComponent.MovementState.GoToVector:
        this.idle_animation = 0;
        this.goto_vector_dist = -1f;
        break;
      case MovementComponent.MovementState.AnimCurve:
        this.idle_animation = 0;
        this._prev_curve_pos = this._curve_delta = Vector2.zero;
        this._curve_normalized_time = 0.0f;
        if (this._curve_based_on_anim_timing)
        {
          this.components.animated_behaviour.RemoveCallbacks(new AnimatedBehaviour.OnAnimUpdate(this.OnCurveUpdate), new GJCommons.VoidDelegate(this.OnCurveLoop));
          break;
        }
        this.components.timer.RemoveCallbacks(new TimerComponent.OnUpdate(this.OnCurveUpdate), new GJCommons.VoidDelegate(this.OnCurveLoop));
        break;
    }
    if (reset_callbacks)
    {
      this.on_complete = this.on_failed = (GJCommons.VoidDelegate) null;
      this.event_on_complete = "";
    }
    this.state = new_state;
    if (this.wgo.obj_def == null || !this.wgo.obj_def.IsNPC())
      return;
    this.wgo.RedrawBubble();
  }

  public bool IsInMovingState()
  {
    if (this.state != MovementComponent.MovementState.None)
      return true;
    return this.astar != null && this.astar.finding;
  }

  public override bool HasFixedUpdate() => true;

  public override void FixedUpdateComponent(float delta_time)
  {
    if (this.state == MovementComponent.MovementState.Dash)
      this.UpdateDash(delta_time);
    else if (this.state == MovementComponent.MovementState.AnimCurve)
    {
      this.CheckCurve();
      Vector2 vector2_1 = this._curve_delta * this._current_curve.Evaluate(this._curve_normalized_time);
      Vector2 vector2_2 = vector2_1 - this._prev_curve_pos;
      this._prev_curve_pos = vector2_1;
      this.body.MovePosition(this.body.position + vector2_2 * 96f);
    }
    else
    {
      if (this.state == MovementComponent.MovementState.GoTo)
        this.UpdatePathfinding(Time.fixedDeltaTime);
      this.UpdateMovement(this.movement_dir, delta_time);
    }
  }

  public void UpdateDash(float delta_time)
  {
    if ((double) this.dash_remaining_time < 0.0)
      return;
    if (!MainGame.me.player_char.control_enabled || GUIElements.me.craft.is_shown || GUIElements.me.body_craft.is_shown || GUIElements.me.mixed_craft.is_shown || GUIElements.me.resource_based_craft.is_shown)
    {
      this.dash_remaining_time = -0.1f;
    }
    else
    {
      Vector2 position = this.body.position;
      float num = (float) (1.5 * (double) delta_time / 0.10000000149011612 * 96.0);
      Vector2 vector2 = new Vector2(this.dash_direction.x * num, (float) ((double) this.dash_direction.y * (double) num * 0.800000011920929));
      this.dash_remaining_time -= delta_time;
      this.body.MovePosition(position + vector2);
    }
  }

  public void UpdateMovement(Vector2 dir, float delta_time)
  {
    if (this.wgo.is_dead)
      return;
    this.movement_dir = dir.normalized;
    if (this.movement_dir.magnitude.EqualsTo(0.0f) && this.delta_vec.magnitude.EqualsTo(0.0f))
    {
      if (!this.max_speed_reached)
        return;
      this.max_speed_reached = false;
    }
    else
    {
      float a = this.wgo.data.GetParam("speed");
      if (a.EqualsTo(LazyConsts.PLAYER_SPEED))
        a += this.wgo.data.GetParam("speed_buff");
      if (this.stopped)
        this.stopped = false;
      bool flag = this.wgo.obj_def.accelerate_always;
      if (this._mod_accel_always != null)
      {
        flag = flag || (double) this._mod_accel_always.value > 1.0;
        this._mod_accel_always.Update(delta_time);
      }
      float acceleration = this.wgo.obj_def.acceleration;
      if (this._mod_accel != null)
      {
        acceleration += this._mod_accel.value;
        this._mod_accel.Update(delta_time);
      }
      float friction = this.wgo.obj_def.friction;
      if (this._mod_friction != null)
      {
        friction += this._mod_friction.value;
        this._mod_friction.Update(delta_time);
      }
      if (this._mod_speed != null)
      {
        a += this._mod_speed.value;
        this._mod_speed.Update(delta_time);
      }
      if ((double) this.movement_dir.magnitude > 0.0)
      {
        if (this.max_speed_reached && !flag)
        {
          this.delta_vec = this.movement_dir;
        }
        else
        {
          acceleration.EqualsTo(0.0f);
          this.delta_vec += acceleration * this.movement_dir;
        }
      }
      else
      {
        this.delta_vec *= friction;
        if ((double) Mathf.Abs(this.delta_vec.x) < 0.0099999997764825821)
          this.delta_vec.x = 0.0f;
        if ((double) Mathf.Abs(this.delta_vec.y) < 0.0099999997764825821)
          this.delta_vec.y = 0.0f;
      }
      this.velocity = this.delta_vec.magnitude;
      if (this.velocity.EqualsTo(0.0f))
      {
        this.max_speed_reached = false;
      }
      else
      {
        if ((double) this.velocity > 1.0)
        {
          this.delta_vec *= 1f / this.velocity;
          this.velocity = this.delta_vec.magnitude;
          this.max_speed_reached = true;
        }
        this.UpdateBodyPhysics();
        this.wgo.round_and_sort.MarkPositionDirty();
        int state = (int) this.state;
        this._cur_move_dv = new Vector2(this.delta_vec.x, this.delta_vec.y * 0.8f) * a * 96f;
        this.body.MovePosition(this.body.position + this._cur_move_dv * delta_time);
        if (this.wgo.is_player)
          return;
        this.wgo.GetComponent<RoundAndSortComponent>().MarkPositionDirty();
      }
    }
  }

  public void UpdatePathfinding(float delta_time)
  {
    if (this.wgo.is_player && this.astar.finding)
      return;
    if (this.astar.finding && (double) this.dir_before_path_finding.magnitude > 0.0)
      this.SetMovementDir(this.dir_before_path_finding);
    if (this.cur_astar_path == null)
      return;
    if (this.path_waypoint >= this.cur_astar_path.Count)
    {
      this.OnCameToLastPoint(false);
    }
    else
    {
      if (this.path_waypoint + 1 < this.cur_astar_path.Count && (double) this.cur_astar_path[this.path_waypoint].z >= 1000.0)
      {
        this.wgo.transform.position = this.cur_astar_path[this.path_waypoint + 1];
        this.wgo.RefreshPositionCache();
        this.path_waypoint += 2;
        if (this.path_waypoint >= this.cur_astar_path.Count)
        {
          this.OnCameToLastPoint(false);
          return;
        }
      }
      Vector2 zero = Vector2.zero;
      Vector2 position = this.body.position;
      this._prev_pos = position;
      Vector2 vector2;
      do
      {
        this.current_point_pos = (Vector2) this.cur_astar_path[this.path_waypoint];
        float num = (float) ((double) this._cur_move_dv.magnitude * (double) delta_time * 0.60000002384185791);
        vector2 = this.current_point_pos - position;
        if ((double) vector2.magnitude > 0.10000000149011612 && (double) vector2.magnitude > (double) num)
          goto label_14;
      }
      while (++this.path_waypoint < this.cur_astar_path.Count);
      this.OnCameToLastPoint(false);
      return;
label_14:
      this.SetMovementDir(vector2.normalized);
    }
  }

  public void StoreSpeedBeforeWalking()
  {
    if (this._in_stored_speed_mode)
      return;
    this._in_stored_speed_mode = true;
    this._stored_speed = this.wgo.data.GetParam("speed");
  }

  public void RestoreSpeedAfterWalking()
  {
    if (!this._in_stored_speed_mode)
      return;
    this._in_stored_speed_mode = false;
    this.SetSpeed(this._stored_speed);
  }

  public void SetSpeed(float speed)
  {
    if ((double) speed <= 0.0)
      return;
    this.wgo.data.SetParam(nameof (speed), speed);
  }

  public void UpdateBodyPhysics()
  {
    if ((UnityEngine.Object) this.body == (UnityEngine.Object) null)
      return;
    if (this.wgo.is_player)
    {
      if (this.wgo.is_dead)
      {
        if (this.body.bodyType == RigidbodyType2D.Static)
          return;
        this.body.bodyType = RigidbodyType2D.Static;
      }
      else if (MainGame.me.player_char.control_enabled)
      {
        if (this.body.bodyType == RigidbodyType2D.Dynamic)
          return;
        this.body.bodyType = RigidbodyType2D.Dynamic;
      }
      else
      {
        switch (this.state)
        {
          case MovementComponent.MovementState.None:
            if (this.body.bodyType == RigidbodyType2D.Static)
              break;
            this.body.bodyType = RigidbodyType2D.Static;
            break;
          case MovementComponent.MovementState.Following:
          case MovementComponent.MovementState.GoTo:
          case MovementComponent.MovementState.GoToVector:
          case MovementComponent.MovementState.AnimCurve:
            if (this.body.bodyType == RigidbodyType2D.Kinematic)
              break;
            this.body.bodyType = RigidbodyType2D.Kinematic;
            break;
          case MovementComponent.MovementState.Dash:
            if (this.body.bodyType == RigidbodyType2D.Dynamic)
              break;
            this.body.bodyType = RigidbodyType2D.Dynamic;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
    else
      this.body.bodyType = this.wgo.obj_def.dynamic_mob ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
  }

  public void StopImmediate()
  {
    if (this.body.bodyType != RigidbodyType2D.Static)
      this.body.velocity = Vector2.zero;
    this.max_speed_reached = false;
    this.velocity = 0.0f;
    this.delta_vec = Vector2.zero;
  }

  public MovementComponent CurveMoveTo(
    WorldGameObject wobj,
    AnimationCurve curve,
    float dist = 0.0f,
    GJCommons.VoidDelegate on_complete = null,
    bool based_on_anim_timing = true)
  {
    return this.CurveMoveTo(wobj.tf, curve, dist, on_complete, based_on_anim_timing);
  }

  public MovementComponent CurveMoveTo(
    Transform tf,
    AnimationCurve curve,
    float dist = 0.0f,
    GJCommons.VoidDelegate on_complete = null,
    bool based_on_anim_timing = true,
    bool set_active_now_because_of_movement = false)
  {
    return this.CurveMove(this.tf.DirTo(tf), curve, dist, on_complete, based_on_anim_timing, set_active_now_because_of_movement: set_active_now_because_of_movement);
  }

  public void CheckCurve()
  {
    if (this._current_curve != null)
      return;
    Debug.LogError((object) ("Null curve, using linear for wgo " + this.wgo.obj_id), (UnityEngine.Object) this.wgo);
    this._current_curve = Resources.Load<MovementCurve>("Curves/linear").curve;
    if (this._current_curve != null)
      return;
    Debug.LogError((object) "Coudln't load curve");
  }

  public MovementComponent CurveMove(
    Vector2 dir,
    AnimationCurve curve,
    float dist = 0.0f,
    GJCommons.VoidDelegate on_complete = null,
    bool based_on_anim_timing = true,
    string event_on_complete = "",
    bool force_state_change = false,
    bool is_dir_change = false,
    bool set_active_now_because_of_movement = false)
  {
    this.cur_astar_path = new List<Vector3>();
    this._current_curve = curve;
    this.CheckCurve();
    this.ChangeMovementState(MovementComponent.MovementState.AnimCurve, force_state_change: force_state_change);
    this._curve_delta = dist.EqualsTo(0.0f) ? dir : dist * dir.normalized;
    this._prev_curve_pos = Vector2.zero;
    this.on_complete = on_complete;
    this.event_on_complete = event_on_complete;
    this._curve_mvmnt_start_frame = Time.frameCount;
    this._curve_based_on_anim_timing = based_on_anim_timing;
    if (this._curve_based_on_anim_timing)
      this.components.animated_behaviour.SetCallbacks(new AnimatedBehaviour.OnAnimUpdate(this.OnCurveUpdate), new GJCommons.VoidDelegate(this.OnCurveLoop));
    else
      this.components.timer.SetCallbacks(new TimerComponent.OnUpdate(this.OnCurveUpdate), new GJCommons.VoidDelegate(this.OnCurveLoop));
    if (is_dir_change && !this._curve_based_on_anim_timing)
      this.SetMovementDir(dir);
    this.stopped = false;
    this.UpdateBodyPhysics();
    if (set_active_now_because_of_movement)
    {
      ChunkedGameObject componentInChildren = this.wgo.GetComponentInChildren<ChunkedGameObject>();
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        componentInChildren.active_now_because_of_movement = true;
    }
    return this;
  }

  public void OnCurveUpdate(float normalized_time) => this._curve_normalized_time = normalized_time;

  public void OnCurveLoop()
  {
    if (this.wgo.is_player)
      Debug.Log((object) $"OnCurveLoop, state = {this.state.ToString()}, fr = {Time.frameCount.ToString()}, start fr = {this._curve_mvmnt_start_frame.ToString()}");
    if (Time.frameCount - this._curve_mvmnt_start_frame == 0)
      return;
    if (this._curve_based_on_anim_timing)
      this.components.animated_behaviour.on_loop -= new GJCommons.VoidDelegate(this.OnCurveLoop);
    if (this.state == MovementComponent.MovementState.AnimCurve)
    {
      this.OnComplete();
      ChunkedGameObject componentInChildren = this.wgo.GetComponentInChildren<ChunkedGameObject>();
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        componentInChildren.active_now_because_of_movement = false;
    }
    this.ChangeMovementState(MovementComponent.MovementState.None);
  }

  public void GoTo(
    GameObject dest,
    bool snap_to_node = false,
    GJCommons.VoidDelegate on_complete = null,
    GJCommons.VoidDelegate on_failed = null,
    bool with_cinematic = false,
    MovementComponent.GoToMethod goto_method = MovementComponent.GoToMethod.AStar,
    string event_on_complete = "",
    uint? filter_astar_area = null,
    bool from_script = false,
    GDPoint target_gdp = null)
  {
    this.GoTo((Vector2) dest.transform.position, snap_to_node, on_complete, on_failed, with_cinematic, goto_method, event_on_complete, filter_astar_area, from_script, target_gdp);
  }

  public void GoTo(
    Vector2 dest,
    bool snap_to_node = false,
    GJCommons.VoidDelegate on_complete = null,
    GJCommons.VoidDelegate on_failed = null,
    bool with_cinematic = false,
    MovementComponent.GoToMethod goto_method = MovementComponent.GoToMethod.AStar,
    string event_on_complete = "",
    uint? filter_astar_area = null,
    bool from_script = false,
    GDPoint target_gd_point = null)
  {
    this._target_gd_point_tag = (UnityEngine.Object) target_gd_point == (UnityEngine.Object) null ? "" : target_gd_point.gd_tag;
    this.wgo.GetComponent<RoundAndSortComponent>().enabled = true;
    if (with_cinematic)
      CameraTools.PlayCinematics(this.wgo, 0.5f, 100f);
    if (!this.astar_following)
      this.ChangeMovementState(MovementComponent.MovementState.GoTo);
    this.event_on_complete = event_on_complete;
    this._using_gd_graph = goto_method == MovementComponent.GoToMethod.GDGraph;
    this.StoreSpeedBeforeWalking();
    if (snap_to_node)
    {
      NNInfo nearest = AstarPath.active.GetNearest((Vector3) dest, (NNConstraint) new PathNNConstraint());
      if (nearest.node != null)
        dest = nearest.node.position.ToVector2();
    }
    string name = this.wgo.name;
    Vector2 vector2 = dest;
    string str = vector2.ToString();
    Debug.Log((object) $"GoTo {name}, dest = {str}", (UnityEngine.Object) this.wgo);
    Debug.DrawLine((Vector3) this.wgo.pos, (Vector3) dest, Color.yellow, 1f);
    this.astar.EnablePathSmoother(goto_method == MovementComponent.GoToMethod.AStar);
    if ((double) this.astar.destination.GridDistTo(dest) < 0.10000000149011612)
      return;
    switch (goto_method)
    {
      case MovementComponent.GoToMethod.AStar:
        int num1 = this.wgo.is_player ? 2 : 0;
        if (this.wgo.is_player)
          AStarTools.RefreshPlayerGraph(this.wgo.pos, dest);
        else
          AStarTools.UpdateAstarBounds(this.wgo.pos, dest);
        this.astar.Find(dest, new GJCommons.VoidDelegate(this.OnPathFound), new GJCommons.VoidDelegate(this.OnPathFailed), 1 << num1);
        break;
      case MovementComponent.GoToMethod.GDGraph:
        if (!filter_astar_area.HasValue || filter_astar_area.Value == 0U)
        {
          if ((UnityEngine.Object) target_gd_point != (UnityEngine.Object) null)
          {
            try
            {
              if (target_gd_point.node != null)
                filter_astar_area = new uint?(target_gd_point.node.Area);
            }
            catch (Exception ex)
            {
              Debug.LogError((object) $"Some problems with WGO [{this.wgo.obj_id}] target_gd_point: {ex?.ToString()}");
            }
            Debug.LogWarning((object) $"Set new filter_astar_area = {(filter_astar_area.HasValue ? filter_astar_area.Value.ToString() : "null")} for WGO [{this.wgo.obj_id}]", (UnityEngine.Object) this.wgo);
          }
        }
        Vector2 pos = this.wgo.pos;
        GDPoint context = (GDPoint) null;
        float num2 = float.PositiveInfinity;
        foreach (GDPoint gdPoint in WorldMap.gd_points)
        {
          if (!((UnityEngine.Object) gdPoint == (UnityEngine.Object) null) && gdPoint.node != null && (!filter_astar_area.HasValue || (int) gdPoint.node.Area == (int) filter_astar_area.Value))
          {
            vector2 = pos - (Vector2) gdPoint.pos;
            float sqrMagnitude = vector2.sqrMagnitude;
            if ((double) sqrMagnitude < (double) num2)
            {
              context = gdPoint;
              num2 = sqrMagnitude;
            }
          }
        }
        if ((UnityEngine.Object) context == (UnityEngine.Object) null)
        {
          Debug.LogError((object) ("Nearest GDPoint is null! filter_astar_area = " + (!filter_astar_area.HasValue ? "null" : filter_astar_area.Value.ToString())), (UnityEngine.Object) this.wgo);
          using (List<GDPoint>.Enumerator enumerator = WorldMap.gd_points.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GDPoint current = enumerator.Current;
              Debug.Log((object) $"GD point {current.name}, area = {current.node.Area.ToString()}", (UnityEngine.Object) current);
            }
            return;
          }
        }
        Debug.Log((object) $"Nearest GD point: {context.name}, pos = {context.transform.position.ToString()}, obj_pos = {pos.ToString()}", (UnityEngine.Object) context);
        this.cur_astar_path = (List<Vector3>) null;
        this._nearest_gd_point_pos = (Vector2) context.pos;
        this.astar.Find((Vector2) context.transform.position, dest, new GJCommons.VoidDelegate(this.OnGDPointsPathFound), new GJCommons.VoidDelegate(this.OnPathFailed), 2);
        break;
      case MovementComponent.GoToMethod.Direct:
        this.astar.SetDest(dest);
        this.path_waypoint = 0;
        this.cur_astar_path = new List<Vector3>()
        {
          (Vector3) this.wgo.pos,
          (Vector3) dest
        };
        this.ProceedToNextPathPoint();
        break;
    }
    if (!this.wgo.is_player)
      this.dir_before_path_finding = this.movement_dir;
    this.path_waypoint = 1;
    if (this.astar_following)
      return;
    if (this.wgo.is_player)
      this.player_controlled_by_script = from_script;
    this.on_complete = !with_cinematic ? (GJCommons.VoidDelegate) (() =>
    {
      this.RestoreSpeedAfterWalking();
      if (on_complete == null)
        return;
      if (this.wgo.is_player)
        this.player_controlled_by_script = false;
      GJCommons.VoidDelegate voidDelegate = on_complete;
      on_complete = (GJCommons.VoidDelegate) null;
      voidDelegate();
    }) : (GJCommons.VoidDelegate) (() =>
    {
      CameraTools.StopCinematics();
      this.RestoreSpeedAfterWalking();
      this.OnComplete();
    });
    this.on_failed = on_failed;
  }

  public void OnComplete()
  {
    if (this.wgo.is_player)
      Debug.Log((object) "Movement.OnComplete");
    if (this.wgo.is_player)
      this.player_controlled_by_script = false;
    GJCommons.VoidDelegate onComplete = this.on_complete;
    this.on_complete = (GJCommons.VoidDelegate) null;
    this.on_failed = (GJCommons.VoidDelegate) null;
    string eventOnComplete = this.event_on_complete;
    this.event_on_complete = "";
    if (this.wgo.is_player && this.cur_astar_path != null && this.cur_astar_path.Count > 0)
    {
      this.wgo.tf.position = this.cur_astar_path[this.cur_astar_path.Count - 1] with
      {
        z = this.wgo.tf.position.z
      };
      this.cur_astar_path = new List<Vector3>();
    }
    if (!string.IsNullOrEmpty(this._target_gd_point_tag))
    {
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(this._target_gd_point_tag);
      this._target_gd_point_tag = string.Empty;
      if ((UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null)
        this.wgo.OnCameToGDPoint(gdPointByGdTag);
    }
    if (this.wgo.obj_def != null && this.wgo.obj_def.IsNPC())
      this.wgo.RedrawBubble();
    onComplete.TryInvoke();
    if (string.IsNullOrEmpty(eventOnComplete))
      return;
    this.wgo.FireEvent(eventOnComplete);
  }

  public void OnPathFound()
  {
    this.path_waypoint = 0;
    this.dir_before_path_finding = Vector2.zero;
    this.CacheCurAstarPath();
    this.ProceedToNextPathPoint();
  }

  public void OnGDPointsPathFound()
  {
    this.path_waypoint = 0;
    this.CacheCurAstarPath();
    this.cur_astar_path.Insert(0, (Vector3) this._nearest_gd_point_pos);
    this.ProceedToNextPathPoint();
  }

  public void CacheCurAstarPath()
  {
    this.cur_astar_path = new List<Vector3>();
    if (this.astar.path == null || this.astar.path.Count == 0)
    {
      Debug.LogError((object) "Can not cache astar path: Incorrect path! #path#", (UnityEngine.Object) this.wgo);
    }
    else
    {
      foreach (Vector3 vector3 in this.astar.path)
        this.cur_astar_path.Add(vector3);
    }
  }

  public void OnPathFailed()
  {
    Debug.LogError((object) $"Failed pathfinding! [{this.wgo.obj_id}]", (UnityEngine.Object) this.wgo);
    this.on_failed.TryInvoke();
    this.on_failed = (GJCommons.VoidDelegate) null;
    this.ChangeMovementState(MovementComponent.MovementState.None);
  }

  public void GoToVector(
    Vector2 dir,
    float dist,
    GJCommons.VoidDelegate on_complete = null,
    string event_on_complete = "")
  {
    this.ChangeMovementState(MovementComponent.MovementState.GoToVector);
    this.goto_vector_dir = dir.normalized;
    this.goto_vector_dist = dist;
    this.on_complete = on_complete;
    this.event_on_complete = event_on_complete;
  }

  public void FollowTarget(
    WorldGameObject target,
    float min_follow_dist = -1f,
    GJCommons.VoidDelegate on_complete = null,
    bool with_cinematic = false,
    string event_on_complete = "")
  {
    this.FollowTarget(target.tf, min_follow_dist, on_complete, with_cinematic, event_on_complete);
  }

  public void FollowTarget(
    Transform target,
    float min_follow_dist = -1f,
    GJCommons.VoidDelegate on_complete = null,
    bool with_cinematic = false,
    string event_on_complete = "")
  {
    if ((double) this.tf.DirTo(target).magnitude < (double) this.min_follow_dist)
    {
      on_complete.TryInvoke();
      this.ChangeMovementState(MovementComponent.MovementState.None);
      this.StopImmediate();
    }
    else
    {
      this.ChangeMovementState(MovementComponent.MovementState.Following);
      this.target = target;
      this.min_follow_dist = min_follow_dist;
      this.on_complete = on_complete;
      this.event_on_complete = event_on_complete;
      this.follow_pathfinding_delay = this.walls_check_delay = 0.0f;
      this.astar_following = true;
      if (!with_cinematic)
        return;
      CameraTools.PlayCinematics(this.wgo, 0.5f, float.MaxValue);
    }
  }

  public void StopFollowByDelay(float delay)
  {
    this.follow_delay = delay;
    if ((double) this.follow_delay <= 0.0)
      return;
    this.UpdateMovement(Vector2.zero, 0.0f);
  }

  public void StopMovement()
  {
    this.ChangeMovementState(MovementComponent.MovementState.None);
    this.StopImmediate();
  }

  public void StopTargetFollowing()
  {
    if (this.state == MovementComponent.MovementState.None)
      return;
    this.StopMovement();
  }

  public void SetMovementDir(Vector2 dir)
  {
    this.movement_dir = dir;
    if (this.on_move_dir == null)
      return;
    this.on_move_dir(dir);
  }

  public void UpdateGoToVector()
  {
    this.goto_vector_dist -= this.last_step;
    if ((double) this.goto_vector_dist <= 0.0)
    {
      this.OnComplete();
      this.ChangeMovementState(MovementComponent.MovementState.None);
    }
    else
      this.SetMovementDir(this.goto_vector_dir.normalized);
  }

  public void UpdateFollowing(float delta_time)
  {
    if (!this.is_following_target)
      return;
    this.follow_delay -= delta_time;
    if ((double) this.follow_delay > 0.0)
      return;
    Vector2 vector2 = this.tf.DirTo(this.target);
    if ((double) vector2.magnitude < (double) this.min_follow_dist)
    {
      this.OnComplete();
      this.ChangeMovementState(MovementComponent.MovementState.None);
      this.StopImmediate();
    }
    else
    {
      this.walls_check_delay -= delta_time;
      if ((double) this.walls_check_delay <= 0.0)
      {
        this.no_walls_to_target = Physics2D.RaycastAll(this.wgo.pos, vector2.normalized, vector2.magnitude * 96f, 1).Length == 0;
        if (this.no_walls_to_target)
          this.follow_pathfinding_delay = 0.0f;
        this.walls_check_delay = 0.1f;
      }
      this.follow_pathfinding_delay -= delta_time;
      if ((double) this.follow_pathfinding_delay <= 0.0 && !this.no_walls_to_target)
      {
        this.GoTo((Vector2) this.target.position);
        this.follow_pathfinding_delay = 0.5f;
      }
      if (this.no_walls_to_target)
        this.SetMovementDir(vector2.normalized);
      else if (this.astar.finding || this.astar.not_avaible)
        this.SetMovementDir((double) this.dir_before_path_finding.magnitude > 0.0 ? this.dir_before_path_finding : vector2.normalized);
      else
        this.UpdatePathfinding(Time.deltaTime);
    }
  }

  public void ProceedToNextPathPoint(bool immediate_stop = true)
  {
    if (this.cur_astar_path == null || ++this.path_waypoint < this.cur_astar_path.Count)
      return;
    this.OnCameToLastPoint(immediate_stop);
  }

  public void OnCameToLastPoint(bool stop = true)
  {
    if (!this.astar_following)
    {
      if (this.wgo.is_player)
        this.wgo.tf.position = (Vector3) this.astar.destination;
      this.ChangeMovementState(MovementComponent.MovementState.None, false);
      if (stop || this.wgo.is_player)
        this.StopImmediate();
    }
    this.astar.Clear();
    this.OnComplete();
  }

  public SerializableWGO.SerializebleMovementComponent GetSerializedMovementComponent()
  {
    return new SerializableWGO.SerializebleMovementComponent()
    {
      avaliable = this.enabled,
      cur_astar_path = JsonUtilityHelper.ToJsonList<Vector3>(this.cur_astar_path),
      path_waypoint = this.path_waypoint,
      state = this.state,
      event_on_complete = this.event_on_complete,
      anchor_gd_tag = this.components.character.anchor_obj_gd_point_tag,
      anchor_custom_tag = this.components.character.anchor_obj_wgo_custom_tag,
      anchor_is_wgo = this.components.character.anchor_is_wgo,
      using_gd_path = this._using_gd_graph,
      idle_animation = this.idle_animation,
      target_gd_point_tag = this._target_gd_point_tag,
      astar_dest = this._astar == null ? Vector2.zero : this._astar.destination,
      current_point_pos = this.current_point_pos,
      current_pos = this.current_pos,
      stored_speed = this._stored_speed,
      in_stored_speed_mode = this._in_stored_speed_mode
    };
  }

  public void DeserializeMovementComponent(SerializableWGO.SerializebleMovementComponent data)
  {
    if (data.state != MovementComponent.MovementState.None)
      Debug.Log((object) ("Deserializing moving object: " + data.state.ToString()), (UnityEngine.Object) this.wgo);
    this.enabled = data.avaliable;
    this.cur_astar_path = JsonUtilityHelper.FromJsonList<Vector3>(data.cur_astar_path);
    this.path_waypoint = data.path_waypoint;
    this.state = data.state;
    this.event_on_complete = data.event_on_complete;
    this.components.character.anchor_obj_gd_point_tag = data.anchor_gd_tag;
    this.components.character.anchor_obj_wgo_custom_tag = data.anchor_custom_tag;
    this.components.character.anchor_is_wgo = data.anchor_is_wgo;
    this._using_gd_graph = data.using_gd_path;
    this.idle_animation = data.idle_animation;
    this._target_gd_point_tag = data.target_gd_point_tag;
    if (!data.astar_dest.sqrMagnitude.EqualsTo(0.0f))
      this.astar.RestoreSerialized(data.astar_dest);
    this.current_point_pos = data.current_point_pos;
    this.current_pos = data.current_pos;
    this._stored_speed = data.stored_speed;
    this._in_stored_speed_mode = data.in_stored_speed_mode;
  }

  public void SetMovementModifiers(
    MovementComponent.Modifier acceleration,
    MovementComponent.Modifier friction,
    MovementComponent.Modifier accel_always,
    MovementComponent.Modifier speed)
  {
    this._mod_accel = acceleration;
    this._mod_friction = friction;
    this._mod_accel_always = accel_always;
    this._mod_speed = speed;
  }

  public void Unstuck() => this.state = MovementComponent.MovementState.GoTo;

  public delegate void OnMove(Vector2 dir);

  [Serializable]
  public enum MovementState
  {
    None,
    Following,
    GoTo,
    GoToVector,
    AnimCurve,
    Dash,
  }

  public enum GoToMethod
  {
    AStar,
    GDGraph,
    Direct,
  }

  [Serializable]
  public class Modifier
  {
    public float value;
    public float dec_speed;

    public Modifier(float v = 0.0f, float decreace_speed = 0.1f)
    {
      this.value = v;
      this.dec_speed = decreace_speed;
    }

    public void Update(float delta_time)
    {
      if (this.value.EqualsTo(0.0f))
        return;
      float num = Mathf.Sign(this.value);
      this.value -= this.dec_speed * delta_time * num;
      if ((double) Mathf.Sign(this.value) == (double) num)
        return;
      this.value = 0.0f;
    }
  }
}
