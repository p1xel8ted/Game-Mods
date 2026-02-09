// Decompiled with JetBrains decompiler
// Type: UIGrid
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
  public UIGrid.Arrangement arrangement;
  public UIGrid.Sorting sorting;
  public UIWidget.Pivot pivot;
  public int maxPerLine;
  public float cellWidth = 200f;
  public float cellHeight = 200f;
  public bool animateSmoothly;
  public bool hideInactive;
  public bool keepWithinPanel;
  public UIGrid.OnReposition onReposition;
  public Comparison<Transform> onCustomSort;
  [HideInInspector]
  [SerializeField]
  public bool sorted;
  public bool mReposition;
  public UIPanel mPanel;
  public bool mInitDone;

  public bool repositionNow
  {
    set
    {
      if (!value)
        return;
      this.mReposition = true;
      this.enabled = true;
    }
  }

  public List<Transform> GetChildList()
  {
    Transform transform = this.transform;
    List<Transform> list = new List<Transform>();
    for (int index = 0; index < transform.childCount; ++index)
    {
      Transform child = transform.GetChild(index);
      if (!this.hideInactive || (bool) (UnityEngine.Object) child && child.gameObject.activeSelf)
        list.Add(child);
    }
    if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
    {
      if (this.sorting == UIGrid.Sorting.Alphabetic)
        list.Sort(new Comparison<Transform>(UIGrid.SortByName));
      else if (this.sorting == UIGrid.Sorting.Horizontal)
        list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
      else if (this.sorting == UIGrid.Sorting.Vertical)
        list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
      else if (this.onCustomSort != null)
        list.Sort(this.onCustomSort);
      else
        this.Sort(list);
    }
    return list;
  }

  public Transform GetChild(int index)
  {
    List<Transform> childList = this.GetChildList();
    return index >= childList.Count ? (Transform) null : childList[index];
  }

  public int GetIndex(Transform trans) => this.GetChildList().IndexOf(trans);

  [Obsolete("Use gameObject.AddChild or transform.parent = gridTransform")]
  public void AddChild(Transform trans)
  {
    if (!((UnityEngine.Object) trans != (UnityEngine.Object) null))
      return;
    trans.parent = this.transform;
    this.ResetPosition(this.GetChildList());
  }

  [Obsolete("Use gameObject.AddChild or transform.parent = gridTransform")]
  public void AddChild(Transform trans, bool sort)
  {
    if (!((UnityEngine.Object) trans != (UnityEngine.Object) null))
      return;
    trans.parent = this.transform;
    this.ResetPosition(this.GetChildList());
  }

  public bool RemoveChild(Transform t)
  {
    List<Transform> childList = this.GetChildList();
    if (!childList.Remove(t))
      return false;
    this.ResetPosition(childList);
    return true;
  }

  public virtual void Init()
  {
    this.mInitDone = true;
    this.mPanel = NGUITools.FindInParents<UIPanel>(this.gameObject);
  }

  public virtual void Start()
  {
    if (!this.mInitDone)
      this.Init();
    bool animateSmoothly = this.animateSmoothly;
    this.animateSmoothly = false;
    this.Reposition();
    this.animateSmoothly = animateSmoothly;
    this.enabled = false;
  }

  public virtual void Update()
  {
    this.Reposition();
    this.enabled = false;
  }

  public void OnValidate()
  {
    if (Application.isPlaying || !NGUITools.GetActive((Behaviour) this))
      return;
    this.Reposition();
  }

  public static int SortByName(Transform a, Transform b) => string.Compare(a.name, b.name);

  public static int SortHorizontal(Transform a, Transform b)
  {
    return a.localPosition.x.CompareTo(b.localPosition.x);
  }

  public static int SortVertical(Transform a, Transform b)
  {
    return b.localPosition.y.CompareTo(a.localPosition.y);
  }

  public virtual void Sort(List<Transform> list)
  {
  }

  [ContextMenu("Execute")]
  public virtual void Reposition()
  {
    if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this.gameObject))
      this.Init();
    if (this.sorted)
    {
      this.sorted = false;
      if (this.sorting == UIGrid.Sorting.None)
        this.sorting = UIGrid.Sorting.Alphabetic;
      NGUITools.SetDirty((UnityEngine.Object) this);
    }
    this.ResetPosition(this.GetChildList());
    if (this.keepWithinPanel)
      this.ConstrainWithinPanel();
    if (this.onReposition == null)
      return;
    this.onReposition();
  }

  public void ConstrainWithinPanel()
  {
    if (!((UnityEngine.Object) this.mPanel != (UnityEngine.Object) null))
      return;
    this.mPanel.ConstrainTargetToBounds(this.transform, true);
    UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.UpdateScrollbars(true);
  }

  public virtual void ResetPosition(List<Transform> list)
  {
    this.mReposition = false;
    int b1 = 0;
    int b2 = 0;
    int a1 = 0;
    int a2 = 0;
    Transform transform1 = this.transform;
    int index1 = 0;
    for (int count = list.Count; index1 < count; ++index1)
    {
      Transform transform2 = list[index1];
      Vector3 pos = transform2.localPosition;
      float z = pos.z;
      if (this.arrangement == UIGrid.Arrangement.CellSnap)
      {
        if ((double) this.cellWidth > 0.0)
          pos.x = Mathf.Round(pos.x / this.cellWidth) * this.cellWidth;
        if ((double) this.cellHeight > 0.0)
          pos.y = Mathf.Round(pos.y / this.cellHeight) * this.cellHeight;
      }
      else
        pos = this.arrangement == UIGrid.Arrangement.Horizontal ? new Vector3(this.cellWidth * (float) b1, -this.cellHeight * (float) b2, z) : new Vector3(this.cellWidth * (float) b2, -this.cellHeight * (float) b1, z);
      if (this.animateSmoothly && Application.isPlaying && (double) Vector3.SqrMagnitude(transform2.localPosition - pos) >= 9.9999997473787516E-05)
      {
        SpringPosition springPosition = SpringPosition.Begin(transform2.gameObject, pos, 15f);
        springPosition.updateScrollView = true;
        springPosition.ignoreTimeScale = true;
      }
      else
        transform2.localPosition = pos;
      a1 = Mathf.Max(a1, b1);
      a2 = Mathf.Max(a2, b2);
      if (++b1 >= this.maxPerLine && this.maxPerLine > 0)
      {
        b1 = 0;
        ++b2;
      }
    }
    if (this.pivot == UIWidget.Pivot.TopLeft)
      return;
    Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
    float num1;
    float num2;
    if (this.arrangement == UIGrid.Arrangement.Horizontal)
    {
      num1 = Mathf.Lerp(0.0f, (float) a1 * this.cellWidth, pivotOffset.x);
      num2 = Mathf.Lerp((float) -a2 * this.cellHeight, 0.0f, pivotOffset.y);
    }
    else
    {
      num1 = Mathf.Lerp(0.0f, (float) a2 * this.cellWidth, pivotOffset.x);
      num2 = Mathf.Lerp((float) -a1 * this.cellHeight, 0.0f, pivotOffset.y);
    }
    for (int index2 = 0; index2 < transform1.childCount; ++index2)
    {
      Transform child = transform1.GetChild(index2);
      SpringPosition component = child.GetComponent<SpringPosition>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.enabled = false;
        component.target.x -= num1;
        component.target.y -= num2;
        component.enabled = true;
      }
      else
      {
        Vector3 localPosition = child.localPosition;
        localPosition.x -= num1;
        localPosition.y -= num2;
        child.localPosition = localPosition;
      }
    }
  }

  public delegate void OnReposition();

  public enum Arrangement
  {
    Horizontal,
    Vertical,
    CellSnap,
  }

  public enum Sorting
  {
    None,
    Alphabetic,
    Horizontal,
    Vertical,
    Custom,
  }
}
