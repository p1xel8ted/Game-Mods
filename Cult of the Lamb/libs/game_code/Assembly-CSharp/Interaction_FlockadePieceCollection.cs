// Decompiled with JetBrains decompiler
// Type: Interaction_FlockadePieceCollection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_FlockadePieceCollection : Interaction
{
  [SerializeField]
  public GameObject lookToObject;
  public string sString;
  public bool Activating;

  public void Start()
  {
    this.UpdateLocalisation();
    this.HasSecondaryInteraction = DataManager.Instance.OnboardedRelics;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = this.Activating ? "" : ScriptLocalization.Interactions.Look;
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.OpenMenuIE());
  }

  public IEnumerator OpenMenuIE()
  {
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadFlockadePiecesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    this.OpenMenu((UIMenuBase) MonoSingleton<UIManager>.Instance.FlockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>());
  }

  public void OpenMenu(UIMenuBase menu)
  {
    Time.timeScale = 0.0f;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    HUD_Manager.Instance.Hide(false, 0);
    menu.Show();
    menu.OnHide += (System.Action) (() =>
    {
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show(0);
      this.HasChanged = true;
    });
    menu.OnHidden += (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      PlayerFarming.SetStateForAllPlayers();
      this.Activating = false;
    });
  }

  [CompilerGenerated]
  public void \u003COpenMenu\u003Eb__8_0()
  {
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show(0);
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COpenMenu\u003Eb__8_1()
  {
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    PlayerFarming.SetStateForAllPlayers();
    this.Activating = false;
  }
}
