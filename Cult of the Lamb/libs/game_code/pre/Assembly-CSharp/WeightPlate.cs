// Decompiled with JetBrains decompiler
// Type: WeightPlate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WeightPlate : BaseMonoBehaviour
{
  public bool Reserved;
  public WeightPlateManager WeightPlateManager;
  public SpriteRenderer DoorWeightSignalColor;
  public SpriteRenderer WeightColor;
  private List<Collider2D> CurrentCollisions = new List<Collider2D>();

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
