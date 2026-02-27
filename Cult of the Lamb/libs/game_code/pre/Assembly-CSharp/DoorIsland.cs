// Decompiled with JetBrains decompiler
// Type: DoorIsland
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DoorIsland : BaseMonoBehaviour
{
  public bool CheckOnEnable;
  public bool North;
  public bool East;
  public bool South;
  public bool West;

  private void OnEnable()
  {
    if (this.CheckOnEnable)
      this.Init();
    else if ((Object) RoomManager.Instance != (Object) null)
      RoomManager.Instance.OnInitEnemies += new RoomManager.InitEnemiesAction(this.Init);
    else
      this.Init();
  }

  private void OnDisable()
  {
    if (this.CheckOnEnable)
      return;
    RoomManager.Instance.OnInitEnemies -= new RoomManager.InitEnemiesAction(this.Init);
  }

  private void Init()
  {
    Debug.Log((object) $"Init N: {(object) RoomManager.r.N_Link}   E: {(object) RoomManager.r.E_Link}  S:{(object) RoomManager.r.S_Link}  W:{(object) RoomManager.r.W_Link}");
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
