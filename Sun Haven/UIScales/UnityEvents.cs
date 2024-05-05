namespace UIScales;

public partial class Plugin
{
    private void Update()
    {
        var isMainMenu = SceneManager.GetActiveScene().name.Equals("MainMenu", StringComparison.InvariantCultureIgnoreCase);
        Utils.UpdateUiScale(isMainMenu);
        Utils.UpdateZoomLevel();
        Utils.UpdateCanvasScaleFactors();
    }
}