using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    public partial class Form1 : Form
    {
        SoundPlayer sounds = new SoundPlayer();
        Button[] buttons = new Button[26];
        public string[] words = { "Method", "Variable", "Class", "Array", "Loop", "Boolean", "Exception", "Interface", "Property", "Delegate" };
        private string[] descriptions = { "A function or subroutine that performs a specific task.",
            "A named storage location for data that can change during program execution.",
            "A blueprint for creating objects, defining their properties and behaviors.", 
            "A collection of elements of the same data type, accessed by an index.",
            "A control structure that repeats a block of code until a condition is met.",
            "A data type representing true or false values.",
            "An error condition that disrupts normal program flow.",
            "A contract that defines a set of methods that a class must implement.",
            "A special method used to get or set the value of an object's field.", 
            "A type that represents a reference to a method with a specific signature."  };
        //public string[] wordDescription = {""}
        string wordToGuess;
        char[] incorrectLetterGuessed;
        char[] correctLetters;
        int tries, correctTries = 2;
        public Form1()
        {
            InitializeComponent();
            randomWord();
            generateButtons();
        }

        public void randomWord()
        {
            label1.Text = "";
            Random rand = new Random();
            int i = rand.Next(words.Length);
            label2.Text = descriptions[i];
            wordToGuess = words[i].ToUpper();
            incorrectLetterGuessed = new char[3];
            correctLetters = new char[wordToGuess.Length];

            for (int j = 0; j < wordToGuess.Length; j++)
            {
                label1.Text += "_ ";
            }
        }
        private void generateButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button();
                buttons[i].Text = ((char)('A' + i)).ToString();
                buttons[i].Click += button_click;
                buttons[i].KeyPress += button_pressed;
                flowLayoutPanel1.Controls.Add(buttons[i]);
            }
        }

        private void button_pressed(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                char buttonText = char.ToUpper(e.KeyChar);
                checkInput(buttonText, (Button)sender);
            }
        }

        private void button_click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            char buttonText = Convert.ToChar(clickedButton.Text);
            checkInput(buttonText, clickedButton);
        }

        private async void playSounds(string title, int delay)
        {
            await Task.Delay(delay);
            sounds.SoundLocation = title;
            sounds.PlayLooping();
        }

        private void playGuess(string title)
        {
            sounds.SoundLocation = title;
            sounds.Play();
        }

        private void checkInput(char letter)
        {
            string displayedWord = label1.Text;

            if (checkIfInputAlreadyExist(letter))
                return;

            if (wordToGuess.Contains(letter))
            {
                correctGuess(letter, displayedWord);
            }
            else
            {
                incorrectGuess(letter);

            }

        }
        private void checkInput(char letter, Button clickedButton)
        {
            string displayedWord = label1.Text;

            clickedButton.Enabled = false;
            if (checkIfInputAlreadyExist(letter))
                return;

            if (wordToGuess.Contains(letter))
            {
                correctGuess(letter, displayedWord);
            }
            else
            {
                incorrectGuess(letter);

            }
        }

        private bool checkIfInputAlreadyExist(char letter)
        {
            if (incorrectLetterGuessed.Contains(letter) || correctLetters.Contains(letter))
            {
                MessageBox.Show("You already entered that letter.");
                return true;
            }
            else
            {
                return false;
            }
        }


        private void correctGuess(char letter, string displayedWord)
        {
            playGuess("correct.wav");
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if (wordToGuess[i] == letter)
                {
                    displayedWord = displayedWord.Remove(i * 2, 1);
                    displayedWord = displayedWord.Insert(i * 2, letter.ToString());
                    correctLetters[i] = letter;
                }
            }
            
            label1.Text = displayedWord.ToString();
            if (displayedWord.Replace(" ", "") == wordToGuess)
            {
                playSounds("win.wav", 0);
                MessageBox.Show("Congratulations! You've guessed the word correctly.", "You Won!");
                Application.Exit();
            }
            playSounds("boss.wav", 1000);
        }

        private void displayMan(int num)
        {
            Bitmap image = new Bitmap(num + "_Man.png");
            pictureBox1.Image = (Image)image;
        }

        private void incorrectGuess(char letter)
        {
            playGuess("wrong.wav");
            incorrectLetterGuessed[tries] = letter;
            tries++;
            displayMan(tries);
            if (tries == 3)
            {
                playSounds("lose.wav", 0);
                MessageBox.Show("You guessed 3 times wrong.");
                MessageBox.Show("You Lose", "You Lost!");
                Application.Exit();
            }
            else
            {
                playSounds("boss.wav", 1000);
                MessageBox.Show("Incorrect Letter", "Wrong!");
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            playSounds("boss.wav", 0);
        }
    }
}
