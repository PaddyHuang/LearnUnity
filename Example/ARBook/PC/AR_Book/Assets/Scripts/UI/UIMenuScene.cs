using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuScene : MonoBehaviour
{
    //退出按钮，确认退出按钮，取消退出按钮
    public Button QuitBtn, ComfirmBtn, CancelBtn;
    public RectTransform ComfirmPanel;
    //展示课本
    public CardDeckInterface CardDeck;
    public Camera Camera;

    private bool isCanHit = true;

    void Start()
    {
        Camera = Camera.main;
        QuitBtn.onClick.AddListener(QuitBtnClick);
        ComfirmBtn.onClick.AddListener(ComfirmQuit);
        CancelBtn.onClick.AddListener(CancelQuit);
    }

    // Update is called once per frame
    void LateUpdate()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        PCRay();
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
        MobileRay();
#endif
    }

    private void PCRay()
    {
        if (Input.GetMouseButtonDown(0) && isCanHit)
        {
            RaycastHit hitInfo;

            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            Transform targetTransform = null;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, -1))
            {
                targetTransform = hitInfo.collider.transform;
            }
            if (targetTransform != null)
            {
                var tt = targetTransform.GetComponent<UIMenuCard>();
                if (tt != null)
                {
                    //Debug.Log(string.Format("tt.Locked: {0}, tt.id: {1}", tt.Locked, tt.id));

                    if (tt.Locked)
                        return;

                    if (tt.id == CardDeck.GetCurrentIndex())
                    {
                        LoadScene(tt.id);
                        //LoadScene();
                        //Debug.Log(CardDeck.GetCurrentIndex());
                    }
                }
            }
        }
    }

    private void MobileRay()
    {
        if (Input.touchCount == 1 && isCanHit)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary)
                return;

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            RaycastHit hitInfo;

            Ray ray = Camera.ScreenPointToRay(touch.position);

            Transform targetTransform = null;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, -1))
            {
                targetTransform = hitInfo.collider.transform;
            }

            if (targetTransform != null)
            {
                var tt = targetTransform.GetComponent<UIMenuCard>();
                if (tt != null)
                {
                    if (tt.Locked)
                        return;
                    
                    if (tt.id == CardDeck.GetCurrentIndex())
                    {
                        LoadScene(tt.id);
                        //LoadScene();
                    }                   
                }
            }
        }
    }

    //private void LoadScene()
    //{
    //    SceneManager.LoadScene("Main");
    //}

    /// <summary>
    /// 加载课本场景(待开发新课本)
    /// </summary>
    /// <param name="id"></param>
    private void LoadScene(int id)
    {
        switch (id)
        {
            case 0:
                StaticData.BookInfo.currentBookID = StaticData.BookInfo.JianZhuJiChuGongChengID;
                SceneManager.LoadScene(StaticData.BookInfo.JianZhuJiChuGongChengName);                
                break;
            case 1:
                StaticData.BookInfo.currentBookID = StaticData.BookInfo.ChanPinShouCeID;
                SceneManager.LoadScene(StaticData.BookInfo.ChanPinShouCeName);
                break;
            case 2:                
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }        
    }

    private void QuitBtnClick()
    {
        ComfirmPanel.gameObject.SetActive(true);
        CardDeck.enabled = false;
        isCanHit = false;
    }

    private void ComfirmQuit()
    {
        Application.Quit();
    }

    private void CancelQuit()
    {
        ComfirmPanel.gameObject.SetActive(false);
        CardDeck.enabled = true;
        StopAllCoroutines();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
        isCanHit = true;
    }
}