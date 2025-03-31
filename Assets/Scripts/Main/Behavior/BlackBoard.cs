using System.Collections.Generic;

public class BlackBoard
{
    private Dictionary<string, object> _blackBoard = new Dictionary<string, object>();
    
    public void SetValue(string key, object value)
    {
        if (_blackBoard.ContainsKey(key))
        {
            _blackBoard[key] = value;
        }
        else
        {
            _blackBoard.Add(key, value);
        }
    }
    
    public object GetValue(string key)
    {
        if (_blackBoard.ContainsKey(key))
        {
            return _blackBoard[key];
        }
        return null;
    }
    
    public T GetValue<T>(string key)
    {
        if (_blackBoard.ContainsKey(key))
        {
            return (T)_blackBoard[key];
        }
        return default(T);
    }
    
    public void RemoveValue(string key)
    {
        if (_blackBoard.ContainsKey(key))
        {
            _blackBoard.Remove(key);
        }
    }
    
    public void Clear()
    {
        _blackBoard.Clear();
    }
    
    public bool ContainsKey(string key)
    {
        return _blackBoard.ContainsKey(key);
    }
}