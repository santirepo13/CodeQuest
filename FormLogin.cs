using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    public partial class FormLogin : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnBack;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private readonly IAdministratorService administratorService;

        public FormLogin()
        {
            administratorService = ServiceFactory.GetAdministratorService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Administrador Login";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitle = new Label();
            lblTitle.Text = "🔐 Acceso de Administrador";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitle.Size = new Size(400, 40);
            lblTitle.Location = new Point(25, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Username label
            lblUsername = new Label();
            lblUsername.Text = "Usuario:";
            lblUsername.Font = new Font("Arial", 12);
            lblUsername.ForeColor = Color.FromArgb(70, 70, 70);
            lblUsername.Size = new Size(100, 30);
            lblUsername.Location = new Point(50, 100);
            this.Controls.Add(lblUsername);

            // Username textbox
            txtUsername = new TextBox();
            txtUsername.Font = new Font("Arial", 12);
            txtUsername.Size = new Size(250, 30);
            txtUsername.Location = new Point(150, 100);
            txtUsername.MaxLength = 50;
            this.Controls.Add(txtUsername);

            // Password label
            lblPassword = new Label();
            lblPassword.Text = "Contraseña:";
            lblPassword.Font = new Font("Arial", 12);
            lblPassword.ForeColor = Color.FromArgb(70, 70, 70);
            lblPassword.Size = new Size(100, 30);
            lblPassword.Location = new Point(50, 150);
            this.Controls.Add(lblPassword);

            // Password textbox
            txtPassword = new TextBox();
            txtPassword.Font = new Font("Arial", 12);
            txtPassword.Size = new Size(250, 30);
            txtPassword.Location = new Point(150, 150);
            txtPassword.MaxLength = 100;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyPress += TxtPassword_KeyPress;
            this.Controls.Add(txtPassword);

            // Login button
            btnLogin = new Button();
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.Font = new Font("Arial", 12, FontStyle.Bold);
            btnLogin.Size = new Size(200, 40);
            btnLogin.Location = new Point(125, 210);
            btnLogin.BackColor = Color.FromArgb(70, 130, 180);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Back button
            btnBack = new Button();
            btnBack.Text = "Volver";
            btnBack.Font = new Font("Arial", 10);
            btnBack.Size = new Size(100, 30);
            btnBack.Location = new Point(175, 270);
            btnBack.BackColor = Color.FromArgb(128, 128, 128);
            btnBack.ForeColor = Color.White;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);

            this.ResumeLayout(false);
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow Enter key to trigger login
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            
            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Por favor ingresa un nombre de usuario.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor ingresa una contraseña.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                var administrator = administratorService.Authenticate(username, password);
                
                if (administrator != null)
                {
                    MessageBox.Show($"¡Bienvenido, {administrator.Username}!", "Acceso Concedido", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Open admin locker form
                    FormAdminLocker formAdminLocker = new FormAdminLocker(administrator);
                    formAdminLocker.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error de Conexión", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            FormStart formStart = new FormStart();
            formStart.Show();
            this.Close();
        }
    }
}