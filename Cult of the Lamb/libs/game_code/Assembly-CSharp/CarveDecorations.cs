// Decompiled with JetBrains decompiler
// Type: CarveDecorations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using UnityEngine;

#nullable disable
public class CarveDecorations : MonoBehaviour
{
  public GenerateRoom generateRoom;
  public Collider2D col;

  public void Awake()
  {
    this.col = this.GetComponent<Collider2D>();
    this.generateRoom = this.GetComponentInParent<GenerateRoom>();
    this.generateRoom.OnGenerated += new GenerateRoom.GenerateEvent(this.OnGenerated);
    this.generateRoom.OnRegenerated += new GenerateRoom.GenerateEvent(this.OnGenerated);
  }

  public void OnDestroy()
  {
    if (!((Object) this.generateRoom != (Object) null))
      return;
    this.generateRoom.OnGenerated -= new GenerateRoom.GenerateEvent(this.OnGenerated);
    this.generateRoom.OnRegenerated -= new GenerateRoom.GenerateEvent(this.OnGenerated);
  }

  public void OnGenerated()
  {
    for (int index = this.generateRoom.SceneryTransform.transform.childCount - 1; index >= 0; --index)
    {
      if (this.col.ClosestPoint((Vector2) this.generateRoom.SceneryTransform.transform.GetChild(index).position) == (Vector2) this.generateRoom.SceneryTransform.transform.GetChild(index).position)
        this.generateRoom.SceneryTransform.transform.GetChild(index).gameObject.SetActive(false);
    }
  }
}
