using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using LLMUnity;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LLMUnitySamples
{
    public class ChatBot : MonoBehaviour
    {
        public LLMCharacter llmCharacter;
        public Text promptText;

        public CharacterDataLoader characterDataLoaderRef;
        
        [SerializeField] private TextMeshProUGUI aiText;
        private string _aiResponseText;
        
        private Dictionary<string, string> _characterColors;
        
        public float scrollSpeed = 50;

        public GameObject addCharactersBtn;
        public Button startGenerationBtn;
        public GameObject scrollSpeeds;
        
        void Start()
        {
            scrollSpeeds.SetActive(false);
            
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
            addCharactersBtn.SetActive(false);
            startGenerationBtn.gameObject.SetActive(false);
            scrollSpeeds.SetActive(true);
            
            
            Task chatTask = llmCharacter.Chat(promptText.text, (responseText) =>
            {
                Debug.Log("Loading...");
                _aiResponseText = responseText;
            },OnFullResponseReceived);
        }
        
        public void OnFullResponseReceived()
        {
            SetAIResponseText(_aiResponseText);

            characterDataLoaderRef.RemoveAllCharacters();
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
            
            
            scrollSpeeds.SetActive(false);
            startGenerationBtn.gameObject.SetActive(true);
            addCharactersBtn.SetActive(true);
            
            
            aiText.text = "Loading...";
        }

        
        public void SetScrollSpeedLow()
        {
            scrollSpeed = 30;
        }
        public void SetScrollSpeedMed()
        {
            scrollSpeed = 50;
        }
        public void SetScrollSpeedHigh()
        {
            scrollSpeed = 70;
        }
    }
}
