// Decompiled with JetBrains decompiler
// Type: Interaction_SoulDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_SoulDoor : Interaction
{
  public Interaction_SoulDoor.DoorTag MyDoorTag;
  public SimpleSetCamera SetCamera;
  private bool Activating;
  private bool Closing;
  public int Cost = 5;
  public float OpeningTime = 1f;
  public BoxCollider2D Collider;
  public GameObject PlayerGoTo;
  public GameObject DevotionTarget;
  private GameObject Player;
  private float Delay;
  private int SoulsInTheAir;
  public float ShakeAmount = 0.2f;
  public float v1 = 0.4f;
  public float v2 = 0.7f;
  public Transform ShakeObject;

  public int CurrentCount
  {
    set
    {
      typeof (DataManager).GetField(this.MyDoorTag.ToString() + "_Count").SetValue((object) DataManager.Instance, (object) value);
    }
    get
    {
      return (int) typeof (DataManager).GetField(this.MyDoorTag.ToString() + "_Count").GetValue((object) DataManager.Instance);
    }
  }

  private void Start()
  {
    this.ContinuouslyHold = true;
    if (!(bool) typeof (DataManager).GetField(this.MyDoorTag.ToString()).GetValue((object) DataManager.Instance))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void GetLabel()
  {
    string str;
    if (this.CurrentCount < this.Cost)
      str = $"{ScriptLocalization.Interactions.OpenDoor} <sprite name=\"icon_spirits\"> x{(object) (this.Cost - this.CurrentCount)} ({(object) Inventory.Souls})";
    else
      str = "";
    this.Label = str;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating || this.Closing)
      return;
    base.OnInteract(state);
    if (Inventory.Souls < this.Cost)
      MonoSingleton<Indicator>.Instance.PlayShake();
    else
      this.PayResources();
  }

  private void GetSoul()
  {
    --this.SoulsInTheAir;
    ++this.CurrentCount;
    if (this.CurrentCount < this.Cost)
      return;
    this.Closing = true;
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.state.transform.position));
    this.StopAllCoroutines();
    PlayerFarming.Instance.GoToAndStop(this.PlayerGoTo, this.ShakeObject.gameObject, GoToCallback: new System.Action(this.Activate));
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null || this.Closing || !this.Activating)
      return;
    if ((double) (this.Delay -= Time.deltaTime) < 0.0 && this.Activating && this.CurrentCount + this.SoulsInTheAir < this.Cost)
    {
      SoulCustomTarget.Create(this.DevotionTarget, this.state.gameObject.transform.position, Color.white, new System.Action(this.GetSoul));
      PlayerFarming.Instance.GetSoul(-1);
      this.Delay = 0.2f;
      ++this.SoulsInTheAir;
    }
    if (this.CurrentCount < this.Cost && Inventory.Souls > 0 && !InputManager.Gameplay.GetInteractButtonUp() && (double) Vector3.Distance(this.transform.position, this.Player.transform.position) <= (double) this.ActivateDistance)
      return;
    this.Activating = false;
  }

  private void Activate()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 7f);
    this.StartCoroutine((IEnumerator) this.UnlockDoorRoutine());
  }

  private void PayResources()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 7f);
    this.Closing = true;
    this.StopAllCoroutines();
    PlayerFarming.Instance.GoToAndStop(this.PlayerGoTo, this.ShakeObject.gameObject, GoToCallback: new System.Action(this.Activate));
  }

  public virtual IEnumerator UnlockDoorRoutine()
  {
    Interaction_SoulDoor interactionSoulDoor = this;
    interactionSoulDoor.Closing = true;
    int i = 0;
    if (interactionSoulDoor.Cost > 20)
      interactionSoulDoor.Cost = 20;
    while (++i <= interactionSoulDoor.Cost)
    {
      SoulCustomTarget.Create(interactionSoulDoor.ShakeObject.gameObject, interactionSoulDoor.state.gameObject.transform.position, Color.white, (System.Action) null);
      PlayerFarming.Instance.GetSoul(-1);
      yield return (object) new WaitForSeconds((float) (0.10000000149011612 - 0.10000000149011612 * (double) (i / interactionSoulDoor.Cost)));
    }
    yield return (object) new WaitForSeconds(0.5f);
    Debug.Log((object) "ORIGINAL!");
    interactionSoulDoor.SetCamera.Play();
    yield return (object) new WaitForSeconds(interactionSoulDoor.SetCamera.Duration + 0.5f);
    typeof (DataManager).GetField(interactionSoulDoor.MyDoorTag.ToString()).SetValue((object) DataManager.Instance, (object) true);
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(interactionSoulDoor.transform.position, interactionSoulDoor.state.transform.position));
    interactionSoulDoor.Collider.enabled = false;
    interactionSoulDoor.ShakeObject.gameObject.SetActive(false);
    interactionSoulDoor.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    interactionSoulDoor.SetCamera.Reset();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionSoulDoor.gameObject);
  }

  private IEnumerator DoShake()
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

  public enum DoorTag
  {
    BaseDoorNorthEast,
    BaseDoorNorthWest,
    BossForest,
    ForestTempleDoor,
    ShrineDoor,
  }
}
