using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using LLMUnity;
using TMPro;
using UnityEngine.UI;

namespace LLMUnitySamples
{
    public class ChatBot : MonoBehaviour
    {
        public Transform chatContainer;
        public Color playerColor = new Color32(81, 164, 81, 255);
        public Color aiColor = new Color32(29, 29, 73, 255);
        public Color fontColor = Color.white;
        public Font font;
        public int fontSize = 16;
        public int bubbleWidth = 600;
        public LLMCharacter llmCharacter;
        public float textPadding = 10f;
        public float bubbleSpacing = 10f;
        public Sprite sprite;

        private List<Bubble> chatBubbles = new List<Bubble>();
        private BubbleUI aiUI;
        private int lastBubbleOutsideFOV = -1;

        [SerializeField] private TextMeshProUGUI aiText;
        private string _aiResponseText;
        private bool _finishedTextGeneration;
        
        private Dictionary<string, string> _characterColors;

        
        private Coroutine _aiTextTimerCoroutine;
        private bool _startedTextGeneration;

        
        public float scrollSpeed = 50;

        public Button startButton;

        void Start()
        {
            if (font == null) font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            aiUI = new BubbleUI
            {
                sprite = sprite,
                font = font,
                fontSize = fontSize,
                fontColor = fontColor,
                bubbleColor = playerColor,
                bottomPosition = 0,
                leftPosition = 0,
                textPadding = textPadding,
                bubbleOffset = bubbleSpacing,
                bubbleWidth = bubbleWidth,
                bubbleHeight = -1
            };
            aiUI.bubbleColor = aiColor;
            aiUI.leftPosition = 1;
            
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
            Bubble aiBubble = new Bubble(chatContainer, aiUI, "AIBubble", "...");
            chatBubbles.Add(aiBubble);
            
            Task chatTask = llmCharacter.Chat("write a unique script incorporating 3 characters", (responseText) =>
            {
                aiBubble.SetText(responseText);
                _aiResponseText = responseText;
                if (_aiTextTimerCoroutine != null)
                {
                    StopCoroutine(_aiTextTimerCoroutine);
                    _aiTextTimerCoroutine = null;
                }
                _aiTextTimerCoroutine = StartCoroutine(AITextTimer());
            },OnFullResponseReceived);
            
            _startedTextGeneration = true;
        }
        
        public void OnFullResponseReceived()
        {
            Debug.Log("Full response received");
        }
        
        private IEnumerator AITextTimer()
        {
            float timer = 1f;

            while (timer > 0)
            {
                timer -= Time.deltaTime; 
                yield return null; 
            }
            _finishedTextGeneration = true;
        }
        void Update()
        {
            if (lastBubbleOutsideFOV != -1)
            {
                // destroy bubbles outside the container
                for (int i = 0; i <= lastBubbleOutsideFOV; i++)
                {
                    chatBubbles[i].Destroy();
                }
                chatBubbles.RemoveRange(0, lastBubbleOutsideFOV + 1);
                lastBubbleOutsideFOV = -1;
            }

            if (_startedTextGeneration)
            {
                StartCoroutine(UpdateAIText());
                _startedTextGeneration = false;
            }
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

        private IEnumerator UpdateAIText()
        {
            yield return new WaitUntil(() => _finishedTextGeneration);
            SetAIResponseText(_aiResponseText);
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
        }

    }
}
