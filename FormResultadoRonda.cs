using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Models;

namespace CodeQuest
{
    public partial class FormResultadoRonda : Form
    {
        private int userId;
        private string username;
        private int roundNumber;
        private RoundResult result;
        
        private Label lblTitulo;
        private Label lblResultados;
        private Button btnSiguienteRonda;
        private Button btnVerResultadosFinales;

        public FormResultadoRonda(int userId, string username, int roundNumber, RoundResult result)
        {
            this.userId = userId;
            this.username = username;
            this.roundNumber = roundNumber;
            this.result = result;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Resultados de Ronda";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = $"Resultados - Ronda {roundNumber}";
            lblTitulo.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(400, 40);
            lblTitulo.Location = new Point(50, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Results label
            lblResultados = new Label();
            string bonusText = "";
            if (result.TiempoTotalSegundos <= 30)
                bonusText = "¡Excelente velocidad! (+20 XP)";
            else if (result.TiempoTotalSegundos <= 60)
                bonusText = "¡Buena velocidad! (+10 XP)";
            else
                bonusText = "Puedes mejorar tu velocidad (sin bonificación)";

            lblResultados.Text = $"Jugador: {username}\n\n" +
                               $"Respuestas correctas: {result.Correctas} de 3\n" +
                               $"Puntuación: {result.Score} puntos\n" +
                               $"Tiempo total: {result.TiempoTotalSegundos} segundos\n" +
                               $"{bonusText}\n\n" +
                               $"XP ganado: {result.XpEarned} XP";
            
            lblResultados.Font = new Font("Arial", 12);
            lblResultados.ForeColor = Color.FromArgb(70, 70, 70);
            lblResultados.Size = new Size(400, 200);
            lblResultados.Location = new Point(50, 90);
            lblResultados.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(lblResultados);

            if (roundNumber < 3)
            {
                // Next round button
                btnSiguienteRonda = new Button();
                btnSiguienteRonda.Text = $"Siguiente Ronda ({roundNumber + 1})";
                btnSiguienteRonda.Font = new Font("Arial", 12, FontStyle.Bold);
                btnSiguienteRonda.Size = new Size(200, 40);
                btnSiguienteRonda.Location = new Point(150, 310);
                btnSiguienteRonda.BackColor = Color.FromArgb(34, 139, 34);
                btnSiguienteRonda.ForeColor = Color.White;
                btnSiguienteRonda.FlatStyle = FlatStyle.Flat;
                btnSiguienteRonda.Click += BtnSiguienteRonda_Click;
                this.Controls.Add(btnSiguienteRonda);
            }
            else
            {
                // Final results button
                btnVerResultadosFinales = new Button();
                btnVerResultadosFinales.Text = "Ver Resultados Finales";
                btnVerResultadosFinales.Font = new Font("Arial", 12, FontStyle.Bold);
                btnVerResultadosFinales.Size = new Size(220, 40);
                btnVerResultadosFinales.Location = new Point(140, 310);
                btnVerResultadosFinales.BackColor = Color.FromArgb(220, 20, 60);
                btnVerResultadosFinales.ForeColor = Color.White;
                btnVerResultadosFinales.FlatStyle = FlatStyle.Flat;
                btnVerResultadosFinales.Click += BtnVerResultadosFinales_Click;
                this.Controls.Add(btnVerResultadosFinales);
            }

            this.ResumeLayout(false);
        }

        private void BtnSiguienteRonda_Click(object sender, EventArgs e)
        {
            FormRonda formRonda = new FormRonda(userId, username);
            formRonda.SetRound(roundNumber + 1);
            formRonda.Show();
            this.Close();
        }

        private void BtnVerResultadosFinales_Click(object sender, EventArgs e)
        {
            FormResultadosFinales formFinales = new FormResultadosFinales(userId, username);
            formFinales.Show();
            this.Close();
        }
    }
}