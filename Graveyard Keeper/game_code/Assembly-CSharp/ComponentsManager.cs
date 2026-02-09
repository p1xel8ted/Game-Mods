// Decompiled with JetBrains decompiler
// Type: ComponentsManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using NodeCanvas.Framework;
using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class ComponentsManager
{
  public const int CHAR_LAYER = 9;
  public static System.Type[] ALL_COMPONENTS = new System.Type[9]
  {
    typeof (BaseCharacterComponent),
    typeof (HPActionComponent),
    typeof (CombatComponent),
    typeof (KickComponent),
    typeof (AnimatedBehaviour),
    typeof (TimerComponent),
    typeof (InteractionComponent),
    typeof (CraftComponent),
    typeof (ToolComponent)
  };
  public GameObject _obj;
  public WorldGameObject _wgo;
  public Dictionary<System.Type, WorldGameObjectComponent> _components = new Dictionary<System.Type, WorldGameObjectComponent>();
  public WorldGameObjectComponent[] _components_fast_list = new WorldGameObjectComponent[0];
  public bool _interaction_buttons_shown;
  [NonSerialized]
  public CustomNetworkAnimatorSync _animator;
  public bool _animator_initialized;

  public WorldGameObject wgo => this._wgo;

  public ComponentsManager(WorldGameObject wgo)
  {
    this._wgo = wgo;
    this._obj = wgo.gameObject;
    this._components.Clear();
    foreach (System.Type type in ComponentsManager.ALL_COMPONENTS)
    {
      WorldGameObjectComponent instance = Activator.CreateInstance(type) as WorldGameObjectComponent;
      instance.Init(this._wgo);
      this._components.Add(type, instance);
    }
    this._components_fast_list = this._components.Values.ToArray<WorldGameObjectComponent>();
  }

  public void RefreshComponentsEnableState()
  {
    if (!Application.isPlaying)
      return;
    if (this._wgo.obj_def == null)
    {
      Debug.LogError((object) ("Definition is null on WGO: " + this._wgo.name), (UnityEngine.Object) this._wgo);
    }
    else
    {
      ObjectDefinition.ObjType type = this._wgo.obj_def.type;
      foreach (WorldGameObjectComponent componentsFast in this._components_fast_list)
        componentsFast.UpdateEnableState(type);
    }
  }

  public T GetComponent<T>() where T : WorldGameObjectComponent
  {
    try
    {
      return this._components[typeof (T)] as T;
    }
    catch (KeyNotFoundException ex)
    {
      Debug.LogError((object) ("Component of requested type not found: " + typeof (T).Name), (UnityEngine.Object) this._wgo);
      return default (T);
    }
    catch (NullReferenceException ex)
    {
      WorldGameObject wgo = this._wgo;
      Debug.LogException((Exception) ex, (UnityEngine.Object) wgo);
      if (this._components == null)
        Debug.LogError((object) "_components is null");
      Debug.Log((object) ("typeof(T) = " + typeof (T)?.ToString()));
      return default (T);
    }
  }

  public InteractionComponent interaction => this.GetComponent<InteractionComponent>();

  public ToolComponent tool => this.GetComponent<ToolComponent>();

  public AnimatedBehaviour animated_behaviour => this.GetComponent<AnimatedBehaviour>();

  public CombatComponent combat => this.GetComponent<CombatComponent>();

  public HPActionComponent hp => this.GetComponent<HPActionComponent>();

  public BaseCharacterComponent character => this.GetComponent<BaseCharacterComponent>();

  public KickComponent kick => this.GetComponent<KickComponent>();

  public CraftComponent craft => this.GetComponent<CraftComponent>();

  public DropCollectorComponent drop_colldector => this.GetComponent<DropCollectorComponent>();

  public TimerComponent timer => this.GetComponent<TimerComponent>();

  public AuraReceiver aura_receiver
  {
    get
    {
      AuraReceiver auraReceiver = new AuraReceiver();
      auraReceiver.enabled = false;
      return auraReceiver;
    }
  }

  public AuraEmitter aura_emitter
  {
    get
    {
      AuraEmitter auraEmitter = new AuraEmitter();
      auraEmitter.enabled = false;
      return auraEmitter;
    }
  }

  public CustomNetworkAnimatorSync animator
  {
    get
    {
      if (!this._animator_initialized)
      {
        this._animator = CustomNetworkAnimatorSync.InitAnimator(this.wgo.gameObject);
        this._animator_initialized = (UnityEngine.Object) this._animator != (UnityEngine.Object) null;
      }
      return this._animator;
    }
  }

  public void PreStartComponents()
  {
    if (this._wgo.obj_def == null)
    {
      Debug.LogError((object) $"Object definition is null for obj_id = \"{this._wgo.obj_id}\"", (UnityEngine.Object) this._wgo);
    }
    else
    {
      if (!string.IsNullOrEmpty(this._wgo.custom_tag) || this.craft.is_crafting || !string.IsNullOrEmpty(this._wgo.obj_def.attached_script))
        this.StartComponents();
      if (!this._wgo.obj_def.IsPorterStation())
        return;
      PorterStation porterStation = this._wgo.porter_station;
    }
  }

  public void StartComponents()
  {
    if (this._wgo.obj_def == null)
    {
      Debug.LogError((object) $"Object definition is null for obj_id = \"{this._wgo.obj_id}\"", (UnityEngine.Object) this._wgo);
    }
    else
    {
      ObjectDefinition.ObjType type = this._wgo.obj_def.type;
      foreach (WorldGameObjectComponent componentsFast in this._components_fast_list)
      {
        componentsFast.UpdateEnableState(type);
        if (componentsFast.enabled)
          componentsFast.StartComponent();
      }
    }
  }

  public void Update(float delta_time)
  {
    if (MainGame.paused)
      return;
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled && componentsFast.HasUpdate())
        componentsFast.UpdateComponent(delta_time);
    }
  }

  public void LateUpdate()
  {
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled && componentsFast.HasLateUpdate())
        componentsFast.LateUpdateComponent();
    }
  }

  public void FixedUpdate()
  {
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled && componentsFast.HasFixedUpdate())
        componentsFast.FixedUpdateComponent(Time.fixedDeltaTime);
    }
  }

  public bool DoAction(WorldGameObject other_obj, float delta_time)
  {
    bool flag = false;
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled && componentsFast.DoAction(other_obj, delta_time))
        flag = true;
    }
    return flag;
  }

  public void Interact(WorldGameObject other_obj, bool interaction_start, float delta_time = -1f)
  {
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled)
        componentsFast.Interact(other_obj, delta_time);
    }
  }

  public void GetAllComponentsAndSort()
  {
  }

  public void InitAllComponents()
  {
    this._animator_initialized = false;
    this.RefreshComponentsEnableState();
    if (this._components_fast_list == null)
    {
      if (this._components == null)
      {
        Debug.LogError((object) ("Broken components manager at WGO " + this.wgo.name), (UnityEngine.Object) this.wgo);
        this._components = new Dictionary<System.Type, WorldGameObjectComponent>();
      }
      this._components_fast_list = this._components.Values.ToArray<WorldGameObjectComponent>();
    }
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled)
        componentsFast.InitComponent();
    }
    if (this.character == null)
      return;
    this.character.Recache();
  }

  public virtual void PrepareForInteraction(BaseCharacterComponent for_whom)
  {
    foreach (WorldGameObjectComponent componentsFast in this._components_fast_list)
    {
      if (componentsFast.enabled)
        componentsFast.PrepareForInteraction(for_whom);
    }
  }

  public virtual void UnprepareForInteraction()
  {
    for (int index = 0; index < this._components_fast_list.Length; ++index)
    {
      WorldGameObjectComponent componentsFast = this._components_fast_list[index];
      if (componentsFast.enabled)
        componentsFast.UnprepareForInteraction();
    }
  }

  public void UpdateComponentsSet()
  {
    this.CheckCharacterStuff(this._wgo.obj_def.IsCharacter());
    this.GetAllComponentsAndSort();
    this.InitAllComponents();
  }

  public void CheckCharacterStuff(bool is_character)
  {
    GameObject gameObject = this._wgo.gameObject;
    Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    Seeker component1 = gameObject.GetComponent<Seeker>();
    Blackboard component2 = gameObject.GetComponent<Blackboard>();
    if (is_character)
    {
      if ((UnityEngine.Object) rigidbody2D == (UnityEngine.Object) null)
        rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
      if (!this.wgo.is_player)
      {
        ObjectDefinition objDef = this.wgo.obj_def;
        rigidbody2D.mass = objDef.mass;
        rigidbody2D.drag = objDef.drag;
      }
      else
      {
        rigidbody2D.mass = 80f;
        rigidbody2D.drag = 50f;
      }
      rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
      rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAsleep;
      rigidbody2D.interpolation = RigidbodyInterpolation2D.Extrapolate;
      rigidbody2D.freezeRotation = true;
      if (gameObject.layer == 9)
        return;
      gameObject.layer = 9;
    }
    else
    {
      if ((UnityEngine.Object) rigidbody2D != (UnityEngine.Object) null)
        this.Destroy((UnityEngine.Object) rigidbody2D);
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        SimpleSmoothModifierXY component3 = gameObject.GetComponent<SimpleSmoothModifierXY>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          this.Destroy((UnityEngine.Object) component3);
        this.Destroy((UnityEngine.Object) component1);
      }
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        this.Destroy((UnityEngine.Object) component2);
      if (gameObject.layer != 9)
        return;
      Debug.LogWarning((object) $"Non character obj ({this._wgo.name}) has Character layer, changed to Default, wgo.def.type = {this._wgo.obj_def.type.ToString()}, id = {this._wgo.obj_def.id}");
      gameObject.layer = 0;
    }
  }

  public void Destroy(UnityEngine.Object obj)
  {
    if (Application.isPlaying)
      UnityEngine.Object.Destroy(obj);
    else
      UnityEngine.Object.DestroyImmediate(obj);
  }

  public void RefreshBubblesData(bool? show_interaction_buttons)
  {
    this.wgo.SetBubbleWidgetData("", BubbleWidgetData.WidgetID.Work);
    this.wgo.SetBubbleWidgetData("", BubbleWidgetData.WidgetID.Interaction);
    if (show_interaction_buttons.HasValue)
      this._interaction_buttons_shown = show_interaction_buttons.Value;
    foreach (WorldGameObjectComponent componentsFast in this._components_fast_list)
    {
      if (componentsFast.enabled)
        componentsFast.RefreshComponentBubbleData(this._interaction_buttons_shown);
    }
    ObjectDefinition objDef = this.wgo.obj_def;
    if (this._interaction_buttons_shown && !this.wgo.bubble.DoesContainWidgetDataWithID(BubbleWidgetData.WidgetID.Interaction) && !this.wgo.bubble.DoesContainWidgetDataWithID(BubbleWidgetData.WidgetID.Work))
    {
      CustomInteractionHint component = this.wgo.GetComponent<CustomInteractionHint>();
      ObjectInteractionDefinition validInteraction = objDef.GetValidInteraction(this.wgo);
      string text;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.has_hint)
        text = component.GetHint();
      else if (validInteraction != null)
      {
        text = validInteraction.hint;
        if (this.craft.enabled && this.craft.is_crafting && this.craft.current_craft.is_auto && !this.craft.current_craft.hidden)
          text = (string) null;
      }
      else
      {
        text = objDef.GetInteractionHint(this.wgo);
        if (string.IsNullOrEmpty(text))
        {
          switch (objDef.interaction_type)
          {
            case ObjectDefinition.InteractionType.Craft:
              text = "craft";
              break;
            case ObjectDefinition.InteractionType.Builder:
              text = "build";
              break;
            case ObjectDefinition.InteractionType.Chest:
              text = "open";
              break;
            case ObjectDefinition.InteractionType.Grave:
              text = "open grave";
              break;
          }
        }
        if (this.craft.enabled && this.craft.is_crafting && this.craft.current_craft.is_auto)
          text = (string) null;
      }
      if (!string.IsNullOrEmpty(text))
        text = GameKeyTip.Get(GameKey.Interaction, text);
      this.wgo.SetBubbleWidgetData(text, BubbleWidgetData.WidgetID.Interaction);
    }
    else if (!this._interaction_buttons_shown)
      this.wgo.SetBubbleWidgetData("", BubbleWidgetData.WidgetID.Interaction);
    if (this.wgo.custom_interaction_events.Count > 0 && !this.wgo.IsMoving())
    {
      string text = string.IsNullOrEmpty(this.wgo.obj_def.custom_interaction_icon) ? (this.wgo.components.character.enabled ? "(speak)" : "(view)") : this.wgo.obj_def.custom_interaction_icon;
      if (this._interaction_buttons_shown)
        text = GameKeyTip.Get(GameKey.Interaction, text);
      this.wgo.SetBubbleWidgetData(text, BubbleWidgetData.WidgetID.Interaction);
    }
    this.wgo.SetBubbleWidgetData((BubbleWidgetData) this.wgo.GetQualityWidgetData(), BubbleWidgetData.WidgetID.Quality);
  }
}
