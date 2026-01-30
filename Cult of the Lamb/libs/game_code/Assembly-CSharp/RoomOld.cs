// Decompiled with JetBrains decompiler
// Type: RoomOld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RoomOld : BaseMonoBehaviour
{
  public int x;
  public int y;
  public Room N_Room;
  public Room E_Room;
  public Room S_Room;
  public Room W_Room;
  public Room N_Link;
  public Room E_Link;
  public Room S_Link;
  public Room W_Link;
  public bool regionSet;
  public int region;
  public bool isHome;
  public bool isEntranceHall;
  public bool pointOfInterest;
  public int[,] RoomGrid;
  public int[,] Structures;
  public float[,] PerlinNoise;
  public float[,] PerlinNoiseRock;
  public Vector2 NorthDoor;
  public Vector2 EastDoor;
  public Vector2 SouthDoor;
  public Vector2 WestDoor;
  public int Width;
  public int Height;
  public bool visited;
  public bool cleared;
  public int HORIZONTAL;
  public int VERTICAL;
  public Vector2 start;
  public int targetX;
  public int targetY;
  public int MidX;
  public int MidY;

  public void NewPointOfInterestRoom(int width, int height)
  {
    this.Width = width;
    this.Height = height;
    this.RoomGrid = new int[this.Width, this.Height];
    this.SetDoorPositions();
    this.CreatePath(this.NorthDoor, this.SouthDoor);
    this.CreatePath(this.EastDoor, this.WestDoor);
    this.SetPath(this.NorthDoor, this.WestDoor, this.VERTICAL);
    this.SetPath(this.NorthDoor, this.EastDoor, this.VERTICAL);
    this.SetPath(this.EastDoor, this.SouthDoor, this.HORIZONTAL);
    this.PerlinNoise = new float[this.Width * 4, this.Height * 4];
    for (int index1 = 0; index1 < this.Width * 4; ++index1)
    {
      for (int index2 = 0; index2 < this.Height * 4; ++index2)
      {
        if (index1 <= 1 || index1 >= this.Width * 4 - 2 || index2 <= 1 || index2 >= height * 4 - 2)
          this.PerlinNoise[index1, index2] = 1f;
      }
    }
    this.Structures = new int[this.Width * 4, this.Height * 4];
    if (Random.Range(0, 100) < 60)
      this.Structures[this.Width * 2, this.Height * 2] = 9;
    else
      this.Structures[this.Width * 2, this.Height * 2] = 8;
  }

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
    this.PerlinNoise = new float[this.Width * 4, this.Height * 4];
    float num1 = (float) Random.Range(0, 1);
    float num2 = (float) Random.Range(0, 1);
    for (int index3 = 0; index3 < this.Width * 4; ++index3)
    {
      for (int index4 = 0; index4 < this.Height * 4; ++index4)
        this.PerlinNoise[index3, index4] = Mathf.PerlinNoise((float) index3 * 0.15f + num1, (float) index4 * 0.15f + num2);
    }
    this.PerlinNoiseRock = new float[this.Width * 4, this.Height * 4];
    float num3 = (float) Random.Range(0, 1);
    float num4 = (float) Random.Range(0, 1);
    for (int index5 = 0; index5 < this.Width * 4; ++index5)
    {
      for (int index6 = 0; index6 < this.Height * 4; ++index6)
        this.PerlinNoiseRock[index5, index6] = Mathf.PerlinNoise((float) index5 * 0.25f + num3, (float) index6 * 0.25f + num4);
    }
    this.Structures = new int[this.Width * 4, this.Height * 4];
    for (int index7 = 0; index7 < this.Width; ++index7)
    {
      for (int index8 = 0; index8 < this.Height; ++index8)
      {
        int num5 = this.RoomGrid[index7, index8];
      }
    }
    for (int index9 = 0; index9 < this.Width * 4; ++index9)
    {
      for (int index10 = 0; index10 < this.Height * 4; ++index10)
      {
        if (this.RoomGrid[index9 / 4, index10 / 4] == 0 && this.Structures[index9, index10] == 0)
        {
          if ((double) this.PerlinNoise[index9, index10] <= 0.2)
          {
            this.Structures[index9, index10] = 4;
            if (index9 - 1 > 0)
              this.Structures[index9 - 1, index10] = 6;
            if (index9 + 1 < this.Width)
              this.Structures[index9 + 1, index10] = 6;
            if (index10 + 1 < this.Height)
              this.Structures[index9, index10 + 1] = 6;
            if (index10 - 1 > 0)
              this.Structures[index9, index10 - 1] = 6;
          }
          else if ((double) this.PerlinNoise[index9, index10] > 0.2 && (double) this.PerlinNoise[index9, index10] <= 0.3)
            this.Structures[index9, index10] = 6;
          else if ((double) this.PerlinNoise[index9, index10] > 0.3 && (double) this.PerlinNoise[index9, index10] <= 0.4)
            this.Structures[index9, index10] = 7;
          else if ((double) this.PerlinNoise[index9, index10] >= 0.800000011920929)
            this.Structures[index9, index10] = 10;
        }
      }
    }
  }

  public void SetDoorPositions()
  {
    this.NorthDoor = new Vector2((float) Random.Range(1, this.Width - 1), (float) this.Height);
    this.EastDoor = new Vector2((float) this.Width, (float) Random.Range(1, this.Height - 1));
    this.SouthDoor = new Vector2((float) Random.Range(1, this.Width - 1), -1f);
    this.WestDoor = new Vector2(-1f, (float) Random.Range(1, this.Height - 1));
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
