// Decompiled with JetBrains decompiler
// Type: UIDodgePromptTutorial
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
public class UIDodgePromptTutorial : BaseMonoBehaviour
{
  public RectTransform PromptRT;
  private SkeletonAnimation[] AttackerSpines;
  public Material NormalMaterial;
  public Material BW_Material;
  private Material enemyNormalMaterial;
  private Material enemyBWMaterial;
  public Shader stencilShader;

  public void Play(GameObject Attacker)
  {
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    this.PromptRT.localScale = Vector3.one * 0.5f;
    this.PromptRT.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.distance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.distance = x), 8f, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 8f, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.AttackerSpines = Attacker.GetComponentsInChildren<SkeletonAnimation>();
    foreach (SkeletonAnimation attackerSpine in this.AttackerSpines)
    {
      Debug.Log((object) attackerSpine.name);
      if ((Object) this.stencilShader == (Object) null)
        return;
      attackerSpine.gameObject.AddComponent<SkeletonRendererCustomMaterials>();
      attackerSpine.CustomMaterialOverride.Clear();
      this.enemyNormalMaterial = attackerSpine.GetComponent<MeshRenderer>().material;
      this.enemyBWMaterial = this.enemyNormalMaterial;
      this.enemyBWMaterial.shader = this.stencilShader;
      this.enemyBWMaterial.SetFloat("_StencilRef", 128f);
      this.enemyBWMaterial.SetFloat("_StencilOp", 2f);
      attackerSpine.CustomMaterialOverride.Add(this.enemyNormalMaterial, this.enemyBWMaterial);
      attackerSpine.CustomMaterialOverride[this.enemyNormalMaterial] = this.enemyBWMaterial;
    }
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    this.StartCoroutine((IEnumerator) this.AwaitInputRoutine());
  }

  private IEnumerator AwaitInputRoutine()
  {
    UIDodgePromptTutorial dodgePromptTutorial = this;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    float Duration = 0.0f;
    while ((double) (Duration += Time.unscaledDeltaTime) < 3.0)
    {
      if (InputManager.Gameplay.GetDodgeButtonHeld())
      {
        DataManager.Instance.ShownDodgeTutorial = true;
        break;
      }
      yield return (object) null;
    }
    foreach (SkeletonRenderer attackerSpine in dodgePromptTutorial.AttackerSpines)
      attackerSpine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    GameManager.GetInstance().CameraResetTargetZoom();
    HUD_Manager.Instance.Show(0);
    PlayerFarming.Instance.DodgeQueued = true;
    Time.timeScale = 1f;
    ++DataManager.Instance.ShownDodgeTutorialCount;
    Object.Destroy((Object) dodgePromptTutorial.gameObject);
  }
}
