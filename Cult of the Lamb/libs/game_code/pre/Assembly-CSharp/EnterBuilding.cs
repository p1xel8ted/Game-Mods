// Decompiled with JetBrains decompiler
// Type: EnterBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class EnterBuilding : BaseMonoBehaviour
{
  public UnityEvent Trigger;
  private GameObject Player;
  public FollowerLocation Destination;
  public bool HideCanvasConstants;
  public bool ShowCanvasConstants;
  private bool placedPlayer;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (MMConversation.isPlaying)
      MMConversation.mmConversation.FinishCloseFadeTweenByForce();
    if (!(collision.gameObject.tag == "Player") || MMConversation.isPlaying || PlayerFarming.Instance.GoToAndStopping || LetterBox.IsPlaying)
      return;
    this.placedPlayer = false;
    PlayerFarming.Instance.DropDeadFollower();
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", new System.Action(this.DoTrigger));
    this.Player = collision.gameObject;
    if (this.HideCanvasConstants)
      CanvasConstants.instance.Hide();
    if (this.ShowCanvasConstants)
      CanvasConstants.instance.Show();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.CheckToEnsurePositioned());
  }

  private IEnumerator CheckToEnsurePositioned()
  {
    EnterBuilding enterBuilding = this;
    yield return (object) new WaitForSeconds(1f);
    if (!enterBuilding.placedPlayer)
    {
      MMTransition.StopCurrentTransition();
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", new System.Action(enterBuilding.DoTrigger));
    }
  }

  private void DoTrigger()
  {
    this.Trigger?.Invoke();
    if (LocationManager.LocationManagers.ContainsKey(this.Destination))
      LocationManager.LocationManagers[this.Destination].PositionPlayer();
    GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.transform.position);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.placedPlayer = true;
  }
}
