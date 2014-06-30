using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Exportador_Ventas_ServP.Controller;
using EstacionDB.VO;
using EstacionDB.Utilidades;

namespace Exportador_Ventas_ServP
{
    public partial class Clientes : Form
    {
        private ControladorPersistencia cp = new ControladorPersistencia();
        private ClienteVO currentClient = null;

        public Clientes()
        {
            InitializeComponent();
        }

        private void Clientes_Load(object sender, EventArgs e)
        {   
            cboTipoId.DataSource = cp.consultarTipos();
            clienteVOBindingSource.DataSource = cp.consultarClientes();
        }

        private void cmdGuardar_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Equals("") || txtNombre.Text.Equals("") || txtDireccion.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar el Tipo de indentificaci�n, el n�mero de identificaci�n, el nombre y la direcci�n del cliente", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                ClienteVO cli = new ClienteVO();

                if (currentClient != null)
                {
                    cli.IdCliente = currentClient.IdCliente;
                }
                else
                {
                    cli.IdCliente = 0;
                }

                cli.TipoId = (long)cboTipoId.SelectedValue;
                cli.Identificacion = txtId.Text;
                cli.Nombre = txtNombre.Text;
                cli.Direccion = txtDireccion.Text;
                cli.Telefono = txtTelefono.Text;
                cli.Contacto = txtContacto.Text;
                cli.Email = txtCorreo.Text;
                cli.Codigo = txtCodigo.Text;
                cli.Consecutivo = txtConsecutivo.Text;

                if (cp.guardarCliente(cli))
                {
                    MessageBox.Show("Datos del cliente han sido guardados", "", MessageBoxButtons.OK, MessageBoxIcon.Information);                                        
                    clienteVOBindingSource.DataSource = cp.consultarClientes();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar los datos del cliente", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            currentClient = null;
            cboTipoId.SelectedIndex = 0;
            txtId.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";
            txtContacto.Text = "";
            txtCorreo.Text = "";
            txtCodigo.Text = "";
            txtConsecutivo.Text = "";
            cboTipoId.Focus();
        }

        private void cmdNuevo_Click(object sender, EventArgs e)
        {
            currentClient = null;
            cboTipoId.SelectedIndex = 0;
            txtId.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";
            txtContacto.Text = "";
            txtCorreo.Text = "";
            txtCodigo.Text = "";
            txtConsecutivo.Text = "";
            cboTipoId.Focus();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (clienteVOBindingSource.Current != null)
            {
                currentClient = (ClienteVO)clienteVOBindingSource.Current;
                cboTipoId.SelectedValue = currentClient.TipoId;
                txtId.Text = currentClient.Identificacion;
                txtCodigo.Text = currentClient.Codigo;
                txtNombre.Text = currentClient.Nombre;
                txtDireccion.Text = currentClient.Direccion;
                txtTelefono.Text = currentClient.Telefono;
                txtContacto.Text = currentClient.Contacto;
                txtCorreo.Text = currentClient.Email;
                txtConsecutivo.Text = currentClient.Consecutivo;
            }
            cboTipoId.Focus();
        }

        private void cmdBuscarNit_Click(object sender, EventArgs e)
        {
            if (!txtId.Text.Equals(""))
            {
                clienteVOBindingSource.Clear();
                clienteVOBindingSource.DataSource = cp.consultarClientesPorCampo(txtId.Text, Utilidades.C_CLI_NIT);
            }
        }

        private void cmdBuscarCodigo_Click(object sender, EventArgs e)
        {
            if (!txtConsecutivo.Text.Equals(""))
            {
                clienteVOBindingSource.Clear();
                clienteVOBindingSource.DataSource = cp.consultarClientesPorCampo(txtConsecutivo.Text, Utilidades.C_CLI_CODIGO);
            }
        }

        private void cmdBuscarNombre_Click(object sender, EventArgs e)
        {
            if (!txtNombre.Text.Equals(""))
            {
                clienteVOBindingSource.Clear();
                clienteVOBindingSource.DataSource = cp.consultarClientesPorCampo(txtNombre.Text, Utilidades.C_CLI_NOMBRE);
            }
        }

        private void cmdConsultar_Click(object sender, EventArgs e)
        {
            clienteVOBindingSource.DataSource = cp.consultarClientes();
        }
    }
}