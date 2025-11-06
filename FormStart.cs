using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    public partial class FormStart : Form
    {
        private TextBox txtUsername;
        private Button btnComenzar;
        private Label lblTitulo;
        private Label lblInstrucciones;
        private PictureBox pbSafeBox;
        private IGameService gameService;

        public FormStart()
        {
            // Don't initialize game service here - delay until needed
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Juego de Programación";
            this.Size = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "¡Bienvenido a CodeQuest!";
            lblTitulo.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(600, 50);
            lblTitulo.Location = new Point(50, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Instructions label
            lblInstrucciones = new Label();
            lblInstrucciones.Text = "Ingresa tu nombre de usuario para comenzar el juego.\n" +
                                   "Responderás preguntas de programación en C# y ganarás XP por cada respuesta correcta.";
            lblInstrucciones.Font = new Font("Arial", 12);
            lblInstrucciones.ForeColor = Color.FromArgb(70, 70, 70);
            lblInstrucciones.Size = new Size(600, 80);
            lblInstrucciones.Location = new Point(50, 120);
            lblInstrucciones.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInstrucciones);

            // Username textbox
            txtUsername = new TextBox();
            txtUsername.Font = new Font("Arial", 14);
            txtUsername.Size = new Size(400, 35);
            txtUsername.Location = new Point(150, 240);
            txtUsername.MaxLength = 50;
            txtUsername.KeyPress += TxtUsername_KeyPress;
            this.Controls.Add(txtUsername);

            // Start button
            btnComenzar = new Button();
            btnComenzar.Text = "Comenzar Juego";
            btnComenzar.Font = new Font("Arial", 14, FontStyle.Bold);
            btnComenzar.Size = new Size(250, 50);
            btnComenzar.Location = new Point(225, 300);
            btnComenzar.BackColor = Color.FromArgb(70, 130, 180);
            btnComenzar.ForeColor = Color.White;
            btnComenzar.FlatStyle = FlatStyle.Flat;
            btnComenzar.Click += BtnComenzar_Click;
            this.Controls.Add(btnComenzar);

            // Safebox icon for admin access
            pbSafeBox = new PictureBox();
            pbSafeBox.Size = new Size(40, 40);
            pbSafeBox.Location = new Point(this.ClientSize.Width - 50, this.ClientSize.Height - 50);
            pbSafeBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pbSafeBox.Cursor = Cursors.Hand;
            pbSafeBox.Click += PbSafeBox_Click;
            
            // Create a simple safebox icon using a bitmap
            Bitmap safeBoxIcon = new Bitmap(40, 40);
            using (Graphics g = Graphics.FromImage(safeBoxIcon))
            {
                g.Clear(Color.Transparent);
                // Draw a simple safebox icon
                g.FillRectangle(Brushes.DarkGray, 5, 10, 30, 25);
                g.FillRectangle(Brushes.Gray, 8, 13, 24, 19);
                g.FillEllipse(Brushes.Goldenrod, 15, 18, 10, 10);
                g.DrawEllipse(Pens.Black, 15, 18, 10, 10);
                g.FillRectangle(Brushes.Black, 18, 23, 4, 7);
            }
            pbSafeBox.Image = safeBoxIcon;
            this.Controls.Add(pbSafeBox);

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
                // Initialize game service only when needed
                if (gameService == null)
                {
                    gameService = ServiceFactory.GetGameService();
                }
                
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

                // Open informative form (only at the beginning)
                FormInformation formInformation = new FormInformation(userId, username);
                formInformation.Show();
                this.Hide();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("conexión") || ex.Message.Contains("connection"))
            {
                MessageBox.Show($"Error de conexión a la base de datos: {ex.Message}\n\n" +
                    "Por favor, verifique que:\n" +
                    "1. SQL Server esté instalado y ejecutándose\n" +
                    "2. La base de datos 'CodeQuest' exista\n" +
                    "3. El servidor 'DESKTOP-FN66L1D\\SQLEXPRESS' sea accesible\n\n" +
                    "Si el problema persiste, contacte al administrador del sistema.",
                    "Error de Conexión a Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la solicitud: {ex.Message}\n\n" +
                    $"Detalles técnicos: {ex.GetType().Name}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PbSafeBox_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.Show();
            this.Hide();
        }
    }
}