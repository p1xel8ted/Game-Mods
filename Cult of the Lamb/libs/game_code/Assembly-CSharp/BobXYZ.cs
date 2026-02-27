// Decompiled with JetBrains decompiler
// Type: BobXYZ
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BobXYZ : MonoBehaviour
{
  [SerializeField]
  public float amplitude = 0.1f;
  [SerializeField]
  public float frequency = 1f;
  [SerializeField]
  public float speed = 1f;
  [SerializeField]
  public bool randomize = true;
  [SerializeField]
  public float amplitudeX = 0.4f;
  [SerializeField]
  public float frequencyX = 1.9f;
  [SerializeField]
  public float speedX = 0.6f;
  [SerializeField]
  public float amplitudeY = 0.6f;
  [SerializeField]
  public float frequencyY = 2.1f;
  [SerializeField]
  public float speedY = 0.4f;
  [SerializeField]
  public float multiplier = 0.5f;
  public bool followExternalMotion;
  public float _phaseOffsetX;
  public float _phaseOffsetY;
  public float _phaseOffsetZ;
  public Vector3 _startPivotLocal;
  public Vector3 _lastBob;

  public void Start()
  {
    this._startPivotLocal = this.transform.localPosition;
    if (!this.randomize)
      return;
    this._phaseOffsetX = Random.Range(0.0f, 6.28318548f);
    this._phaseOffsetY = Random.Range(0.0f, 6.28318548f);
    this._phaseOffsetZ = Random.Range(0.0f, 6.28318548f);
  }

  public void LateUpdate()
  {
    Vector3 vector3_1 = this.followExternalMotion ? this.transform.localPosition - this._lastBob : this._startPivotLocal;
    Vector3 vector3_2 = vector3_1;
    if (!Mathf.Approximately(this.amplitudeX, 0.0f))
    {
      float f = (float) (((double) Time.time + (double) this._phaseOffsetX) * (double) this.speedX * 3.1415927410125732) * this.frequencyX;
      vector3_2.x += Mathf.Sin(f) * this.amplitudeX * this.multiplier;
    }
    if (!Mathf.Approximately(this.amplitudeY, 0.0f))
    {
      float f = (float) (((double) Time.time + (double) this._phaseOffsetY) * (double) this.speedY * 3.1415927410125732) * this.frequencyY;
      vector3_2.y += Mathf.Sin(f) * this.amplitudeY * this.multiplier;
    }
    if (!Mathf.Approximately(this.amplitude, 0.0f))
    {
      float f = (float) (((double) Time.time + (double) this._phaseOffsetZ) * (double) this.speed * 3.1415927410125732) * this.frequency;
      vector3_2.z += Mathf.Sin(f) * this.amplitude * this.multiplier;
    }
    this.transform.localPosition = vector3_2;
    this._lastBob = vector3_2 - vector3_1;
  }
}
