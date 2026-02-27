// Decompiled with JetBrains decompiler
// Type: PlacementStructureHighlighter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlacementStructureHighlighter : BaseMonoBehaviour
{
  public StructureBrain.TYPES StructureType;
  public float Distance;
  public Vector3 PositionOffset;
  private List<Structure> _inRange = new List<Structure>();
  private List<Structure> _outOfRange = new List<Structure>();
  private List<Structure> _newInRange = new List<Structure>();
  private List<Structure> _newOutOfRange = new List<Structure>();
  private BoxCollider2D bounds;

  private void Start()
  {
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type == this.StructureType)
        this._outOfRange.Add(structure);
    }
    this.bounds = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) this.bounds == (Object) null)
    {
      this.bounds = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      this.bounds.isTrigger = true;
    }
    this.bounds.size = Vector2.one * this.Distance;
    this.bounds.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
  }

  private void OnDestroy()
  {
    for (int index = 0; index < this._inRange.Count; ++index)
      this.SetHighlight(this._inRange[index], false);
    this._inRange.Clear();
    this._outOfRange.Clear();
    this._newInRange.Clear();
    this._newOutOfRange.Clear();
  }

  private void Update()
  {
    this.bounds.transform.position = this.transform.position + this.PositionOffset;
    for (int index = 0; index < this._outOfRange.Count; ++index)
    {
      Structure structure = this._outOfRange[index];
      if (this.bounds.OverlapPoint((Vector2) structure.transform.position))
      {
        this._newInRange.Add(structure);
        this._outOfRange.RemoveAt(index--);
        this.SetHighlight(structure, true);
      }
    }
    for (int index = 0; index < this._inRange.Count; ++index)
    {
      Structure structure = this._inRange[index];
      if (!this.bounds.OverlapPoint((Vector2) structure.transform.position))
      {
        this._newOutOfRange.Add(structure);
        this._inRange.RemoveAt(index--);
        this.SetHighlight(structure, false);
      }
    }
    this._inRange.AddRange((IEnumerable<Structure>) this._newInRange);
    this._newInRange.Clear();
    this._outOfRange.AddRange((IEnumerable<Structure>) this._newOutOfRange);
    this._newOutOfRange.Clear();
  }

  private void SetHighlight(Structure structure, bool active)
  {
    foreach (SpriteRenderer componentsInChild in structure.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = active ? Color.green : Color.white;
  }
}
