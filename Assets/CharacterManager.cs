using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance; // Singleton instance
    private CharacterDataLoader CharacterDataLoader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign this instance to the static instance
            DontDestroyOnLoad(gameObject); // Optional: Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }


    public Character selectedCharacter;


    
    public void SetSelectedCharacter(Character character, SceneManager sceneManager)
    {
        selectedCharacter = character;
        sceneManager.LoadScene("ProfilePage"); 
    }
    public void RemoveCharacter(Character character, GameObject characterPanel)
    {
      CharacterDataLoader.selectedCharacters.Remove(character);
        Destroy(characterPanel); // Destroy the panel from the UI
        Debug.Log("Removed character: " + character.name);
    }
}
