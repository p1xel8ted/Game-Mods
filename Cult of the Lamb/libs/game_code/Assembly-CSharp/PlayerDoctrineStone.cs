// Decompiled with JetBrains decompiler
// Type: PlayerDoctrineStone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerDoctrineStone : MonoBehaviour
{
  public SkeletonGraphic Spine;
  public CanvasGroup CanvasGroup;
  public const int TARGET_COUNT = 3;
  public static System.Action OnIncreaseCount;
  public static System.Action OnDecreaseCount;
  public static System.Action OnCachePosition;
  public PlayerFarming playerFarming;
  public bool GivenAnswer;
  public bool firstChoice;
  public bool giveFullDoctrine;
  public Coroutine collectRoutine;
  public TrackEntry t;
  public Sequence FadeOutSequence;
  public bool increamentCountSequenceFinished = true;

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

  public void HandleEvent(TrackEntry trackentry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "CameraShake":
        CameraManager.instance.ShakeCameraForDuration(0.7f, 1f, 0.2f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        break;
      case "Break":
        AudioManager.Instance.PlayOneShot("event:/temple_key/become_whole", this.playerFarming.gameObject);
        AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.playerFarming.gameObject);
        break;
      case "Shake":
        AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", this.playerFarming.gameObject);
        break;
      case "Close":
        AudioManager.Instance.PlayOneShot("event:/ui/close_menu", this.playerFarming.gameObject);
        break;
    }
  }

  public void OnEnable()
  {
    if (this.collectRoutine != null)
    {
      this.collectRoutine = this.StartCoroutine((IEnumerator) this.PlayCollectAnimation());
    }
    else
    {
      if (this.increamentCountSequenceFinished)
        return;
      this.CanvasGroup.alpha = 0.0f;
      this.Spine.enabled = false;
      ++this.CompletedDoctrineStones;
      this.increamentCountSequenceFinished = true;
    }
  }

  public void OnDisable()
  {
    if (this.FadeOutSequence == null)
      return;
    this.FadeOutSequence.Kill();
    this.FadeOutSequence = (Sequence) null;
  }

  public void Start()
  {
    this.playerFarming = this.GetComponentInParent<PlayerFarming>();
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.Spine.AnimationState.SetAnimation(0, DataManager.Instance.DoctrineCurrentCount.ToString(), false);
    this.CanvasGroup.alpha = 0.0f;
    this.Spine.enabled = false;
  }

  public void OnDestroy()
  {
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    if (this.t == null)
      return;
    this.t.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.CompleteAnimation);
  }

  public void Play() => this.GivePiece();

  public void GivePiece()
  {
    this.giveFullDoctrine = DataManager.Instance.DoctrineCurrentCount >= 3;
    if (this.giveFullDoctrine)
    {
      this.SetPlayerCanvasesEnabled(false);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FindDoctrineStone);
      System.Action onCachePosition = PlayerDoctrineStone.OnCachePosition;
      if (onCachePosition != null)
        onCachePosition();
      if (DataManager.Instance.ShowFirstDoctrineStone)
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(this.playerFarming.CrownBone.gameObject, 7f);
      }
    }
    if (this.collectRoutine != null)
    {
      this.StopCoroutine(this.collectRoutine);
      this.collectRoutine = (Coroutine) null;
    }
    this.collectRoutine = this.StartCoroutine((IEnumerator) this.PlayCollectAnimation());
  }

  public void SetPlayerCanvasesEnabled(bool state)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.playerCanvas != (UnityEngine.Object) null)
        player.playerCanvas.enabled = state;
    }
  }

  public IEnumerator PlayCollectAnimation()
  {
    if (this.FadeOutSequence != null)
      this.FadeOutSequence.Kill();
    this.CanvasGroup.DOKill();
    this.CanvasGroup.alpha = 1f;
    this.Spine.enabled = true;
    this.t = this.Spine.AnimationState.SetAnimation(0, DataManager.Instance.DoctrineCurrentCount.ToString() + "-activate", false);
    this.Spine.AnimationState.AddAnimation(0, DataManager.Instance.DoctrineCurrentCount.ToString(), false, 0.0f);
    if (this.giveFullDoctrine)
    {
      AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_last_piece");
      if (DataManager.Instance.ShowFirstDoctrineStone)
        this.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
    }
    else
      AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_piece");
    yield return (object) new WaitForSpineAnimationComplete(this.t);
    this.CompleteAnimation(this.t);
    this.collectRoutine = (Coroutine) null;
  }

  public void CompleteAnimation(TrackEntry trackentry)
  {
    this.t.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.CompleteAnimation);
    if (this.giveFullDoctrine)
    {
      this.EndDeclareDoctrine();
    }
    else
    {
      this.giveFullDoctrine = false;
      this.EndPlayingAndFadeOut(false);
    }
  }

  public void EndDeclareDoctrine()
  {
    this.SetPlayerCanvasesEnabled(true);
    if (DataManager.Instance.ShowFirstDoctrineStone)
      GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.ShowFirstDoctrineStone = false;
    DataManager.Instance.DoctrineCurrentCount = 0;
    this.giveFullDoctrine = false;
    this.EndPlayingAndFadeOut(true);
  }

  public void EndPlayingAndFadeOut(bool IncrementCount)
  {
    this.FadeOutSequence = DOTween.Sequence();
    if (IncrementCount)
    {
      this.increamentCountSequenceFinished = false;
      this.FadeOutSequence.AppendInterval(0.25f);
      this.FadeOutSequence.AppendCallback((TweenCallback) (() =>
      {
        this.CanvasGroup.alpha = 0.0f;
        this.Spine.enabled = false;
        ++this.CompletedDoctrineStones;
        this.increamentCountSequenceFinished = true;
      }));
      this.FadeOutSequence.AppendInterval(1f);
    }
    this.FadeOutSequence.Play<Sequence>();
  }

  [CompilerGenerated]
  public void \u003CEndPlayingAndFadeOut\u003Eb__28_0()
  {
    this.CanvasGroup.alpha = 0.0f;
    this.Spine.enabled = false;
    ++this.CompletedDoctrineStones;
    this.increamentCountSequenceFinished = true;
  }
}
