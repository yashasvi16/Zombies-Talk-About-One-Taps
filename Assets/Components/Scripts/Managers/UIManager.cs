using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEditor.Rendering;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panel")]
    [SerializeField] GameObject _inGame;
    [SerializeField] GameObject _menu;

    [Header("Text Fields")]
    [SerializeField] TMP_Text commandTXT;
    [SerializeField] TMP_Text instructionTXT;
    [SerializeField] TMP_Text scoreTXT;

    [Header("Transforms")]
    [SerializeField] Transform _animationStartTransform;
    [SerializeField] Transform _animationEndTransform;
    [SerializeField] RectTransform _healthForeground;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public void UpdateScore(int score)
    {
        scoreTXT.text = "Kill Count: " + score.ToString();
    }

    public void StartMenu()
    {
        _menu.SetActive(true);
    }

    public void StartInGame()
    {
        _inGame.SetActive(true);
    }

    public void ShowCommand()
    {
        commandTXT.gameObject.SetActive(true);
    }
    public void HideCommand()
    {
        commandTXT.gameObject.SetActive(false);
    }

    public void ShowInstruction()
    {
        instructionTXT.gameObject.SetActive(true);
        instructionTXT.transform.DOMove(_animationEndTransform.position, 1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            StartCoroutine(IEHideInstruction());
        });
    }

    IEnumerator IEHideInstruction()
    {
        yield return new WaitForSeconds(2.5f);
        instructionTXT.transform.DOMove(_animationStartTransform.position, 1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            instructionTXT.gameObject.SetActive(false);
        });
    }

    public void UpdatePlayerHealth(int health)
    {
        _healthForeground.sizeDelta = new Vector2(health * 2f, _healthForeground.sizeDelta.y);
        Debug.Log(health);
    }
}
