using UnityEngine;
using System.Collections;

public class WindowBase : MonoBehaviour {
    #region setting at the object
    public WindowManager.ShowingType InShowingType = WindowManager.ShowingType.Pop;
    public Vector2 InPosition = Vector2.zero;
    public WindowManager.ShowingType OutShowingType = WindowManager.ShowingType.Pop;
    public Vector2 OutPosition = Vector2.zero;
    public float ShowingTime = 0.2f;
    #endregion setting at the object
}
