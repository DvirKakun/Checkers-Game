using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersUI 
{
    public class FormGameBoard : Form
    {
        private readonly GameBoard r_GameBoard;
        private readonly Button[,] r_GameBoardButtonsMatrix;
        private readonly StringBuilder r_EndGameStringBuilder;
        private readonly Font r_ActivatePlayerFont;
        private readonly Font r_PlayerFont;
        private readonly Color r_EnabledButtonsColor;
        private readonly Color r_DisabledButtonsColor;
        private readonly Color r_AvailableMovesColor;
        private readonly Color r_SelectedButtonColor;
        private Timer m_DelayAndActiveComputerMoveTimer;
        private Label m_FirstPlayerLabel;
        private Label m_SecondPlayerLabel;
        private MovingPictureBox m_MovingPieceImage;
        private Button m_FromButton = null;
        private Button m_ToButton = null;
        private Move m_LastMove = null;
        private bool m_IsAnimationStopped = true;
        private const int k_ButtonSize = 50;
        private const int k_Padding = 16;


        public FormGameBoard(GameBoard i_GameBoard)
        {
            this.r_GameBoard = i_GameBoard;
            this.r_GameBoard.MoveDone += r_GameBoard_MoveDone;
            this.r_GameBoard.PieceCaptured += r_GameBoard_PieceCaptured;
            this.r_GameBoardButtonsMatrix = new Button[(int)this.r_GameBoard.BoardSize, (int)this.r_GameBoard.BoardSize];
            this.r_EndGameStringBuilder = new StringBuilder();
            this.r_ActivatePlayerFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
            this.r_PlayerFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
            this.r_DisabledButtonsColor = Color.FromArgb(82, 65, 45);
            this.r_EnabledButtonsColor = Color.FromArgb(240, 217, 181);
            this.r_AvailableMovesColor = Color.FromArgb(200, 255, 223, 0);
            this.r_SelectedButtonColor = Color.LightBlue;
            initializeComponents();
        }

        private void initializeComponents()
        {
            initializePlayersLabels();
            initializeMovingPieceImage();
            initializeGameBoardButtonsMatrix();
            initializeGameBoardForm();
            initializeDelayComputerMoveTimer();
        }

        private void initializeMovingPieceImage()
        {
            this.m_MovingPieceImage = new MovingPictureBox(k_ButtonSize, r_EnabledButtonsColor);
            this.m_MovingPieceImage.AnimationStopped += this.m_MovingPieceImage_AnimationStopped;
            this.Controls.Add(this.m_MovingPieceImage);
        }

        private void initializeGameBoardButtonsMatrix()
        {
            Button currentButton = null;

            for(int rowIndex = 0; rowIndex < (int)this.r_GameBoard.BoardSize; ++rowIndex)
            {
                for(int columnIndex = 0; columnIndex < (int)this.r_GameBoard.BoardSize; ++columnIndex)
                {
                    CheckerPiece currentCheckerPiece = this.r_GameBoard.Board[rowIndex, columnIndex].CurrentCheckerPiece;

                    currentButton = new Button
                                        {
                                            Width = k_ButtonSize,
                                            Height = k_ButtonSize,
                                            Tag = new Position(rowIndex, columnIndex),
                                            Left = k_Padding + (columnIndex * k_ButtonSize),
                                            Top = (k_Padding * 2) + this.m_FirstPlayerLabel.Bottom + (rowIndex * k_ButtonSize),
                                            FlatStyle = FlatStyle.Popup,
                                            Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                                            BackgroundImageLayout = ImageLayout.Stretch
                                        };

                    if(!isAlternatingCell(rowIndex, columnIndex))
                    {
                        currentButton.Enabled = false;
                        currentButton.BackColor = this.r_DisabledButtonsColor;
                    }
                    else
                    {
                        currentButton.BackColor = this.r_EnabledButtonsColor;
                    }

                    if (currentCheckerPiece != null)
                    {
                        currentButton.BackgroundImage = currentCheckerPiece.OwnerPlayer == this.r_GameBoard.FirstPlayer ? 
                                                            Properties.Resources.black : Properties.Resources.red;
                    }

                    currentButton.Click += this.gameBoardButton_Click;
                    this.r_GameBoardButtonsMatrix[rowIndex, columnIndex] = currentButton;
                    this.Controls.Add(currentButton);
                }
            }
        }

        private void initializeGameBoardForm()
        {
            this.Text = "Damka";
            this.BackColor = this.r_EnabledButtonsColor;
            this.ShowInTaskbar = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = new Size(
                (k_ButtonSize * (int)this.r_GameBoard.BoardSize) + (2 * k_Padding),
                (k_ButtonSize * (int)this.r_GameBoard.BoardSize) + this.m_FirstPlayerLabel.Bottom + 3 * k_Padding);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void initializePlayersLabels()
        {
            this.m_FirstPlayerLabel = new Label
                                          {
                                              Text =
                                                  $"{this.r_GameBoard.FirstPlayer.PlayerName}: {this.r_GameBoard.FirstPlayer.Score}",
                                              Font = r_ActivatePlayerFont,
                                              Left = k_Padding + k_ButtonSize,
                                              Top = k_Padding,
                                              AutoSize = true,
                                          };

            this.m_SecondPlayerLabel = new Label
                                          {
                                              Text =
                                                  $"{this.r_GameBoard.SecondPlayer.PlayerName}: {this.r_GameBoard.SecondPlayer.Score}",
                                              Font = r_PlayerFont,
                                              Left = k_Padding + (k_ButtonSize * ((int)this.r_GameBoard.BoardSize - 3)),
                                              Top = k_Padding,
                                              AutoSize = true,
                                          };

            this.Controls.Add(this.m_FirstPlayerLabel);
            this.Controls.Add(this.m_SecondPlayerLabel);
        }

        private void initializeDelayComputerMoveTimer()
        {
            this.m_DelayAndActiveComputerMoveTimer = new Timer();
            this.m_DelayAndActiveComputerMoveTimer.Interval = 500;
            this.m_DelayAndActiveComputerMoveTimer.Tick += m_DelayAndActiveComputerMoveTimer_Tick;
        }

        private bool isAlternatingCell(int i_RowIndex, int i_ColumnIndex)
        {
            return (i_RowIndex % 2 == 0 && i_ColumnIndex % 2 != 0) || (i_RowIndex % 2 != 0 && i_ColumnIndex % 2 == 0);
        }

        private void gameBoardButton_Click(object sender, EventArgs e)
        {
            if(this.m_IsAnimationStopped)
            {
                Button clickedButton = (sender as Button);
                Position buttonPosition = (Position)clickedButton.Tag;

                if(this.m_FromButton == null)
                {
                    if(isButtonValid(buttonPosition))
                    {
                        this.m_FromButton = clickedButton;
                        clickedButton.BackColor = this.r_SelectedButtonColor;
                        changeAvailableMovesBackColor(this.r_AvailableMovesColor);
                    }
                }
                else
                {
                    changeAvailableMovesBackColor(this.r_EnabledButtonsColor);

                    if (clickedButton == this.m_FromButton)
                    {
                        this.m_FromButton = null;
                        clickedButton.BackColor = this.r_EnabledButtonsColor;
                    }
                    else
                    {
                        this.m_ToButton = clickedButton;
                        makeMove();
                    }
                }
            }
        }

        private bool isButtonValid(Position i_ButtonPosition)
        {
            return this.r_GameBoard.Board[i_ButtonPosition.RowPositionOnBoard, i_ButtonPosition.ColumnPositionOnBoard].CurrentCheckerPiece != null;
        }

        private void changeAvailableMovesBackColor(Color i_BackColor)
        {
            Button markedButton;
            Position fromButtonPosition = (Position)this.m_FromButton.Tag;

            foreach(Move captureMove in this.r_GameBoard.CurrentPlayer.PlayerCaptureMovesList)
            {
                if(captureMove.StartPosition.Equals(fromButtonPosition))
                {
                    markedButton = this.r_GameBoardButtonsMatrix[captureMove.EndPosition.RowPositionOnBoard,
                        captureMove.EndPosition.ColumnPositionOnBoard];
                    markedButton.BackColor = i_BackColor;
                }
            }

            foreach (Move availableMove in this.r_GameBoard.CurrentPlayer.PlayerPossibleMovesList)
            {
                if (availableMove.StartPosition.Equals(fromButtonPosition))
                {
                    markedButton = this.r_GameBoardButtonsMatrix[availableMove.EndPosition.RowPositionOnBoard,
                        availableMove.EndPosition.ColumnPositionOnBoard];
                    markedButton.BackColor = i_BackColor;
                }
            }
        }

        private void makeMove()
        {
            Position fromPosition = (Position)this.m_FromButton.Tag;
            Position toPosition = (Position)this.m_ToButton.Tag;
            bool isMoveSucceed = this.r_GameBoard.TryMove(fromPosition, toPosition);

            if(!isMoveSucceed)
            {
                MessageBox.Show(
                    "Invalid move, please try again",
                    "Invalid Move",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                resetFromAndToButtons();
            }
        }

        private void m_MovingPieceImage_AnimationStopped()
        {
            this.m_IsAnimationStopped = true;
            updateButtonStatusByPosition(m_LastMove.EndPosition);

            if(!this.r_GameBoard.IsGameFinished)
            {
                updateActivatePlayerUI();

                if(this.r_GameBoard.CurrentPlayer.PlayerType == ePlayerType.Computer)
                {
                    this.m_IsAnimationStopped = false;
                    this.m_DelayAndActiveComputerMoveTimer.Start();
                }
            }
            else
            {
                this.endGame();
            }
        }

        private void m_DelayAndActiveComputerMoveTimer_Tick(object sender, EventArgs e)
        {
            this.m_DelayAndActiveComputerMoveTimer.Stop();
            this.r_GameBoard.ActivateComputerMove();
        }

        private void r_GameBoard_MoveDone(Move i_LastMove)
        {
            this.m_FromButton = this.r_GameBoardButtonsMatrix[i_LastMove.StartPosition.RowPositionOnBoard, i_LastMove.StartPosition.ColumnPositionOnBoard];
            this.m_ToButton = this.r_GameBoardButtonsMatrix[i_LastMove.EndPosition.RowPositionOnBoard, i_LastMove.EndPosition.ColumnPositionOnBoard];

            Image currentMovePieceImage = m_FromButton.BackgroundImage;

            this.m_LastMove = i_LastMove;
            updateButtonStatusByPosition(m_LastMove.StartPosition);
            this.m_IsAnimationStopped = false;
            this.m_MovingPieceImage.StartAnimation(m_FromButton, m_ToButton, currentMovePieceImage);
            resetFromAndToButtons();
        }

        private void r_GameBoard_PieceCaptured(Position i_ChangedCellPosition)
        {
            updateButtonStatusByPosition(i_ChangedCellPosition);
        }

        private void updateActivatePlayerUI()
        {
            if(this.r_GameBoard.CurrentPlayer == this.r_GameBoard.FirstPlayer)
            {
                this.m_FirstPlayerLabel.Font = r_ActivatePlayerFont;
                this.m_SecondPlayerLabel.Font = r_PlayerFont;
            }
            else
            {
                this.m_SecondPlayerLabel.Font = r_ActivatePlayerFont;
                this.m_FirstPlayerLabel.Font = r_PlayerFont;
            }
        }

        private void resetFromAndToButtons()
        {
            this.m_FromButton.BackColor = this.r_EnabledButtonsColor;
            this.m_FromButton = null;
            this.m_ToButton = null;
        }

        private void resetBoardUI()
        {
            for(int rowIndex = 0; rowIndex < (int)this.r_GameBoard.BoardSize; ++rowIndex)
            {
                for(int columnIndex = 0; columnIndex < (int)this.r_GameBoard.BoardSize; ++columnIndex)
                {
                    updateButtonStatusByPosition(new Position(rowIndex, columnIndex));
                }   
            }
        }

        private void updateButtonStatusByPosition(Position i_ButtonPosition)
        {
            int rowIndex = i_ButtonPosition.RowPositionOnBoard;
            int columnIndex = i_ButtonPosition.ColumnPositionOnBoard;
            CheckerPiece currentCheckerPiece =
                this.r_GameBoard.Board[rowIndex, columnIndex].CurrentCheckerPiece;
            Button currentButton = this.r_GameBoardButtonsMatrix[rowIndex, columnIndex];

            if (currentCheckerPiece == null)
            {
                currentButton.BackgroundImage = null;
            }
            else
            {
                if (currentCheckerPiece.OwnerPlayer == this.r_GameBoard.FirstPlayer)
                {
                    if (currentCheckerPiece.PieceType == ePieceType.King)
                    {
                        currentButton.BackgroundImage= Properties.Resources.black_king;
                    }
                    else
                    {
                        currentButton.BackgroundImage = Properties.Resources.black;
                    }
                }
                else
                {
                    if (currentCheckerPiece.PieceType == ePieceType.King)
                    {
                        currentButton.BackgroundImage = Properties.Resources.red_king;
                    }
                    else
                    {
                        currentButton.BackgroundImage = Properties.Resources.red;
                    }
                }
            }
        }

        private void resetScoreUI()
        {
            this.m_FirstPlayerLabel.Text =
                $"{this.r_GameBoard.FirstPlayer.PlayerName}: {this.r_GameBoard.FirstPlayer.Score}";
            this.m_SecondPlayerLabel.Text =
                $"{this.r_GameBoard.SecondPlayer.PlayerName}: {this.r_GameBoard.SecondPlayer.Score}";
        }

        private void endGame()
        {
            resetScoreUI();
            createEndGameText();

            DialogResult userChoice = MessageBox.Show(this.r_EndGameStringBuilder.ToString(), "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(userChoice == DialogResult.Yes)
            {
                this.r_GameBoard.Restart();
                resetBoardUI();
                updateActivatePlayerUI();
            }
            else if(userChoice == DialogResult.No)
            {
                this.Close();
            }
        }

        private void createEndGameText()
        {
            this.r_EndGameStringBuilder.Clear();
            this.r_EndGameStringBuilder.AppendLine(
                this.r_GameBoard.WinnerPlayer == null ? "Tie!" : $"{this.r_GameBoard.WinnerPlayer.PlayerName} Won!");
            this.r_EndGameStringBuilder.Append("Another Round?");
        }
    }
}