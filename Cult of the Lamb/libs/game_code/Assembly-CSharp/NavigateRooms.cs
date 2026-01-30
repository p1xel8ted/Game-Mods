// Decompiled with JetBrains decompiler
// Type: NavigateRooms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NavigateRooms : BaseMonoBehaviour
{
  public GameObject North;
  public GameObject East;
  public GameObject South;
  public GameObject West;
  public GameObject ground;
  public Text text;
  public static int CurrentX = -1;
  public static int CurrentY = -1;
  public static int PrevCurrentX = -1;
  public static int PrevCurrentY = -1;
  public GameObject player;
  public static Room r;
  public bool init;
  public DungeonDecorator decorator;
  public static NavigateRooms instance;

  public static NavigateRooms GetInstance() => NavigateRooms.instance;

  public void Start() => NavigateRooms.instance = this;

  public void Update()
  {
    if (!this.init)
    {
      if (NavigateRooms.CurrentX == -1 && NavigateRooms.CurrentY == -1)
      {
        NavigateRooms.CurrentX = WorldGen.startRoom.x;
        NavigateRooms.CurrentY = WorldGen.startRoom.y;
        CameraFollowTarget.Instance.distance = 30f;
      }
      NavigateRooms.r = WorldGen.getRoom(NavigateRooms.CurrentX, NavigateRooms.CurrentY);
      NavigateRooms.r.visited = true;
      if ((Object) NavigateRooms.r.N_Link == (Object) null)
        this.North.SetActive(false);
      if ((Object) NavigateRooms.r.E_Link == (Object) null)
        this.East.SetActive(false);
      if ((Object) NavigateRooms.r.S_Link == (Object) null)
        this.South.SetActive(false);
      if ((Object) NavigateRooms.r.W_Link == (Object) null)
        this.West.SetActive(false);
      this.decorator.Decorate(NavigateRooms.r.Width * 4, NavigateRooms.r.Height * 4, NavigateRooms.r);
      this.decorator.AddBlockedAreas(NavigateRooms.r);
      this.decorator.SetDoors(NavigateRooms.r, this.North, this.East, this.South, this.West);
      this.decorator.AddOuterWalls(NavigateRooms.r);
      this.decorator.PlaceWallTiles(NavigateRooms.r);
      this.decorator.PlaceStructures(NavigateRooms.r);
      this.decorator.PlaceEnemies(NavigateRooms.r);
      this.decorator.PlaceWildLife(NavigateRooms.r);
      GameManager.RecalculatePaths();
      this.player = Object.Instantiate<GameObject>(UnityEngine.Resources.Load("Prefabs/Units/Player") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true);
      if (NavigateRooms.PrevCurrentX == -1 && NavigateRooms.PrevCurrentY == -1)
        this.player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
      else if (NavigateRooms.PrevCurrentX < NavigateRooms.CurrentX)
      {
        this.player.transform.position = this.West.transform.position + Vector3.right * 0.2f;
        this.player.GetComponent<StateMachine>().facingAngle = 0.0f;
      }
      else if (NavigateRooms.PrevCurrentX > NavigateRooms.CurrentX)
      {
        this.player.transform.position = this.East.transform.position + Vector3.left * 0.2f;
        this.player.GetComponent<StateMachine>().facingAngle = 180f;
      }
      else if (NavigateRooms.PrevCurrentY > NavigateRooms.CurrentY)
      {
        this.player.transform.position = this.North.transform.position + Vector3.down * 0.2f;
        this.player.GetComponent<StateMachine>().facingAngle = 270f;
      }
      else if (NavigateRooms.PrevCurrentY < NavigateRooms.CurrentY)
      {
        this.player.GetComponent<StateMachine>().facingAngle = 90f;
        this.player.transform.position = this.South.transform.position + Vector3.up * 0.2f;
      }
      NavigateRooms.PrevCurrentX = NavigateRooms.CurrentX;
      NavigateRooms.PrevCurrentY = NavigateRooms.CurrentY;
      CameraFollowTarget.Instance.SnapTo(this.player.transform.position);
      this.text.text = $"X: {NavigateRooms.CurrentX.ToString()}   Y: {NavigateRooms.CurrentY.ToString()}{(NavigateRooms.r.isHome ? " HOME" : "")}{(NavigateRooms.r.isEntranceHall ? " ENTRANCE HALL" : "")}{(NavigateRooms.r.pointOfInterest ? " POINT OF INTEREST" : "")}";
      this.init = true;
    }
    if (this.West.activeSelf && (Object) this.player != (Object) null && (double) this.player.transform.position.x < (double) this.West.transform.position.x)
    {
      --NavigateRooms.CurrentX;
      this.player = (GameObject) null;
    }
    if (this.East.activeSelf && (Object) this.player != (Object) null && (double) this.player.transform.position.x > (double) this.East.transform.position.x)
    {
      ++NavigateRooms.CurrentX;
      this.player = (GameObject) null;
    }
    if (this.South.activeSelf && (Object) this.player != (Object) null && (double) this.player.transform.position.y < (double) this.South.transform.position.y)
    {
      --NavigateRooms.CurrentY;
      this.player = (GameObject) null;
    }
    if (!this.North.activeSelf || !((Object) this.player != (Object) null) || (double) this.player.transform.position.y <= (double) this.North.transform.position.y)
      return;
    ++NavigateRooms.CurrentY;
    this.player = (GameObject) null;
  }

  public bool WithinRoom(Vector3 position)
  {
    return (double) position.x >= (double) this.West.transform.position.x && (double) position.x <= (double) this.East.transform.position.x && (double) position.y <= (double) this.North.transform.position.y && (double) position.y >= (double) this.South.transform.position.y;
  }
}
