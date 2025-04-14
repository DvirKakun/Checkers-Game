using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersUI
{
    public class WinFormUI
    {
        public static void StartGame()
        {
            FormGameSettings formGameSettings = new FormGameSettings();
           
            formGameSettings.ShowDialog();

            if(formGameSettings.IsClosedByDone)
            {
                if(GameAuthenticator.ValidateNames(formGameSettings.FirstPlayerName, formGameSettings.SecondPlayerName))
                {
                    GameBoard gameBoard = new GameBoard(
                        formGameSettings.BoardSize,
                        formGameSettings.FirstPlayerName,
                        formGameSettings.SecondPlayerName,
                        formGameSettings.GameMode);
                    FormGameBoard formGameBoard = new FormGameBoard(gameBoard);

                    formGameBoard.ShowDialog();
                }
                else
                {
                    DialogResult userChoice = MessageBox.Show(
                        "Invalid game settings",
                        "Error",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);

                    if(userChoice == DialogResult.Retry)
                    {
                        StartGame();
                    }
                }
            }
        }
    }
}