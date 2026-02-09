// Decompiled with JetBrains decompiler
// Type: NodeCanvas.ActionListPlayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas;

[AddComponentMenu("NodeCanvas/Action List")]
public class ActionListPlayer : MonoBehaviour, ITaskSystem, ISerializationCallbackReceiver
{
  [SerializeField]
  public string _serializedList;
  [SerializeField]
  public List<UnityEngine.Object> _objectReferences;
  [NonSerialized]
  public ActionList _actionList;
  [SerializeField]
  public Blackboard _blackboard;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    this._actionList = JSONSerializer.Deserialize<ActionList>(this._serializedList, this._objectReferences);
    if (this._actionList != null)
      return;
    this._actionList = (ActionList) Task.Create(typeof (ActionList), (ITaskSystem) this);
  }

  public ActionList actionList => this._actionList;

  Component ITaskSystem.agent => (Component) this;

  public IBlackboard blackboard
  {
    get => (IBlackboard) this._blackboard;
    set
    {
      if (this._blackboard == value)
        return;
      this._blackboard = (Blackboard) value;
      this.SendTaskOwnerDefaults();
    }
  }

  public float elapsedTime => this.actionList.elapsedTime;

  UnityEngine.Object ITaskSystem.contextObject => (UnityEngine.Object) this;

  public static ActionListPlayer Create()
  {
    return new GameObject("ActionList").AddComponent<ActionListPlayer>();
  }

  public void SendTaskOwnerDefaults()
  {
    this.actionList.SetOwnerSystem((ITaskSystem) this);
    foreach (Task action in this.actionList.actions)
      action.SetOwnerSystem((ITaskSystem) this);
  }

  void ITaskSystem.SendEvent(EventData eventData)
  {
    Debug.LogWarning((object) "Sending events to action lists has no effect");
  }

  void ITaskSystem.RecordUndo(string name)
  {
  }

  public void Awake() => this.SendTaskOwnerDefaults();

  [ContextMenu("Play")]
  public void Play() => this.Play((Component) this, this.blackboard, (Action<bool>) null);

  public void Play(Action<bool> OnFinish) => this.Play((Component) this, this.blackboard, OnFinish);

  public void Play(Component agent, IBlackboard blackboard, Action<bool> OnFinish)
  {
    if (!Application.isPlaying)
      return;
    this.actionList.ExecuteAction(agent, blackboard, OnFinish);
  }

  public Status ExecuteAction() => this.actionList.ExecuteAction((Component) this, this.blackboard);

  public Status ExecuteAction(Component agent)
  {
    return this.actionList.ExecuteAction(agent, this.blackboard);
  }
}
