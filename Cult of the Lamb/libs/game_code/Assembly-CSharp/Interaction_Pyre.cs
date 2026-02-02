// Decompiled with JetBrains decompiler
// Type: Interaction_Pyre
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Pyre : Interaction
{
  public GameObject ChoiceIndicator;
  public GameObject CameraObject;
  public bool Activated;
  public string sSacrificialPyre;
  public string sRescue;
  public string sRescueSubtitle;
  public string sLightPyre;
  public string sLightPyreSubtitle;
  public global::ChoiceIndicator c;
  public FollowerInfo _followerInfo;
  public FollowerOutfit _outfit;
  public SkeletonAnimation followerSpine;
  public GameObject Fire;

  public void Start()
  {
    this.HasSecondaryInteraction = false;
    this.UpdateLocalisation();
    this.InitFollower();
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void OnInteract(StateMachine state)
  {
    this.Activated = true;
    base.OnInteract(state);
    this.c = UnityEngine.Object.Instantiate<GameObject>(this.ChoiceIndicator, GameObject.FindWithTag("Canvas").transform).GetComponent<global::ChoiceIndicator>();
    this.c.Show(this.sRescue, this.sRescueSubtitle, this.sLightPyre, this.sLightPyreSubtitle, new System.Action(this.CutDown), new System.Action(this.LightPyre), this.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.CameraObject, 8f);
  }

  public override void Update()
  {
    base.Update();
    if (!((UnityEngine.Object) this.c != (UnityEngine.Object) null))
      return;
    this.c.UpdatePosition(this.transform.position);
  }

  public void InitFollower()
  {
    this._followerInfo = FollowerInfo.NewCharacter(PlayerFarming.Location);
    this._outfit = new FollowerOutfit(this._followerInfo);
    this._outfit.SetOutfit(this.followerSpine, false);
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sSacrificialPyre;

  public void CutDown() => this.StartCoroutine((IEnumerator) this.CutDownRoutine());

  public IEnumerator CutDownRoutine()
  {
    Interaction_Pyre interactionPyre = this;
    yield return (object) new WaitForSeconds(interactionPyre.followerSpine.AnimationState.SetAnimation(0, "spider-out", false).Animation.Duration);
    float duration = interactionPyre.followerSpine.AnimationState.SetAnimation(0, "spider-pop2", false).Animation.Duration;
    CameraManager.shakeCamera(0.5f);
    yield return (object) new WaitForSeconds(duration);
    FollowerManager.CreateNewFollower(interactionPyre._followerInfo, interactionPyre.transform.position + Vector3.back * 3f, true);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionPyre.followerSpine.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    interactionPyre.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void LightPyre() => this.StartCoroutine((IEnumerator) this.LightPyreRoutine());

  public IEnumerator LightPyreRoutine()
  {
    Interaction_Pyre interactionPyre = this;
    GameManager.GetInstance().OnConversationNext(interactionPyre.CameraObject, 5f);
    yield return (object) new WaitForSeconds(1.5f);
    interactionPyre.Fire.SetActive(true);
    CameraManager.shakeCamera(0.8f);
    GameManager.GetInstance().OnConversationNext(interactionPyre.CameraObject, 7f);
    yield return (object) new WaitForSeconds(2f);
    interactionPyre.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    if (DataManager.Instance.FirstTimeResurrecting)
    {
      UIAbilityUnlock.Play(UIAbilityUnlock.Ability.PyreResurrect);
      DataManager.Instance.FirstTimeResurrecting = false;
      yield return (object) new WaitForSeconds(0.5f);
    }
    ResurrectOnHud.ResurrectionType = ResurrectionType.Pyre;
  }
}
