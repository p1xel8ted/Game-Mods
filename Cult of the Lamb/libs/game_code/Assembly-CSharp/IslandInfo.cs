// Decompiled with JetBrains decompiler
// Type: IslandInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.U2D;

#nullable disable
[ExecuteInEditMode]
public class IslandInfo : BaseMonoBehaviour
{
  public bool NorthCollider = true;
  public bool EastCollider = true;
  public bool SouthCollider = true;
  public bool WestCollider = true;
  public float NorthColliderSpacing = 2f;
  public float EastColliderSpacing = 2f;
  public float SouthColliderSpacing = 2f;
  public float WestColliderSpacing = 2f;
  public float NorthOffset;
  public float EastOffset;
  public float SouthOffset;
  public float WestOffset;
  public Vector3 Position;
  public Vector3 Size;
  public bool TopLeft = true;
  public bool TopRight = true;
  public bool BottomLeft = true;
  public bool BottomRight = true;

  public void SetSpriteShape()
  {
    SpriteShapeController componentInChildren = this.GetComponentInChildren<SpriteShapeController>();
    MeshRenderer component = this.GetComponent<MeshRenderer>();
    float x = component.bounds.extents.x;
    float y = component.bounds.extents.y;
    Spline spline = componentInChildren.spline;
    spline.isOpenEnded = false;
    spline.Clear();
    Vector3 vector3 = component.bounds.center - this.transform.position;
    spline.InsertPointAt(0, vector3 + new Vector3((float) (-(double) x + 1.0), y - 1f));
    spline.InsertPointAt(1, vector3 + new Vector3(x - 1f, y - 1f));
    spline.InsertPointAt(2, vector3 + new Vector3(x - 1f, (float) (-(double) y + 1.0)));
    spline.InsertPointAt(3, vector3 + new Vector3((float) (-(double) x + 1.0), (float) (-(double) y + 1.0)));
  }

  public void SetColliderGapsToZero()
  {
    this.NorthColliderSpacing = this.EastColliderSpacing = this.SouthColliderSpacing = this.WestColliderSpacing = 0.0f;
    this.CreateColliders();
  }

  public void CreateColliders()
  {
    foreach (Component componentsInChild in this.GetComponentsInChildren<Collider2D>())
      Object.DestroyImmediate((Object) componentsInChild.gameObject);
    MeshRenderer component = this.GetComponent<MeshRenderer>();
    Bounds bounds = component.bounds;
    Vector3 extents1 = bounds.extents;
    bounds = component.bounds;
    Vector3 extents2 = bounds.extents;
    if (this.NorthCollider)
      this.CreateNorthCollider();
    if (this.EastCollider)
      this.CreateEastCollider();
    if (this.SouthCollider)
      this.CreateSouthCollider();
    if (this.WestCollider)
      this.CreateWestCollider();
    if (!((Object) this.GetComponent<MeshCollider>() == (Object) null))
      return;
    this.gameObject.AddComponent<MeshCollider>();
  }

