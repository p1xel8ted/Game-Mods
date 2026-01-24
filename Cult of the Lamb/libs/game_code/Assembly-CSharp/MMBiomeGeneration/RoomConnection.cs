// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.RoomConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
