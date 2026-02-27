// Decompiled with JetBrains decompiler
// Type: PlayerDoctrineStone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class PlayerDoctrineStone : MonoBehaviour
{
  public SkeletonGraphic Spine;
  public CanvasGroup CanvasGroup;
  private const int TotalCount = 3;
  public static System.Action OnIncreaseCount;
  public static System.Action OnDecreaseCount;
  public static System.Action OnCachePosition;
  [SerializeField]
  private bool IsPlaying;
  private bool GivenAnswer;
  private bool firstChoice;
  public static PlayerDoctrineStone Instance;
  private TrackEntry t;
  private Sequence FadeOutSequence;

  [SerializeField]
  public int CompletedDoctrineStones
  {
    get => DataManager.Instance.CompletedDoctrineStones;
    set
    {
      int completedDoctrineStones = DataManager.Instance.CompletedDoctrineStones;
      DataManager.Instance.CompletedDoctrineStones = value;
      if (value > completedDoctrineStones)
      {
        System.Action onIncreaseCount = PlayerDoctrineStone.OnIncreaseCount;
        if (onIncreaseCount == null)
          return;
        onIncreaseCount();
      }
      else
      {
        System.Action onDecreaseCount = PlayerDoctrineStone.OnDecreaseCount;
        if (onDecreaseCount == null)
          return;
        onDecreaseCount();
      }
    }
  }

  [SerializeField]
  private int CurrentCount
  {
    get => DataManager.Instance.DoctrineCurrentCount;
    set => DataManager.Instance.DoctrineCurrentCount = value;
  }

  [SerializeField]
  private int TargetCount
  {
    get => DataManager.Instance.DoctrineTargetCount;
    set => DataManager.Instance.DoctrineTargetCount = value;
  }

  private void OnEnable() => PlayerDoctrineStone.Instance = this;

  private void HandleEvent(TrackEntry trackentry, Spine.Event e)
  {
    Debug.Log((object) e.Data.Name);
    switch (e.Data.Name)
    {
      case "CameraShake":
        CameraManager.instance.ShakeCameraForDuration(0.7f, 1f, 0.2f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        break;
      case "Break":
        AudioManager.Instance.PlayOneShot("event:/temple_key/become_whole", PlayerFarming.Instance.gameObject);
        AudioManager.Instance.PlayOneShot("event:/material/stone_break", PlayerFarming.Instance.gameObject);
        break;
      case "Shake":
        AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", PlayerFarming.Instance.gameObject);
        break;
      case "Close":
        AudioManager.Instance.PlayOneShot("event:/ui/close_menu", PlayerFarming.Instance.gameObject);
        break;
    }
  }

  private void OnDisable()
  {
    if (this.FadeOutSequence != null)
    {
      this.FadeOutSequence.Kill();
      this.FadeOutSequence = (Sequence) null;
    }
    if (!((UnityEngine.Object) this == (UnityEngine.Object) PlayerDoctrineStone.Instance))
      return;
    PlayerDoctrineStone.Instance = (PlayerDoctrineStone) null;
  }

  private void Start()
  {
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.Spine.AnimationState.SetAnimation(0, this.CurrentCount.ToString(), false);
    this.CanvasGroup.alpha = 0.0f;
    this.Spine.enabled = false;
  }

  private void OnDestroy()
  {
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    if (this.t == null)
      return;
    this.t.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.CompleteAnimation);
  }

  public static void Play(int Delta)
  {
    if ((UnityEngine.Object) PlayerDoctrineStone.Instance == (UnityEngine.Object) null)
      return;
    PlayerDoctrineStone.Instance.GivePiece(Delta);
  }

  public void GivePiece(int Delta)
  {
    this.TargetCount += Delta;
    if (this.TargetCount < 3)
      return;
    if ((UnityEngine.Object) PlayerCanvas.Instance != (UnityEngine.Object) null)
      PlayerCanvas.Instance.enabled = false;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FindDoctrineStone);
    System.Action onCachePosition = PlayerDoctrineStone.OnCachePosition;
    if (onCachePosition != null)
      onCachePosition();
    if (!DataManager.Instance.ShowFirstDoctrineStone)
      return;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CrownBone.gameObject, 7f);
  }

  private void Update()
  {
    if (this.IsPlaying || this.CurrentCount >= this.TargetCount)
      return;
    if (this.FadeOutSequence != null)
      this.FadeOutSequence.Kill();
    this.CanvasGroup.DOKill();
    this.CanvasGroup.alpha = 1f;
    this.Spine.enabled = true;
    this.IsPlaying = true;
    this.t = this.Spine.AnimationState.SetAnimation(0, (this.CurrentCount + 1).ToString() + "-activate", false);
    this.t.Complete += new Spine.AnimationState.TrackEntryDelegate(this.CompleteAnimation);
    this.Spine.AnimationState.AddAnimation(0, (this.CurrentCount + 1).ToString(), false, 0.0f);
    if (this.CurrentCount + 1 >= 3)
    {
      AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_last_piece");
      if (!DataManager.Instance.ShowFirstDoctrineStone)
        return;
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    }
    else
      AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_piece");
  }

  private void CompleteAnimation(TrackEntry trackentry)
  {
    this.t.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.CompleteAnimation);
    ++this.CurrentCount;
    if (this.CurrentCount >= 3)
      this.EndDeclareDoctrine();
    else
      this.EndPlayingAndFadeOut(false);
  }

  private void EndDeclareDoctrine()
  {
    if ((UnityEngine.Object) PlayerCanvas.Instance != (UnityEngine.Object) null)
      PlayerCanvas.Instance.enabled = true;
    if (DataManager.Instance.ShowFirstDoctrineStone)
      GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.ShowFirstDoctrineStone = false;
    this.CurrentCount = 0;
    this.TargetCount -= 3;
    this.EndPlayingAndFadeOut(true);
  }

  private void EndPlayingAndFadeOut(bool IncrementCount)
  {
    this.FadeOutSequence = DOTween.Sequence();
    if (IncrementCount)
    {
      this.FadeOutSequence.AppendInterval(0.25f);
      this.FadeOutSequence.AppendCallback((TweenCallback) (() =>
      {
        this.CanvasGroup.alpha = 0.0f;
        this.Spine.enabled = false;
        ++this.CompletedDoctrineStones;
      }));
      this.FadeOutSequence.AppendInterval(1f);
      this.FadeOutSequence.AppendCallback((TweenCallback) (() => this.IsPlaying = false));
    }
    else
      this.FadeOutSequence.AppendInterval(0.5f).OnComplete<Sequence>((TweenCallback) (() => this.IsPlaying = false));
    this.FadeOutSequence.Play<Sequence>();
  }
}
