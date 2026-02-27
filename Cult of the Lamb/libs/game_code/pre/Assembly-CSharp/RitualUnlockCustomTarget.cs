// Decompiled with JetBrains decompiler
// Type: RitualUnlockCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMTools;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualUnlockCustomTarget : MonoBehaviour
{
  private DoctrineUpgradeSystem.DoctrineType unlockType;
  private const string Path = "Prefabs/Resources/RitualUnlock Custom Target";
  public SpriteRenderer SpriteRenderer;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  private TarotCards.TarotCard DrawnCard;
  private System.Action Callback;
  private int level;
  private UpgradeSystem.Type type;

  public static void Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    DoctrineUpgradeSystem.DoctrineType _unlockType,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/RitualUnlock Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<RitualUnlockCustomTarget>().Play(EndPosition, Duration, _unlockType, Callback);
  }

  private StateMachine state => PlayerFarming.Instance.state;

  public void Play(
    Vector3 EndPosition,
    float Duration,
    DoctrineUpgradeSystem.DoctrineType _unlockType,
    System.Action Callback)
  {
    this.unlockType = _unlockType;
    switch (this.unlockType)
    {
      case DoctrineUpgradeSystem.DoctrineType.Special_Brainwashed:
        this.level = 2;
        this.type = UpgradeSystem.Type.Ritual_Brainwashing;
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice:
        this.level = 1;
        this.type = UpgradeSystem.Type.Ritual_Sacrifice;
        break;
      case DoctrineUpgradeSystem.DoctrineType.Special_Consume:
        this.level = 3;
        this.type = UpgradeSystem.Type.Ritual_ConsumeFollower;
        break;
    }
    this.Callback = Callback;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SpriteRenderer.enabled = false;
      this.StartCoroutine((IEnumerator) this.GiveRitual());
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  private IEnumerator GiveRitual()
  {
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, this.level, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    UpgradeSystem.UnlockAbility(this.type);
    this.BackToIdle();
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine()
  {
    RitualUnlockCustomTarget unlockCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    unlockCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    unlockCustomTarget.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) unlockCustomTarget.DelayEffectsRoutine());
    System.Action callback = unlockCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  private IEnumerator DelayEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualUnlockCustomTarget unlockCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) unlockCustomTarget.gameObject);
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
