// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Task
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using ParadoxNotion.Services;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
[Serializable]
public abstract class Task
{
  [SerializeField]
  public bool _isDisabled;
  [SerializeField]
  public TaskAgent overrideAgent;
  [NonSerialized]
  public IBlackboard _blackboard;
  [NonSerialized]
  public ITaskSystem _ownerSystem;
  [NonSerialized]
  public Component current;
  [NonSerialized]
  public bool _agentTypeInit;
  [NonSerialized]
  public System.Type _agentType;
  [NonSerialized]
  public string _taskName;
  [NonSerialized]
  public string _taskDescription;
  [NonSerialized]
  public string _obsoleteInfo;
  [CompilerGenerated]
  public string \u003CfirstWarningMessage\u003Ek__BackingField;

  public static T Create<T>(ITaskSystem newOwnerSystem) where T : Task
  {
    return (T) Task.Create(typeof (T), newOwnerSystem);
  }

  public static Task Create(System.Type type, ITaskSystem newOwnerSystem)
  {
    Task instance = (Task) Activator.CreateInstance(type);
    newOwnerSystem.RecordUndo("New Task");
    instance.SetOwnerSystem(newOwnerSystem);
    BBParameter.SetBBFields((object) instance, newOwnerSystem.blackboard);
    instance.OnValidate(newOwnerSystem);
    instance.OnCreate(newOwnerSystem);
    return instance;
  }

  public virtual Task Duplicate(ITaskSystem newOwnerSystem)
  {
    Task o = JSONSerializer.Clone<Task>(this);
    newOwnerSystem.RecordUndo("Duplicate Task");
    o.SetOwnerSystem(newOwnerSystem);
    BBParameter.SetBBFields((object) o, newOwnerSystem.blackboard);
    o.OnValidate(newOwnerSystem);
    return o;
  }

  public virtual void OnCreate(ITaskSystem ownerSystem)
  {
  }

  public virtual void OnValidate(ITaskSystem ownerSystem)
  {
  }

  public void SetOwnerSystem(ITaskSystem newOwnerSystem)
  {
    if (newOwnerSystem == null)
      ParadoxNotion.Services.Logger.LogError((object) "ITaskSystem set in task is null!!", "Init", (object) this);
    else
      this.ownerSystem = newOwnerSystem;
  }

  public ITaskSystem ownerSystem
  {
    get => this._ownerSystem;
    set => this._ownerSystem = value;
  }

  public Component ownerAgent
  {
    get => this.ownerSystem == null ? (Component) null : this.ownerSystem.agent;
  }

  public IBlackboard ownerBlackboard
  {
    get => this.ownerSystem == null ? (IBlackboard) null : this.ownerSystem.blackboard;
  }

  public float ownerElapsedTime => this.ownerSystem == null ? 0.0f : this.ownerSystem.elapsedTime;

  public bool isActive
  {
    get => !this._isDisabled;
    set => this._isDisabled = !value;
  }

  public string obsolete
  {
    get
    {
      if (this._obsoleteInfo == null)
      {
        ObsoleteAttribute attribute = ReflectionTools.RTGetAttribute<ObsoleteAttribute>(this.GetType(), true);
        this._obsoleteInfo = attribute != null ? attribute.Message : string.Empty;
      }
      return this._obsoleteInfo;
    }
  }

  public string name
  {
    get
    {
      if (this._taskName == null)
      {
        NameAttribute attribute = ReflectionTools.RTGetAttribute<NameAttribute>(this.GetType(), false);
        this._taskName = attribute != null ? attribute.name : this.GetType().FriendlyName().SplitCamelCase();
      }
      return this._taskName;
    }
  }

  public string description
  {
    get
    {
      if (this._taskDescription == null)
      {
        DescriptionAttribute attribute = ReflectionTools.RTGetAttribute<DescriptionAttribute>(this.GetType(), true);
        this._taskDescription = attribute != null ? attribute.description : string.Empty;
      }
      return this._taskDescription;
    }
  }

  public virtual System.Type agentType => (System.Type) null;

