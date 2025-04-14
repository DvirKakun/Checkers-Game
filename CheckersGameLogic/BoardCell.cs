using System;

namespace CheckersGameLogic
{
    public class BoardCell
    {
        private readonly Position r_CurrentCellPostion;
        private CheckerPiece m_CellCheckerPiece;

        public BoardCell(Position i_CurrentCellPosition, CheckerPiece i_CheckerPiece)
        {
            this.r_CurrentCellPostion = i_CurrentCellPosition;
            this.m_CellCheckerPiece = i_CheckerPiece;
        }

        public CheckerPiece CurrentCheckerPiece
        {
            get
            {
                return this.m_CellCheckerPiece;
            }
            set
            {
                if (value == null)
                {
                    this.m_CellCheckerPiece = null;
                }
                else
                {
                    this.m_CellCheckerPiece = value;
                    this.m_CellCheckerPiece.CheckerPiecePosition = this.r_CurrentCellPostion;
                }
            }
        }

        public bool IsEmpty()
        {
            return this.m_CellCheckerPiece == null;
        }

        public void Clear()
        {
            this.m_CellCheckerPiece = null;
        }
    }
}