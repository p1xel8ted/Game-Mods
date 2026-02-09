// Decompiled with JetBrains decompiler
// Type: DeleteIfDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class DeleteIfDoor : BaseMonoBehaviour
{
  public GenerateRoom generateRoom;
  public DeleteIfDoor.Direction MyDirection;

  public void Start()
  {
    if ((Object) this.generateRoom == (Object) null)
      this.generateRoom = this.GetComponentInParent<GenerateRoom>(true);
    if (!(bool) (Object) this.generateRoom)
      return;
    if (this.generateRoom.generated)
      this.Delete();
    else
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.DeleteIE());
  }

  public IEnumerator DeleteIE()
  {
    DeleteIfDoor deleteIfDoor = this;
    while ((Object) deleteIfDoor.generateRoom == (Object) null || !deleteIfDoor.generateRoom.generated)
      yield return (object) null;
    if ((Object) deleteIfDoor != (Object) null)
      deleteIfDoor.Delete();
  }

  public void Delete()
  {
    switch (this.MyDirection)
    {
      case DeleteIfDoor.Direction.North:
        GenerateRoom generateRoom1 = this.generateRoom;
        if ((generateRoom1 != null ? (generateRoom1.North != 0 ? 1 : 0) : 1) == 0)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfDoor.Direction.East:
        GenerateRoom generateRoom2 = this.generateRoom;
        if ((generateRoom2 != null ? (generateRoom2.East != 0 ? 1 : 0) : 1) == 0)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfDoor.Direction.South:
        GenerateRoom generateRoom3 = this.generateRoom;
        if ((generateRoom3 != null ? (generateRoom3.South != 0 ? 1 : 0) : 1) == 0)
          break;
        Object.Destroy((Object) this.gameObject);
        break;
      case DeleteIfDoor.Direction.West:
        GenerateRoom generateRoom4 = this.generateRoom;
        if ((generateRoom4 != null ? (generateRoom4.West != 0 ? 1 : 0) : 1) == 0)
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
