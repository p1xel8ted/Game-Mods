// Decompiled with JetBrains decompiler
// Type: Bob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Bob : MonoBehaviour
{
  [SerializeField]
  public float amplitude = 1f;
  [SerializeField]
  public float frequency = 1f;
  [SerializeField]
  public float speed = 1f;
  [SerializeField]
  public Bob.Axis axis = Bob.Axis.Y;
  [Tooltip("Whether the bobbing is random or uniform (globally)")]
  [SerializeField]
  public bool randomize = true;
  public float _phaseOffset;
  public Vector3 _pivot;

  public void Start()
  {
    if (this.randomize)
      this._phaseOffset = Random.Range(0.0f, 6.28318548f);
    this._pivot = this.transform.localPosition;
  }

  public void Update()
  {
    double num1 = (double) Time.time * (double) this.speed + (double) this._phaseOffset;
    Vector3 pivot = this._pivot;
    float num2 = Mathf.Sin((float) (num1 * 3.1415927410125732) * this.frequency) * this.amplitude;
    switch (this.axis)
    {
      case Bob.Axis.X:
        pivot.x += num2;
        break;
      case Bob.Axis.Y:
        pivot.y += num2;
        break;
      case Bob.Axis.Z:
        pivot.z += num2;
        break;
    }
    this.transform.localPosition = pivot;
  }

  public enum Axis
  {
    X,
    Y,
    Z,
  }
}
