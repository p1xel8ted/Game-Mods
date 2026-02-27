// Decompiled with JetBrains decompiler
// Type: PickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PickUp : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE type;
  public int quantity = 1;
  public AudioClip PickUpClip;
  private float Scale;
  private float ScaleSpeed;
  private Vector2 SquishScale = Vector2.one;
  private Vector2 SquishScaleSpeed = Vector2.zero;
  private float FacingAngle = -1f;
  public float Speed;
  private float vx;
  private float vy;
  private Collider2D m_Collider;
  private float Timer;
  public bool PlayerDeath;
  public bool MagnetToPlayer = true;
  public bool CanBePickedUp = true;
  public GameObject child;
  private float vz;
  private float childZ;
  public bool CanStopFollowingPlayer = true;
  public bool CanBeStolenByCritter = true;
  public UnityEvent Callback;
  public bool Bounce = true;
  public bool EmitPickUpFX = true;
  [HideInInspector]
  public GameObject Reserved;
  public static List<PickUp> PickUps = new List<PickUp>();
  public bool DestroysAfterDelay = true;
  private float destroyTimer;
  private const float destroyDelay = 60f;
  public bool DisableSeperation;
  public InventoryItemDisplay inventoryItemDisplay;
  private bool Collected;
  public bool AddToChestIfNotCollected;
  public float MagnetDistance = 7f;
  public Vector3 Seperator = Vector3.zero;
  private float SeperationRadius = 0.5f;
  private bool SetInitialSpeed;
  private GameObject Player;
  public bool AddToInventory = true;

  public bool StolenByCritter { get; private set; }

  public event PickUp.PickUpEvent OnPickedUp;

  private void Start()
  {
    if ((double) this.transform.position.z == 0.0)
    {
      this.Scale = 0.0f;
      this.childZ = -0.5f;
    }
    else
    {
      this.Scale = 0.8f;
      this.childZ = this.transform.position.z;
      this.transform.position = this.transform.position with
      {
        z = 0.0f
      };
    }
    this.vz = UnityEngine.Random.Range(-0.3f, -0.15f);
    this.FacingAngle = (double) this.FacingAngle == -1.0 ? (float) UnityEngine.Random.Range(0, 360) : this.FacingAngle;
  }

  public void SetInitialSpeedAndDiraction(float Speed, float FacingAngle)
  {
    this.Speed = Speed;
    this.FacingAngle = FacingAngle;
  }

  public void SetInitialFacing(float FacingAngle) => this.FacingAngle = FacingAngle;

  public void SetImage(InventoryItem.ITEM_TYPE Item)
  {
    if (!((UnityEngine.Object) this.inventoryItemDisplay != (UnityEngine.Object) null))
      return;
    this.inventoryItemDisplay.SetImage(Item);
  }

  private void OnEnable()
  {
    this.MagnetDistance = 7f;
    this.StolenByCritter = false;
    PickUp.PickUps.Add(this);
    this.Timer = 0.0f;
    this.SquishScale = Vector2.one;
    this.SquishScaleSpeed = Vector2.zero;
    this.Activated = false;
    this.childZ = 0.0f;
    this.destroyTimer = 0.0f;
    this.child.transform.localScale = Vector3.zero;
    this.m_Collider = this.GetComponent<Collider2D>();
    this.Speed = 0.0f;
    if (this.Collected)
    {
      if ((double) this.transform.position.z == 0.0)
      {
        this.Scale = 0.0f;
        this.childZ = -0.5f;
      }
      else
      {
        this.Scale = 0.8f;
        this.childZ = this.transform.position.z;
        this.transform.position = this.transform.position with
        {
          z = 0.0f
        };
      }
      this.vz = UnityEngine.Random.Range(-0.3f, -0.15f);
      this.Speed = UnityEngine.Random.Range(5f, 10f);
      this.FacingAngle = (double) this.FacingAngle == -1.0 ? (float) UnityEngine.Random.Range(0, 360) : this.FacingAngle;
      this.Collected = false;
    }
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  private void OnDisable()
  {
    PickUp.PickUps.Remove(this);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  private void OnChestRevealed()
  {
    if (this.StolenByCritter)
      return;
    this.MagnetDistance = 100f;
  }

  private void OnRecycle()
  {
    if (!this.AddToChestIfNotCollected || this.Collected || PlayerFarming.Location != FollowerLocation.Base || !ObjectPool.IsSpawned(this.gameObject))
      return;
    List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(PlayerFarming.Location);
    if (structuresOfType.Count <= 0)
      return;
    structuresOfType[0].AddItem(this.type, 1);
  }

  private void OnDestroy()
  {
  }

  public static bool UnreservedExists(InventoryItem.ITEM_TYPE type)
  {
    foreach (PickUp pickUp in PickUp.PickUps)
    {
      if (pickUp.type == type && (UnityEngine.Object) pickUp.Reserved == (UnityEngine.Object) null)
        return true;
    }
    return false;
  }

  public static PickUp UnreservedAnyExists()
  {
    foreach (PickUp pickUp in PickUp.PickUps)
    {
      if ((UnityEngine.Object) pickUp.Reserved == (UnityEngine.Object) null)
        return pickUp;
    }
    return (PickUp) null;
  }

  public static List<PickUp> UnreservedAnyExistsAll()
  {
    List<PickUp> pickUpList = new List<PickUp>();
    foreach (PickUp pickUp in PickUp.PickUps)
    {
      if ((UnityEngine.Object) pickUp.Reserved == (UnityEngine.Object) null)
        pickUpList.Add(pickUp);
    }
    return pickUpList;
  }

  private void Update()
  {
    if ((double) this.Timer < 3.0 && this.Bounce)
      this.BounceChild();
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    this.destroyTimer += Time.deltaTime;
    if ((double) this.destroyTimer < 60.0 || !this.DestroysAfterDelay)
      return;
    this.OnRecycle();
    this.gameObject.Recycle();
  }

  private void FixedUpdate()
  {
    if ((double) this.Timer < 3.0)
    {
      if (!this.Bounce)
      {
        this.Scale += (float) ((1.0 - (double) this.Scale) / 7.0);
        this.SquishScaleSpeed.x += (float) ((1.0 - (double) this.SquishScale.x) * 0.20000000298023224);
        this.SquishScale.x += (this.SquishScaleSpeed.x *= 0.9f);
        this.SquishScaleSpeed.y += (float) ((1.0 - (double) this.SquishScale.y) * 0.20000000298023224);
        this.SquishScale.y += (this.SquishScaleSpeed.y *= 0.9f);
        if ((double) Time.timeScale > 0.0)
          this.child.transform.localScale = new Vector3(this.Scale * this.SquishScale.x, this.Scale * this.SquishScale.y, this.Scale);
      }
      else
      {
        this.Scale += (float) ((1.0 - (double) this.Scale) / 7.0);
        this.SquishScaleSpeed.x += (float) ((1.0 - (double) this.SquishScale.x) * 0.20000000298023224);
        this.SquishScale.x += (this.SquishScaleSpeed.x *= 0.9f);
        this.SquishScaleSpeed.y += (float) ((1.0 - (double) this.SquishScale.y) * 0.20000000298023224);
        this.SquishScale.y += (this.SquishScaleSpeed.y *= 0.9f);
        if ((double) Time.timeScale > 0.0)
          this.child.transform.localScale = new Vector3(this.Scale * this.SquishScale.x, this.Scale * this.SquishScale.y, this.Scale);
      }
    }
    if (this.PlayerDeath)
    {
      this.Speed += (float) ((0.0 - (double) this.Speed) / 12.0 / ((double) Time.fixedUnscaledDeltaTime * 60.0));
      this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
      this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
      this.transform.position = this.transform.position + new Vector3(this.vx, this.vy) * Time.fixedUnscaledDeltaTime;
    }
    else
    {
      if (!this.DisableSeperation)
      {
        this.Seperator = (Vector3) Vector2.zero;
        for (int index = 0; index < PickUp.PickUps.Count; ++index)
        {
          if (!((UnityEngine.Object) PickUp.PickUps[index] == (UnityEngine.Object) null) && !((UnityEngine.Object) PickUp.PickUps[index] == (UnityEngine.Object) this) && !PickUp.PickUps[index].DisableSeperation)
          {
            float distanceBetween = this.MagnitudeFindDistanceBetween((Vector2) PickUp.PickUps[index].transform.position, (Vector2) this.transform.position);
            if ((double) distanceBetween < (double) this.SeperationRadius)
            {
              float num1 = Time.fixedDeltaTime * 60f;
              float angleR = Utils.GetAngleR(PickUp.PickUps[index].transform.position, this.transform.position);
              float num2 = (float) (((double) this.SeperationRadius - (double) distanceBetween) / 2.0);
              this.Seperator.x += num2 * Mathf.Cos(angleR) * num1;
              this.Seperator.y += num2 * Mathf.Sin(angleR) * num1;
              PickUp.PickUps[index].Seperator.x -= num2 * Mathf.Cos(angleR) * num1;
              PickUp.PickUps[index].Seperator.y -= num2 * Mathf.Sin(angleR) * num1;
            }
          }
        }
      }
      if ((double) (this.Timer += Time.fixedDeltaTime) > 0.30000001192092896)
        this.gameObject.layer = LayerMask.NameToLayer("Pick Ups");
      if ((double) this.Timer > 0.699999988079071)
      {
        if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
            this.Player = PlayerFarming.Instance.gameObject;
        }
        else if (this.MagnetToPlayer)
        {
          if (this.Activated && !this.SetInitialSpeed)
          {
            this.Speed = -5f;
            this.SetInitialSpeed = true;
          }
          float num = Vector3.Distance(this.transform.position, this.Player.transform.position);
          if ((double) num < (double) this.MagnetDistance)
          {
            if ((double) this.Speed < 45.0)
              this.Speed += 45f * Time.fixedDeltaTime;
            this.FacingAngle = Utils.GetAngle(this.transform.position, this.Player.transform.position);
            if ((double) num < 0.5 && this.CanBePickedUp)
            {
              CameraManager.shakeCamera(0.2f, Utils.GetAngle(this.transform.position, this.Player.transform.position));
              this.PickMeUp();
            }
            if (!this.Activated)
              this.IgnoreCollision(true);
            this.Activated = true;
            this.childZ = Mathf.Lerp(this.childZ, -0.5f, 5f * Time.fixedDeltaTime);
            this.child.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
            if (!this.CanStopFollowingPlayer)
              this.MagnetDistance = (float) int.MaxValue;
          }
          else
          {
            this.Activated = false;
            this.IgnoreCollision(false);
          }
        }
        else if (this.CanBePickedUp && (double) Vector3.Distance(this.transform.position, this.Player.transform.position) < 0.5)
        {
          CameraManager.shakeCamera(0.2f, Utils.GetAngle(this.transform.position, this.Player.transform.position));
          this.PickMeUp();
        }
      }
      if (this.PlayerDeath)
        return;
      if (!this.Activated)
        this.Speed += (float) ((0.0 - (double) this.Speed) / 12.0) * GameManager.FixedDeltaTime;
      this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
      this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
      this.transform.position += new Vector3(this.vx, this.vy) * Time.fixedDeltaTime + this.Seperator * Time.fixedDeltaTime;
    }
  }

  private void LateUpdate()
  {
    RaycastHit hitInfo;
    if (Physics.Raycast(this.transform.position + Vector3.back * 3f, Vector3.forward, out hitInfo, float.PositiveInfinity))
    {
      if (!((UnityEngine.Object) hitInfo.collider.gameObject.GetComponent<MeshCollider>() != (UnityEngine.Object) null))
        return;
      this.transform.position = this.transform.position with
      {
        z = hitInfo.point.z
      };
    }
    else
      this.transform.position = this.transform.position with
      {
        z = 0.0f
      };
  }

  public bool Activated { get; private set; }

  private void BounceChild()
  {
    if ((double) this.childZ >= 0.0)
    {
      if ((double) this.vz > 0.079999998211860657)
      {
        this.vz *= -0.4f;
        this.SquishScale = new Vector2(0.8f, 1.2f);
      }
      else
        this.vz = 0.0f;
      this.childZ = 0.0f;
    }
    else if (!this.Activated)
      this.vz += (float) (0.019999999552965164 * (double) Time.deltaTime * 60.0);
    this.childZ += (float) ((double) this.vz * (double) Time.deltaTime * 60.0);
    this.child.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
  }

  public void PickMeUpSimpleInventory(GameObject playerGO)
  {
    if (this.Collected)
      return;
    PlayerSimpleInventory simpleInventory = playerGO.GetComponent<PlayerFarming>().simpleInventory;
    simpleInventory.DropItem();
    simpleInventory.GiveItem(this.type);
    this.Collected = true;
    this.FacingAngle = -1f;
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this.gameObject.Recycle())));
  }

  public virtual void PickMeUp()
  {
    if (this.Collected)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.gameObject);
    if (this.EmitPickUpFX)
    {
      BiomeConstants.Instance.EmitPickUpVFX(this.child.transform.position);
      string[] strArray = new string[2]
      {
        "BloodImpact_Small_0",
        "BloodImpact_Small_1"
      };
      int index = UnityEngine.Random.Range(0, strArray.Length - 1);
      if (strArray[index] != null)
        BiomeConstants.Instance.EmitBloodImpact(this.child.transform.position, Quaternion.identity.x, "black", strArray[index]);
    }
    this.Callback?.Invoke();
    if (this.AddToInventory)
      Inventory.AddItem((int) this.type, 1);
    this.PickedUp();
    this.Collected = true;
    this.FacingAngle = -1f;
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this.gameObject.Recycle())));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void PickedUp()
  {
    PickUp.PickUpEvent onPickedUp = this.OnPickedUp;
    if (onPickedUp == null)
      return;
    onPickedUp(this);
  }

  public void IgnoreCollision(bool Toggle)
  {
  }

  public void TargetedByCritter()
  {
    this.StolenByCritter = true;
    this.MagnetDistance = 7f;
  }

  private float MagnitudeFindDistanceBetween(Vector2 a, Vector2 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    return (float) (num1 * num1 + (double) num2 * (double) num2);
  }

  public delegate void PickUpEvent(PickUp pickUp);
}
