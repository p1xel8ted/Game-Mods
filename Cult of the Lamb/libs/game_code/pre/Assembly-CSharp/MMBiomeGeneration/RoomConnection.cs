// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.RoomConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
