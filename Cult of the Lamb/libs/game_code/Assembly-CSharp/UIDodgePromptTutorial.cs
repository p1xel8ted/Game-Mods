// Decompiled with JetBrains decompiler
// Type: UIDodgePromptTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class UIDodgePromptTutorial : BaseMonoBehaviour
{
  public RectTransform PromptRT;
  public MMControlPrompt Prompt;
  public SkeletonAnimation[] AttackerSpines;
  public Material NormalMaterial;
  public Material BW_Material;
  public Material enemyNormalMaterial;
  public Material enemyBWMaterial;
  public Shader stencilShader;

  public void Play(GameObject Attacker, PlayerFarming playerFarming)
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
    playerFarming.Spine.CustomMaterialOverride.Clear();
    playerFarming.Spine.CustomMaterialOverride.Add(playerFarming.originalMaterial, playerFarming.BW_Material);
    this.Prompt.playerFarming = playerFarming;
    this.Prompt.ForceUpdate();
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    this.StartCoroutine((IEnumerator) this.AwaitInputRoutine(playerFarming));
  }

  public IEnumerator AwaitInputRoutine(PlayerFarming playerFarming)
  {
    UIDodgePromptTutorial dodgePromptTutorial = this;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    float Duration = 0.0f;
    while ((double) (Duration += Time.unscaledDeltaTime) < 3.0 && (double) Time.timeScale <= 0.0)
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
    playerFarming.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    GameManager.GetInstance().CameraResetTargetZoom();
    HUD_Manager.Instance.Show(0);
    playerFarming.DodgeQueued = true;
    Time.timeScale = 1f;
    ++DataManager.Instance.ShownDodgeTutorialCount;
    Object.Destroy((Object) dodgePromptTutorial.gameObject);
  }
}
