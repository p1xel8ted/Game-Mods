// Decompiled with JetBrains decompiler
// Type: SimpleForceDoorsClosed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class SimpleForceDoorsClosed : MonoBehaviour
{
  [SerializeField]
  public GenerateRoom generateRoom;

  public void Start()
  {
    this.generateRoom.OnGenerated += new GenerateRoom.GenerateEvent(this.GenerateRoom_OnGenerated);
  }

  public void OnDestroy()
  {
    this.generateRoom.OnGenerated -= new GenerateRoom.GenerateEvent(this.GenerateRoom_OnGenerated);
  }

  public void GenerateRoom_OnGenerated() => RoomLockController.CloseAll();
}
