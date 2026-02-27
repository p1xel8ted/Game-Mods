// Decompiled with JetBrains decompiler
// Type: Interaction_CultDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_CultDoor : Interaction
{
  public Transform ShakeObject;
  public GameObject KeyToShow;
  public Collider2D collider;
  public float ShakeAmount = 0.2f;
  public float v1 = 0.4f;
  public float v2 = 0.7f;
  public bool Activated;
  public string sOpenDoor;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sOpenDoor = ScriptLocalization.Interactions.OpenDoor;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sOpenDoor;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StopAllCoroutines();
  }

  public IEnumerator OpenDoor()
  {
    Interaction_CultDoor interactionCultDoor = this;
    interactionCultDoor.Activated = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionCultDoor.state.gameObject, 5f);
    Vector3 BookTargetPosition = interactionCultDoor.state.gameObject.GetComponent<PlayerSimpleInventory>().ItemImage.transform.position;
    interactionCultDoor.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionCultDoor.KeyToShow.SetActive(true);
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 2.5)
    {
      interactionCultDoor.KeyToShow.transform.position = Vector3.Lerp(interactionCultDoor.KeyToShow.transform.position, BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionCultDoor.state.CURRENT_STATE = StateMachine.State.InActive;
    interactionCultDoor.state.facingAngle = 90f;
    interactionCultDoor.KeyToShow.SetActive(false);
    GameManager.GetInstance().OnConversationNext(interactionCultDoor.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      CameraManager.shakeCamera(Random.Range(0.15f, 0.2f), (float) Random.Range(0, 360));
      Vector3 localPosition = interactionCultDoor.ShakeObject.localPosition with
      {
        z = (float) (2.0 * ((double) Progress / (double) Duration))
      };
      interactionCultDoor.ShakeObject.localPosition = localPosition;
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(1f);
    interactionCultDoor.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionCultDoor.collider.enabled = false;
    GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.SetVariable(DataManager.Variables.Goat_Guardian_Door_Open, true);
  }

  public IEnumerator DoShake()
  {
    float Timer = 0.0f;
    float ShakeSpeed = this.ShakeAmount;
    float Shake = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 3.0)
    {
      ShakeSpeed += (0.0f - Shake) * this.v1;
      Shake += (ShakeSpeed *= this.v2);
      this.ShakeObject.localPosition = Vector3.left * Shake;
      yield return (object) null;
    }
  }
}
