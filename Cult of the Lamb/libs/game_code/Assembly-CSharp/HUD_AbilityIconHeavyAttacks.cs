// Decompiled with JetBrains decompiler
// Type: HUD_AbilityIconHeavyAttacks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class HUD_AbilityIconHeavyAttacks : MonoBehaviour
{
  public Transform Container;

  public void OnEnable()
  {
    this.Container.gameObject.SetActive(false);
    EnemyHasShield.OnTutorialShown += new System.Action(this.DrawAttention);
    BiomeGenerator.OnTutorialShown += new System.Action(this.DrawAttention);
    Interaction_WeaponSelectionPodium.OnTutorialShown += new System.Action(this.DrawAttention);
  }

  public void OnDisable()
  {
    EnemyHasShield.OnTutorialShown -= new System.Action(this.DrawAttention);
    BiomeGenerator.OnTutorialShown -= new System.Action(this.DrawAttention);
    Interaction_WeaponSelectionPodium.OnTutorialShown -= new System.Action(this.DrawAttention);
  }

  public void DrawAttention()
  {
    Vector3 vector3 = new Vector3(0.0f, -250f);
    if (!this.Container.gameObject.activeSelf)
      this.Container.transform.localPosition = vector3;
    this.Container.gameObject.SetActive(true);
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.AppendCallback((TweenCallback) (() => UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear")));
    s.Append((Tween) this.Container.transform.DOLocalMove(new Vector3(0.0f, 200f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.AppendCallback((TweenCallback) (() => UIManager.PlayAudio("event:/ui/objective_group_complete")));
    s.Append((Tween) this.Container.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.Append((Tween) this.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.AppendInterval(1f);
    s.AppendCallback((TweenCallback) (() => UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear")));
    s.Append((Tween) this.Container.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
  }
}