  public void CreateNorthCollider()
  {
    MeshRenderer component = this.GetComponent<MeshRenderer>();
    float x = component.bounds.extents.x;
    float y = component.bounds.extents.y;
    Transform transform1 = this.transform.Find("Collider North");
    if ((Object) transform1 != (Object) null)
      Object.DestroyImmediate((Object) transform1.gameObject);
    Transform transform2 = this.transform.Find("Collider North Top");
    if ((Object) transform2 != (Object) null)
      Object.DestroyImmediate((Object) transform2.gameObject);
    Transform transform3 = this.transform.Find("Collider North Bottom");
    if ((Object) transform3 != (Object) null)
      Object.DestroyImmediate((Object) transform3.gameObject);
    if (!this.NorthCollider)
      return;
    if ((double) this.NorthColliderSpacing > 0.0)
    {
      if (!this.TopRight)
      {
        this.Position = component.bounds.center + new Vector3((float) ((double) x / 2.0 + (double) this.NorthColliderSpacing / 4.0 + (double) this.NorthOffset / 4.0), y + 0.5f);
        this.Size = (Vector3) new Vector2((float) ((double) x * 2.0 / 2.0 - (double) this.NorthColliderSpacing / 2.0 - (double) this.NorthOffset / 2.0), 1f);
      }
      else
      {
        this.Position = component.bounds.center + new Vector3((float) ((double) x / 2.0 + (double) this.NorthColliderSpacing / 4.0 + 0.5 + (double) this.NorthOffset / 4.0), y + 0.5f);
        this.Size = (Vector3) new Vector2((float) (((double) x * 2.0 + 2.0) / 2.0 - (double) this.NorthColliderSpacing / 2.0 - (double) this.NorthOffset / 2.0), 1f);
      }
      this.CreateCollider("Collider North Top", this.Position, (Vector2) this.Size);
      if (!this.TopLeft)
      {
        this.Position = component.bounds.center + new Vector3((float) (-((double) x / 2.0) - (double) this.NorthColliderSpacing / 4.0 + (double) this.NorthOffset / 4.0), y + 0.5f);
        this.Size = (Vector3) new Vector2((float) ((double) x * 2.0 / 2.0 - (double) this.NorthColliderSpacing / 2.0 + (double) this.NorthOffset / 2.0), 1f);
      }
      else
      {
        this.Position = component.bounds.center + new Vector3((float) (-((double) x / 2.0) - (double) this.NorthColliderSpacing / 4.0 - 0.5 + (double) this.NorthOffset / 4.0), y + 0.5f);
        this.Size = (Vector3) new Vector2((float) (((double) x * 2.0 + 2.0) / 2.0 - (double) this.NorthColliderSpacing / 2.0 + (double) this.NorthOffset / 2.0), 1f);
      }
      this.CreateCollider("Collider North Bottom", this.Position, (Vector2) this.Size);
    }
    else if (this.TopLeft && this.TopRight)
      this.CreateCollider("Collider North", component.bounds.center + new Vector3(0.0f, y + 0.5f), new Vector2((float) ((double) x * 2.0 + 2.0), 1f));
    else if (this.TopLeft)
      this.CreateCollider("Collider North", component.bounds.center + new Vector3(-0.5f, y + 0.5f), new Vector2((float) ((double) x * 2.0 + 1.0), 1f));
    else if (this.TopRight)
      this.CreateCollider("Collider North", component.bounds.center + new Vector3(0.5f, y + 0.5f), new Vector2((float) ((double) x * 2.0 + 1.0), 1f));
    else
      this.CreateCollider("Collider North", component.bounds.center + new Vector3(0.0f, y + 0.5f), new Vector2(x * 2f, 1f));
  }

  public void CreateEastCollider()
  {
    MeshRenderer component = this.GetComponent<MeshRenderer>();
    float x = component.bounds.extents.x;
    float y = component.bounds.extents.y;
    Transform transform1 = this.transform.Find("Collider East");
    if ((Object) transform1 != (Object) null)
      Object.DestroyImmediate((Object) transform1.gameObject);
    Transform transform2 = this.transform.Find("Collider East Top");
    if ((Object) transform2 != (Object) null)
      Object.DestroyImmediate((Object) transform2.gameObject);
    Transform transform3 = this.transform.Find("Collider East Bottom");
    if ((Object) transform3 != (Object) null)
      Object.DestroyImmediate((Object) transform3.gameObject);
    if (!this.EastCollider)
      return;
    if ((double) this.EastColliderSpacing > 0.0)
    {
      if (!this.TopRight)
      {
        this.Position = component.bounds.center + new Vector3(x + 0.5f, (float) ((double) y / 2.0 + (double) this.EastColliderSpacing / 4.0 + (double) this.EastOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) ((double) y * 2.0 / 2.0 - (double) this.EastColliderSpacing / 2.0 - (double) this.EastOffset / 2.0));
      }
      else
      {
        this.Position = component.bounds.center + new Vector3(x + 0.5f, (float) ((double) y / 2.0 + (double) this.EastColliderSpacing / 4.0 + 0.5 + (double) this.EastOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) (((double) y * 2.0 + 2.0) / 2.0 - (double) this.EastColliderSpacing / 2.0 - (double) this.EastOffset / 2.0));
      }
      this.CreateCollider("Collider East Top", this.Position, (Vector2) this.Size);
      if (!this.BottomRight)
      {
        this.Position = component.bounds.center + new Vector3(x + 0.5f, (float) (-((double) y / 2.0) - (double) this.EastColliderSpacing / 4.0 + (double) this.EastOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) ((double) y * 2.0 / 2.0 - (double) this.EastColliderSpacing / 2.0 + (double) this.EastOffset / 2.0));
      }
      else
      {
        this.Position = component.bounds.center + new Vector3(x + 0.5f, (float) (-((double) y / 2.0) - (double) this.EastColliderSpacing / 4.0 - 0.5 + (double) this.EastOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) (((double) y * 2.0 + 2.0) / 2.0 - (double) this.EastColliderSpacing / 2.0 + (double) this.EastOffset / 2.0));
      }
      this.CreateCollider("Collider East Bottom", this.Position, (Vector2) this.Size);
    }
    else if (this.TopRight && this.BottomRight)
      this.CreateCollider("Collider East", component.bounds.center + new Vector3(x + 0.5f, 0.0f), new Vector2(1f, (float) ((double) y * 2.0 + 2.0)));
    else if (this.TopRight)
      this.CreateCollider("Collider East", component.bounds.center + new Vector3(x + 0.5f, 0.5f), new Vector2(1f, (float) ((double) y * 2.0 + 1.0)));
    else if (this.BottomRight)
      this.CreateCollider("Collider East", component.bounds.center + new Vector3(x + 0.5f, -0.5f), new Vector2(1f, (float) ((double) y * 2.0 + 1.0)));
    else
      this.CreateCollider("Collider East", component.bounds.center + new Vector3(x + 0.5f, 0.0f), new Vector2(1f, y * 2f));
  }

