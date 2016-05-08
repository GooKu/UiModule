using UnityEngine;
using System.Collections;

public class WindowBase : MonoBehaviour {
    #region setting at the object
    [HideInInspector]
    public WindowManager.ShowingType InShowingType = WindowManager.ShowingType.Pop;
    [HideInInspector]
    public Vector2 InPosition = Vector2.zero;
    [HideInInspector]
    public WindowManager.ShowingType OutShowingType = WindowManager.ShowingType.Pop;
    [HideInInspector]
    public Vector2 OutPosition = Vector2.zero;
    [HideInInspector]
    public float ShowingTime = 0.2f;
    #endregion setting at the object
}
