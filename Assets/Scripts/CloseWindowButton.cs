using UnityEngine;
using UnityEngine.UI;

public class CloseWindowButton : MonoBehaviour {
    #region setting at the object
    public WindowBase Window;
    #endregion setting at the object

    private void Awake()
    {
        if (Window == null)
            Window = transform.parent.GetComponent<WindowBase>();

        if (Window != null)
            GetComponent<Button>().onClick.AddListener(delegate ()
            { WindowManager.Instance.Close(Window); });
    }
}
