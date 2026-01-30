// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.RoomConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;

#nullable disable
namespace MMBiomeGeneration;

public class RoomConnection
{
  public BiomeRoom Room;
  public GenerateRoom.ConnectionTypes ConnectionType;

  public bool Connected
  {
    get
    {
      return this.ConnectionType != GenerateRoom.ConnectionTypes.False && this.ConnectionType != GenerateRoom.ConnectionTypes.Exit && this.ConnectionType != GenerateRoom.ConnectionTypes.Entrance && this.ConnectionType != GenerateRoom.ConnectionTypes.DoorRoom;
    }
  }

  public RoomConnection(BiomeRoom Room) => this.Room = Room;

  public RoomConnection(GenerateRoom.ConnectionTypes ConnectionType)
  {
    this.Room = (BiomeRoom) null;
    this.ConnectionType = ConnectionType;
  }

  public void SetConnection(GenerateRoom.ConnectionTypes ConnectionType)
  {
    this.ConnectionType = ConnectionType;
  }

  public void SetConnectionAndRoom(BiomeRoom Room, GenerateRoom.ConnectionTypes ConnectionType)
  {
    this.Room = Room;
    this.ConnectionType = ConnectionType;
  }
}
