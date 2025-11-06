using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;
using CodeQuest.Models;

namespace CodeQuest
{
    public partial class FormAdminLocker : Form
    {
        private Administrator currentAdmin;
        
        private Label lblTitulo;
        private Label lblUserInfo;
        private DataGridView dgvUsers;
        private Button btnEditUser;
        private Button btnResetXP;
        private Button btnDeleteUser;
        private Button btnRefreshUsers;
        private Button btnLogout;
        private Button btnBackToStart;
        
        private readonly IGameService gameService;
        private readonly IAdministratorService administratorService;

        public FormAdminLocker(Administrator administrator)
        {
            this.currentAdmin = administrator;
            gameService = ServiceFactory.GetGameService();
            administratorService = ServiceFactory.GetAdministratorService();
            InitializeComponent();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Panel de Administración";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "🔐 PANEL DE ADMINISTRACIÓN";
            lblTitulo.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(600, 40);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(lblTitulo);

            // User info label
            lblUserInfo = new Label();
            lblUserInfo.Text = $"Administrador: {currentAdmin.Username}";
            lblUserInfo.Font = new Font("Arial", 12);
            lblUserInfo.ForeColor = Color.FromArgb(70, 70, 70);
            lblUserInfo.Size = new Size(300, 30);
            lblUserInfo.Location = new Point(650, 25);
            lblUserInfo.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblUserInfo);

            // DataGridView for users
            dgvUsers = new DataGridView();
            dgvUsers.Location = new Point(20, 80);
            dgvUsers.Size = new Size(1050, 500);
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.Fixed3D;
            dgvUsers.Font = new Font("Arial", 10);
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvUsers.EnableHeadersVisualStyles = false;
            this.Controls.Add(dgvUsers);

            // User management buttons
            btnEditUser = new Button();
            btnEditUser.Text = "Editar Nombre";
            btnEditUser.Font = new Font("Arial", 10, FontStyle.Bold);
            btnEditUser.Size = new Size(120, 35);
            btnEditUser.Location = new Point(20, 600);
            btnEditUser.BackColor = Color.FromArgb(255, 165, 0);
            btnEditUser.ForeColor = Color.White;
            btnEditUser.FlatStyle = FlatStyle.Flat;
            btnEditUser.Click += BtnEditUser_Click;
            this.Controls.Add(btnEditUser);

            btnResetXP = new Button();
            btnResetXP.Text = "Resetear XP";
            btnResetXP.Font = new Font("Arial", 10, FontStyle.Bold);
            btnResetXP.Size = new Size(120, 35);
            btnResetXP.Location = new Point(150, 600);
            btnResetXP.BackColor = Color.FromArgb(255, 140, 0);
            btnResetXP.ForeColor = Color.White;
            btnResetXP.FlatStyle = FlatStyle.Flat;
            btnResetXP.Click += BtnResetXP_Click;
            this.Controls.Add(btnResetXP);

            btnDeleteUser = new Button();
            btnDeleteUser.Text = "Eliminar Usuario";
            btnDeleteUser.Font = new Font("Arial", 10, FontStyle.Bold);
            btnDeleteUser.Size = new Size(120, 35);
            btnDeleteUser.Location = new Point(280, 600);
            btnDeleteUser.BackColor = Color.FromArgb(220, 20, 60);
            btnDeleteUser.ForeColor = Color.White;
            btnDeleteUser.FlatStyle = FlatStyle.Flat;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            this.Controls.Add(btnDeleteUser);

            btnRefreshUsers = new Button();
            btnRefreshUsers.Text = "Refrescar";
            btnRefreshUsers.Font = new Font("Arial", 10, FontStyle.Bold);
            btnRefreshUsers.Size = new Size(120, 35);
            btnRefreshUsers.Location = new Point(410, 600);
            btnRefreshUsers.BackColor = Color.FromArgb(34, 139, 34);
            btnRefreshUsers.ForeColor = Color.White;
            btnRefreshUsers.FlatStyle = FlatStyle.Flat;
            btnRefreshUsers.Click += (s, e) => LoadUserData();
            this.Controls.Add(btnRefreshUsers);

