// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.EightPlayersExample_Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (CharacterController))]
public class EightPlayersExample_Player : MonoBehaviour
{
  public int playerId;
  public float moveSpeed = 3f;
  public float bulletSpeed = 15f;
  public GameObject bulletPrefab;
  public Player player;
  public CharacterController cc;
  public Vector3 moveVector;
  public bool fire;
  [NonSerialized]
  public bool initialized;

  public void Awake() => this.cc = this.GetComponent<CharacterController>();

  public void Initialize()
  {
    this.player = ReInput.players.GetPlayer(this.playerId);
    this.initialized = true;
  }

  public void Update()
  {
    if (!ReInput.isReady)
      return;
    if (!this.initialized)
      this.Initialize();
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
    UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, this.transform.position + this.transform.right, this.transform.rotation).GetComponent<Rigidbody>().AddForce(this.transform.right * this.bulletSpeed, ForceMode.VelocityChange);
  }
}
