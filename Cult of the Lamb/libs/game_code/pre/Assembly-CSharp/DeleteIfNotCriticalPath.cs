// Decompiled with JetBrains decompiler
// Type: DeleteIfNotCriticalPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class DeleteIfNotCriticalPath : BaseMonoBehaviour
{
  public DeleteIfNotCriticalPath.Direction MyDirection;

  private void Start()
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
