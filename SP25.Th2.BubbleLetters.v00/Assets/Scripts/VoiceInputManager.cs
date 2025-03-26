using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;

public class VoiceInputManager : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public TextMeshProUGUI displayText;
    private string currentText = "";

    void Start()
    {
        // Initialize the keywords dictionary with letters A-Z
        for (char c = 'A'; c <= 'Z'; c++)
        {
            string letter = c.ToString();
            keywords.Add(letter, () => AddLetter(letter));
        }

        // Create and start the keyword recognizer
        keywordRecognizer = new KeywordRecognizer(new List<string>(keywords.Keys).ToArray());
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        Debug.Log("Voice recognition started");
            
            
        // Debug microphone detection
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            Debug.Log("Detected microphones:");
            foreach (string device in devices)
            {
                Debug.Log("- " + device);
            }
        }
        else
        {
            Debug.LogWarning("No microphone detected!");
        }
    
    
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void AddLetter(string letter)
    {
        currentText += letter;
        if (displayText != null)
        {
            displayText.text = currentText;
        }
        Debug.Log($"Letter added: {letter}");
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }




}