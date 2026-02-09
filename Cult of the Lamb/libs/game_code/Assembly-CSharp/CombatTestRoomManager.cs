// Decompiled with JetBrains decompiler
// Type: CombatTestRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class CombatTestRoomManager : MonoBehaviour
{
  public PlayerFarming Player;
  public GenerateRoom roomToGenerate;

  public void Awake()
  {
    if (!(bool) (Object) this.roomToGenerate)
      return;
    this.roomToGenerate.Generate(this.roomToGenerate.Seed, GenerateRoom.ConnectionTypes.False, GenerateRoom.ConnectionTypes.False, GenerateRoom.ConnectionTypes.False, GenerateRoom.ConnectionTypes.False);
  }

  public void Start() => this.Player.gameObject.SetActive(true);
}
