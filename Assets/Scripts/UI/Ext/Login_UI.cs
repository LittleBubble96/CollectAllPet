
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_UI : UIBase
{
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _loginButton;
    // Start is called before the first frame update
    void Start()
    {
        _loginButton.onClick.AddListener(() =>
        {
            string username = _usernameInput.text;
            string password = _passwordInput.text;
            ClientRequestFunc.SendLoginRequest(username, password);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
