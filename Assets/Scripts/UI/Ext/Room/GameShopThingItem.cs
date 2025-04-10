using System;
using UnityEngine;
using UnityEngine.UI;

public class GameShopThingItem : MonoBehaviour
{
    [SerializeField] private int payNum = 0;
    private Button addOneBtn;
    private Button addTenBtn;
    private Button subOneBtn;
    private Button subTenBtn;
    private Text countText;
    
    public int Count { get; private set; } = 0;
    public int PayNum => payNum;
    public Action<int> OnCountChange;

    private void Awake()
    {
        addOneBtn = transform.Find("Count/+1").GetComponent<Button>();
        addTenBtn = transform.Find("Count/+10").GetComponent<Button>();
        subOneBtn = transform.Find("Count/-1").GetComponent<Button>();
        subTenBtn = transform.Find("Count/-10").GetComponent<Button>();
        countText = transform.Find("Count/Num").GetComponent<Text>();
        
        addOneBtn.onClick.AddListener(OnClickAddOne);
        addTenBtn.onClick.AddListener(OnClickAddTen);
        subOneBtn.onClick.AddListener(OnClickSubOne);
        subTenBtn.onClick.AddListener(OnClickSubTen);
    }
    
    private void OnClickAddOne()
    {
        Count++;
        UpdateCountText();
    }
    
    private void OnClickAddTen()
    {
        Count += 10;
        UpdateCountText();
    }
    
    private void OnClickSubOne()
    {
        Count--;
        if (Count < 0)
        {
            Count = 0;
        }
        UpdateCountText();
    }
    
    private void OnClickSubTen()
    {
        Count -= 10;
        if (Count < 0)
        {
            Count = 0;
        }
        UpdateCountText();
    }
    
    private void UpdateCountText()
    {
        countText.text = Count.ToString();
        OnCountChange?.Invoke(Count);
    }
}