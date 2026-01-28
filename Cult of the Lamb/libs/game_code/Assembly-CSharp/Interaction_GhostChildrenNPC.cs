// Decompiled with JetBrains decompiler
// Type: Interaction_GhostChildrenNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_GhostChildrenNPC : Interaction
{
  [SerializeField]
  public Objectives_FindChildren.ChildLocation childLocation;
  [SerializeField]
  public GameObject childrenParent;
  public string interactionText = "UI/Search";
  public GhostChildrenNPC ghostNPC;
  public bool found;
  public EventInstance laughingLoop;
  public bool moveToConversationPointFirst;

  public void Awake() => this.ghostNPC = this.childrenParent.GetComponent<GhostChildrenNPC>();

  public void Start() => this.childrenParent.SetActive(false);

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.laughingLoop);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.childrenParent == (UnityEngine.Object) null || this.found)
      return;
    this.Interactable = this.IsObjectiveActive();
    if (!this.Interactable)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (InputManager.Gameplay.GetBleatButtonDown(player))
      {
        AudioManager.Instance.StopLoop(this.laughingLoop);
        this.laughingLoop = AudioManager.Instance.CreateLoop("event:/dialogue/followers/ghost_children_hide_hint", this.gameObject, true);
        GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => AudioManager.Instance.StopLoop(this.laughingLoop)));
      }
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.IsObjectiveActive())
      this.Label = this.interactionText.Localized();
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.FindChildrenRoutine());
  }

  public IEnumerator FindChildrenRoutine()
  {
    Interaction_GhostChildrenNPC ghostChildrenNpc = this;
    if (!ghostChildrenNpc.found)
    {
      ghostChildrenNpc.found = true;
      ghostChildrenNpc.Interactable = false;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(ghostChildrenNpc.gameObject, 8f);
      if (ghostChildrenNpc.moveToConversationPointFirst)
        ghostChildrenNpc.ghostNPC.GotoConversationPoint();
      while (ghostChildrenNpc.playerFarming.GoToAndStopping)
        yield return (object) null;
      GameManager.GetInstance().CameraZoom(6f, 3f);
      EventInstance introInstance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/intro/intro_bass");
      yield return (object) new WaitForSeconds(3f);
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", ghostChildrenNpc.gameObject);
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", ghostChildrenNpc.gameObject);
      GameManager.GetInstance().CameraZoom(8f, 0.3f);
      AudioManager.Instance.StopOneShotInstanceEarly(introInstance, STOP_MODE.IMMEDIATE);
      ghostChildrenNpc.childrenParent.SetActive(true);
      yield return (object) ghostChildrenNpc.ghostNPC.PlayFoundConversation();
      ghostChildrenNpc.childrenParent.SetActive(false);
    }
  }

  public bool IsObjectiveActive()
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_FindChildren objectivesFindChildren && this.childLocation == objectivesFindChildren.Location)
        return !objectivesFindChildren.IsComplete && (objectivesFindChildren.Location != Objectives_FindChildren.ChildLocation.SporeGrotto || TimeManager.CurrentPhase != DayPhase.Night) && (objectivesFindChildren.Location != Objectives_FindChildren.ChildLocation.SmugglersSanctuary || TimeManager.CurrentPhase == DayPhase.Night) && (objectivesFindChildren.Location != Objectives_FindChildren.ChildLocation.MidasCave || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter);
    }
    return false;
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__9_0() => AudioManager.Instance.StopLoop(this.laughingLoop);
}
