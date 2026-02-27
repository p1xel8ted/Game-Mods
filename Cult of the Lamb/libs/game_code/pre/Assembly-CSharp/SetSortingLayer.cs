// Decompiled with JetBrains decompiler
// Type: SetSortingLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SetSortingLayer : BaseMonoBehaviour
{
  public Renderer MyRenderer;
  public string MySortingLayer;
  public int MySortingOrderInLayer;

  private void Start()
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
