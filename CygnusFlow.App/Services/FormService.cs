using CygnusFlow.Application.Services;
using CygnusFlow.Domain.Shared;

namespace CygnusFlow.App.Services
{
    public class FormService
    {
        private readonly NotificationService _notificationService;

        public FormService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ExibirNotificacoes(Form form, NotificationResult notifications)
        {
            if (notifications.Errors.Any())
            {
                ExibirErros(form, notifications.Errors);
            }

            if (notifications.Warnings.Any())
            {
                ExibirAlertas(form, notifications.Warnings);
            }
        }

        public void ExibirErros(Form form, List<Error> errors)
        {
            var errorPanel = form.Controls.Find("pnlErrors", true).FirstOrDefault();
            if (errorPanel != null)
            {
                errorPanel.Controls.Clear();
                errorPanel.Visible = true;

                foreach (var error in errors)
                {
                    var label = new Label
                    {
                        Text = $"• {error.Message}",
                        ForeColor = Color.DarkRed,
                        AutoSize = true,
                        Dock = DockStyle.Top
                    };
                    errorPanel.Controls.Add(label);
                }
            }
            else
            {
                var errorMessage = string.Join("\n", errors.Select(e => e.Message));
                MessageBox.Show(errorMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExibirAlertas(Form form, List<Warning> warnings)
        {
            var warningMessage = string.Join("\n", warnings.Select(w => w.Message));
            MessageBox.Show(warningMessage, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void LimparErros(Form form)
        {
            var errorPanel = form.Controls.Find("pnlErrors", true).FirstOrDefault();
            if (errorPanel != null)
            {
                errorPanel.Controls.Clear();
                errorPanel.Visible = false;
            }
        }

        public void ConfigurarDataGridView(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(24, 65, 148);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgv.EnableHeadersVisualStyles = false;
        }

        public void EstiloFormulario(Form form)
        {
            form.Font = new Font("Segoe UI", 9);
            form.BackColor = Color.White;
        }
    }
}
