namespace RestInPatches;

public class DestroyWhenInvisible : MonoBehaviour
{
    private void Start()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() => Destroy(gameObject);
}
