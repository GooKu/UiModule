using UnityEngine;
using System.Collections;

public class WindowZoomSampleManager : MonoBehaviour {

    public void OpenByInspector(WindowBase window)
    {
        WindowManager.Instance.Open(window);
    }

    private WindowBaseInheritSample InheritSample;

    public void OpenByPrefab()
    {
        if(InheritSample == null)
        {
            InheritSample = Instantiate(Resources.Load<GameObject>("Prefab/TestWindow_3")).GetComponent<WindowBaseInheritSample>();
            InheritSample.transform.SetParent(transform);
            InheritSample.transform.localPosition = Vector3.zero;
            InheritSample.transform.localScale = Vector3.one;
            InheritSample.gameObject.SetActive(false);
        }
        InheritSample.Refresh();
        WindowManager.Instance.Open(InheritSample);        
    }

    public void CloseAllWindow()
    {
        WindowManager.Instance.CloseAllWindow(completeCloseAllWindow);
    }

    private void completeCloseAllWindow()
    {
        Debug.Log("Close All Window!");
    }

    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
