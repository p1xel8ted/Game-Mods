// Decompiled with JetBrains decompiler
// Type: JobBoardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public abstract class JobBoardManager : MonoBehaviour
{
  [SerializeField]
  public Interaction_JobBoard _jobBoard;
  [SerializeField]
  public GhostNPC _ghost;

  public Interaction_JobBoard JobBoard => this._jobBoard;

  public void Start()
  {
    this._jobBoard.OnJobCompleted += new Interaction_JobBoard.JobEvent(this.OnJobCompleted);
  }

  public void OnJobCompleted(ObjectivesData objective)
  {
    this.StartCoroutine((IEnumerator) this.JobCompleteRoutine(objective));
  }

  public IEnumerator JobCompleteRoutine(ObjectivesData objective)
  {
    yield return (object) this.GiveJobReward(objective);
    if (this._jobBoard.BoardCompleted)
    {
      yield return (object) this.GiveBoardReward();
      this._jobBoard.AddHideLock();
      yield return (object) this.HideBoard();
      if ((Object) this._ghost?.JobBoardCompleteConvo != (Object) null)
      {
        this._ghost.JobBoardCompleteConvo.ResetConvo();
        yield return (object) this._ghost.JobBoardCompleteConvo.PlayIE();
      }
      this._jobBoard.RemoveHideLock();
    }
    else
    {
      GhostNPC ghost = this._ghost;
      if ((ghost != null ? (ghost.HasJobCompleteConvos ? 1 : 0) : 0) != 0)
      {
        Interaction_SimpleConversation jobCompleteConvo = this._ghost.NextJobCompleteConvo;
        jobCompleteConvo.ResetConvo();
        yield return (object) jobCompleteConvo.PlayIE();
      }
    }
  }

  public virtual IEnumerator GiveJobReward(ObjectivesData objective)
  {
    yield break;
  }

  public virtual IEnumerator HideBoard()
  {
    if (!((Object) this._jobBoard == (Object) null))
      yield return (object) this._jobBoard.HideIE();
  }

  public virtual IEnumerator GiveBoardReward()
  {
    yield break;
  }
}
