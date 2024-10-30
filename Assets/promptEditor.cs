using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class promptEditor : MonoBehaviour
{
    
    public Text itemNamesText;
    public Text scriptName;
    public Text playerCountText;
    public Text itemCountText;
    public Text playerNamesText;
    public CharacterDataLoader characterDataLoader;

    private List<string> playerNames = new List<string>();
    private List<string> itemNames = new List<string>();

    public List<string> availablePlayers = new List<string>{"Jake", "Maggie", "Lola", "Ben", "Gary", "Rachel", "Todd"};
    public List<string> availableItems = new List<string>{"Cookie", "TV Remote", "Wrench", "Broom", "Weight", "Vacume", "Spray Bottle", "Apple", "Chocolate", "Paint Brush", "Calculator", "Rubber Chicken"};

    void Start()
    {
       
       
        
    }



    public void AddPlayer()
    {
        string playerName = characterDataLoader.selectedCharacters[0].name;
       // string playerName = playerDropdown.options[playerDropdown.value].text;
        if (!playerNames.Contains(playerName))
        {
            playerNames.Add(playerName);
          
        }
    }

    public void AddItem()
    {
       string itemName = characterDataLoader.GetSingleProp();
        //string itemName = itemDropdown.options[itemDropdown.value].text;
        if (!itemNames.Contains(itemName))
        {
            itemNames.Add(itemName);
        }
    }


    public string GenerateRandomScriptName()
    {
        string[] adjectives = { "Mysteriuos", "Majestic", "Enigmatic", "Dazzing", "Quirky", "Silly", "Crazy", "Vibrant", "Fancy", "Glamorous" };
        string[] nouns = { " Cookie", " Spider", " Singer", " TV", " Girl friend", " Performance", " Opportunity", " News", " Birthday", " Chocolate" };
        string randomName = adjectives[Random.Range(0, adjectives.Length)] + nouns[Random.Range(0, nouns.Length)]; 
        // Debug.Log(scriptName.text);

        string playerCount = characterDataLoader.GetPlayerCount().ToString();

        Debug.Log("The player count is" + playerCount);
        scriptName.text = randomName;


        return randomName;


    }

    public void EditTextBox(Text textBox)
    {
        GenerateRandomScriptName();
        int playerCount = characterDataLoader.GetPlayerCount();
        int itemCount = characterDataLoader.GetProps().Count;
        string propNames = string.Join(", ", characterDataLoader.GetProps());
        string playerNames = string.Join(", ", characterDataLoader.GetPlayerNames());
        //get the list of the selected players
       

        if (itemCount > 0 &&  playerCount> 0 && !string.IsNullOrEmpty(scriptName.text))
        {
            // Create a comma-separated list of character names
          //  string characterNames = string.Join(", ", playerNames);
            
            // Create a comma-separated list of prop names
           // string propNames = string.Join(", ", itemNames);

            // Generate the script prompt
            textBox.text = "I would like you to create a 90's sitcom script that lasts about 90 seconds long. " +
                           "The script should have a clear start and end. The script should include " + 
                           playerCount + " characters. The characters' names are " + playerNames + ". " +
                           "The script should include " + itemCount + " props. These props should be " + propNames + ". " +
                           "Finally, I would also like the script to be about a " + scriptName.text + ".";

            Debug.Log(textBox.text);
            characterDataLoader.RemoveAllCharacters();


        }

        else if(itemCount == 0 && playerCount > 0 && !string.IsNullOrEmpty(scriptName.text))
        {
            // Create a comma-separated list of character names
            string characterNames = string.Join(", ", playerNames);

            textBox.text = "I would like you to create a 90's sitcom script that lasts about 90 seconds long. " +
                            "The scripts should also have a clear start and end. The script should include " + 
                            playerCount + " characters. The characters names are " + characterNames + ". " +
                            "The script should not include any props." + " Finally I would also like the script to be about a " + scriptName.text + "." ;

            Debug.Log(textBox.text);
            characterDataLoader.RemoveAllCharacters();


        }
        Debug.Log("Please add at least one player and one item to the script.");




    }

}
    