// Decompiled with JetBrains decompiler
// Type: PathfindingCarve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PathfindingCarve : MonoBehaviour
{
  private void Start()
  {
    foreach (Collider2D componentsInChild in this.GetComponentsInChildren<Collider2D>())
    {
      if ((Object) AstarPath.active != (Object) null)
        AstarPath.active.UpdateGraphs(componentsInChild.bounds);
    }
  }
}
