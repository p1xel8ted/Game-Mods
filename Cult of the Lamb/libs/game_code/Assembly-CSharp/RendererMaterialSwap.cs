// Decompiled with JetBrains decompiler
// Type: RendererMaterialSwap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RendererMaterialSwap : BaseMonoBehaviour
{
  [SerializeField]
  public RendererMaterialSwap.RendererMaterialPair[] rendMatPairs;

  public void SwapAll()
  {
    for (int index = 0; index < this.rendMatPairs.Length; ++index)
    {
      if ((UnityEngine.Object) this.rendMatPairs[index].targetRenderer != (UnityEngine.Object) null && (UnityEngine.Object) this.rendMatPairs[index].newMaterial != (UnityEngine.Object) null)
      {
        this.rendMatPairs[index].targetRenderer.gameObject.SetActive(true);
        this.rendMatPairs[index].curMaterial = this.rendMatPairs[index].targetRenderer.sharedMaterial;
        this.rendMatPairs[index].targetRenderer.sharedMaterial = this.rendMatPairs[index].newMaterial;
        this.rendMatPairs[index].newMaterial = this.rendMatPairs[index].curMaterial;
        this.rendMatPairs[index].curMaterial = this.rendMatPairs[index].targetRenderer.sharedMaterial;
      }
    }
  }

  public void DisableAll()
  {
    for (int index = 0; index < this.rendMatPairs.Length; ++index)
    {
      if ((UnityEngine.Object) this.rendMatPairs[index].targetRenderer != (UnityEngine.Object) null && (UnityEngine.Object) this.rendMatPairs[index].newMaterial != (UnityEngine.Object) null)
        this.rendMatPairs[index].targetRenderer.gameObject.SetActive(false);
    }
  }

  [Serializable]
  public struct RendererMaterialPair
  {
    public Renderer targetRenderer;
    public Material curMaterial;
    public Material newMaterial;
  }
}
