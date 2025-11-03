using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    public partial class FormRonda : Form
    {
        private int userId;
        private string username;
        private int currentRound = 1;
        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblRondaInfo;
        private Label lblInstrucciones;
        private Button btnIniciarRonda;
        private readonly IGameService gameService;

        public FormRonda(int userId, string username)
        {
            this.userId = userId;
            this.username = username;
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Preparación de Ronda";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "CodeQuest - Ronda de Preguntas";
            lblTitulo.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(500, 40);
            lblTitulo.Location = new Point(50, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // User label
            lblUsuario = new Label();
            lblUsuario.Text = $"Jugador: {username}";
            lblUsuario.Font = new Font("Arial", 12, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(70, 70, 70);
            lblUsuario.Size = new Size(300, 25);
            lblUsuario.Location = new Point(150, 80);
            lblUsuario.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblUsuario);

            // Round info label
            lblRondaInfo = new Label();
            lblRondaInfo.Text = $"Ronda {currentRound} - Dificultad: {currentRound}";
            lblRondaInfo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblRondaInfo.ForeColor = Color.FromArgb(220, 20, 60);
            lblRondaInfo.Size = new Size(400, 30);
            lblRondaInfo.Location = new Point(100, 120);
            lblRondaInfo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblRondaInfo);

            // Instructions label
            lblInstrucciones = new Label();
            lblInstrucciones.Text = "• Responderás 3 preguntas de programación en C#\n" +
                                   "• Ganarás 10 XP por cada respuesta correcta\n" +
                                   "• Bonificación por tiempo:\n" +
                                   "  - ≤30 segundos: +20 XP\n" +
                                   "  - ≤60 segundos: +10 XP\n" +
                                   "  - >60 segundos: +0 XP\n\n" +
                                   "¡Responde rápido y correctamente para maximizar tu XP!";
            lblInstrucciones.Font = new Font("Arial", 10);
            lblInstrucciones.ForeColor = Color.FromArgb(70, 70, 70);
            lblInstrucciones.Size = new Size(450, 150);
            lblInstrucciones.Location = new Point(75, 170);
            this.Controls.Add(lblInstrucciones);

            // Start round button
            btnIniciarRonda = new Button();
            btnIniciarRonda.Text = "Iniciar Ronda";
            btnIniciarRonda.Font = new Font("Arial", 12, FontStyle.Bold);
            btnIniciarRonda.Size = new Size(200, 40);
            btnIniciarRonda.Location = new Point(200, 330);
            btnIniciarRonda.BackColor = Color.FromArgb(34, 139, 34);
            btnIniciarRonda.ForeColor = Color.White;
            btnIniciarRonda.FlatStyle = FlatStyle.Flat;
            btnIniciarRonda.Click += BtnIniciarRonda_Click;
            this.Controls.Add(btnIniciarRonda);

            this.ResumeLayout(false);
        }

        private void BtnIniciarRonda_Click(object sender, EventArgs e)
        {
            try
            {
                // Create new round in database
                int roundId = gameService.StartNewRound(userId);
                
                // Get questions for this round
                var questions = gameService.GetQuestionsForRound(currentRound);
                
                if (questions.Count == 3)
                {
                    // Open questions form
                    FormPreguntas formPreguntas = new FormPreguntas(userId, username, roundId, currentRound, questions);
                    formPreguntas.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error: No se pudieron cargar las preguntas para esta ronda.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la ronda: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetRound(int roundNumber)
        {
            currentRound = roundNumber;
            lblRondaInfo.Text = $"Ronda {currentRound} - Dificultad: {currentRound}";
        }
    }
}