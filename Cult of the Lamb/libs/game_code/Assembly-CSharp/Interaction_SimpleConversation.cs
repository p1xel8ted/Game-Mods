// Decompiled with JetBrains decompiler
// Type: Interaction_SimpleConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_SimpleConversation : Interaction
{
  public bool OverrideSetCameras;
  [CompilerGenerated]
  public bool \u003CSpoken\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CFinished\u003Ek__BackingField;
  public Vector3 CameraOffset = Vector3.zero;
  public List<ConversationEntry> Entries;
  public bool IncrementStoryVariable;
  public DataManager.Variables StoryPosition;
  public DataManager.Variables LastRun;
  public bool PopulateStory;
  public int StoryDungeon;
  public int StoryDungeonPosition;
  [TermsPopup("")]
  public string StoryCharacterName = "-";
  public GameObject StorySpeakPosition;
  public SkeletonAnimation StorySpine;
  public bool gotoDisableCollider;
  public float gotoTimeout = 20f;
  public int StartingIndex;
  public List<MMTools.Response> Responses;
  public bool MovePlayerToListenPosition = true;
  public Vector3 ListenPosition = Vector3.left * 2f;
  public bool DeleteIfConditionsMet = true;
  public List<Interaction_SimpleConversation.VariableAndCondition> DeleteConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public List<Interaction_SimpleConversation.VariableAndCondition> SetConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public UnityEvent Callback;
  public bool UnlockDoorsAfterConversation;
  public string customCameraWooshSFX;
  public string sLabel;
  public bool ConditionMet;
  public bool AnimateBeforeConversation;
  public SkeletonAnimation Spine;
  public MeshRenderer SpinemeshRenderer;
  public bool HideBeforeTriggered = true;
  [SpineAnimation("", "Spine", true, false)]
  public string TriggeredAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string EndOnAnimation;
  public UnityEvent CallbackBeforeConversation;
  public bool CallOnConversationEnd = true;
  public bool SetPlayerInactiveOnStart = true;

  [HideInInspector]
  public bool Spoken
  {
    get => this.\u003CSpoken\u003Ek__BackingField;
    set => this.\u003CSpoken\u003Ek__BackingField = value;
  }

  public bool Finished
  {
    get => this.\u003CFinished\u003Ek__BackingField;
    set => this.\u003CFinished\u003Ek__BackingField = value;
  }

  public void AddEntry()
  {
    if (this.Entries.Count <= 0)
      this.Entries.Add(new ConversationEntry(this.gameObject, ""));
    else
      this.Entries.Add(ConversationEntry.Clone(this.Entries[this.Entries.Count - 1]));
  }

  public void PopulateFromManualPath()
  {
    string str = $"Conversation_NPC/Story/Dungeon{this.StoryDungeon.ToString()}/Leader{this.StoryDungeonPosition.ToString()}/";
    this.Entries.Clear();
    int num = -1;
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
      this.Entries.Add(new ConversationEntry(this.gameObject, str + num.ToString())
      {
        CharacterName = this.StoryCharacterName,
        Speaker = this.StorySpeakPosition,
        SkeletonData = this.StorySpine
      });
  }

  public void IncrementEntry()
  {
    if (this.Entries.Count <= 0)
      return;
    int startingIndex = this.StartingIndex;
    while (LocalizationManager.GetTermData(ConversationEntry.Clone(this.Entries[this.StartingIndex]).TermToSpeak.Replace(this.StartingIndex.ToString(), (++startingIndex).ToString())) != null)
    {
      ConversationEntry conversationEntry = ConversationEntry.Clone(this.Entries[this.StartingIndex]);
      conversationEntry.TermToSpeak = conversationEntry.TermToSpeak.Replace(this.StartingIndex.ToString(), startingIndex.ToString());
      this.Entries.Add(conversationEntry);
    }
  }

  public void Start()
  {
    this.IgnoreTutorial = true;
    this.ConditionMet = false;
    if (this.DeleteConditions.Count > 0)
    {
      this.ConditionMet = true;
      foreach (Interaction_SimpleConversation.VariableAndCondition deleteCondition in this.DeleteConditions)
      {
        Debug.Log((object) $"{this.gameObject.name} {deleteCondition.Variable.ToString()} {deleteCondition.Condition.ToString()}  {DataManager.Instance.GetVariable(deleteCondition.Variable).ToString()}");
        if (DataManager.Instance.GetVariable(deleteCondition.Variable) != deleteCondition.Condition)
        {
          this.ConditionMet = false;
          break;
        }
      }
      if (this.ConditionMet)
      {
        Debug.Log((object) "ConditionMet");
        if (this.DeleteIfConditionsMet)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
          return;
        }
        this.enabled = false;
        return;
      }
    }
    this.UpdateLocalisation();
    if (!this.AnimateBeforeConversation || !this.HideBeforeTriggered || !((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.SpinemeshRenderer = this.Spine.gameObject.GetComponent<MeshRenderer>();
    this.SpinemeshRenderer.enabled = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    if (this.label == null)
      this.sLabel = this.Interactable ? ScriptLocalization.Interactions.Talk : "";
    else
      this.sLabel = LocalizationManager.GetTranslation(this.label);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.Spoken)
    {
      this.Label = "";
    }
    else
    {
      if (string.IsNullOrEmpty(this.sLabel))
        this.UpdateLocalisation();
      this.Label = this.sLabel;
    }
  }

  public void Play() => this.Play((GameObject) null);

  public IEnumerator PlayIE() => this.PlayIE((GameObject) null);

  public IEnumerator PlayIE(GameObject player = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_SimpleConversation simpleConversation = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    simpleConversation.Play(player);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(simpleConversation.\u003CPlayIE\u003Eb__43_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Play(GameObject player = null)
  {
    if (this.ConditionMet)
      return;
    GameObject gameObject = PlayerFarming.FindClosestPlayerGameObject(this.transform.position);
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      gameObject = GameObject.FindWithTag("Player");
    this.OnInteract((UnityEngine.Object) player == (UnityEngine.Object) null ? gameObject.GetComponent<StateMachine>() : player.GetComponent<StateMachine>());
  }

  public void Complete(TrackEntry trackEntry)
  {
    if (this.OverrideSetCameras)
      SimpleSetCamera.DisableAll();
    MMConversation.Play(new ConversationObject(this.Entries, this.Responses, new System.Action(this.DoCallBack)), this.CallOnConversationEnd);
    GameManager.GetInstance().CameraSetOffset(this.CameraOffset);
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.Complete);
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.Spoken)
      return;
    this.CallbackBeforeConversation?.Invoke();
    base.OnInteract(state);
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null ? PlayerFarming.Instance : this.playerFarming;
    if (this.AnimateBeforeConversation)
    {
      if (this.OverrideSetCameras)
        SimpleSetCamera.DisableAll();
      LetterBox.Show(false);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().AddToCamera(this.Spine.gameObject);
      this.Spine.AnimationState.SetAnimation(0, this.TriggeredAnimation, false);
      this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.Complete);
      this.Spine.AnimationState.AddAnimation(0, this.EndOnAnimation, false, 0.0f);
      this.StartCoroutine((IEnumerator) this.ReEnableMeshRenderer());
    }
    else
    {
      if (this.OverrideSetCameras)
        SimpleSetCamera.DisableAll();
      MMConversation.Play(new ConversationObject(this.Entries, this.Responses, new System.Action(this.DoCallBack)), this.CallOnConversationEnd, this.SetPlayerInactiveOnStart);
      GameManager.GetInstance().CameraSetOffset(this.CameraOffset);
    }
    AudioManager.Instance.PlayOneShot("event:/ui/conversation_start");
    if (!string.IsNullOrEmpty(this.customCameraWooshSFX))
      AudioManager.Instance.PlayOneShot(this.customCameraWooshSFX);
    this.Spoken = true;
    this.Label = "";
    this.Finished = false;
    if (!((UnityEngine.Object) state != (UnityEngine.Object) null) || !this.MovePlayerToListenPosition)
      return;
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
      this.playerFarming.GoToAndStop(this.transform.position + this.ListenPosition, this.gameObject, DisableCollider: this.gotoDisableCollider, GoToCallback: (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
      {
        if (!this.Finished || this.playerFarming.state.CURRENT_STATE != StateMachine.State.InActive)
          return;
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
            player.AbortGoTo();
        }
        PlayerFarming.SetStateForAllPlayers();
      })))), maxDuration: this.gotoTimeout, forcePositionOnTimeout: true, groupAction: true);
    PlayerPrisonerController component = state.GetComponent<PlayerPrisonerController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    Debug.Log((object) ("Move prisoner to: " + (this.transform.position + this.ListenPosition).ToString()));
    AstarPath p = AstarPath.active;
    AstarPath.active = (AstarPath) null;
    component.GoToAndStop(this.transform.position + this.ListenPosition, StateMachine.State.InActive, (System.Action) (() => AstarPath.active = p));
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator ReEnableMeshRenderer()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) this.SpinemeshRenderer != (UnityEngine.Object) null)
      this.SpinemeshRenderer.enabled = true;
  }

  public virtual void DoCallBack()
  {
    if (this.OverrideSetCameras)
      SimpleSetCamera.EnableAll();
    this.Finished = true;
    AudioManager.Instance.PlayOneShot("event:/ui/conversation_end");
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    foreach (Interaction_SimpleConversation.VariableAndCondition setCondition in this.SetConditions)
      DataManager.Instance.SetVariable(setCondition.Variable, setCondition.Condition);
    this.Callback?.Invoke();
    if (this.UnlockDoorsAfterConversation)
      RoomLockController.RoomCompleted();
    if (this.IncrementStoryVariable)
    {
      DataManager.Instance.SetVariableInt(this.StoryPosition, DataManager.Instance.GetVariableInt(this.StoryPosition) + 1);
      DataManager.Instance.SetVariableInt(this.LastRun, DataManager.Instance.dungeonRun);
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming && player.GoToAndStopping)
        player.AbortGoTo();
    }
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    if (!this.MovePlayerToListenPosition)
      return;
    Utils.DrawCircleXY(this.transform.position + this.ListenPosition, 0.4f, Color.blue);
  }

  public void ResetConvo()
  {
    this.Spoken = false;
    this.Finished = false;
    this.HasChanged = true;
    if (!((UnityEngine.Object) this._playerFarming != (UnityEngine.Object) null) || !((UnityEngine.Object) this._playerFarming.interactor.CurrentInteraction == (UnityEngine.Object) this))
      return;
    this._playerFarming.interactor.CurrentInteraction = (Interaction) null;
  }

  [CompilerGenerated]
  public bool \u003CPlayIE\u003Eb__43_0() => this.Finished;

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__55_0()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      if (!this.Finished || this.playerFarming.state.CURRENT_STATE != StateMachine.State.InActive)
        return;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
          player.AbortGoTo();
      }
      PlayerFarming.SetStateForAllPlayers();
    })));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__55_1()
  {
    if (!this.Finished || this.playerFarming.state.CURRENT_STATE != StateMachine.State.InActive)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
        player.AbortGoTo();
    }
    PlayerFarming.SetStateForAllPlayers();
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }
}
