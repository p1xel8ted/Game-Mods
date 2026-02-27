// Decompiled with JetBrains decompiler
// Type: MouseManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MouseManager : BaseMonoBehaviour
{
  public TargetMarker target;
  private static MouseManager Instance;
  private LineRenderer lineRenderer;
  private Transform lineStartPosition;
  public bool showLine;
  private bool dragCam;
  private Vector3 oldPos;
  private Vector3 panOrigin;
  private Vector3 panTargetPosition;
  private Vector3 camZoom;
  private float camZoomTarget;

  private void Start()
  {
    MouseManager.Instance = this;
    this.lineRenderer = this.GetComponent<LineRenderer>();
    this.lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    this.lineRenderer.startWidth = 0.05f;
    this.lineRenderer.endWidth = 0.05f;
    this.lineRenderer.positionCount = 2;
    this.lineRenderer.enabled = false;
    this.panTargetPosition = Camera.main.transform.position;
    this.camZoom.z = 0.0f;
  }

  private void Update()
  {
    if (this.showLine)
    {
      if ((Object) this.lineStartPosition == (Object) null)
      {
        this.showLine = false;
      }
      else
      {
        this.lineRenderer.SetPosition(0, this.lineStartPosition.position);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition with
        {
          z = -Camera.main.transform.position.z
        });
        this.lineRenderer.SetPosition(1, worldPoint);
        MouseManager.Instance.target.reveal(worldPoint);
      }
    }
    if (Input.GetMouseButtonDown(1))
    {
      this.dragCam = true;
      this.oldPos = Camera.main.transform.transform.position - this.camZoom;
      this.panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
    if (Input.GetMouseButton(1))
      this.panTargetPosition = this.oldPos + -(Camera.main.ScreenToViewportPoint(Input.mousePosition) - this.panOrigin) * -Camera.main.transform.position.z;
    if (Input.GetMouseButtonUp(1))
      this.dragCam = false;
    if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0)
      ++this.camZoomTarget;
    if ((double) Input.GetAxis("Mouse ScrollWheel") < 0.0)
      --this.camZoomTarget;
    if ((double) this.camZoomTarget < -4.0)
      this.camZoomTarget = -4f;
    if ((double) this.camZoomTarget > 2.0)
      this.camZoomTarget = 2f;
    this.camZoom.z += (float) (((double) this.camZoomTarget - (double) this.camZoom.z) / 5.0);
    Vector3 position = Camera.main.transform.position;
    Camera.main.transform.position = position + (this.panTargetPosition + this.camZoom - position) / 3f;
  }

  private void OnDestroy() => MouseManager.Instance = (MouseManager) null;

  private static MouseManager getInstance() => MouseManager.Instance;

  public static void placeTarget(Vector3 position)
  {
    if ((Object) MouseManager.Instance == (Object) null)
      return;
    MouseManager.Instance.target.reveal(position);
  }

  public static void ShowLine(Transform position)
  {
    if ((Object) MouseManager.Instance == (Object) null)
      return;
    MouseManager.Instance.lineRenderer.enabled = true;
    MouseManager.Instance.lineStartPosition = position;
    MouseManager.Instance.showLine = true;
  }

  public static void HideLine()
  {
    if ((Object) MouseManager.Instance == (Object) null)
      return;
    MouseManager.Instance.showLine = false;
    MouseManager.Instance.lineRenderer.enabled = false;
  }
}
