// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.DialogueTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[GraphInfo(packageName = "NodeCanvas", docsURL = "http://nodecanvas.paradoxnotion.com/documentation/", resourcesURL = "http://nodecanvas.paradoxnotion.com/downloads/", forumsURL = "http://nodecanvas.paradoxnotion.com/forums-page/")]
public class DialogueTree : Graph
{
  public const string INSTIGATOR_NAME = "INSTIGATOR";
  [SerializeField]
  public List<DialogueTree.ActorParameter> _actorParameters = new List<DialogueTree.ActorParameter>();
  public DTNode currentNode;
  [CompilerGenerated]
  public static DialogueTree \u003CcurrentDialogue\u003Ek__BackingField;
  [CompilerGenerated]
  public static DialogueTree \u003CpreviousDialogue\u003Ek__BackingField;

  public override object OnDerivedDataSerialization()
  {
    return (object) new DialogueTree.DerivedSerializationData()
    {
      actorParameters = this._actorParameters
    };
  }

  public override void OnDerivedDataDeserialization(object data)
  {
    if (!(data is DialogueTree.DerivedSerializationData serializationData))
      return;
    this._actorParameters = serializationData.actorParameters;
  }

  public static event Action<DialogueTree> OnDialogueStarted;

  public static event Action<DialogueTree> OnDialoguePaused;

  public static event Action<DialogueTree> OnDialogueFinished;

  public static event Action<SubtitlesRequestInfo> OnSubtitlesRequest;

  public static event Action<MultipleChoiceRequestInfo> OnMultipleChoiceRequest;

  public static DialogueTree currentDialogue
  {
    get => DialogueTree.\u003CcurrentDialogue\u003Ek__BackingField;
    set => DialogueTree.\u003CcurrentDialogue\u003Ek__BackingField = value;
  }

  public static DialogueTree previousDialogue
  {
    get => DialogueTree.\u003CpreviousDialogue\u003Ek__BackingField;
    set => DialogueTree.\u003CpreviousDialogue\u003Ek__BackingField = value;
  }

  public override System.Type baseNodeType => typeof (DTNode);

  public override bool requiresAgent => false;

  public override bool requiresPrimeNode => true;

  public override bool autoSort => true;

  public override bool useLocalBlackboard => true;

  public List<DialogueTree.ActorParameter> actorParameters => this._actorParameters;

  public List<string> definedActorParameterNames
  {
    get
    {
      List<string> list = this.actorParameters.Select<DialogueTree.ActorParameter, string>((Func<DialogueTree.ActorParameter, string>) (r => r.name)).ToList<string>();
      list.Insert(0, "INSTIGATOR");
      return list;
    }
  }

  public DialogueTree.ActorParameter GetParameterByID(string id)
  {
    return this.actorParameters.Find((Predicate<DialogueTree.ActorParameter>) (p => p.ID == id));
  }

  public DialogueTree.ActorParameter GetParameterByName(string paramName)
  {
    return this.actorParameters.Find((Predicate<DialogueTree.ActorParameter>) (p => p.name == paramName));
  }

  public IDialogueActor GetActorReferenceByID(string id)
  {
    DialogueTree.ActorParameter parameterById = this.GetParameterByID(id);
    return parameterById == null ? (IDialogueActor) null : this.GetActorReferenceByName(parameterById.name);
  }

  public IDialogueActor GetActorReferenceByName(string paramName)
  {
    if (paramName == "INSTIGATOR")
    {
      if (this.agent is IDialogueActor)
        return (IDialogueActor) this.agent;
      return (UnityEngine.Object) this.agent != (UnityEngine.Object) null ? (IDialogueActor) new ProxyDialogueActor(this.agent.gameObject.name, this.agent.transform) : (IDialogueActor) new ProxyDialogueActor("Null Instigator", (Transform) null);
    }
    DialogueTree.ActorParameter actorParameter = this.actorParameters.Find((Predicate<DialogueTree.ActorParameter>) (r => r.name == paramName));
    if (actorParameter != null && actorParameter.actor != null)
      return actorParameter.actor;
    Debug.Log((object) $"<b>DialogueTree:</b> An actor entry '{paramName}' on DialogueTree has no reference. A dummy Actor will be used with the entry Key for name", (UnityEngine.Object) this);
    return (IDialogueActor) new ProxyDialogueActor(paramName, (Transform) null);
  }

  public void SetActorReference(string paramName, IDialogueActor actor)
  {
    DialogueTree.ActorParameter actorParameter = this.actorParameters.Find((Predicate<DialogueTree.ActorParameter>) (p => p.name == paramName));
    if (actorParameter == null)
      Debug.LogError((object) $"There is no defined Actor key name '{paramName}'");
    else
      actorParameter.actor = actor;
  }

