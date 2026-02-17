// Decompiled with JetBrains decompiler
// Type: RelicCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RelicCustomTarget : MonoBehaviour
{
  public static List<RelicCustomTarget> relicCustomTargets = new List<RelicCustomTarget>();
  public const string Path = "Prefabs/Resources/Relic Custom Target";
  public SpriteRenderer SpriteRenderer;
  public PlayerFarming pFarming;
  public CameraFollowTarget c;
  public RelicType RelicType;
  public System.Action Callback;
  public bool Activating;

  public static GameObject Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    RelicType relicType,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/Relic Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject;
    RelicCustomTarget component = gameObject.GetComponent<RelicCustomTarget>();
    RelicCustomTarget.relicCustomTargets.Add(component);
    component.Play(EndPosition, Duration, relicType, Callback);
    return gameObject;
  }

  public static bool IsRelicPickUpActive(RelicType relicType)
  {
    foreach (RelicCustomTarget relicCustomTarget in RelicCustomTarget.relicCustomTargets)
    {
      if (relicCustomTarget.RelicType == relicType)
        return true;
    }
    return false;
  }

  public StateMachine state => PlayerFarming.Instance.state;

  public void Play(Vector3 EndPosition, float Duration, RelicType relicType, System.Action Callback)
  {
    this.RelicType = relicType;
    this.Callback = Callback;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.SpriteRenderer.sprite = EquipmentManager.GetRelicData(relicType).Sprite;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SpriteRenderer.enabled = false;
      if (DataManager.Instance.PlayerFoundRelics.Contains(relicType))
        return;
      this.OpenMenu();
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  public void OpenMenu()
  {
    AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_pickup", this.gameObject);
    BiomeConstants.Instance.EmitPickUpVFX(this.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    if (this.RelicType != RelicType.None)
      DataManager.UnlockRelic(this.RelicType);
    UIRelicMenuController relicMenuController = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
    relicMenuController.Show(this.RelicType, (PlayerFarming) null);
    relicMenuController.OnHidden = relicMenuController.OnHidden + new System.Action(this.BackToIdle);
  }

  public void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  public IEnumerator BackToIdleRoutine()
  {
    RelicCustomTarget relicCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    relicCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    relicCustomTarget.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) relicCustomTarget.DelayEffectsRoutine());
    System.Action callback = relicCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  public IEnumerator DelayEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RelicCustomTarget relicCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) relicCustomTarget.gameObject);
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

  public void OnDestroy() => RelicCustomTarget.relicCustomTargets.Remove(this);
}
