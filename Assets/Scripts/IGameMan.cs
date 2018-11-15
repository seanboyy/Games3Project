using UnityEngine;

public interface IGameMan
{
    void SetActiveMenu(Menu newMenu);

    void PlaceUnit(GameObject location, UnitType type);

    void ReturnUnit(GameObject unit, GameObject owner);

    // Goes to the next level
    void EndLevel();

    // Goes to the menu
    void EndGame();

    void EndTurn();

    // Handle left/right inputs
    void HandleHorizontalMovement(GameObject player, float horizontal);
   
    // Handle left/right inputs
    void HandleVerticalMovement(GameObject player, float vertical);

    // Handle the X button being pressed
    void HandleCrossButton(GameObject player);

    // Handle the triangle button being pressed
    void HandleTriangleButton(GameObject player);

    // Handle the circle button being pressed
    void HandleCircleButton(GameObject player);

    // Handle the square button being pressed
    void HandleSquareButton(GameObject player);
}
