// Decompiled with JetBrains decompiler
// Type: MMDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MMDebug
{
  public static void DrawCube(Vector3 center, Vector3 size, Color color, float duration = 0.0f)
  {
    MMDebug.DrawCube(center, size, Quaternion.identity, color, duration);
  }

  public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation, float duration = 0.0f)
  {
    MMDebug.DrawCube(center, size, rotation, Color.red, duration);
  }

  public static void DrawCube(
    Vector3 center,
    Vector3 size,
    Quaternion rotation,
    Color color,
    float duration = 0.0f)
  {
    size /= 2f;
    Vector3 vector3_1 = new Vector3();
    Vector3 vector3_2 = new Vector3();
    Vector3 vector3_3 = new Vector3();
    Vector3 vector3_4 = new Vector3();
    vector3_1.x = size.x;
    vector3_1.y = size.y;
    vector3_1.z = size.z;
    vector3_2.x = -size.x;
    vector3_2.y = size.y;
    vector3_2.z = size.z;
    vector3_3.x = size.x;
    vector3_3.y = size.y;
    vector3_3.z = -size.z;
    vector3_4.x = -size.x;
    vector3_4.y = size.y;
    vector3_4.z = -size.z;
    Vector3 vector3_5 = new Vector3();
    Vector3 vector3_6 = new Vector3();
    Vector3 vector3_7 = new Vector3();
    Vector3 vector3_8 = new Vector3();
    Vector3 vector3_9 = vector3_1 with { y = -size.y };
    Vector3 vector3_10 = vector3_2 with { y = -size.y };
    Vector3 vector3_11 = vector3_3 with { y = -size.y };
    Vector3 vector3_12 = vector3_4 with { y = -size.y };
    Vector3 vector3_13 = rotation * vector3_1;
    Vector3 vector3_14 = rotation * vector3_2;
    Vector3 vector3_15 = rotation * vector3_3;
    Vector3 vector3_16 = rotation * vector3_4;
    Vector3 vector3_17 = rotation * vector3_9;
    Vector3 vector3_18 = rotation * vector3_10;
    Vector3 vector3_19 = rotation * vector3_11;
    Vector3 vector3_20 = rotation * vector3_12;
    Vector3 start = center + vector3_13;
    Vector3 vector3_21 = center + vector3_14;
    Vector3 vector3_22 = center + vector3_15;
    Vector3 vector3_23 = center + vector3_16;
    Vector3 vector3_24 = center + vector3_17;
    Vector3 vector3_25 = center + vector3_18;
    Vector3 vector3_26 = center + vector3_19;
    Vector3 end = center + vector3_20;
    Debug.DrawLine(start, vector3_21, color, duration);
    Debug.DrawLine(vector3_21, vector3_23, color, duration);
    Debug.DrawLine(vector3_22, vector3_23, color, duration);
    Debug.DrawLine(start, vector3_22, color, duration);
    Debug.DrawLine(vector3_24, vector3_25, color, duration);
    Debug.DrawLine(vector3_25, end, color, duration);
    Debug.DrawLine(vector3_26, end, color, duration);
    Debug.DrawLine(vector3_24, vector3_26, color, duration);
    Debug.DrawLine(start, vector3_24, color, duration);
    Debug.DrawLine(vector3_21, vector3_25, color, duration);
    Debug.DrawLine(vector3_22, vector3_26, color, duration);
    Debug.DrawLine(vector3_23, end, color, duration);
  }

  public static void DrawArrow(Vector3 position, Vector3 dir, Color colour, float size = 1f)
  {
    Vector3 vector3_1 = Quaternion.LookRotation(dir) * Quaternion.Euler(new Vector3(0.0f, -135f, 0.0f)) * Vector3.forward;
    Vector3 vector3_2 = Quaternion.LookRotation(dir) * Quaternion.Euler(new Vector3(0.0f, 135f, 0.0f)) * Vector3.forward;
    Vector3 end1 = position + vector3_1 * size;
    Vector3 end2 = position + vector3_2 * size;
    Debug.DrawLine(position, end1, colour);
    Debug.DrawLine(position, end2, colour);
  }

  public static void DrawCross(Vector3 position, Color colour, float size = 1f, float duration = 0.0f)
  {
    Vector3 vector3_1 = new Vector3(size, 0.0f, 0.0f);
    Vector3 vector3_2 = new Vector3(0.0f, size, 0.0f);
    Debug.DrawLine(position - vector3_1, position + vector3_1, colour, duration);
    Debug.DrawLine(position - vector3_2, position + vector3_2, colour, duration);
  }
}
