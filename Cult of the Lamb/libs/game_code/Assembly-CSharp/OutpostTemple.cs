// Decompiled with JetBrains decompiler
// Type: OutpostTemple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class OutpostTemple : Interaction
{
  [TermsPopup("")]
  public string Place;
  public GameObject UnclaimedObject;
  public GameObject ClaimedObject;
  public BlockingDoor blockingDoor;
  public string sClaim;

  public void Start()
  {
    this.UpdateLocalisation();
    this.SetGameObjects();
  }

  public void SetGameObjects()
  {
    if (DataManager.Instance.GetVariable(this.Place.Remove(0, 6) + "_OutpostTemple"))
    {
      this.UnclaimedObject.SetActive(false);
      this.ClaimedObject.SetActive(true);
      if (!((UnityEngine.Object) this.blockingDoor != (UnityEngine.Object) null))
        return;
      this.blockingDoor.Open();
    }
    else
    {
      this.UnclaimedObject.SetActive(true);
      this.ClaimedObject.SetActive(false);
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sClaim = "Claim shrine";
  }

  public override void GetLabel()
  {
    this.Label = DataManager.Instance.GetVariable(this.Place.Remove(0, 6) + "_OutpostTemple") ? "" : this.sClaim;
  }

  public override void OnInteract(StateMachine state)
  {
    if (DataManager.Instance.GetVariable(this.Place.Remove(0, 6) + "_OutpostTemple"))
      return;
    base.OnInteract(state);
    this.Fade();
  }

  public void Fade()
  {
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 3f, "", new System.Action(this.RevealMap));
  }

  public void RevealMap()
  {
  }

  public void Build()
  {
    this.UnclaimedObject.SetActive(false);
    this.ClaimedObject.SetActive(true);
    MMTransition.ResumePlay();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (!((UnityEngine.Object) this.blockingDoor != (UnityEngine.Object) null))
      return;
    this.blockingDoor.Open();
  }
}
