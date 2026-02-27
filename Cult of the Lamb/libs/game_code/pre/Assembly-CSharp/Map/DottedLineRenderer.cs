// Decompiled with JetBrains decompiler
// Type: Map.DottedLineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Map;

public class DottedLineRenderer : MonoBehaviour
{
  public bool scaleInUpdate;
  private LineRenderer lR;
  private Renderer rend;

  private void Start()
  {
    this.ScaleMaterial();
    this.enabled = this.scaleInUpdate;
  }

  public void ScaleMaterial()
  {
    this.lR = this.GetComponent<LineRenderer>();
    this.rend = this.GetComponent<Renderer>();
    this.rend.material.mainTextureScale = new Vector2(Vector2.Distance((Vector2) this.lR.GetPosition(0), (Vector2) this.lR.GetPosition(this.lR.positionCount - 1)) / this.lR.widthMultiplier, 1f);
  }

  private void Update()
  {
    this.rend.material.mainTextureScale = new Vector2(Vector2.Distance((Vector2) this.lR.GetPosition(0), (Vector2) this.lR.GetPosition(this.lR.positionCount - 1)) / this.lR.widthMultiplier, 1f);
  }
}
