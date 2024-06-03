using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreGUI;
    public TextMeshProUGUI timerGUI;
    public Animator quadAnim;
    private int score = 0;
    public int secs = 60;
    public StaticHandGesture gesture;

    void Start()
    {
        scoreGUI.text = score.ToString();
        ProjectileEvent.Service.onProjectileHit.AddListener(AddScore);
        StartCoroutine(TimerCoroutine());

    }



    IEnumerator TimerCoroutine()
    {
        while (secs != 0)
        {
            secs--;
            timerGUI.text = secs.ToString();
            if (secs == 30)
                SceneManagement.Instance.ChangeSceneAsync(0);
            yield return new WaitForSeconds(1);
        }
        gesture.gesturePerformed.RemoveAllListeners();
        gesture.gestureEnded.RemoveAllListeners();
        GameEvent.Service.onGameEnded.InvokeEvent();
        yield return new WaitForSeconds(2);
        quadAnim.CrossFade("Fade In", 0);
        yield return new WaitForSeconds(1.25f);
        SceneManagement.Instance.TriggerLoadedScene();
    }

    private void AddScore(int score)
    {
        this.score += score;
        scoreGUI.text = this.score.ToString();
    }
}
