using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] private int lettersPerSecond;

    private Action onDialogFinished;
    
    private Dialog dialog;

    private int currentline = 0;
    private bool isShowing;
    private bool isInConverse;
    public bool IsInConverse
    {
        get => isInConverse;
    }

    public event Action OnShowDialog;
    public event Action OnCloseDialog;
    
    public static DialogManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public IEnumerator ShowDialog(Dialog dialog, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog.Invoke();
        
        isInConverse = true;
        this.dialog = dialog;

        onDialogFinished = onFinished;
        
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isShowing)
        {
            ++currentline;

            if (currentline < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentline]));
            }
            else
            {
                currentline = 0;
                isInConverse = false;
                dialogBox.SetActive(false);
                onDialogFinished.Invoke();
                OnCloseDialog.Invoke();
            }
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        isShowing = true;
        
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isShowing = false;
    }
}
