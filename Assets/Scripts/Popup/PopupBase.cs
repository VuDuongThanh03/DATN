using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public bool isModal = false;
    public bool addToBackStack = true;
    public bool hideIsDestroy = true;
    public UnityEngine.Events.UnityEvent onPopupClosed;

    protected virtual void Awake()
    {
        PopupManager.Instance.OnCreatePopup(this);
    }

    protected virtual void OnEnable()
    {
        PopupManager.Instance.OnShowPopup(this);
    }

    protected virtual void OnDisable()
    {
        PopupManager.Instance.OnHidePopup(this);
        onPopupClosed?.Invoke();
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackBtnClick();
        }
#endif
    }

    public void MakePopupToTop()
    {
        transform.SetAsLastSibling();
    }

    protected virtual void OnCleanUp()
    {

    }

    public static void DestroyPopup(PopupBase popup)
    {
        if (PopupManager.Instance)
            PopupManager.Instance.RemovePopup(popup);
    }

    public virtual void OnBackBtnClick()
    {
        if (isModal == false && addToBackStack)
        {
            OnCloseBtnClick();
        }

        // SoundManager.Instance.PlaySoundFX(SoundFXID.SOUND_FX_UI_tap);
    }

    public virtual void OnCloseBtnClick()
    {

        //SoundManager.Instance?.PlaySoundFX(SoundFXID.SOUND_UI_BUTTON_CLICK);
        OnCleanUp();
        Hide();
    }

    protected virtual void Hide()
    {

        if (gameObject)
        {
            if (hideIsDestroy)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}