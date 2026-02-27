// Decompiled with JetBrains decompiler
// Type: SimpleSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class SimpleSceneManager : BaseMonoBehaviour
{
  private GameObject player;
  public CameraFollowTarget Camera;
  public GameObject PlayerPrefab;
  public Transform StartPlayerPosition;
  public bool HideHUD;

  private void Start()
  {
    this.StartCoroutine((IEnumerator) this.PlaceAndPositionPlayer());
    if (!this.HideHUD)
      return;
    HUD_Manager.Instance.Hide(true, 0);
  }

  private IEnumerator PlaceAndPositionPlayer()
  {
    if ((Object) this.player == (Object) null)
      this.player = Object.Instantiate<GameObject>(this.PlayerPrefab, GameObject.FindGameObjectWithTag("Unit Layer").transform, true);
    if ((Object) this.StartPlayerPosition != (Object) null)
    {
      this.player.transform.position = this.StartPlayerPosition.position;
      this.player.GetComponent<StateMachine>().facingAngle = 90f;
    }
    yield return (object) new WaitForEndOfFrame();
    this.Camera.SnapTo(GameObject.FindWithTag("Player Camera Bone").transform.position);
    yield return (object) new WaitForSeconds(2f);
    HUD_DisplayName.Play("The Realm Beyond");
    MMTransition.ResumePlay();
  }
}
