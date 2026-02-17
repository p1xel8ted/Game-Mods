// Decompiled with JetBrains decompiler
// Type: Interaction_DoctrineStone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_DoctrineStone : Interaction
{
  public static int SpritePiece = DataManager.Instance.DoctrineStoneTotalCount % 3;
  public SpriteRenderer SpriteRenderer;
  public List<Sprite> Sprites = new List<Sprite>();
  public UnityEvent Callback;
  public static List<Interaction_DoctrineStone> DoctrineStones = new List<Interaction_DoctrineStone>();
  public bool EnableAutoCollect = true;
  public string sDoctrine;
  public string sPickUp;
  public System.Action OnCollect;

  public void Start()
  {
    if (DataManager.Instance.DoctrineStoneTotalCount == 0)
    {
      PickUp component = this.GetComponent<PickUp>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.MagnetToPlayer && (double) component.Speed == 0.0)
      {
        component.DisableSeperation = true;
        component.enabled = false;
        if ((UnityEngine.Object) component.child != (UnityEngine.Object) null)
          component.child.transform.localScale = Vector3.one;
      }
    }
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
    this.SetSprite();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_DoctrineStone.DoctrineStones.Add(this);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_DoctrineStone.DoctrineStones.Remove(this);
  }

  public override void Update()
  {
    base.Update();
    if (!this.EnableAutoCollect || !((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null) || (double) Vector3.Distance(this.transform.position, this.playerFarming.transform.position) >= (double) this.ActivateDistance / 3.0 || !this.Interactable)
      return;
    this.OnInteract(this.playerFarming.state);
  }

  public void SetSprite()
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

  public void Collect()
  {
    ++DataManager.Instance.DoctrineStoneTotalCount;
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
    ++DataManager.Instance.DoctrineCurrentCount;
    if (DataManager.Instance.DoctrineCurrentCount > 3)
    {
      DataManager.Instance.DoctrineCurrentCount = 1;
      ++DataManager.Instance.CompletedDoctrineStones;
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.state == (UnityEngine.Object) this.state)
      {
        player.PlayerDoctrineStone.Play();
        this.Callback?.Invoke();
        System.Action onCollect = this.OnCollect;
        if (onCollect != null)
          onCollect();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }
  }

  public void MagnetToPlayer(GameObject playerObject = null)
  {
    this.StartCoroutine((IEnumerator) this.IMagnetToPlayer(playerObject));
    Debug.Log((object) "magnet to player".Colour(Color.green));
  }

  public IEnumerator IMagnetToPlayer(GameObject playerObject = null)
  {
    Interaction_DoctrineStone interactionDoctrineStone = this;
    yield return (object) new WaitForSeconds(0.5f);
    PickUp component = interactionDoctrineStone.GetComponent<PickUp>();
    if ((UnityEngine.Object) playerObject != (UnityEngine.Object) null)
    {
      component.PlayerOverride = true;
      component.Player = playerObject;
    }
    component.MagnetDistance = 100f;
    component.AddToInventory = false;
    component.MagnetToPlayer = true;
    component.Callback.AddListener(new UnityAction(interactionDoctrineStone.Collect));
    interactionDoctrineStone.AutomaticallyInteract = true;
  }
}
