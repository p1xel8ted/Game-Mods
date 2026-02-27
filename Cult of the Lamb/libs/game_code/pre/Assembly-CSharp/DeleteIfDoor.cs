// Decompiled with JetBrains decompiler
// Type: DeleteIfDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMRoomGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class DeleteIfDoor : BaseMonoBehaviour
{
  public GenerateRoom generateRoom;
  public DeleteIfDoor.Direction MyDirection;

  private void Start()
  {
    if (!(bool) (Object) this.generateRoom)
      return;
    if (this.generateRoom.generated)
      this.Delete();
    else
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.DeleteIE());
  }

  private IEnumerator DeleteIE()
  {
    while ((Object) this.generateRoom == (Object) null || !this.generateRoom.generated)
      yield return (object) null;
    this.Delete();
  }

  private void Delete()
  {
    switch (this.MyDirection)
    {
      case DeleteIfDoor.Direction.North:
        if (this.generateRoom.North == GenerateRoom.ConnectionTypes.False)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfDoor.Direction.East:
        if (this.generateRoom.East == GenerateRoom.ConnectionTypes.False)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfDoor.Direction.South:
        if (this.generateRoom.South == GenerateRoom.ConnectionTypes.False)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfDoor.Direction.West:
        if (this.generateRoom.West == GenerateRoom.ConnectionTypes.False)
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
