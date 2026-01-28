// Decompiled with JetBrains decompiler
// Type: WeightPlate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
