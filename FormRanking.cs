using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    public partial class FormRanking : Form
    {
        private int userId;
        private string username;
        
        private Label lblTitulo;
        private DataGridView dgvRanking;
        private Button btnJugarDeNuevo;
        private Button btnInicio;
        private readonly IGameService gameService;

        public FormRanking(int userId, string username)
        {
            this.userId = userId;
            this.username = username;
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
            LoadRanking();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Ranking de Jugadores";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "🏆 TOP 10 MEJORES JUGADORES 🏆";
            lblTitulo.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(700, 40);
            lblTitulo.Location = new Point(50, 20);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // DataGridView for ranking
            dgvRanking = new DataGridView();
            dgvRanking.Location = new Point(50, 80);
            dgvRanking.Size = new Size(700, 300);
            dgvRanking.ReadOnly = true;
            dgvRanking.AllowUserToAddRows = false;
            dgvRanking.AllowUserToDeleteRows = false;
            dgvRanking.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRanking.BackgroundColor = Color.White;
            dgvRanking.BorderStyle = BorderStyle.Fixed3D;
            dgvRanking.Font = new Font("Arial", 10);
            dgvRanking.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvRanking.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRanking.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvRanking.EnableHeadersVisualStyles = false;
            this.Controls.Add(dgvRanking);

            // Play again button
            btnJugarDeNuevo = new Button();
            btnJugarDeNuevo.Text = "Jugar de Nuevo";
            btnJugarDeNuevo.Font = new Font("Arial", 12, FontStyle.Bold);
            btnJugarDeNuevo.Size = new Size(180, 40);
            btnJugarDeNuevo.Location = new Point(250, 400);
            btnJugarDeNuevo.BackColor = Color.FromArgb(34, 139, 34);
            btnJugarDeNuevo.ForeColor = Color.White;
            btnJugarDeNuevo.FlatStyle = FlatStyle.Flat;
            btnJugarDeNuevo.Click += BtnJugarDeNuevo_Click;
            this.Controls.Add(btnJugarDeNuevo);

            // Home button
            btnInicio = new Button();
            btnInicio.Text = "Volver al Inicio";
            btnInicio.Font = new Font("Arial", 12, FontStyle.Bold);
            btnInicio.Size = new Size(180, 40);
            btnInicio.Location = new Point(450, 400);
            btnInicio.BackColor = Color.FromArgb(220, 20, 60);
            btnInicio.ForeColor = Color.White;
            btnInicio.FlatStyle = FlatStyle.Flat;
            btnInicio.Click += BtnInicio_Click;
            this.Controls.Add(btnInicio);

            this.ResumeLayout(false);
        }

        private void LoadRanking()
        {
            try
            {
                var dataTable = gameService.GetTopRanking();
                
                // Add position column
                dataTable.Columns.Add("Posición", typeof(int));
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["Posición"] = i + 1;
                }
                
                // Set column order
                dataTable.Columns["Posición"].SetOrdinal(0);
                
                dgvRanking.DataSource = dataTable;
                
                // Configure columns
                if (dgvRanking.Columns.Count > 0)
                {
                    dgvRanking.Columns["Posición"].Width = 80;
                    dgvRanking.Columns["Username"].HeaderText = "Jugador";
                    dgvRanking.Columns["Username"].Width = 150;
                    dgvRanking.Columns["Xp"].HeaderText = "XP Total";
                    dgvRanking.Columns["Xp"].Width = 100;
                    dgvRanking.Columns["Level"].HeaderText = "Nivel";
                    dgvRanking.Columns["Level"].Width = 80;
                    dgvRanking.Columns["RondasJugadas"].HeaderText = "Rondas";
                    dgvRanking.Columns["RondasJugadas"].Width = 80;
                    dgvRanking.Columns["ScorePromedio"].HeaderText = "Promedio";
                    dgvRanking.Columns["ScorePromedio"].Width = 100;
                    dgvRanking.Columns["ScorePromedio"].DefaultCellStyle.Format = "F1";
                    
                    if (dgvRanking.Columns["UltimaRonda"] != null)
                    {
                        dgvRanking.Columns["UltimaRonda"].HeaderText = "Última Ronda";
                        dgvRanking.Columns["UltimaRonda"].Width = 120;
                        dgvRanking.Columns["UltimaRonda"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                }
                
                // Highlight current user
                foreach (DataGridViewRow row in dgvRanking.Rows)
                {
                    if (row.Cells["Username"].Value?.ToString() == username)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                        row.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el ranking: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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