// Decompiled with JetBrains decompiler
// Type: PlayerDistanceTint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class PlayerDistanceTint : BaseMonoBehaviour
{
  public GameObject tintObjectLocation;
  public float distanceToTint;
  public float fillAmount = 0.5f;
  public Color tintColor;
  public Material playerMaterial;
  public int colorID;
  public int floatID;
  public MaterialPropertyBlock block;
  public float dist;
  public Color lerpedColor;
  public float distPercent;
  public bool ResetToZero;
  [SerializeField]
  public bool useItemInWoodsColor;
  public bool wasInRange;

  public void Start()
  {
    this.block = new MaterialPropertyBlock();
    this.floatID = Shader.PropertyToID("_FadeIntoDoor");
    this.SetDefaults();
  }

  public void SetDefaults()
  {
    if (!((Object) this.playerMaterial != (Object) null))
      return;
    this.playerMaterial.SetFloat(this.floatID, 0.0f);
  }

  public IEnumerator WaitForPlayer()
  {
    while ((Object) this.playerMaterial == (Object) null)
    {
      yield return (object) new WaitForSeconds(0.1f);
      if ((Object) PlayerFarming.Instance != (Object) null)
        this.playerMaterial = PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().material;
    }
  }

  public void OnDisable() => this.SetDefaults();

  public void OnDestroy() => this.SetDefaults();

  public void OnEnable()
  {
    this.SetDefaults();
    this.CheckAndSetTint();
  }

  public void Update() => this.CheckAndSetTint();

  public void CheckAndSetTint()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.dist = Vector3.Distance(PlayerFarming.Instance.gameObject.transform.position, this.tintObjectLocation.transform.position);
    if ((double) this.distanceToTint == 0.0)
      return;
    if ((double) this.dist <= (double) this.distanceToTint)
    {
      this.distPercent = Mathf.Abs((float) ((double) this.dist / (double) this.distanceToTint - 1.0));
      if ((Object) this.playerMaterial != (Object) null)
        this.playerMaterial.SetFloat(this.floatID, this.distPercent);
      this.wasInRange = true;
    }
    else
    {
      if (!this.wasInRange)
        return;
      if ((Object) this.playerMaterial != (Object) null)
        this.playerMaterial.SetFloat(this.floatID, 0.0f);
      this.wasInRange = false;
    }
  }
}
