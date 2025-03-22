using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string Scene;
    [SerializeField] Image _image ;
    [SerializeField] Sprite deadScreen,escapedScreen;
    [SerializeField] Button restart,lastCheckpoint, mainMenu;
        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == "DeathScreen")
            {
                _image.sprite = Player.isDead ? deadScreen : escapedScreen;
                lastCheckpoint.gameObject.SetActive(Player.isDead);
            }
          
        }
    
        private void Start()
        {
       
        }

        public void LoadScene(string scene)
        {
            InputManager.DisableInGame();
            SceneManager.LoadScene(scene);
        }
        public void  goToMainMenu()
        {
            Scene = "FinalMainMenu";
            SceneManager.LoadScene(Scene);
        }
    
        public void restartGame()
        {
            PlayerPrefs.SetInt("MeltingStage", 5);
            Scene = "NewMap";
            SceneManager.LoadScene(Scene);
        }
    
        public void GoToLastCheckpoint()
        {
            GameManager.loaded = true;
            SceneManager.LoadScene("NewMap");
        }
    public void mainScene()
    {
        SceneManager.LoadScene(Scene);
    }
}
