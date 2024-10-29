using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance; // Singleton instance

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
}
