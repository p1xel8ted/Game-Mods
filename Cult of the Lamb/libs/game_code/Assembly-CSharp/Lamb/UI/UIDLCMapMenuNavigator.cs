// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDLCMapMenuNavigator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIDLCMapMenuNavigator : BaseMonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float deadzone = 0.55f;
  [SerializeField]
  [Range(1f, 180f)]
  public float angleCone = 70f;
  [SerializeField]
  public float initialDelay = 0.28f;
  [SerializeField]
  public float repeatDelay = 0.12f;
  [SerializeField]
  [Range(0.0f, 30f)]
  public float sectorHysteresisDeg = 12f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float axisSnapThreshold = 0.35f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float minScoreToSelect = 0.15f;
  [SerializeField]
  public float distanceNormalize = 800f;
  [SerializeField]
  [Range(0.0f, 2f)]
  public float perpPenalty = 0.35f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float distancePenalty = 0.1f;
  [SerializeField]
  public float nextMoveAllowedAt;
  [SerializeField]
  public int lastSector = -999;
  [SerializeField]
  public bool hadInput;
  public UIDLCMapMenuController _menuController;
  public Camera _uiCamera;
  public const int UnsetSector = -999;
  public const float SectorStepDeg = 45f;

  public void Awake() => this._menuController = this.GetComponent<UIDLCMapMenuController>();

  public void OnEnable()
  {
    this._uiCamera = !(bool) (UnityEngine.Object) this._menuController || !(bool) (UnityEngine.Object) this._menuController.Canvas ? (Camera) null : this._menuController.Canvas.worldCamera;
    this.hadInput = false;
    this.lastSector = -999;
    this.nextMoveAllowedAt = 0.0f;
  }

  public void Update() => this.NavigateToSurroundingNodes();

  public void NavigateToSurroundingNodes()
  {
    Vector2 vector2_1 = new Vector2(InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), InputManager.UI.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
    if ((double) vector2_1.magnitude < (double) this.deadzone)
    {
      this.hadInput = false;
      this.lastSector = -999;
    }
    else
    {
      Vector2 normalized1 = new Vector2((double) Mathf.Abs(vector2_1.x) > (double) this.axisSnapThreshold ? Mathf.Sign(vector2_1.x) : 0.0f, (double) Mathf.Abs(vector2_1.y) > (double) this.axisSnapThreshold ? Mathf.Sign(vector2_1.y) : 0.0f).normalized;
      if (normalized1 == Vector2.zero)
        return;
      float unscaledTime = Time.unscaledTime;
      if ((double) unscaledTime < (double) this.nextMoveAllowedAt)
        return;
      int sector = this.GetSector(normalized1);
      bool flag = this.SectorChanged(normalized1);
      this.nextMoveAllowedAt = unscaledTime + (flag || !this.hadInput ? this.initialDelay : this.repeatDelay);
      this.lastSector = sector;
      this.hadInput = true;
      Button currentSelectable = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable as Button;
      DungeonWorldMapIcon component = (UnityEngine.Object) currentSelectable != (UnityEngine.Object) null ? currentSelectable.GetComponent<DungeonWorldMapIcon>() : (DungeonWorldMapIcon) null;
      if (!(bool) (UnityEngine.Object) component)
        return;
      List<DungeonWorldMapIcon> outList;
      using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out outList))
      {
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
          return;
        UIDLCMapMenuNavigator.GatherNavigableNeighbours(component, outList);
        if (outList.Count == 0)
          return;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(this._uiCamera, component.RectTransform.position);
        DungeonWorldMapIcon dungeonWorldMapIcon = (DungeonWorldMapIcon) null;
        float num1 = float.NegativeInfinity;
        foreach (DungeonWorldMapIcon n in outList)
        {
          if (UIDLCMapMenuNavigator.IsValidCandidate(n))
          {
            Vector2 vector2_2 = RectTransformUtility.WorldToScreenPoint(this._uiCamera, n.RectTransform.position) - screenPoint;
            if ((double) vector2_2.sqrMagnitude >= 9.9999997473787516E-05)
            {
              Vector2 normalized2 = vector2_2.normalized;
              if ((double) Vector2.Angle(normalized1, normalized2) <= (double) this.angleCone)
              {
                double num2 = (double) Vector2.Dot(normalized1, normalized2);
                float num3 = Mathf.Abs(Vector2.Dot(new Vector2(-normalized1.y, normalized1.x), normalized2));
                float num4 = Mathf.Clamp01(vector2_2.magnitude / this.distanceNormalize);
                double num5 = (double) this.perpPenalty * (double) num3;
                float num6 = (float) (num2 - num5 - (double) this.distancePenalty * (double) num4);
                if ((double) num6 > (double) num1)
                {
                  num1 = num6;
                  dungeonWorldMapIcon = n;
                }
              }
            }
          }
        }
        if (dungeonWorldMapIcon == null || (double) num1 <= (double) this.minScoreToSelect)
          return;
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) dungeonWorldMapIcon.Button);
      }
    }
  }

  public bool SectorChanged(Vector2 currentDir)
  {
    if (this.lastSector == -999)
      return true;
    float f = (float) ((double) this.lastSector * (Math.PI / 180.0) * 45.0);
    return !this.InSameSectorWithHysteresis(new Vector2(Mathf.Round(Mathf.Cos(f)), Mathf.Round(Mathf.Sin(f))).normalized, currentDir);
  }

  public static bool IsValidCandidate(DungeonWorldMapIcon n)
  {
    return (UnityEngine.Object) n != (UnityEngine.Object) null && (UnityEngine.Object) n.Button != (UnityEngine.Object) null && n.Button.interactable && n.isActiveAndEnabled;
  }

  public int GetSector(Vector2 v)
  {
    float num = Mathf.Atan2(v.y, v.x) * 57.29578f;
    if ((double) num < 0.0)
      num += 360f;
    return Mathf.RoundToInt(num / 45f) % 8;
  }

  public bool InSameSectorWithHysteresis(Vector2 prevDir, Vector2 curDir)
  {
    return (double) Vector2.Angle(prevDir, curDir) < (double) this.sectorHysteresisDeg;
  }

  public static void GatherNavigableNeighbours(
    DungeonWorldMapIcon from,
    List<DungeonWorldMapIcon> outList)
  {
    outList.Clear();
    List<DungeonWorldMapIcon> neighbours;
    using (CollectionPool<List<DungeonWorldMapIcon>, DungeonWorldMapIcon>.Get(out neighbours))
    {
      Dictionary<int, byte> visited;
      using (CollectionPool<Dictionary<int, byte>, KeyValuePair<int, byte>>.Get(out visited))
      {
        from.GatherActiveNeighbours(neighbours);
        foreach (DungeonWorldMapIcon node in neighbours)
          UIDLCMapMenuNavigator.ExpandThroughLocks(node, from.ID, outList, visited);
        for (int index = outList.Count - 1; index >= 0; --index)
        {
          if (outList[index] == null || outList[index].ID == from.ID)
            outList.RemoveAt(index);
        }
      }
    }
  }

  public static void ExpandThroughLocks(
    DungeonWorldMapIcon node,
    int fromId,
    List<DungeonWorldMapIcon> outList,
    Dictionary<int, byte> visited)
  {
    if ((UnityEngine.Object) node == (UnityEngine.Object) null || !node.isActiveAndEnabled || !visited.TryAdd(node.ID, (byte) 1))
      return;
    if (node.IsLock)
    {
      foreach (DungeonWorldMapIcon childNode in node.ChildNodes)
      {
        if (!((UnityEngine.Object) childNode == (UnityEngine.Object) null) && childNode.isActiveAndEnabled)
          UIDLCMapMenuNavigator.ExpandThroughLocks(childNode, fromId, outList, visited);
      }
      DungeonWorldMapIcon parent = node.Parent;
      if (!((UnityEngine.Object) parent != (UnityEngine.Object) null) || !parent.isActiveAndEnabled)
        return;
      UIDLCMapMenuNavigator.ExpandThroughLocks(parent, fromId, outList, visited);
    }
    else
    {
      if (node.ID == fromId)
        return;
      outList.Add(node);
    }
  }
}
