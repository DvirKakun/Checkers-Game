using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameLogic;

namespace CheckersUI
{
    public class FormGameSettings : Form
    {
        private Label m_BoardSizeLabel;
        private List<RadioButton> m_BoardSizeRadioButtons;
        private Label m_PlayersLabel;
        private Label m_FirstPlayerLabel;
        private Label m_SecondPlayerLabel;
        private CheckBox m_SecondPlayerCheckBox;
        private TextBox m_FirstPlayerTextBox;
        private TextBox m_SecondPlayerTextBox;
        private Button m_DoneButton;
        private eBoardSize m_BoardSize = eBoardSize.Small;
        private eGameMode m_GameMode = eGameMode.AgainstComputer;
        private bool m_IsClosedByDone = false;

        public FormGameSettings()
        {
            initializeComponents();
        }

        private void initializeComponents()
        {
            initializeBoardSizeLabel();
            initializeBoardSizeRadioButtons();
            initializePlayersLabel();
            initializeFirstPlayerLabel();
            initializeSecondPlayerCheckBox();
            initializeSecondPlayerLabel();
            initializeFirstPlayerTextBox();
            initializeSecondPlayerTextBox();
            initializeDoneButton();
            initializeForm();
        }

        private void initializeForm()
        {
            this.Text = "Game Settings";
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ClientSize = new Size(this.m_DoneButton.Right + 16, this.m_DoneButton.Bottom + 16);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void initializeBoardSizeLabel()
        {
            this.m_BoardSizeLabel = new Label
                                        {
                                            Text = "Board Size:", 
                                            Font = new Font(FontFamily.GenericSansSerif, 12), 
                                            Left = 16, 
                                            Top = 16, 
                                            AutoSize = true
                                        };

            this.Controls.Add(this.m_BoardSizeLabel);
        }

        private void initializeBoardSizeRadioButtons()
        {
            int topPosition = this.m_BoardSizeLabel.Bottom + 5;
            int leftPosition = this.m_BoardSizeLabel.Left + 16;
            RadioButton currentSizeRadioButton = null;

            this.m_BoardSizeRadioButtons = new List<RadioButton>();

            foreach (int possibleSize in Enum.GetValues(typeof(eBoardSize)))
            {
                currentSizeRadioButton = new RadioButton
                                                         {
                                                             Text = $"{possibleSize} x {possibleSize}", 
                                                             Left = leftPosition, 
                                                             Top = topPosition, 
                                                             AutoSize = true, 
                                                             Tag = possibleSize, 
                                                             FlatStyle = FlatStyle.Popup, 
                                                             Font = new Font(FontFamily.GenericSansSerif, 12),
                                                         };

                currentSizeRadioButton.Click += this.sizeRadioButton_Click;
                this.m_BoardSizeRadioButtons.Add(currentSizeRadioButton);
                this.Controls.Add(currentSizeRadioButton);
                leftPosition = currentSizeRadioButton.Right + 5;
            }

            this.m_BoardSizeRadioButtons[0].Checked = true;
        }

        private void initializePlayersLabel()
        {
            this.m_PlayersLabel = new Label
                                      {
                                          Text = "Players:", 
                                          Font = new Font(FontFamily.GenericSansSerif, 12), 
                                          Left = this.m_BoardSizeLabel.Left, 
                                          Top = this.m_BoardSizeRadioButtons[0].Bottom + 5, 
                                          AutoSize = true
                                      };

            this.Controls.Add(this.m_PlayersLabel);
        }

        private void initializeFirstPlayerLabel()
        {
            this.m_FirstPlayerLabel = new Label
                                          {
                                              Text = "Player 1:", 
                                              Font = new Font(FontFamily.GenericSansSerif, 12),
                                              Left = this.m_PlayersLabel.Left + 16, 
                                              Top = this.m_PlayersLabel.Bottom + 10, 
                                              AutoSize = true
                                          };

            this.Controls.Add(this.m_FirstPlayerLabel);
        }

        private void initializeSecondPlayerCheckBox()
        {
            this.m_SecondPlayerCheckBox = new CheckBox()
                                              {
                                                  Left = this.m_FirstPlayerLabel.Left,
                                                  Top = this.m_FirstPlayerLabel.Bottom + 16,
                                                  AutoSize = true
                                              };

            this.m_SecondPlayerCheckBox.CheckedChanged += this.m_SecondPlayerCheckBox_CheckChanged;
            this.Controls.Add(this.m_SecondPlayerCheckBox);
        }

        private void initializeSecondPlayerLabel()
        {
            this.m_SecondPlayerLabel = new Label
                                          {
                                              Text = "Player 2:",
                                              Font = new Font(FontFamily.GenericSansSerif, 12),
                                              Left = this.m_SecondPlayerCheckBox.Right + 2,
                                              AutoSize = true
                                          };

            this.m_SecondPlayerLabel.Top = this.m_SecondPlayerCheckBox.Top
                                                 + this.m_SecondPlayerCheckBox.Height / 2
                                                 - this.m_SecondPlayerLabel.Height / 2;
            this.Controls.Add(this.m_SecondPlayerLabel);
        }

        private void initializeFirstPlayerTextBox()
        {
            this.m_FirstPlayerTextBox = new TextBox
                                            {
                                                Left = this.ClientSize.Width / 2,
                                                Font = new Font(FontFamily.GenericSansSerif, 12),
                                            };

            this.m_FirstPlayerTextBox.Top = this.m_FirstPlayerLabel.Top
                                            + this.m_FirstPlayerLabel.Height / 2
                                            - this.m_FirstPlayerTextBox.Height / 2;
            this.Controls.Add(this.m_FirstPlayerTextBox);
        }

        private void initializeSecondPlayerTextBox()
        {
            this.m_SecondPlayerTextBox = new TextBox
                                            {
                                                Left = this.ClientSize.Width / 2,
                                                Enabled = false,
                                                Text = "[Computer]",
                                                Font = new Font(FontFamily.GenericSansSerif, 12)
                                            };

            this.m_SecondPlayerTextBox.Top = this.m_SecondPlayerLabel.Top
                                             + this.m_SecondPlayerLabel.Height / 2
                                             - this.m_SecondPlayerTextBox.Height / 2;
            this.Controls.Add(this.m_SecondPlayerTextBox);
        }

        private void initializeDoneButton()
        {
            this.m_DoneButton = new Button
                                    {
                                        Text = "Done", 
                                        TextAlign = ContentAlignment.MiddleCenter, 
                                        Top = this.m_SecondPlayerTextBox.Bottom + 16, 
                                        Font = new Font(FontFamily.GenericSansSerif, 12),
                                        FlatStyle = FlatStyle.Flat,
                                        FlatAppearance = { BorderSize = 1}
                                    };

            this.m_DoneButton.Left = this.m_SecondPlayerTextBox.Left
                                     + (this.m_SecondPlayerTextBox.Width - this.m_DoneButton.Width);
            this.m_DoneButton.Click += m_DoneButton_Click;
            this.Controls.Add(this.m_DoneButton);


        }

        private void sizeRadioButton_Click(object sender, EventArgs e)
        {
            this.m_BoardSize = (eBoardSize)Enum.Parse(typeof(eBoardSize), (sender as RadioButton).Tag.ToString());
        }

        private void m_SecondPlayerCheckBox_CheckChanged(object sender, EventArgs e)
        {
            this.m_SecondPlayerTextBox.Enabled = this.m_SecondPlayerCheckBox.Checked;
            this.m_GameMode = this.m_SecondPlayerCheckBox.Checked ? eGameMode.AgainstHuman : eGameMode.AgainstComputer;
        }

        private void m_DoneButton_Click(object sender, EventArgs e)
        {
            this.m_IsClosedByDone = true;
            this.Close();
        }

        public bool IsClosedByDone
        {
            get
            {
                return this.m_IsClosedByDone;
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return this.m_FirstPlayerTextBox.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return this.m_GameMode == eGameMode.AgainstComputer ? "Computer" : this.m_SecondPlayerTextBox.Text;
            }
        }

        public eBoardSize BoardSize
        {
            get
            {
                return this.m_BoardSize;
            }
        }

        public eGameMode GameMode
        {
            get
            {
                return this.m_GameMode;
            }
        }
    }
}