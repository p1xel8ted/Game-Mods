// Decompiled with JetBrains decompiler
// Type: PermanentHeart_CustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PermanentHeart_CustomTarget : MonoBehaviour
{
  public const string Path = "Prefabs/Resources/Permanent Half Heart Custom Target";
  [SerializeField]
  public SpriteRenderer SpriteRenderer;
  [SerializeField]
  public Interaction_PermanentHeart InteractionPermanentHeart;
  public PlayerFarming pFarming;
  public CameraFollowTarget c;
  public Action<Interaction_PermanentHeart> Callback;
  public bool Activating;

  public static void Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    Action<Interaction_PermanentHeart> Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/Permanent Half Heart Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<PermanentHeart_CustomTarget>().Play(EndPosition, Duration, Callback);
  }

  public StateMachine state => PlayerFarming.Instance.state;

  public void Play(
    Vector3 EndPosition,
    float Duration,
    Action<Interaction_PermanentHeart> Callback)
  {
    this.InteractionPermanentHeart.enabled = false;
    this.InteractionPermanentHeart.Particles.gameObject.SetActive(false);
    this.Callback = Callback;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.InteractionPermanentHeart.enabled = true;
      this.InteractionPermanentHeart.OnInteract(PlayerFarming.Instance.state);
      Action<Interaction_PermanentHeart> action = Callback;
      if (action == null)
        return;
      action(this.InteractionPermanentHeart);
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  public void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  public IEnumerator BackToIdleRoutine()
  {
    PermanentHeart_CustomTarget heartCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    heartCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    heartCustomTarget.StopAllCoroutines();
    Action<Interaction_PermanentHeart> callback = heartCustomTarget.Callback;
    if (callback != null)
      callback(heartCustomTarget.InteractionPermanentHeart);
  }

  public IEnumerator DelayEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    PermanentHeart_CustomTarget heartCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) heartCustomTarget.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
