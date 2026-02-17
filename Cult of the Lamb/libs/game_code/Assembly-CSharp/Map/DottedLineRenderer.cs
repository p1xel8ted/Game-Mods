// Decompiled with JetBrains decompiler
// Type: Map.DottedLineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Map;

public class DottedLineRenderer : MonoBehaviour
{
  public bool scaleInUpdate;
  public LineRenderer lR;
  public Renderer rend;

  public void Start()
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

  public void Update()
  {
    this.rend.material.mainTextureScale = new Vector2(Vector2.Distance((Vector2) this.lR.GetPosition(0), (Vector2) this.lR.GetPosition(this.lR.positionCount - 1)) / this.lR.widthMultiplier, 1f);
  }
}
