using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLama;
using LLama.Common;
using System;
using static System.Collections.Specialized.BitVector32;
using TMPro;
using System.Text;

public class LLMInteractor : MonoBehaviour
{
    private ChatSession chatSession;
    [SerializeField] TextMeshProUGUI outputText;
    private int promptCount = 0;

    void Start()
    {
        string modelPath = Application.dataPath + "\\Scripts\\LLM\\wizardLM-7B.ggmlv3.q4_0.bin";
        var ex = new InteractiveExecutor(new LLamaModel(new ModelParams(modelPath, contextSize: 1024, seed: 1337, gpuLayerCount: 5), "UTF-8"));
        chatSession = new ChatSession(ex);
        promptCount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PromptLLM("Please name 5 blocks or items from the video game Minecraft.");
        }
    }

    public void PromptLLM(string prompt)
    {
        promptCount++;
        StringBuilder sb = new StringBuilder();

        InferenceParams iParams = new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } };
        
        if (promptCount == 1)
        {
            prompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:";
        }
        else if (promptCount == 2)
        {
            prompt = " Please recite the United States 'Pledge of Allegiance'.";
        }
        else if (promptCount == 3)
        {
            prompt = " Well done; now recite the United States 'Pledge of Allegiance', but please replace all vowels in the text with a different vowel.";
        }

        foreach (var text in chatSession.Chat(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
        {
            Debug.Log(text);
            sb.Append(text);
        }
        outputText.text = sb.ToString();
    }
}
