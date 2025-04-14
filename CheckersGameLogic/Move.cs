namespace CheckersGameLogic
{
    public class Move
    {
        private readonly Position r_StartPosition;
        private readonly Position r_EndPosition;
        private bool m_ShouldCapture;

        public Move(Position i_StartPosition, Position i_EndPosition)
        {
            this.r_StartPosition = i_StartPosition;
            this.r_EndPosition = i_EndPosition;
            this.m_ShouldCapture = false;
        }

        public Position StartPosition
        {
            get
            {
                return this.r_StartPosition;
            }
        }

        public Position EndPosition
        {
            get
            {
                return this.r_EndPosition;
            }
        }

        public bool ShouldCapture
        {
            get
            {
                return this.m_ShouldCapture;
            }
            set
            {
                this.m_ShouldCapture = value;
            }
        }
    }
}