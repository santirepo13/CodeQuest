using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;
using CodeQuest.Models;

namespace CodeQuest
{
    public partial class FormQuestionManagement : Form
    {
        private readonly IGameService gameService;
        private DataGridView dgvQuestions;
        private Button btnCreateQuestion;
        private Button btnEditQuestion;
        private Button btnManageOptions;
        private Button btnDeleteQuestion;
        private Button btnRefresh;
        private Button btnClose;

        public FormQuestionManagement()
        {
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
            LoadQuestions();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Gestión de Preguntas";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 248, 255);

            dgvQuestions = new DataGridView();
            dgvQuestions.Location = new Point(20, 20);
            dgvQuestions.Size = new Size(840, 380);
            dgvQuestions.ReadOnly = true;
            dgvQuestions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuestions.AllowUserToAddRows = false;
            dgvQuestions.AllowUserToDeleteRows = false;
            dgvQuestions.BackgroundColor = Color.White;
            dgvQuestions.AutoGenerateColumns = true;
            this.Controls.Add(dgvQuestions);

            btnCreateQuestion = new Button();
            btnCreateQuestion.Text = "Crear Pregunta";
            btnCreateQuestion.Size = new Size(140, 36);
            btnCreateQuestion.Location = new Point(20, 420);
            btnCreateQuestion.Click += BtnCreateQuestion_Click;
            this.Controls.Add(btnCreateQuestion);

            btnEditQuestion = new Button();
            btnEditQuestion.Text = "Editar Pregunta";
            btnEditQuestion.Size = new Size(140, 36);
            btnEditQuestion.Location = new Point(175, 420);
            btnEditQuestion.Click += BtnEditQuestion_Click;
            this.Controls.Add(btnEditQuestion);

 
            btnManageOptions = new Button();
            btnManageOptions.Text = "Gestionar Opciones";
            btnManageOptions.Size = new Size(140, 36);
            btnManageOptions.Location = new Point(330, 420);
            btnManageOptions.Click += BtnManageOptions_Click;
            this.Controls.Add(btnManageOptions);
 
            btnDeleteQuestion = new Button();
            btnDeleteQuestion.Text = "Eliminar Pregunta";
            btnDeleteQuestion.Size = new Size(140, 36);
            btnDeleteQuestion.Location = new Point(485, 420);
            btnDeleteQuestion.Click += BtnDeleteQuestion_Click;
            this.Controls.Add(btnDeleteQuestion);
 
            btnRefresh = new Button();
            btnRefresh.Text = "Refrescar";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.Location = new Point(640, 420);
            btnRefresh.Click += (s, e) => LoadQuestions();
            this.Controls.Add(btnRefresh);
 
            btnClose = new Button();
            btnClose.Text = "Cerrar";
            btnClose.Size = new Size(120, 36);
            btnClose.Location = new Point(795, 470);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            this.ResumeLayout(false);
        }

        private void LoadQuestions()
        {
            try
            {
                var questions = gameService.GetAllQuestions();
                dgvQuestions.DataSource = questions;
                if (dgvQuestions.Columns["Choices"] != null)
                    dgvQuestions.Columns["Choices"].Visible = false;
                if (dgvQuestions.Columns["QuestionID"] != null)
                    dgvQuestions.Columns["QuestionID"].HeaderText = "ID";
                if (dgvQuestions.Columns["Text"] != null)
                    dgvQuestions.Columns["Text"].HeaderText = "Pregunta";
                if (dgvQuestions.Columns["Difficulty"] != null)
                    dgvQuestions.Columns["Difficulty"].HeaderText = "Dificultad";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar preguntas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                string text = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el texto de la pregunta:", "Crear Pregunta", "");
                if (string.IsNullOrWhiteSpace(text)) return;
                string diffStr = Microsoft.VisualBasic.Interaction.InputBox("Ingrese dificultad (1-3):", "Crear Pregunta", "1");
                if (!int.TryParse(diffStr, out int difficulty) || difficulty < 1 || difficulty > 3)
                {
                    MessageBox.Show("Dificultad inválida. Debe ser 1, 2 o 3.", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var question = new Question(text.Trim(), difficulty);
                int questionId = gameService.CreateQuestion(question);

                string countStr = Microsoft.VisualBasic.Interaction.InputBox("¿Cuántas opciones desea agregar? (2-4)", "Opciones", "4");
                if (!int.TryParse(countStr, out int count) || count < 2 || count > 4) count = 4;

                bool hasCorrect = false;
                for (int i = 1; i <= count; i++)
                {
                    string ctext = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese texto para la opción #{i}:", "Crear Opción", "");
                    if (string.IsNullOrWhiteSpace(ctext))
                    {
                        i--;
                        continue;
                    }
                    var isCorrect = MessageBox.Show($"¿Es esta opción la correcta?\n\n{ctext}", "Marcar correcta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    var choice = new Choice(ctext.Trim(), isCorrect);
                    gameService.CreateChoice(questionId, choice);
                    if (isCorrect) hasCorrect = true;
                }

                if (!hasCorrect)
                {
                    MessageBox.Show("Advertencia: No se marcó ninguna opción como correcta. Puede editar la pregunta y agregar opciones correctas después.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                MessageBox.Show("Pregunta creada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadQuestions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear pregunta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvQuestions.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una pregunta.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var row = dgvQuestions.SelectedRows[0];
                int qid = Convert.ToInt32(row.Cells["QuestionID"].Value);
                string currentText = row.Cells["Text"].Value.ToString();
                int currentDiff = Convert.ToInt32(row.Cells["Difficulty"].Value);

                string newText = Microsoft.VisualBasic.Interaction.InputBox("Editar texto de la pregunta:", "Editar Pregunta", currentText);
                if (string.IsNullOrWhiteSpace(newText)) return;
                string diffStr = Microsoft.VisualBasic.Interaction.InputBox("Editar dificultad (1-3):", "Editar Pregunta", currentDiff.ToString());
                if (!int.TryParse(diffStr, out int newDiff) || newDiff < 1 || newDiff > 3)
                {
                    MessageBox.Show("Dificultad inválida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var q = new Question() { QuestionID = qid, Text = newText.Trim(), Difficulty = newDiff };
                if (gameService.UpdateQuestion(q))
                {
                    MessageBox.Show("Pregunta actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadQuestions();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la pregunta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar pregunta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnManageOptions_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvQuestions.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una pregunta.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var row = dgvQuestions.SelectedRows[0];
                int qid = Convert.ToInt32(row.Cells["QuestionID"].Value);
                string qtext = row.Cells["Text"].Value.ToString();

                using (var form = new FormManageOptions(qid, qtext))
                {
                    form.ShowDialog();
                }

                LoadQuestions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestor de opciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvQuestions.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una pregunta.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var row = dgvQuestions.SelectedRows[0];
                int qid = Convert.ToInt32(row.Cells["QuestionID"].Value);
                string text = row.Cells["Text"].Value.ToString();

                var res = MessageBox.Show($"¿Eliminar la pregunta?\n\n{text}", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    if (gameService.DeleteQuestion(qid))
                    {
                        MessageBox.Show("Pregunta eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadQuestions();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar la pregunta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar pregunta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}