// Decompiled with JetBrains decompiler
// Type: RoomTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RoomTest : BaseMonoBehaviour
{
  public GameObject Tile;
  public GameObject EmptyTile;
  public GameObject DoorTile;
  public int[,] RoomGrid;
  public Vector2 NorthDoor;
  public Vector2 EastDoor;
  public Vector2 SouthDoor;
  public Vector2 WestDoor;
  public bool NorthEast;
  public bool NorthSouth;
  public bool NorthWest;
  public bool EastSouth;
  public bool EastWest;
  public bool SouthWest;
  public int Width;
  public int Height;
  public int HORIZONTAL;
  public int VERTICAL;
  public Vector2 start;
  public int targetX;
  public int targetY;
  public int MidX;
  public int MidY;

  public void Start() => this.NewRoom(Random.Range(5, 20), Random.Range(5, 20));

  public void NewRoom(int width, int height)
  {
    this.Width = width;
    this.Height = height;
    this.RoomGrid = new int[this.Width, this.Height];
    this.SetDoorPositions();
    for (int index1 = 0; index1 < this.Width; ++index1)
    {
      for (int index2 = 0; index2 < this.Height; ++index2)
      {
        if ((double) Random.Range(0.0f, 10f) < 3.5)
          this.RoomGrid[index1, index2] = 1;
      }
    }
    this.CreatePath(this.NorthDoor, this.SouthDoor);
    this.CreatePath(this.EastDoor, this.WestDoor);
    this.SetPath(this.NorthDoor, this.WestDoor, this.VERTICAL);
    this.SetPath(this.NorthDoor, this.EastDoor, this.VERTICAL);
    this.SetPath(this.EastDoor, this.SouthDoor, this.HORIZONTAL);
    for (int x = 0; x < this.Width; ++x)
    {
      for (int y = 0; y < this.Height; ++y)
      {
        switch (this.RoomGrid[x, y])
        {
          case 0:
            Object.Instantiate<GameObject>(this.EmptyTile, this.transform.parent, true).transform.position = new Vector3((float) x, (float) y, 0.0f);
            break;
          case 1:
            Object.Instantiate<GameObject>(this.Tile, this.transform.parent, true).transform.position = new Vector3((float) x, (float) y, 0.0f);
            break;
        }
      }
    }
  }

  public void SetDoorPositions()
  {
    this.NorthDoor = new Vector2((float) Random.Range(1, this.Width - 1), (float) this.Height);
    Object.Instantiate<GameObject>(this.DoorTile, new Vector3(this.NorthDoor.x, this.NorthDoor.y, 0.0f), Quaternion.identity);
    this.EastDoor = new Vector2((float) this.Width, (float) Random.Range(1, this.Height - 1));
    Object.Instantiate<GameObject>(this.DoorTile, new Vector3(this.EastDoor.x, this.EastDoor.y, 0.0f), Quaternion.identity);
    this.SouthDoor = new Vector2((float) Random.Range(1, this.Width - 1), -1f);
    Object.Instantiate<GameObject>(this.DoorTile, new Vector3(this.SouthDoor.x, this.SouthDoor.y, 0.0f), Quaternion.identity);
    this.WestDoor = new Vector2(-1f, (float) Random.Range(1, this.Height - 1));
    Object.Instantiate<GameObject>(this.DoorTile, new Vector3(this.WestDoor.x, this.WestDoor.y, 0.0f), Quaternion.identity);
  }

  public void SetPath(Vector2 _start, Vector2 _end, int InitialDir)
  {
    _start.x = Mathf.Min(_start.x, (float) (this.Width - 1));
    _start.x = Mathf.Max(_start.x, 0.0f);
    _start.y = Mathf.Min(_start.y, (float) (this.Height - 1));
    _start.y = Mathf.Max(_start.y, 0.0f);
    _end.x = Mathf.Min(_end.x, (float) (this.Width - 1));
    _end.x = Mathf.Max(_end.x, 0.0f);
    _end.y = Mathf.Min(_end.y, (float) (this.Height - 1));
    _end.y = Mathf.Max(_end.y, 0.0f);
    this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
    if (InitialDir == this.VERTICAL)
    {
      while ((double) _start.y < (double) _end.y)
      {
        ++_start.y;
        this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
      }
      while ((double) _start.y > (double) _end.y)
      {
        --_start.y;
        this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
      }
      while ((double) _start.x < (double) _end.x)
      {
        ++_start.x;
        this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
      }
      while ((double) _start.x > (double) _end.x)
      {
        --_start.x;
        this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
      }
    }
    if (InitialDir != this.HORIZONTAL)
      return;
    while ((double) _start.x < (double) _end.x)
    {
      ++_start.x;
      this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
    }
    while ((double) _start.x > (double) _end.x)
    {
      --_start.x;
      this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
    }
    while ((double) _start.y < (double) _end.y)
    {
      ++_start.y;
      this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
    }
    while ((double) _start.y > (double) _end.y)
    {
      --_start.y;
      this.RoomGrid[(int) _start.x, (int) _start.y] = 0;
    }
  }

  public void CreatePath(Vector2 _start, Vector2 end)
  {
    this.start = _start;
    this.targetX = (int) end.x;
    if (this.targetX < 0)
      this.targetX = 0;
    if (this.targetX > this.Width)
      this.targetX = this.Width;
    this.targetY = (int) end.y;
    if (this.targetY < 0)
      this.targetY = 0;
    if (this.targetY > this.Height)
      this.targetY = this.Height;
    this.start.x = Mathf.Min(this.start.x, (float) (this.Width - 1));
    this.start.x = Mathf.Max(this.start.x, 0.0f);
    this.start.y = Mathf.Min(this.start.y, (float) (this.Height - 1));
    this.start.y = Mathf.Max(this.start.y, 0.0f);
    this.MidX = (int) Mathf.Abs((float) this.targetX - this.start.x);
    this.MidY = (int) Mathf.Abs((float) this.targetY - this.start.y);
    if ((double) Mathf.Abs(this.start.x - (float) this.targetX) > (double) Mathf.Abs(this.start.y - (float) this.targetY))
    {
      while ((double) this.start.x < (double) this.targetX)
      {
        ++this.start.x;
        this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
        if ((int) this.start.x == this.MidX)
          this.MoveVertically();
      }
      while ((double) this.start.x > (double) this.targetX)
      {
        --this.start.x;
        this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
        if ((int) this.start.x == this.MidX)
          this.MoveVertically();
      }
    }
    else
    {
      while ((double) this.start.y < (double) this.targetY)
      {
        ++this.start.y;
        this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
        if ((int) this.start.y == this.MidY)
          this.MoveHorizontally();
      }
      while ((double) this.start.y > (double) this.targetY)
      {
        --this.start.y;
        this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
        if ((int) this.start.y == this.MidY)
          this.MoveHorizontally();
      }
    }
  }

  public void MoveVertically()
  {
    while ((double) this.start.y < (double) this.targetY)
    {
      ++this.start.y;
      this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
    }
    while ((double) this.start.y > (double) this.targetY)
    {
      --this.start.y;
      this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
    }
  }

  public void MoveHorizontally()
  {
    while ((double) this.start.x > (double) this.targetX)
    {
      --this.start.x;
      this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
    }
    while ((double) this.start.x < (double) this.targetX)
    {
      ++this.start.x;
      this.RoomGrid[(int) this.start.x, (int) this.start.y] = 0;
    }
  }
}
