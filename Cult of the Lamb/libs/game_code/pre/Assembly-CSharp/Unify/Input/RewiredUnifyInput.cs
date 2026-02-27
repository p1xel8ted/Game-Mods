// Decompiled with JetBrains decompiler
// Type: Unify.Input.RewiredUnifyInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using Rewired.Integration.UnityUI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Unify.Input;

internal class RewiredUnifyInput : MonoBehaviour
{
  [Tooltip("Reference to the RewiredUIInputModule used by the game for menus etc. so only active users controllers are allowed.  DO NOT set this to the instance on an Unify owner EventSystems as they must remain enabled at all times.")]
  public RewiredStandaloneInputModule RewiredUIInputModule;
  public bool AllowDisableUIInput;
  public string VertAxis;
  public string HorzAxis;
  public string SubmitAction;
  public string CancelAction;
  public string QuitAction;
  private RewiredInputManager rewiredInputManager;
  private int currentPlayerId = -2;
  private int[] currentPlayerIds = new int[1]{ -1 };
  private int[] noPlayerIds = new int[0];
  private bool enableInputOnAllButtonsUp;

  public static RewiredUnifyInput Instance { get; private set; }

  public void Awake()
  {
    Debug.Log((object) "RewiredUnifyInput: Awake");
    RewiredUnifyInput.Instance = this;
    this.rewiredInputManager = new RewiredInputManager();
    InputManager.Init((InputManager) this.rewiredInputManager);
  }

  public void Start()
  {
    SessionManager.OnSessionEnd += new SessionManager.SessionEventDelegate(this.OnSessionEnd);
    if (!((UnityEngine.Object) this.RewiredUIInputModule != (UnityEngine.Object) null))
      return;
    this.RewiredUIInputModule.UseAllRewiredGamePlayers = false;
  }

  public void OnDestroy()
  {
    SessionManager.OnSessionEnd -= new SessionManager.SessionEventDelegate(this.OnSessionEnd);
  }

  public void OnSessionEnd(Guid sessionGuid, User sessionUser)
  {
    if (!((UnityEngine.Object) this.RewiredUIInputModule != (UnityEngine.Object) null))
      return;
    this.currentPlayerId = -1;
    this.RewiredUIInputModule.UseAllRewiredGamePlayers = false;
    this.RewiredUIInputModule.RewiredPlayerIds = this.noPlayerIds;
  }

  public void ResetCurrentPlayerID() => this.currentPlayerId = -2;

  public void Update()
  {
    if ((UnityEngine.Object) this.RewiredUIInputModule != (UnityEngine.Object) null)
    {
      if (!this.AllowDisableUIInput || InputManager.InputEnabled)
      {
        this.RewiredUIInputModule.UseAllRewiredGamePlayers = true;
        this.RewiredUIInputModule.RewiredPlayerIds = (int[]) null;
      }
      else
      {
        this.RewiredUIInputModule.UseAllRewiredGamePlayers = false;
        this.RewiredUIInputModule.RewiredPlayerIds = this.noPlayerIds;
      }
    }
    if (!this.enableInputOnAllButtonsUp)
      return;
    bool flag = false;
    foreach (Player player in (IEnumerable<Player>) ReInput.players.Players)
    {
      if (player.GetAnyButtonDown())
      {
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.enableInputOnAllButtonsUp = false;
    InputManager.InputEnabled = true;
  }

  public void DisableInput() => InputManager.InputEnabled = false;

  public void EnableInput() => this.enableInputOnAllButtonsUp = true;
}
