using UnityEngine;

public interface IGameMan
{
    // Goes to the next level
    void EndLevel();

    // Goes to the menu
    void EndGame();

    void EndTurn();

    // Tells each player object this gameMan knows about to update the activeMenu
    void SetActiveMenu(Menu activeMenu);
}
