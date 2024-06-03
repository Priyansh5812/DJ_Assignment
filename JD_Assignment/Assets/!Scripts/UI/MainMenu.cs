using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Animator quadAnim;
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            btn.interactable = false;
            StartCoroutine(StartGame());
        });
    }

    IEnumerator StartGame()
    {
        SceneManagement.Instance.ChangeSceneAsync(1);
        quadAnim.CrossFadeInFixedTime("Fade In", 0);
        yield return new WaitForSeconds(1.25f);
        SceneManagement.Instance.TriggerLoadedScene();

    }

}
