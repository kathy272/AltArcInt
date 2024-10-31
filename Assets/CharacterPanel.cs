using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterPanel : MonoBehaviour
{
    public SceneManager sceneManager;

    public void OnPanelClick()
    {
        CharacterManager.Instance.SetSelectedCharacter(sceneManager);
    }
}

