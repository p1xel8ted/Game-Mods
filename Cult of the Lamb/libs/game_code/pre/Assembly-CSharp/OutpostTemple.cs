// Decompiled with JetBrains decompiler
// Type: OutpostTemple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private string sClaim;

  private void Start()
  {
    this.UpdateLocalisation();
    this.SetGameObjects();
  }

  private void SetGameObjects()
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

  private void Fade()
  {
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 3f, "", new System.Action(this.RevealMap));
  }

  private void RevealMap()
  {
  }

  private void Build()
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
