namespace AlchemyResearchRedux;

internal class WidgetPos : MonoBehaviour
{
    private void Disable()
    {
        if (gameObject.name.Contains("result"))
        {
            enabled = false;
        }
    }
    public void OnEnable()
    {
        Disable();
    }
    public void Start()
    {
        Disable();
    }
    public void Update()
    {
        transform.localPosition = transform.localPosition with {y = 25f};
    }
}