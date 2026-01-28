// Decompiled with JetBrains decompiler
// Type: GraveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GraveManager : MonoBehaviour
{
  [SerializeField]
  public GameObject gravesI;
  [SerializeField]
  public GameObject gravesII;
  [SerializeField]
  public GameObject gravesIII;

  public void Start()
  {
    this.gravesI.SetActive(false);
    this.gravesII.SetActive(false);
    this.gravesIII.SetActive(false);
    if (!LoreSystem.LoreAvailable(10))
    {
      this.gravesI.SetActive(true);
      this.gravesII.SetActive(false);
      this.gravesIII.SetActive(false);
    }
    else if (!LoreSystem.LoreAvailable(11))
    {
      this.gravesI.SetActive(false);
      this.gravesII.SetActive(true);
      this.gravesIII.SetActive(false);
    }
    else if (!LoreSystem.LoreAvailable(12))
    {
      this.gravesI.SetActive(false);
      this.gravesII.SetActive(false);
      this.gravesIII.SetActive(true);
    }
    else
    {
      this.gravesI.SetActive(true);
      Debug.Log((object) "No Lores Available!");
    }
  }
}
