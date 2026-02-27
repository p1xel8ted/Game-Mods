// Decompiled with JetBrains decompiler
// Type: DeleteIfNoDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class DeleteIfNoDoor : BaseMonoBehaviour
{
  public GenerateRoom generateRoom;
  public DeleteIfNoDoor.Direction MyDirection;

  private void Start()
  {
    if ((Object) this.generateRoom == (Object) null)
      this.generateRoom = this.GetComponentInParent<GenerateRoom>();
    switch (this.MyDirection)
    {
      case DeleteIfNoDoor.Direction.North:
        if (this.generateRoom.North != GenerateRoom.ConnectionTypes.False)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfNoDoor.Direction.East:
        if (this.generateRoom.East != GenerateRoom.ConnectionTypes.False)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfNoDoor.Direction.South:
        if (this.generateRoom.South != GenerateRoom.ConnectionTypes.False)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfNoDoor.Direction.West:
        if (this.generateRoom.West != GenerateRoom.ConnectionTypes.False)
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
