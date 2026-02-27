// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllerDemo_Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (CharacterController))]
public class CustomControllerDemo_Player : MonoBehaviour
{
  public int playerId;
  public float speed = 1f;
  public float bulletSpeed = 20f;
  public GameObject bulletPrefab;
  private Player _player;
  private CharacterController cc;

  private Player player
  {
    get
    {
      if (this._player == null)
        this._player = ReInput.players.GetPlayer(this.playerId);
      return this._player;
    }
  }

  private void Awake() => this.cc = this.GetComponent<CharacterController>();

  private void Update()
  {
    if (!ReInput.isReady)
      return;
    int num = (int) this.cc.Move((Vector3) (new Vector2(this.player.GetAxis("Move Horizontal"), this.player.GetAxis("Move Vertical")) * this.speed * Time.deltaTime));
    if (this.player.GetButtonDown("Fire"))
      Object.Instantiate<GameObject>(this.bulletPrefab, this.transform.position + Vector3.Scale(new Vector3(1f, 0.0f, 0.0f), this.transform.right), Quaternion.identity).GetComponent<Rigidbody>().velocity = new Vector3(this.bulletSpeed * this.transform.right.x, 0.0f, 0.0f);
    if (!this.player.GetButtonDown("Change Color"))
      return;
    Renderer component = this.GetComponent<Renderer>();
    Material material = component.material;
    material.color = new Color(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), 1f);
    component.material = material;
  }
}
