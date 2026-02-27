// Decompiled with JetBrains decompiler
// Type: RendererMaterialSwap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RendererMaterialSwap : BaseMonoBehaviour
{
  [SerializeField]
  private RendererMaterialSwap.RendererMaterialPair[] rendMatPairs;

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
  private struct RendererMaterialPair
  {
    public Renderer targetRenderer;
    public Material curMaterial;
    public Material newMaterial;
  }
}
