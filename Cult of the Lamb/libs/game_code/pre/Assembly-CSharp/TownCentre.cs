// Decompiled with JetBrains decompiler
// Type: TownCentre
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TownCentre : BaseMonoBehaviour
{
  public Transform Centre;
  public static TownCentre Instance;
  public float Height;
  public float Width;
  private static Vector3 CachedCentrePos;
  private static float CachedHeight;
  private static float CachedWidth;
  private static List<float> townCentreRandomAngles = new List<float>(TownCentre.numTownCentreRandomAngles);
  private static int numTownCentreRandomAngles = 50;
  private static int townCentreRandomIndex = 0;

  private void Awake()
  {
    TownCentre.Instance = this;
    TownCentre.CachedCentrePos = this.Centre.position;
    TownCentre.CachedHeight = this.Height;
    TownCentre.CachedWidth = this.Width;
    for (int index = 0; index < TownCentre.numTownCentreRandomAngles; ++index)
      TownCentre.townCentreRandomAngles.Add(360f / (float) TownCentre.numTownCentreRandomAngles * (float) index);
    int count = TownCentre.townCentreRandomAngles.Count;
    int num = count - 1;
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = Random.Range(index1, count);
      float centreRandomAngle = TownCentre.townCentreRandomAngles[index1];
      TownCentre.townCentreRandomAngles[index1] = TownCentre.townCentreRandomAngles[index2];
      TownCentre.townCentreRandomAngles[index2] = centreRandomAngle;
    }
    TownCentre.townCentreRandomIndex = 0;
  }

  public Vector3 RandomPositionInTownCentre()
  {
    Vector3 a;
    do
    {
      a = this.Centre.position + new Vector3(Random.Range((float) (-(double) this.Width / 2.0), this.Width / 2f), Random.Range((float) (-(double) this.Height / 2.0), this.Height / 2f));
    }
    while ((double) Vector3.Distance(a, this.Centre.position) < 2.0 && (double) this.Width > 0.0 && (double) this.Height > 0.0);
    return a;
  }

  public static Vector3 RandomCircleFromTownCentre(float Radius)
  {
    return TownCentre.CachedCentrePos + (Vector3) (Random.insideUnitCircle * Radius);
  }

  public static Vector3 RandomPositionInCachedTownCentre()
  {
    TownCentre.townCentreRandomIndex = (TownCentre.townCentreRandomIndex + 1) % TownCentre.numTownCentreRandomAngles;
    return TownCentre.townCentreRandomIndex >= TownCentre.townCentreRandomAngles.Count ? Vector3.zero : TownCentre.CachedCentrePos + (Vector3) Utils.DegreeToVector2(TownCentre.townCentreRandomAngles[TownCentre.townCentreRandomIndex]) * Random.Range(2f, Mathf.Max(TownCentre.CachedWidth, TownCentre.CachedHeight));
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(0.0f, 0.0f, 1f, 0.3f);
    Gizmos.DrawCube(this.Centre.position, new Vector3(this.Width, this.Height, 0.0f));
  }
}
