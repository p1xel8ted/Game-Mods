// Decompiled with JetBrains decompiler
// Type: Interaction_FinalBossAltar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_FinalBossAltar : Interaction
{
  public bool UseSimpleSetCamera;
  public UnityEvent KneelCallback;
  public UnityEvent RefuseCallback;
  public GameObject LookToObject;
  public SimpleSetCamera SimpleSetCamera;
  public string sInteraction;
  public GameObject ChoiceIndicator;
  public global::ChoiceIndicator c;
  public bool Activated;

  public void Start()
  {
    this.UpdateLocalisation();
    this.HoldToInteract = false;
    this.AutomaticallyInteract = true;
    this.ActivateDistance = 3f;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sInteraction = ScriptLocalization.Interactions_Intro.Kneel;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sInteraction;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    this.Activated = true;
    if (this.UseSimpleSetCamera)
      this.SimpleSetCamera.Play();
    HUD_Manager.Instance.Hide(false, 0);
    this.playerFarming.GoToAndStop(this.transform.position, this.LookToObject, GoToCallback: (System.Action) (() =>
    {
      if (DungeonSandboxManager.Active)
        this.Refuse();
      else
        this.StartCoroutine((IEnumerator) this.IOnInteract());
    }));
  }

  public IEnumerator IOnInteract()
  {
    Interaction_FinalBossAltar interactionFinalBossAltar = this;
    GameManager.GetInstance().OnConversationNew(true, false, false, interactionFinalBossAltar.playerFarming);
    GameManager.GetInstance().OnConversationNext(interactionFinalBossAltar.playerFarming.gameObject, 8f);
    interactionFinalBossAltar.playerFarming._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    interactionFinalBossAltar.playerFarming.Spine.AnimationState.SetAnimation(0, "final-boss/decide-start", false);
    interactionFinalBossAltar.playerFarming.Spine.AnimationState.AddAnimation(0, "final-boss/decide-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    interactionFinalBossAltar.GiveChoice();
  }

  public void GiveChoice()
  {
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ChoiceIndicator, GameObject.FindWithTag("Canvas").transform);
    gameObject.SetActive(true);
    this.c = gameObject.GetComponent<global::ChoiceIndicator>();
    this.c.Offset = new Vector3(0.0f, -300f, 0.0f);
    this.c.Show("Conversation_NPC/Story/Dungeon2/Leader2/2_Choice2", "UI/Generic/Accept", new System.Action(this.Refuse), new System.Action(this.Kneel), this.state.transform.position);
    this.c.ShowPrompt("Interactions/Intro/Kneel");
  }

  public void LateUpdate()
  {
    if (!((UnityEngine.Object) this.c != (UnityEngine.Object) null))
      return;
    this.c.UpdatePosition(this.state.transform.position);
  }

  public void Kneel()
  {
    UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ChoiceIndicator, GameObject.FindWithTag("Canvas").transform);
    gameObject.SetActive(true);
    this.c = gameObject.GetComponent<global::ChoiceIndicator>();
    this.c.Offset = new Vector3(0.0f, -300f, 0.0f);
    this.c.Show("Conversation_NPC/Fox/Response_No", "Conversation_NPC/Fox/Response_Yes", (System.Action) (() => this.GiveChoice()), (System.Action) (() =>
    {
      if (this.UseSimpleSetCamera)
        this.SimpleSetCamera.Reset();
      UIManager.PlayAudio("event:/ui/heretics_defeated");
      this.KneelCallback.Invoke();
    }), this.state.transform.position);
    this.c.ShowPrompt("FollowerInteractions/AreYouSure");
  }

  public void Refuse()
  {
    if (this.UseSimpleSetCamera)
      this.SimpleSetCamera.Reset();
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    this.RefuseCallback?.Invoke();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__12_0()
  {
    if (DungeonSandboxManager.Active)
      this.Refuse();
    else
      this.StartCoroutine((IEnumerator) this.IOnInteract());
  }

  [CompilerGenerated]
  public void \u003CKneel\u003Eb__16_0() => this.GiveChoice();

  [CompilerGenerated]
  public void \u003CKneel\u003Eb__16_1()
  {
    if (this.UseSimpleSetCamera)
      this.SimpleSetCamera.Reset();
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    this.KneelCallback.Invoke();
  }
}
