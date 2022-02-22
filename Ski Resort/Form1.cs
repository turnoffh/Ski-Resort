using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ski_Resort
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "txt Files |*.txt";
            DialogResult dr = openFileDialog.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
                lblRoute.Text = openFileDialog.FileName;

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (lblRoute.Text == "")
            {
                MessageBox.Show("Seleccione un archivo para continuar");
            }
            else if (ddlTipoSeparador.SelectedItem == null || ddlTipoSeparador.SelectedItem?.ToString() == "")
            {
                MessageBox.Show("Seleccione un tipo de separador para continuar");
            }
            else
            {

                try
                {
                    Process objProcesar = new Process();
                    char separador;
                    switch (ddlTipoSeparador.SelectedItem.ToString())
                    {
                        case "Coma":
                            separador = ',';
                            break;
                        case "Punto y Coma":
                            separador = ';';
                            break;
                        case "Pipe":
                            separador = '|';
                            break;
                        case "Espacio":
                            separador = ' ';
                            break;
                        default:
                            separador = ' ';
                            break;
                    }
                    MessageBox.Show(objProcesar.Procesar(lblRoute.Text, separador));
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                
            }
        }
    }
}
