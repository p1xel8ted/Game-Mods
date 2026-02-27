// Decompiled with JetBrains decompiler
// Type: Interaction_Chalice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Chalice : Interaction
{
  public SkeletonAnimation Spine;
  private string sString;
  [HideInInspector]
  public bool Activating;
  public Interaction_Chalice.DrinkType Drink;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.DrinkFromChalice());
  }

  private IEnumerator DrinkFromChalice()
  {
    Interaction_Chalice interactionChalice = this;
    interactionChalice.Activating = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    interactionChalice.Spine.gameObject.SetActive(false);
    switch (interactionChalice.Drink)
    {
      case Interaction_Chalice.DrinkType.Poison:
        GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
        interactionChalice.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        yield return (object) new WaitForEndOfFrame();
        PlayerFarming.Instance.simpleSpineAnimator.Animate("chalice-drink-bad", 0, false);
        PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
        yield return (object) new WaitForSeconds(3.5f);
        CameraManager.shakeCamera(0.5f);
        yield return (object) new WaitForSeconds(2.33333325f);
        GameManager.GetInstance().OnConversationEnd();
        yield return (object) new WaitForSeconds(0.5f);
        if ((double) PlayerFarming.Instance.GetComponent<HealthPlayer>().HP - 4.0 <= 0.0)
        {
          PlayerFarming.Instance.health.DealDamage(4f, interactionChalice.gameObject, interactionChalice.transform.position);
          break;
        }
        PlayerFarming.Instance.health.HP -= 4f;
        break;
      case Interaction_Chalice.DrinkType.Vitality:
        GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
        interactionChalice.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        yield return (object) new WaitForEndOfFrame();
        PlayerFarming.Instance.simpleSpineAnimator.Animate("chalice-drink-good", 0, false);
        PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
        yield return (object) new WaitForSeconds(3.5f);
        CameraManager.shakeCamera(0.5f);
        yield return (object) new WaitForSeconds(2.33333325f);
        GameManager.GetInstance().OnConversationEnd();
        yield return (object) new WaitForSeconds(0.5f);
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlueHearts += 4f;
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
