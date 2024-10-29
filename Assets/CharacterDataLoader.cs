using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
    public List<Character> characters; // List of available characters for selection

    public Text nameText;
    public Text ageText;
    public Text descShortText;
    public Text descLongText;
    public Text quirkText;
    public Text propsText;

    //also get path of the character image folder - name = character name + Img
    public string characterImgPath = "Assets/UI/CharacterImg/";
    public RawImage characterImage;
    public List<Character> selectedCharacters = new List<Character>(); // List to hold selected characters
    public GameObject characterPanelPrefab;
    public GameObject teamPanel;
    public Button deleteButton;

    public GameObject AddCharacterPanel;
    private void Start()
    {
        LoadCharacterData();
    }

    public void LoadCharacterData()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "character.json");

        if (File.Exists(jsonPath))
        {
            string dataAsJson = File.ReadAllText(jsonPath);
            characterData = JsonUtility.FromJson<CharacterData>(dataAsJson); // Load data into characterData
            characters = characterData.characters; // Initialize characters list from loaded data
            Debug.Log("Loaded character data, total characters: " + characters.Count);
        }
        else
        {
            Debug.LogError("Cannot load character data! File not found: " + jsonPath);
        }
    }

    public void DisplayRandomCharacter()
    {
        if (characters.Count == 0)
        {
            Debug.LogError("No characters available to display.");
            return; // Exit the method if there are no characters
        }

        int randomIndex = Random.Range(0, characters.Count); // Use Count instead of 6
        Debug.Log("Random index: " + randomIndex);
        Character selectedCharacter = characters[randomIndex];
        if (!selectedCharacters.Contains(selectedCharacter))
        {
            // Add the character to the selected list
            selectedCharacters.Add(selectedCharacter);

            // Create a new UI panel for the character
            GameObject characterPanel = Instantiate(characterPanelPrefab, teamPanel.transform);
            // Set the character info in the new panel
            Text[] texts = characterPanel.GetComponentsInChildren<Text>();
            texts[0].text = selectedCharacter.name;
            texts[1].text = selectedCharacter.age.ToString();
            texts[2].text = selectedCharacter.descShort;
            texts[3].text = selectedCharacter.descLong;
            texts[4].text = selectedCharacter.quirk;

            if (selectedCharacter.props.Count > 0)
            {
                int propIndex = Random.Range(0, selectedCharacter.props.Count);
                texts[5].text = selectedCharacter.props[propIndex]; // Assuming props is the sixth text component
            }

            characters.RemoveAt(randomIndex);
        }
        else
        {
            Debug.Log("Character already selected.");

        }
    }
    public Transform characterPanelParent; // The parent object to hold all character panels
    public int maxCharacterPanels = 5; // Maximum number of character panels to display
    public void LoadCharacterCard()
    {
        // Check if there are characters available to display
        if (characters.Count == 0)
        {
            Debug.LogError("No characters available to display.");
            return;
        }
        if (characterPanelParent.childCount >= maxCharacterPanels)
        {
            //hide AddPanel
            AddCharacterPanel.SetActive(false);
            Debug.LogWarning("Maximum number of character panels reached. Cannot add more.");
            return; 
        }
        AddCharacterPanel.SetActive(true);

        // Select a random character from the list
        int randomIndex = Random.Range(0, characters.Count);
        Debug.Log("Random index: " + randomIndex);
        Character selectedCharacter = characters[randomIndex];

        // Instantiate a new character panel from the prefab
        GameObject newCharacterPanel = Instantiate(characterPanelPrefab, characterPanelParent);

        // Get the UI components from the newly created panel
        Text nameText = newCharacterPanel.transform.Find("Name").GetComponent<Text>();
        Text descShortText = newCharacterPanel.transform.Find("Description").GetComponent<Text>();
        Text propsText = newCharacterPanel.transform.Find("Props").GetComponent<Text>();
        RawImage characterImage = newCharacterPanel.transform.Find("CharacterImg").GetComponent<RawImage>();

        // Update UI text components with character details
        nameText.text = selectedCharacter.name;
        descShortText.text = selectedCharacter.descShort;

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

        Debug.Log("Loading image from: " + imgPath);

        AddCharacter(selectedCharacter);
        // Remove the character from the list to avoid re-selection
        characters.RemoveAt(randomIndex);
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

    //get the single prop that is added
    public string GetSingleProp()
    {
        string prop = "";
        foreach (Character character in selectedCharacters)
        {
            if (character.props.Count > 0)
            {
                int propIndex = Random.Range(0, character.props.Count);
                prop = character.props[propIndex];
            }
        }
        return prop;
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
    public int GetPropsCount()
    {
        return GetProps().Count;
    }
    //get playerCount
    public int GetPlayerCount()
    {
        return selectedCharacters.Count;
    }

  

}


