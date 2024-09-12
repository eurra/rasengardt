// Sistema de Jaileo.
// Hecho por CLiffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using System.IO;
using System.Xml;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;

namespace Server.Jailing
{
 	public class JailSystem
 	{
		private static Hashtable m_Jailings = new Hashtable();
 		
 		public static Hashtable Table
		{
			get
			{
				return m_Jailings;
			}
		}

		public static JailInfo GetJailing( Account acc )
		{
			return m_Jailings[acc] as JailInfo;
		}

		public static void Initialize()
		{
			Server.Commands.Register( "JailSys", AccessLevel.Counselor, new CommandEventHandler( Jail_OnCommand ) );
			Server.Commands.Register( "JailInfo", AccessLevel.Player, new CommandEventHandler( JailInfo_OnCommand ) );
			EventSink.Login += new LoginEventHandler( OnLogin );
		}
		
		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler( Load );
			EventSink.WorldSave += new WorldSaveEventHandler( Save );
		}

                [Usage( "JailSystem" )]
		[Description( "Abre el menu del sistema de Jail" )]
		private static void Jail_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendGump( new JailGump( e.Mobile, Pages.PagePrincipal ) );
		}
		
		[Usage( "JailInfo" )]
		[Description( "Abre el menu del sistema de Jail" )]
		private static void JailInfo_OnCommand( CommandEventArgs e )
		{
			Account acc = e.Mobile.Account as Account;
			
			if( acc == null )
			{
				return;
			}
			else
			{
                        	JailInfo info = GetJailing( acc );
                        	
                        	if( info == null )
                        	{
                                	e.Mobile.SendMessage( "No hay registros de algún encarcelamiento tuyo." );
                                	return;
                                }
                                else
                                {
                                	if( info.Duration == TimeSpan.MaxValue )
                                        	e.Mobile.SendMessage( "Tu condena esta siendo configurada..." );
					else if( !info.Active )
                                        	e.Mobile.SendMessage( "Tu condena ya esta cumplida, para salir de la carcel debes reconectarte." );

					e.Mobile.SendGump( new JailGump( e.Mobile, Pages.PageInfo, info ) );
                                }
                        }
		}

		public static void Save( WorldSaveEventArgs e )
		{
			if ( !Directory.Exists( "Saves/Jailings" ) )
				Directory.CreateDirectory( "Saves/Jailings" );

			string filePath = Path.Combine( "Saves/Jailings", "jailings.xml" );

			using ( StreamWriter op = new StreamWriter( filePath ) )
			{
				XmlTextWriter xml = new XmlTextWriter( op );

				xml.Formatting = Formatting.Indented;
				xml.IndentChar = '\t';
				xml.Indentation = 1;

				xml.WriteStartDocument( true );

				xml.WriteStartElement( "jailings" );

				xml.WriteAttributeString( "count", m_Jailings.Count.ToString() );

				foreach ( JailInfo i in JailSystem.Table.Values )
					i.Save( xml );

				xml.WriteEndElement();

				xml.Close();
			}
		}

		public static void Load()
		{
			m_Jailings = new Hashtable( 32, 1.0f, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default );

			string filePath = Path.Combine( "Saves/Jailings", "jailings.xml" );

			if ( !File.Exists( filePath ) )
				return;

			XmlDocument doc = new XmlDocument();
			doc.Load( filePath );

			XmlElement root = doc["jailings"];

			foreach ( XmlElement jailing in root.GetElementsByTagName( "jailing" ) )
			{
				try
				{
					JailInfo info = new JailInfo( jailing );

					m_Jailings[info.Convicto] = info;
				}
				catch
				{
					Console.WriteLine( "Warning: Jailing instance load failed" );
				}
			}
		}

		private static Point3D[] CellsLocations = new Point3D[]
		{
                	new Point3D(5276,1164,0),
			new Point3D(5286,1164,0),
			new Point3D(5296,1164,0),
			new Point3D(5306,1164,0),
			new Point3D(5276,1174,0),
			new Point3D(5286,1174,0),
			new Point3D(5286,1174,0),
			new Point3D(5306,1174,0),
			new Point3D(5280,1184,0)
		};
		
		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;
			
			if( from.AccessLevel > AccessLevel.Player )
				return;
			
			Account acc = from.Account as Account;
			
			if( acc == null )
			{
				return;
			}
			else
			{
				JailInfo info = GetJailing( acc );

				if( info != null )
				{
					if( info.Active )
						JailMobile( from, null );
					else
						UnJail( acc );
				}
				else
				{
					if( from.Region is Jail )
                                        	UnJailMobile( from );
                                }
                        }
		}

                public static void JailMobile( Mobile m, Mobile jailer )
		{
			if( m == null || m.NetState == null || m.Region is Jail )
				return;

                       	Point3D dest = CellsLocations[ Utility.Random( CellsLocations.Length ) ];

			m.Location = dest;
			m.Map = Map.Felucca;

			if( jailer != null && jailer.NetState != null )
			{
				jailer.Location = dest;
				jailer.Map = Map.Felucca;
			}

			m.SendMessage( "Tu cuenta ha sido encarcelada, por lo que este personaje ha sido movido a la prisión.\nPara ver los datos de tu encarcelamiento, usa el comando \".JailInfo\"" );
                }

		public static JailInfo Jail( Account acc, Mobile jailer )
		{
			return Jail( acc, jailer, null );
		}

		public static JailInfo Jail( Account acc, Mobile jailer, string comentarios )
		{
			if( acc == null || acc.Count < 1 )
				return null;

			JailInfo info = new JailInfo( acc, jailer, comentarios );

			for( int i = 0; i < acc.Length; i++ )
			{
				JailMobile( acc[i], jailer );
                        }

                        m_Jailings[acc] = info;

                        return info;
                }
                
                public static void UnJailMobile( Mobile m )
                {
                	if( m == null || m.NetState == null || !( m.Region is Jail ) )
				return;
				
			m.Location = JailInfo.DefLocation;
			m.Map = Map.Felucca;
			
			m.SendMessage( "Tu cuenta ha cumplido su condena, ahora eres libre." );
		}
                
                public static bool UnJail( Account acc )
		{
			if( acc == null )
				return false;

			JailInfo info = GetJailing( acc );

			if( info != null )
			{
				for( int i = 0; i < acc.Length; i++ )
				{
					UnJailMobile( acc[i] );
                        	}
                                
                                m_Jailings.Remove( acc );
                                return true;
                        }
                        else
                        {
                        	return false;
                        }
                }
        }
}
