// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressAnyButtonToJoinExample_GamePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (CharacterController))]
public class PressAnyButtonToJoinExample_GamePlayer : MonoBehaviour
{
  public int playerId;
  public float moveSpeed = 3f;
  public float bulletSpeed = 15f;
  public GameObject bulletPrefab;
  private CharacterController cc;
  private Vector3 moveVector;
  private bool fire;

  private Player player
  {
    get => !ReInput.isReady ? (Player) null : ReInput.players.GetPlayer(this.playerId);
  }

  private void OnEnable() => this.cc = this.GetComponent<CharacterController>();

  private void Update()
  {
    if (!ReInput.isReady || this.player == null)
      return;
    this.GetInput();
    this.ProcessInput();
  }

  private void GetInput()
  {
    this.moveVector.x = this.player.GetAxis("Move Horizontal");
    this.moveVector.y = this.player.GetAxis("Move Vertical");
    this.fire = this.player.GetButtonDown("Fire");
  }

  private void ProcessInput()
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
