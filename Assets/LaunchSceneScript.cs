using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using System;

public class LaunchSceneScript : MonoBehaviour {

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public string ExerciseScene;
    public GameObject Loading;

    Color selectedColor = Color.yellow;

    // Use this for initialization
    void Start()
    {
        Loading = GameObject.Find("Loading");
        //Loading.GetComponent<TextMesh>().text = "Multiverse Rescue";

        keywords.Add("Go", () =>
        {
            Debug.Log("Yo did someone say Go?");
            OnGo();
        });

        keywords.Add("Cycle", () =>
        {
            StartGame("Cycle");
        });

        keywords.Add("Warmup", () =>
        {
            StartGame("Warmup");
        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void SetSelectedColor(string selectedRoutine)
    {
        GameObject.Find(selectedRoutine).GetComponent<Renderer>().material.color = selectedColor;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void Update()
    {
        if (Input.GetAxis("Jump") != 0.0f)
        {
            OnGo();
        }

        if(Input.GetKey("1"))
        {
            StartGame("Cycle");
        }

        if (Input.GetKey("2"))
        {
            StartGame("Warmup");
        }

    }

    public void OnGo()
    {
        Loading.GetComponent<TextMesh>().text = "Loading ";// + ExerciseScene;
        SceneManager.LoadScene(ExerciseScene);
    }

    public void StartGame(String selectedRoutine)
    {
        SetSelectedColor(selectedRoutine);
        ExerciseScene = selectedRoutine;
        OnGo();
    }
     
}
