using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.View
{
    public partial class ExemplarsForm
    {
        private void InitializeComponent()
        {
            dgvExemplars = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvExemplars.CellClick += dgvExemplars_CellClick;
            dgvExemplars.RowPrePaint += dgvExemplars_RowPrePaint;

            Controls.Add(dgvExemplars);
        }
    }
}
