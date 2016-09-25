using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class LaunchSceneScript : MonoBehaviour {

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public string ExerciseScene;
    public GameObject Loading;

    // Use this for initialization
    void Start()
    {
        Loading = GameObject.Find("Loading");
        Loading.GetComponent<TextMesh>().text = "";

        keywords.Add("Go", () =>
        {
            Debug.Log("Yo did someone say Go?");
            Loading.GetComponent<TextMesh>().text = "";

            SceneManager.LoadScene(ExerciseScene);
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
