using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindowBase), true)]
public class WindowBaseEditor : Editor {
    public override void OnInspectorGUI()
    {
        WindowBase windowBase = (WindowBase)target;
        windowBase.InShowingType = (WindowManager.ShowingType)EditorGUILayout.EnumPopup("InShowingType:", windowBase.InShowingType);
        switch (windowBase.InShowingType)
        {
            case WindowManager.ShowingType.Panning:
                windowBase.InPosition = EditorGUILayout.Vector2Field("InPosition:", windowBase.InPosition);
                break;
        }
        windowBase.OutShowingType = (WindowManager.ShowingType)EditorGUILayout.EnumPopup("OutShowingType:", windowBase.OutShowingType);
        switch (windowBase.OutShowingType)
        {
            case WindowManager.ShowingType.Panning:
                windowBase.OutPosition = EditorGUILayout.Vector2Field("OutPosition:", windowBase.OutPosition);
                break;
        }
        windowBase.ShowingTime = EditorGUILayout.FloatField("ShowingTime:", windowBase.ShowingTime);
        base.OnInspectorGUI();
    }
}
