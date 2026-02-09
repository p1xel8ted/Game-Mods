// Decompiled with JetBrains decompiler
// Type: SmartAnimationController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SmartAnimationController : MonoBehaviour
{
  public SmartAnimationController.ObjectType object_type;
  public string _object_type_message = "";
  [Space(10f)]
  public List<SmartAnimationController.Animation> animations = new List<SmartAnimationController.Animation>();
  public List<string> _state_names = new List<string>();
  public List<string> _trigger_names = new List<string>();
  public List<string> _param_names = new List<string>();
  public float _t;
  public const float REROLL_PERIOD = 1f;
  public Animator _cached_animator;
  public WorldGameObject _cached_wgo;
  public List<SmartAnimationController> linked_animators = new List<SmartAnimationController>();
  [NonSerialized]
  public static List<string> _trigger_names_array;
  [Header("Roll triggers on start")]
  [Space(10f)]
  public List<SmartAnimationController.TriggerGroup> trigger_groups = new List<SmartAnimationController.TriggerGroup>();

  public bool Editor_ValidateObjectType(SmartAnimationController.ObjectType object_type)
  {
    this._object_type_message = "";
    switch (object_type)
    {
      case SmartAnimationController.ObjectType.Character:
        this._object_type_message = "Character - animated object which can switch animations and don't roll triggers during cutscenes.";
        break;
      case SmartAnimationController.ObjectType.Object:
        this._object_type_message = "Object - non-character object, which animations don't depend on cutscenes, walking phase, etc.";
        break;
    }
    return true;
  }

  public void OnCustomInspectorGUI()
  {
    GJEditorAnimatorHelper.ScanAnimator(this.gameObject, ref this._state_names, ref this._trigger_names, ref this._param_names);
    SmartAnimationController._trigger_names_array = this._trigger_names;
  }

  public void Start()
  {
    foreach (SmartAnimationController.TriggerGroup triggerGroup in this.trigger_groups)
    {
      if (triggerGroup.triggers != null && triggerGroup.triggers.Count != 0)
      {
        int index = Mathf.FloorToInt(UnityEngine.Random.Range(0.0f, (float) triggerGroup.triggers.Count - 1f / 1000f));
        if (index >= triggerGroup.triggers.Count)
          --index;
        this.CheckComponentsCache();
        this._cached_animator.SetTrigger(triggerGroup.triggers[index].trigger);
      }
    }
  }

  public void Update()
  {
    this._t += Time.deltaTime;
    if ((double) this._t < 1.0)
      return;
    this._t = 1f;
    foreach (SmartAnimationController.Animation animation in this.animations)
    {
      if (!animation.executed_with_trigger)
      {
        animation.cur_period += this._t;
        if ((double) animation.cur_period >= (double) animation.min_period)
        {
          float num = 1f / (float) animation.est_period;
          if ((double) UnityEngine.Random.Range(0.0f, 1f) < (double) num && (this.object_type != SmartAnimationController.ObjectType.Character || MainGame.me.player_char.control_enabled) && this.IsAnimationCanBeTriggered(animation))
          {
            animation.cur_period = 0.0f;
            this._cached_animator.SetTrigger(animation.trigger);
          }
        }
      }
    }
    this._t = 0.0f;
  }

  public bool IsAnimationCanBeTriggered(SmartAnimationController.Animation a)
  {
    this.CheckComponentsCache();
    if (this.object_type == SmartAnimationController.ObjectType.Object)
      return true;
    BaseCharacterComponent character = this._cached_wgo.components.character;
    if (character == null)
    {
      Debug.LogError((object) "BaseCharacterComponent not found", (UnityEngine.Object) this);
      return false;
    }
    if (character.anim_state != CharAnimState.Idle && character.anim_state != CharAnimState.Disabled)
      return false;
    if (a.dir == SmartAnimationController.AnimationDirectionLimit.Any)
      return true;
    Direction direction = character.direction.ToDirection();
    switch (a.dir)
    {
      case SmartAnimationController.AnimationDirectionLimit.Down:
        return direction == Direction.Down;
      case SmartAnimationController.AnimationDirectionLimit.Left:
        return direction == Direction.Left;
      case SmartAnimationController.AnimationDirectionLimit.Right:
        return direction == Direction.Right;
      case SmartAnimationController.AnimationDirectionLimit.Up:
        return direction == Direction.Up;
      default:
        return true;
    }
  }

  public void TriggerAimation(string smart_trigger_id)
  {
    this.CheckComponentsCache();
    bool flag = false;
    foreach (SmartAnimationController.Animation animation in this.animations)
    {
      if (animation.id == smart_trigger_id)
      {
        flag = true;
        if (this.IsAnimationCanBeTriggered(animation))
        {
          animation.cur_period = 0.0f;
          this._cached_animator.SetTrigger(animation.trigger);
        }
      }
    }
    if (flag)
      return;
    Debug.LogWarning((object) ("Smart trigger not found = " + smart_trigger_id), (UnityEngine.Object) this);
  }

  public void CheckComponentsCache()
  {
    if (!Application.isPlaying || (UnityEngine.Object) this._cached_animator == (UnityEngine.Object) null)
      this._cached_animator = this.GetComponentInChildren<Animator>();
    if (this.object_type != SmartAnimationController.ObjectType.Character || Application.isPlaying && !((UnityEngine.Object) this._cached_wgo == (UnityEngine.Object) null))
      return;
    this._cached_wgo = this.GetComponentsInParent<WorldGameObject>(true)[0];
  }

  public void TriggerOtherAnimation(int index, string triggers)
  {
    if (index >= this.linked_animators.Count)
    {
      Debug.LogError((object) $"TriggerOtherAnimation: Can't trigger animator #{index.ToString()} because there's only {this.linked_animators.Count.ToString()} in the list");
    }
    else
    {
      string str = triggers;
      char[] separator = new char[1]{ ',' };
      foreach (string smart_trigger_id in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        this.linked_animators[index].TriggerAimation(smart_trigger_id);
    }
  }

  public void TriggerOtherAnimation0(string smart_trigger)
  {
    this.TriggerOtherAnimation(0, smart_trigger);
  }

  public void TriggerOtherAnimation1(string smart_trigger)
  {
    this.TriggerOtherAnimation(1, smart_trigger);
  }

  public void TriggerOtherAnimation2(string smart_trigger)
  {
    this.TriggerOtherAnimation(2, smart_trigger);
  }

  public void TriggerOtherAnimation3(string smart_trigger)
  {
    this.TriggerOtherAnimation(3, smart_trigger);
  }

  public void TriggerOtherAnimation4(string smart_trigger)
  {
    this.TriggerOtherAnimation(4, smart_trigger);
  }

  public enum ObjectType
  {
    Character,
    Object,
  }

  public enum AnimationDirectionLimit
  {
    Any,
    Down,
    Left,
    Right,
    Up,
  }

  [Serializable]
  public struct TriggerGroup
  {
    public List<SmartAnimationController.TriggerDefine> triggers;
  }

  [Serializable]
  public struct TriggerDefine
  {
    public string trigger;

    public List<string> _trigger_names_array => SmartAnimationController._trigger_names_array;
  }

  [Serializable]
  public class Animation
  {
    public string id;
    public string trigger;
    public SmartAnimationController.AnimationDirectionLimit dir;
    public bool executed_with_trigger = true;
    public int min_period = 10;
    public int est_period = 30;
    [NonSerialized]
    public float cur_period;

    public bool NoPeriods() => this.executed_with_trigger;

    public List<string> _trigger_names_array => SmartAnimationController._trigger_names_array;
  }
}
