// Decompiled with JetBrains decompiler
// Type: VelocityTextureOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (TrailRenderer))]
public class VelocityTextureOffset : BaseMonoBehaviour
{
  public Vector3 lastPos;
  public Vector3 transformSpeed = Vector3.zero;
  public float OffsetScale = 1f;
  public float moveSpeed;
  public TrailRenderer _renderer;
  public static int velocityOffsetID = Shader.PropertyToID("_VelocityOffset");
  public bool randomizeStartOffset = true;
  public MaterialPropertyBlock m_propBlock;

  public MaterialPropertyBlock PropBlock
  {
    get
    {
      if (this.m_propBlock == null)
        this.m_propBlock = new MaterialPropertyBlock();
      return this.m_propBlock;
    }
  }

  public void Start()
  {
    if ((Object) this._renderer == (Object) null)
      this._renderer = this.GetComponent<TrailRenderer>();
    this.lastPos = this.transform.position;
    if (!this.randomizeStartOffset)
      return;
    Vector4 vector = this._renderer.material.GetVector(VelocityTextureOffset.velocityOffsetID);
    this._renderer.material.SetVector(VelocityTextureOffset.velocityOffsetID, new Vector4(vector.x, vector.y, Random.Range(0.0f, 1f), Random.Range(0.0f, 1f)));
  }

  public void Update()
  {
    this.transformSpeed = this.transform.position - this.lastPos;
    this.transformSpeed /= Time.deltaTime;
    this.moveSpeed = this.transformSpeed.magnitude;
    float num = (float) (100.0 / (1.0 / (double) Time.deltaTime));
    Vector4 vector = this._renderer.material.GetVector(VelocityTextureOffset.velocityOffsetID);
    this._renderer.material.SetVector(VelocityTextureOffset.velocityOffsetID, new Vector4(vector.x, vector.y, vector.z - (float) ((double) num * (double) this.OffsetScale * 0.0099999997764825821) * this.moveSpeed * vector.x, vector.w));
    this.lastPos = this.transform.position;
  }
}
