// Decompiled with JetBrains decompiler
// Type: UIHeartsIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private SkeletonGraphic[] hearts;

  public IEnumerator HeartRoutine()
  {
    UIHeartsIntro uiHeartsIntro = this;
    int heartsCount = DataManager.Instance.PLAYER_STARTING_HEALTH / 2;
    for (int index = 0; index < uiHeartsIntro.hearts.Length; ++index)
    {
      uiHeartsIntro.hearts[index].transform.localScale = Vector3.zero;
      if (index >= heartsCount)
        uiHeartsIntro.hearts[index].gameObject.SetActive(false);
    }
    while ((Object) HUD_Hearts.Instance == (Object) null)
      yield return (object) null;
    HUD_Hearts.Instance.GetComponent<CanvasGroup>().alpha = 0.0f;
    while ((Object) PlayerFarming.Instance == (Object) null || PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    PlayerFarming.Instance.state.LookAngle = 0.0f;
    PlayerFarming.Instance.state.facingAngle = 0.0f;
    GameManager.GetInstance().OnConversationNew(true, false, false);
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(1f);
    int i;
    for (i = 0; i < heartsCount; ++i)
    {
      uiHeartsIntro.hearts[i].transform.localScale = Vector3.one * 2.7f;
      uiHeartsIntro.hearts[i].AnimationState.SetAnimation(0, "fill-whole", false);
      AudioManager.Instance.PlayOneShot("event:/player/collect_heart");
      yield return (object) new WaitForSeconds(0.25f);
    }
    HUD_Manager.Instance.Show(0);
    for (i = 0; i < heartsCount; ++i)
    {
      uiHeartsIntro.StartCoroutine((IEnumerator) uiHeartsIntro.AnimateHeart(uiHeartsIntro.hearts[i], i));
      yield return (object) new WaitForSeconds(0.5f);
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (Component heart in uiHeartsIntro.hearts)
      heart.gameObject.SetActive(false);
    HUD_Hearts.Instance.GetComponent<CanvasGroup>().alpha = 1f;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) uiHeartsIntro.gameObject);
  }

  private IEnumerator AnimateHeart(SkeletonGraphic heart, int index)
  {
    heart.transform.DOScale(heart.transform.localScale * 1.25f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    heart.transform.DOMove(HUD_Hearts.Instance.HeartIcons[index].transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.6f);
    heart.transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.1f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    yield return (object) new WaitForSeconds(0.3f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
  }
}
