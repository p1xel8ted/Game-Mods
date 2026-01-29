// Decompiled with JetBrains decompiler
// Type: DungeonDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Tilemaps;

#nullable disable
public class DungeonDecorator : BaseMonoBehaviour
{
  public RuleTile Tile0;
  public RuleTile Tile1;
  public RuleTile Tile2;
  public RuleTile Tile3;
  public RuleTile Tile4;
  public Tilemap TileMap;
  public float xl;
  public float _x;
  public float yl;
  public float _y;
  public float sample;
  public int width = 50;
  public int height = 50;
  public Vector3 Size;
  public GridLayout gridLayout;
  public static DungeonDecorator instance;
  public SpriteRenderer ground;

  public void OnEnable() => DungeonDecorator.instance = this;

  public void OnDisable() => DungeonDecorator.instance = (DungeonDecorator) null;

  public static DungeonDecorator getInsance() => DungeonDecorator.instance;

  public void Decorate(int w, int h, Room r)
  {
    this.width = w;
    this.height = h;
    for (int index1 = 0; index1 < this.width; ++index1)
    {
      for (int index2 = 0; index2 < this.height; ++index2)
      {
        this.sample = r.PerlinNoise[index1, index2];
        if ((double) this.sample <= 0.4)
          this.TileMap.SetTile(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0), (TileBase) this.Tile1);
      }
    }
    this.gridLayout = Object.FindObjectOfType<GridLayout>();
    this.Size = this.gridLayout.CellToWorld(new Vector3Int(this.width, this.height, 0));
    if (!((Object) this.ground != (Object) null))
      return;
    this.ground.size = (Vector2) this.Size;
  }

  public void AddCorner()
  {
  }

  public void AddGrass(Room r)
  {
    for (int index1 = 0; index1 < this.width; ++index1)
    {
      for (int index2 = 0; index2 < this.height; ++index2)
      {
        if ((Object) this.TileMap.GetTile(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) == (Object) this.Tile1)
          Object.Instantiate<GameObject>(Resources.Load("Prefabs/Particles/Long Grass") as GameObject, this.transform.parent, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0));
      }
    }
  }

  public void AddBlockedAreas(Room r)
  {
    for (int index1 = 0; index1 < r.Width; ++index1)
    {
      for (int index2 = 0; index2 < r.Height; ++index2)
      {
        if (r.RoomGrid[index1, index2] == 1)
        {
          for (int index3 = 0; index3 < 4; ++index3)
          {
            for (int index4 = 0; index4 < 4; ++index4)
              this.TileMap.SetTile(new Vector3Int(index1 * 4 - this.width / 2 + index3, index2 * 4 - this.height / 2 + index4, 0), (TileBase) this.Tile4);
          }
        }
      }
    }
  }

  public void AddOuterWalls(Room r)
  {
    for (int index1 = -2; index1 < this.width + 2; ++index1)
    {
      for (int index2 = -2; index2 < this.height + 2; ++index2)
      {
        if ((index1 == -1 || index1 == -2 || index1 == this.width || index1 == this.width + 1 || index2 == -1 || index2 == this.height || index2 == -2 || index2 == this.height + 1) && (Object) this.TileMap.GetTile(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) != (Object) this.Tile1)
          this.TileMap.SetTile(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0), (TileBase) this.Tile3);
      }
    }
  }

  public void SetDoors(Room r, GameObject N, GameObject E, GameObject S, GameObject W)
  {
    N.transform.position = this.gridLayout.CellToWorld(new Vector3Int((int) ((double) r.NorthDoor.x * 4.0) - this.width / 2 + 2, (int) ((double) r.NorthDoor.y * 4.0) - this.height / 2, 0));
    E.transform.position = this.gridLayout.CellToWorld(new Vector3Int((int) ((double) r.EastDoor.x * 4.0) - this.width / 2, (int) ((double) r.EastDoor.y * 4.0) - this.height / 2 + 2, 0));
    S.transform.position = this.gridLayout.CellToWorld(new Vector3Int((int) ((double) r.SouthDoor.x * 4.0) - this.width / 2 + 2, -1 - this.height / 2, 0));
    W.transform.position = this.gridLayout.CellToWorld(new Vector3Int(-1 - this.width / 2, (int) ((double) r.WestDoor.y * 4.0) - this.height / 2 + 2, 0));
    if ((Object) r.N_Link != (Object) null)
    {
      for (int index = 0; index < 4; ++index)
      {
        this.TileMap.SetTile(new Vector3Int((int) ((double) r.NorthDoor.x * 4.0) - this.width / 2 + index, (int) ((double) r.NorthDoor.y * 4.0) - this.height / 2, 0), (TileBase) this.Tile1);
        this.TileMap.SetTile(new Vector3Int((int) ((double) r.NorthDoor.x * 4.0) - this.width / 2 + index, (int) ((double) r.NorthDoor.y * 4.0) - this.height / 2 + 1, 0), (TileBase) this.Tile1);
      }
    }
    if ((Object) r.E_Link != (Object) null)
    {
      for (int index = 0; index < 4; ++index)
      {
        this.TileMap.SetTile(new Vector3Int((int) r.EastDoor.x * 4 - this.width / 2, (int) r.EastDoor.y * 4 - this.height / 2 + index, 0), (TileBase) this.Tile1);
        this.TileMap.SetTile(new Vector3Int((int) r.EastDoor.x * 4 - this.width / 2 + 1, (int) r.EastDoor.y * 4 - this.height / 2 + index, 0), (TileBase) this.Tile1);
      }
    }
    if ((Object) r.S_Link != (Object) null)
    {
      for (int index = 0; index < 4; ++index)
      {
        this.TileMap.SetTile(new Vector3Int((int) ((double) r.SouthDoor.x * 4.0) - this.width / 2 + index, -1 - this.height / 2, 0), (TileBase) this.Tile1);
        this.TileMap.SetTile(new Vector3Int((int) ((double) r.SouthDoor.x * 4.0) - this.width / 2 + index, -1 - this.height / 2 - 1, 0), (TileBase) this.Tile1);
      }
    }
    if (!((Object) r.W_Link != (Object) null))
      return;
    for (int index = 0; index < 4; ++index)
    {
      this.TileMap.SetTile(new Vector3Int(-1 - this.width / 2, (int) r.WestDoor.y * 4 - this.height / 2 + index, 0), (TileBase) this.Tile1);
      this.TileMap.SetTile(new Vector3Int(-1 - this.width / 2 - 1, (int) r.WestDoor.y * 4 - this.height / 2 + index, 0), (TileBase) this.Tile1);
    }
  }

  public void PlaceWallTiles(Room r)
  {
  }

  public void PlaceStructures(Room r)
  {
    for (int index1 = 0; index1 < this.width; ++index1)
    {
      for (int index2 = 0; index2 < this.height; ++index2)
      {
        switch (r.StructuresOld[index1, index2])
        {
          case 1:
            GameObject gameObject1 = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Tile-Breakable") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true);
            gameObject1.transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            float num1 = (float) (0.40000000596046448 + (double) r.PerlinNoiseRock[index1, index2] * 0.60000002384185791);
            foreach (SpriteRenderer componentsInChild in gameObject1.GetComponentsInChildren<SpriteRenderer>())
              componentsInChild.color = new Color(num1, num1, num1, 1f);
            break;
          case 2:
            GameObject gameObject2 = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Tile-Breakable2") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true);
            gameObject2.transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            float num2 = (float) (0.40000000596046448 + (double) r.PerlinNoiseRock[index1, index2] * 0.60000002384185791);
            foreach (SpriteRenderer componentsInChild in gameObject2.GetComponentsInChildren<SpriteRenderer>())
              componentsInChild.color = new Color(num2, num2, num2, 1f);
            break;
          case 3:
            GameObject gameObject3 = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Tile-Breakable3") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true);
            gameObject3.transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            float num3 = (float) (0.40000000596046448 + (double) r.PerlinNoiseRock[index1, index2] * 0.60000002384185791);
            foreach (SpriteRenderer componentsInChild in gameObject3.GetComponentsInChildren<SpriteRenderer>())
              componentsInChild.color = new Color(num3, num3, num3, 1f);
            break;
          case 4:
            Object.Instantiate<GameObject>(Resources.Load("Prefabs/Environment/Base/Tree/Tree_Base") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            break;
          case 5:
            GameObject gameObject4 = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Environment/Base/Tree/Tree_Base") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true);
            gameObject4.transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            gameObject4.GetComponent<Tree>().StartAsStump();
            break;
          case 6:
            GameObject gameObject5 = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Crops/Bush") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true);
            gameObject5.transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            float num4 = Mathf.Min(1f, (float) (1.0 - ((double) r.PerlinNoise[index1, index2] - 0.20000000298023224) / 0.15000000596046448 * 0.34999999403953552));
            gameObject5.transform.localScale = new Vector3(num4, num4, num4);
            break;
          case 7:
            if (Random.Range(0, 100) == 0)
            {
              Object.Instantiate<GameObject>(Resources.Load("Prefabs/Crops/Flower") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
              break;
            }
            Object.Instantiate<GameObject>(Resources.Load("Prefabs/Crops/Long Grass") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            break;
          case 8:
            Object.Instantiate<GameObject>(Resources.Load("Prefabs/Structures/Statue - Sword") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            break;
          case 9:
            Object.Instantiate<GameObject>(Resources.Load("Prefabs/Structures/Village/Village") as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            break;
          case 10:
            Object.Instantiate<GameObject>(Resources.Load("Prefabs/Environment/Base/Rocks/ROCK_" + Random.Range(0, 3).ToString()) as GameObject, GameObject.FindGameObjectWithTag("Structures Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 - this.width / 2, index2 - this.height / 2, 0)) + this.gridLayout.cellSize / 2f;
            break;
        }
      }
    }
  }

  public void UpdateStructures(Room r, Vector3 position, int value)
  {
    if ((Object) this.gridLayout == (Object) null)
      return;
    position = (Vector3) this.gridLayout.WorldToCell(position);
    if (r.StructuresOld == null)
      return;
    r.StructuresOld[(int) position.x + this.width / 2, (int) position.y + this.height / 2] = value;
  }

  public void PlaceEnemies(Room r)
  {
    if (r.isHome || r.pointOfInterest)
      return;
    for (int index1 = 0; index1 < r.Width; ++index1)
    {
      for (int index2 = 0; index2 < r.Height; ++index2)
      {
        if (r.RoomGrid[index1, index2] == 0 && Random.Range(0, 30) < 1)
        {
          Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Enemy2") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 * 4 - this.width / 2 + 1, index2 * 4 - this.height / 2 + 2, 0)) + this.gridLayout.cellSize / 2f;
          Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Enemy2") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true).transform.position = this.gridLayout.CellToWorld(new Vector3Int(index1 * 4 - this.width / 2 + 3, index2 * 4 - this.height / 2 + 2, 0)) + this.gridLayout.cellSize / 2f;
        }
      }
    }
  }

  public void PlaceWildLife(Room r)
  {
  }

  public enum STRUCTURES
  {
    NONE,
    STRUCTURE_TILE1,
    STRUCTURE_TILE2,
    STRUCTURE_TILE3,
    STRUCTURE_TREE,
    STRUCTURE_TREE_STUMP,
    STRUCTURE_BUSH,
    STRUCTURE_GRASS,
    STRUCTURE_STATUE_SWORD,
    STRUCTURE_VILLAGE,
    STRUCTURE_STONE,
  }
}