            // Logout button
            btnLogout = new Button();
            btnLogout.Text = "Cerrar Sesión";
            btnLogout.Font = new Font("Arial", 12, FontStyle.Bold);
            btnLogout.Size = new Size(150, 40);
            btnLogout.Location = new Point(950, 720);
            btnLogout.BackColor = Color.FromArgb(220, 20, 60);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);

            // Back to start button
            btnBackToStart = new Button();
            btnBackToStart.Text = "Volver al Inicio";
            btnBackToStart.Font = new Font("Arial", 12, FontStyle.Bold);
            btnBackToStart.Size = new Size(150, 40);
            btnBackToStart.Location = new Point(780, 720);
            btnBackToStart.BackColor = Color.FromArgb(70, 130, 180);
            btnBackToStart.ForeColor = Color.White;
            btnBackToStart.FlatStyle = FlatStyle.Flat;
            btnBackToStart.Click += BtnBackToStart_Click;
            this.Controls.Add(btnBackToStart);

            this.ResumeLayout(false);
        }

        private void LoadUserData()
        {
            try
            {
                var dataTable = gameService.GetTopRanking(); // Get top ranking for admin
                
                // Add position column
                dataTable.Columns.Add("Posición", typeof(int));
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["Posición"] = i + 1;
                }
                
                // Set column order
                dataTable.Columns["Posición"].SetOrdinal(0);
                
                dgvUsers.DataSource = dataTable;
                
                // Configure columns
                if (dgvUsers.Columns.Count > 0)
                {
                    dgvUsers.Columns["Posición"].Width = 60;
                    dgvUsers.Columns["Username"].HeaderText = "Jugador";
                    dgvUsers.Columns["Username"].Width = 150;
                    dgvUsers.Columns["Xp"].HeaderText = "XP Total";
                    dgvUsers.Columns["Xp"].Width = 80;
                    dgvUsers.Columns["Level"].HeaderText = "Nivel";
                    dgvUsers.Columns["Level"].Width = 60;
                    dgvUsers.Columns["RondasJugadas"].HeaderText = "Rondas";
                    dgvUsers.Columns["RondasJugadas"].Width = 60;
                    dgvUsers.Columns["ScorePromedio"].HeaderText = "Promedio";
                    dgvUsers.Columns["ScorePromedio"].Width = 80;
                    dgvUsers.Columns["ScorePromedio"].DefaultCellStyle.Format = "F1";
                    
                    if (dgvUsers.Columns["UltimaRonda"] != null)
                    {
                        dgvUsers.Columns["UltimaRonda"].HeaderText = "Última Ronda";
                        dgvUsers.Columns["UltimaRonda"].Width = 120;
                        dgvUsers.Columns["UltimaRonda"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor selecciona un usuario.", "Selección Requerida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvUsers.SelectedRows[0];
                string currentUsername = selectedRow.Cells["Username"].Value.ToString();

                string newUsername = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Ingresa el nuevo nombre para '{currentUsername}':", 
                    "Editar Nombre de Usuario", 
                    currentUsername);

                if (!string.IsNullOrWhiteSpace(newUsername) && newUsername != currentUsername)
                {
                    int realUserId = gameService.GetUserId(currentUsername);

                    if (realUserId > 0 && gameService.UpdateUsername(realUserId, newUsername))
                    {
                        MessageBox.Show($"Nombre actualizado exitosamente de '{currentUsername}' a '{newUsername}'", 
                            "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUserData();
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

        private void BtnResetXP_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor selecciona un usuario.", "Selección Requerida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvUsers.SelectedRows[0];
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
                        LoadUserData();
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

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor selecciona un usuario.", "Selección Requerida", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvUsers.SelectedRows[0];
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
                        LoadUserData();
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

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que quieres cerrar sesión?", 
                "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                FormLogin formLogin = new FormLogin();
                formLogin.Show();
                this.Close();
            }
        }

        private void BtnBackToStart_Click(object sender, EventArgs e)
        {
            FormStart formStart = new FormStart();
            formStart.Show();
            this.Close();
        }
    }
}