// Decompiled with JetBrains decompiler
// Type: TempleEmblem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.Assets;
using System.Collections;
using UnityEngine;

#nullable disable
public class TempleEmblem : MonoBehaviour
{
  public SpriteRenderer Emblem;
  public SpriteRenderer Border;
  public CultUpgradeIconMapping iconMapping;
  public GameObject SpawnEffect;
  public int LAST_EMBLEM_LEVEL_ANIMATED;
  public int LAST_BORDER_LEVEL_ANIMATED;
  public Coroutine AnimateDecorations;

  public void OnEnable()
  {
    Debug.Log((object) $" Emblem animated level is {this.LAST_EMBLEM_LEVEL_ANIMATED.ToString()} {DataManager.Instance.TempleLevel.ToString()}");
    UICultUpgradesMenuController.OnCultUpgraded += new System.Action(this.SetImages);
    this.SetImages();
  }

  public void OnDisable()
  {
    UICultUpgradesMenuController.OnCultUpgraded -= new System.Action(this.SetImages);
  }

  public void SetImages()
  {
    this.Emblem.enabled = DataManager.Instance.TempleLevel >= 1;
    this.Border.enabled = DataManager.Instance.TempleBorder >= 100;
    Debug.Log((object) ("Border.enabled " + this.Border.enabled.ToString()));
    bool animateEmblemIntro = this.Emblem.enabled && this.LAST_EMBLEM_LEVEL_ANIMATED != 0 && this.LAST_EMBLEM_LEVEL_ANIMATED < DataManager.Instance.TempleLevel;
    bool animateBorderIntro = this.Border.enabled && this.LAST_BORDER_LEVEL_ANIMATED != 0 && this.LAST_BORDER_LEVEL_ANIMATED < DataManager.Instance.TempleBorder;
    this.LAST_EMBLEM_LEVEL_ANIMATED = DataManager.Instance.TempleLevel;
    this.LAST_BORDER_LEVEL_ANIMATED = DataManager.Instance.TempleBorder;
    if (this.AnimateDecorations != null)
      this.StopCoroutine(this.AnimateDecorations);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_TempleAltar.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_TempleAltar.Instance.state != (UnityEngine.Object) null && (UnityEngine.Object) PlayerFarming.Instance.state != (UnityEngine.Object) null && (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation || Interaction_TempleAltar.Instance.state.CURRENT_STATE == StateMachine.State.CustomAnimation))
    {
      animateEmblemIntro = false;
      animateBorderIntro = false;
    }
    if (animateEmblemIntro)
    {
      this.Emblem.sprite = this.iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleLevel - 1, 1));
      if (DataManager.Instance.TempleLevel <= 1)
        this.Emblem.gameObject.SetActive(false);
    }
    else
      this.Emblem.sprite = this.iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleLevel, 1));
    if (animateBorderIntro)
    {
      this.Border.sprite = this.iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleBorder - 1, 1));
      if (DataManager.Instance.TempleLevel <= 1)
        this.Border.gameObject.SetActive(false);
    }
    else
      this.Border.sprite = this.iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleBorder, 1));
    if (!(animateEmblemIntro | animateBorderIntro))
      return;
    this.AnimateDecorations = this.StartCoroutine((IEnumerator) this.AnimateIndividualDecorations(animateEmblemIntro, animateBorderIntro));
  }

  public IEnumerator AnimateIndividualDecorations(bool animateEmblemIntro, bool animateBorderIntro)
  {
    TempleEmblem templeEmblem = this;
    yield return (object) new WaitForSeconds(0.1f);
    if (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation && Interaction_TempleAltar.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation)
    {
      bool lambInPlace = false;
      PlayerFarming.Instance.GoToAndStop(templeEmblem.transform.position - Vector3.up * 3f, GoToCallback: (System.Action) (() => lambInPlace = true));
      while (!lambInPlace)
        yield return (object) new WaitForEndOfFrame();
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: PlayerFarming.Instance);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
      yield return (object) new WaitForSeconds(0.5f);
      float currentZoom = GameManager.GetInstance().CamFollowTarget.targetDistance;
      if (animateBorderIntro)
      {
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddToCamera(templeEmblem.Border.gameObject);
        GameManager.GetInstance().CameraSetTargetZoom(7f);
        GameManager.GetInstance().CameraSetZoom(7f);
        yield return (object) new WaitForSeconds(1f);
        templeEmblem.Border.sprite = templeEmblem.iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleBorder, 1));
        Vector3 localScale = templeEmblem.Border.transform.localScale;
        templeEmblem.Border.transform.localScale = Vector3.zero;
        templeEmblem.Border.transform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
        templeEmblem.Border.gameObject.SetActive(true);
        if ((UnityEngine.Object) templeEmblem.SpawnEffect != (UnityEngine.Object) null)
        {
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(templeEmblem.SpawnEffect, templeEmblem.Border.transform.parent);
          gameObject.transform.position = templeEmblem.Border.transform.position;
          gameObject.SetActive(true);
          UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 3f);
        }
        AudioManager.Instance.PlayOneShot("event:/explosion/explosion", templeEmblem.Border.transform.position);
        CameraManager.shakeCamera(4f, Utils.GetAngle(templeEmblem.transform.position, templeEmblem.Border.transform.position));
        yield return (object) new WaitForSeconds(2f);
      }
      if (animateEmblemIntro)
      {
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddToCamera(templeEmblem.Emblem.gameObject);
        GameManager.GetInstance().CameraSetTargetZoom(5f);
        GameManager.GetInstance().CameraSetZoom(5f);
        yield return (object) new WaitForSeconds(1f);
        templeEmblem.Emblem.sprite = templeEmblem.iconMapping.GetImage((CultUpgradeData.TYPE) Mathf.Max(DataManager.Instance.TempleLevel, 1));
        Vector3 localScale = templeEmblem.Emblem.transform.localScale;
        templeEmblem.Emblem.transform.localScale = Vector3.zero;
        templeEmblem.Emblem.transform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
        templeEmblem.Emblem.gameObject.SetActive(true);
        if ((UnityEngine.Object) templeEmblem.SpawnEffect != (UnityEngine.Object) null)
        {
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(templeEmblem.SpawnEffect, templeEmblem.Emblem.transform.parent);
          gameObject.transform.position = templeEmblem.Emblem.transform.position;
          gameObject.SetActive(true);
          UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 3f);
        }
        AudioManager.Instance.PlayOneShot("event:/explosion/explosion", templeEmblem.Emblem.transform.position);
        CameraManager.shakeCamera(4f, Utils.GetAngle(templeEmblem.transform.position, templeEmblem.Emblem.transform.position));
        yield return (object) new WaitForSeconds(2f);
      }
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().AddPlayerToCamera();
      GameManager.GetInstance().CameraResetTargetZoom();
      GameManager.GetInstance().CameraSetZoom(currentZoom);
      yield return (object) new WaitForSeconds(1f);
      PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      yield return (object) new WaitForSeconds(1f);
      PlayerFarming.SetStateForAllPlayers();
    }
  }
}
