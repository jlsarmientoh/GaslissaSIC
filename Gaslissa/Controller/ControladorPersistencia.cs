using System;
using System.Collections.Generic;
using System.Text;
using EstacionDB.Exceptions;
using EstacionDB.VO;
using System.Collections;
using EstacionDB.DTO;
using System.Globalization;
using NHibernate.Mapping;
using EstacionDB.DAO;
using EstacionDB.Utilidades;
using Exportador_Ventas_ServP.Reports.DTO;

namespace Exportador_Ventas_ServP.Controller
{
    public class ControladorPersistencia
    {
        #region definici�n de los DAO's
        private VentasDAO ventasDAO = null;
        private LecturasDAO lecturasDAO = null;
        private ClientesDAO clientesDAO = null;
        private TipoIdDAO tipoDAO = null;
        private BancosDAO bancosDAO = null;
        private IngresosDAO ingresosDAO = null;
        private EgresosDAO egresosDAO = null;
        private CierreVentasDAO cierreVentasDAO = null;
        private EmpleadoDAO empleadosDAO = null;
        private ProductosTurnoDAO productosTurnoDAO = null;
        private SobretasasDAO sobretasasDAO = null;
        private ProductosDAO productosDAO = null;
        private ControlCombustibleDAO controlCombustibleDAO = null;
        private TanquesDAO tanquesDAO = null;
        private VolumenTanqueDAO volumenesDAO = null;
        private CompraCombustibleDAO comprasDAO = null;
        #endregion
        #region m�todos para obtener instancias de los DAO's
        private VentasDAO getVentasDAO()
        {
            if (ventasDAO == null)
            {
                ventasDAO = new VentasDAO();
            }
            return ventasDAO;
        }

        private LecturasDAO getLecturasDAO()
        {
            if (lecturasDAO == null)
            {
                lecturasDAO =  new LecturasDAO();
            }
            return lecturasDAO;
        }

        private ClientesDAO getClientesDAO()
        {
            if (clientesDAO == null)
            {
                clientesDAO = new ClientesDAO();
            }
            return clientesDAO;
        }

        private TipoIdDAO getTiposDAO()
        {
            if (tipoDAO == null)
            {
                tipoDAO = new TipoIdDAO();
            }
            return tipoDAO;
        }

        private BancosDAO getBancosDAO()
        {
            if (bancosDAO == null)
            {
                bancosDAO = new BancosDAO();
            }
            return bancosDAO;
        }

        private IngresosDAO getIngresosDAO()
        {
            if (ingresosDAO == null)
            {
                ingresosDAO = new IngresosDAO();
            }
            return ingresosDAO;
        }

        private EgresosDAO getEgresosDAO()
        {
            if (egresosDAO == null)
            {
                egresosDAO = new EgresosDAO();
            }
            return egresosDAO;
        }

        private CierreVentasDAO getCierreDAO()
        {
            if (cierreVentasDAO == null)
            {
                cierreVentasDAO = new CierreVentasDAO();
            }
            return cierreVentasDAO;
        }

        private EmpleadoDAO getEmpleadoDAO()
        {
            if (empleadosDAO == null)
            {
                empleadosDAO = new EmpleadoDAO();
            }
            return empleadosDAO;
        }

        private ProductosTurnoDAO getProductosTurnoDAO()
        {
            if (productosTurnoDAO == null)
            {
                productosTurnoDAO = new ProductosTurnoDAO();
            }
            return productosTurnoDAO;
        }

        private SobretasasDAO getSobreTasasDAO()
        {
            if (sobretasasDAO == null)
            {
                sobretasasDAO = new SobretasasDAO();
            }
            return sobretasasDAO;
        }

        private ProductosDAO getProductosDAO()
        {
            if (productosDAO == null)
            {
                productosDAO = new ProductosDAO();
            }
            return productosDAO;
        }

        private ControlCombustibleDAO getControlCombustibleDAO()
        {
            if (controlCombustibleDAO == null)
            {
                controlCombustibleDAO = new ControlCombustibleDAO();
            }
            return controlCombustibleDAO;
        }

        private TanquesDAO getTanquesDAO()
        {
            if (tanquesDAO == null)
            {
                tanquesDAO = new TanquesDAO();
            }
            return tanquesDAO;
        }

        private VolumenTanqueDAO getVolumenesDAO()
        {
            if(volumenesDAO == null)
            {
                volumenesDAO = new VolumenTanqueDAO();
            }
            return volumenesDAO;
        }

        private CompraCombustibleDAO getComprasDAO()
        {
            if (comprasDAO == null)
            {
                comprasDAO = new CompraCombustibleDAO();
            }
            return comprasDAO;
        }
        #endregion

        #region ventas
        public List<VentaVO> consultarVentasFidelizados(long codEmp, DateTime fecha1, DateTime fecha2, long isla, long turno)
        {
            try
            {
                List<VentaVO> ventas;
                int i = int.Parse(isla.ToString());
                i--;
                string[] islas = Exportador_Ventas_ServP.Properties.Settings.Default.grupoIsla[i].Split(',');
                
                ventas = getVentasDAO().consultarVentasFidelizados(codEmp, fecha1, fecha2, islas, int.Parse(turno.ToString()));
                foreach(VentaVO v in ventas)
                {
                    v.Isla = int.Parse(isla.ToString());
                }
                return ventas;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.",ex);
            }
        }

