// Decompiled with JetBrains decompiler
// Type: BellTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BellTower : Interaction
{
  public UnityEvent OnHitCallback;
  public Health health;
  public GameObject Bell;
  public GameObject BellBottom;
  public float BellSpeed;
  public float zRotation;
  [Range(0.1f, 0.9f)]
  public float Elastic = 0.1f;
  [Range(0.1f, 0.9f)]
  public float Friction = 0.9f;
  public float ImpactForce = 20f;
  [SerializeField]
  [TermsPopup("")]
  public string interactableTerm;
  [SerializeField]
  [TermsPopup("")]
  public string nonInteractableTerm;
  [SerializeField]
  public GameObject availableObject;
  [SerializeField]
  public bool isPub;

  public override void GetLabel()
  {
    this.Label = this.Interactable ? LocalizationManager.GetTranslation(this.interactableTerm) : LocalizationManager.GetTranslation(this.nonInteractableTerm);
    if (!this.isPub || Interaction_Pub.Pubs[0].GetAmountOfPreparedDrinks() > 0)
      return;
    this.Label = LocalizationManager.GetTranslation("Interactions/NoDrinksBrewed");
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    Vector3 position1 = this.transform.position;
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", position1);
    Vector3 position2 = this.playerFarming.gameObject.transform.position;
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(position1, position2));
    this.BellSpeed = this.ImpactForce * ((double) position2.x < (double) position1.x ? 1f : -1f);
    if (this.OnHitCallback != null)
      this.OnHitCallback.Invoke();
    this.HasChanged = true;
  }

  public override void Update()
  {
    base.Update();
    this.BellSpeed += (0.0f - this.zRotation) * this.Elastic;
    this.zRotation += (this.BellSpeed *= this.Friction);
    this.Bell.transform.eulerAngles = new Vector3(-60f, 0.0f, this.zRotation);
    this.BellBottom.transform.eulerAngles = new Vector3(-60f, 0.0f, this.zRotation * 3f);
    this.availableObject.gameObject.SetActive(this.Interactable);
  }
}
