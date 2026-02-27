// Decompiled with JetBrains decompiler
// Type: PlayerDistanceTint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayerDistanceTint : BaseMonoBehaviour
{
  public GameObject tintObjectLocation;
  public float distanceToTint;
  public float fillAmount = 0.5f;
  public Color tintColor;
  public Material playerMaterial;
  private int colorID;
  private int floatID;
  private MaterialPropertyBlock block;
  public float dist;
  public Color lerpedColor;
  public float distPercent;
  public bool ResetToZero;
  [SerializeField]
  private bool useItemInWoodsColor;
  private bool wasInRange;

  private void Start()
  {
    this.block = new MaterialPropertyBlock();
    this.floatID = Shader.PropertyToID("_FillAlpha");
    this.SetDefaults();
  }

  private void SetDefaults()
  {
    if (!((Object) this.playerMaterial != (Object) null))
      return;
    this.playerMaterial.SetFloat(this.floatID, 0.0f);
  }

  private void OnDisable() => this.SetDefaults();

  private void OnDestroy() => this.SetDefaults();

  private void OnEnable() => this.playerMaterial.SetFloat(this.floatID, 0.0f);

  private void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.dist = Vector3.Distance(PlayerFarming.Instance.gameObject.transform.position, this.tintObjectLocation.transform.position);
    if ((double) this.dist <= (double) this.distanceToTint)
    {
      this.distPercent = Mathf.Abs((float) ((double) this.dist / (double) this.distanceToTint - 1.0));
      this.playerMaterial.SetFloat(this.floatID, this.distPercent);
      this.wasInRange = true;
    }
    else
    {
      if (!this.wasInRange)
        return;
      this.playerMaterial.SetFloat(this.floatID, 0.0f);
      this.wasInRange = false;
    }
  }
}
