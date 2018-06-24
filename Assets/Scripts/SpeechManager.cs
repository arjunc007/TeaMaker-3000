using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Academy.HoloToolkit.Unity;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        keywords.Add("Reset world", () =>
        {
            // Call the OnReset method on every descendant object.
            GameManager.Instance.OnReset();
        });

        keywords.Add("Add Water", () =>
        {
            GameManager.Instance.AddWaterToKettle();
        });

        keywords.Add("Switch On Kettle", () =>
        {
            GameManager.Instance.TurnOnKettle();
        });

        keywords.Add("Add Teabag", () =>
        {
            GameManager.Instance.AddTeabag();
        });

        keywords.Add("Add Sugar", () =>
        {
            GameManager.Instance.AddSugar();
        });

        keywords.Add("Add Milk", () =>
        {
            GameManager.Instance.AddMilk();
        });

        keywords.Add("Add Hot Water", () =>
        {
            GameManager.Instance.AddHotWater();
        });

        keywords.Add("Remove Teabag", () =>
        { 
            GameManager.Instance.RemoveTeabag();
        });

        keywords.Add("Mix Tea", () =>
        {
            GameManager.Instance.MixTea();
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