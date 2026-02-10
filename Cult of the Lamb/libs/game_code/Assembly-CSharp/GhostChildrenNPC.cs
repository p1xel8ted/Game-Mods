// Decompiled with JetBrains decompiler
// Type: GhostChildrenNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class GhostChildrenNPC : BaseMonoBehaviour
{
  [SerializeField]
  public List<Interaction_SimpleConversation> conversations = new List<Interaction_SimpleConversation>();
  [SerializeField]
  public SkeletonAnimation goodTwinSkeleton;
  [SerializeField]
  public SkeletonAnimation badTwinSkeleton;

  public int JobsCompletedCount
  {
    get
    {
      int jobsCompletedCount = 0;
      for (int index = 0; index < this.conversations.Count; ++index)
      {
        if (DataManager.Instance.JobBoardsClaimedQuests.Contains(200 + index))
          ++jobsCompletedCount;
      }
      return jobsCompletedCount;
    }
  }

  public void GotoConversationPoint()
  {
    Interaction_SimpleConversation simpleConversation = this.conversations.ElementAtOrDefault<Interaction_SimpleConversation>(this.JobsCompletedCount);
    if (!simpleConversation.MovePlayerToListenPosition)
      return;
    PlayerFarming.Instance.GoToAndStop(this.transform.position + simpleConversation.ListenPosition, this.gameObject, maxDuration: 2f, forcePositionOnTimeout: true, groupAction: true);
  }

  public IEnumerator PlayFoundConversation()
  {
    Interaction_SimpleConversation currentConversation = this.conversations.ElementAtOrDefault<Interaction_SimpleConversation>(this.JobsCompletedCount);
    this.goodTwinSkeleton.gameObject.SetActive(false);
    this.badTwinSkeleton.gameObject.SetActive(false);
    this.goodTwinSkeleton.gameObject.SetActive(true);
    this.goodTwinSkeleton.transform.DOScale(this.goodTwinSkeleton.transform.localScale, 0.5f).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.goodTwinSkeleton.AnimationState.SetAnimation(0, "appear", false);
    this.goodTwinSkeleton.AnimationState.AddAnimation(0, "idle-good", true, 0.0f);
    yield return (object) new WaitForSeconds(0.3f);
    this.badTwinSkeleton.gameObject.SetActive(true);
    this.badTwinSkeleton.transform.DOScale(this.badTwinSkeleton.transform.localScale, 0.5f).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.badTwinSkeleton.AnimationState.SetAnimation(0, "appear", false);
    this.badTwinSkeleton.AnimationState.AddAnimation(0, "idle-evil", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    if ((UnityEngine.Object) currentConversation != (UnityEngine.Object) null)
    {
      currentConversation.Spoken = false;
      currentConversation.Play();
      yield return (object) new WaitUntil((Func<bool>) (() => currentConversation.Finished));
      this.badTwinSkeleton.AnimationState.SetAnimation(0, "disappear", false);
      yield return (object) new WaitForSeconds(0.3f);
      this.goodTwinSkeleton.AnimationState.SetAnimation(0, "disappear", false);
      yield return (object) new WaitForSeconds(0.3f);
      CompanionBaseArea.hasGhostTwins = true;
      CompanionBaseArea.SpawnCompanionGhosts();
      ObjectiveManager.CompleteFindChildrenObjective();
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Tarot", Objectives.CustomQuestTypes.ReturnToTarotJobBoard), true, true);
      yield return (object) new WaitForSeconds(1.5f);
    }
  }
}
