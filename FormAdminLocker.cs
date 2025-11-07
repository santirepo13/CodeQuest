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
        private Button btnManageAdmins;
        private Button btnManageQuestions;

        // Responsive layout containers
        private Panel pnlToolsBar;
        private Panel pnlBottomBar;
        private Panel pnlHeader;
        private Panel pnlCenter;     
        
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
            this.Size = new Size(1200, 720);
            this.MinimumSize = new Size(1024, 650);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.Resize += (s, e) => LayoutControls();

            // Panels for header and bottom toolbars (avoid overlap at 1366x768)
            pnlHeader = new Panel();
            pnlHeader.Height = 70;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.BackColor = Color.FromArgb(224, 235, 255);

            pnlCenter = new Panel();
            pnlCenter.Dock = DockStyle.None;
            pnlCenter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlCenter.BackColor = Color.Transparent;
            pnlCenter.AutoScroll = true;
            pnlCenter.Padding = new Padding(0, 8, 0, 8);

            pnlToolsBar = new Panel();
            pnlToolsBar.Height = 55;
            pnlToolsBar.Dock = DockStyle.Bottom;
            pnlToolsBar.BackColor = Color.FromArgb(224, 235, 255);

            pnlBottomBar = new Panel();
            pnlBottomBar.Height = 60;
            pnlBottomBar.Dock = DockStyle.Bottom;
            pnlBottomBar.BackColor = Color.FromArgb(224, 235, 255);

            // Add docked containers in order: header (top), tools (above bottom), bottom (edge), center (fill last)
            // Adding the Fill panel last ensures it gets sized after header and bottom bars.
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlToolsBar);
            this.Controls.Add(pnlBottomBar);
            this.Controls.Add(pnlCenter);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "🔐 PANEL DE ADMINISTRACIÓN";
            lblTitulo.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(600, 40);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            pnlHeader.Controls.Add(lblTitulo);

            // User info label
            lblUserInfo = new Label();
            lblUserInfo.Text = $"Administrador: {currentAdmin.Username}";
            lblUserInfo.Font = new Font("Arial", 12);
            lblUserInfo.ForeColor = Color.FromArgb(70, 70, 70);
            lblUserInfo.Size = new Size(300, 30);
            lblUserInfo.Location = new Point(650, 25);
            lblUserInfo.TextAlign = ContentAlignment.MiddleRight;
            lblUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlHeader.Controls.Add(lblUserInfo);

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
            dgvUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersHeight = 36;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvUsers.Dock = DockStyle.Fill;
            pnlCenter.Controls.Add(dgvUsers);

            // User management buttons
            btnEditUser = new Button();
            btnEditUser.Text = "Editar Nombre";
            btnEditUser.Font = new Font("Arial", 10, FontStyle.Bold);
            btnEditUser.Size = new Size(120, 35);
            btnEditUser.Location = new Point(20, 600);
            btnEditUser.BackColor = Color.FromArgb(255, 165, 0);
            btnEditUser.ForeColor = Color.White;
            btnEditUser.FlatStyle = FlatStyle.Flat;
            btnEditUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditUser.Click += BtnEditUser_Click;

            btnResetXP = new Button();
            btnResetXP.Text = "Resetear XP";
            btnResetXP.Font = new Font("Arial", 10, FontStyle.Bold);
            btnResetXP.Size = new Size(120, 35);
            btnResetXP.Location = new Point(150, 600);
            btnResetXP.BackColor = Color.FromArgb(255, 140, 0);
            btnResetXP.ForeColor = Color.White;
            btnResetXP.FlatStyle = FlatStyle.Flat;
            btnResetXP.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnResetXP.Click += BtnResetXP_Click;

            btnDeleteUser = new Button();
            btnDeleteUser.Text = "Eliminar Usuario";
            btnDeleteUser.Font = new Font("Arial", 10, FontStyle.Bold);
            btnDeleteUser.Size = new Size(120, 35);
            btnDeleteUser.Location = new Point(280, 600);
            btnDeleteUser.BackColor = Color.FromArgb(220, 20, 60);
            btnDeleteUser.ForeColor = Color.White;
            btnDeleteUser.FlatStyle = FlatStyle.Flat;
            btnDeleteUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteUser.Click += BtnDeleteUser_Click;

            btnRefreshUsers = new Button();
            btnRefreshUsers.Text = "Refrescar";
            btnRefreshUsers.Font = new Font("Arial", 10, FontStyle.Bold);
            btnRefreshUsers.Size = new Size(120, 35);
            btnRefreshUsers.Location = new Point(410, 600);
            btnRefreshUsers.BackColor = Color.FromArgb(34, 139, 34);
            btnRefreshUsers.ForeColor = Color.White;
            btnRefreshUsers.FlatStyle = FlatStyle.Flat;
            btnRefreshUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRefreshUsers.Click += (s, e) => LoadUserData();

            // Logout button
            btnLogout = new Button();
            btnLogout.Text = "Cerrar Sesión";
            btnLogout.Font = new Font("Arial", 12, FontStyle.Bold);
            btnLogout.Size = new Size(150, 40);
            btnLogout.Location = new Point(950, 720);
            btnLogout.BackColor = Color.FromArgb(220, 20, 60);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Anchor = AnchorStyles.Bottom;
            btnLogout.Click += BtnLogout_Click;

            // Back to start button
            btnBackToStart = new Button();
            btnBackToStart.Text = "Volver al Inicio";
            btnBackToStart.Font = new Font("Arial", 12, FontStyle.Bold);
            btnBackToStart.Size = new Size(150, 40);
            btnBackToStart.Location = new Point(780, 720);
            btnBackToStart.BackColor = Color.FromArgb(70, 130, 180);
            btnBackToStart.ForeColor = Color.White;
            btnBackToStart.FlatStyle = FlatStyle.Flat;
            btnBackToStart.Anchor = AnchorStyles.Bottom;
            btnBackToStart.Click += BtnBackToStart_Click;

            // Administrators management button
            btnManageAdmins = new Button();
            btnManageAdmins.Text = "Administradores";
            btnManageAdmins.Font = new Font("Arial", 12, FontStyle.Bold);
            btnManageAdmins.Size = new Size(150, 40);
            btnManageAdmins.Location = new Point(610, 720);
            btnManageAdmins.BackColor = Color.FromArgb(70, 130, 180);
            btnManageAdmins.ForeColor = Color.White;
            btnManageAdmins.FlatStyle = FlatStyle.Flat;
            btnManageAdmins.Anchor = AnchorStyles.Bottom;
            btnManageAdmins.Click += BtnManageAdmins_Click;

            // Questions management button
            btnManageQuestions = new Button();
            btnManageQuestions.Text = "Preguntas";
            btnManageQuestions.Font = new Font("Arial", 12, FontStyle.Bold);
            btnManageQuestions.Size = new Size(150, 40);
            btnManageQuestions.Location = new Point(440, 720);
            btnManageQuestions.BackColor = Color.FromArgb(70, 130, 180);
            btnManageQuestions.ForeColor = Color.White;
            btnManageQuestions.FlatStyle = FlatStyle.Flat;
            btnManageQuestions.Anchor = AnchorStyles.Bottom;
            btnManageQuestions.Click += BtnManageQuestions_Click;

            // Place action buttons inside their panels
            pnlToolsBar.Controls.AddRange(new Control[] { btnEditUser, btnResetXP, btnDeleteUser, btnRefreshUsers });
            pnlBottomBar.Controls.AddRange(new Control[] { btnManageQuestions, btnManageAdmins, btnBackToStart, btnLogout });

            // Docking order is set by the order we add panels; no manual Z-order adjustments required.

            this.ResumeLayout(false);
            LayoutControls();
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
                    // Use Fill weights instead of fixed widths so it scales to 1366x768 and resizes nicely
                    dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvUsers.RowHeadersVisible = false;

                    DataGridViewColumn col;

                    col = dgvUsers.Columns["Posición"];
                    if (col != null) { col.HeaderText = "Posición"; col.FillWeight = 8; col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }

                    col = dgvUsers.Columns["Username"];
                    if (col != null) { col.HeaderText = "Jugador"; col.FillWeight = 20; }

                    col = dgvUsers.Columns["Xp"];
                    if (col != null) { col.HeaderText = "XP Total"; col.FillWeight = 12; col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }

                    col = dgvUsers.Columns["Level"];
                    if (col != null) { col.HeaderText = "Nivel"; col.FillWeight = 10; col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }

                    col = dgvUsers.Columns["RondasJugadas"];
                    if (col != null) { col.HeaderText = "Rondas"; col.FillWeight = 10; col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }

                    col = dgvUsers.Columns["ScorePromedio"];
                    if (col != null) { col.HeaderText = "Promedio"; col.DefaultCellStyle.Format = "F1"; col.FillWeight = 12; col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }

                    col = dgvUsers.Columns["UltimaRonda"];
                    if (col != null) { col.HeaderText = "Última Ronda"; col.DefaultCellStyle.Format = "dd/MM/yyyy"; col.FillWeight = 18; }
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

        private void BtnManageAdmins_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new FormAdminManagement();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de administradores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnManageQuestions_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new FormQuestionManagement();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de preguntas: {ex.Message}", "Error",
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

        // Ajuste responsivo para 1366x768 y redimensionamientos (basado en paneles dockeados)
        private void LayoutControls()
        {
            int margin = 20;
            int spacing = 10;
    
            // Header: posicionar título a la izquierda y usuario a la derecha dentro del panel superior
            if (pnlHeader != null)
            {
                if (lblTitulo != null)
                {
                    int yTitle = Math.Max(0, (pnlHeader.ClientSize.Height - lblTitulo.Height) / 2);
                    lblTitulo.Location = new Point(margin, yTitle);
                }
    
                if (lblUserInfo != null)
                {
                    int yUser = Math.Max(0, (pnlHeader.ClientSize.Height - lblUserInfo.Height) / 2);
                    int xUser = Math.Max(margin, pnlHeader.ClientSize.Width - margin - lblUserInfo.Width);
                    lblUserInfo.Location = new Point(xUser, yUser);
                }
            }
    
            // Tools bar: alinear a la izquierda dentro del panel inmediatamente superior al grid
            if (pnlToolsBar != null && btnEditUser != null)
            {
                int y = Math.Max(0, (pnlToolsBar.ClientSize.Height - btnEditUser.Height) / 2);
                int x = margin;
    
                btnEditUser.Location = new Point(x, y); x = btnEditUser.Right + spacing;
                if (btnResetXP != null) { btnResetXP.Location = new Point(x, y); x = btnResetXP.Right + spacing; }
                if (btnDeleteUser != null) { btnDeleteUser.Location = new Point(x, y); x = btnDeleteUser.Right + spacing; }
                if (btnRefreshUsers != null) { btnRefreshUsers.Location = new Point(x, y); }
            }
    
            // Bottom bar: centrar botones dentro del panel inferior
            if (pnlBottomBar != null && btnManageQuestions != null && btnManageAdmins != null && btnBackToStart != null && btnLogout != null)
            {
                int totalWidth = btnManageQuestions.Width + spacing +
                                 btnManageAdmins.Width + spacing +
                                 btnBackToStart.Width + spacing +
                                 btnLogout.Width;
    
                int startX = Math.Max(margin, (pnlBottomBar.ClientSize.Width - totalWidth) / 2);
                int y = Math.Max(0, (pnlBottomBar.ClientSize.Height - btnManageQuestions.Height) / 2);
    
                btnManageQuestions.Location = new Point(startX, y);
                btnManageAdmins.Location = new Point(btnManageQuestions.Right + spacing, y);
                btnBackToStart.Location = new Point(btnManageAdmins.Right + spacing, y);
                btnLogout.Location = new Point(btnBackToStart.Right + spacing, y);
            }
    
            // Position and size center panel explicitly to avoid overlap with header and bottom bars
            if (pnlHeader != null && pnlToolsBar != null && pnlBottomBar != null && pnlCenter != null)
            {
                int top = pnlHeader.Height;
                int bottom = pnlToolsBar.Height + pnlBottomBar.Height;
                pnlCenter.Location = new Point(0, top);
                pnlCenter.Size = new Size(this.ClientSize.Width, Math.Max(0, this.ClientSize.Height - top - bottom));
                pnlCenter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
    
            // Ensure header and toolbars are on top of the z-order
            if (pnlHeader != null) pnlHeader.BringToFront();
            if (pnlToolsBar != null) pnlToolsBar.BringToFront();
            if (pnlBottomBar != null) pnlBottomBar.BringToFront();
    
            // Ensure the DataGridView fills the center panel
            if (dgvUsers != null && pnlCenter != null)
            {
                dgvUsers.Dock = DockStyle.Fill;
            }
        }
    }
}