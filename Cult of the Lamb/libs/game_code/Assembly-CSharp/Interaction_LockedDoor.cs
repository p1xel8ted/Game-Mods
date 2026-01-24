// Decompiled with JetBrains decompiler
// Type: Interaction_LockedDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_LockedDoor : Interaction
{
  public bool Activated;
  public string sOpenDoor;
  public float OpeningTime = 1f;
  public BoxCollider2D Collider;
  public static List<Interaction_LockedDoor> LockedDoors = new List<Interaction_LockedDoor>();
  public float ShakeAmount = 0.2f;
  public float v1 = 0.4f;
  public float v2 = 0.7f;
  public Transform ShakeObject;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_LockedDoor.LockedDoors.Add(this);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_LockedDoor.LockedDoors.Remove(this);
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.Collider = this.GetComponentInChildren<BoxCollider2D>();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sOpenDoor = ScriptLocalization.Interactions.OpenDoor;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sOpenDoor;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, state.transform.position));
    this.StopAllCoroutines();
    if (!BiomeGenerator.Instance.HasKey)
      this.StartCoroutine((IEnumerator) this.DoShake());
    else
      Interaction_LockedDoor.OpenAll();
  }

  public static void OpenAll()
  {
    foreach (Interaction_LockedDoor lockedDoor in Interaction_LockedDoor.LockedDoors)
      lockedDoor.Open();
  }

  public void Open()
  {
    this.Activated = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.OpenRoutine());
  }

  public IEnumerator OpenRoutine()
  {
    Interaction_LockedDoor interactionLockedDoor = this;
    BiomeGenerator.Instance.HasKey = false;
    float Progress = 0.0f;
    Vector3 StartPosition = interactionLockedDoor.transform.localPosition;
    Vector3 TargetPosition = new Vector3(interactionLockedDoor.transform.localPosition.x, interactionLockedDoor.transform.localPosition.y, 2f);
    Vector3 Position = interactionLockedDoor.transform.localPosition;
    float x = interactionLockedDoor.transform.localPosition.x;
    while ((double) (Progress += Time.deltaTime) < (double) interactionLockedDoor.OpeningTime)
    {
      if ((double) Progress / (double) interactionLockedDoor.OpeningTime >= 0.5 && interactionLockedDoor.Collider.enabled)
        interactionLockedDoor.Collider.enabled = false;
      Position.z = Mathf.SmoothStep(StartPosition.z, TargetPosition.z, Progress / interactionLockedDoor.OpeningTime);
      interactionLockedDoor.transform.localPosition = Position;
      yield return (object) null;
    }
    interactionLockedDoor.transform.localPosition = new Vector3(x, interactionLockedDoor.transform.localPosition.y, Position.z);
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