  public string summaryInfo
  {
    get
    {
      switch (this)
      {
        case ActionTask _:
          return (this.agentIsOverride ? "* " : "") + this.info;
        case ConditionTask _:
          return (this.agentIsOverride ? "* " : "") + ((this as ConditionTask).invert ? "If <b>!</b> " : "If ") + this.info;
        default:
          return this.info;
      }
    }
  }

  public virtual string info => this.name;

  public string agentInfo
  {
    get => this.overrideAgent == null ? "<b>owner</b>" : this.overrideAgent.ToString();
  }

  public bool agentIsOverride
  {
    get => this.overrideAgent != null;
    set
    {
      if (!value && this.overrideAgent != null)
        this.overrideAgent = (TaskAgent) null;
      if (!value || this.overrideAgent != null)
        return;
      this.overrideAgent = new TaskAgent();
      this.overrideAgent.bb = this.blackboard;
    }
  }

  public string overrideAgentParameterName
  {
    get => this.overrideAgent == null ? (string) null : this.overrideAgent.name;
  }

  public Component agent
  {
    get
    {
      return (UnityEngine.Object) this.current != (UnityEngine.Object) null ? this.current : Task.TransformAgent(this.agentIsOverride ? (Component) this.overrideAgent.value : this.ownerAgent, this.agentType);
    }
  }

  public IBlackboard blackboard
  {
    get => this._blackboard;
    set
    {
      if (this._blackboard == value)
        return;
      this._blackboard = value;
      BBParameter.SetBBFields((object) this, value);
      if (this.overrideAgent == null)
        return;
      this.overrideAgent.bb = value;
    }
  }

  public string firstWarningMessage
  {
    get => this.\u003CfirstWarningMessage\u003Ek__BackingField;
    set => this.\u003CfirstWarningMessage\u003Ek__BackingField = value;
  }

  public Coroutine StartCoroutine(IEnumerator routine)
  {
    return MonoManager.current.StartCoroutine(routine);
  }

  public void StopCoroutine(Coroutine routine) => MonoManager.current.StopCoroutine(routine);

  public void SendEvent(string eventName) => this.SendEvent(new EventData(eventName));

  public void SendEvent<T>(string eventName, T value)
  {
    this.SendEvent((EventData) new EventData<T>(eventName, value));
  }

  public void SendEvent(EventData eventData)
  {
    if (this.ownerSystem == null)
      return;
    this.ownerSystem.SendEvent(eventData);
  }

  public virtual string OnInit() => (string) null;

  public bool Set(Component newAgent, IBlackboard newBB)
  {
    this.blackboard = newBB;
    if (this.agentIsOverride)
      newAgent = (Component) this.overrideAgent.value;
    return (UnityEngine.Object) this.current != (UnityEngine.Object) null && (UnityEngine.Object) newAgent != (UnityEngine.Object) null && (UnityEngine.Object) this.current.gameObject == (UnityEngine.Object) newAgent.gameObject ? (this.isActive = true) : (this.isActive = this.Initialize(newAgent));
  }

  public static Component TransformAgent(Component currentAgent, System.Type type)
  {
    if ((UnityEngine.Object) currentAgent != (UnityEngine.Object) null && System.Type.op_Inequality(type, (System.Type) null) && !type.RTIsAssignableFrom(currentAgent.GetType()) && (type.RTIsSubclassOf(typeof (Component)) || type.RTIsInterface()))
      currentAgent = currentAgent.GetComponent(type);
    return currentAgent;
  }

  public bool Initialize(Component newAgent)
  {
    this.UnRegisterAllEvents();
    newAgent = Task.TransformAgent(newAgent, this.agentType);
    this.current = newAgent;
    if ((UnityEngine.Object) newAgent == (UnityEngine.Object) null && System.Type.op_Inequality(this.agentType, (System.Type) null))
      return this.Error($"Failed to resolve Agent to requested type '{this.agentType?.ToString()}', or new Agent is NULL. Does the Agent has the requested Component?");
    Task.EventReceiverAttribute attribute = ReflectionTools.RTGetAttribute<Task.EventReceiverAttribute>(this.GetType(), true);
    if (attribute != null)
      this.RegisterEvents(attribute.eventMessages);
    if (!this.InitializeAttributes(newAgent))
      return false;
    string error = this.OnInit();
    return error == null || this.Error(error);
  }

