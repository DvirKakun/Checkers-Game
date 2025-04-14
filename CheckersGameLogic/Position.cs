namespace CheckersGameLogic
{
    public struct Position
    {
        private int m_RowPositionOnBoard;
        private int m_ColumnPositionOnBoard;

        public Position(int i_RowPositionOnBoard, int i_ColumnPositionOnBoard)
        {
            this.m_RowPositionOnBoard = i_RowPositionOnBoard;
            this.m_ColumnPositionOnBoard = i_ColumnPositionOnBoard;
        }

        public int RowPositionOnBoard
        {
            get
            {
                return this.m_RowPositionOnBoard;
            }
            set
            {
                this.m_RowPositionOnBoard = value;
            }
        }

        public int ColumnPositionOnBoard
        {
            get
            {
                return this.m_ColumnPositionOnBoard;
            }
            set
            {
                this.m_ColumnPositionOnBoard = value;
            }
        }
    }
}