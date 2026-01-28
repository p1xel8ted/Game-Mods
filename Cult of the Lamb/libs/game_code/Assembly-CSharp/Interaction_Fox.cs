// Decompiled with JetBrains decompiler
// Type: Interaction_Fox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Fox : Interaction
{
  public Interaction_SimpleConversation firstMeeting;
  public Interaction_SimpleConversation secondMeeting;
  public Interaction_SimpleConversation YesOption;
  public Interaction_SimpleConversation NoOption;
  public DOTweenAnimation Animation;
  public Transform inWater;
  public Transform outWater;
  public GameObject Fox;
  public bool Activated;
  public bool showingFox;
  public GameObject ChoiceIndicator;
  public GameObject CameraObject;
  public GameObject Conversations;
  public GameObject KeyPiece;
  public global::ChoiceIndicator c;
  public GameObject Moon;
  public string sYes;
  public string sYesSubtitle;
  public string sNo;
  public string sNoSubtitle;

  public void GiveOverFollowerChoice()
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ChoiceIndicator, GameObject.FindWithTag("Canvas").transform);
    gameObject.SetActive(true);
    this.c = gameObject.GetComponent<global::ChoiceIndicator>();
    this.c.Offset = new Vector3(0.0f, 200f, 0.0f);
    this.c.Show(this.sYes, this.sYesSubtitle, this.sNo, this.sNoSubtitle, new System.Action(this.Yes), new System.Action(this.No), this.Fox.transform.position);
  }

  public void Yes() => this.StartCoroutine((IEnumerator) this.YesRoutine());

  public IEnumerator YesRoutine()
  {
    DataManager.Instance.GaveFollowerToFox = true;
    BiomeConstants.Instance.ShakeCamera();
    RumbleManager.Instance.Rumble();
    yield return (object) new WaitForSeconds(1f);
    UnityEngine.Random.Range(0, DataManager.Instance.Followers.Count);
    bool flag = false;
    FollowerBrain allBrain1 = FollowerBrain.AllBrains[UnityEngine.Random.Range(0, FollowerBrain.AllBrains.Count)];
    foreach (FollowerBrain allBrain2 in FollowerBrain.AllBrains)
    {
      if (allBrain2.Info.ID == allBrain1.Info.ID)
      {
        Debug.Log((object) ("Kill Follower: " + allBrain2.Info.Name));
        allBrain2.Die(NotificationCentre.NotificationType.Died);
        flag = true;
        break;
      }
    }
    if (!flag)
      Debug.Log((object) "Didnt find a follower :(");
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    this.YesOption.gameObject.SetActive(true);
  }

  public void TakeFollower()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask.Type == FollowerTaskType.FollowPlayer)
      {
        allBrain.Die(NotificationCentre.NotificationType.Died);
        break;
      }
    }
  }

  public void No()
  {
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
    this.NoOption.gameObject.SetActive(true);
  }

  public void GiveKey() => this.StartCoroutine((IEnumerator) this.GiveKeyRoutine());

  public IEnumerator GiveKeyRoutine()
  {
    Interaction_Fox interactionFox = this;
    interactionFox.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionFox.KeyPiece.GetComponent<PickUp>().enabled = false;
    interactionFox.KeyPiece.GetComponent<Interaction_KeyPiece>().enabled = false;
    interactionFox.KeyPiece.GetComponent<Interaction_KeyPiece>().AutomaticallyInteract = true;
    interactionFox.KeyPiece.GetComponent<Interaction_KeyPiece>().ActivateDistance = 5f;
    interactionFox.KeyPiece.SetActive(true);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFox.KeyPiece, 6f);
    interactionFox.KeyPiece.transform.DOPunchScale(Vector3.one, 1f);
    BiomeConstants.Instance.ShakeCamera();
    RumbleManager.Instance.Rumble();
    yield return (object) new WaitForSeconds(1f);
    interactionFox.KeyPiece.transform.DOMove(interactionFox.playerFarming.gameObject.transform.position, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCirc);
    yield return (object) new WaitForSeconds(4.5f);
    interactionFox.KeyPiece.GetComponent<Interaction_KeyPiece>().enabled = true;
    yield return (object) new WaitForSeconds(1f);
    interactionFox.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.SetStateForAllPlayers();
  }

  public void Start()
  {
    this.KeyPiece.SetActive(false);
    this.UpdateLocalisation();
    this.CheckTimeOfDay();
  }

  public override void Update()
  {
    base.Update();
    if (DataManager.Instance.GaveFollowerToFox)
      return;
    this.CheckTimeOfDay();
    if (!((UnityEngine.Object) this.c != (UnityEngine.Object) null))
      return;
    this.c.UpdatePosition(this.transform.position);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if (DataManager.Instance.GaveFollowerToFox)
      this.gameObject.SetActive(false);
    this.secondMeeting.gameObject.SetActive(false);
  }

  public void CheckIfFollowerFromConvo()
  {
    this.secondMeeting.gameObject.SetActive(false);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask.Type == FollowerTaskType.FollowPlayer)
      {
        this.secondMeeting.gameObject.SetActive(true);
        break;
      }
    }
  }

  public void CheckTimeOfDay()
  {
    if (!DataManager.Instance.GaveFollowerToFox)
    {
      if (TimeManager.CurrentPhase == DayPhase.Night && DataManager.Instance.Followers.Count > 1)
      {
        if (this.showingFox)
          return;
        this.Moon.SetActive(true);
        this.Fox.SetActive(true);
        this.Conversations.SetActive(true);
        this.Animation.target = (Component) this.inWater;
        this.showingFox = true;
      }
      else if (this.showingFox)
      {
        this.Moon.SetActive(false);
      }
      else
      {
        if (!((UnityEngine.Object) this.Fox != (UnityEngine.Object) null))
          return;
        this.Fox.SetActive(false);
        this.Conversations.SetActive(false);
      }
    }
    else
    {
      this.gameObject.SetActive(false);
      this.Fox.SetActive(false);
      this.Moon.SetActive(false);
      this.Conversations.SetActive(false);
    }
  }

  public IEnumerator CheckTime()
  {
    while (TimeManager.CurrentPhase != DayPhase.Night)
    {
      this.CheckTimeOfDay();
      yield return (object) new WaitForSeconds(1f);
    }
  }

  public IEnumerator TurnOffWolf()
  {
    yield return (object) new WaitForSeconds(1f);
    this.showingFox = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sYes = "Interactions/SacrificeFollower";
    this.sYesSubtitle = "Interactions/SacrificeFollowerSubtitle";
    this.sNo = "Interactions/SacrificeMaybeNot";
    this.sNoSubtitle = "Interactions/SacrificeMaybeNotSubtitle";
  }

  public override void GetLabel()
  {
  }

  public override void OnInteract(StateMachine state) => base.OnInteract(state);
}
