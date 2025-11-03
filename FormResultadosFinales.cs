using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    public partial class FormResultadosFinales : Form
    {
        private int userId;
        private string username;
        
        private Label lblTitulo;
        private Label lblEstadisticas;
        private Button btnVerRanking;
        private Button btnJugarDeNuevo;
        private Button btnInicio;
        private readonly IGameService gameService;

        public FormResultadosFinales(int userId, string username)
        {
            this.userId = userId;
            this.username = username;
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
            LoadUserStats();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Resultados Finales";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "¡Juego Completado!";
            lblTitulo.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(500, 40);
            lblTitulo.Location = new Point(50, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Statistics label
            lblEstadisticas = new Label();
            lblEstadisticas.Font = new Font("Arial", 12);
            lblEstadisticas.ForeColor = Color.FromArgb(70, 70, 70);
            lblEstadisticas.Size = new Size(500, 200);
            lblEstadisticas.Location = new Point(50, 90);
            this.Controls.Add(lblEstadisticas);

            // View ranking button
            btnVerRanking = new Button();
            btnVerRanking.Text = "Ver Ranking";
            btnVerRanking.Font = new Font("Arial", 12, FontStyle.Bold);
            btnVerRanking.Size = new Size(150, 40);
            btnVerRanking.Location = new Point(80, 320);
            btnVerRanking.BackColor = Color.FromArgb(70, 130, 180);
            btnVerRanking.ForeColor = Color.White;
            btnVerRanking.FlatStyle = FlatStyle.Flat;
            btnVerRanking.Click += BtnVerRanking_Click;
            this.Controls.Add(btnVerRanking);

            // Play again button
            btnJugarDeNuevo = new Button();
            btnJugarDeNuevo.Text = "Jugar de Nuevo";
            btnJugarDeNuevo.Font = new Font("Arial", 12, FontStyle.Bold);
            btnJugarDeNuevo.Size = new Size(150, 40);
            btnJugarDeNuevo.Location = new Point(250, 320);
            btnJugarDeNuevo.BackColor = Color.FromArgb(34, 139, 34);
            btnJugarDeNuevo.ForeColor = Color.White;
            btnJugarDeNuevo.FlatStyle = FlatStyle.Flat;
            btnJugarDeNuevo.Click += BtnJugarDeNuevo_Click;
            this.Controls.Add(btnJugarDeNuevo);

            // Home button
            btnInicio = new Button();
            btnInicio.Text = "Inicio";
            btnInicio.Font = new Font("Arial", 12, FontStyle.Bold);
            btnInicio.Size = new Size(150, 40);
            btnInicio.Location = new Point(420, 320);
            btnInicio.BackColor = Color.FromArgb(220, 20, 60);
            btnInicio.ForeColor = Color.White;
            btnInicio.FlatStyle = FlatStyle.Flat;
            btnInicio.Click += BtnInicio_Click;
            this.Controls.Add(btnInicio);

            this.ResumeLayout(false);
        }

        private void LoadUserStats()
        {
            try
            {
                var user = gameService.GetUserStats(userId);
                if (user != null)
                {
                    lblEstadisticas.Text = $"Jugador: {user.Username}\n\n" +
                                         $"=== ESTADÍSTICAS TOTALES ===\n" +
                                         $"XP total: {user.Xp} XP\n" +
                                         $"Nivel actual: {user.Level}\n" +
                                         $"XP para siguiente nivel: {((user.Level * 100) - user.Xp)} XP\n\n" +
                                         $"¡Felicidades por completar las 3 rondas!\n" +
                                         $"Continúa jugando para subir de nivel.";
                }
                else
                {
                    lblEstadisticas.Text = "Error al cargar estadísticas del usuario.";
                }
            }
            catch (Exception ex)
            {
                lblEstadisticas.Text = $"Error al cargar estadísticas: {ex.Message}";
            }
        }

        private void BtnVerRanking_Click(object sender, EventArgs e)
        {
            FormRanking formRanking = new FormRanking(userId, username);
            formRanking.Show();
            this.Hide();
        }

        private void BtnJugarDeNuevo_Click(object sender, EventArgs e)
        {
            FormRonda formRonda = new FormRonda(userId, username);
            formRonda.Show();
            this.Close();
        }

        private void BtnInicio_Click(object sender, EventArgs e)
        {
            FormInicio formInicio = new FormInicio();
            formInicio.Show();
            this.Close();
        }
    }
}