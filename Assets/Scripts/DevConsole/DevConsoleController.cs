using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class DevConsoleController : MonoBehaviour
{
    private static DevConsoleController _instance;
    public static DevConsoleController Instance
    {
        get
        { 
            if (_instance == null)
            {
                _instance = FindObjectOfType<DevConsoleController>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    [SerializeField] private InputField _inputField;
    [SerializeField] private TextMeshProUGUI _log;
    [SerializeField] private Scrollbar _logScrollbar;

    public TimeManager TimeManagerInstance;

    private string _lastCommand = "";
    
    public bool CallMethod(string method, string[] args)
    {
        try
        {
            Type type = typeof(DevConsoleCommand);
            System.Reflection.MethodInfo methodInfo = type.GetMethod(method);
            methodInfo.Invoke(method, args);
            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    private void Awake()
    {
        _instance = this;
        TimeManagerInstance = FindObjectOfType<TimeManager>();
    }

    public void ProcessCommand(string input)
    {
        if (input == "")
        {
            _inputField.text = _lastCommand;
            _inputField.ActivateInputField();
            _inputField.Select();
            _inputField.caretPosition = _inputField.text.Length;
            return;
        }
        _lastCommand = input;

        // Debug.Log("Input: " + input);
        string[] args = input.Split(' ');
        string method = args[0];
        // Debug.Log("Method: " + method);
        // method = char.ToUpper(method.First()) + method.Substring(1);
        args = args.Skip(1).ToArray();

        _log.text += "\n";

        if (!CallMethod(method, args))
        {
            _log.text += "Error: ";
        }
        _log.text += _lastCommand;

        _inputField.text = "";
        _inputField.ActivateInputField();
        _inputField.Select();
        _inputField.caretPosition = _inputField.text.Length;
    }

    public void ClearLog()
    {
        _log.text = "";
    }
}

public class DevConsoleCommand
{
    public static void clear()
    {
        DevConsoleController.Instance.ClearLog();
    }

    public static void settickjump(string tickJump)
    {
        DevConsoleController.Instance.TimeManagerInstance.SetTickJump(tickJump);
    }
    public static void settickrate(string tickRate)
    {
        DevConsoleController.Instance.TimeManagerInstance.SetTickRate(tickRate);
    }

    public static void tick()
    {
        DevConsoleController.Instance.TimeManagerInstance.TickJump();
    }
    public static void pause()
    {
        DevConsoleController.Instance.TimeManagerInstance.SetPaused(true);
    }
    public static void play()
    {
        DevConsoleController.Instance.TimeManagerInstance.SetPaused(false);
    }
}
