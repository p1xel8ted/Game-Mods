// Decompiled with JetBrains decompiler
// Type: Interaction_Chalice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Chalice : Interaction
{
  public SkeletonAnimation Spine;
  public string sString;
  [HideInInspector]
  public bool Activating;
  public Interaction_Chalice.DrinkType Drink;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.DrinkFromChalice());
  }

  public IEnumerator DrinkFromChalice()
  {
    Interaction_Chalice interactionChalice = this;
    interactionChalice.Activating = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionChalice.playerFarming.CameraBone, 6f);
    interactionChalice.Spine.gameObject.SetActive(false);
    switch (interactionChalice.Drink)
    {
      case Interaction_Chalice.DrinkType.Poison:
        GameManager.GetInstance().OnConversationNext(interactionChalice.playerFarming.CameraBone, 4f);
        interactionChalice.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        yield return (object) new WaitForEndOfFrame();
        interactionChalice.playerFarming.simpleSpineAnimator.Animate("chalice-drink-bad", 0, false);
        interactionChalice.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
        yield return (object) new WaitForSeconds(3.5f);
        CameraManager.shakeCamera(0.5f);
        yield return (object) new WaitForSeconds(2.33333325f);
        GameManager.GetInstance().OnConversationEnd();
        yield return (object) new WaitForSeconds(0.5f);
        if ((double) interactionChalice.playerFarming.GetComponent<HealthPlayer>().HP - 4.0 <= 0.0)
        {
          interactionChalice.playerFarming.health.DealDamage(4f, interactionChalice.gameObject, interactionChalice.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
          break;
        }
        interactionChalice.playerFarming.health.HP -= 4f;
        break;
      case Interaction_Chalice.DrinkType.Vitality:
        GameManager.GetInstance().OnConversationNext(interactionChalice.playerFarming.CameraBone, 4f);
        interactionChalice.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        yield return (object) new WaitForEndOfFrame();
        interactionChalice.playerFarming.simpleSpineAnimator.Animate("chalice-drink-good", 0, false);
        interactionChalice.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
        yield return (object) new WaitForSeconds(3.5f);
        CameraManager.shakeCamera(0.5f);
        yield return (object) new WaitForSeconds(2.33333325f);
        GameManager.GetInstance().OnConversationEnd();
        yield return (object) new WaitForSeconds(0.5f);
        interactionChalice.playerFarming.GetComponent<HealthPlayer>().BlueHearts += 4f;
        break;
    }
    ++interactionChalice.Drink;
  }

  public enum DrinkType
  {
    Poison,
    Vitality,
  }
}
