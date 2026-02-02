// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeNpcBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Collections;
using System.Threading;
using UnityEngine;

#nullable disable
namespace Flockade;

public abstract class FlockadeNpcBase : FlockadePlayerBase
{
  public const string _CHANGE_SELECTION_SOUND = "event:/ui/change_selection";
  public const float _CONFIRM_ACTION_SIMULATION_PACING = 0.6f;
  public const float _SELECT_ACTION_SIMULATION_PACING = 1.2f;

  public virtual bool TurnSkipped => false;

  public override void PrefocusChoice(FlockadeGamePieceChoice choice)
  {
    choice.TryPerformSelectAction(this.Highlight);
    UIManager.PlayAudio("event:/ui/change_selection");
  }

  public override IEnumerator SelectGamePiece(FlockadeGamePieceChoice[] availableChoices)
  {
    FlockadeNpcBase requester = this;
    if (!requester.TurnSkipped)
      yield return (object) requester.WaitForSeconds(1.2f);
    IEnumerator coroutine;
    yield return (object) (coroutine = requester.Select(availableChoices));
    FlockadeGamePieceChoice choice = coroutine.Current as FlockadeGamePieceChoice;
    if ((Object) availableChoices[0] != (Object) choice)
    {
      availableChoices[0].TryPerformDeselectAction();
      choice.TryPerformSelectAction(requester.Highlight);
      UIManager.PlayAudio("event:/ui/change_selection");
      yield return (object) requester.WaitForSeconds(0.6f);
    }
    choice.Selectable.TryPerformConfirmAction();
    requester._gameBoard.Information.Lock((object) requester);
    choice.TryPerformDeselectAction();
    yield return (object) choice;
  }

  public abstract IEnumerator Select(FlockadeGamePieceChoice[] choices);

  public override IEnumerator PlaceGamePiece(
    FlockadeGameBoardTile[] availableTiles,
    CancellationToken? cancellation = null)
  {
    FlockadeNpcBase flockadeNpcBase = this;
    if (!flockadeNpcBase.TurnSkipped)
    {
      availableTiles[0].TryPerformSelectAction(flockadeNpcBase.Highlight);
      UIManager.PlayAudio("event:/ui/change_selection");
      flockadeNpcBase.SelectedTile = availableTiles[0];
      yield return (object) flockadeNpcBase.WaitForSeconds(1.2f);
    }
    IEnumerator coroutine;
    yield return (object) (coroutine = flockadeNpcBase.Place(availableTiles));
    FlockadeGameBoardTile target = coroutine.Current as FlockadeGameBoardTile;
    if ((Object) target != (Object) flockadeNpcBase.SelectedTile)
    {
      if ((bool) (Object) flockadeNpcBase.SelectedTile)
        flockadeNpcBase.SelectedTile.TryPerformDeselectAction();
      target.TryPerformSelectAction(flockadeNpcBase.Highlight);
      UIManager.PlayAudio("event:/ui/change_selection");
      flockadeNpcBase.SelectedTile = target;
      yield return (object) flockadeNpcBase.WaitForSeconds(0.6f);
    }
    target.Selectable.TryPerformConfirmAction();
    target.TryPerformDeselectAction();
    yield return (object) target;
  }

  public abstract IEnumerator Place(FlockadeGameBoardTile[] availableTiles);

  public IEnumerator WaitForSeconds(float time)
  {
    float startTime = Time.time;
    while (!this.TurnSkipped && (double) Time.time < (double) startTime + (double) time)
      yield return (object) null;
  }
}
