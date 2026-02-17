// Decompiled with JetBrains decompiler
// Type: DeleteIfNoDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class DeleteIfNoDoor : BaseMonoBehaviour
{
  public GenerateRoom generateRoom;
  public DeleteIfNoDoor.Direction MyDirection;

  public void Start()
  {
    if ((Object) this.generateRoom == (Object) null)
      this.generateRoom = this.GetComponentInParent<GenerateRoom>();
    this.generateRoom.OnGenerated += new GenerateRoom.GenerateEvent(this.DeleteOnRoomGenerated);
  }

  public void DeleteOnRoomGenerated()
  {
    switch (this.MyDirection)
    {
      case DeleteIfNoDoor.Direction.North:
        if (this.generateRoom.North == GenerateRoom.ConnectionTypes.False)
        {
          Object.Destroy((Object) this.gameObject);
          break;
        }
        break;
      case DeleteIfNoDoor.Direction.East:
        if (this.generateRoom.East == GenerateRoom.ConnectionTypes.False)
        {
          Object.Destroy((Object) this.gameObject);
          break;
        }
        break;
      case DeleteIfNoDoor.Direction.South:
        if (this.generateRoom.South == GenerateRoom.ConnectionTypes.False)
        {
          Object.Destroy((Object) this.gameObject);
          break;
        }
        break;
      case DeleteIfNoDoor.Direction.West:
        if (this.generateRoom.West == GenerateRoom.ConnectionTypes.False)
        {
          Object.Destroy((Object) this.gameObject);
          break;
        }
        break;
    }
    this.generateRoom.OnGenerated -= new GenerateRoom.GenerateEvent(this.DeleteOnRoomGenerated);
  }

  public enum Direction
  {
    North,
    East,
    South,
    West,
  }
}
