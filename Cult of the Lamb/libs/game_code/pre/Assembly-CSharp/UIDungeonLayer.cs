// Decompiled with JetBrains decompiler
// Type: UIDungeonLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDungeonLayer : BaseMonoBehaviour
{
  public static UIDungeonLayer Instance;
  [SerializeField]
  private CanvasGroup canvasGroup;
  [SerializeField]
  private Image leaderIcon;
  [SerializeField]
  private Sprite[] leaderIcons;
  [SerializeField]
  private UIDungeonLayerNode[] nodes;

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

  private IEnumerator ShowIE(float duration)
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
