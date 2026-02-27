// Decompiled with JetBrains decompiler
// Type: Interaction_DoctrineStone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_DoctrineStone : Interaction
{
  public static int SpritePiece;
  public SpriteRenderer SpriteRenderer;
  public List<Sprite> Sprites = new List<Sprite>();
  public UnityEvent Callback;
  public static List<Interaction_DoctrineStone> DoctrineStones = new List<Interaction_DoctrineStone>();
  public bool EnableAutoCollect = true;
  private string sDoctrine;
  private string sPickUp;
  public System.Action OnCollect;

  private void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.SetSprite();
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    Interaction_DoctrineStone.DoctrineStones.Add(this);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_DoctrineStone.DoctrineStones.Remove(this);
  }

  protected override void Update()
  {
    base.Update();
    if (!this.EnableAutoCollect || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) >= (double) this.ActivateDistance / 3.0 || !this.Interactable)
      return;
    this.OnInteract(PlayerFarming.Instance.state);
  }

  private void SetSprite()
  {
    this.SpriteRenderer.sprite = this.Sprites[Interaction_DoctrineStone.SpritePiece];
    if (++Interaction_DoctrineStone.SpritePiece < 3)
      return;
    Interaction_DoctrineStone.SpritePiece = 0;
  }

  public override void UpdateLocalisation()
  {
    this.sDoctrine = ScriptLocalization.Inventory.DOCTRINE_STONE;
    this.sPickUp = ScriptLocalization.Interactions.PickUp;
  }

  public override void GetLabel() => this.Label = $"{this.sPickUp} {this.sDoctrine}";

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Collect();
  }

  private void Collect()
  {
    ++DataManager.Instance.DoctrineStoneTotalCount;
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
    PlayerDoctrineStone.Play(1);
    this.Callback?.Invoke();
    System.Action onCollect = this.OnCollect;
    if (onCollect != null)
      onCollect();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void MagnetToPlayer()
  {
    this.StartCoroutine((IEnumerator) this.IMagnetToPlayer());
    Debug.Log((object) "magnet to player".Colour(Color.green));
  }

  private IEnumerator IMagnetToPlayer()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_DoctrineStone interactionDoctrineStone = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      PickUp component = interactionDoctrineStone.GetComponent<PickUp>();
      component.MagnetDistance = 100f;
      component.AddToInventory = false;
      component.MagnetToPlayer = true;
      component.Callback.AddListener(new UnityAction(interactionDoctrineStone.Collect));
      interactionDoctrineStone.AutomaticallyInteract = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
