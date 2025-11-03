using System;
using System.Data;
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
        private Button btnEditarNombre;
        private Button btnEliminarUsuario;
        private Button btnResetearXP;
        private Button btnRefrescar;
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
            this.Size = new Size(900, 600);
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
            dgvRanking.Size = new Size(800, 350);
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

            // Admin buttons
            btnEditarNombre = new Button();
            btnEditarNombre.Text = "Editar Nombre";
            btnEditarNombre.Font = new Font("Arial", 10, FontStyle.Bold);
            btnEditarNombre.Size = new Size(120, 35);
            btnEditarNombre.Location = new Point(50, 450);
            btnEditarNombre.BackColor = Color.FromArgb(255, 165, 0);
            btnEditarNombre.ForeColor = Color.White;
            btnEditarNombre.FlatStyle = FlatStyle.Flat;
            btnEditarNombre.Click += BtnEditarNombre_Click;
            this.Controls.Add(btnEditarNombre);

            btnResetearXP = new Button();
            btnResetearXP.Text = "Resetear XP";
            btnResetearXP.Font = new Font("Arial", 10, FontStyle.Bold);
            btnResetearXP.Size = new Size(120, 35);
            btnResetearXP.Location = new Point(180, 450);
            btnResetearXP.BackColor = Color.FromArgb(255, 140, 0);
            btnResetearXP.ForeColor = Color.White;
            btnResetearXP.FlatStyle = FlatStyle.Flat;
            btnResetearXP.Click += BtnResetearXP_Click;
            this.Controls.Add(btnResetearXP);

            btnEliminarUsuario = new Button();
            btnEliminarUsuario.Text = "Eliminar Usuario";
            btnEliminarUsuario.Font = new Font("Arial", 10, FontStyle.Bold);
            btnEliminarUsuario.Size = new Size(120, 35);
            btnEliminarUsuario.Location = new Point(310, 450);
            btnEliminarUsuario.BackColor = Color.FromArgb(220, 20, 60);
            btnEliminarUsuario.ForeColor = Color.White;
            btnEliminarUsuario.FlatStyle = FlatStyle.Flat;
            btnEliminarUsuario.Click += BtnEliminarUsuario_Click;
            this.Controls.Add(btnEliminarUsuario);

            btnRefrescar = new Button();
            btnRefrescar.Text = "Refrescar";
            btnRefrescar.Font = new Font("Arial", 10, FontStyle.Bold);
            btnRefrescar.Size = new Size(120, 35);
            btnRefrescar.Location = new Point(440, 450);
            btnRefrescar.BackColor = Color.FromArgb(34, 139, 34);
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.Click += BtnRefrescar_Click;
            this.Controls.Add(btnRefrescar);

            // Navigation buttons
            btnJugarDeNuevo = new Button();
            btnJugarDeNuevo.Text = "Jugar de Nuevo";
            btnJugarDeNuevo.Font = new Font("Arial", 12, FontStyle.Bold);
            btnJugarDeNuevo.Size = new Size(150, 40);
            btnJugarDeNuevo.Location = new Point(580, 450);
            btnJugarDeNuevo.BackColor = Color.FromArgb(34, 139, 34);
            btnJugarDeNuevo.ForeColor = Color.White;
            btnJugarDeNuevo.FlatStyle = FlatStyle.Flat;
            btnJugarDeNuevo.Click += BtnJugarDeNuevo_Click;
            this.Controls.Add(btnJugarDeNuevo);

            btnInicio = new Button();
            btnInicio.Text = "Volver al Inicio";
            btnInicio.Font = new Font("Arial", 12, FontStyle.Bold);
            btnInicio.Size = new Size(150, 40);
            btnInicio.Location = new Point(580, 500);
            btnInicio.BackColor = Color.FromArgb(70, 130, 180);
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

        private void BtnEditarNombre_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRanking.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor selecciona un usuario del ranking.", "Selección Requerida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvRanking.SelectedRows[0];
                string currentUsername = selectedRow.Cells["Username"].Value.ToString();
                int selectedUserId = Convert.ToInt32(selectedRow.Cells["Posición"].Value);

                // Obtener el UserID real de la fila seleccionada
                var dataTable = (DataTable)dgvRanking.DataSource;
                var userRow = dataTable.Rows[selectedRow.Index];
                
                string newUsername = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Ingresa el nuevo nombre para '{currentUsername}':", 
                    "Editar Nombre de Usuario", 
                    currentUsername);

                if (!string.IsNullOrWhiteSpace(newUsername) && newUsername != currentUsername)
                {
                    // Buscar el UserID real en los datos
                    int realUserId = 0;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["Username"].ToString() == currentUsername)
                        {
                            // Necesitamos obtener el UserID de alguna manera
                            // Por ahora usaremos una consulta directa
                            realUserId = gameService.GetUserId(currentUsername);
                            break;
                        }
                    }

                    if (realUserId > 0 && gameService.UpdateUsername(realUserId, newUsername))
                    {
                        MessageBox.Show($"Nombre actualizado exitosamente de '{currentUsername}' a '{newUsername}'", 
                            "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRanking(); // Refrescar el ranking
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el nombre. Puede que ya exista ese nombre.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar nombre: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetearXP_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRanking.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor selecciona un usuario del ranking.", "Selección Requerida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvRanking.SelectedRows[0];
                string selectedUsername = selectedRow.Cells["Username"].Value.ToString();

                var result = MessageBox.Show($"¿Estás seguro de que quieres resetear el XP de '{selectedUsername}' a 0?", 
                    "Confirmar Reset de XP", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int realUserId = gameService.GetUserId(selectedUsername);
                    
                    if (realUserId > 0 && gameService.ResetUserXP(realUserId))
                    {
                        MessageBox.Show($"XP de '{selectedUsername}' reseteado exitosamente.", 
                            "Reset Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRanking(); // Refrescar el ranking
                    }
                    else
                    {
                        MessageBox.Show("Error al resetear el XP del usuario.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al resetear XP: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRanking.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor selecciona un usuario del ranking.", "Selección Requerida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvRanking.SelectedRows[0];
                string selectedUsername = selectedRow.Cells["Username"].Value.ToString();

                var result = MessageBox.Show($"¿Estás seguro de que quieres ELIMINAR COMPLETAMENTE a '{selectedUsername}'?\n\n" +
                    "ADVERTENCIA: Esto eliminará al usuario y TODOS sus datos (rondas, respuestas, etc.)\n" +
                    "Esta acción NO se puede deshacer.", 
                    "CONFIRMAR ELIMINACIÓN COMPLETA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int realUserId = gameService.GetUserId(selectedUsername);
                    
                    if (realUserId > 0 && gameService.DeleteUserFromRanking(realUserId))
                    {
                        MessageBox.Show($"Usuario '{selectedUsername}' eliminado completamente.", 
                            "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRanking(); // Refrescar el ranking
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el usuario.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                LoadRanking();
                MessageBox.Show("Ranking actualizado.", "Refrescado", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al refrescar: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}