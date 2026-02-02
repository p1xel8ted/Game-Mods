// Decompiled with JetBrains decompiler
// Type: UIDeathScreenLevelNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDeathScreenLevelNode : BaseMonoBehaviour
{
  public SkeletonGraphic Spine;
  public Image icon;

  public void Play(
    float Delay,
    UIDeathScreenLevelNode.ResultTypes ResultType,
    UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin,
    int index,
    bool endNode)
  {
    this.StartCoroutine((IEnumerator) this.PlayRoutine(Delay, ResultType, LevelNodeSkin, index, endNode));
  }

  public IEnumerator PlayRoutine(
    float Delay,
    UIDeathScreenLevelNode.ResultTypes ResultType,
    UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin,
    int index,
    bool endNode)
  {
    UIDeathScreenLevelNode deathScreenLevelNode = this;
    deathScreenLevelNode.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(deathScreenLevelNode.HandleEvent);
    DOTweenModuleUI.DOFade(deathScreenLevelNode.icon, 0.01f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
    if (index >= 0 && index <= DataManager.Instance.FollowersRecruitedInNodes.Count - 1 && DataManager.Instance.FollowersRecruitedInNodes[index] > 0)
      deathScreenLevelNode.Spine.Skeleton.SetSkin(LevelNodeSkin.ToString() + "-follower");
    else if (LevelNodeSkin != UIDeathScreenLevelNode.LevelNodeSkins.boss)
      deathScreenLevelNode.Spine.Skeleton.SetSkin(LevelNodeSkin.ToString() + (endNode ? "-end" : string.Empty));
    else
      deathScreenLevelNode.Spine.Skeleton.SetSkin(LevelNodeSkin.ToString());
    deathScreenLevelNode.Spine.AnimationState.SetAnimation(0, "appear", false);
    switch (ResultType)
    {
      case UIDeathScreenLevelNode.ResultTypes.Completed:
        yield return (object) new WaitForSecondsRealtime(Delay);
        if (LevelNodeSkin == UIDeathScreenLevelNode.LevelNodeSkins.boss)
        {
          deathScreenLevelNode.Spine.AnimationState.SetAnimation(0, "fill-boss", false);
          UIManager.PlayAudio("event:/ui/level_node_beat_boss");
          MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
          break;
        }
        UIManager.PlayAudio("event:/ui/level_node_beat_level");
        deathScreenLevelNode.Spine.AnimationState.SetAnimation(0, "fill", false);
        deathScreenLevelNode.Spine.AnimationState.AddAnimation(0, "full", false, 0.0f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
        break;
      case UIDeathScreenLevelNode.ResultTypes.Killed:
        yield return (object) new WaitForSecondsRealtime(Delay);
        deathScreenLevelNode.Spine.AnimationState.SetAnimation(0, "fail", false);
        deathScreenLevelNode.Spine.AnimationState.AddAnimation(0, "failed", false, 0.0f);
        UIManager.PlayAudio("event:/ui/level_node_die");
        MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
        break;
    }
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "reveal"))
      return;
    DOTweenModuleUI.DOFade(this.icon, 1f, 0.33f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void OnDisable()
  {
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public enum ResultTypes
  {
    Completed,
    Killed,
    Unreached,
  }

  public enum LevelNodeSkins
  {
    normal,
    boss,
    other,
  }
}
