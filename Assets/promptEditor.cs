using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class promptEditor : MonoBehaviour
{
    
    public Text scriptName;
    
    public CharacterDataLoader characterDataLoader;
    
    public void GenerateRandomScriptName()
    {
        string[] adjectives = { "Mysterious", "Majestic", "Enigmatic", "Dazzling", "Quirky", "Silly", "Crazy", "Vibrant", "Fancy", "Glamorous" };
        string[] nouns = { " Cookie", " Spider", " Singer", " TV", " Girl friend", " Performance", " Opportunity", " News", " Birthday", " Chocolate" };
        string randomName = adjectives[Random.Range(0, adjectives.Length)] + nouns[Random.Range(0, nouns.Length)]; 
        
        scriptName.text = randomName;
    }

    public void EditTextBox(Text textBox)
    {
        GenerateRandomScriptName();
        int playerCount = characterDataLoader.GetPlayerCount();
        int itemCount = characterDataLoader.GetProps().Count;
        string propNames = string.Join(", ", characterDataLoader.GetProps());
        string playerNames = string.Join(", ", characterDataLoader.GetPlayerNames());
       

        if (itemCount > 0 &&  playerCount> 0 && !string.IsNullOrEmpty(scriptName.text))
        {

            // Generate the script prompt
            textBox.text = "I would like you to create a 90's sitcom script that lasts about 90 seconds long. " +
                           "The script should have a clear start and end. The script should include " + 
                           playerCount + " characters. The characters' names are " + playerNames + ". " +
                           "No new characters should be introduced" +
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
                            "No new characters should be introduced" +
                            "The script should not include any props." + " Finally I would also like the script to be about a " + scriptName.text + "." ;

            Debug.Log(textBox.text);
            characterDataLoader.RemoveAllCharacters();


        }
        Debug.Log("Please add at least one player and one item to the script.");




    }

}
    