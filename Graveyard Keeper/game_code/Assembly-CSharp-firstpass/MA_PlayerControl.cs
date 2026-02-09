// Decompiled with JetBrains decompiler
// Type: MA_PlayerControl
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class MA_PlayerControl : MonoBehaviour
{
  public GameObject ProjectilePrefab;
  public bool canShoot = true;
  public const float MoveSpeed = 10f;
  public Transform _trans;
  public float _lastMoveAmt;

  public void Awake()
  {
    this.useGUILayout = false;
    this._trans = this.transform;
  }

  public void OnCollisionEnter(Collision collision)
  {
    if (!collision.gameObject.name.StartsWith("Enemy("))
      return;
    this.gameObject.SetActive(false);
  }

  public void OnDisable()
  {
  }

  public void OnBecameInvisible()
  {
  }

  public void OnBecameVisible()
  {
  }

  public void Update()
  {
    float num = Input.GetAxis("Horizontal") * 10f * AudioUtil.FrameTime;
    if ((double) num != 0.0)
    {
      if ((double) this._lastMoveAmt == 0.0)
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent("PlayerMoved", this._trans);
    }
    else if ((double) this._lastMoveAmt != 0.0)
      DarkTonic.MasterAudio.MasterAudio.FireCustomEvent("PlayerStoppedMoving", this._trans);
    this._lastMoveAmt = num;
    Vector3 position1 = this._trans.position;
    position1.x += num;
    this._trans.position = position1;
    if (!this.canShoot || !Input.GetMouseButtonDown(0))
      return;
    Vector3 position2 = this._trans.position;
    ++position2.y;
    Object.Instantiate<GameObject>(this.ProjectilePrefab, position2, this.ProjectilePrefab.transform.rotation);
  }
}
