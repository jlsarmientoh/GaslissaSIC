using System;
using System.Collections.Generic;
using System.Text;
using EstacionDB.DAO;
using EstacionDB.Exceptions;
using EstacionDB.DTO;
using EstacionDB.VO;
using System.Collections;

namespace EstacionDB.Core
{
    public class DisposicionEfectivoCore
    {
        //Atributos
        private volatile static DisposicionEfectivoCore instance;
        private EgresosDAO egresosDAO;
        private CierreVentasDAO cierresDAO;
        //Constructor
        private DisposicionEfectivoCore()
        {
            egresosDAO = new EgresosDAO();
            cierresDAO = new CierreVentasDAO();
        }
        //M�todos
        /// <summary>
        /// Obtiene la instancia de DisposicionEfectivoCore
        /// </summary>
        /// <returns>Instancia de DisposicionEfectivoCore</returns>
        public static DisposicionEfectivoCore getInstance()
        {
            if (instance == null)
            {
                instance = new DisposicionEfectivoCore();
            }
            return instance;
        }
        /// <summary>
        /// <para>Retorna el total del efectivo a disponer para la fecha indicada.</para>
        /// <para>CierreException.  Si la operaci�n falla.</para>
        /// </summary>
        /// <param name="fecha">DateTime, fecha del cierre.</param>
        /// <returns>El total del efectivo a disponer.</returns>
        public double getTotalEfectivoFecha(DateTime fecha)
        {
            try
            {
                return cierresDAO.consultarTotalMedioPago("Efectivo", fecha, fecha);
            }
            catch (EstacionDBException ex)
            {
                throw new CierreException("No se pudo obtener la informaci�n del efectivo a disponer", ex);
            }
        }
        /// <summary>
        /// <para>Retorna el total del efectivo dispuesto para la fecha indicada.</para>
        /// <para>CierreException.  Si la operaci�n falla.</para>
        /// </summary>
        /// <param name="fecha">DateTime,  fecha a la que se aplicaron lo egresos.</param>
        /// <returns>El total de efectivo dispuesto.</returns>
        public double getTotalEfectivoDispuestoFecha(DateTime fecha)
        {
            try
            {
                return egresosDAO.consultarTotalEgresosAplicados(fecha, fecha);
            }
            catch (EstacionDBException ex)
            {
                throw new CierreException("No se pudo obtener informaci�n del efectivo dispuesto", ex);
            }
        }
        /// <summary>
        /// <para>Obtien la lista de egresos aplicados en la fecha indicada.</para>
        /// <para>CierreException.  Si la operaci�n falla.</para>
        /// </summary>
        /// <param name="fecha">DateTime,  fecha a la que se aplicaron lo egresos.</param>
        /// <returns>Lista de DTO con la informaci�n de los egresos.</returns>
        public IList<EgresoDTO> consultarEgresosAplicados(DateTime fecha)
        {
            try
            {
                IList<EgresoDTO> dtos = new List<EgresoDTO>();
                IList vos = egresosDAO.consultarEgresosAplicados(fecha, fecha);
                
                foreach (EgresoVO vo in vos)
                {
                    EgresoDTO dto = new EgresoDTO();
                    dto.IdEgreso = vo.IdEgreso;
                    dto.TipoDocumento = vo.Documento;
                    dto.Numero = vo.Numero;
                    dto.Beneficiario = vo.Beneficiario;
                    dto.Valor = vo.Valor;
                    dto.Fecha = vo.Fecha;
                    dto.FechaAplica = vo.FechaAplica;

                    dtos.Add(dto);
                }

                return dtos;
            }
            catch (EstacionDBException ex)
            {
                throw new CierreException("No se pudo obtener los egresos aplicados a la fecha indicada", ex);
            }
        }

        /// <summary>
        /// <para>Elimina el egreso que recibe como par�metros.</para>
        /// </summary>
        /// <param name="egreso">EgrestoDTO debe contener al menos el campo IdEgreso.</param>
        /// <returns>int, n�mero de egreso eliminados.</returns>
        public int eliminarEgreso(EgresoDTO egreso)
        {
            try
            {
                EgresoVO e = new EgresoVO();
                e.Beneficiario = egreso.Beneficiario;
                e.Documento = egreso.TipoDocumento;
                e.Fecha = egreso.Fecha;
                e.FechaAplica = egreso.FechaAplica;
                e.IdEgreso = egreso.IdEgreso;
                e.Isla = 0;
                e.Numero = egreso.Numero;
                e.Valor = egreso.Valor;

                return egresosDAO.eliminarEgreso(e);
            }
            catch (EstacionDBException ex)
            {
                throw new CierreException("No se pudo eliminar el egreso", ex);
            }
        }

        /// <summary>
        /// Guarda o actualiza los agresos que recibe como par�metro.
        /// </summary>
        /// <param name="egresos">IList lista de egresos.</param>
        /// <returns>int, n�mero de egresos guardados o actualizados</returns>
        public int guardarEgresos(IList egresos)
        {
            try
            {
                IList<EgresoVO> vos = new List<EgresoVO>();
                foreach (EgresoDTO dto in egresos)
                {
                    EgresoVO e = new EgresoVO();
                    e.Beneficiario = dto.Beneficiario;
                    e.Documento = dto.TipoDocumento;
                    e.Fecha = dto.Fecha;
                    e.FechaAplica = dto.FechaAplica;
                    e.IdEgreso = dto.IdEgreso;
                    e.Isla = 0;
                    e.Numero = dto.Numero;
                    e.Valor = dto.Valor;

                    vos.Add(e);
                }

                return egresosDAO.guardarEgresos(vos);
            }
            catch (EstacionDBException ex)
            {
                throw new CierreException("No se pudo acutalizar los egresos", ex);
            }
        }
        /// <summary>
        /// <para>Compara la informaci�n del egreso contenida en el DTO con la informaci�n
        /// registrada.  En caso de inconsistencia lanza una excepci�n de tipo
        /// CierreException con la respectiva descripci�n.</para>
        /// </summary>
        /// <param name="dto">EgresoDTO DTO con los datos a validar</param>
        public void validarEgreso(EgresoDTO dto)
        {
            EgresoVO vo = egresosDAO.consultarEgreso(dto.Numero);

            if (vo == null)
            {
                throw new CierreException("Egreso # " + dto.Numero + ".  No existe");
            }

            if (vo.Valor.CompareTo(dto.Valor) != 0)
            {
                throw new CierreException("Egreso # " + dto.Numero + ".  Diferencia de valor: [Registrado : "
                    + Utilidades.Utilidades.formatearDecimal(vo.Valor) + "][Archivo : "
                    + Utilidades.Utilidades.formatearDecimal(dto.Valor) + "]");
            }
            DateTime fechaVo = DateTime.Parse(vo.FechaAplica.ToString("yyyy/MM/dd"));
            DateTime fechaDto = DateTime.Parse(dto.FechaAplica.ToString("yyyy/MM/dd"));

            if (fechaVo.CompareTo(fechaDto) != 0)
            {
                throw new CierreException("Egreso # " + dto.Numero + ".  Fechas no coninciden: [Registrado : ]"
                    + fechaVo.ToShortDateString() + "][Archivo : "
                    + fechaDto.ToShortDateString() + "]");
            }
        }
    }
}
