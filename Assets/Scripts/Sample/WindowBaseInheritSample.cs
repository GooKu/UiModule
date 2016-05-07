using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WindowBaseInheritSample : WindowBase {
    #region setting at the object
    public Text Contents;
    #endregion setting at the object

    public void Refresh()
    {
        Contents.text = System.DateTime.Now.ToString();
    }
}