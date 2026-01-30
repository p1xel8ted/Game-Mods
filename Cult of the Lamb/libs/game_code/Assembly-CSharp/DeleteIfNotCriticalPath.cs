// Decompiled with JetBrains decompiler
// Type: DeleteIfNotCriticalPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class DeleteIfNotCriticalPath : BaseMonoBehaviour
{
  public DeleteIfNotCriticalPath.Direction MyDirection;

  public void Start()
  {
    if ((Object) BiomeGenerator.Instance == (Object) null)
      return;
    BiomeRoom.Direction criticalPathDirection = BiomeGenerator.Instance.CurrentRoom.CriticalPathDirection;
    switch (this.MyDirection)
    {
      case DeleteIfNotCriticalPath.Direction.North:
        if (criticalPathDirection == BiomeRoom.Direction.North)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfNotCriticalPath.Direction.East:
        if (criticalPathDirection == BiomeRoom.Direction.East)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfNotCriticalPath.Direction.South:
        if (criticalPathDirection == BiomeRoom.Direction.South)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfNotCriticalPath.Direction.West:
        if (criticalPathDirection == BiomeRoom.Direction.West)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
    }
  }

  public enum Direction
  {
    North,
    East,
    South,
    West,
  }
}
