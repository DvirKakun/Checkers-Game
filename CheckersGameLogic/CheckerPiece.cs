namespace CheckersGameLogic
{
    public class CheckerPiece
    {
        private readonly Player r_OwnerPlayer;
        private ePieceType m_PieceType;
        private Position m_CheckerPiecePosition;

        public CheckerPiece(Player i_OwnerPlayer, ePieceType i_PieceType, Position i_CheckerPiecePosition)
        {
            this.r_OwnerPlayer = i_OwnerPlayer;
            this.m_PieceType = i_PieceType;
            this.m_CheckerPiecePosition = i_CheckerPiecePosition;
        }

        public Player OwnerPlayer
        {
            get
            {
                return this.r_OwnerPlayer;
            }
        }

        public ePieceType PieceType
        {
            get
            {
                return this.m_PieceType;
            }
            set
            {
                this.m_PieceType = value;
            }
        }

        public Position CheckerPiecePosition
        {
            get
            {
                return this.m_CheckerPiecePosition;
            }
            set
            {
                this.m_CheckerPiecePosition = value;
            }
        }
    }
}