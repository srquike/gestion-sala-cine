using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCine
{
    public partial class FormularioCine : Form
    {
        public FormularioCine()
        {
            InitializeComponent();
        }

        int butacasSelec = 0,
            butacasSoli = 0,
            adultos = 0,
            aMayores = 0,
            niños = 0,
            total = 0;

        bool salir = false;

        string[,] arrayAsientos = new string[10, 20];

        Image asientoDisponible = Properties.Resources.Disponible2;
        Image asientoReservado = Properties.Resources.Ocupado1;
        Image asientoSeleccionado = Properties.Resources.Seleccionado1;

        private void RestaurarDataGridView()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    DGVAsientos.Rows[i].Cells[j].Value = asientoDisponible;
                    arrayAsientos[i, j] = "Disponible";
                }
            }
        }

        private int AsientosDisponibles()
        {
            int numDispo = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    string estado = arrayAsientos[i, j];
                    if (estado == "Disponible")

                    {
                        numDispo++;
                    }
                }
            }
            return numDispo;
        }

        private void DeseleccionarAsientos()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    string estadoAsientos = arrayAsientos[i, j];
                    if (estadoAsientos == "Seleccionado")
                    {
                        arrayAsientos[i, j] = "Disponible";
                        DGVAsientos
.Rows[i].Cells[j].Value = asientoDisponible;
                    }
                }
            }
            butacasSelec = 0;
        }

        private void Reservar()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    string estadoAsiento = arrayAsientos[i, j];
                    if (estadoAsiento == "Seleccionado")
                    {
                        arrayAsientos[i, j] = "Reservado";
                        DGVAsientos.Rows[i].Cells[j].Value = asientoReservado;
                    }
                }
            }
        }

        private void LlenarDGV()
        {
            DGVAsientos.Rows.Clear();
            string letraMayu;
            int numLetra = 64;
            for (int i = 0; i < 10; i++)
            {
                DGVAsientos.Rows.Add();
                numLetra++;

                for (int j = 0; j < 20; j++)
                {
                    letraMayu = Convert.ToChar(numLetra).ToString();
                    DGVAsientos.Rows[i].HeaderCell.Value = letraMayu;
                    DGVAsientos.Columns[j].Resizable = DataGridViewTriState.False;
                    DGVAsientos.Rows[i].Cells[j].Value = asientoDisponible;
                    arrayAsientos[i, j] = "Disponible";
                }
            }
        }

        private void FormularioCine_Load(object sender, EventArgs e)
        {
            LlenarDGV();
        }

        private void DGVAsientos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (butacasSelec == butacasSoli)
                {
                    MessageBox.Show("Seleccione " + butacasSoli + " asientos.");
                    DeseleccionarAsientos();
                }
                else
                {
                    string estadoAsiento = arrayAsientos[e.RowIndex, e.ColumnIndex];
                    if (estadoAsiento == "Disponible")
                    {
                        arrayAsientos[e.RowIndex, e.ColumnIndex] = "Seleccionado";
                        DGVAsientos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = asientoSeleccionado;

                        butacasSelec++;
                    }
                    else if (estadoAsiento == "Seleccionado")
                    {
                        arrayAsientos[e.RowIndex, e.ColumnIndex] = "Disponible";
                        DGVAsientos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = asientoDisponible;

                        butacasSelec--;
                    }
                    else if (estadoAsiento == "Reservado")
                    {
                        MessageBox.Show("El asiento esta reservado.");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Selección Invalida.");
            }
        }

        private void BotonSelec_Click(object sender, EventArgs e)
        {
            butacasSoli = Convert.ToInt32(NumericAdulto.Value + NumericAmayor.Value + NumericNino.Value);
            if (butacasSoli > AsientosDisponibles())
            {
                MessageBox.Show("Los asientos disponibles no son sufientes.");
            }
            else if (NumericAdulto.Value == 0 && NumericNino.Value > 0 && NumericAmayor.Value == 0)
            {
                MessageBox.Show("Los niños deberán estar siempre acompañados de al menos un adulto.");
            }
            else
            {
                if (RadioManual.Checked)
                {
                    DGVAsientos.Enabled = true;
                }
                else if (RadioAuto.Checked)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (salir)
                        {
                            break;
                        }
                        else
                            for (int j = 0; j < 20; j++)
                            {
                                string estadoAsiento = arrayAsientos[i, j];
                                if (butacasSelec == butacasSoli)
                                {
                                    salir = true;
                                    break;
                                }
                                else
                                {
                                    if (estadoAsiento == "Disponible")
                                    {
                                        arrayAsientos[i, j] = "Seleccionado";
                                        DGVAsientos.Rows[i].Cells[j].Value = asientoSeleccionado;

                                        butacasSelec++;
                                    }
                                    else if (estadoAsiento == "Reservado")
                                    {
                                        continue;
                                    }
                                }
                            }
                    }
                    salir = false;
                }
            }
        }

        private void BotonGuardar_Click(object sender, EventArgs e)
        {
            if (butacasSelec == butacasSoli)
            {
                Reservar();

                adultos += Convert.ToInt32(NumericAdulto.Value);
                aMayores += Convert.ToInt32(NumericAmayor.Value);
                niños += Convert.ToInt32(NumericNino.Value);

                total += butacasSoli;

                LabelAdulto.Text = "Adulto: " + adultos;
                LabelAmayor.Text = "Adulto Mayor: " + aMayores;
                LabelNino.Text = "Niño: " + niños;
                LabelTotal.Text = "Total: " + total;

                butacasSelec = 0;
                butacasSoli = 0;

                DGVAsientos.Enabled = false;
            }
            else
            {
                MessageBox.Show("Selecciones faltantes " + (butacasSoli - butacasSelec));
            }
        }

        private void BotonCancelar_Click(object sender, EventArgs e)
        {
            DeseleccionarAsientos();

            DGVAsientos.Enabled = false;
            RadioManual.Checked = false;
            RadioAuto.Checked = false;
            butacasSelec = 0;
            butacasSoli = 0;
        }

        private void BotonLimpiar_Click(object sender, EventArgs e)
        {
            RestaurarDataGridView();

            butacasSoli = 0;
            butacasSelec = 0;

            NumericAdulto.Value = 0;
            NumericAmayor.Value = 0;
            NumericNino.Value = 0;

            RadioManual.Checked = false;
            RadioAuto.Checked = false;

            adultos = 0;
            aMayores = 0;
            niños = 0;
            total = 0;

            LabelAdulto.Text = "Adulto: ";
            LabelAmayor.Text = "Adulto Mayor: ";
            LabelNino.Text = "Niño: ";
            LabelTotal.Text = "Total: ";
        }
    }
}