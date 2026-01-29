// Decompiled with JetBrains decompiler
// Type: CustomScene_BaseDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CustomScene_BaseDemo : BaseMonoBehaviour
{
  public GameObject PlayerStartPosition;
  public GameObject Player;
  public StateMachine PlayerState;
  public float Timer;

  public void Play()
  {
    if (!DataManager.Instance.PlayerIsASpirit)
      return;
    this.Player = GameObject.FindGameObjectWithTag("Player");
    this.Player.transform.position = this.PlayerStartPosition.transform.position;
    GameManager.GetInstance().CamFollowTarget.SnapTo(this.Player.transform.position);
    GameManager.GetInstance().CameraSetZoom(4f);
    GameManager.GetInstance().OnConversationNew(false);
    this.PlayerState = this.Player.GetComponent<StateMachine>();
    this.StartCoroutine((IEnumerator) this.ConvertPlayer());
    DataManager.Instance.PlayerIsASpirit = false;
  }

  public IEnumerator ConvertPlayer()
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