  public void CreateSouthCollider()
  {
    MeshRenderer component = this.GetComponent<MeshRenderer>();
    float x = component.bounds.extents.x;
    float y = component.bounds.extents.y;
    Transform transform1 = this.transform.Find("Collider South");
    if ((Object) transform1 != (Object) null)
      Object.DestroyImmediate((Object) transform1.gameObject);
    Transform transform2 = this.transform.Find("Collider South Top");
    if ((Object) transform2 != (Object) null)
      Object.DestroyImmediate((Object) transform2.gameObject);
    Transform transform3 = this.transform.Find("Collider South Bottom");
    if ((Object) transform3 != (Object) null)
      Object.DestroyImmediate((Object) transform3.gameObject);
    if (!this.SouthCollider)
      return;
    if ((double) this.SouthColliderSpacing > 0.0)
    {
      if (!this.BottomRight)
      {
        this.Position = component.bounds.center + new Vector3((float) ((double) x / 2.0 + (double) this.SouthColliderSpacing / 4.0 + (double) this.SouthOffset / 4.0), (float) (-(double) y - 0.5));
        this.Size = (Vector3) new Vector2((float) ((double) x * 2.0 / 2.0 - (double) this.SouthColliderSpacing / 2.0 - (double) this.SouthOffset / 2.0), 1f);
      }
      else
      {
        this.Position = component.bounds.center + new Vector3((float) ((double) x / 2.0 + (double) this.SouthColliderSpacing / 4.0 + 0.5 + (double) this.SouthOffset / 4.0), (float) (-(double) y - 0.5));
        this.Size = (Vector3) new Vector2((float) (((double) x * 2.0 + 2.0) / 2.0 - (double) this.SouthColliderSpacing / 2.0 - (double) this.SouthOffset / 2.0), 1f);
      }
      this.CreateCollider("Collider South Top", this.Position, (Vector2) this.Size);
      if (!this.BottomLeft)
      {
        this.Position = component.bounds.center + new Vector3((float) (-((double) x / 2.0) - (double) this.SouthColliderSpacing / 4.0 + (double) this.SouthOffset / 4.0), (float) (-(double) y - 0.5));
        this.Size = (Vector3) new Vector2((float) ((double) x * 2.0 / 2.0 - (double) this.SouthColliderSpacing / 2.0 + (double) this.SouthOffset / 2.0), 1f);
      }
      else
      {
        this.Position = component.bounds.center + new Vector3((float) (-((double) x / 2.0) - (double) this.SouthColliderSpacing / 4.0 - 0.5 + (double) this.SouthOffset / 4.0), (float) (-(double) y - 0.5));
        this.Size = (Vector3) new Vector2((float) (((double) x * 2.0 + 2.0) / 2.0 - (double) this.SouthColliderSpacing / 2.0 + (double) this.SouthOffset / 2.0), 1f);
      }
      this.CreateCollider("Collider South Bottom", this.Position, (Vector2) this.Size);
    }
    else if (this.BottomLeft && this.BottomRight)
      this.CreateCollider("Collider South", component.bounds.center + new Vector3(0.0f, (float) (-(double) y - 0.5)), new Vector2((float) ((double) x * 2.0 + 2.0), 1f));
    else if (this.BottomLeft)
      this.CreateCollider("Collider South", component.bounds.center + new Vector3(-0.5f, (float) (-(double) y - 0.5)), new Vector2((float) ((double) x * 2.0 + 1.0), 1f));
    else if (this.BottomRight)
      this.CreateCollider("Collider South", component.bounds.center + new Vector3(0.5f, (float) (-(double) y - 0.5)), new Vector2((float) ((double) x * 2.0 + 1.0), 1f));
    else
      this.CreateCollider("Collider South", component.bounds.center + new Vector3(0.0f, (float) (-(double) y - 0.5)), new Vector2(x * 2f, 1f));
  }

