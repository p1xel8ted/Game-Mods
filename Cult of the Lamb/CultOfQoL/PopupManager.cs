namespace CultOfQoL;

internal class PopupManager : MonoBehaviour
{
    internal bool showPopup;
    private string popupMessage = "";
    private Rect popupRect = new(Screen.width / 2f - 150, Screen.height / 2f - 75, 500, 300);
    private GUIStyle? messageStyle;
    private GUIStyle? windowStyle;
    private GUIStyle? titleStyle;
    private void Start()
    {
        titleStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 22,
            normal = {textColor = Color.black}
        };

        messageStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 20,
            normal = {textColor = Color.black}
        };

        windowStyle = new GUIStyle
        {
            normal = {background = Texture2D.whiteTexture, textColor = Color.black},
            richText = true,
            padding = new RectOffset(10, 10, 10, 10),
        };
    }

    private void OnGUI()
    {
        if (showPopup)
        {
            popupRect = GUI.ModalWindow(0, popupRect, DrawPopup, string.Empty, windowStyle);
        }
    }

    private bool dontShowAgainThisSession;

    private void DrawPopup(int windowID)
    {
        // Add flexible space before the content
        GUILayout.FlexibleSpace();

        // Apply the title style
        GUI.Label(new Rect(10, 10, popupRect.width - 20, 30), Plugin.PluginName, titleStyle);

        GUILayout.FlexibleSpace();

        // Use the custom style in the label for the message
        GUILayout.Label(popupMessage, messageStyle);

        // Add some space between the label and the first button
        GUILayout.FlexibleSpace();

        // Center and create the first button
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Add flexible space before the button
        if (GUILayout.Button("OK", [GUILayout.Width(100), GUILayout.Height(40)]))
        {
            showPopup = false;
        }
        if (!ignoreDontShow)
        {
            if (GUILayout.Button("Close & Don't Show Again This Session", [GUILayout.Width(250), GUILayout.Height(40)]))
            {
                dontShowAgainThisSession = true;
                showPopup = false;
            }
        }
        GUILayout.FlexibleSpace(); // Add flexible space after the button
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace(); 
    }

    private bool ignoreDontShow;
    public void ShowPopup(string message, bool showCloseAndDontShowAgain)
    {
        ignoreDontShow = !showCloseAndDontShowAgain;
        if (dontShowAgainThisSession && showCloseAndDontShowAgain) return;
        popupMessage = message;
        showPopup = true;
    }
}