  public bool InitializeAttributes(Component newAgent)
  {
    foreach (FieldInfo field in this.GetType().RTGetFields())
    {
      if ((UnityEngine.Object) newAgent != (UnityEngine.Object) null && typeof (Component).RTIsAssignableFrom(field.FieldType) && field.RTIsDefined<Task.GetFromAgentAttribute>(true))
      {
        Component component = newAgent.GetComponent(field.FieldType);
        field.SetValue((object) this, (object) component);
        if (component == null)
          return this.Error($"GetFromAgent Attribute failed to get the required Component of type '{field.FieldType.Name}' from '{this.agent.gameObject.name}'. Does it exist?");
      }
    }
    return true;
  }

  public bool Error(string error)
  {
    ParadoxNotion.Services.Logger.LogError((object) $"{error} | {(this.ownerSystem != null ? (object) this.ownerSystem.contextObject : (object) (UnityEngine.Object) null)}", nameof (Task), (object) this);
    return false;
  }

  public void RegisterEvent(string eventName) => this.RegisterEvents(eventName);

  public void RegisterEvents(params string[] eventNames)
  {
    if ((UnityEngine.Object) this.agent == (UnityEngine.Object) null)
      return;
    MessageRouter messageRouter = this.agent.GetComponent<MessageRouter>();
    if ((UnityEngine.Object) messageRouter == (UnityEngine.Object) null)
      messageRouter = this.agent.gameObject.AddComponent<MessageRouter>();
    messageRouter.Register((object) this, eventNames);
  }

  public void UnRegisterEvent(string eventName) => this.UnRegisterEvents(eventName);

  public void UnRegisterEvents(params string[] eventNames)
  {
    if ((UnityEngine.Object) this.agent == (UnityEngine.Object) null)
      return;
    MessageRouter component = this.agent.GetComponent<MessageRouter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.UnRegister((object) this, eventNames);
  }

  public void UnRegisterAllEvents()
  {
    if ((UnityEngine.Object) this.agent == (UnityEngine.Object) null)
      return;
    MessageRouter component = this.agent.GetComponent<MessageRouter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.UnRegister((object) this);
  }

  public string GetWarning()
  {
    this.firstWarningMessage = this.Internal_GetWarning();
    return this.firstWarningMessage;
  }

  public string Internal_GetWarning()
  {
    if (this.obsolete != string.Empty)
      return $"Task is obsolete: '{this.obsolete}'.";
    if ((UnityEngine.Object) this.agent == (UnityEngine.Object) null && System.Type.op_Inequality(this.agentType, (System.Type) null))
      return $"'{this.agentType.Name}' target is currently null.";
    foreach (FieldInfo field in this.GetType().RTGetFields())
    {
      if (field.RTIsDefined<RequiredFieldAttribute>(true))
      {
        object obj = field.GetValue((object) this);
        if (obj == null || obj.Equals((object) null))
          return $"Required field '{field.Name.SplitCamelCase()}' is currently null.";
        if (System.Type.op_Equality(field.FieldType, typeof (string)) && string.IsNullOrEmpty((string) obj))
          return $"Required string field '{field.Name.SplitCamelCase()}' is currently null or empty.";
        if (typeof (BBParameter).RTIsAssignableFrom(field.FieldType))
        {
          if (!(obj is BBParameter bbParameter))
            return $"BBParameter '{field.Name.SplitCamelCase()}' is null.";
          if (bbParameter.isNull)
            return $"Required parameter '{field.Name.SplitCamelCase()}' is currently null.";
        }
      }
    }
    return (string) null;
  }

  public sealed override string ToString() => this.summaryInfo;

  public virtual void OnDrawGizmos()
  {
  }

  public virtual void OnDrawGizmosSelected()
  {
  }

  [AttributeUsage(AttributeTargets.Class)]
  public class EventReceiverAttribute : Attribute
  {
    public string[] eventMessages;

    public EventReceiverAttribute(params string[] args) => this.eventMessages = args;
  }

  [AttributeUsage(AttributeTargets.Field)]
  public class GetFromAgentAttribute : Attribute
  {
  }
}
