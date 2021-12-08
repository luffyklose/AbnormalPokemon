using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogBox : MonoBehaviour
{
    [SerializeField] private Text dialogText;
    [SerializeField] private Text ppText;
    [SerializeField] private Text typeText;
    [SerializeField] private GameObject actionSelector;
    [SerializeField] private GameObject moveSelector;
    [SerializeField] private GameObject moveDetails;

    [SerializeField] private List<Text> actionTexts;
    [SerializeField] private List<Text> moveTexts;
    
    [SerializeField] private int lettersPerSecond;
    [SerializeField] private Color highlightColor;
    
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1.0f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public int getActionNumber()
    {
        return actionTexts.Count;
    }
    
    public int getMoveNumber()
    {
        return moveTexts.Count;
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightColor;
            }
            else
            {
                actionTexts[i].color=Color.black;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedMove)
            {
                moveTexts[i].color = highlightColor;
            }
            else
            {
                moveTexts[i].color=Color.black;
            }
        }

        ppText.text = $"PP {move.PP}/{move.Base.Pp}";
        typeText.text = move.Base.Type.ToString();
    }

    public void SetMoveNames(List<Move> moveList)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moveList.Count)
            {
                moveTexts[i].text = moveList[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }
}
