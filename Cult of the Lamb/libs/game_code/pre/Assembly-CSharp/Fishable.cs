// Decompiled with JetBrains decompiler
// Type: Fishable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class Fishable : BaseMonoBehaviour
{
  [SerializeField]
  private SpriteRenderer spriteRender;
  [SerializeField]
  private float fishMoveSpeed = 0.15f;
  [SerializeField]
  private float fishRotationSpeed = 0.35f;
  [SerializeField]
  private float targetHookMoveSpeed;
  [Space]
  [SerializeField]
  [Range(0.0f, 1f)]
  private float hookTargetAngle;
  [SerializeField]
  private float hookTargetingDistance;
  [SerializeField]
  private float hookSplashDistance = 3f;
  [SerializeField]
  private float biteHookDistance = 0.3f;
  [SerializeField]
  private float fishInterestMaxDistance;
  public StateMachine.State state;
  private Interaction_Fishing fishing;
  private Vector3 originalCenterPoint;
  private float moveSpeed;
  private float rotationSpeed;
  private const float moveRadius = 2.5f;
  private float moveTimer;
  private Vector2 timeInterval = new Vector2(1f, 3f);
  private Vector3 targetPosition;
  private Vector3 targetLookAt;
  private Vector3 playerPosition;
  private bool chasing;

  public Interaction_Fishing.FishType FishType { get; private set; }

  public InventoryItem.ITEM_TYPE ItemType => this.FishType.Type;

  public int Quantity => this.FishType.Quantity;

  public void Configure(Interaction_Fishing.FishType fishType, Interaction_Fishing fishingParent)
  {
    this.FishType = fishType;
    this.fishing = fishingParent;
    this.fishing.OnCasted += new Interaction_Fishing.FishEvent(this.HookLanded);
    this.transform.localScale = Vector3.one * Random.Range(fishType.Scale.x, fishType.Scale.y);
    this.originalCenterPoint = new Vector3(fishingParent.transform.position.x, this.transform.position.y, 0.0f);
    this.state = this.GetComponent<StateMachine>().CURRENT_STATE;
    this.moveSpeed = this.fishMoveSpeed;
    this.rotationSpeed = this.fishRotationSpeed;
  }

  private void OnDestroy()
  {
    if (!((Object) this.fishing != (Object) null))
      return;
    this.fishing.OnCasted -= new Interaction_Fishing.FishEvent(this.HookLanded);
  }

  private void OnDisable()
  {
    if (!((Object) this.fishing != (Object) null))
      return;
    this.fishing.OnCasted -= new Interaction_Fishing.FishEvent(this.HookLanded);
  }

  private void HookLanded()
  {
    if ((double) Vector3.Distance(this.fishing.FishingHook.position, this.transform.position) >= (double) this.hookSplashDistance || !this.fishing.IsClosestFish(this))
      return;
    this.TargetedHook();
  }

  public void FadeIn()
  {
    this.spriteRender.material.color = new Color(this.spriteRender.material.color.r, this.spriteRender.material.color.g, this.spriteRender.material.color.b, 0.0f);
    this.spriteRender.material.DOFade(1f, 1f);
  }

  private void Update()
  {
    if (this.state == StateMachine.State.Idle)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float moveTimer = this.moveTimer;
      if ((double) currentTime.GetValueOrDefault() > (double) moveTimer & currentTime.HasValue)
        this.SetRandomTargetPosition(2.5f);
    }
    if (this.state == StateMachine.State.Attacking)
    {
      this.transform.position = this.fishing.FishingHook.position;
      this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0.0f, 0.0f, Utils.GetAngle(this.playerPosition, this.transform.position)), this.rotationSpeed * Time.deltaTime);
    }
    else
    {
      this.transform.position = Vector3.Lerp(this.transform.position, this.targetPosition, this.moveSpeed * (this.fishInterestMaxDistance - Vector3.Distance(this.transform.position, this.targetPosition)) * Time.deltaTime);
      this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0.0f, 0.0f, Utils.GetAngle(this.targetLookAt, this.transform.position)), this.rotationSpeed * Time.deltaTime);
    }
    if (!((Object) this.fishing != (Object) null) || this.fishing.state.CURRENT_STATE != StateMachine.State.Reeling)
      return;
    float num = Vector3.Distance(this.transform.position, this.fishing.FishingHook.position);
    if ((double) num < (double) this.hookTargetingDistance)
      this.TargetedHook();
    else if (this.chasing)
    {
      this.targetPosition = this.fishing.FishingHook.position;
      this.targetLookAt = this.fishing.FishingHook.position;
    }
    if ((double) num < (double) this.biteHookDistance)
    {
      this.Hooked();
    }
    else
    {
      if ((double) num <= (double) this.fishInterestMaxDistance || !this.fishing.FishChasing)
        return;
      this.fishing.FishChasing = false;
      this.chasing = false;
      this.SetRandomTargetPosition(2.5f);
      this.moveSpeed = this.fishMoveSpeed;
    }
  }

  private void TargetedHook()
  {
    this.targetLookAt = this.fishing.FishingHook.position;
    this.targetPosition = this.fishing.FishingHook.position;
    this.fishing.FishChasing = true;
    this.chasing = true;
    this.moveSpeed = this.targetHookMoveSpeed;
  }

  private void Hooked()
  {
    this.fishing.FishOn(this);
    this.state = StateMachine.State.Attacking;
    this.playerPosition = this.fishing.PlayerPosition;
  }

  public void Spooked()
  {
    if (this.chasing)
    {
      this.fishing.FishChasing = false;
      this.chasing = false;
    }
    this.SetRandomTargetPosition(10f);
    this.moveSpeed = 1f;
    this.rotationSpeed = 1f;
    this.transform.DOScale(0.0f, 2f);
    this.state = StateMachine.State.Fleeing;
  }

  private void SetRandomTargetPosition(float r)
  {
    this.targetPosition = this.originalCenterPoint + (Vector3) (Random.insideUnitCircle * r);
    this.targetLookAt = this.targetPosition;
    this.moveTimer = GameManager.GetInstance().CurrentTime + Random.Range(this.timeInterval.x, this.timeInterval.y);
  }
}
