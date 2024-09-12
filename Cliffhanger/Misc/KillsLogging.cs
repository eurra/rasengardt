using System;
using System.IO;
using Server;
using Server.Accounting;

namespace Server.Misc
{
	public class KillsLogging
	{
		private static StreamWriter m_Output;
		private static bool m_Enabled = true;

		public static bool Enabled{ get{ return m_Enabled; } set{ m_Enabled = value; } }

		public static StreamWriter Output{ get{ return m_Output; } }

		public static string BuildText( Mobile killer, Mobile victim )
		{
			return String.Format( "\"{0}\" ('{1}') mata a \"{2}\" ('{3}').", killer.Name == null ? "Sin Nombre" : killer.Name, killer.Account == null ? "Sin Cuenta" : ((Account)killer.Account).Username, victim.Name == null ? "(Sin Nombre)" : victim.Name, victim.Account == null ? "(Sin Cuenta)" : ((Account)victim.Account).Username );
		}

		public static void WriteLine( Mobile killer, Mobile victim )
		{
			if ( !m_Enabled )
				return;

			try
			{
				string path = Core.BaseDirectory;

				AppendPath( ref path, "Logs" );
				AppendPath( ref path, "Kills" );
				AppendPath( ref path, DateTime.Now.Year.ToString() );
				AppendPath( ref path, ObtenerMes( DateTime.Now.Month ) );
				path = Path.Combine( path, String.Format( "{0}-{1}-{2}.log", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year ) );

				DateTime now = DateTime.Now;

				using ( StreamWriter sw = new StreamWriter( path, true ) )
					sw.WriteLine( "{0}, {1}", now.Hour + ":" + now.Minute + ":" + now.Second, BuildText( killer, victim ) );
			}
			catch
			{
			}
		}
		
		private static string ObtenerMes( int mes )
		{
			switch( mes )
			{
				case 1: return "Enero";
				case 2: return "Febrero";
				case 3: return "Marzo";
				case 4: return "Abril";
				case 5: return "Mayo";
				case 6: return "Junio";
				case 7: return "Julio";
				case 8: return "Agosto";
				case 9: return "Septiembre";
				case 10: return "Octubre";
				case 11: return "Noviembre";
				case 12: return "Diciembre";
				default : return "N/A";
			}
		}

		public static void AppendPath( ref string path, string toAppend )
		{
			path = Path.Combine( path, toAppend );

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		public static void LogKill( Mobile from, Mobile to )
		{
			WriteLine( from, to );
		}
	}
}