        public List<VentaVO> consultarVentasNoFidelizados(long codEmp, DateTime fecha1, DateTime fecha2, long isla, long turno)
        {
            try
            {
                List<VentaVO> ventas;
                int i = int.Parse(isla.ToString());
                i--;
                string[] islas = Exportador_Ventas_ServP.Properties.Settings.Default.grupoIsla[i].Split(',');

                ventas = getVentasDAO().consultarVentasNoFidelizados(codEmp, fecha1, fecha2, islas, int.Parse(turno.ToString()));
                foreach (VentaVO v in ventas)
                {
                    v.Isla = int.Parse(isla.ToString());
                }
                return ventas;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public List<VentaVO> consultarVentasAgrupadas(DateTime fecha1, DateTime fecha2)
        {
            try
            {   
                return getVentasDAO().consultarVentasAgrupadas(fecha1, fecha2);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public List<VentaVO> consultarVentasAgrupadas(DateTime fecha1, DateTime fecha2, string nit, string codigo)
        {
            try
            {
                return getVentasDAO().consultarVentasAgrupadas(fecha1, fecha2, nit, codigo);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public List<VentaVO> consultarVentasAgrupadas(DateTime fecha1, DateTime fecha2, string nit, string codigo, long modoPago)
        {
            try
            {
                return getVentasDAO().consultarVentasAgrupadas(fecha1, fecha2, nit, codigo, modoPago);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public List<MovimientoContableDTO> getMovimientosContables(DateTime fecha1, DateTime fecha2, string doc, List<ClienteVO> clientes)
        {
            List<MovimientoContableDTO> movimientos = null;
            movimientos = new List<MovimientoContableDTO>();

            try
            {
                double sobretasaCorriente = 0;
                double sobretasaSuper = 0;
                double sobretasaDiesel = 0;
                double subtotalSobretasaCorriente = 0;
                double subtotalSobretasaSuper = 0;
                double subtotalSobretasaDiesel = 0;
                double totalCreditos = 0;
                double totalDebitos = 0;

                List<VentaVO> ventasCredito = null;
                List<CierreVentasVO> cierresVenta = null;
                List<SobretasaVO> sobretasas = null;
                List<ProductoTurnoVO> productos = null;                
                CierreVentasVO cv = null;

                ventasCredito = getVentasDAO().consultarVentasAgrupadas(fecha1, fecha2);
                cierresVenta = getCierreDAO().consultarCierresAgrupados(fecha1, fecha2);
                sobretasas = getSobreTasasDAO().consultarSobretasas(fecha1.Month, fecha1.Year, fecha1.Day);
                productos = getProductosTurnoDAO().consultarProductosAgrupados(fecha1, fecha2);
                clientes = getClientesDAO().consultarClientes();
                cv = cierresVenta[0];

                #region Movimientos Debito
                foreach (VentaVO vc in ventasCredito)
                {
                    MovimientoContableDTO mc = new MovimientoContableDTO();
                    
                    mc.Fecha = vc.Fecha.ToString("dd/MM/yyyy");
                    mc.TipoDoc = Utilidades.TipoMovimiento;
                    mc.Doc = doc;
                    if (Utilidades.HomologarNits) // Si se trata de una venta fidelizada
                    {
                        try
                        {
                            ClienteVO tmpCliente = null;
                            foreach (ClienteVO cTmp in clientes)
                            {
                                if ((cTmp.Codigo != null) && (cTmp.Codigo.Trim().Equals(vc.Nit.Trim())))
                                {
                                    tmpCliente = cTmp;
                                    break;
                                }
                            }
                            //ClienteVO tmpCliente = getClientesDAO().consultarClienteByCodigo(vc.Nit);
                            if (tmpCliente != null)
                            {
                                mc.Nit = tmpCliente.Identificacion.Trim();
                            }
                            else
                            {
                                NitDTO nitDto = Utilidades.formatearNit(vc.Nit);
                                mc.Nit = nitDto.Nit;
                            }
                        }
                        catch (EstacionDBException ex)
                        {
                            NitDTO nitDto = Utilidades.formatearNit(vc.Nit);
                            mc.Nit = nitDto.Nit;
                        }
                    }
                    else
                    {
                        NitDTO nitDto = Utilidades.formatearNit(vc.Nit);
                        mc.Nit = nitDto.Nit;
                    }

                    if (vc.ModoPago == 2)
                    {
                        mc.Cuenta = Utilidades.CuentaTarjetaPlus;
                    }
                    else
                    {
                        mc.Cuenta = Utilidades.CuentaCredito;
                    }

                    mc.Naturaleza = Utilidades.NatutalezaDebito;
                    mc.Valor = vc.Total.ToString("0.00", CultureInfo.InvariantCulture);
                    mc.CentroCosto = "";
                    mc.CuentaBancaria = "";
                    movimientos.Add(mc);
                }
                                
                // Movimiento contable para Efectivo
                MovimientoContableDTO mc1 = new MovimientoContableDTO();                
                mc1.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc1.TipoDoc = Utilidades.TipoMovimiento;
                mc1.Doc = doc;
                mc1.Nit = "";
                mc1.Nit = Utilidades.NitEDS;
                mc1.Cuenta = Utilidades.CuentaEfectivo;
                mc1.Naturaleza = Utilidades.NatutalezaDebito;
                mc1.Valor = cv.Efectivo.ToString("0.00", CultureInfo.InvariantCulture);
                mc1.CentroCosto = "";
                mc1.CuentaBancaria = "";
                movimientos.Add(mc1);

                // Movimiento contable para Sodexo
                MovimientoContableDTO mc2 = new MovimientoContableDTO();
                mc2.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc2.TipoDoc = Utilidades.TipoMovimiento;
                mc2.Doc = doc;
                mc2.Nit = Utilidades.NitSodexo;
                mc2.Cuenta = Utilidades.CuentaSodexo;
                mc2.Naturaleza = Utilidades.NatutalezaDebito;
                mc2.Valor = cv.Sodexo.ToString("0.00", CultureInfo.InvariantCulture);
                mc2.CentroCosto = "";
                mc2.CuentaBancaria = "";
                movimientos.Add(mc2);

                // Movimiento contable para Big Pass
                MovimientoContableDTO mc3 = new MovimientoContableDTO();
                mc3.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc3.TipoDoc = Utilidades.TipoMovimiento;
                mc3.Doc = doc;
                mc3.Nit = Utilidades.NitBigPass;
                mc3.Cuenta = Utilidades.CuentaBigPass;
                mc3.Naturaleza = Utilidades.NatutalezaDebito;
                mc3.Valor = cv.BigPass.ToString("0.00", CultureInfo.InvariantCulture);
                mc3.CentroCosto = "";
                mc3.CuentaBancaria = "";
                movimientos.Add(mc3);

                // Movimiento contable para Tarjetas de credito
                MovimientoContableDTO mc4 = new MovimientoContableDTO();
                mc4.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc4.TipoDoc = Utilidades.TipoMovimiento;
                mc4.Doc = doc;
                mc4.Nit = "";
                mc4.Nit = Utilidades.NitBanco;
                mc4.Cuenta = Utilidades.CuentaTarjetas;
                mc4.Naturaleza = Utilidades.NatutalezaDebito;
                mc4.Valor = cv.Tarjetas.ToString("0.00", CultureInfo.InvariantCulture);
                mc4.CentroCosto = "";
                mc4.CuentaBancaria = Utilidades.CuentaBancaria;
                movimientos.Add(mc4);

                // Movimiento contable para Tarjeta Plus
                /*MovimientoContableDTO mc5 = new MovimientoContableDTO();
                mc5.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc5.TipoDoc = Utilidades.TipoMovimiento;
                mc5.Doc = doc;
                mc5.Nit = Utilidades.NitEDS;
                mc5.Cuenta = Utilidades.CuentaTarjetaPlus;
                mc5.Naturaleza = Utilidades.NatutalezaDebito;
                mc5.Valor = cv.TarjetaPlus.ToString("0.00", CultureInfo.InvariantCulture);
                mc5.CentroCosto = "";
                mc5.CuentaBancaria = "";
                movimientos.Add(mc5);*/

                // Movimiento contable para Ticket Tronik
                MovimientoContableDTO mc6 = new MovimientoContableDTO();
                mc6.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc6.TipoDoc = Utilidades.TipoMovimiento;
                mc6.Doc = doc;
                mc6.Nit = Utilidades.NitTicketTronik;
                mc6.Cuenta = Utilidades.CuentaTicketTronik;
                mc6.Naturaleza = Utilidades.NatutalezaDebito;
                mc6.Valor = cv.TicketTronik.ToString("0.00", CultureInfo.InvariantCulture);
                mc6.CentroCosto = "";
                mc6.CuentaBancaria = "";
                movimientos.Add(mc6);

                // Movimiento contable para Otros
                MovimientoContableDTO mc7 = new MovimientoContableDTO();
                mc7.Fecha = cv.Fecha.ToString("dd/MM/yyyy");
                mc7.TipoDoc = Utilidades.TipoMovimiento;
                mc7.Doc = doc;
                mc7.Nit = Utilidades.NitEDS;
                mc7.Cuenta = Utilidades.CuentaOtros;
                mc7.Naturaleza = Utilidades.NatutalezaDebito;
                mc7.Valor = cv.Otros.ToString("0.00", CultureInfo.InvariantCulture);
                mc7.CentroCosto = "";
                mc7.CuentaBancaria = "";
                movimientos.Add(mc7);
                #endregion

                #region Movimientos credito

                // Sobretasas
                foreach(SobretasaVO st in sobretasas){
                    switch (st.IdProducto)
                    {
                        case 1: {
                            sobretasaCorriente = st.Sobretasa;
                            break; 
                        }
                        case 2: {
                            sobretasaSuper = st.Sobretasa;
                            break; 
                        }
                        case 3: {
                            sobretasaDiesel = st.Sobretasa;
                            break; 
                        }
                    }
                }

                // Movimientos deSobretasas
                foreach (ProductoTurnoVO pt in productos)
                {

                    if (pt.Producto.Trim().Equals(Exportador_Ventas_ServP.Properties.Settings.Default.NombreCorriente))
                    {
                        MovimientoContableDTO mcs = new MovimientoContableDTO();
                        mcs.Fecha = pt.Fecha.ToString("dd/MM/yyyy");
                        mcs.TipoDoc = Utilidades.TipoMovimiento;
                        mcs.Doc = doc;
                        mcs.Nit = Utilidades.NitEDS;
                        mcs.Naturaleza = Utilidades.NatutalezaCredito;
                        mcs.Cuenta = Utilidades.CuentaSobretasaCorriente;
                        subtotalSobretasaCorriente = pt.Galones * sobretasaCorriente;
                        mcs.Valor = subtotalSobretasaCorriente.ToString("0.00", CultureInfo.InvariantCulture);
                        mcs.CentroCosto = "";
                        mcs.CuentaBancaria = "";
                        movimientos.Add(mcs);

                        MovimientoContableDTO mcn = new MovimientoContableDTO();
                        mcn.Fecha = pt.Fecha.ToString("dd/MM/yyyy");
                        mcn.TipoDoc = Utilidades.TipoMovimiento;
                        mcn.Doc = doc;
                        mcn.Nit = Utilidades.NitEDS;
                        mcn.Naturaleza = Utilidades.NatutalezaCredito;
                        mcn.Cuenta = Utilidades.CuentaVentaCorriente;
                        double diffsub = pt.Valor - subtotalSobretasaCorriente;
                        mcn.Valor = diffsub.ToString("0.00", CultureInfo.InvariantCulture);
                        mcn.CentroCosto = Exportador_Ventas_ServP.Properties.Settings.Default.CentroCostos;
                        mcn.CuentaBancaria = "";
                        movimientos.Add(mcn);
                    }
                    else if (pt.Producto.Trim().Equals(Exportador_Ventas_ServP.Properties.Settings.Default.NombreSuper))
                    {
                        MovimientoContableDTO mcs = new MovimientoContableDTO();
                        mcs.Fecha = pt.Fecha.ToString("dd/MM/yyyy");
                        mcs.TipoDoc = Utilidades.TipoMovimiento;
                        mcs.Doc = doc;
                        mcs.Nit = Utilidades.NitEDS;
                        mcs.Naturaleza = Utilidades.NatutalezaCredito;
                        mcs.Cuenta = Utilidades.CuentaSobretasaSuper;
                        subtotalSobretasaSuper = pt.Galones * sobretasaSuper;
                        mcs.Valor = subtotalSobretasaSuper.ToString("0.00", CultureInfo.InvariantCulture);
                        mcs.CentroCosto = "";
                        mcs.CuentaBancaria = "";
                        movimientos.Add(mcs);

                        MovimientoContableDTO mcn = new MovimientoContableDTO();
                        mcn.Fecha = pt.Fecha.ToString("dd/MM/yyyy");
                        mcn.TipoDoc = Utilidades.TipoMovimiento;
                        mcn.Doc = doc;
                        mcn.Nit = Utilidades.NitEDS;
                        mcn.Naturaleza = Utilidades.NatutalezaCredito;
                        mcn.Cuenta = Utilidades.CuentaVentaSuper;
                        double diffsub1 = pt.Valor - subtotalSobretasaSuper;
                        mcn.Valor = diffsub1.ToString("0.00", CultureInfo.InvariantCulture);
                        mcn.CentroCosto = Exportador_Ventas_ServP.Properties.Settings.Default.CentroCostos;
                        mcn.CuentaBancaria = "";
                        movimientos.Add(mcn);
                    }
                    else if (pt.Producto.Trim().Equals(Exportador_Ventas_ServP.Properties.Settings.Default.NombreDiesel))
                    {
                        MovimientoContableDTO mcs = new MovimientoContableDTO();
                        mcs.Fecha = pt.Fecha.ToString("dd/MM/yyyy");
                        mcs.TipoDoc = Utilidades.TipoMovimiento;
                        mcs.Doc = doc;
                        mcs.Nit = Utilidades.NitEDS;
                        mcs.Naturaleza = Utilidades.NatutalezaCredito;
                        mcs.Cuenta = Utilidades.CuentaSobretasaDiesel;
                        subtotalSobretasaDiesel = pt.Galones * sobretasaDiesel;
                        mcs.Valor = subtotalSobretasaDiesel.ToString("0.00", CultureInfo.InvariantCulture);
                        mcs.CentroCosto = "";
                        mcs.CuentaBancaria = "";
                        movimientos.Add(mcs);

                        MovimientoContableDTO mcn = new MovimientoContableDTO();
                        mcn.Fecha = pt.Fecha.ToString("dd/MM/yyyy");
                        mcn.TipoDoc = Utilidades.TipoMovimiento;
                        mcn.Doc = doc;
                        mcn.Nit = Utilidades.NitEDS;
                        mcn.Naturaleza = Utilidades.NatutalezaCredito;
                        mcn.Cuenta = Utilidades.CuentaVentaDiesel;
                        double diffsub2 = pt.Valor - subtotalSobretasaDiesel;
                        mcn.Valor = diffsub2.ToString("0.00", CultureInfo.InvariantCulture);
                        mcn.CentroCosto = Exportador_Ventas_ServP.Properties.Settings.Default.CentroCostos;
                        mcn.CuentaBancaria = "";
                        movimientos.Add(mcn);
                    }
                }
                #endregion

                #region totales debito/creditos y validacion de ecuacion contable

                foreach (MovimientoContableDTO mcDto in movimientos)
                {
                    if (mcDto.Naturaleza.Equals(Utilidades.NatutalezaCredito))
                    {
                        totalCreditos += double.Parse(mcDto.Valor, CultureInfo.InvariantCulture);
                    }
                    else if (mcDto.Naturaleza.Equals(Utilidades.NatutalezaDebito))
                    {
                        totalDebitos += double.Parse(mcDto.Valor, CultureInfo.InvariantCulture);
                    }
                }

                if (totalCreditos > totalDebitos)
                {
                    double diff1 = totalCreditos - totalDebitos;
                    MovimientoContableDTO mca = new MovimientoContableDTO();
                    mca.Fecha = fecha1.ToString("dd/MM/yyyy");
                    mca.TipoDoc = Utilidades.TipoMovimiento;
                    mca.Doc = doc;
                    mca.Nit = Utilidades.NitEDS;
                    mca.Naturaleza = Utilidades.NatutalezaDebito;
                    mca.Cuenta = Utilidades.CuentaAjuste;
                    mca.Valor = diff1.ToString("0.00", CultureInfo.InvariantCulture);
                    mca.CentroCosto = Exportador_Ventas_ServP.Properties.Settings.Default.CentroCostos;
                    mca.CuentaBancaria = "";
                    movimientos.Add(mca);
                }
                else if (totalDebitos > totalCreditos)
                {
                    double diff = totalDebitos - totalCreditos;
                    MovimientoContableDTO mca = new MovimientoContableDTO();
                    mca.Fecha = fecha1.ToString("dd/MM/yyyy");
                    mca.TipoDoc = Utilidades.TipoMovimiento;
                    mca.Doc = doc;
                    mca.Nit = Utilidades.NitEDS;
                    mca.Naturaleza = Utilidades.NatutalezaCredito;
                    mca.Cuenta = Utilidades.CuentaAjuste;
                    mca.Valor = diff.ToString("0.00", CultureInfo.InvariantCulture);
                    mca.CentroCosto = Exportador_Ventas_ServP.Properties.Settings.Default.CentroCostos;
                    mca.CuentaBancaria = "";
                    movimientos.Add(mca);
                }
                #endregion
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Ha ocurrido un error al consultar los movimientos contables", ex);
            }

            return movimientos;
        }

        public VentaVO consultarVenta(long nroTiquete)
        {
            try
            {
                return getVentasDAO().consultarVentasByTiquete(nroTiquete);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public VentaVO consultarVentaExpo(long nroTiquete)
        {
            try
            {
                return getVentasDAO().consultarVentasByTiqueteExpo(nroTiquete);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public VentaVO consultarVentaTurno(long nroTiquete, DateTime fecha1, DateTime fecha2, int isla, int turno)
        {
            try
            {
                VentaVO venta;
                int i = int.Parse(isla.ToString());
                i--;
                string[] islas = Exportador_Ventas_ServP.Properties.Settings.Default.grupoIsla[i].Split(',');

                venta = getVentasDAO().consultarVentasByTiqueteTurno(nroTiquete,fecha1,fecha2, islas, turno);
                venta.Isla = isla;

                return venta;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public double consultarTotalVentasFidelizadas(long codEmpleado, long turno, long isla, DateTime fecha1, DateTime fecha2)
        {
            try
            {   
                int i = int.Parse(isla.ToString());
                i--;
                string islas = Exportador_Ventas_ServP.Properties.Settings.Default.grupoIsla[i];

                return getVentasDAO().consultarTotalVentasFidelizados(codEmpleado, turno, islas, fecha1, fecha2);                
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public int guardarVentasDia(List<VentaVO> ventas)
        {
            try
            {
                return getVentasDAO().guardarVentas(ventas);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la actualizacion de las ventas en DB app.", ex);
            }
        }

        public int guardarVenta(VentaVO venta)
        {
            try
            {
                return getVentasDAO().guardarVenta(venta);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la actualizacion de las ventas en DB app.", ex);
            }
        }

        public List<VentaTurnoVO> consultarVentasTurno(long codEmp, DateTime fecha1, DateTime fecha2, long isla, long turno)
        {
            try
            {
                List<VentaTurnoVO> ventas;
                int i = int.Parse(isla.ToString());
                i--;
                string[] islas = Exportador_Ventas_ServP.Properties.Settings.Default.grupoIsla[i].Split(',');

                ventas = getVentasDAO().consultarVentasTurno(codEmp, fecha1, fecha2, islas, int.Parse(turno.ToString()));
                foreach (VentaTurnoVO v in ventas)
                {
                    v.Isla = int.Parse(isla.ToString());
                }
                return ventas;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public int guardarVentasTurno(List<VentaTurnoVO> ventas)
        {
            try
            {
                return getVentasDAO().guardarVentasTurno(ventas);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la actualizacion de las ventas en DB app.", ex);
            }
        }

        public List<ClienteVO> consultarClientesVentas()
        {
            try
            {
                return getVentasDAO().consultarClientesVentas();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la actualizacion de las ventas en DB app.", ex);
            }
        }

        public List<VentaDTO> consultarVentaCliente(DateTime fecha1, DateTime fecha2, string nit, string codigo)
        {
            try
            {
                List<VentaDTO> result = new List<VentaDTO>();
                List<VentaVO> list = getVentasDAO().consultarVentasCliente(fecha1, fecha2, nit, codigo);
                foreach (VentaVO v in list)
                {
                    VentaDTO dto = new VentaDTO();
                    dto.Consecutivo = v.Tiquete;
                    dto.Fecha = v.Fecha;
                    dto.Kilometraje = v.Kilometraje;
                    dto.Placa = v.Placa;
                    dto.Producto = v.Producto.Trim();
                    dto.Total = v.Total;

                    switch (v.ModoPago)
                    {
                        case 1: {
                            dto.ModoPago = "Cr�dito sin chip";
                            break; }
                        case 2: {
                            dto.ModoPago = "Tarjeta Plus";
                            break; }
                        case 7: {
                            dto.ModoPago = "Cr�dito con chip";
                            break; }
                    }

                    result.Add(dto);
                }
                return result;
            }
            catch (EstacionDBException)
            {
                throw new PersistenciaException("Error al consultar las ventas para el cliente y fechas seleccionados");
            }
        }
        #endregion

        #region lecturas
        public List<LecturaVO> consultarLecturas(DateTime fecha1, DateTime fecha2)
        {
            try
            {
                return getLecturasDAO().consultarLecturas(fecha1, fecha2);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta lecturas en DB estaci�n.", ex);
            }
        }

        public double consultarVentaProducto(DateTime fecha1, DateTime fecha2, int idProdcuto)
        {
            try
            {
                double diferencia = 0;
                int prod = 0;
                switch (idProdcuto)
                {
                    case 1: {
                        prod = Utilidades.codigoCorriente;
                        break; }
                    case 2: {
                        prod = Utilidades.codigoSuper;
                        break; }
                    case 3: {
                        prod = Utilidades.codigoDiesel;
                        break; }
                }
                diferencia = getLecturasDAO().consultarDiferenciaProducto(fecha1, fecha2, prod);
                diferencia = Math.Round(diferencia, 0, MidpointRounding.AwayFromZero);
                
                return diferencia;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta lecturas en DB estaci�n.", ex);
            }
        }

        public LecturaDTO consultarNivel(int idProducto, int idTanque, double nivel)
        {
            LecturaDTO lectura = null;
            nivel = Utilidades.redondear(nivel);
            VolumenTanqueVO vt = getVolumenesDAO().consultarVolumen(idTanque, idProducto, nivel);
            if (vt != null)
            {
                lectura = new LecturaDTO();
                lectura.Galones = Math.Round(vt.Galones, 0, MidpointRounding.AwayFromZero);
                lectura.IdProducto = vt.IdProducto;
                lectura.IdTanque = vt.IdTanque;                
            }
            return lectura;
        }

        public double consultarLecturaFinalProducto(DateTime fecha1, DateTime fecha2, int idProdcuto)
        {
            try
            {
                double diferencia = 0;
                double resultado = 0;
                int prod = 0;
                switch (idProdcuto)
                {
                    case 1:
                        {
                            prod = Utilidades.codigoCorriente;
                            break;
                        }
                    case 2:
                        {
                            prod = Utilidades.codigoSuper;
                            break;
                        }
                    case 3:
                        {
                            prod = Utilidades.codigoDiesel;
                            break;
                        }
                }
                diferencia = getLecturasDAO().consultarDiferenciaProducto(fecha1, fecha2, prod);
                diferencia = Math.Round(diferencia, 0, MidpointRounding.AwayFromZero);

                resultado = this.consultarSaldoAnterior(idProdcuto, fecha1, fecha2);
                resultado = resultado - diferencia;

                return resultado;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta lecturas en DB estaci�n.", ex);
            }
        }
        #endregion

        #region clientes
        public List<ClienteVO> consultarClientes()
        {
            try
            {
                return getClientesDAO().consultarClientes();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta clientes en DB app.", ex);
            }
        }

        public List<ClienteVO> consultarClientesPorCampo(string valor, int campo)
        {
            try
            {
                return getClientesDAO().consultarClientes(valor, campo);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta clientes en DB app.", ex);
            }
        }

        public List<ClienteVO> consultarClientesServP()
        {
            try
            {
                return getClientesDAO().consultarClientesServP();
            }
            catch(EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta clientes en DB Servipunto.", ex);
            }
        }

        public ClienteVO consultarCliente(long idCliente)
        {
            try
            {
                return getClientesDAO().consultarClientesById(idCliente);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta clientes en DB app.", ex);
            }
        }

        public bool guardarCliente(ClienteVO cliente)
        {
            bool result = false;
            try
            {
                int rows = getClientesDAO().guardarCliente(cliente);
                if (rows > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la actualizacion de cliente en DB app.", ex);
            }

        }
        #endregion

        #region tipos
        public List<TipoIdVO> consultarTipos()
        {
            try
            {
                return getTiposDAO().consultarTipos();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta tipos id en DB app.", ex);
            }
        }
        #endregion

        #region bancos
        public List<BancoVO> consultarBancos()
        {
            try
            {
                return getBancosDAO().consultarBancos();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta bancos en DB app.", ex);
            }
        }
        #endregion

        #region ingresos / egresos
        public int guardarIngresos(List<IngresoVO> ingresos, DateTime fecha)
        {
            try
            {
                List<IngresoVO> tmp = getIngresosDAO().consultarIngresosFecha(fecha, fecha.AddDays(1));
                if (tmp.Count > 0)
                {
                    throw new CierreException("Ya se ha realizado cierre para la fecha seleccionada");
                }
                return getIngresosDAO().guardarIngresos(ingresos, fecha);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("No se pudo realizar el cierre de ingresos para la fecha seleccionada", ex);
            }
        }

        public int guardarEgresos(List<EgresoVO> egresos, DateTime fecha, DateTime fechaAplica)
        {
            try
            {   
                return getEgresosDAO().guardarEgresos(egresos, fecha, fechaAplica);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("No se pudo realizar el cierre de egresos para la fecha seleccionada", ex);
            }
        }

        public double consultarTotalEfectivoFecha(DateTime fecha1, DateTime fecha2)
        {
            double total = 0;
            IList cierres = null;

            cierres = getCierreDAO().consultarCierres(fecha1, fecha2);


            if (cierres != null)
            {
                foreach (CierreVentasVO cierre in cierres)
                {
                    total += cierre.Efectivo;
                }
            }

            return total;
        }

        public double consultarTotalEgresosAplicadosFecha(DateTime fecha1, DateTime fecha2)
        {
            double total = 0;
            List<EgresoVO> egresos = null;

            egresos = getEgresosDAO().consultarEgresosAplicadosFecha(fecha1, fecha2);


            if (egresos != null)
            {
                foreach (EgresoVO egreso in egresos)
                {
                    total += egreso.Valor;
                }
            }

            return total;
        }
        #endregion

        #region Cierres
        public int existeCierre(long codEmpleado, long turno, long isla, DateTime fecha1, DateTime fecha2)
        {
            try
            {
                return getCierreDAO().consultarCuentaCierres(codEmpleado, turno, isla, fecha1, fecha2);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta cierres en DB Expo.", ex);
            }
        }

        public int guardarCierre(CierreVentasVO cierre)
        {
            try
            {
                return getCierreDAO().guardarCierre(cierre);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("No se pudo realizar el cierre de ventas para la fecha seleccionada", ex);
            }
        }

        public int guardarCierres(List<CierreVentasVO> cierre)
        {
            try
            {
                return getCierreDAO().guardarCierres(cierre);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("No se pudo realizar el cierre de ventas para la fecha seleccionada", ex);
            }
        }

        public CierreInfoDTO getCierreInfo(DateTime fecha1, DateTime fecha2)
        {
            CierreInfoDTO cierre = new CierreInfoDTO();
            try
            {
                cierre.CierreDia = getCierreDAO().consultarCierresAgrupados(fecha1, fecha2)[0];
                cierre.CierresTurnos = getCierreDAO().consultarCierres(fecha1, fecha2);
                cierre.TotalConsumo = getProductosTurnoDAO().consultarTotalConsumo(fecha1, fecha2);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar la informaci�n de cierre", ex);
            }
            catch (NullReferenceException nre)
            {
                throw new PersistenciaException("No hay Informaci�n de cierre para la fecha seleccionada");
            }
            return cierre;
        }
        #endregion

        #region empleados
        public IList getEmpleados()
        {
            try
            {
                return getEmpleadoDAO().consultarEmpleados();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta de empleados", ex);
            }
        }
        #endregion

        #region productos turno
        public List<ProductoTurnoVO> consultarProductosTurno(DateTime fecha1, DateTime fecha2, long isla, long turno)
        {
            try
            {
                List<ProductoTurnoVO> ventas;
                int i = int.Parse(isla.ToString());
                i--;
                string islas = Exportador_Ventas_ServP.Properties.Settings.Default.grupoIsla[i];

                ventas = getProductosTurnoDAO().consultarProductosTurno(islas, turno, fecha1, fecha2);
                foreach (ProductoTurnoVO v in ventas)
                {
                    v.Isla = isla;
                }
                return ventas;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la consulta ventas en DB estaci�n.", ex);
            }
        }

        public int guardarProductosTurno(List<ProductoTurnoVO> productosTurno)
        {
            try
            {
                return getProductosTurnoDAO().guardarProductosTurno(productosTurno);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error en la actualizacion de los productos del turno en DB app.", ex);
            }
        }
        #endregion

        #region Control combustible
        public int procesarControles(DateTime fecha, LecturaDTO[] lecturas)
        {
            int rows = 0;
            bool isCorriente = false;
            bool isSuper = false;
            bool isDiesel = false;
            ControlCombustibleVO controlCorriente = new ControlCombustibleVO();
            ControlCombustibleVO controlSuper = new ControlCombustibleVO();
            ControlCombustibleVO controlDiesel = new ControlCombustibleVO();

            try
            {

                for (int i = 0; i < lecturas.Length; i++)
                {
                    int idProd = lecturas[i].IdProducto;
                    switch (idProd)
                    {
                        //Corriente
                        case 1:
                            {
                                controlCorriente.InventarioFinal += (int)lecturas[i].Galones;
                                isCorriente = true;
                                break;
                            }
                        //Super
                        case 2:
                            {
                                controlSuper.InventarioFinal += (int)lecturas[i].Galones;
                                isSuper = true;
                                break;
                            }
                        //Diesel
                        case 3:
                            {
                                controlDiesel.InventarioFinal += (int)lecturas[i].Galones;
                                isDiesel = true;
                                break;
                            }
                    }
                }
                DateTime diaAnterior = fecha.AddDays(-1);
                //Corriente
                if (isCorriente)
                {
                    controlCorriente.IdProducto = 1;
                    controlCorriente.Fecha = fecha;
                    controlCorriente.GalonesCompra = (int)this.consultarComprasGalonesFechaProducto(1, fecha);
                    controlCorriente.InventarioInicial = (int)this.consultarSaldoAnterior(1, diaAnterior, diaAnterior);
                    controlCorriente.VentaMedida = (controlCorriente.GalonesCompra + controlCorriente.InventarioInicial) - controlCorriente.InventarioFinal;
                    controlCorriente.VentaSurtidor = (int)this.consultarVentaProducto(fecha, fecha, 1);
                    controlCorriente.SobranteDia = controlCorriente.VentaSurtidor - controlCorriente.VentaMedida;
                    controlCorriente.SobranteAcumulado = controlCorriente.SobranteDia + (int)this.consultarAcumuladoAnterior(1, diaAnterior, diaAnterior);
                    controlCorriente.Procentaje = (double)(((double)controlCorriente.SobranteDia / (double)(controlCorriente.GalonesCompra + controlCorriente.InventarioInicial)) * 100);

                    rows += this.guardarControlCombustible(controlCorriente);
                }

                //Super
                if (isSuper)
                {
                    controlSuper.IdProducto = 2;
                    controlSuper.Fecha = fecha;
                    controlSuper.GalonesCompra = (int)this.consultarComprasGalonesFechaProducto(2, fecha);
                    controlSuper.InventarioInicial = (int)this.consultarSaldoAnterior(2, diaAnterior, diaAnterior);
                    controlSuper.VentaMedida = (controlSuper.GalonesCompra + controlSuper.InventarioInicial) - controlSuper.InventarioFinal;
                    controlSuper.VentaSurtidor = (int)this.consultarVentaProducto(fecha, fecha, 2);
                    controlSuper.SobranteDia = controlSuper.VentaSurtidor - controlSuper.VentaMedida;
                    controlSuper.SobranteAcumulado = controlSuper.SobranteDia + (int)this.consultarAcumuladoAnterior(2, diaAnterior, diaAnterior);
                    controlSuper.Procentaje = (double)(((double)controlSuper.SobranteDia / (double)(controlSuper.GalonesCompra + controlSuper.InventarioInicial)) * 100);

                    rows += this.guardarControlCombustible(controlSuper);
                }

                //Diesel
                if (isDiesel)
                {
                    controlDiesel.IdProducto = 3;
                    controlDiesel.Fecha = fecha;
                    controlDiesel.GalonesCompra = (int)this.consultarComprasGalonesFechaProducto(3, fecha);
                    controlDiesel.InventarioInicial = (int)this.consultarSaldoAnterior(3, diaAnterior, diaAnterior);
                    controlDiesel.VentaMedida = (controlDiesel.GalonesCompra + controlDiesel.InventarioInicial) - controlDiesel.InventarioFinal;
                    controlDiesel.VentaSurtidor = (int)this.consultarVentaProducto(fecha, fecha, 3);
                    controlDiesel.SobranteDia = controlDiesel.VentaSurtidor - controlDiesel.VentaMedida;
                    controlDiesel.SobranteAcumulado = controlDiesel.SobranteDia + (int)this.consultarAcumuladoAnterior(3, diaAnterior, diaAnterior);
                    controlDiesel.Procentaje = (double)(((double)controlDiesel.SobranteDia / (double)(controlDiesel.GalonesCompra + controlDiesel.InventarioInicial)) * 100);

                    rows += this.guardarControlCombustible(controlDiesel);
                }
            }
            catch (PersistenciaException ex)
            {
                throw new PersistenciaException("Error al procesar las lecturas", ex);
            }
            return rows;
        }

        public int guardarControlCombustible(ControlCombustibleVO cc)
        {
            try
            {
                return getControlCombustibleDAO().guardarControlCombustible(cc);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al guardar el control de combustible", ex);
            }
        }

        public List<ControlCombustibleVO> consultarControlProductoFecha(int idProducto, DateTime fechadesde, DateTime fechaHasta)
        {
            try
            {
                return getControlCombustibleDAO().consultarControles(idProducto, fechadesde, fechaHasta);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar el control de combustible por fecha y producto", ex);
            }
        }

        private double consultarSaldoAnterior(int idProducto, DateTime fechadesde, DateTime fechaHasta)
        {
            try
            {
                double saldo = 0;
                List<ControlCombustibleVO> lista = getControlCombustibleDAO().consultarControles(idProducto, fechadesde, fechaHasta);
                
                if (lista != null && lista.Count > 0)
                {
                    ControlCombustibleVO control = lista[0];
                    saldo = control.InventarioFinal;
                }
                return saldo;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar el control de combustible por fecha y producto", ex);
            }
        }

        private double consultarAcumuladoAnterior(int idProducto, DateTime fechadesde, DateTime fechaHasta)
        {
            try
            {
                double acumulado = 0;
                if (fechadesde.Day > 1)
                {
                    List<ControlCombustibleVO> lista = getControlCombustibleDAO().consultarControles(idProducto, fechadesde, fechaHasta);

                    if (lista != null && lista.Count > 0)
                    {
                        ControlCombustibleVO control = lista[0];
                        acumulado = control.SobranteAcumulado;
                    }
                }
                return acumulado;
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar el control de combustible por fecha y producto", ex);
            }
        }

        public int guardarCompraCombustible(CompraCombustibleVO c)
        {
            try
            {
                return getComprasDAO().guardar(c);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al guardar la compra de combustible", ex);
            }
        }

        public List<CompraCombustibleVO> consultarComprasFecha(DateTime fecha)
        {
            try
            {
                return getComprasDAO().consultarComprasFecha(fecha, fecha);
            }
            catch (EstacionDBException ex)
            {
                throw new EstacionDBException("Error al consultar las compras de combustible", ex);
            }
        }

        private double consultarComprasGalonesFechaProducto(long idProducto, DateTime fecha)
        {
            try
            {
                return getComprasDAO().consultarComprasGalonesFechaProducto(idProducto, fecha, fecha);
            }
            catch (EstacionDBException ex)
            {
                throw new EstacionDBException("Error al consultar las compras de combustible", ex);
            }
        }
        #endregion

        #region Tanques / volumenes / medidas
        public List<TanqueVO> consultarTanques()
        {
            try
            {
                return getTanquesDAO().consultarTanques();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar los tanques", ex);
            }
        }

        public int guardarTanque(TanqueVO t)
        {
            try
            {
                return getTanquesDAO().guardarTanque(t);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al guardar el tanque" , ex);
            }
        }

        public int guardarVolumen(VolumenTanqueVO v)
        {
            try{
                return getVolumenesDAO().guardarVolumenTanque(v);
            }catch(EstacionDBException ex){
                throw new PersistenciaException("Error al guardar el volumen", ex);
            }
        }

        public VolumenTanqueVO consultarVolumen(int idTanque, int idProducto, double cms)
        {
            try
            {
                return getVolumenesDAO().consultarVolumen(idTanque, idProducto, cms);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar el volumen", ex);
            }
        }

        
        #endregion
        #region productos
        public List<ProductoVO> consultarProductos()
        {
            try
            {
                return getProductosDAO().consultarProductos();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar los productos", ex);
            }
        }
        #endregion

        #region sobretasas
        public List<SobretasaVO> consultarSobretasas()
        {
            try
            {
                return getSobreTasasDAO().consultarSobretasas();
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar las sobretasas", ex);
            }
        }

        public SobretasaVO consultarSobretasasProductoFecha(int mes, int anio, int idProducto, int dia)
        {
            try
            {
                return getSobreTasasDAO().consultarSobretasaProducto(mes, anio, idProducto, dia);
            }
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al consultar las sobretasas", ex);
            }
        }

        public bool guardarSobertasa(SobretasaVO sobretasa)
        {
            bool resultado = false;
            int rows = 0;
            try
            {
                rows = getSobreTasasDAO().guardarSobretasa(sobretasa);
                if (rows > 0)
                {
                    resultado = true;
                }
                return resultado;
            }  
            catch (EstacionDBException ex)
            {
                throw new PersistenciaException("Error al guardar la sobretasa", ex);
            }
        }
        #endregion
    }
}
