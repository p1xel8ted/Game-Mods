// Decompiled with JetBrains decompiler
// Type: PathfindingCarve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PathfindingCarve : MonoBehaviour
{
  public Collider2D[] cols;

  public void Start() => this.UpdateCarving();

  public void UpdateCarving()
  {
    if (this.cols == null)
      this.cols = this.GetComponentsInChildren<Collider2D>();
    foreach (Collider2D col in this.cols)
    {
      if ((Object) AstarPath.active != (Object) null)
        AstarPath.active.UpdateGraphs(col.bounds);
    }
  }

  public void OnDestroy() => this.UpdateCarving();
}
