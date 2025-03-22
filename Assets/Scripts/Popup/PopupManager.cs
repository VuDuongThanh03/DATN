using System.Collections.Generic;
using UnityEngine;
public class PopupManager : MonoBehaviour
{
    static PopupManager instance = null;
    public static PopupManager Instance => instance ??= FindObjectOfType<PopupManager>() ?? new PopupManager();
    [System.NonSerialized]
    public List<PopupBase> activePopups = new List<PopupBase>();
    private Stack<PopupBase> backStackPopups = new Stack<PopupBase>();

    private static Vector3 POPUP_DEFAULT_POSITION = Vector3.zero;
    private Dictionary<string, GameObject> popupsPref = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> popupsCache = new Dictionary<string, GameObject>();

    public bool IsHasPopupShowing
    {
        get
        {
            return activePopups.Count > 0;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.ClearAll();
    }
    private void Start()
    {
        //PopupRandomEvent.Create("LuckySpin");
    }
    public void ClearAll()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i) != null)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        this.activePopups.Clear();
    }
    public void ShowAllCachedPopups()
    {
        foreach (var popup in instance.popupsCache)
        {
            if (popup.Value == null)
                continue;
            popup.Value.SetActive(true);
        }
        instance.popupsCache.Clear();
    }
    public void RemovePopup(PopupBase popup)
    {
        if (this.activePopups.Contains(popup))
        {
            this.activePopups.Remove(popup);
        }
    }
    public void HideAndCacheAllPreviousPopup()
    {
        foreach (var popup in this.activePopups)
        {
            if (popup.gameObject.activeInHierarchy)
            {
                popup.gameObject.SetActive(false);
                this.popupsCache.Add(popup.GetType().Name, popup.gameObject);
            }
        }
    }
    public bool HaveBackStackPopup()
    {
        return backStackPopups.Count > 0;
    }
    public void OnBackBtnClick()
    {
        if (HaveBackStackPopup())
        {
            var popup = backStackPopups.Peek();
            popup.OnBackBtnClick();
        }
    }
    public void OnShowPopup(PopupBase popup)
    {
        if (this.activePopups.Contains(popup) == false) this.activePopups.Add(popup);
        if (popup.addToBackStack) backStackPopups.Push(popup);
    }
    public void OnHidePopup(PopupBase popup)
    {
        if (popup == null) return;
        int index = this.activePopups.FindIndex(v => v == popup);
        if (index >= 0)
            this.activePopups.RemoveAt(index);
        if (backStackPopups.Contains(popup))
        {
            var tempList = new List<PopupBase>();
            var peek = backStackPopups.Count > 0 ? backStackPopups.Peek() : null;
            while (backStackPopups.Count > 0 && peek != popup)
            {
                tempList.Add(backStackPopups.Pop());
                peek = backStackPopups.Count > 0 ? backStackPopups.Peek() : null;
            }

            if (peek == popup)
            {
                backStackPopups.Pop();
            }

            for (int i = tempList.Count - 1; i >= 0; --i)
            {
                backStackPopups.Push(tempList[i]);
            }
            tempList.Clear();
        }
    }
    public void OnCreatePopup(PopupBase popup)
    {
        if (popup == null) return;
    }
    private GameObject GetPopupPref(string popupName)
    {
        GameObject pref = null;
        if (!popupsPref.ContainsKey(popupName))
        {
            pref = Resources.Load("Prefabs/UI/Popups/" + popupName) as GameObject;
            popupsPref[popupName] = pref;
        }
        else
        {
            pref = popupsPref[popupName];
            if (pref == null)
            {
                popupsPref.Remove(popupName);
                this.GetPopupPref(popupName);
            }
        }
        return pref;
    }
    public GameObject GetPopup(string popupName, bool useDefaultPos = true, Vector3 pos = default(Vector3), bool isMultiInstance = false, bool shouldActive = true)
    {
        GameObject obj = null;
        string name = popupName + "(Clone)";
        if (!popupsCache.ContainsKey(name) || isMultiInstance)
        {
            GameObject pref = this.GetPopupPref(popupName);
            if (pref == null)
                return null;
            obj = Instantiate(pref, transform);
        }
        else
        {
            obj = popupsCache[name];
            if (obj == null)
            {
                popupsCache.Remove(name);
                this.GetPopup(popupName, useDefaultPos, pos);
            }
        }
        if (shouldActive)
        {
            obj.SetActive(true);
        }
        if (useDefaultPos) pos = POPUP_DEFAULT_POSITION;
        obj.transform.localPosition = pos;
        if (obj.GetComponent<RectTransform>() != null)
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return obj;
    }
}