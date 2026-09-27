using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KeycodeAutomata
{
    public partial class MainWindow : Window
    {
        public enum state { Start, DigitOneCorrect, DigitTwoCorrect, DigitThreeCorret, DigitFourCorrect, Correct, IncorrectWaiting, Incorrect };
        public enum alphabet { KeyEntered, Delete, Submit };
        private state currentState = state.Start;

        private string currentInput = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string key = button.Content.ToString();

                switch (key)
                {
                    case "X":
                        UpdateState(alphabet.Delete, key[0]);
                        break;

                    case ">":
                        if (currentInput.Length > 0)
                        {
                            UpdateState(alphabet.Submit, key[0]);
                            await ProcessSubmission();
                        }
                        break;

                    default:
                        if (currentInput.Length < 4)
                        {
                            currentInput += key;
                            UpdateState(alphabet.KeyEntered, key[0]);
                        }
                        break;
                }

                txtDisplay.Text = string.IsNullOrEmpty(currentInput) ? "----" : currentInput;
            }
        }

        // password = 7158
        private void UpdateState(alphabet userAction, char key)
        {
            // Start state
            if (currentState == state.Start)
            {
                if (userAction == alphabet.KeyEntered)
                {
                    if (key == '7')
                    {
                        TransitionToState(state.DigitOneCorrect);
                    }
                    else
                    {
                        TransitionToState(state.IncorrectWaiting);
                    }
                }

                if (userAction == alphabet.Submit)
                {
                    TransitionToState(state.Incorrect);
                    goto TerminalState;
                }

                if (userAction == alphabet.Delete && currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                }

                return;
            }

            // Digit one state
            if (currentState == state.DigitOneCorrect)
            {
                if (userAction == alphabet.Delete)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    TransitionToState(state.Start);
                }

                if (userAction == alphabet.KeyEntered)
                {
                    if (key == '1')
                    {
                        TransitionToState(state.DigitTwoCorrect);
                    }
                    else
                    {
                        TransitionToState(state.IncorrectWaiting);
                    }
                }

                if (userAction == alphabet.Submit)
                {
                    TransitionToState(state.Incorrect);
                    goto TerminalState;
                }

                return;
            }

            // Digit Two State
            if (currentState == state.DigitTwoCorrect)
            {
                if (userAction == alphabet.Delete)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    TransitionToState(state.DigitOneCorrect);
                }

                if (userAction == alphabet.KeyEntered)
                {
                    if (key == '5')
                    {
                        TransitionToState(state.DigitThreeCorret);
                    }
                    else
                    {
                        TransitionToState(state.IncorrectWaiting);
                    }
                }

                if (userAction == alphabet.Submit)
                {
                    TransitionToState(state.Incorrect);
                    goto TerminalState;
                }

                return;
            }

            // Digit Three State
            if (currentState == state.DigitThreeCorret)
            {
                if (userAction == alphabet.Delete)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    TransitionToState(state.DigitTwoCorrect);
                }

                if (userAction == alphabet.KeyEntered)
                {
                    if (key == '8')
                    {
                        TransitionToState(state.DigitFourCorrect);
                    }
                    else
                    {
                        TransitionToState(state.IncorrectWaiting);
                    }
                }

                if (userAction == alphabet.Submit)
                {
                    TransitionToState(state.Incorrect);
                    goto TerminalState;
                }

                return;
            }

            // Digit Four State
            if (currentState == state.DigitFourCorrect)
            {
                if (userAction == alphabet.Delete)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    TransitionToState(state.DigitThreeCorret);
                }

                if (userAction == alphabet.Submit)
                {
                    TransitionToState(state.Correct);
                    goto TerminalState;
                }

                return;
            }

            // Incorrect Waiting State
            if (currentState == state.IncorrectWaiting)
            {
                if (userAction == alphabet.Delete)
                {
                    if (currentInput.Length > 0)
                    {
                        currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    }

                    if (currentInput == "715")
                    {
                        TransitionToState(state.DigitThreeCorret);
                    }
                    else if (currentInput == "71")
                    {
                        TransitionToState(state.DigitTwoCorrect);
                    }
                    else if (currentInput == "7")
                    {
                        TransitionToState(state.DigitOneCorrect);
                    }
                    else if (currentInput == "")
                    {
                        TransitionToState(state.Start);
                    }
                }

                if (userAction == alphabet.Submit)
                {
                    TransitionToState(state.Incorrect);
                    goto TerminalState;
                }

                return;
            }

        TerminalState:

            if (currentState == state.Incorrect || currentState == state.Correct)
            {
                // this represents a terminal state that we wait at until we transition to start
            }
        }

        private void TransitionToState(state stateToTransitionTo)
        {
            currentState = stateToTransitionTo;

            switch (stateToTransitionTo)
            {
                case state.Start:
                    Response_Code.Text = "Start";
                    break;

                case state.DigitOneCorrect:
                    Response_Code.Text = "Digit One Correct";
                    break;

                case state.DigitTwoCorrect:
                    Response_Code.Text = "Digit Two Correct";
                    break;

                case state.DigitThreeCorret:
                    Response_Code.Text = "Digit Three Correct";
                    break;

                case state.DigitFourCorrect:
                    Response_Code.Text = "Digit Four Correct";
                    break;

                case state.Correct:
                    Response_Code.Text = "Correct";
                    break;

                case state.IncorrectWaiting:
                    Response_Code.Text = "Incorrect Waiting";
                    break;

                case state.Incorrect:
                    Response_Code.Text = "Incorrect";
                    break;
            }
        }

        private async Task ProcessSubmission()
        {
            lblStatus.Content = "Authenticating...";

            await Task.Delay(500);

            if (currentState == state.Correct)
            {
                MessageBox.Show($"Access Granted for Keycode: {currentInput}", "Security System", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Access Denied for Keycode: {currentInput}", "Security System", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lblStatus.Content = "Type code and press > to submit.";
            currentInput = "";
            txtDisplay.Text = "----";
            TransitionToState(state.Start);
        }
    }
}