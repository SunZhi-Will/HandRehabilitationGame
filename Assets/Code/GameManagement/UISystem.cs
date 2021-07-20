using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class UISystem : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    
    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void LeaveTheGame(){
        Application.Quit();
    }
    /// <summary>
    /// 清除紀錄
    /// </summary>
    public void ClearGame(){
        PlayerPrefs.DeleteAll();
        BackToMainScreen();
    }
    /// <summary>
    /// 回主畫面
    /// </summary>
    public void BackToMainScreen(){
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartTheGame(){
        SceneManager.LoadScene(2);
    }
}
