// Decompiled with JetBrains decompiler
// Type: PoisonTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PoisonTrail : BaseMonoBehaviour
{
  [SerializeField]
  private bool startActive;
  [SerializeField]
  private GameObject poisonPrefab;
  [SerializeField]
  private float distanceBetweenPoint;
  private Vector3 previousPlacedPosition = Vector3.zero;
  private Transform parent;

  public GameObject PoisonPrefab
  {
    get => this.poisonPrefab;
    set => this.poisonPrefab = value;
  }

  public Transform Parent
  {
    get => this.parent;
    set => this.parent = value;
  }

  private void Awake()
  {
    if (!this.startActive)
      this.enabled = false;
    this.parent = this.transform.parent;
  }

  private void Update()
  {
    if (!(this.previousPlacedPosition == Vector3.zero) && (double) Vector3.Distance(this.transform.position, this.previousPlacedPosition) <= (double) this.distanceBetweenPoint)
      return;
    this.previousPlacedPosition = this.transform.position;
    Object.Instantiate<GameObject>(this.poisonPrefab, new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), Quaternion.identity, this.parent);
  }
}
