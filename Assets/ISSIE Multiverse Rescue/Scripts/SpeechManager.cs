﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    GameManager GameManager;

    // Use this for initialization
    void Start()
    {
        GameManager = this.GetComponent<GameManager>();

        keywords.Add("Restart", () =>
        {
            // Call the OnReset method on every descendant object.
            Debug.Log("Yo did someone say Restart?");
            //this.BroadcastMessage("OnReset");
            GameManager.RestartGame();
        });

      /*  keywords.Add("Go", () =>
        {
            Debug.Log("Yo did someone say Go?");
            GameManager.OnGo();
        });
*/
        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}