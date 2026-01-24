// Decompiled with JetBrains decompiler
// Type: PoisonTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PoisonTrail : BaseMonoBehaviour
{
  [SerializeField]
  public bool startActive;
  [SerializeField]
  public GameObject poisonPrefab;
  [SerializeField]
  public float distanceBetweenPoint;
  public Vector3 previousPlacedPosition = Vector3.zero;
  public Transform parent;
  public bool UseSmallPoison;
  public bool isPoison;
  public bool hasCheckedPrafab;

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

  public void Awake()
  {
    if (!this.startActive)
      this.enabled = false;
    this.parent = this.transform.parent;
  }

  public void Start()
  {
    if (this.hasCheckedPrafab || !this.PoisonPrefab.name.Contains("Trap Poison"))
      return;
    this.UseSmallPoison = false;
    this.isPoison = true;
    if (!this.PoisonPrefab.name.Contains("Trap Poison Small"))
      return;
    this.UseSmallPoison = true;
  }

  public void OnEnable() => this.parent = this.transform.parent;

  public void Update()
  {
    if (!(this.previousPlacedPosition == Vector3.zero) && (double) Vector3.Distance(this.transform.position, this.previousPlacedPosition) <= (double) this.distanceBetweenPoint)
      return;
    this.previousPlacedPosition = this.transform.position;
    if (this.isPoison)
    {
      if (!this.UseSmallPoison)
        TrapPoison.CreatePoison(new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), 1, 0.0f, this.parent);
      else
        TrapPoison.CreatePoison(new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), 1, 0.0f, this.parent, true);
    }
    else
      ObjectPool.Spawn(this.PoisonPrefab, this.parent, new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), Quaternion.identity);
  }
}
