using System;
using System.Collections.Generic;
using System.Text;
using EstacionDB.DTO;

namespace EstacionDB.Utilidades
{
    public static class Utilidades
    {
        public static string cadenaConexion;
        public static string appCadenaConexion;
        public static string configServ;
        public static string configExpo;
        public static string nombreVistaVentas;
        public static string nombreVistaLecturas;
        public static string nombreTablaEmpleados;
        public static string rutaPrincipalExport;        
        public static string separador;
        public static bool usarEncabezados;        
        public static int codigoSuper;
        public static int codigoCorriente;
        public static int codigoDiesel;
        public static string grupo1;
        public static string grupo2;
        public static int corrienteMultiplicarX;
        public static int superMultiplicarX;
        public static int dieselMultiplicarX;
        public static string TipoMovimiento;
        public static string NatutalezaDebito;
        public static string NatutalezaCredito;
        public static string NitEDS;
        public static string NitSodexo;
        public static string NitBigPass;
        public static string NitTicketTronik;
        public static string CuentaCredito;
        public static string CuentaEfectivo;
        public static string CuentaSodexo;
        public static string CuentaBigPass;
        public static string CuentaTarjetas;
        public static string CuentaOtros;
        public static string CuentaTarjetaPlus;
        public static string CuentaTicketTronik;
        public static string CuentaSobretasaCorriente;
        public static string CuentaSobretasaSuper;
        public static string CuentaSobretasaDiesel;
        public static string CuentaVentaCorriente;
        public static string CuentaVentaSuper;
        public static string CuentaVentaDiesel;
        public static string CuentaAjuste;
        public static Boolean HomologarNits;
        public static string CuentaBancaria;
        public static string NitBanco;
        public static UsuarioDTO usuarioSesion;
        // constantes
        public const int C_CLI_NIT = 1;
        public const int C_CLI_CODIGO = 2;
        public const int C_CLI_NOMBRE = 3;

        public static NitDTO formatearNit(String nit)
        {
            NitDTO resultado = new NitDTO();
            string numericalNit;

            StringBuilder origin = new StringBuilder();
            char[] charArray = nit.ToCharArray();

            // se tienen en cuenta solo los digitos num�ricos
            foreach(char c in charArray)
            {
                int flag;
                if (!c.Equals('-') && !c.Equals(',') && !c.Equals('.') && !c.Equals('#') && !c.Equals('/') && !c.Equals('*') && !c.Equals('&') && !c.Equals('%') && !c.Equals('$'))
                {
                    origin.Append(c);
                }
            }
            
            numericalNit = origin.ToString();

            if (numericalNit.Length <= 9)
            {
                resultado.Nit = numericalNit;
                resultado.DigitoVerfificacion = "0";
            }

            if (numericalNit.Length > 9) //Cuando se tiene el nit y el digito de verificacion en el mismo campo
            {
                resultado.Nit = numericalNit.Substring(0, 9);
                resultado.DigitoVerfificacion = numericalNit.Substring(9, 1);
            }
            if (numericalNit.Length > 10) //Cuando ademas se tienen consecutivos adicionales
            {
                resultado.Nit = numericalNit.Substring(0, 9);
                resultado.DigitoVerfificacion = numericalNit.Substring(9, 1);
            }
            //Se guarda el nit original de servP en el campo codigo para homologar con Expo
            resultado.Codigo = numericalNit;

            return resultado;
        }

        public static double redondear(double a)
        {
            double b = 0;
            int e = (int)a;
            double c = a - e;
            if (c < 0.5)
            {
                b = a - c;
            }
            if (c == 0.5)
            {
                b = a;
            }
            if (c > 0.5)
            {
                b = e + 0.5;
            }
            return b;
        }

        public static double redondearPosicionesDecimales(double a)
        {
            double b = 0;
            decimal valorRedondeado = 0;

            try
            {
                valorRedondeado = Math.Round(Convert.ToDecimal(a), 2);

                b = Convert.ToDouble(valorRedondeado);
            }
            catch (OverflowException ex)
            {
                //TODO meter algun mensaje de error
            }
            return b;
        }

        public static string formatearDecimal(double valor)
        {
            return String.Format("{0,10:#,0.00}", valor);
        }

        public static double parsearDecimal(string valor)
        {
            string formateado = valor.Replace(".", ",");
            return double.Parse(formateado);
        }

        public static double parsearDecimal(string valor, char separadorMil)
        {
            char[] charArray = valor.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach(char c in charArray){
                if (c != separadorMil)
                {
                    sb.Append(c);
                }
            }
            //string formateado = parsearDecimal(sb.ToString());
            return parsearDecimal(sb.ToString());
        }
    }

}
