using System.Collections.Generic;
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
            Debug.Log("Yo did someone say Restart?");
            GameManager.RestartGame();
        });
            
        keywords.Add("Monitor", () =>
        {
            Debug.Log("Yo did someone say Monitor?");
            GameManager.Diagnostics.GetComponent<Monitor>().Debug = !(GameManager.Diagnostics.GetComponent<Monitor>().Debug);
        });

      

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