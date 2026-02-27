// Decompiled with JetBrains decompiler
// Type: HUDDoctrineStoneCount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.UI.Overlays.TutorialOverlay;
using TMPro;
using UnityEngine;

#nullable disable
public class HUDDoctrineStoneCount : MonoBehaviour
{
  public static HUDDoctrineStoneCount Instance;
  public TextMeshProUGUI IconText;
  public TextMeshProUGUI CountText;
  public Transform Container;
  public TextMeshProUGUI IconToMove;
  private int previousCount;
  private Vector3 TargetPositon;

  private void OnEnable()
  {
    this.UpdateCount();
    PlayerDoctrineStone.OnIncreaseCount += new System.Action(this.Fly);
    PlayerDoctrineStone.OnDecreaseCount += new System.Action(this.UpdateCount);
    PlayerDoctrineStone.OnCachePosition += new System.Action(this.CachePosition);
    HUDDoctrineStoneCount.Instance = this;
    this.previousCount = DataManager.Instance.CompletedDoctrineStones;
  }

  private void OnDisable()
  {
    PlayerDoctrineStone.OnIncreaseCount -= new System.Action(this.Fly);
    PlayerDoctrineStone.OnDecreaseCount -= new System.Action(this.UpdateCount);
    PlayerDoctrineStone.OnCachePosition -= new System.Action(this.CachePosition);
    if (!((UnityEngine.Object) HUDDoctrineStoneCount.Instance == (UnityEngine.Object) this))
      return;
    HUDDoctrineStoneCount.Instance = (HUDDoctrineStoneCount) null;
  }

  private void CachePosition() => this.TargetPositon = this.IconText.transform.position;

  private void Fly()
  {
    this.IconToMove.text = "<sprite name=\"icon_DoctrineStone\">";
    this.IconToMove.transform.localScale = Vector3.one * 2f;
    this.IconToMove.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    this.IconToMove.transform.position = Camera.main.WorldToScreenPoint(PlayerDoctrineStone.Instance.gameObject.transform.position);
    this.IconToMove.transform.DOMove(this.TargetPositon, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.CommandmentStone))
      {
        RoomLockController.RoomCompleted();
        UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.CommandmentStone);
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/DeclareDoctrine", Objectives.CustomQuestTypes.DeclareDoctrine), true));
      }
      this.UpdateCount();
    }));
  }

  private void UpdateCount()
  {
    this.IconToMove.text = "";
    if (DataManager.Instance.CompletedDoctrineStones <= 0)
    {
      this.CountText.text = "";
      this.IconText.text = "";
    }
    else
    {
      this.IconText.text = "<sprite name=\"icon_DoctrineStone\">";
      this.CountText.text = DataManager.Instance.CompletedDoctrineStones.ToString();
      if (this.previousCount != DataManager.Instance.CompletedDoctrineStones)
      {
        this.CountText.transform.DOKill();
        CameraManager.instance?.ShakeCameraForDuration(0.7f, 0.8f, 0.2f);
        this.CountText.transform.localScale = Vector3.one * 1.2f;
        this.CountText.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      }
      this.previousCount = DataManager.Instance.CompletedDoctrineStones;
    }
  }
}
