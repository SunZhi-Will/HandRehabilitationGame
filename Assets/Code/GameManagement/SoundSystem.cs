using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 聲音系統
/// </summary>
public class SoundSystem : MonoBehaviour
{
    [SerializeField]
    [Header("聲音來源")]
    private AudioSource[] g_Otohara;
    
    [SerializeField]
    [Header("有無音量條")]
    private bool g_BoolVolumeBar;
    [SerializeField]
    [Header("音量條")]
    private GameObject g_VolumeBar;
    [SerializeField]
    [Header("無音量顏色塊")]
    private Sprite g_NoVolumeColorBlock;
    [SerializeField]
    [Header("音量顏色塊")]
    private Sprite g_ColorBlock;

    /// <summary>
    /// 音量區塊
    /// </summary>
    private Image[] m_VolumePiece;
    /// <summary>
    /// 音量按鈕
    /// </summary>
    private Button[] m_VolumeButton;
    void Start()
    {
        if(g_BoolVolumeBar){
            m_VolumePiece = g_VolumeBar.GetComponentsInChildren<Image>();
            m_VolumeButton = g_VolumeBar.GetComponentsInChildren<Button>();
            for (int i = 0; i < m_VolumePiece.Length; i++)
            {
                ButtonTriggerValueTransfer(i);
            }
        }
        StartCoroutine(FadeIn(RecordSystem.m_Volume));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private float _NowTheVolumeTS;
    /// <summary>
    /// 淡入
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn(float _volume){
        yield return new WaitForSeconds(0.001f);
        _NowTheVolumeTS += _volume * Time.deltaTime;
        VolumeAdjustment(_NowTheVolumeTS);
        if(_NowTheVolumeTS < _volume){
            StartCoroutine(FadeIn(_volume));
        }else{
            VolumeAdjustment(_volume);
        }

    }
    /// <summary>
    /// 按鍵觸發傳值
    /// <para>for 迴圈跑得比 AddListener 快</para>
    /// <para>所以 迴圈跑完 的時候 AddListener 才會被執行</para>
    /// <para>為此做了這函式，以防上述事情</para>
    /// </summary>
    private void ButtonTriggerValueTransfer(int _Number){
        m_VolumeButton[_Number].onClick.AddListener(delegate{VolumeAdjustment(_Number + 1);});
    }
    /// <summary>
    /// 設定音量
    /// </summary>
    /// <param name="_volume">音量 1~10</param>
    public void VolumeAdjustment(float _volume){
        if(_volume >= 0 && _volume <= 10){
            RecordSystem.m_Volume = _volume;
            foreach (var item in g_Otohara)
            {
                item.volume = RecordSystem.m_Volume / 10;
            }
            
            VolumeBarAdjustment();
        }
        
    }
    /// <summary>
    /// 音量條調整
    /// </summary>
    private void VolumeBarAdjustment(){
        if(g_BoolVolumeBar){
            for (int i = 0; i < m_VolumePiece.Length; i++)
            {
                if(RecordSystem.m_Volume >= i + 1){
                    m_VolumePiece[i].overrideSprite = g_ColorBlock;
                }else{
                    m_VolumePiece[i].overrideSprite = g_NoVolumeColorBlock;
                }
                
            }
        }
    }
    /// <summary>
    /// 增加音量
    /// </summary>
    public void AddVolume(int _volume){

        VolumeAdjustment(RecordSystem.m_Volume + _volume);
    }

}
