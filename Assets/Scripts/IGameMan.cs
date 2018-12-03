using UnityEngine;

public interface IGameMan
{
    // Goes to the next level
    void EndLevel();

    // Goes to the menu
    void EndGame();

    void EndTurn();
}
