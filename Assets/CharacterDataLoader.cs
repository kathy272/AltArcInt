using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterDataLoader : MonoBehaviour
{
  
    public Character character;
    [System.Serializable]
    public class CharacterData
    {
        public List<Character> characters;
    }


    public CharacterData characterData; // Keep this to hold the loaded character data
    private List<Character> availableCharacters; // List of available characters for selection
    public List<Character> selectedCharacters = new List<Character>(); // List to hold selected characters
    private int characterCounter = 1;
    private Text nameText;
    private Text ageText;
    private Text descShortText;
    private Text descLongText;
    private Text quirkText;
    private Text propsText;

    //also get path of the character image folder - name = character name + Img
    private string characterImgPath = "Assets/UI/CharacterImg/";
    private RawImage characterImage;
    public GameObject characterPanelPrefab;
    public GameObject AddCharacterPanel;
    private void Start()
    {
        LoadCharacterData();
        AddCharacterPanel.SetActive(true);
    }

    public void LoadCharacterData()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "character.json");

        if (File.Exists(jsonPath))
        {
            string dataAsJson = File.ReadAllText(jsonPath);
            characterData = JsonUtility.FromJson<CharacterData>(dataAsJson); // Load data into characterData
            availableCharacters = characterData.characters; // Initialize characters list from loaded data
        }
        else
        {
            Debug.LogError("Cannot load character data! File not found: " + jsonPath);
        }
    }

    
    public Transform characterPanelParent;
    public Transform scriptLoadingParent;// The parent object to hold all character panels
    public int maxCharacterPanels = 4; // Maximum number of character panels to display
    public void LoadCharacterCard()
    {
        // Check if there are characters available to display
        if (availableCharacters.Count == 0)
        {
            return;
        }
        if (selectedCharacters.Count == maxCharacterPanels)
        {
            //hide AddPanel
            AddCharacterPanel.SetActive(false);
            Debug.LogWarning("Maximum number of character panels reached. Cannot add more.");
            return; 
        }

        // Select a random character from the list
        int randomIndex = Random.Range(0, availableCharacters.Count);
        Character selectedCharacter = new List<Character>(availableCharacters)[randomIndex];

        // Instantiate a new character panel from the prefab
        GameObject newCharacterPanel = Instantiate(characterPanelPrefab, characterPanelParent);
        
        // Get the UI components from the newly created panel
        Text nameText = newCharacterPanel.transform.Find("Name").GetComponent<Text>();
        Text descShortText = newCharacterPanel.transform.Find("Description").GetComponent<Text>();
        Text propsText = newCharacterPanel.transform.Find("Props").GetComponent<Text>();
        Text CharacterCounterText = newCharacterPanel.transform.Find("CharacterNumber").GetComponent<Text>();
        RawImage characterImage = newCharacterPanel.transform.Find("CharacterImg").GetComponent<RawImage>();
        Button deleteButton = newCharacterPanel.transform.Find("DeleteButton").GetComponent<Button>();
        Text characterColor = newCharacterPanel.transform.Find("Color").GetComponent<Text>();


        // Update UI text components with character details
        nameText.text = selectedCharacter.name;
        descShortText.text = selectedCharacter.descShort;
        CharacterCounterText.text = "Character " + selectedCharacter.name;
        characterColor.text = "Color: " + selectedCharacter.charColor;

        // Get a random prop
        if (selectedCharacter.props.Count > 0) // Check if there are props available
        {
            int propIndex = Random.Range(0, selectedCharacter.props.Count);
            propsText.text = "Props: " + selectedCharacter.props[propIndex]; // Display one random prop
          //  AddProp(selectedCharacter.props[propIndex]);
        }
        else
        {
            propsText.text = "No props available";
        }
        deleteButton.onClick.AddListener(() => RemoveCharacter(selectedCharacter, newCharacterPanel)); // Pass panel to remove it

        // Load character image
        string imgPath = Path.Combine(characterImgPath, selectedCharacter.name + "Img.png");
        if (File.Exists(imgPath))
        {
            byte[] fileData = File.ReadAllBytes(imgPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            characterImage.texture = texture; // Assuming characterImage is a RawImage
        }
        else
        {
            Debug.LogError("Cannot load character image! File not found: " + imgPath);
        }

        
        
        AddCharacter(selectedCharacter);
        // Remove the character from the list to avoid re-selection
        availableCharacters.RemoveAt(randomIndex);
    }

    //get all the playerNames that are added
    public List<string> GetPlayerNames()
    {
        List<string> playerNames = new List<string>();
        foreach (Character character in selectedCharacters)
        {
            playerNames.Add(character.name);
        }
        return playerNames;
    }

    //get all the props that are added
    public List<string> GetProps()
    {
        List<string> props = new List<string>();
        foreach (Character character in selectedCharacters)
        {
            if (character.props.Count > 0)
            {
                int propIndex = Random.Range(0, character.props.Count);
                props.Add(character.props[propIndex]);
            }
        }
        return props;
    }
    
    //add character to list
    public void AddCharacter(Character character)
    {
        selectedCharacters.Add(character);
    }
    //add prop to list
    public void AddProp(string prop)
    {
        selectedCharacters.Add(character);
    }
    //get playerCount
    public int GetPlayerCount()
    {
        return selectedCharacters.Count;
    }

    //if clicked on delete button, remove the character from the list
    public void RemoveCharacter(Character character, GameObject characterPanel)
    {
        if (AddCharacterPanel.activeSelf == false)
        {
            AddCharacterPanel.SetActive(true);
        }
        selectedCharacters.Remove(character); // Remove character from list
        Destroy(characterPanel); // Destroy the panel from the UI
        availableCharacters.Add(character); // Add character back to available list


    }
    public void RemoveAllCharacters() 
    {
        Debug.Log("1");
        selectedCharacters.Clear();
        Debug.Log("2");
        availableCharacters.Clear();
        Debug.Log("3");
        LoadCharacterData();
        Debug.Log("4");
        
        foreach (Transform child in scriptLoadingParent)
        {
            Debug.Log("5");
            Destroy(child.gameObject);
        }   
    }


    public void MoveCharactersToSecondScreen()
    {
        List<Transform> childrenToMove = new List<Transform>();

        foreach (Transform child in characterPanelParent)
        {
            if (child.name != "AddCharacterPanel")
            {
                childrenToMove.Add(child); // Collect the children
            }
        }

        foreach (Transform child in childrenToMove)
        {
            child.SetParent(scriptLoadingParent);

            // Disable the DeleteButton
            Transform deleteButtonTransform = child.Find("DeleteButton");
            if (deleteButtonTransform != null)
            {
                Button deleteButton = deleteButtonTransform.GetComponent<Button>();
                if (deleteButton != null)
                {
                    deleteButton.gameObject.SetActive(false);
                }
            }
        }
    }

}


