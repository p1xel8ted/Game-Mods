// Decompiled with JetBrains decompiler
// Type: DustCloudOverDistanceEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DustCloudOverDistanceEmitter : MonoBehaviour
{
  [SerializeField]
  public float distanceBetweenParticles = 1f;
  [SerializeField]
  public float dustSize = 1f;
  [SerializeField]
  public int dustAmount = 3;
  public Vector3 lastEmissionPosition;
  public float distanceSinceLastEmission;

  public void Start()
  {
    this.lastEmissionPosition = this.transform.position;
    this.distanceSinceLastEmission = 0.0f;
  }

  public void Update()
  {
    Vector3 position = this.transform.position;
    for (this.distanceSinceLastEmission += Vector2.Distance(new Vector2(position.x, position.z), new Vector2(this.lastEmissionPosition.x, this.lastEmissionPosition.z)); (double) this.distanceSinceLastEmission >= (double) this.distanceBetweenParticles; this.distanceSinceLastEmission -= this.distanceBetweenParticles)
      BiomeConstants.Instance.EmitDustCloudParticles(position, this.dustAmount, this.dustSize);
    this.lastEmissionPosition = position;
  }
}
