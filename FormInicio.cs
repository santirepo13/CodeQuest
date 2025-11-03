using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    public partial class FormInicio : Form
    {
        private TextBox txtUsername;
        private Button btnComenzar;
        private Label lblTitulo;
        private Label lblInstrucciones;
        private readonly IGameService gameService;

        public FormInicio()
        {
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Juego de Programación";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "¡Bienvenido a CodeQuest!";
            lblTitulo.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(400, 40);
            lblTitulo.Location = new Point(50, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Instructions label
            lblInstrucciones = new Label();
            lblInstrucciones.Text = "Ingresa tu nombre de usuario para comenzar el juego.\n" +
                                   "Responderás preguntas de programación en C# y ganarás XP por cada respuesta correcta.";
            lblInstrucciones.Font = new Font("Arial", 10);
            lblInstrucciones.ForeColor = Color.FromArgb(70, 70, 70);
            lblInstrucciones.Size = new Size(400, 60);
            lblInstrucciones.Location = new Point(50, 90);
            lblInstrucciones.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInstrucciones);

            // Username textbox
            txtUsername = new TextBox();
            txtUsername.Font = new Font("Arial", 12);
            txtUsername.Size = new Size(300, 30);
            txtUsername.Location = new Point(100, 180);
            txtUsername.MaxLength = 50;
            txtUsername.KeyPress += TxtUsername_KeyPress;
            this.Controls.Add(txtUsername);

            // Start button
            btnComenzar = new Button();
            btnComenzar.Text = "Comenzar Juego";
            btnComenzar.Font = new Font("Arial", 12, FontStyle.Bold);
            btnComenzar.Size = new Size(200, 40);
            btnComenzar.Location = new Point(150, 230);
            btnComenzar.BackColor = Color.FromArgb(70, 130, 180);
            btnComenzar.ForeColor = Color.White;
            btnComenzar.FlatStyle = FlatStyle.Flat;
            btnComenzar.Click += BtnComenzar_Click;
            this.Controls.Add(btnComenzar);

            this.ResumeLayout(false);
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only letters, numbers, and underscore
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '_' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
            
            // Allow Enter key to trigger start button
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnComenzar_Click(sender, e);
            }
        }

        private void BtnComenzar_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            
            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Por favor ingresa un nombre de usuario válido.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("El nombre de usuario debe tener al menos 3 caracteres.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            try
            {
                int userId;
                
                // Check if user exists, if not create new user
                if (gameService.UserExists(username))
                {
                    userId = gameService.GetUserId(username);
                    MessageBox.Show($"¡Bienvenido de vuelta, {username}!", "Usuario Encontrado", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    userId = gameService.CreateUser(username);
                    MessageBox.Show($"¡Usuario creado exitosamente! Bienvenido, {username}!", "Nuevo Usuario", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Open round selection form
                FormRonda formRonda = new FormRonda(userId, username);
                formRonda.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error de Conexión", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}