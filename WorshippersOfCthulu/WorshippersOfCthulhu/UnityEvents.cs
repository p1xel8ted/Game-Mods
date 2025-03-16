namespace WorshippersOfCthulhu;

public class UnityEvents : MonoBehaviour
{
    private void Awake()
    {
        Plugin.Logger.LogInfo("UnityEvents Awake");
    }

    private void Update()
    {
        if (Input.GetKeyUp(Plugin.KeybindReload.Value))
        {
            Plugin.Instance.Config.Reload();
        }
    }

}