// Decompiled with JetBrains decompiler
// Type: Interaction_SimpleConversation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_SimpleConversation : Interaction
{
  public bool OverrideSetCameras;
  [HideInInspector]
  public bool Spoken;
  private bool finished;
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
  public int StartingIndex;
  public List<MMTools.Response> Responses;
  public bool MovePlayerToListenPosition = true;
  public Vector3 ListenPosition = Vector3.left * 2f;
  public bool DeleteIfConditionsMet = true;
  public List<Interaction_SimpleConversation.VariableAndCondition> DeleteConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public List<Interaction_SimpleConversation.VariableAndCondition> SetConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public UnityEvent Callback;
  public bool UnlockDoorsAfterConversation;
  protected string sLabel;
  private bool ConditionMet;
  public bool AnimateBeforeConversation;
  public SkeletonAnimation Spine;
  private MeshRenderer SpinemeshRenderer;
  public bool HideBeforeTriggered = true;
  [SpineAnimation("", "Spine", true, false)]
  public string TriggeredAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string EndOnAnimation;
  public bool CallOnConversationEnd = true;
  public bool SetPlayerInactiveOnStart = true;

  public void AddEntry()
  {
    if (this.Entries.Count <= 0)
      this.Entries.Add(new ConversationEntry(this.gameObject, ""));
    else
      this.Entries.Add(ConversationEntry.Clone(this.Entries[this.Entries.Count - 1]));
  }

  private void PopulateFromManualPath()
  {
    string str = $"Conversation_NPC/Story/Dungeon{(object) this.StoryDungeon}/Leader{(object) this.StoryDungeonPosition}/";
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

  private void IncrementEntry()
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

  private void Start()
  {
    this.IgnoreTutorial = true;
    this.ConditionMet = false;
    if (this.DeleteConditions.Count > 0)
    {
      this.ConditionMet = true;
      foreach (Interaction_SimpleConversation.VariableAndCondition deleteCondition in this.DeleteConditions)
      {
        Debug.Log((object) $"{this.gameObject.name} {(object) deleteCondition.Variable} {deleteCondition.Condition.ToString()}  {DataManager.Instance.GetVariable(deleteCondition.Variable).ToString()}");
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
    this.sLabel = this.Interactable ? ScriptLocalization.Interactions.Talk : "";
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
      if (this.sLabel == "")
        this.UpdateLocalisation();
      this.Label = this.sLabel;
    }
  }

  public void Play()
  {
    if (this.ConditionMet)
      return;
    this.OnInteract(GameObject.FindWithTag("Player").GetComponent<StateMachine>());
  }

  private void Complete(TrackEntry trackEntry)
  {
    if (this.OverrideSetCameras)
      SimpleSetCamera.DisableAll();
    MMConversation.Play(new ConversationObject(this.Entries, this.Responses, new System.Action(this.DoCallBack)), this.CallOnConversationEnd);
    GameManager.GetInstance().CameraSetOffset(this.CameraOffset);
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.Complete);
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Spoken)
      return;
    base.OnInteract(state);
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
    this.Spoken = true;
    this.Label = "";
    this.finished = false;
    if (!((UnityEngine.Object) state != (UnityEngine.Object) null) || !this.MovePlayerToListenPosition)
      return;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      PlayerFarming.Instance.GoToAndStop(this.transform.position + this.ListenPosition, this.gameObject, GoToCallback: (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
      {
        if (!this.finished || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
          return;
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      })))));
    PlayerPrisonerController component = state.GetComponent<PlayerPrisonerController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    Debug.Log((object) ("Move prisoner to: " + (object) (this.transform.position + this.ListenPosition)));
    AstarPath p = AstarPath.active;
    AstarPath.active = (AstarPath) null;
    component.GoToAndStop(this.transform.position + this.ListenPosition, StateMachine.State.InActive, (System.Action) (() => AstarPath.active = p));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator ReEnableMeshRenderer()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) this.SpinemeshRenderer != (UnityEngine.Object) null)
      this.SpinemeshRenderer.enabled = true;
  }

  public virtual void DoCallBack()
  {
    if (this.OverrideSetCameras)
      SimpleSetCamera.EnableAll();
    this.finished = true;
    AudioManager.Instance.PlayOneShot("event:/ui/conversation_end");
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    foreach (Interaction_SimpleConversation.VariableAndCondition setCondition in this.SetConditions)
      DataManager.Instance.SetVariable(setCondition.Variable, setCondition.Condition);
    this.Callback?.Invoke();
    if (this.UnlockDoorsAfterConversation)
      RoomLockController.RoomCompleted();
    if (!this.IncrementStoryVariable)
      return;
    DataManager.Instance.SetVariableInt(this.StoryPosition, DataManager.Instance.GetVariableInt(this.StoryPosition) + 1);
    DataManager.Instance.SetVariableInt(this.LastRun, DataManager.Instance.dungeonRun);
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    if (!this.MovePlayerToListenPosition)
      return;
    Utils.DrawCircleXY(this.transform.position + this.ListenPosition, 0.4f, Color.blue);
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }
}
