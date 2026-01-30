// Decompiled with JetBrains decompiler
// Type: FlyingCrown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class FlyingCrown : MonoBehaviour
{
  public bool Active;
  public GameObject StartPosition;
  public GameObject TargetPosition;
  public SkeletonAnimation Spine;
  public MeshRenderer SpineMeshRenderer;
  public Transform ParentTransform;
  public SkeletonAnimation PlayerSpine;
  [SpineAttachment(true, false, false, "", "", "", true, false, dataField = "PlayerSpine")]
  public string CrownAttachment;
  [SpineSlot("", "", false, true, false, dataField = "PlayerSpine")]
  public string CrownAttachmentSlot;
  public Transform FlyingTransform;
  [SpineAttachment(true, false, false, "", "", "", true, false, dataField = "PlayerSpine")]
  public string CrownEyeAttachment;
  [SpineSlot("", "", false, true, false, dataField = "PlayerSpine")]
  public string CrownEyeAttachmentSlot;
  public Vector2 StartingTurnSpeed = new Vector2(10f, 15f);
  public float StartingTurnAcceleration = 20f;
  public float TurnAcceleration = 20f;
  public float StartingSpeed = 10f;
  public float StartingDelay = 0.5f;
  public float Acceleration;
  public float StartingAcceleration = 0.01f;
  public float MaxSpeed;
  public float StartingMaxSpeed = 5f;
  public float ZSpeed = 1f;
  public float StartingZSpeed = 1f;
  public float TargetDistance = 0.025f;
  public PlayerFarming playerFarming;
  public float Speed;
  public float Timer;
  public float Delay;
  public float TargetAngle;
  public float Angle;
  public float TurnSpeed;
  public float ZPosition;
  public float ZTime;
  public float StartingZPosition;
  public bool Hiding;

  public void Start()
  {
    this.gameObject.SetActive(false);
    this.PlayerSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.Close);
    this.playerFarming.OnCrownReturn += new System.Action(this.Play);
    this.playerFarming.OnCrownReturnSubtle += new System.Action(this.PlayWeapon);
    this.playerFarming.OnHideCrown += new System.Action(this.Hide);
    if (this.playerFarming.isLamb)
    {
      string skinName = "Lamb";
      if (DataManager.Instance.PlayerVisualFleece == 676)
        skinName = "Cowboy";
      this.Spine.initialSkinName = skinName;
      this.Spine.skeleton.SetSkin(skinName);
    }
    else
    {
      this.Spine.initialSkinName = "Goat";
      this.Spine.skeleton.SetSkin("Goat");
    }
  }

  public void OnDestroy()
  {
    this.PlayerSpine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.playerFarming.OnCrownReturn -= new System.Action(this.Play);
    this.playerFarming.OnCrownReturnSubtle -= new System.Action(this.PlayWeapon);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.Close);
    this.playerFarming.OnHideCrown -= new System.Action(this.Hide);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "CROWN_HIDE_CANCEL":
        this.Close();
        break;
      case "CROWN_HIDE":
        this.PlayWeapon();
        break;
    }
  }

  public void Play() => this.PlayWeapon();

  public void PlayWeapon()
  {
    this.Delay = 0.0f;
    this.Angle = (float) UnityEngine.Random.Range(0, 360);
    this.Speed = 1f;
    this.TurnSpeed = UnityEngine.Random.Range(10f, 15f);
    this.TurnAcceleration = 5f;
    this.Acceleration = 10f;
    this.MaxSpeed = 100f;
    this.ZSpeed = 2f;
    this.InitPlay();
  }

  public void InitPlay()
  {
    this.transform.DOKill();
    this.StopAllCoroutines();
    if ((UnityEngine.Object) this.ParentTransform == (UnityEngine.Object) null)
      return;
    this.transform.parent = this.ParentTransform.parent;
    this.transform.localScale = Vector3.one;
    this.transform.localRotation = (Quaternion) quaternion.identity;
    this.Active = true;
    this.transform.position = this.StartPosition.transform.position;
    this.gameObject.SetActive(true);
    this.Timer = 0.0f;
    this.Spine.AnimationState.SetAnimation(0, "return", false);
    this.SpineMeshRenderer.enabled = true;
    this.PlayerSpine.Skeleton.SetAttachment("CROWN", (string) null);
    this.PlayerSpine.Skeleton.SetAttachment("CROWN_EYE", (string) null);
    this.StartingZPosition = this.StartPosition.transform.position.z;
    this.ZTime = 0.0f;
  }

  public IEnumerator AnimateAndClose()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlyingCrown flyingCrown = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      flyingCrown.Close();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    flyingCrown.Active = false;
    flyingCrown.transform.parent = flyingCrown.TargetPosition.transform;
    flyingCrown.Spine.AnimationState.SetAnimation(0, "land", false);
    flyingCrown.transform.DOLocalMove(Vector3.zero, 0.25f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Close()
  {
    if ((UnityEngine.Object) this.PlayerSpine != (UnityEngine.Object) null)
    {
      this.PlayerSpine.Skeleton.SetAttachment(this.CrownAttachmentSlot, this.CrownAttachment);
      this.PlayerSpine.Skeleton.SetAttachment(this.CrownEyeAttachmentSlot, this.CrownEyeAttachment);
    }
    this.gameObject.SetActive(false);
    this.Active = false;
    this.Hiding = false;
  }

  public void Hide()
  {
    Debug.Log((object) "HIDE!");
    this.Hiding = true;
    this.StopAllCoroutines();
    this.Active = false;
    this.playerFarming.StartCoroutine((IEnumerator) this.HideCrown());
    this.gameObject.SetActive(false);
  }

  public IEnumerator HideCrown()
  {
    Debug.Log((object) "routine: HideCrown");
    while (this.Hiding)
    {
      Debug.Log((object) "Hiding".Colour(Color.yellow));
      if ((UnityEngine.Object) this.PlayerSpine != (UnityEngine.Object) null)
      {
        this.PlayerSpine.Skeleton.SetAttachment(this.CrownAttachmentSlot, (string) null);
        this.PlayerSpine.Skeleton.SetAttachment(this.CrownEyeAttachmentSlot, (string) null);
      }
      yield return (object) null;
    }
  }

  public void Update()
  {
    if (!this.Active)
      return;
    if ((UnityEngine.Object) this.TargetPosition == (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.Delay -= Time.deltaTime;
      if ((double) this.Delay <= 0.0)
        this.TurnSpeed = this.Lerp(this.TurnSpeed, 1f, this.TurnAcceleration * Time.deltaTime);
      this.TargetAngle = Utils.GetAngle(this.transform.position, this.TargetPosition.transform.position);
      this.Angle += Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0)))) * 57.29578f / this.TurnSpeed;
      if ((double) this.Speed < (double) this.MaxSpeed)
        this.Speed += this.Acceleration * Time.deltaTime;
      this.Speed = Mathf.Clamp(this.Speed, 0.0f, this.MaxSpeed);
      this.ZPosition = Mathf.Lerp(this.StartingZPosition, this.TargetPosition.transform.position.z, Mathf.SmoothStep(0.0f, 1f, this.ZTime += this.ZSpeed * Time.deltaTime));
      this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.ZPosition) + new Vector3(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f))) * Time.deltaTime;
      this.PlayerSpine.Skeleton.SetAttachment("CROWN", (string) null);
      this.PlayerSpine.Skeleton.SetAttachment("CROWN_EYE", (string) null);
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Meditate)
        this.StartCoroutine((IEnumerator) this.AnimateAndClose());
      if ((double) FlyingCrown.Distance(this.transform.position, this.TargetPosition.transform.position) >= (double) this.TargetDistance)
        return;
      this.StartCoroutine((IEnumerator) this.AnimateAndClose());
    }
  }

  public float Lerp(float firstFloat, float secondFloat, float by)
  {
    return firstFloat + (secondFloat - firstFloat) * by;
  }

  public static float Distance(Vector3 a, Vector3 b) => (a - b).sqrMagnitude;
}
