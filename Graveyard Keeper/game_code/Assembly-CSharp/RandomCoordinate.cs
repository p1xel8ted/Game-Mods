// Decompiled with JetBrains decompiler
// Type: RandomCoordinate
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RandomCoordinate : MonoBehaviour
{
  public Vector3 random = Vector3.zero;
  public float speed = 1f;
  public float roll_period_sec = 0.05f;
  public bool animate_z;
  public Vector3 _cur_target = Vector3.zero;
  public float _time;
  public Transform _tr;

  public void Update()
  {
    float num1 = Mathf.Min(Time.deltaTime, 0.05f);
    this._time += num1;
    if ((double) this._time > (double) this.roll_period_sec)
    {
      while ((double) this._time > (double) this.roll_period_sec)
        this._time -= this.roll_period_sec;
      this._cur_target = new Vector3(Random.Range(-this.random.x, this.random.x), Random.Range(-this.random.y, this.random.y), Random.Range(-this.random.z, this.random.z));
    }
    if ((Object) this._tr == (Object) null)
      this._tr = this.transform;
    float num2 = Mathf.Min(1f, this.speed * num1);
    if (this.animate_z)
    {
      this._tr.localPosition += (this._cur_target - this._tr.localPosition) * num2;
    }
    else
    {
      Vector2 vector2 = (Vector2) (this._cur_target - this._tr.localPosition);
      if ((double) vector2.x > 10000.0)
        vector2.x = 0.0f;
      if ((double) vector2.y > 10000.0)
        vector2.y = 0.0f;
      this._tr.localPosition += (Vector3) (vector2 * num2);
    }
  }
}
