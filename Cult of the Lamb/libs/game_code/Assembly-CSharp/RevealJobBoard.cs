// Decompiled with JetBrains decompiler
// Type: RevealJobBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RevealJobBoard : MonoBehaviour
{
  [SerializeField]
  public bool revealOnEnable;
  [SerializeField]
  public DataManager.Variables _boardVariable;
  [SerializeField]
  public GameObject _jobBoard;

  public void SetJobBoard(GameObject jobBoard) => this._jobBoard = jobBoard;

  public void OnEnable() => this.Initialize();

  public void Initialize()
  {
    if (!((Object) this._jobBoard != (Object) null))
      return;
    Interaction_JobBoard component = this._jobBoard.GetComponent<Interaction_JobBoard>();
    if (!this.revealOnEnable || DataManager.Instance.GetVariable(component.ActiveVariable) || !DataManager.Instance.GetVariable(this._boardVariable))
      return;
    component.Reveal();
  }

  public void OnRevealEvent() => this._jobBoard.GetComponent<Interaction_JobBoard>().Reveal();
}
