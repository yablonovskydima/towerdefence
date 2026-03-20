using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void OnStartClicked()
    {
        GameManager.Instance.ChangeState(GameState.Preparation);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}