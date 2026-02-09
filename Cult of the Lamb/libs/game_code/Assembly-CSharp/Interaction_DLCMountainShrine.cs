// Decompiled with JetBrains decompiler
// Type: Interaction_DLCMountainShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class Interaction_DLCMountainShrine : Interaction
{
  [SerializeField]
  public GameObject teleporter;
  public bool triggered;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/YngyaShrine") : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", new System.Action(this.PlayVideo));
  }

  public void PlayVideo()
  {
    AudioManager.Instance.StopCurrentMusic();
    HUD_Manager.Instance.Hide(true);
    MMVideoPlayer.Play("DLC_Intro", new System.Action(this.VideoComplete), MMVideoPlayer.Options.DISABLE, MMVideoPlayer.Options.DISABLE, false);
    AudioManager.Instance.PlayOneShot("event:/music/intro/dlc_intro_video");
    MMTransition.ResumePlay();
  }

  public void VideoComplete()
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) <= 0)
      Inventory.AddItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, 5);
    UnityEngine.Object.Destroy((UnityEngine.Object) MMVideoPlayer.Instance.gameObject);
    MMVideoPlayer.Instance = (GameObject) null;
    SeasonsManager.ActivateSeasons();
    DataManager.Instance.OnboardedWolf = true;
    DataManager.Instance.ForeshadowedWolf = true;
    DataManager.Instance.CurrentLocation = FollowerLocation.Base;
    MMTransition.ResumePlay();
    MMTransition.IsPlaying = false;
    AudioManager.Instance.StartMusic();
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.YNGYA_GHOST, 20, PlayerFarming.Instance.transform.position);
    this.Interactable = false;
    this.HasChanged = true;
    GameManager.GetInstance().OnConversationEnd();
    this.teleporter.gameObject.SetActive(true);
  }
}
