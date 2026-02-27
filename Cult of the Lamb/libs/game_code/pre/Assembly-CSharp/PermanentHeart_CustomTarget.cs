// Decompiled with JetBrains decompiler
// Type: PermanentHeart_CustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class PermanentHeart_CustomTarget : MonoBehaviour
{
  private const string Path = "Prefabs/Resources/Permanent Half Heart Custom Target";
  [SerializeField]
  private SpriteRenderer SpriteRenderer;
  [SerializeField]
  private Interaction_PermanentHeart InteractionPermanentHeart;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  private System.Action Callback;
  private bool Activating;

  public static void Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/Permanent Half Heart Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<PermanentHeart_CustomTarget>().Play(EndPosition, Duration, Callback);
  }

  private StateMachine state => PlayerFarming.Instance.state;

  public void Play(Vector3 EndPosition, float Duration, System.Action Callback)
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
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine()
  {
    PermanentHeart_CustomTarget heartCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    heartCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    heartCustomTarget.StopAllCoroutines();
    System.Action callback = heartCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  private IEnumerator DelayEffectsRoutine()
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
