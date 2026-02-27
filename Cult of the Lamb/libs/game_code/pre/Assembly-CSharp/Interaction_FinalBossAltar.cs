// Decompiled with JetBrains decompiler
// Type: Interaction_FinalBossAltar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_FinalBossAltar : Interaction
{
  private bool UseSimpleSetCamera;
  public UnityEvent KneelCallback;
  public UnityEvent RefuseCallback;
  public GameObject LookToObject;
  public SimpleSetCamera SimpleSetCamera;
  private string sInteraction;
  public GameObject ChoiceIndicator;
  private global::ChoiceIndicator c;
  private bool Activated;

  private void Start()
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
    PlayerFarming.Instance.GoToAndStop(this.transform.position, this.LookToObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.IOnInteract())));
  }

  private IEnumerator IOnInteract()
  {
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "final-boss/decide-start", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "final-boss/decide-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    this.GiveChoice();
  }

  private void GiveChoice()
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

  private void LateUpdate()
  {
    if (!((UnityEngine.Object) this.c != (UnityEngine.Object) null))
      return;
    this.c.UpdatePosition(this.state.transform.position);
  }

  private void Kneel()
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

  private void Refuse()
  {
    if (this.UseSimpleSetCamera)
      this.SimpleSetCamera.Reset();
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    this.RefuseCallback?.Invoke();
  }
}
