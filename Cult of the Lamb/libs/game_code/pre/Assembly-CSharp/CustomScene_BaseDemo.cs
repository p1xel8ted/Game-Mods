// Decompiled with JetBrains decompiler
// Type: CustomScene_BaseDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CustomScene_BaseDemo : BaseMonoBehaviour
{
  public GameObject PlayerStartPosition;
  private GameObject Player;
  private StateMachine PlayerState;
  private float Timer;

  public void Play()
  {
    if (!DataManager.Instance.PlayerIsASpirit)
      return;
    this.Player = GameObject.FindGameObjectWithTag("Player");
    this.Player.transform.position = this.PlayerStartPosition.transform.position;
    GameManager.GetInstance().CamFollowTarget.SnapTo(this.Player.transform.position);
    GameManager.GetInstance().CameraSetZoom(4f);
    GameManager.GetInstance().OnConversationNew(false, true);
    this.PlayerState = this.Player.GetComponent<StateMachine>();
    this.StartCoroutine((IEnumerator) this.ConvertPlayer());
    DataManager.Instance.PlayerIsASpirit = false;
  }

  private IEnumerator ConvertPlayer()
  {
    yield return (object) new WaitForEndOfFrame();
    this.PlayerState.CURRENT_STATE = StateMachine.State.Unconverted;
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationNext(this.Player, 4f);
    yield return (object) new WaitForSeconds(0.5f);
    this.PlayerState.CURRENT_STATE = StateMachine.State.Converting;
    yield return (object) new WaitForSeconds(4f);
    GameManager.GetInstance().OnConversationNext(this.Player, 6f);
    CameraManager.shakeCamera(0.5f, (float) Random.Range(0, 360));
    while (this.PlayerState.CURRENT_STATE == StateMachine.State.Converting)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    GameManager.GetInstance().AddPlayerToCamera();
  }
}
