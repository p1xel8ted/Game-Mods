// Decompiled with JetBrains decompiler
// Type: DungeonRoomInteriorEditor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DungeonRoomInteriorEditor : MonoBehaviour
{
  public string current_interior_name = "interior_name";
  public DungeonRoomInterior.BiomType biom_type = DungeonRoomInterior.BiomType.Unknown;
  public string room_type = "";
  public DungeonRoomInterior.RoomSize room_size = DungeonRoomInterior.RoomSize.Unknown;
  [HideInInspector]
  public string saved_possible_enters_log = "";
  public const float CHECK_COLLIDER_DEPTH = 0.5f;
  public const float DOWN_CHECK_COLLIDER_DEPTH = 1.25f;
  public int _room_height = 1;
  public int _room_width = 1;

  public int[,] GenerateInteriorMatrix(WorldSimpleObject[] wsos)
  {
    int[,] interiorMatrix = new int[this._room_width, this._room_height];
    foreach (WorldSimpleObject wso in wsos)
    {
      Vector2 localPosition = (Vector2) wso.transform.localPosition;
      if (wso.wso_type == WorldSimpleObject.WSOType.WallStraight)
        interiorMatrix[Mathf.FloorToInt(localPosition.x), Mathf.FloorToInt(localPosition.y)] = 1;
    }
    return interiorMatrix;
  }

  public List<List<IntVector2>> FindPossibleEnters(
    int[,] t_room_interior_matrix,
    int t_corridor_width = 3)
  {
    if (t_corridor_width < 1)
    {
      Debug.LogError((object) "Corridor width can not be less than 1!");
      return (List<List<IntVector2>>) null;
    }
    if (this._room_height < 2 + t_corridor_width && this._room_width < 2 + t_corridor_width)
    {
      Debug.LogError((object) ("Corridor width can not be more than (room_height/2 - 1)! " + t_corridor_width.ToString()));
      return (List<List<IntVector2>>) null;
    }
    List<List<IntVector2>> possible_enters = new List<List<IntVector2>>();
    for (int index = 0; index < 4; ++index)
      possible_enters.Add(new List<IntVector2>());
    for (int t_x = 1; t_x < this._room_width - t_corridor_width; ++t_x)
    {
      int num1 = 0;
      while (t_room_interior_matrix[t_x + num1, 0] == 1)
      {
        ++num1;
        if (num1 == t_corridor_width)
          break;
      }
      if (num1 == t_corridor_width)
        possible_enters[3].Add(new IntVector2(t_x));
      int num2 = 0;
      while (t_room_interior_matrix[t_x + num2, this._room_height - 1] == 1)
      {
        ++num2;
        if (num2 == t_corridor_width)
          break;
      }
      if (num2 == t_corridor_width)
        possible_enters[1].Add(new IntVector2(t_x, this._room_height - 1));
    }
    for (int t_y = 1; t_y < this._room_height - t_corridor_width; ++t_y)
    {
      int num3 = 0;
      while (t_room_interior_matrix[0, t_y + num3] == 1)
      {
        ++num3;
        if (num3 == t_corridor_width)
          break;
      }
      if (num3 == t_corridor_width)
        possible_enters[0].Add(new IntVector2(t_y: t_y));
      int num4 = 0;
      while (t_room_interior_matrix[this._room_width - 1, t_y + num4] == 1)
      {
        ++num4;
        if (num4 == t_corridor_width)
          break;
      }
      if (num4 == t_corridor_width)
        possible_enters[2].Add(new IntVector2(this._room_width - 1, t_y));
    }
    this.CheckPossibleEntersWithColliders(ref possible_enters, t_corridor_width);
    return possible_enters;
  }

  public bool IsCorrectCoords()
  {
    foreach (WorldGameObject componentsInChild in this.gameObject.GetComponentsInChildren<WorldGameObject>())
    {
      if ((double) componentsInChild.transform.localPosition.x < 0.0 || (double) componentsInChild.transform.localPosition.y < 0.0)
        return false;
    }
    return true;
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0.0f, 0.4f, 0.0f, 0.1f);
    Gizmos.DrawCube(this.transform.position + new Vector3(336f, 336f), new Vector3(768f, 768f, 0.0f));
    Gizmos.color = new Color(0.4f, 0.0f, 0.0f, 0.1f);
    Gizmos.DrawCube(this.transform.position + new Vector3(480f, 480f), new Vector3(1056f, 1056f, 0.0f));
    Gizmos.color = new Color(0.0f, 0.0f, 0.4f, 0.1f);
    Gizmos.DrawCube(this.transform.position + new Vector3(624f, 624f), new Vector3(1344f, 1344f, 0.0f));
    Gizmos.color = new Color(0.2f, 0.0f, 0.2f, 0.1f);
    Gizmos.DrawCube(this.transform.position + new Vector3(768f, 768f), new Vector3(1632f, 1632f, 0.0f));
    Gizmos.color = new Color(0.0f, 0.2f, 0.2f, 0.1f);
    Gizmos.DrawCube(this.transform.position + new Vector3(912f, 912f), new Vector3(1920f, 1920f, 0.0f));
  }

  public void CheckPossibleEntersWithColliders(
    ref List<List<IntVector2>> possible_enters,
    int corridor_width)
  {
    foreach (OptimizedCollider2D componentsInChild in this.gameObject.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
    List<List<IntVector2>> intVector2ListList = new List<List<IntVector2>>();
    for (int index = 0; index < 4; ++index)
      intVector2ListList.Add(new List<IntVector2>());
    for (int index = 0; index < 4; ++index)
    {
      foreach (IntVector2 intVector2 in possible_enters[index])
      {
        Vector2 zero1 = Vector2.zero;
        Vector2 zero2 = Vector2.zero;
        Vector2 position = (Vector2) this.gameObject.transform.position;
        switch (index)
        {
          case 0:
            zero1.x = position.x + (float) intVector2.x * 96f;
            zero1.y = position.y + (float) intVector2.y * 96f;
            zero2.x = zero1.x + 48f;
            zero2.y = zero1.y + (float) (corridor_width - 1) * 96f;
            break;
          case 1:
            zero1.x = position.x + (float) intVector2.x * 96f;
            zero1.y = position.y + (float) intVector2.y * 96f;
            zero2.x = zero1.x + (float) (corridor_width - 1) * 96f;
            zero2.y = zero1.y - 48f;
            break;
          case 2:
            zero1.x = position.x + (float) intVector2.x * 96f;
            zero1.y = position.y + (float) intVector2.y * 96f;
            zero2.x = zero1.x - 48f;
            zero2.y = zero1.y + (float) (corridor_width - 1) * 96f;
            break;
          case 3:
            zero1.x = position.x + (float) intVector2.x * 96f;
            zero1.y = position.y + (float) intVector2.y * 96f;
            zero2.x = zero1.x + (float) (corridor_width - 1) * 96f;
            zero2.y = zero1.y + 120f;
            break;
          default:
            Debug.LogError((object) "This is IMPOSSIBRU!!!!");
            break;
        }
        if (!(zero1 == Vector2.zero) && !(zero2 == Vector2.zero))
        {
          Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(zero1, zero2, 1);
          if (collider2DArray != null && collider2DArray.Length != 0)
          {
            string str = "";
            int num = 0;
            foreach (Collider2D collider2D in collider2DArray)
            {
              if (collider2D.gameObject.layer != 0)
              {
                Debug.LogError((object) "This is can't be.");
              }
              else
              {
                WorldSimpleObject componentInParent1 = collider2D.transform.GetComponentInParent<WorldSimpleObject>();
                if ((Object) componentInParent1 != (Object) null)
                {
                  if (componentInParent1.wso_type != WorldSimpleObject.WSOType.WallCorner && componentInParent1.wso_type != WorldSimpleObject.WSOType.WallStraight && componentInParent1.wso_type != WorldSimpleObject.WSOType.Floor)
                  {
                    if (!intVector2ListList[index].Contains(intVector2))
                      intVector2ListList[index].Add(intVector2);
                    str = $"{str}{componentInParent1.name}=>{collider2D.name}\n";
                    ++num;
                  }
                }
                else
                {
                  WorldObjectPart componentInParent2 = collider2D.transform.GetComponentInParent<WorldObjectPart>();
                  if ((Object) componentInParent2 != (Object) null)
                  {
                    if (!intVector2ListList[index].Contains(intVector2))
                      intVector2ListList[index].Add(intVector2);
                    str = $"{str}{componentInParent2.name}=>{collider2D.name}\n";
                    ++num;
                  }
                }
              }
            }
            if (num > 0)
              Debug.Log((object) $"[{zero1.ToString()}]:[{zero2.ToString()}]:: Found {num.ToString()} overlaping colliders: \n{str}");
          }
        }
      }
    }
    string message = "Removed wrong enter: ";
    for (int index = 0; index < 4; ++index)
    {
      message = $"{message}\n{((DungeonWalker.Direction) index).ToString()}: ";
      if (intVector2ListList[index].Count == 0)
      {
        message += "None";
      }
      else
      {
        foreach (IntVector2 intVector2 in intVector2ListList[index])
        {
          possible_enters[index].Remove(intVector2);
          message = $"{message}{intVector2?.ToString()}; ";
        }
      }
    }
    Debug.Log((object) message);
  }

  public void PreparePossibleEntersLog(DungeonRoomInterior asset)
  {
    if ((Object) asset == (Object) null)
      return;
    this.saved_possible_enters_log = "";
    for (int index = 3; index <= 7; ++index)
    {
      List<IntVectors> intVectorsList = (List<IntVectors>) null;
      switch (index)
      {
        case 3:
          intVectorsList = asset.possible_enters_3;
          break;
        case 4:
          intVectorsList = asset.possible_enters_4;
          break;
        case 5:
          intVectorsList = asset.possible_enters_5;
          break;
        case 6:
          intVectorsList = asset.possible_enters_6;
          break;
        case 7:
          intVectorsList = asset.possible_enters_7;
          break;
      }
      if (intVectorsList != null)
      {
        string str = "";
        foreach (IntVectors intVectors in intVectorsList)
        {
          if (intVectors == null || intVectors.list == null || intVectors.list.Count == 0)
            str = $"{str} {((DungeonWalker.Direction) intVectorsList.IndexOf(intVectors)).ToString()}";
        }
        if (!string.IsNullOrEmpty(str))
          this.saved_possible_enters_log = $"{this.saved_possible_enters_log}\n{index.ToString()}.{str};";
      }
    }
  }
}
