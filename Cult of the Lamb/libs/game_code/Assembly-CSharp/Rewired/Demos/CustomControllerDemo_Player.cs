// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllerDemo_Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Player _player;
  public CharacterController cc;

  public Player player
  {
    get
    {
      if (this._player == null)
        this._player = ReInput.players.GetPlayer(this.playerId);
      return this._player;
    }
  }

  public void Awake() => this.cc = this.GetComponent<CharacterController>();

  public void Update()
  {
    if (!ReInput.isReady)
      return;
    int num = (int) this.cc.Move((Vector3) (new Vector2(this.player.GetAxis("Move Horizontal"), this.player.GetAxis("Move Vertical")) * this.speed * Time.deltaTime));
    if (this.player.GetButtonDown("Fire"))
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.bulletPrefab, this.transform.position + Vector3.Scale(new Vector3(1f, 0.0f, 0.0f), this.transform.right), Quaternion.identity);
      Vector3 vector3 = new Vector3(this.bulletSpeed * this.transform.right.x, 0.0f, 0.0f);
      gameObject.GetComponent<Rigidbody>().velocity = vector3;
    }
    if (!this.player.GetButtonDown("Change Color"))
      return;
    Renderer component = this.GetComponent<Renderer>();
    Material material = component.material;
    material.color = new Color(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), 1f);
    component.material = material;
  }
}
