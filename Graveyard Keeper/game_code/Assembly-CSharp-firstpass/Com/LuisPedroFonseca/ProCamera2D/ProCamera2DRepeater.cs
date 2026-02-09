// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DRepeater
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-repeater/")]
public class ProCamera2DRepeater : BasePC2D, IPostMover
{
  public static string ExtensionName = "Repeater";
  public Transform ObjectToRepeat;
  public Vector2 ObjectSize = new Vector2(2f, 2f);
  public Vector2 ObjectBottomLeft = Vector2.zero;
  public bool ObjectOnStage = true;
  public bool _repeatHorizontal = true;
  public bool _repeatVertical = true;
  public Camera CameraToUse;
  public Transform _cameraToUseTransform;
  public Vector3 _objStartPosition;
  public List<RepeatedObject> _allRepeatedObjects = new List<RepeatedObject>(2);
  public Queue<RepeatedObject> _inactiveRepeatedObjects = new Queue<RepeatedObject>();
  public IntPoint _prevStartIndex;
  public IntPoint _prevEndIndex;
  public Dictionary<IntPoint, bool> _occupiedIndices = new Dictionary<IntPoint, bool>();
  public int _pmOrder = 2000;

  public bool RepeatHorizontal
  {
    get => this._repeatHorizontal;
    set
    {
      this._repeatHorizontal = value;
      this.Refresh();
    }
  }

  public bool RepeatVertical
  {
    get => this._repeatVertical;
    set
    {
      this._repeatVertical = value;
      this.Refresh();
    }
  }

  public override void Awake()
  {
    base.Awake();
    if ((Object) this.ObjectToRepeat == (Object) null)
    {
      Debug.LogWarning((object) "ProCamera2D Repeater extension - No ObjectToRepeat defined!");
    }
    else
    {
      this._objStartPosition = new Vector3(this.Vector3H(this.ObjectToRepeat.position), this.Vector3V(this.ObjectToRepeat.position), this.Vector3D(this.ObjectToRepeat.position));
      this._cameraToUseTransform = this.CameraToUse.transform;
      if (this.ObjectOnStage)
        this.InitCopy(this.ObjectToRepeat);
      Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPostMover((IPostMover) this);
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePostMover((IPostMover) this);
  }

  public void PostMove(float deltaTime)
  {
    if (!this.enabled)
      return;
    Vector2 sizeInWorldCoords = Utils.GetScreenSizeInWorldCoords(this.CameraToUse, this.Vector3D(this.ProCamera2D.LocalPosition - this._objStartPosition));
    Vector3 position = this._cameraToUseTransform.position;
    Vector2 vector2_1 = new Vector2(this.Vector3H(position) - sizeInWorldCoords.x / 2f, this.Vector3V(position) - sizeInWorldCoords.y / 2f);
    Vector2 vector2_2 = new Vector2(vector2_1.x - this._objStartPosition.x - this.ObjectBottomLeft.x, vector2_1.y - this._objStartPosition.y - this.ObjectBottomLeft.y);
    IntPoint startIndex = new IntPoint(Mathf.FloorToInt(vector2_2.x / this.ObjectSize.x), Mathf.FloorToInt(vector2_2.y / this.ObjectSize.y));
    IntPoint intPoint = new IntPoint(Mathf.CeilToInt(sizeInWorldCoords.x / this.ObjectSize.x), Mathf.CeilToInt(sizeInWorldCoords.y / this.ObjectSize.y));
    IntPoint endIndex = new IntPoint(startIndex.X + intPoint.X, startIndex.Y + intPoint.Y);
    if (!startIndex.Equals(this._prevStartIndex) || !endIndex.Equals(this._prevEndIndex))
    {
      this.FreeOutOfRangeObjects(startIndex, endIndex);
      this.FillGrid(startIndex, endIndex);
    }
    this._prevStartIndex = startIndex;
    this._prevEndIndex = endIndex;
  }

  public int PMOrder
  {
    get => this._pmOrder;
    set => this._pmOrder = value;
  }

  public void FreeOutOfRangeObjects(IntPoint startIndex, IntPoint endIndex)
  {
    for (int index = 0; index < this._allRepeatedObjects.Count; ++index)
    {
      RepeatedObject allRepeatedObject = this._allRepeatedObjects[index];
      if (allRepeatedObject.GridPos.X != int.MaxValue && (allRepeatedObject.GridPos.X < startIndex.X || allRepeatedObject.GridPos.X > endIndex.X) || allRepeatedObject.GridPos.Y != int.MaxValue && (allRepeatedObject.GridPos.Y < startIndex.Y || allRepeatedObject.GridPos.Y > endIndex.Y))
      {
        this._occupiedIndices.Remove(allRepeatedObject.GridPos);
        this._inactiveRepeatedObjects.Enqueue(allRepeatedObject);
        this.PositionObject(allRepeatedObject, IntPoint.MaxValue);
      }
    }
  }

  public void FillGrid(IntPoint startIndex, IntPoint endIndex)
  {
    if (!this._repeatHorizontal)
    {
      startIndex.X = 0;
      endIndex.X = 0;
    }
    if (!this._repeatVertical)
    {
      startIndex.Y = 0;
      endIndex.Y = 0;
    }
    for (int x = startIndex.X; x <= endIndex.X; ++x)
    {
      for (int y = startIndex.Y; y <= endIndex.Y; ++y)
      {
        IntPoint intPoint = new IntPoint(x, y);
        bool flag = false;
        if (!this._occupiedIndices.TryGetValue(intPoint, out flag))
        {
          if (this._inactiveRepeatedObjects.Count == 0)
            this.InitCopy(Object.Instantiate<Transform>(this.ObjectToRepeat), false);
          this._occupiedIndices[intPoint] = true;
          this.PositionObject(this._inactiveRepeatedObjects.Dequeue(), intPoint);
        }
      }
    }
  }

  public void InitCopy(Transform newCopy, bool positionOffscreen = true)
  {
    RepeatedObject repeatedObject = new RepeatedObject()
    {
      Transform = newCopy
    };
    repeatedObject.Transform.parent = this.ObjectToRepeat.parent;
    this._allRepeatedObjects.Add(repeatedObject);
    this._inactiveRepeatedObjects.Enqueue(repeatedObject);
    if (!positionOffscreen)
      return;
    this.PositionObject(repeatedObject, IntPoint.MaxValue);
  }

  public void PositionObject(RepeatedObject obj, IntPoint index)
  {
    obj.GridPos = index;
    obj.Transform.position = this.VectorHVD(this._objStartPosition.x + (float) index.X * this.ObjectSize.x, this._objStartPosition.y + (float) index.Y * this.ObjectSize.y, this._objStartPosition.z);
  }

  public void Refresh()
  {
    this.FreeOutOfRangeObjects(IntPoint.MaxValue, IntPoint.MaxValue);
    this.FillGrid(this._prevStartIndex, this._prevEndIndex);
  }
}
