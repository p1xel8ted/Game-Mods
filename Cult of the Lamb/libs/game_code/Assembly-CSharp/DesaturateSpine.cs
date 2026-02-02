// Decompiled with JetBrains decompiler
// Type: DesaturateSpine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DesaturateSpine : BaseMonoBehaviour
{
  [SerializeField]
  public float desaturationDuration = 0.5f;
  [SerializeField]
  public bool desaturateOnStart = true;
  public MeshRenderer meshRenderer;
  public int desaturationPropertyId;
  public MaterialPropertyBlock materialBlock;

  public void Start()
  {
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    this.desaturationPropertyId = Shader.PropertyToID("_Desaturation");
    this.materialBlock = new MaterialPropertyBlock();
    this.meshRenderer.GetPropertyBlock(this.materialBlock);
    if (!this.desaturateOnStart)
      return;
    this.Desaturate();
  }

  public void Desaturate() => this.StartCoroutine((IEnumerator) this.DesaturateRoutine());

  public IEnumerator DesaturateRoutine()
  {
    float startTime = Time.time;
    while ((double) Time.time - (double) startTime < (double) this.desaturationDuration)
    {
      this.materialBlock.SetFloat(this.desaturationPropertyId, Mathf.Lerp(0.0f, 1f, (Time.time - startTime) / this.desaturationDuration));
      this.meshRenderer.SetPropertyBlock(this.materialBlock);
      yield return (object) null;
    }
  }
}