  public void CreateWestCollider()
  {
    MeshRenderer component = this.GetComponent<MeshRenderer>();
    float x = component.bounds.extents.x;
    float y = component.bounds.extents.y;
    Transform transform1 = this.transform.Find("Collider West");
    if ((Object) transform1 != (Object) null)
      Object.DestroyImmediate((Object) transform1.gameObject);
    Transform transform2 = this.transform.Find("Collider West Top");
    if ((Object) transform2 != (Object) null)
      Object.DestroyImmediate((Object) transform2.gameObject);
    Transform transform3 = this.transform.Find("Collider West Bottom");
    if ((Object) transform3 != (Object) null)
      Object.DestroyImmediate((Object) transform3.gameObject);
    if (!this.WestCollider)
      return;
    if ((double) this.WestColliderSpacing > 0.0)
    {
      if (!this.TopLeft)
      {
        this.Position = component.bounds.center + new Vector3((float) (-(double) x - 0.5), (float) ((double) y / 2.0 + (double) this.WestColliderSpacing / 4.0 + (double) this.WestOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) ((double) y * 2.0 / 2.0 - (double) this.WestColliderSpacing / 2.0 - (double) this.WestOffset / 2.0));
      }
      else
      {
        this.Position = component.bounds.center + new Vector3((float) (-(double) x - 0.5), (float) ((double) y / 2.0 + (double) this.WestColliderSpacing / 4.0 + 0.5 + (double) this.WestOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) (((double) y * 2.0 + 2.0) / 2.0 - (double) this.WestColliderSpacing / 2.0 - (double) this.WestOffset / 2.0));
      }
      this.CreateCollider("Collider West Top", this.Position, (Vector2) this.Size);
      if (!this.BottomLeft)
      {
        this.Position = component.bounds.center + new Vector3((float) (-(double) x - 0.5), (float) (-((double) y / 2.0) - (double) this.WestColliderSpacing / 4.0 + (double) this.WestOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) ((double) y * 2.0 / 2.0 - (double) this.WestColliderSpacing / 2.0 + (double) this.WestOffset / 2.0));
      }
      else
      {
        this.Position = component.bounds.center + new Vector3((float) (-(double) x - 0.5), (float) (-((double) y / 2.0) - (double) this.WestColliderSpacing / 4.0 - 0.5 + (double) this.WestOffset / 4.0));
        this.Size = (Vector3) new Vector2(1f, (float) (((double) y * 2.0 + 2.0) / 2.0 - (double) this.WestColliderSpacing / 2.0 + (double) this.WestOffset / 2.0));
      }
      this.CreateCollider("Collider West Bottom", this.Position, (Vector2) this.Size);
    }
    else if (this.TopLeft && this.BottomLeft)
      this.CreateCollider("Collider West", component.bounds.center + new Vector3((float) (-(double) x - 0.5), 0.0f), new Vector2(1f, (float) ((double) y * 2.0 + 2.0)));
    else if (this.TopLeft)
      this.CreateCollider("Collider West", component.bounds.center + new Vector3((float) (-(double) x - 0.5), 0.5f), new Vector2(1f, (float) ((double) y * 2.0 + 1.0)));
    else if (this.BottomLeft)
      this.CreateCollider("Collider West", component.bounds.center + new Vector3((float) (-(double) x - 0.5), -0.5f), new Vector2(1f, (float) ((double) y * 2.0 + 1.0)));
    else
      this.CreateCollider("Collider West", component.bounds.center + new Vector3((float) (-(double) x - 0.5), 0.0f), new Vector2(1f, y * 2f));
  }

  public void CreateCollider(string Name, Vector3 Position, Vector2 Size)
  {
    GameObject gameObject = new GameObject();
    gameObject.transform.parent = this.transform;
    gameObject.name = Name;
    gameObject.transform.position = Position;
    gameObject.AddComponent<BoxCollider2D>().size = Size;
    gameObject.layer = LayerMask.NameToLayer("Island");
  }
}
