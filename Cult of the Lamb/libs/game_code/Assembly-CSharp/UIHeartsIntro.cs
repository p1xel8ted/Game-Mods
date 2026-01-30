// Decompiled with JetBrains decompiler
// Type: UIHeartsIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class UIHeartsIntro : MonoBehaviour
{
  [SerializeField]
  public SkeletonGraphic[] hearts;

  public IEnumerator HeartRoutine(PlayerFarming playerFarming)
  {
    UIHeartsIntro uiHeartsIntro = this;
    int heartsCount = playerFarming.health.PLAYER_STARTING_HEALTH / 2;
    for (int index = 0; index < uiHeartsIntro.hearts.Length; ++index)
    {
      uiHeartsIntro.hearts[index].transform.localScale = Vector3.zero;
      if (index >= heartsCount)
        uiHeartsIntro.hearts[index].gameObject.SetActive(false);
    }
    while ((Object) playerFarming.hudHearts == (Object) null)
      yield return (object) null;
    playerFarming.hudHearts.GetComponent<CanvasGroup>().alpha = 0.0f;
    while ((Object) playerFarming == (Object) null || playerFarming.GoToAndStopping)
      yield return (object) null;
    playerFarming.state.LookAngle = 0.0f;
    playerFarming.state.facingAngle = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false, playerFarming: playerFarming);
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(1f);
    int i;
    for (i = 0; i < heartsCount; ++i)
    {
      if (!((Object) uiHeartsIntro.hearts[i] == (Object) null))
      {
        uiHeartsIntro.hearts[i].transform.localScale = Vector3.one * 2.7f;
        uiHeartsIntro.hearts[i].AnimationState.SetAnimation(0, "fill-whole", false);
        AudioManager.Instance.PlayOneShot("event:/player/collect_heart");
        yield return (object) new WaitForSeconds(0.25f);
      }
    }
    HUD_Manager.Instance.Show(0);
    for (i = 0; i < heartsCount; ++i)
    {
      uiHeartsIntro.StartCoroutine((IEnumerator) uiHeartsIntro.AnimateHeart(uiHeartsIntro.hearts[i], i, playerFarming));
      yield return (object) new WaitForSeconds(0.5f);
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (Component heart in uiHeartsIntro.hearts)
      heart.gameObject.SetActive(false);
    playerFarming.hudHearts.GetComponent<CanvasGroup>().alpha = 1f;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) uiHeartsIntro.gameObject);
  }

  public IEnumerator AnimateHeart(SkeletonGraphic heart, int index, PlayerFarming playerFarming)
  {
    heart.transform.DOScale(heart.transform.localScale * 1.25f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    heart.transform.DOMove(playerFarming.hudHearts.HeartIcons[index].transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.6f);
    heart.transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.1f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    yield return (object) new WaitForSeconds(0.3f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
  }
}
