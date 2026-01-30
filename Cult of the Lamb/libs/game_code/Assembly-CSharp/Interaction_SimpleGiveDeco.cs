// Decompiled with JetBrains decompiler
// Type: Interaction_SimpleGiveDeco
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_SimpleGiveDeco : Interaction
{
  [SerializeField]
  public StructureBrain.TYPES deco;
  [SerializeField]
  public string sfx;
  [SerializeField]
  public bool requiresGoatSkin;
  [SerializeField]
  public bool requiresNight;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = !StructuresData.HasRevealed(this.deco);
    PlayerFarming playerFarming = (PlayerFarming) null;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null || (double) Vector3.Distance(player.transform.position, this.transform.position) < (double) Vector3.Distance(playerFarming.transform.position, this.transform.position))
        playerFarming = player;
    }
    if (this.requiresGoatSkin)
    {
      if (!playerFarming.IsGoat && !playerFarming.IsGoat && playerFarming.isLamb)
        this.Interactable = false;
    }
    else if (this.requiresNight && TimeManager.CurrentPhase != DayPhase.Night)
      this.Interactable = false;
    this.Label = this.Interactable ? ScriptLocalization.FollowerInteractions.MakeDemand : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }

  public IEnumerator InteractIE()
  {
    Interaction_SimpleGiveDeco interactionSimpleGiveDeco = this;
    AudioManager.Instance.PlayOneShot(interactionSimpleGiveDeco.sfx, interactionSimpleGiveDeco.transform.position);
    Vector3 offset = GameManager.GetInstance().CamFollowTarget.TargetOffset;
    float zoom = GameManager.GetInstance().CamFollowTarget.targetDistance;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionSimpleGiveDeco.gameObject, 7f);
    GameManager.GetInstance().CameraSetOffset(Vector3.up * 1f);
    yield return (object) new WaitForSeconds(1f);
    interactionSimpleGiveDeco.transform.DOShakePosition(2f, 0.1f).SetEase<Tweener>(Ease.InSine);
    yield return (object) new WaitForSeconds(2f);
    DecorationCustomTarget.Create(interactionSimpleGiveDeco.transform.position + Vector3.back * 2f, interactionSimpleGiveDeco.state.transform.position, 2f, interactionSimpleGiveDeco.deco, (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetZoom(zoom);
      GameManager.GetInstance().CameraSetOffset(offset);
      GameManager.GetInstance().OnConversationEnd();
      this.HasChanged = true;
      this.Interactable = false;
    }));
  }
}
