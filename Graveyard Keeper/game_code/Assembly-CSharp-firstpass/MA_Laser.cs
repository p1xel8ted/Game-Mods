// Decompiled with JetBrains decompiler
// Type: MA_Laser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class MA_Laser : MonoBehaviour
{
  public Transform _trans;

  public void Awake()
  {
    this.useGUILayout = false;
    this._trans = this.transform;
  }

  public void OnCollisionEnter(Collision collision)
  {
    if (!collision.gameObject.name.StartsWith("Enemy("))
      return;
    Object.Destroy((Object) collision.gameObject);
    Object.Destroy((Object) this.gameObject);
  }

  public void Update()
  {
    float num = 10f * AudioUtil.FrameTime;
    Vector3 position = this._trans.position;
    position.y += num;
    this._trans.position = position;
    if ((double) this._trans.position.y <= 7.0)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
