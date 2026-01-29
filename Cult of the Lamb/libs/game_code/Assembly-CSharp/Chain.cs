// Decompiled with JetBrains decompiler
// Type: Chain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteAlways]
public class Chain : BaseMonoBehaviour
{
  public Transform FixedPoint1;
  public Transform FixedPoint2;
  public List<float> DeltasLX = new List<float>();
  public List<float> DeltasRX = new List<float>();
  public List<float> DeltasLY = new List<float>();
  public List<float> DeltasRY = new List<float>();
  public List<float> DeltasLZ = new List<float>();
  public List<float> DeltasRZ = new List<float>();
  public ChainLink[] Links;
  public ChainConnection[] Connections;
  public float xSpread = 0.025f;
  public float ySpread = 0.025f;
  public float zSpread = 0.025f;

  public void Start()
  {
    this.Links = this.GetComponentsInChildren<ChainLink>();
    this.Connections = this.GetComponentsInChildren<ChainConnection>();
    int num = 0;
    foreach (ChainLink link in this.Links)
    {
      this.DeltasLX.Add(0.0f);
      this.DeltasRX.Add(0.0f);
      this.DeltasLY.Add(0.0f);
      this.DeltasRY.Add(0.0f);
      this.DeltasLZ.Add(0.0f);
      this.DeltasRZ.Add(0.0f);
      if (++num % 2 == 0)
        link.transform.localScale = link.transform.localScale * 0.5f;
    }
    for (int index = 0; index < this.Links.Length; ++index)
      this.Links[index].transform.position = Vector3.Lerp(this.FixedPoint1.position, this.FixedPoint2.position, (float) (index / this.Links.Length));
  }

  public void SetConnection(Transform ConnectionTransform)
  {
    this.FixedPoint2 = ConnectionTransform;
    this.gameObject.SetActive(true);
  }

  public void Disconnect() => this.FixedPoint2 = (Transform) null;

  public void Update()
  {
    if ((Object) this.FixedPoint1 == (Object) null || (Object) this.FixedPoint2 == (Object) null)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      for (int index = 0; index < this.Links.Length; ++index)
      {
        if (index == 0)
          this.Links[index].transform.position = this.FixedPoint1.position;
        else if (index == this.Links.Length - 1)
          this.Links[index].transform.position = this.FixedPoint2.position;
        else
          this.Links[index].UpdatePositions(this.Links[index].transform.position);
      }
      for (int index = 0; index < this.Connections.Length; ++index)
        this.Connections[index].UpdatePosition(this.Links[index].transform.position, this.Links[index + 1].transform.position);
      for (int index1 = 0; index1 < 8; ++index1)
      {
        for (int index2 = 0; index2 < this.Links.Length; ++index2)
        {
          if (index2 > 0)
          {
            this.DeltasLX[index2] = this.xSpread * (this.Links[index2].x - this.Links[index2 - 1].x);
            this.Links[index2 - 1].xSpeed -= this.DeltasLX[index2];
            this.DeltasLY[index2] = this.ySpread * (this.Links[index2].y - this.Links[index2 - 1].y);
            this.Links[index2 - 1].ySpeed -= this.DeltasLY[index2];
            this.DeltasLZ[index2] = this.zSpread * (this.Links[index2].z - this.Links[index2 - 1].z);
            this.Links[index2 - 1].zSpeed -= this.DeltasLZ[index2];
          }
          if (index2 < this.Links.Length - 1)
          {
            this.DeltasRX[index2] = this.xSpread * (this.Links[index2].x - this.Links[index2 + 1].x);
            this.Links[index2 + 1].xSpeed -= this.DeltasRX[index2];
            this.DeltasRY[index2] = this.ySpread * (this.Links[index2].y - this.Links[index2 + 1].y);
            this.Links[index2 + 1].ySpeed -= this.DeltasRY[index2];
            this.DeltasRZ[index2] = this.zSpread * (this.Links[index2].z - this.Links[index2 + 1].z);
            this.Links[index2 + 1].zSpeed -= this.DeltasRZ[index2];
          }
        }
        for (int index3 = 0; index3 < this.Links.Length; ++index3)
        {
          if (index3 > 0)
          {
            this.Links[index3 - 1].x += this.DeltasLX[index3];
            this.Links[index3 - 1].y += this.DeltasLY[index3];
            this.Links[index3 - 1].z += this.DeltasLZ[index3];
          }
          if (index3 < this.Links.Length - 1)
          {
            this.Links[index3 + 1].x += this.DeltasRX[index3];
            this.Links[index3 + 1].y += this.DeltasRY[index3];
            this.Links[index3 + 1].z += this.DeltasRZ[index3];
          }
        }
      }
    }
  }
}
