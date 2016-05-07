using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class WindowManager : Singleton<WindowManager>
{
    #region setting at the object
    public GameObject MaskBackgroundSample;
    public float PopMinScale = 0.1f;
    #endregion setting at the object

    public enum ShowingType {Pop, Panning}
    public enum ControlType
    {
        NORMAL,
        UI_ONLY,
        UI_MASK,
        ALL_MASK
    }

    public Action zoomCompletedAction;
    public ControlType ControlStatus{ get; protected set; }

    private GameObject[] backMask = new GameObject[2];
    private GameObject frontMask;
    private List<WindowBase> currentOpenWindowList = new List<WindowBase>();
    private Tweener tweener;
    private Action closeAllWindowCompletedAction;

    public void Open(WindowBase window, Action showCompletedAction=null)
    {
        ControlStatus = ControlType.ALL_MASK;

        float showTime = window.ShowingTime;
        int index = unactiveMaskBgIndex();
        GameObject currentBackground = backMask[index];
        GameObject lastBackground = backMask[1-index];

        if (currentOpenWindowList.Count > 0)
        {
            lastBackground.GetComponent<Image>().DOFade(0, showTime).SetEase(Ease.InSine).OnComplete(()=> 
            {
                lastBackground.SetActive(false);
            });
        }

        currentBackground.transform.SetParent(window.transform.parent);
        currentBackground.transform.localPosition = Vector3.zero;
        currentBackground.transform.localScale = Vector3.one;
        currentBackground.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        currentBackground.GetComponent<Image>().DOFade(0, 0);
        currentBackground.SetActive(true);
        currentBackground.transform.SetAsLastSibling();
        currentBackground.GetComponent<Image>().DOFade(0.5f, showTime).SetEase(Ease.InSine);

        window.gameObject.SetActive(true);
        window.transform.SetAsLastSibling();

        frontMask.transform.SetParent(window.transform.parent);
        frontMask.transform.localPosition = Vector3.zero;
        frontMask.transform.localScale = Vector3.one;
        frontMask.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        frontMask.transform.SetAsLastSibling();
        frontMask.SetActive(true);
        
        switch (window.InShowingType)
        {
            case ShowingType.Panning:
                window.transform.localScale = Vector3.one;
                window.transform.localPosition = window.InPosition;
                tweener = window.transform.DOLocalMove(Vector2.zero, showTime).SetEase(Ease.OutSine);
                break;
            case ShowingType.Pop:
                window.transform.DOScale(PopMinScale, 0);
                tweener = window.transform.DOScale(1, showTime).SetEase(Ease.OutBack);
                break;
        }

        tweener.OnComplete(() =>
        {
            if (showCompletedAction != null)
                showCompletedAction();
            ControlStatus = ControlType.UI_ONLY;
            frontMask.SetActive(false);
        });

        if (!currentOpenWindowList.Contains(window))
            currentOpenWindowList.Add(window);
        else
        {
            int currentIndex = currentOpenWindowList.IndexOf(window);
            int lastIndex = currentOpenWindowList.Count - 1;
            currentOpenWindowList[currentIndex] = currentOpenWindowList[lastIndex];
            currentOpenWindowList[lastIndex] = window;
        }

    }

    public void Close(WindowBase window, Action showCompletedAction = null)
    {
        ControlStatus = ControlType.ALL_MASK;
        float showTime = window.ShowingTime;
        int index = unactiveMaskBgIndex();
        GameObject lastBackground = backMask[index];
        GameObject currentBackground = backMask[1 - index];

        frontMask.SetActive(true);

        currentOpenWindowList.Remove(window);
        if (currentOpenWindowList.Count > 0)
        {
            lastBackground.SetActive(true);
            lastBackground.GetComponent<Image>().DOFade(0.5f, showTime).SetEase(Ease.InSine);
        }

        currentBackground.GetComponent<Image>().DOFade(0, showTime).SetEase(Ease.InSine).OnComplete(() =>
        {
            currentBackground.SetActive(false);
        });


        switch (window.OutShowingType)
        {
            case ShowingType.Panning:
                tweener = window.transform.DOLocalMove(window.OutPosition, showTime).SetEase(Ease.InSine);
                break;
            case ShowingType.Pop:
                tweener = window.transform.DOScale(PopMinScale, showTime).SetEase(Ease.InBack);
                break;
        }

        tweener.OnComplete(() =>
        {
            if (showCompletedAction != null)
                showCompletedAction();

            ControlStatus = ControlType.UI_ONLY;
            frontMask.SetActive(false);
            window.gameObject.SetActive(false);
        });
    }

    public void CloseAllWindow(Action completedAction =null)
    {
        closeAllWindowCompletedAction = completedAction; 
        autoCloseWindow();
    }

    private void autoCloseWindow()
    {
        if (currentOpenWindowList.Count == 0)
        {
            if(closeAllWindowCompletedAction != null)
                closeAllWindowCompletedAction();
            return;
        }

        Close(currentOpenWindowList[currentOpenWindowList.Count - 1], autoCloseWindow);
    }


    protected override void customerAwake()
    {
        init();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (Instance != this)
            return;
        init();
    }

    private void init()
    {
        ControlStatus = ControlType.NORMAL;

        for (int i = 0; i < backMask.Length; i++)
        {
            if (backMask[i] != null)
                continue;
            backMask[i] = makeMask();
            backMask[i].name += "_back" + i;
        }

        if (frontMask == null)
        {
            frontMask = makeMask();
            frontMask.name += "_front";
        }

        List<WindowBase> existWindows = new List<WindowBase>();

        for (int i = 0; i < currentOpenWindowList.Count; i++)
            if (currentOpenWindowList[i] != null)
                existWindows.Add(currentOpenWindowList[i]);

        currentOpenWindowList = existWindows;
    }

    private GameObject makeMask()
    {
        GameObject mask = Instantiate(MaskBackgroundSample);
        mask.transform.SetParent(transform);
        mask.GetComponent<Image>().DOFade(0, 0);
        mask.SetActive(false);
        return mask;
    }

    private int unactiveMaskBgIndex()
    {
        if (backMask[0].activeSelf)
            return 1;
        else
            return 0;
    }
}
