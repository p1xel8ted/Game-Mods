// Decompiled with JetBrains decompiler
// Type: DoorIsland
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DoorIsland : BaseMonoBehaviour
{
  public bool CheckOnEnable;
  public bool North;
  public bool East;
  public bool South;
  public bool West;

  public void OnEnable()
  {
    if (this.CheckOnEnable)
      this.Init();
    else if ((Object) RoomManager.Instance != (Object) null)
      RoomManager.Instance.OnInitEnemies += new RoomManager.InitEnemiesAction(this.Init);
    else
      this.Init();
  }

  public void OnDisable()
  {
    if (this.CheckOnEnable)
      return;
    RoomManager.Instance.OnInitEnemies -= new RoomManager.InitEnemiesAction(this.Init);
  }

  public void Init()
  {
    Debug.Log((object) $"Init N: {((object) RoomManager.r.N_Link)?.ToString()}   E: {((object) RoomManager.r.E_Link)?.ToString()}  S:{((object) RoomManager.r.S_Link)?.ToString()}  W:{((object) RoomManager.r.W_Link)?.ToString()}");
    if (this.North && (Object) RoomManager.r.N_Link != (Object) null)
      this.gameObject.SetActive(true);
    else if (this.East && (Object) RoomManager.r.E_Link != (Object) null)
      this.gameObject.SetActive(true);
    else if (this.South && (Object) RoomManager.r.S_Link != (Object) null)
      this.gameObject.SetActive(true);
    else if (this.West && (Object) RoomManager.r.W_Link != (Object) null)
      this.gameObject.SetActive(true);
    else
      this.gameObject.SetActive(false);
  }
}
