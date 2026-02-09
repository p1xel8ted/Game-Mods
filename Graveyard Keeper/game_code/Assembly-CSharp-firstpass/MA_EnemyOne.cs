// Decompiled with JetBrains decompiler
// Type: MA_EnemyOne
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class MA_EnemyOne : MonoBehaviour
{
  public GameObject ExplosionParticlePrefab;
  public Transform _trans;
  public float _speed;
  public float _horizSpeed;

  public void Awake()
  {
    this.useGUILayout = false;
    this._trans = this.transform;
    this._speed = (float) Random.Range(-3, -8) * AudioUtil.FrameTime;
    this._horizSpeed = (float) Random.Range(-3, 3) * AudioUtil.FrameTime;
  }

  public void OnCollisionEnter(Collision collision)
  {
    Object.Instantiate<GameObject>(this.ExplosionParticlePrefab, this._trans.position, Quaternion.identity);
  }

  public void Update()
  {
    Vector3 position = this._trans.position;
    position.x += this._horizSpeed;
    position.y += this._speed;
    this._trans.position = position;
    this._trans.Rotate(Vector3.down * 300f * AudioUtil.FrameTime);
    if ((double) this._trans.position.y >= -5.0)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
