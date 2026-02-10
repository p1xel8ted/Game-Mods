// Decompiled with JetBrains decompiler
// Type: HUDDoctrineStoneCount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.UI.Overlays.TutorialOverlay;
using System.Runtime.CompilerServices;
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
  [SerializeField]
  public CanvasGroup _canvasGroup;
  public int previousCount;
  public Vector3 TargetPositon;

  public void OnEnable()
  {
    this.UpdateCount();
    PlayerDoctrineStone.OnIncreaseCount += new System.Action(this.Fly);
    PlayerDoctrineStone.OnDecreaseCount += new System.Action(this.UpdateCount);
    PlayerDoctrineStone.OnCachePosition += new System.Action(this.CachePosition);
    HUDDoctrineStoneCount.Instance = this;
    this.previousCount = DataManager.Instance.CompletedDoctrineStones;
    if (!GameManager.IsDungeon(PlayerFarming.Location))
      return;
    this._canvasGroup.alpha = 0.0f;
  }

  public void OnDisable()
  {
    PlayerDoctrineStone.OnIncreaseCount -= new System.Action(this.Fly);
    PlayerDoctrineStone.OnDecreaseCount -= new System.Action(this.UpdateCount);
    PlayerDoctrineStone.OnCachePosition -= new System.Action(this.CachePosition);
    if (!((UnityEngine.Object) HUDDoctrineStoneCount.Instance == (UnityEngine.Object) this))
      return;
    HUDDoctrineStoneCount.Instance = (HUDDoctrineStoneCount) null;
  }

  public void CachePosition() => this.TargetPositon = this.IconText.transform.position;

  public void Fly()
  {
    this._canvasGroup.alpha = 1f;
    if (DataManager.Instance.InitialDoctrineStone)
    {
      RoomLockController.RoomCompleted();
      DataManager.Instance.FirstDoctrineStone = true;
      DataManager.Instance.InitialDoctrineStone = false;
      this.IconToMove.text = "<sprite name=\"icon_DoctrineStone\">";
      this.IconToMove.transform.localScale = Vector3.one * 2f;
      this.IconToMove.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
      this.IconToMove.transform.position = Camera.main.WorldToScreenPoint(PlayerFarming.Instance.PlayerDoctrineStone.gameObject.transform.position);
      this.IconToMove.transform.DOMove(this.TargetPositon, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.CommandmentStone))
        {
          UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.CommandmentStone);
          overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
          {
            ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/DeclareDoctrine", Objectives.CustomQuestTypes.DeclareDoctrine), true);
            this._canvasGroup.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<float, float, FloatOptions>>(1f);
          });
        }
        this.UpdateCount();
      }));
    }
    else
      this.UpdateCount();
  }

  public void UpdateCount()
  {
    this._canvasGroup.alpha = 1f;
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

  [CompilerGenerated]
  public void \u003CFly\u003Eb__11_0()
  {
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.CommandmentStone))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.CommandmentStone);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/DeclareDoctrine", Objectives.CustomQuestTypes.DeclareDoctrine), true);
        this._canvasGroup.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<float, float, FloatOptions>>(1f);
      });
    }
    this.UpdateCount();
  }

  [CompilerGenerated]
  public void \u003CFly\u003Eb__11_1()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/DeclareDoctrine", Objectives.CustomQuestTypes.DeclareDoctrine), true);
    this._canvasGroup.DOFade(0.0f, 0.5f).SetDelay<TweenerCore<float, float, FloatOptions>>(1f);
  }
}
