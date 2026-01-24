// Decompiled with JetBrains decompiler
// Type: UIDungeonLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDungeonLayer : BaseMonoBehaviour
{
  public static UIDungeonLayer Instance;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public Image leaderIcon;
  [SerializeField]
  public Sprite[] leaderIcons;
  [SerializeField]
  public UIDungeonLayerNode[] nodes;

  public static void Play(int currentLayer, float duration, FollowerLocation location)
  {
    if ((Object) UIDungeonLayer.Instance == (Object) null)
      UIDungeonLayer.Instance = Object.Instantiate<GameObject>(UnityEngine.Resources.Load("Prefabs/UI/UI Dungeon Layer") as GameObject, GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<UIDungeonLayer>();
    if (location == FollowerLocation.Dungeon1_1)
      UIDungeonLayer.Instance.leaderIcon.sprite = UIDungeonLayer.Instance.leaderIcons[0];
    if (location == FollowerLocation.Dungeon1_2)
      UIDungeonLayer.Instance.leaderIcon.sprite = UIDungeonLayer.Instance.leaderIcons[1];
    if (location == FollowerLocation.Dungeon1_3)
      UIDungeonLayer.Instance.leaderIcon.sprite = UIDungeonLayer.Instance.leaderIcons[2];
    if (location == FollowerLocation.Dungeon1_4)
      UIDungeonLayer.Instance.leaderIcon.sprite = UIDungeonLayer.Instance.leaderIcons[3];
    for (int index = 0; index < UIDungeonLayer.Instance.nodes.Length; ++index)
    {
      if (index < currentLayer - 1)
        UIDungeonLayer.Instance.nodes[index].SetState(UIDungeonLayerNode.State.Visted);
      else if (index == currentLayer - 1)
        UIDungeonLayer.Instance.nodes[index].SetState(UIDungeonLayerNode.State.Selected);
    }
    UIDungeonLayer.Instance.StartCoroutine((IEnumerator) UIDungeonLayer.Instance.ShowIE(duration));
  }

  public IEnumerator ShowIE(float duration)
  {
    this.canvasGroup.alpha = 0.0f;
    this.canvasGroup.DOFade(1f, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(duration);
    this.canvasGroup.DOFade(0.0f, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    UIDungeonLayer.Hide();
  }

  public static void Hide()
  {
    if (!(bool) (Object) UIDungeonLayer.Instance)
      return;
    Object.Destroy((Object) UIDungeonLayer.Instance.gameObject);
  }
}
