// Decompiled with JetBrains decompiler
// Type: ShadowCaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class ShadowCaster : MonoBehaviour
{
  [SerializeField]
  public float baseScale = 1f;
  [SerializeField]
  public float shrinkPerUnit = 0.1f;
  [SerializeField]
  public GameObject shadowPrefab;
  public GameObject shadowInstance;

  public void Start()
  {
    this.shadowInstance = Object.Instantiate<GameObject>(this.shadowPrefab, this.transform.position, Quaternion.identity);
    this.shadowInstance.transform.position = new Vector3(this.transform.position.x, 0.0f, 0.0f);
    this.shadowInstance.transform.localScale = new Vector3(this.baseScale, this.baseScale, this.baseScale);
    this.shadowInstance.transform.parent = this.transform;
  }

  public void Update()
  {
    if ((Object) this.shadowInstance == (Object) null)
      return;
    float num1 = math.abs(this.transform.position.z);
    this.shadowInstance.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    float num2 = this.baseScale - num1 * this.shrinkPerUnit;
    this.shadowInstance.transform.localScale = new Vector3(num2, num2, num2);
  }
}
