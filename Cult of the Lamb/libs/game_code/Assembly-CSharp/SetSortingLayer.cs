// Decompiled with JetBrains decompiler
// Type: SetSortingLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SetSortingLayer : BaseMonoBehaviour
{
  public Renderer MyRenderer;
  public string MySortingLayer;
  public int MySortingOrderInLayer;

  public void Start()
  {
    if ((Object) this.MyRenderer == (Object) null)
      this.MyRenderer = this.GetComponent<Renderer>();
    this.SetLayer();
  }

  public void SetLayer()
  {
    if ((Object) this.MyRenderer == (Object) null)
      this.MyRenderer = this.GetComponent<Renderer>();
    this.MyRenderer.sortingLayerName = this.MySortingLayer;
    this.MyRenderer.sortingOrder = this.MySortingOrderInLayer;
  }
}
