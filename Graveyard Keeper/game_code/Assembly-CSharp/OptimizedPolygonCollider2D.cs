// Decompiled with JetBrains decompiler
// Type: OptimizedPolygonCollider2D
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OptimizedPolygonCollider2D : OptimizedCollider2D
{
  public Vector2 offset = Vector2.zero;
  public Vector2[] points;
  public PolygonCollider2D _collider;
  public Mesh _mesh;
  public const bool DRAW_AS_MESH = true;

  public override void OnInit()
  {
    SpriteRenderer component = this.gameObject.GetComponent<SpriteRenderer>();
    if ((Object) component != (Object) null)
    {
      UnityEngine.Sprite sprite = component.sprite;
      component.sprite = (UnityEngine.Sprite) null;
      this.AddPolygonCollider();
      component.sprite = sprite;
    }
    else
      this.AddPolygonCollider();
  }

  public void AddPolygonCollider()
  {
    this._collider = this.gameObject.AddComponent<PolygonCollider2D>();
    this._collider.offset = this.offset;
    this._collider.isTrigger = this.is_trigger;
    this._collider.points = this.points;
  }

  public override void OnDrawGizmosSelected()
  {
  }

  public void RebuildColliderMesh() => this._mesh = this.GetGizmosMesh();

  public override Bounds CalculateLocalBounds()
  {
    Bounds localBounds = new Bounds();
    for (int index = 0; index < this.points.Length; ++index)
    {
      Vector2 point = this.points[index];
      if (index == 0)
        localBounds = new Bounds((Vector3) (point + this.offset), Vector3.one * (1f / 1000f));
      else
        localBounds.Encapsulate(new Bounds((Vector3) (point + this.offset), Vector3.one * (1f / 1000f)));
    }
    return localBounds;
  }

  public Mesh GetGizmosMesh()
  {
    Vector3[] vector3Array = new Vector3[this.points.Length * 2];
    for (int index = 0; index < this.points.Length; ++index)
    {
      vector3Array[index * 2] = new Vector3(this.points[index].x, this.points[index].y, 0.0f);
      vector3Array[index * 2 + 1] = new Vector3(this.points[index].x + 0.0001f, this.points[index].y, 0.0f);
    }
    int length = this.points.Length;
    int[] numArray = new int[length * 3];
    for (int index = 0; index < length; ++index)
    {
      numArray[index * 3] = index * 2;
      numArray[index * 3 + 1] = index * 2 + 1;
      numArray[index * 3 + 2] = index == length - 1 ? 0 : index * 2 + 2;
    }
    Mesh gizmosMesh = new Mesh();
    gizmosMesh.vertices = vector3Array;
    gizmosMesh.triangles = numArray;
    gizmosMesh.RecalculateNormals();
    gizmosMesh.RecalculateBounds();
    return gizmosMesh;
  }
}
