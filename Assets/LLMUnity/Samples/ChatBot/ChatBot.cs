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
        public Text promptText;
        
        [SerializeField] private TextMeshProUGUI aiText;
        private string _aiResponseText;
        
        private Dictionary<string, string> _characterColors;
        
        public float scrollSpeed = 50;
        
        void Start()
        {
            
            _characterColors = new Dictionary<string, string>
            {
                { "JIM", "<color=#00FF00>" }, // Green
                { "LUCY", "<color=#FF0000>" }, // Red
                { "ALLEN", "<color=#0000FF>" }, // Blue
                
                { "Jim", "<color=#00FF00>" }, // Green
                { "Lucy", "<color=#FF0000>" }, // Red
                { "Allen", "<color=#0000FF>" } // Blue
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

        bool onValidateWarning = true;
        void OnValidate()
        {
            if (onValidateWarning && !llmCharacter.remote && llmCharacter.llm != null && llmCharacter.llm.model == "")
            {
                Debug.LogWarning($"Please select a model in the {llmCharacter.llm.gameObject.name} GameObject!");
                onValidateWarning = false;
            }
        }
        
        public void SetAIResponseText(string responseText)
        {
            // Color the character names in the response text
            foreach (var character in _characterColors)
            {
                responseText = responseText.Replace(character.Key, character.Value + character.Key + "</color>");
            }

            // Set the formatted text
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
