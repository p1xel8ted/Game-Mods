// Decompiled with JetBrains decompiler
// Type: CombatTestRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
