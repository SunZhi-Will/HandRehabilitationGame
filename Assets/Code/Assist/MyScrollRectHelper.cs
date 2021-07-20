using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
/// <summary>
/// 滾動矩形助手
/// (加大小變化擴充)
/// </summary>
public class MyScrollRectHelper : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public EquipmentSystem g_EquipmentSystem;
    private float smooting;                          //滑動速度
    private float normalSpeed = 5;
    private float highSpeed = 100;
    private int pageCount = 1;                           //每頁顯示的專案
    public GameObject listItem;
    private ScrollRect sRect;
    private float pageIndex;                            //總頁數
    private bool isDrag = false;                                //是否拖拽結束
    private List<float> listPageValue = new List<float> { 0 };  //總頁數索引比列 0-1


    private float m_targetPos = 0;                                //滑動的目標位置

    public float nowindex = 0;                                 //當前位置索引
    private float beginDragPos;
    private float endDragPos;

    private float sensitivity = 0.15f;                          //靈敏度

    public bool m_Button; //是否使用按鍵換頁
    public Button nextPage; //下一頁按鈕
    public Button prePage; //上一頁按鈕
    public RectTransform[] m_Equipment; //每頁大小比例
    private int onePageCount = 10;

    public Image m_Page1; //第一頁
    public Image m_Page2; //第二頁

    public Sprite m_nextImage; //第一頁標示
    public Sprite m_preImage; //第二頁標示

    void Awake(){
        sRect = this.GetComponent<ScrollRect>();
        ListPageValueInit();
        smooting = normalSpeed;
        if(m_Button){
            ButtonInit();
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialization(RectTransform[] _Equipment)
    {
        m_Equipment = _Equipment;
        listPageValue = new List<float> { 0 };
        ListPageValueInit();
        m_Equipment[(int)nowindex].localScale = new Vector3(1f, 1f, 1f);
        
    }

    //每頁比例
    void ListPageValueInit()
    {
        pageIndex = (listItem.transform.childCount / pageCount) - 1;
        if (listItem != null && listItem.transform.childCount != 0)
        {
            for (float i = 1; i <= pageIndex; i++)
            {
                listPageValue.Add((i / pageIndex));
            }
        }
    }

    void ButtonInit()
    {
        nextPage.onClick.AddListener(BtnRightGo);
        prePage.onClick.AddListener(BtnLeftGo);
    }
    void Update()
    {
        if (!isDrag)
            sRect.horizontalNormalizedPosition = Mathf.Lerp(sRect.horizontalNormalizedPosition, m_targetPos, Time.deltaTime * smooting);
        
    }

    /// <summary>
    /// 拖動開始
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        beginDragPos = sRect.horizontalNormalizedPosition;
        
    }
    
    /// <summary>
    /// 拖拽結束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        endDragPos = sRect.horizontalNormalizedPosition; //獲取拖動的值
        endDragPos = endDragPos > beginDragPos ? endDragPos + sensitivity : endDragPos - sensitivity;
        int index = 0;
        float offset = Mathf.Abs(listPageValue[index] - endDragPos);    //拖動的絕對值
        for (int i = 1; i < listPageValue.Count; i++)
        {
            float temp = Mathf.Abs(endDragPos - listPageValue[i]);
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
        }
        m_targetPos = listPageValue[index];
        
        RestoreSize();

        nowindex = index;

        if(nowindex == 0 && !m_Button){
            m_Page1.overrideSprite = m_nextImage;
            m_Page2.overrideSprite = m_preImage;
            m_Page1.SetNativeSize();
            m_Page2.SetNativeSize();
        }else if(!m_Button){
            m_Page1.overrideSprite = m_preImage;
            m_Page2.overrideSprite = m_nextImage;
            m_Page1.SetNativeSize();
            m_Page2.SetNativeSize();
        }
        Enlarge();
        
    }

    public void BtnLeftGo()
    {
        RestoreSize();
        //Debug.Log(nowindex);
        nowindex = Mathf.Clamp(nowindex - 1, 0, pageIndex);
        m_targetPos = listPageValue[Convert.ToInt32(nowindex)];
        Enlarge();
    }

    public void BtnRightGo()
    {
        RestoreSize();
        //Debug.Log(nowindex);
        nowindex = Mathf.Clamp(nowindex + 1, 0, pageIndex);
        m_targetPos = listPageValue[Convert.ToInt32(nowindex)];
        Enlarge();
    }
    /// <summary>
    /// 變回小尺寸
    /// </summary>
    private void RestoreSize(){ 
        if(m_Button){
            m_Equipment[(int)nowindex].localScale = new Vector3(0.8f, 0.8f, 1f);
        }
    }
    /// <summary>
    /// 變回大尺寸
    /// </summary>
    private void Enlarge(){
        if(m_Button){
            if(m_Equipment[(int)nowindex].localScale.x != 1)
                m_Equipment[(int)nowindex].localScale = new Vector3(1f, 1f, 1f);
                //選擇裝備數值
                g_EquipmentSystem.EquipmentBeingSelected((int)nowindex);
            if(pageIndex == nowindex){
                nextPage.gameObject.SetActive(false);
                prePage.gameObject.SetActive(true);
            }else if(0 == nowindex){
                prePage.gameObject.SetActive(false);
                nextPage.gameObject.SetActive(true);
            }else if(m_Button){
                nextPage.gameObject.SetActive(true);
                prePage.gameObject.SetActive(true);
            }
        }
    }
}