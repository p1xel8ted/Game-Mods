// Decompiled with JetBrains decompiler
// Type: SimpleSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class SimpleSceneManager : BaseMonoBehaviour
{
  public GameObject player;
  public CameraFollowTarget Camera;
  public GameObject PlayerPrefab;
  public Transform StartPlayerPosition;
  public bool HideHUD;

  public void Start()
  {
    this.StartCoroutine((IEnumerator) this.PlaceAndPositionPlayer());
    if (!this.HideHUD)
      return;
    HUD_Manager.Instance.Hide(true, 0);
  }

  public IEnumerator PlaceAndPositionPlayer()
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
