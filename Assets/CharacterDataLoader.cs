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
    private List<Character> availableCharacters; // List of available characters for selection
    public List<Character> selectedCharacters = new List<Character>(); // List to hold selected characters
    private int characterCounter = 1;
    public Text CharacterCounterText;
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
    public GameObject teamPanel;
    public GameObject AddCharacterPanel;
    public Canvas CharacterProfileOverlay;
    private void Start()
    {
        LoadCharacterData();
        AddCharacterPanel.SetActive(true);
        CharacterProfileOverlay.gameObject.SetActive(false);
    }

    public void LoadCharacterData()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "character.json");

        if (File.Exists(jsonPath))
        {
            string dataAsJson = File.ReadAllText(jsonPath);
            characterData = JsonUtility.FromJson<CharacterData>(dataAsJson); // Load data into characterData
            availableCharacters = characterData.characters; // Initialize characters list from loaded data
            Debug.Log("Loaded character data, total characters: " + availableCharacters.Count);
        }
        else
        {
            Debug.LogError("Cannot load character data! File not found: " + jsonPath);
        }
    }
    public void DisplayRandomCharacter()
    {
  
        int randomIndex = Random.Range(0, availableCharacters.Count);
        Character selectedCharacter = availableCharacters[randomIndex];

        if (!selectedCharacters.Contains(selectedCharacter))
        {
            selectedCharacters.Add(selectedCharacter);

            // Create and update the character UI panel here...
            GameObject characterPanel = Instantiate(characterPanelPrefab, teamPanel.transform);
            Text[] texts = characterPanel.GetComponentsInChildren<Text>();
            texts[0].text = selectedCharacter.name;
            texts[1].text = selectedCharacter.age.ToString();
            texts[2].text = selectedCharacter.descShort;
            texts[3].text = selectedCharacter.descLong;
            texts[4].text = selectedCharacter.quirk;

            if (selectedCharacter.props.Count > 0)
            {
                int propIndex = Random.Range(0, selectedCharacter.props.Count);
                texts[5].text = selectedCharacter.props[propIndex];
            }

            availableCharacters.RemoveAt(randomIndex); 
           

        }
        else
        {
            Debug.Log("Character already selected.");
        }
    }



 

    //if clicked on character name in panel, will display the character details in another scene
    public void DisplayCharacterDetails(Character character)
    {
        nameText.text = character.name;
        ageText.text = character.age.ToString();
        descShortText.text = character.descShort;
        descLongText.text = character.descLong;
        quirkText.text = character.quirk;

        if (character.props.Count > 0)
        {
            int propIndex = Random.Range(0, character.props.Count);
            propsText.text = character.props[propIndex];
        }
    }
    public Transform characterPanelParent; // The parent object to hold all character panels
    public int maxCharacterPanels = 4; // Maximum number of character panels to display
    public void LoadCharacterCard()
    {
        // Check if there are characters available to display
        if (availableCharacters.Count == 0)
        {
            Debug.Log("No available characters.");
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
        Debug.Log("Random index: " + randomIndex);
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
        Button characterDetailsButton = newCharacterPanel.transform.Find("CharacterDetailButton").GetComponent<Button>();

        // Update UI text components with character details
        nameText.text = selectedCharacter.name;
        descShortText.text = selectedCharacter.descShort;
        CharacterCounterText.text = "Character " + selectedCharacter.name;

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
        Debug.Log("Delete button: " + deleteButton.name);
        deleteButton.onClick.AddListener(() => RemoveCharacter(selectedCharacter, newCharacterPanel)); // Pass panel to remove it
        characterDetailsButton.onClick.AddListener(() => LoadCharacterDetails(selectedCharacter));

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
        Debug.Log("Removed character: " + character.name);


    }
    public void RemoveAllCharacters() 
    {
        Debug.Log("RemoveAllCharacters called successfully");
        selectedCharacters.Clear();
        availableCharacters.Clear();
        LoadCharacterData();

        //also remove all the character panels
        foreach (Transform child in characterPanelParent)
        {
            if (child.name != "AddCharacterPanel")
            {
                Destroy(child.gameObject);
            }
        }
        //if AddCharacterPanel is not active, make it active
        if (AddCharacterPanel.activeSelf == false)
        {
            AddCharacterPanel.SetActive(true);
        }
        Debug.Log("RemoveAllCharacters called successfully");

    }
    //if clicked on the name of the added character, display the character details in another scene
    public void LoadCharacterDetails(Character character)
    {
        //replace the text with the selected character details
        //get the text components from the overlay
        Text nameText = CharacterProfileOverlay.transform.Find("CharacterName").GetComponent<Text>();
        Text ageText = CharacterProfileOverlay.transform.Find("CharacterAge").GetComponent<Text>();
        Text descText = CharacterProfileOverlay.transform.Find("CharacterDescription").GetComponent<Text>();
        Text quirkText = CharacterProfileOverlay.transform.Find("CharacterQuirk").GetComponent<Text>();
        List<string> props = GetProps();
        Text propsText = CharacterProfileOverlay.transform.Find("CharacterProps").GetComponent<Text>();
        //update the text components with the selected character details

        nameText.text = character.name;
        ageText.text = (character.age.ToString() + " years old");
        descText.text = character.descLong;
        quirkText.text = character.quirk;
        //change the overlay to active
        CharacterProfileOverlay.gameObject.SetActive(true);
        //also get the path of the character image folder - name = character name + Img
        string characterImgPath = "Assets/UI/CharacterImg/";
        RawImage characterImage = CharacterProfileOverlay.transform.Find("CharacterImage").GetComponent<RawImage>();
        //load the character image
        string imgPath = Path.Combine(characterImgPath, character.name + "Img.png");
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


    }
    public void CloseCharacterDetails()
    {
        CharacterProfileOverlay.gameObject.SetActive(false);
    }


}