  public void SetActorReferences(Dictionary<string, IDialogueActor> actors)
  {
    foreach (KeyValuePair<string, IDialogueActor> actor in actors)
    {
      KeyValuePair<string, IDialogueActor> pair = actor;
      DialogueTree.ActorParameter actorParameter = this.actorParameters.Find((Predicate<DialogueTree.ActorParameter>) (p => p.name == pair.Key));
      if (actorParameter == null)
        Debug.LogWarning((object) $"There is no defined Actor key name '{pair.Key}'. Seting actor skiped");
      else
        actorParameter.actor = pair.Value;
    }
  }

  public void Continue(int index = 0)
  {
    if (!this.isRunning)
      return;
    if (index < 0 || index > this.currentNode.outConnections.Count - 1)
      this.Stop();
    else
      this.EnterNode((DTNode) this.currentNode.outConnections[index].targetNode);
  }

  public void EnterNode(DTNode node)
  {
    this.currentNode = node;
    this.currentNode.Reset(false);
    if (this.currentNode.Execute(this.agent, this.blackboard) != NodeCanvas.Status.Error)
      return;
    this.Stop(false);
  }

  public static void RequestSubtitles(SubtitlesRequestInfo info)
  {
    if (DialogueTree.OnSubtitlesRequest != null)
      DialogueTree.OnSubtitlesRequest(info);
    else
      Debug.LogWarning((object) "<b>DialogueTree:</b> Subtitle Request event has no subscribers. Make sure to add the default '@DialogueGUI' prefab or create your own GUI.");
  }

  public static void RequestMultipleChoices(MultipleChoiceRequestInfo info)
  {
    if (DialogueTree.OnMultipleChoiceRequest != null)
      DialogueTree.OnMultipleChoiceRequest(info);
    else
      Debug.LogWarning((object) "<b>DialogueTree:</b> Multiple Choice Request event has no subscribers. Make sure to add the default '@DialogueGUI' prefab or create your own GUI.");
  }

  public override void OnGraphStarted()
  {
    DialogueTree.previousDialogue = DialogueTree.currentDialogue;
    DialogueTree.currentDialogue = this;
    Debug.Log((object) $"<b>DialogueTree:</b> Dialogue Started '{this.name}'");
    if (DialogueTree.OnDialogueStarted != null)
      DialogueTree.OnDialogueStarted(this);
    if (!(this.agent is IDialogueActor))
      Debug.Log((object) "<b>DialogueTree:</b> INSTIGATOR agent used in DialogueTree does not implement IDialogueActor. A dummy actor will be used.");
    this.currentNode = this.currentNode != null ? this.currentNode : (DTNode) this.primeNode;
    this.EnterNode(this.currentNode);
  }

  public override void OnGraphUnpaused()
  {
    this.currentNode = this.currentNode != null ? this.currentNode : (DTNode) this.primeNode;
    this.EnterNode(this.currentNode);
    Debug.Log((object) $"<b>DialogueTree:</b> Dialogue Resumed '{this.name}'");
    if (DialogueTree.OnDialogueStarted == null)
      return;
    DialogueTree.OnDialogueStarted(this);
  }

  public override void OnGraphStoped()
  {
    DialogueTree.currentDialogue = DialogueTree.previousDialogue;
    DialogueTree.previousDialogue = (DialogueTree) null;
    this.currentNode = (DTNode) null;
    Debug.Log((object) $"<b>DialogueTree:</b> Dialogue Finished '{this.name}'");
    if (DialogueTree.OnDialogueFinished == null)
      return;
    DialogueTree.OnDialogueFinished(this);
  }

  public override void OnGraphPaused()
  {
    Debug.Log((object) $"<b>DialogueTree:</b> Dialogue Paused '{this.name}'");
    if (DialogueTree.OnDialoguePaused == null)
      return;
    DialogueTree.OnDialoguePaused(this);
  }

  [Serializable]
  public struct DerivedSerializationData
  {
    public List<DialogueTree.ActorParameter> actorParameters;
  }

  [Serializable]
  public class ActorParameter
  {
    [SerializeField]
    public string _keyName;
    [SerializeField]
    public string _id;
    [SerializeField]
    public UnityEngine.Object _actorObject;
    [NonSerialized]
    public IDialogueActor _actor;

    public string name
    {
      get => this._keyName;
      set => this._keyName = value;
    }

    public string ID
    {
      get => !string.IsNullOrEmpty(this._id) ? this._id : (this._id = Guid.NewGuid().ToString());
    }

    public IDialogueActor actor
    {
      get
      {
        if (this._actor == null)
          this._actor = this._actorObject as IDialogueActor;
        return this._actor;
      }
      set
      {
        this._actor = value;
        this._actorObject = value as UnityEngine.Object;
      }
    }

    public ActorParameter()
    {
    }

    public ActorParameter(string name) => this.name = name;

    public ActorParameter(string name, IDialogueActor actor)
    {
      this.name = name;
      this.actor = actor;
    }

    public override string ToString() => this.name;
  }
}
