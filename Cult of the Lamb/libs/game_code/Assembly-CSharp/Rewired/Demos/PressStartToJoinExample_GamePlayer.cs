// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressStartToJoinExample_GamePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (CharacterController))]
public class PressStartToJoinExample_GamePlayer : MonoBehaviour
{
  public int gamePlayerId;
  public float moveSpeed = 3f;
  public float bulletSpeed = 15f;
  public GameObject bulletPrefab;
  public CharacterController cc;
  public Vector3 moveVector;
  public bool fire;

  public Player player => PressStartToJoinExample_Assigner.GetRewiredPlayer(this.gamePlayerId);

  public void OnEnable() => this.cc = this.GetComponent<CharacterController>();

  public void Update()
  {
    if (!ReInput.isReady || this.player == null)
      return;
    this.GetInput();
    this.ProcessInput();
  }

  public void GetInput()
  {
    this.moveVector.x = this.player.GetAxis("Move Horizontal");
    this.moveVector.y = this.player.GetAxis("Move Vertical");
    this.fire = this.player.GetButtonDown("Fire");
  }

  public void ProcessInput()
  {
    if ((double) this.moveVector.x != 0.0 || (double) this.moveVector.y != 0.0)
    {
      int num = (int) this.cc.Move(this.moveVector * this.moveSpeed * Time.deltaTime);
    }
    if (!this.fire)
      return;
    Object.Instantiate<GameObject>(this.bulletPrefab, this.transform.position + this.transform.right, this.transform.rotation).GetComponent<Rigidbody>().AddForce(this.transform.right * this.bulletSpeed, ForceMode.VelocityChange);
  }
}
