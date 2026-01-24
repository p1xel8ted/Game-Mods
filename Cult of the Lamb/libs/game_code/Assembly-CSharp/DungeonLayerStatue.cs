// Decompiled with JetBrains decompiler
// Type: DungeonLayerStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using MMBiomeGeneration;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DungeonLayerStatue : BaseMonoBehaviour
{
  [SerializeField]
  public DungeonLayerStatue.Statue[] statues;
  [SerializeField]
  public GameObject[] layerNodes;
  public static bool ShownDungeonLayer;

  public void Start() => this.gameObject.SetActive(false);

  public void OnBiomeActive()
  {
    BiomeGenerator.OnRoomActive -= new BiomeGenerator.BiomeAction(this.OnBiomeActive);
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.ShowIE());
  }

  public IEnumerator ShowIE()
  {
    int index = GameManager.CurrentDungeonLayer - 1;
    this.layerNodes[index].SetActive(true);
    this.layerNodes[index].transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 1);
    DungeonLayerStatue.ShownDungeonLayer = true;
    yield return (object) new WaitForEndOfFrame();
  }

  [Serializable]
  public struct Statue
  {
    public GameObject[] LayerObjects;
  }
}
