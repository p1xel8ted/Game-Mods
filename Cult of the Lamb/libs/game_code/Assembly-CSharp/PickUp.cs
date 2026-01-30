// Decompiled with JetBrains decompiler
// Type: PickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PickUp : BaseMonoBehaviour
{
  public const float STOP_SEPERATION_DELAY = 2f;
  public const float START_SEPERATION_DELAY = 0.3f;
  public InventoryItem.ITEM_TYPE type;
  public int quantity = 1;
  public AudioClip PickUpClip;
  public float Scale;
  public float ScaleSpeed;
  public Vector2 SquishScale = Vector2.one;
  public Vector2 SquishScaleSpeed = Vector2.zero;
  public float FacingAngle = -1f;
  public float Speed;
  public float vx;
  public float vy;
  public Collider2D m_Collider;
  public float Timer;
  public bool PlayerDeath;
  public bool MagnetToPlayer = true;
  public bool CanBePickedUp = true;
  public GameObject child;
  public float vz;
  public float childZ;
  public bool CanStopFollowingPlayer = true;
  public bool CanBeStolenByCritter = true;
  [CompilerGenerated]
  public bool \u003CStolenByCritter\u003Ek__BackingField;
  public UnityEvent Callback;
  public bool Bounce = true;
  public bool EmitPickUpFX = true;
  [HideInInspector]
  public GameObject Reserved;
  public static List<PickUp> PickUps = new List<PickUp>();
  public bool DestroysAfterDelay = true;
  public bool ScaleIn = true;
  public float destroyTimer;
  public const float destroyDelay = 60f;
  public bool DisableSeperation;
  [SerializeField]
  public bool moveDuringSeparation = true;
  public float seperationTimer;
  public float initialSeperationTimer = 0.3f;
  public bool WasMovedByOther;
  public bool ignoreBoundsCheck;
  [CompilerGenerated]
  public GameObject \u003CPlayer\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CPlayerOverride\u003Ek__BackingField;
  public bool isPlayerTarget;
  public InventoryItemDisplay inventoryItemDisplay;
  public bool Collected;
  public bool AddToChestIfNotCollected;
  public float MagnetDistance = 7f;
  public Vector3 Seperator = Vector3.zero;
  [SerializeField]
  public float SeperationRadius = 0.5f;
  public bool isSeperated;
  public bool SetInitialSpeed;
  [CompilerGenerated]
  public bool \u003CActivated\u003Ek__BackingField;
  public bool AddToInventory = true;

  public int Quantity
  {
    get => this.quantity;
    set => this.quantity = Mathf.Max(value, 0);
  }

  public bool StolenByCritter
  {
    get => this.\u003CStolenByCritter\u003Ek__BackingField;
    set => this.\u003CStolenByCritter\u003Ek__BackingField = value;
  }

  public event PickUp.PickUpEvent OnPickedUp;

  public GameObject Player
  {
    get => this.\u003CPlayer\u003Ek__BackingField;
    set => this.\u003CPlayer\u003Ek__BackingField = value;
  }

  public bool PlayerOverride
  {
    get => this.\u003CPlayerOverride\u003Ek__BackingField;
    set => this.\u003CPlayerOverride\u003Ek__BackingField = value;
  }

  public void Start()
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
    this.m_Collider = this.GetComponent<Collider2D>();
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

  public void OnEnable()
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
    this.seperationTimer = 0.0f;
    this.initialSeperationTimer = 0.3f;
    this.WasMovedByOther = false;
    if ((UnityEngine.Object) this.child != (UnityEngine.Object) null && this.ScaleIn)
      this.child.transform.localScale = Vector3.zero;
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
    if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
      GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
      {
        if (!GameManager.IsDungeon(PlayerFarming.Location) || !((UnityEngine.Object) this != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || !this.enabled || this.ignoreBoundsCheck)
          return;
        Vector3 closestPoint = this.transform.position;
        BiomeGenerator.PointWithinIsland(this.transform.position, out closestPoint);
        this.transform.position = closestPoint;
      }));
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  public void OnDisable()
  {
    PickUp.PickUps.Remove(this);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.OnChestRevealed);
    this.ignoreBoundsCheck = false;
  }

  public void OnChestRevealed()
  {
    if (this.StolenByCritter)
      return;
    this.MagnetDistance = 100f;
  }

  public void OnRecycle()
  {
    if (this.AddToChestIfNotCollected && !this.Collected && PlayerFarming.Location == FollowerLocation.Base && ObjectPool.IsSpawned(this.gameObject))
    {
      List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(in PlayerFarming.Location);
      if (structuresOfType.Count > 0)
        structuresOfType[0].AddItem(this.type, this.quantity);
    }
    this.quantity = 1;
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

  public void Update()
  {
    this.initialSeperationTimer -= Time.deltaTime;
    if ((double) this.Timer < 3.0 && this.Bounce)
      this.BounceChild();
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    this.destroyTimer += Time.deltaTime;
    if ((double) this.destroyTimer < 60.0 || !this.DestroysAfterDelay)
      return;
    this.OnRecycle();
    this.Player = (GameObject) null;
    this.gameObject.Recycle();
  }

  public void FixedUpdate()
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
        this.child.transform.localScale = (double) Time.timeScale <= 0.0 || !this.ScaleIn ? Vector3.one : new Vector3(this.Scale * this.SquishScale.x, this.Scale * this.SquishScale.y, this.Scale);
      }
      else
      {
        this.Scale += (float) ((1.0 - (double) this.Scale) / 7.0);
        this.SquishScaleSpeed.x += (float) ((1.0 - (double) this.SquishScale.x) * 0.20000000298023224);
        this.SquishScale.x += (this.SquishScaleSpeed.x *= 0.9f);
        this.SquishScaleSpeed.y += (float) ((1.0 - (double) this.SquishScale.y) * 0.20000000298023224);
        this.SquishScale.y += (this.SquishScaleSpeed.y *= 0.9f);
        this.child.transform.localScale = (double) Time.timeScale <= 0.0 || !this.ScaleIn ? Vector3.one : new Vector3(this.Scale * this.SquishScale.x, this.Scale * this.SquishScale.y, this.Scale);
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
        this.CheckSeperationOptimized();
      if ((double) this.initialSeperationTimer < 0.0)
        this.seperationTimer += Time.fixedDeltaTime;
      if ((double) (this.Timer += Time.fixedDeltaTime) > 0.30000001192092896)
        this.gameObject.layer = LayerMask.NameToLayer("Pick Ups");
      if ((double) this.Timer > 0.699999988079071)
      {
        if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
        {
          this.isPlayerTarget = false;
          if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
          {
            this.isPlayerTarget = true;
            this.Player = PlayerFarming.Instance.gameObject;
          }
        }
        else
        {
          if (this.isPlayerTarget && PlayerFarming.playersCount > 1 && !this.PlayerOverride)
            this.Player = PlayerFarming.FindClosestPlayer(this.transform.position, true).gameObject;
          if (this.MagnetToPlayer)
          {
            if (this.Activated && !this.SetInitialSpeed)
            {
              this.Speed = -5f;
              this.SetInitialSpeed = true;
            }
            float num = Vector3.Distance(new Vector3(this.transform.position.x, this.transform.position.y, this.Player.transform.position.z), this.Player.transform.position);
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

  public void CheckSeperation()
  {
    this.Seperator = (Vector3) Vector2.zero;
    for (int index = 0; index < PickUp.PickUps.Count; ++index)
    {
      if (!((UnityEngine.Object) PickUp.PickUps[index] == (UnityEngine.Object) null) && !((UnityEngine.Object) PickUp.PickUps[index] == (UnityEngine.Object) this) && !PickUp.PickUps[index].DisableSeperation && this.moveDuringSeparation)
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

  public void CheckSeperationOptimized()
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
          double num2 = ((double) this.SeperationRadius - (double) distanceBetween) / 2.0;
          float num3 = (float) (num2 * (double) Mathf.Cos(angleR) * (double) num1 * 2.0);
          float num4 = (float) (num2 * (double) Mathf.Sin(angleR) * (double) num1 * 2.0);
          this.Seperator.x += num3;
          this.Seperator.y += num4;
          PickUp.PickUps[index].Seperator.x -= num3;
          PickUp.PickUps[index].Seperator.y -= num4;
          PickUp.PickUps[index].WasMovedByOther = true;
        }
      }
    }
  }

  public void LateUpdate()
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

  public bool Activated
  {
    get => this.\u003CActivated\u003Ek__BackingField;
    set => this.\u003CActivated\u003Ek__BackingField = value;
  }

  public void BounceChild()
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
      Inventory.AddItem((int) this.type, this.quantity);
    this.PickedUp();
    this.Collected = true;
    this.FacingAngle = -1f;
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      this.Player = (GameObject) null;
      this.gameObject.Recycle();
    })));
  }

  public IEnumerator FrameDelay(System.Action callback)
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

  public float MagnitudeFindDistanceBetween(Vector2 a, Vector2 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    return (float) (num1 * num1 + (double) num2 * (double) num2);
  }

  public void SetIgnoreBoundsCheck() => this.ignoreBoundsCheck = true;

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__63_0()
  {
    if (!GameManager.IsDungeon(PlayerFarming.Location) || !((UnityEngine.Object) this != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || !this.enabled || this.ignoreBoundsCheck)
      return;
    Vector3 closestPoint = this.transform.position;
    BiomeGenerator.PointWithinIsland(this.transform.position, out closestPoint);
    this.transform.position = closestPoint;
  }

  [CompilerGenerated]
  public void \u003CPickMeUpSimpleInventory\u003Eb__87_0() => this.gameObject.Recycle();

  [CompilerGenerated]
  public void \u003CPickMeUp\u003Eb__89_0()
  {
    this.Player = (GameObject) null;
    this.gameObject.Recycle();
  }

  public delegate void PickUpEvent(PickUp pickUp);
}
