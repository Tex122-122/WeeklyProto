using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{

    #region Attributes

    #endregion

    #region Player Pref Key Constants

    private const string RESOLUTION_PREF_KEY = "resolution";

    #endregion

    #region Resolution
    [SerializeField]
    private Text resolutionText;

    private Resolution[] resolutions;

    private int currentResolutionsIndex = 0;


    #endregion



    void Start()
    {
        resolutions = Screen.resolutions;

        currentResolutionsIndex = PlayerPrefs.GetInt(RESOLUTION_PREF_KEY, 0);

        SetResolutionText(resolutions[currentResolutionsIndex]);
    }

    #region Resolution Cycling

    private void SetResolutionText(Resolution resolution)
    {
        resolutionText.text = resolution.width + "X" + resolution.height;
    }

    public void SetNextResolution()
    {
        currentResolutionsIndex = GetNextWrappedIndex(resolutions, currentResolutionsIndex);
            SetResolutionText(resolutions[currentResolutionsIndex]);
    }

    public void SetPreviousResolution()
    {
        currentResolutionsIndex = GetPreviousWrappedIndex(resolutions, currentResolutionsIndex);
            SetResolutionText(resolutions[currentResolutionsIndex]);
    }

    #endregion

    #region Apply Resolution

    private void SetAndApplyResolution(int newResolutionIndex)
    {
        currentResolutionsIndex = newResolutionIndex;
        ApplyCurrentResolution();
    }

    private void ApplyCurrentResolution()
    {
        ApplyResolution(resolutions[currentResolutionsIndex]);
    }

    private void ApplyResolution(Resolution resolution)
    {

        SetResolutionText(resolution);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_PREF_KEY, currentResolutionsIndex);
    }

    #endregion


    #region Misc Helpers

    #region Index Wrap Helpers
    private int GetNextWrappedIndex<T>(IList<T> collection, int currentIdex)
    {
        if (collection.Count < 1) return 0;
        return (currentIdex + 1) % collection.Count;
    }

    private int GetPreviousWrappedIndex<T>(IList<T> collection, int currentIdex)
    {
        if (collection.Count < 1) return 0;
        if ((currentIdex - 1) < 0) return collection.Count - 1;
        return (currentIdex - 1) % collection.Count;
    }

    #endregion

    #endregion

    public void ApplyChanges()
    {
        SetAndApplyResolution(currentResolutionsIndex);
    }
}