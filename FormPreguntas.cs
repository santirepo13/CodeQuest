using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;
using CodeQuest.Models;

namespace CodeQuest
{
    public partial class FormPreguntas : Form
    {
        private int userId;
        private string username;
        private int roundId;
        private int roundNumber;
        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private DateTime questionStartTime;
        private readonly IGameService gameService;
        
        private Label lblTitulo;
        private Label lblProgreso;
        private Label lblPregunta;
        private RadioButton[] radioButtons;
        private Button btnSiguiente;
        private Label lblTiempo;
        private System.Windows.Forms.Timer timer;
        private int totalSeconds = 0;

        public FormPreguntas(int userId, string username, int roundId, int roundNumber, List<Question> questions)
        {
            this.userId = userId;
            this.username = username;
            this.roundId = roundId;
            this.roundNumber = roundNumber;
            this.questions = questions;
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
            LoadQuestion();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Preguntas";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = $"Ronda {roundNumber} - {username}";
            lblTitulo.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(600, 30);
            lblTitulo.Location = new Point(50, 20);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Progress label
            lblProgreso = new Label();
            lblProgreso.Font = new Font("Arial", 10, FontStyle.Bold);
            lblProgreso.ForeColor = Color.FromArgb(70, 70, 70);
            lblProgreso.Size = new Size(200, 20);
            lblProgreso.Location = new Point(50, 60);
            this.Controls.Add(lblProgreso);

            // Time label
            lblTiempo = new Label();
            lblTiempo.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTiempo.ForeColor = Color.FromArgb(220, 20, 60);
            lblTiempo.Size = new Size(200, 20);
            lblTiempo.Location = new Point(450, 60);
            lblTiempo.TextAlign = ContentAlignment.TopRight;
            this.Controls.Add(lblTiempo);

            // Question label
            lblPregunta = new Label();
            lblPregunta.Font = new Font("Arial", 12, FontStyle.Bold);
            lblPregunta.ForeColor = Color.FromArgb(25, 25, 112);
            lblPregunta.Size = new Size(600, 80);
            lblPregunta.Location = new Point(50, 100);
            this.Controls.Add(lblPregunta);

            // Radio buttons for choices
            radioButtons = new RadioButton[4];
            for (int i = 0; i < 4; i++)
            {
                radioButtons[i] = new RadioButton();
                radioButtons[i].Font = new Font("Arial", 11);
                radioButtons[i].ForeColor = Color.FromArgb(70, 70, 70);
                radioButtons[i].Size = new Size(550, 30);
                radioButtons[i].Location = new Point(70, 200 + (i * 40));
                radioButtons[i].UseVisualStyleBackColor = true;
                this.Controls.Add(radioButtons[i]);
            }

            // Next button
            btnSiguiente = new Button();
            btnSiguiente.Text = "Siguiente";
            btnSiguiente.Font = new Font("Arial", 12, FontStyle.Bold);
            btnSiguiente.Size = new Size(150, 40);
            btnSiguiente.Location = new Point(275, 400);
            btnSiguiente.BackColor = Color.FromArgb(70, 130, 180);
            btnSiguiente.ForeColor = Color.White;
            btnSiguiente.FlatStyle = FlatStyle.Flat;
            btnSiguiente.Click += BtnSiguiente_Click;
            this.Controls.Add(btnSiguiente);

            // Timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;
            timer.Start();

            this.ResumeLayout(false);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            totalSeconds++;
            lblTiempo.Text = $"Tiempo: {totalSeconds}s";
        }

        private void LoadQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                
                lblProgreso.Text = $"Pregunta {currentQuestionIndex + 1} de {questions.Count}";
                lblPregunta.Text = question.Text;
                
                // Load choices
                for (int i = 0; i < radioButtons.Length && i < question.Choices.Count; i++)
                {
                    radioButtons[i].Text = question.Choices[i].ChoiceText;
                    radioButtons[i].Tag = question.Choices[i];
                    radioButtons[i].Checked = false;
                    radioButtons[i].Visible = true;
                }
                
                // Hide unused radio buttons
                for (int i = question.Choices.Count; i < radioButtons.Length; i++)
                {
                    radioButtons[i].Visible = false;
                }
                
                questionStartTime = DateTime.Now;
                
                // Update button text
                if (currentQuestionIndex == questions.Count - 1)
                {
                    btnSiguiente.Text = "Finalizar Ronda";
                }
            }
        }

        private void BtnSiguiente_Click(object sender, EventArgs e)
        {
            // Validate selection
            Choice selectedChoice = null;
            foreach (var rb in radioButtons)
            {
                if (rb.Checked && rb.Tag is Choice choice)
                {
                    selectedChoice = choice;
                    break;
                }
            }

            if (selectedChoice == null)
            {
                MessageBox.Show("Por favor selecciona una respuesta.", "Selección Requerida", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate time spent on this question
            int timeSpent = (int)(DateTime.Now - questionStartTime).TotalSeconds;
            
            try
            {
                // Submit answer
                var currentQuestion = questions[currentQuestionIndex];
                gameService.SubmitAnswer(roundId, currentQuestion.QuestionID, selectedChoice.ChoiceID, timeSpent);
                
                currentQuestionIndex++;
                
                if (currentQuestionIndex < questions.Count)
                {
                    // Load next question
                    LoadQuestion();
                }
                else
                {
                    // Round completed, show results
                    timer.Stop();
                    ShowRoundResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar respuesta: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowRoundResults()
        {
            try
            {
                var result = gameService.CompleteRound(roundId);
                
                if (result != null)
                {
                    FormResultadoRonda formResultado = new FormResultadoRonda(userId, username, roundNumber, result);
                    formResultado.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error al procesar los resultados de la ronda.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al finalizar la ronda: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timer?.Stop();
            timer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}