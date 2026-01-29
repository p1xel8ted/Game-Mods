// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDLCMapKeysWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIDLCMapKeysWidget : BaseMonoBehaviour
{
  [SerializeField]
  public KeyWidget keyOnePrefab;
  [SerializeField]
  public KeyWidget keyTwoPrefab;
  [SerializeField]
  public KeyWidget keyThreePrefab;
  [SerializeField]
  public Transform keyContainer;
  [SerializeField]
  public Image keyDecorationImage;
  public List<KeyWidget> keyInstances = new List<KeyWidget>();

  public void Start() => this.Init();

  public void Init()
  {
    this.ClearKeys();
    int dlcKey1 = DataManager.Instance.DLCKey_1;
    int dlcKey2 = DataManager.Instance.DLCKey_2;
    int dlcKey3 = DataManager.Instance.DLCKey_3;
    this.keyDecorationImage.enabled = false;
    for (int index = 0; index < dlcKey1; ++index)
      this.AddKey(DLCKeyType.KeyOne);
    for (int index = 0; index < dlcKey2; ++index)
      this.AddKey(DLCKeyType.KeyTwo);
    for (int index = 0; index < dlcKey3; ++index)
      this.AddKey(DLCKeyType.KeyThree);
  }

  public void ClearKeys()
  {
    foreach (Component keyInstance in this.keyInstances)
      UnityEngine.Object.Destroy((UnityEngine.Object) keyInstance.gameObject);
    this.keyInstances.Clear();
    this.keyDecorationImage.enabled = false;
  }

  public void RemoveKey(DLCKeyType keyType)
  {
    for (int index = this.keyInstances.Count - 1; index >= 0; --index)
    {
      if (this.keyInstances[index].KeyType == keyType)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.keyInstances[index].gameObject);
        this.keyInstances.RemoveAt(index);
        break;
      }
    }
    this.keyDecorationImage.enabled = this.keyInstances.Count > 0;
  }

  public void AddKey(DLCKeyType keyType)
  {
    this.keyDecorationImage.enabled = true;
    switch (keyType)
    {
      case DLCKeyType.KeyOne:
        KeyWidget keyWidget1 = UnityEngine.Object.Instantiate<KeyWidget>(this.keyOnePrefab, this.keyContainer);
        keyWidget1.transform.SetAsLastSibling();
        this.keyInstances.Add(keyWidget1);
        break;
      case DLCKeyType.KeyTwo:
        KeyWidget keyWidget2 = UnityEngine.Object.Instantiate<KeyWidget>(this.keyTwoPrefab, this.keyContainer);
        keyWidget2.transform.SetAsLastSibling();
        this.keyInstances.Add(keyWidget2);
        break;
      case DLCKeyType.KeyThree:
        KeyWidget keyWidget3 = UnityEngine.Object.Instantiate<KeyWidget>(this.keyThreePrefab, this.keyContainer);
        keyWidget3.transform.SetAsLastSibling();
        this.keyInstances.Add(keyWidget3);
        break;
    }
    this.SortKeyInstances();
  }

  public async System.Threading.Tasks.Task HandleAddKey(DungeonWorldMapIcon location)
  {
    DungeonWorldMapIcon.NodeType type = location.Type;
    DLCKeyType dlcKeyType;
    switch (type)
    {
      case DungeonWorldMapIcon.NodeType.Key:
        dlcKeyType = DLCKeyType.KeyOne;
        break;
      case DungeonWorldMapIcon.NodeType.Key_2:
        dlcKeyType = DLCKeyType.KeyTwo;
        break;
      case DungeonWorldMapIcon.NodeType.Key_3:
        dlcKeyType = DLCKeyType.KeyThree;
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
        break;
    }
    DLCKeyType keyType = dlcKeyType;
    Vector3 position = location.transform.position;
    Vector3 positionOfLastKey = this.GetPositionOfLastKey(keyType);
    this.IncrementDataManagerKeyCount(keyType);
    await this.HandleKeyAnimationAsync(keyType, position, positionOfLastKey);
    this.AddKey(keyType);
  }

  public async System.Threading.Tasks.Task HandleRemoveKey(DungeonWorldMapIcon location)
  {
    DungeonWorldMapIcon.NodeType type = location.Type;
    DLCKeyType dlcKeyType1;
    switch (type)
    {
      case DungeonWorldMapIcon.NodeType.Lock:
        dlcKeyType1 = DLCKeyType.KeyOne;
        break;
      case DungeonWorldMapIcon.NodeType.Lock_2:
        dlcKeyType1 = DLCKeyType.KeyTwo;
        break;
      case DungeonWorldMapIcon.NodeType.Lock_3:
        dlcKeyType1 = DLCKeyType.KeyThree;
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
        break;
    }
    DLCKeyType dlcKeyType2 = dlcKeyType1;
    Vector3 position = location.transform.position;
    Vector3 positionOfLastKey = this.GetPositionOfLastKey(dlcKeyType2);
    this.DecrementDataManagerKeyCount(dlcKeyType2);
    this.RemoveKey(dlcKeyType2);
    await this.HandleKeyAnimationAsync(dlcKeyType2, positionOfLastKey, position);
  }

  public async System.Threading.Tasks.Task HandleKeyAnimationAsync(
    DLCKeyType type,
    Vector3 from,
    Vector3 to)
  {
    UIDLCMapKeysWidget uidlcMapKeysWidget = this;
    KeyWidget original;
    switch (type)
    {
      case DLCKeyType.KeyOne:
        original = uidlcMapKeysWidget.keyOnePrefab;
        break;
      case DLCKeyType.KeyTwo:
        original = uidlcMapKeysWidget.keyTwoPrefab;
        break;
      case DLCKeyType.KeyThree:
        original = uidlcMapKeysWidget.keyThreePrefab;
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
        break;
    }
    KeyWidget tempKey = UnityEngine.Object.Instantiate<KeyWidget>(original, uidlcMapKeysWidget.transform);
    await tempKey.transform.DOMove(to, 2f).From<Vector3, Vector3, VectorOptions>(from).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCubic).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).AsyncWaitForCompletion();
    UnityEngine.Object.Destroy((UnityEngine.Object) tempKey.gameObject);
    tempKey = (KeyWidget) null;
  }

  public Vector3 GetPositionOfLastKey(DLCKeyType type)
  {
    for (int index = this.keyInstances.Count - 1; index >= 0; --index)
    {
      if (this.keyInstances[index].KeyType == type)
        return this.keyInstances[index].transform.position;
    }
    return this.transform.GetChild(this.transform.childCount - 1).position;
  }

  public void IncrementDataManagerKeyCount(DLCKeyType type) => this.AddDataManagerKeyCount(type, 1);

  public void DecrementDataManagerKeyCount(DLCKeyType type)
  {
    this.AddDataManagerKeyCount(type, -1);
  }

  public void AddDataManagerKeyCount(DLCKeyType type, int amount)
  {
    switch (type)
    {
      case DLCKeyType.KeyOne:
        DataManager.Instance.DLCKey_1 += amount;
        break;
      case DLCKeyType.KeyTwo:
        DataManager.Instance.DLCKey_2 += amount;
        break;
      case DLCKeyType.KeyThree:
        DataManager.Instance.DLCKey_3 += amount;
        break;
    }
  }

  public void SortKeyInstances()
  {
    this.keyInstances.Sort((Comparison<KeyWidget>) ((a, b) => a.KeyType.CompareTo((object) b.KeyType)));
    for (int index = 0; index < this.keyInstances.Count; ++index)
      this.keyInstances[index].transform.SetSiblingIndex(index + 1);
  }
}
