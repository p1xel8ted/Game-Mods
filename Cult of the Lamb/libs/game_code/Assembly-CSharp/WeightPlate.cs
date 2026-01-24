// Decompiled with JetBrains decompiler
// Type: WeightPlate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WeightPlate : BaseMonoBehaviour
{
  public bool Reserved;
  public WeightPlateManager WeightPlateManager;
  public SpriteRenderer DoorWeightSignalColor;
  public SpriteRenderer WeightColor;
  public List<Collider2D> CurrentCollisions = new List<Collider2D>();

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (this.CurrentCollisions.Count <= 0)
    {
      ++this.WeightPlateManager.ActivatedCount;
      this.DoorWeightSignalColor.color = Color.green;
      this.WeightColor.color = Color.green;
    }
    this.CurrentCollisions.Add(other);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    this.CurrentCollisions.Remove(other);
    if (this.CurrentCollisions.Count > 0)
      return;
    --this.WeightPlateManager.ActivatedCount;
    this.DoorWeightSignalColor.color = Color.red;
    this.WeightColor.color = Color.red;
  }
}
