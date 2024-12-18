using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using LLMUnity;
using TMPro;
using UnityEngine.UI;

namespace LLMUnitySamples
{
    public class ChatBot : MonoBehaviour
    {
        public LLMCharacter llmCharacter;
        
        [SerializeField] private TextMeshProUGUI aiText;
        private string _aiResponseText;
        private bool _finishedTextGeneration;
        
        private Dictionary<string, string> _characterColors;

        
        private Coroutine _aiTextTimerCoroutine;
        private bool _startedTextGeneration;

        
        public float scrollSpeed = 50;


        public Text promptText;

        void Start()
        {
            
            _characterColors = new Dictionary<string, string>
            {
                { "Jake", "<color=#FFD1BA>" },  // Apricot
                { "JAKE", "<color=#FFD1BA>" },  // Apricot (Uppercase)
    
                { "Maggie", "<color=#22AED1>" }, // Pacific Cyan
                { "MAGGIE", "<color=#22AED1>" }, // Pacific Cyan (Uppercase)

                { "Ben", "<color=#64F58D>" },    // Spring Green
                { "BEN", "<color=#64F58D>" },    // Spring Green (Uppercase)

                { "Lola", "<color=#F03A47>" },   // Imperial Red
                { "LOLA", "<color=#F03A47>" },   // Imperial Red (Uppercase)

                { "Rachel", "<color=#E6C229>" }, // Saffron
                { "RACHEL", "<color=#E6C229>" }, // Saffron (Uppercase)

                { "Gary", "<color=#6610F2>" },   // Electric Indigo
                { "GARY", "<color=#6610F2>" },   // Electric Indigo (Uppercase)

                { "Todd", "<color=#870058>" },   // Murrey
                { "TODD", "<color=#870058>" }    // Murrey (Uppercase)
            };

            
        }


        public void StartGeneration()
        {

            Task chatTask = llmCharacter.Chat(promptText.text, (responseText) =>
            {
                _aiResponseText = responseText;
            },OnFullResponseReceived);
            
        }
        
        public void OnFullResponseReceived()
        {
            SetAIResponseText(_aiResponseText);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
        
        public void SetAIResponseText(string responseText)
        {
            foreach (var character in _characterColors)
            {
                responseText = responseText.Replace(character.Key, character.Value + character.Key + "</color>");
            }

            aiText.text = responseText;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(aiText.GetComponent<RectTransform>());


            StartCoroutine(ScrollText());
        }

        private IEnumerator ScrollText()
        {
            RectTransform rectTransform = aiText.GetComponent<RectTransform>();
    
            float startY = rectTransform.anchoredPosition.y;
            
            float scrollHeightLimit = rectTransform.rect.height;
            
            while (rectTransform.anchoredPosition.y < scrollHeightLimit * 2)
            {
                rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
                yield return null; 
            }
            rectTransform.anchoredPosition = new Vector2(0,startY);
            aiText.text = "Loading...";
        }

    }
}
