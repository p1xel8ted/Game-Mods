// Decompiled with JetBrains decompiler
// Type: DungeonLayerStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using MMBiomeGeneration;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DungeonLayerStatue : BaseMonoBehaviour
{
  [SerializeField]
  private DungeonLayerStatue.Statue[] statues;
  [SerializeField]
  private GameObject[] layerNodes;
  public static bool ShownDungeonLayer;

  private void Start() => this.gameObject.SetActive(false);

  private void OnBiomeActive()
  {
    BiomeGenerator.OnRoomActive -= new BiomeGenerator.BiomeAction(this.OnBiomeActive);
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.ShowIE());
  }

  private IEnumerator ShowIE()
  {
    int index = GameManager.CurrentDungeonLayer - 1;
    this.layerNodes[index].SetActive(true);
    this.layerNodes[index].transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 1);
    DungeonLayerStatue.ShownDungeonLayer = true;
    yield return (object) new WaitForEndOfFrame();
  }

  [Serializable]
  private struct Statue
  {
    public GameObject[] LayerObjects;
  }
}
