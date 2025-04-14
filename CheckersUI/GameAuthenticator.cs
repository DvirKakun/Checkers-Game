using CheckersGameLogic;

namespace CheckersUI
{
    public class GameAuthenticator
    {
        public static bool ValidateNames(string i_FirstPlayerName, string i_SecondPlayerName)
        {
            return GameBoard.IsNameValid(i_FirstPlayerName) && GameBoard.IsNameValid(i_SecondPlayerName);
        }
    }
}