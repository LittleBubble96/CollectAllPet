using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading_UI : UIBase
{
    [SerializeField][Bubble_Name("进度条")] private Slider m_Slider;
    [SerializeField] private TextMeshProUGUI m_DesText;
    [SerializeField] private TextMeshProUGUI m_ProgressText;
    [SerializeField][Bubble_Name("增长进度")] private float m_Speed = 1;
    
    private float startProgress = 0;
    private float endProgress = 0;
    
    
    public override void OnInit()
    {
        base.OnInit();
        m_Slider.value = 0;
        m_DesText.text = "Loading...";
        m_ProgressText.text = "0%";
        startProgress = 0;
        endProgress = 0;
    }

    public override void OnShow()
    {
        base.OnShow();
        GameManager.GetAppEventDispatcher().AddEventListener<MultiEvent<float>>(EventName.EVENT_LoadingUIProcess, OnLoadingProgress);
    }
    
    public override void OnHide()
    {
        base.OnHide();
        GameManager.GetAppEventDispatcher().RemoveEventListener<MultiEvent<float>>(EventName.EVENT_LoadingUIProcess, OnLoadingProgress);
    }

    private void OnLoadingProgress(MultiEvent<float> obj)
    {
        SetProgress(obj.Value);
    }
    
    public void SetProgress(float progress)
    {
        startProgress = m_Slider.value;
        endProgress = progress;
    }


    public override void DoUpdate(float dt)
    {
        base.DoUpdate(dt);
        if (startProgress < endProgress)
        {
            startProgress += m_Speed * dt;
            if (startProgress > endProgress)
            {
                startProgress = endProgress;
            }
            m_Slider.value = startProgress;
            m_ProgressText.text = $"{(int)(startProgress * 100)}%";
        }

        if (startProgress >= 1)
        {
            Hide();
        }
    }
}