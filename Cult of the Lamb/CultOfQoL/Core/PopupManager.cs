namespace CultOfQoL.Core;

internal class PopupManager : MonoBehaviour
{
    internal bool ShowPopup;
    private string _popupMessage = "";
    private Rect _popupRect = new(Screen.width / 2f - 150, Screen.height / 2f - 75, 500, 300);
    private GUIStyle _messageStyle;
    private GUIStyle _windowStyle;
    private GUIStyle _titleStyle;
    private void Start()
    {
        _titleStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 22,
            normal = {textColor = Color.black}
        };

        _messageStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 20,
            normal = {textColor = Color.black}
        };

        _windowStyle = new GUIStyle
        {
            normal = {background = Texture2D.whiteTexture, textColor = Color.black},
            richText = true,
            padding = new RectOffset(10, 10, 10, 10),
        };
    }

    private void OnGUI()
    {
        if (ShowPopup)
        {
            _popupRect = GUI.ModalWindow(0, _popupRect, DrawPopup, string.Empty, _windowStyle);
        }
    }

    private bool _dontShowAgainThisSession;

    private void DrawPopup(int windowID)
    {
        // Add flexible space before the content
        GUILayout.FlexibleSpace();

        // Apply the title style
        GUI.Label(new Rect(10, 10, _popupRect.width - 20, 30), Plugin.PluginName, _titleStyle);

        GUILayout.FlexibleSpace();

        // Use the custom style in the label for the message
        GUILayout.Label(_popupMessage, _messageStyle);

        // Add some space between the label and the first button
        GUILayout.FlexibleSpace();

        // Center and create the first button
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Add flexible space before the button
        if (GUILayout.Button("OK", GUILayout.Width(100), GUILayout.Height(40)))
        {
            ShowPopup = false;
        }
        if (!_ignoreDontShow)
        {
            if (GUILayout.Button("Close & Don't Show Again This Session", GUILayout.Width(250), GUILayout.Height(40)))
            {
                _dontShowAgainThisSession = true;
                ShowPopup = false;
            }
        }
        GUILayout.FlexibleSpace(); // Add flexible space after the button
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace(); 
    }

    private bool _ignoreDontShow;
    public void ShowPopupDlg(string message, bool showCloseAndDontShowAgain)
    {
        _ignoreDontShow = !showCloseAndDontShowAgain;
        if (_dontShowAgainThisSession && showCloseAndDontShowAgain) return;
        _popupMessage = message;
        ShowPopup = true;
    }
}