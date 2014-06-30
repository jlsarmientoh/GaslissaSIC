using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EstacionDB.VO;
using Exportador_Ventas_ServP.Controller;
using EstacionDB.Utilidades;
using System.Configuration;
using System.IO;
using EstacionDB.Exceptions;

namespace Exportador_Ventas_ServP
{
    public partial class ExportadorContingencia : Form
    {
        private double totalVentas = 0;
        private double totalConsumos = 0;
        private double totalCredito = 0;
        private double totalSodexo = 0;
        private double totalEfectivo = 0;
        private double totalBigPass = 0;
        private double totalOtros = 0;
        private double totalTarjetas = 0;
        private double totalPlus = 0;
        private double totalTicketTronik = 0;

        private ControladorPersistencia cp = new ControladorPersistencia();

        private List<ProductoTurnoVO> productosTurno;

        private List<VentaVO> ventasExportar;

        public List<VentaVO> VentasExportar
        {
            get { return ventasExportar; }
            set { ventasExportar = value; }
        }

        public ExportadorContingencia()
        {
            InitializeComponent();                        
            frameAdicionar.Enabled = false;
            empleadoVOBindingSource.DataSource = cp.getEmpleados();
            productosTurno = new List<ProductoTurnoVO>();
        }

        private void cmdConsultar_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            ventaVOBindingSource.DataSource = null;
            totalEfectivo = 0;
            totalOtros = 0;
            totalSodexo = 0;
            totalBigPass = 0;
            totalCredito = 0;
            totalTarjetas = 0;
            totalPlus = 0;
            totalTicketTronik = 0;
            totalVentas = 0;
            totalConsumos = 0;
            try
            {
                if (cboEmpleados.SelectedIndex != -1 && !txtIsla.Text.Equals("") && !txtTurno.Text.Equals(""))
                {

                    long codEmp = long.Parse(cboEmpleados.SelectedValue.ToString());
                    long isla = long.Parse(txtIsla.Text);
                    long turno = long.Parse(txtTurno.Text);
                    cp = new ControladorPersistencia();
                    DateTime fecha = new DateTime(fechaDesdePicker.Value.Year, fechaDesdePicker.Value.Month, fechaDesdePicker.Value.Day);
                    if (cp.existeCierre(codEmp, turno, isla, fecha, fecha) == 0)
                    {
                        totalCredito = cp.consultarTotalVentasFidelizadas(codEmp, turno, isla, fecha, fecha);
                        ventaVOBindingSource.DataSource = cp.consultarVentasFidelizados(codEmp, fecha, fecha, isla, turno);
                        //productosTurno = cp.consultarProductosTurno(fecha, fecha, isla, turno);

                        /*if (productosTurno != null && productosTurno.Count > 0)
                        {   
                            foreach (ProductoTurnoVO pt in productosTurno)
                            {
                                totalConsumos += pt.Valor;
                            }
                        }*/

                        if (ventaVOBindingSource.Count == 0)
                        {
                            MessageBox.Show("No se encontraron ventas cr�dito para la fecha, turno e isla seleccionados", "Resultado del cierre ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        cmdGuardar.Enabled = true;
                        frameAdicionar.Enabled = false;

                        txtNombre.Text = "";
                        txtId.Text = "";
                        txtTiquete.Text = "";
                        txtPlaca.Text = "";

                        actualizarTotales();
                    }
                    else
                    {
                        MessageBox.Show("Ya se ha realizado cierre para la fecha seleccionada", "Resultado del cierre ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Debe ingresar la fecha, el c�digo de empleado, el turno y la isla", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    restaurarCampos();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;                
                if(ex.InnerException != null) 
                {
                    msg += " " + ex.InnerException.Message;
                }
                MessageBox.Show(msg);
            }
            this.UseWaitCursor = false;
        }

        private void cmdAgregar_Click(object sender, EventArgs e)
        {
            if (txtTiquete.Text.Equals("") || txtId.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar el n�mero del tiquete y la identificaci�n del cliente", "", MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }
            else
            {
                cp = new ControladorPersistencia();
                DateTime fecha = new DateTime(fechaDesdePicker.Value.Year, fechaDesdePicker.Value.Month, fechaDesdePicker.Value.Day);
                VentaVO venta = cp.consultarVentaTurno(long.Parse(txtTiquete.Text), fecha, fecha, int.Parse(txtIsla.Text), int.Parse(txtTurno.Text));
                if (venta != null)
                {
                    venta.Nit = txtId.Text;
                    if (txtNombre.Text.Equals(""))
                    {
                        venta.Cliente = "CLIENTE NO FIDELIZADO";
                    }
                    else
                    {
                        venta.Cliente = txtNombre.Text;
                    }
                    if (!txtPlaca.Text.Equals(""))
                    {
                        venta.Placa = txtPlaca.Text;
                    }

                    if (chkPlus.Checked)
                    {
                        venta.ModoPago = 2;
                        totalPlus += venta.Total;
                    }
                    else
                    {
                        venta.ModoPago = 1;
                        totalCredito += venta.Total;
                    }

                    ventaVOBindingSource.Add(venta);
                    actualizarTotales();

                    MessageBox.Show("Tiquete agregado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtNombre.Text = "";
                    txtId.Text = "";
                    txtTiquete.Text = "";
                    txtPlaca.Text = "";                    
                    cboCliente.SelectedIndex = 0;
                    chkPlus.Checked = false;
                }
                else
                {
                    MessageBox.Show("N�mero de tiquete no existe", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmdLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                long tiquete = long.Parse(txtTiquete.Text);

                foreach (VentaVO venta in ventaVOBindingSource.List)
                {
                    if (venta.Tiquete == tiquete)
                    {
                        switch (venta.ModoPago)
                        {
                            case 1:
                                {
                                    totalCredito -= venta.Total;
                                    break;
                                }
                            case 2:
                                {
                                    totalPlus -= venta.Total;
                                    break;
                                }
                            case 7:
                                {
                                    totalCredito -= venta.Total;
                                    break;
                                }
                        }
                        ventaVOBindingSource.Remove(venta);
                        GridVentasCargadas.Update();
                        actualizarTotales();
                    }
                }
            }
            catch (Exception ex)
            {
                txtNombre.Text = "";
                txtId.Text = "";
                txtTiquete.Text = "";
            }
        }

        private void cmdHabilitarAdicion_Click(object sender, EventArgs e)
        {
            frameAdicionar.Enabled = true;
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNombre.Text = cboCliente.Text;
            txtId.Text = (string)cboCliente.SelectedValue;
        }

        private void Exportador_Load(object sender, EventArgs e)
        {
            cboCliente.DataSource = cp.consultarClientes();
        }

        private void cmdGuardar_Click(object sender, EventArgs e)
        {
            String resumen = "";
            resumen = "Resumen del cierre:\n\n" +
                "\tCreditos:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalCredito) + "\n" +
                "\tEfectivo:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalEfectivo) + "\n" +
                "\tSodexho:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalSodexo) + "\n" +                
                "\tBig Pass:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalBigPass) + "\n" +
                "\tTarjetas:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalTarjetas) + "\n" +
                "\tTarjeta Plus:\t\t$ " + String.Format("{0,10:#,0.00}", totalPlus) + "\n" +
                "\tTicket Prom:\t\t$ " + String.Format("{0,10:#,0.00}", totalTicketTronik) + "\n" +
                "\tOtros:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalOtros) + "\n" +
                "TOTAL:\t\t\t$ " + String.Format("{0,10:#,0.00}", totalVentas) + "\n\n" +
                "�Desea continuar con el cierre?\n";
            DialogResult res = MessageBox.Show(resumen, "Resumen del cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                DateTime fecha = new DateTime(fechaDesdePicker.Value.Year, fechaDesdePicker.Value.Month, fechaDesdePicker.Value.Day);
                this.UseWaitCursor = true;
                CierreVentasVO cierre = new CierreVentasVO();
                cierre.CodEmpleado = long.Parse(cboEmpleados.SelectedValue.ToString());
                cierre.Isla = long.Parse(txtIsla.Text.Trim());
                cierre.Turno = long.Parse(txtTurno.Text.Trim());
                cierre.Efectivo = totalEfectivo;
                cierre.Creditos = totalCredito;
                cierre.Sodexo = totalSodexo;
                cierre.BigPass = totalBigPass;
                cierre.Tarjetas = totalTarjetas;
                cierre.TarjetaPlus = totalPlus;
                cierre.Otros = totalOtros;
                cierre.TicketTronik = totalTicketTronik;
                cierre.TotalVentas = totalVentas;
                cierre.Fecha = fechaDesdePicker.Value;

                if (cp.existeCierre(cierre.CodEmpleado, cierre.Turno, cierre.Isla, fecha, fecha) == 0)
                {

                    int rows = 0;
                    int rowsTurno = 0;

                    rows = cp.guardarCierre(cierre);

                    if (rows > 0)
                    {

                        System.Collections.IList tmpVentas = ventaVOBindingSource.List;
                        VentaVO[] ventas = new VentaVO[tmpVentas.Count];
                        tmpVentas.CopyTo(ventas, 0);
                        ventasExportar = new List<VentaVO>(ventas);

                        rows = cp.guardarVentasDia(ventasExportar);
                        rowsTurno = cp.guardarProductosTurno(productosTurno);
                        MessageBox.Show("Se guardaron (" + rows + ") ventas en este cierre", "Resultado del cierre ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        totalEfectivo = 0;
                        totalOtros = 0;
                        totalSodexo = 0;
                        totalBigPass = 0;
                        totalCredito = 0;
                        totalTarjetas = 0;
                        totalPlus = 0;
                        totalTicketTronik = 0;
                        totalVentas = 0;
                        totalConsumos = 0;

                        restaurarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo registrar el cierre", "Resultado del cierre ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ya se ha realizado cierre para la fecha seleccionada", "Resultado del cierre ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                  
                UseWaitCursor = false;
            }
        }

        private void cmdRecargarCliente_Click(object sender, EventArgs e)
        {
            cboCliente.DataSource = cp.consultarClientes();
        }

        private void actualizarTotales()
        {
            totalVentas = 0;
            totalVentas += totalEfectivo;
            totalVentas += totalOtros;
            totalVentas += totalSodexo;
            totalVentas += totalBigPass;
            totalVentas += totalCredito;
            totalVentas += totalTarjetas;
            totalVentas += totalPlus;
            totalVentas += totalTicketTronik;

            txtCreditos.Text = String.Format("{0,10:#,0.00}", totalCredito);
            txtEfectivo.Text = String.Format("{0,10:#,0.00}", totalEfectivo);
            txtOtros.Text = String.Format("{0,10:#,0.00}", totalOtros);
            txtSodexo.Text = String.Format("{0,10:#,0.00}", totalSodexo);
            txtBigpass.Text = String.Format("{0,10:#,0.00}", totalBigPass);
            txtTarjetas.Text = String.Format("{0,10:#,0.00}", totalTarjetas);
            txtTarjetaPlus.Text = String.Format("{0,10:#,0.00}", totalPlus);
            txtTotal.Text = String.Format("{0,10:#,0.00}", totalVentas);
            txtTicketTronik.Text = String.Format("{0,10:#,0.00}", totalTicketTronik);
            txtTotalVentas.Text = String.Format("{0,10:#,0.00}", totalConsumos);
        }

        private void restaurarCampos()
        {
            cmdGuardar.Enabled = false;
            frameAdicionar.Enabled = false;
            ventaVOBindingSource.Clear();
            actualizarTotales();
        }

        private void txtEfectivo_Leave(object sender, EventArgs e)
        {
            try
            {
                totalEfectivo = double.Parse(txtEfectivo.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtSodexo_Leave(object sender, EventArgs e)
        {
            try
            {
                totalSodexo = double.Parse(txtSodexo.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtBigpass_Leave(object sender, EventArgs e)
        {
            try
            {
                totalBigPass = double.Parse(txtBigpass.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtCreditos_Leave(object sender, EventArgs e)
        {
            try
            {
                totalCredito = double.Parse(txtCreditos.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtOtros_Leave(object sender, EventArgs e)
        {
            try
            {
                totalOtros = double.Parse(txtOtros.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtEfectivo_Enter(object sender, EventArgs e)
        {
            txtEfectivo.SelectionStart = 0;
            txtEfectivo.SelectionLength = txtEfectivo.Text.Length;
        }

        private void txtSodexo_Enter(object sender, EventArgs e)
        {
            txtSodexo.SelectionStart = 0;
            txtSodexo.SelectionLength = txtSodexo.Text.Length;
        }

        private void txtBigpass_Enter(object sender, EventArgs e)
        {
            txtBigpass.SelectionStart = 0;
            txtBigpass.SelectionLength = txtBigpass.Text.Length;
        }

        private void txtOtros_Enter(object sender, EventArgs e)
        {
            txtOtros.SelectionStart = 0;
            txtOtros.SelectionLength = txtOtros.Text.Length;
        }

        private void txtTarjetaPlus_Enter(object sender, EventArgs e)
        {
            txtTarjetaPlus.SelectionStart = 0;
            txtTarjetaPlus.SelectionLength = txtTarjetaPlus.Text.Length;
        }

        private void txtTarjetas_Enter(object sender, EventArgs e)
        {
            txtTarjetas.SelectionStart = 0;
            txtTarjetas.SelectionLength = txtTarjetas.Text.Length;
        }

        private void txtTarjetas_Leave(object sender, EventArgs e)
        {
            try
            {
                totalTarjetas = double.Parse(txtTarjetas.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtEfectivo_MouseClick(object sender, MouseEventArgs e)
        {
            txtEfectivo.SelectionStart = 0;
            txtEfectivo.SelectionLength = txtEfectivo.Text.Length;
        }

        private void txtSodexo_MouseClick(object sender, MouseEventArgs e)
        {
            txtSodexo.SelectionStart = 0;
            txtSodexo.SelectionLength = txtSodexo.Text.Length;
        }

        private void txtBigpass_MouseClick(object sender, MouseEventArgs e)
        {
            txtBigpass.SelectionStart = 0;
            txtBigpass.SelectionLength = txtBigpass.Text.Length;
        }

        private void txtTarjetas_MouseClick(object sender, MouseEventArgs e)
        {
            txtTarjetas.SelectionStart = 0;
            txtTarjetas.SelectionLength = txtTarjetas.Text.Length;
        }

        private void txtOtros_MouseClick(object sender, MouseEventArgs e)
        {
            txtOtros.SelectionStart = 0;
            txtOtros.SelectionLength = txtOtros.Text.Length;
        }

        private void txtTicketTronik_Leave(object sender, EventArgs e)
        {
            try
            {
                totalTicketTronik = double.Parse(txtTicketTronik.Text);
            }
            catch { }
            actualizarTotales();
        }

        private void txtTicketTronik_Enter(object sender, EventArgs e)
        {
            txtTicketTronik.SelectionStart = 0;
            txtTicketTronik.SelectionLength = txtTicketTronik.Text.Length;
        }

        private void txtTicketTronik_MouseClick(object sender, MouseEventArgs e)
        {
            txtTicketTronik.SelectionStart = 0;
            txtTicketTronik.SelectionLength = txtTicketTronik.Text.Length;
        }

        private void cierreWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void cmdCargarLecturas_Click(object sender, EventArgs e)
        {
            totalConsumos = 0;
            DateTime fecha = new DateTime(fechaDesdePicker.Value.Year, fechaDesdePicker.Value.Month, fechaDesdePicker.Value.Day);
            productosTurno.Clear();

            if (!txtLecIniCorriente.Text.Equals("") && !txtLecFinCorriente.Text.Equals(""))
            {
                double lecInicialCorriente = Utilidades.parsearDecimal(txtLecIniCorriente.Text);
                double lecFinCorriente = Utilidades.parsearDecimal(txtLecFinCorriente.Text);
                double consumoCorriente = lecFinCorriente - lecInicialCorriente;

                SobretasaVO s = cp.consultarSobretasasProductoFecha(fecha.Month, fecha.Year, 1, fecha.Day);
                if (s != null)
                {
                    ProductoTurnoVO ptCorriente = new ProductoTurnoVO();
                    ptCorriente.Fecha = fecha;
                    ptCorriente.Galones = consumoCorriente;
                    ptCorriente.Isla = long.Parse(txtIsla.Text.Trim());
                    ptCorriente.Turno = long.Parse(txtTurno.Text.Trim());

                    consumoCorriente *= s.Venta;

                    ptCorriente.Valor = consumoCorriente;
                    productosTurno.Add(ptCorriente);

                    totalConsumos += consumoCorriente;
                }
                else
                {
                    MessageBox.Show("Falta la informaci�n de sobretasa Corriente vigente para la fecha seleccionada", "Falta informaci�n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!txtLecIniSuper.Text.Equals("") && !txtLecFinSuper.Text.Equals(""))
            {
                double lecInicialSuper = Utilidades.parsearDecimal(txtLecIniSuper.Text);
                double lecFinSuper = Utilidades.parsearDecimal(txtLecFinSuper.Text);
                double consumoSuper = lecFinSuper - lecInicialSuper;

                SobretasaVO s = cp.consultarSobretasasProductoFecha(fecha.Month, fecha.Year, 2, fecha.Day);
                if (s != null)
                {
                    ProductoTurnoVO ptSuper = new ProductoTurnoVO();
                    ptSuper.Fecha = fecha;
                    ptSuper.Galones = consumoSuper;
                    ptSuper.Isla = long.Parse(txtIsla.Text.Trim());
                    ptSuper.Turno = long.Parse(txtTurno.Text.Trim());

                    consumoSuper *= s.Venta;

                    ptSuper.Valor = consumoSuper;
                    productosTurno.Add(ptSuper);

                    totalConsumos += consumoSuper;
                }
                else
                {
                    MessageBox.Show("Falta la informaci�n de sobretasa Super vigente para la fecha seleccionada", "Falta informaci�n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!txtLecIniDiesel.Text.Equals("") && !txtLecFinDiesel.Text.Equals(""))
            {
                double lecInicialDiesel = Utilidades.parsearDecimal(txtLecIniDiesel.Text);
                double lecFinDiesel = Utilidades.parsearDecimal(txtLecFinDiesel.Text);
                double consumoDiesel = lecFinDiesel - lecInicialDiesel;

                SobretasaVO s = cp.consultarSobretasasProductoFecha(fecha.Month, fecha.Year, 3, fecha.Day);
                if (s != null)
                {
                    ProductoTurnoVO ptDiesel = new ProductoTurnoVO();
                    ptDiesel.Fecha = fecha;
                    ptDiesel.Galones = consumoDiesel;
                    ptDiesel.Isla = long.Parse(txtIsla.Text.Trim());
                    ptDiesel.Turno = long.Parse(txtTurno.Text.Trim());

                    consumoDiesel *= s.Venta;

                    ptDiesel.Valor = consumoDiesel;
                    productosTurno.Add(ptDiesel);

                    totalConsumos += consumoDiesel;
                }
                else
                {
                    MessageBox.Show("Falta la informaci�n de sobretasa Diesel vigente para la fecha seleccionada", "Falta informaci�n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            actualizarTotales();
        }
    }
}