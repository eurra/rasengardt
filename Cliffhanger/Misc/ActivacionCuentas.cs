using System;
using System.Data;
using System.Collections;
using System.Data.Odbc;
using System.Web.Mail;
using Server.Gumps;
using Server.Network;
using Server.Accounting;

namespace Server.Scripts.Commands
{
 	public class ActivacionCuentas
 	{
 		public static void Initialize()
		{
			Server.Commands.Register( "ActCuentas", AccessLevel.Administrator, new CommandEventHandler( ActCuentas_OnCommand ) );
		}

       		[Usage( "ActCuentas" )]
		[Description( "Administra peticiones de cuentas." )]
		private static void ActCuentas_OnCommand( CommandEventArgs e )
		{
                	e.Mobile.SendGump( new ActCuentasGump( e.Mobile ) );
		}
		
		public enum Pages
		{
			ListarItems,
			DetalleItem,
			ConfirmarActivacion,
			ConfirmarBorrado,
			Busqueda
		}

		private static readonly string usertable = "usuario";
                private static readonly string idrow = "ID_User";
                private static readonly string nickrow = "Nickname";
                private static readonly string emailrow = "Email";
                private static readonly string actrow = "Estado";
                private static readonly string staffmail = "rasengardt@lan-z.net";
                private static readonly string smtpserver = "mail.lan-z.net";

		public class ActCuentasGump : Gump
		{
		        private const int EntryCount = 8;
		        
		        public enum Botones
		        {
		        	ListarActivadas,
		        	ListarNoActivadas,
		        	PagSig,
		        	PagPrev,
		        	Buscar,
		        	Activar,
		        	Borrar,
		        	Volver,
		        	ConfirmarActivar,
		        	ConfirmarBorrar,
		        	MenuBuscar,
		        	VolverConfirmacion,
		        	Salir
		        }
		        
		        private int m_Dis;
		        private bool m_Act;
		        private DataRow m_Row;
		        private ArrayList m_List;
		        private string[] m_BusParam;

			public ActCuentasGump( Mobile from ) : this( from, Pages.ListarItems, false, 0, null, null )
			{
			}

			public ActCuentasGump( Mobile from, Pages page, bool activas, int dis, DataRow row, string[] busparam ) : base( 30, 30 )
			{
				from.CloseGump( typeof( ActCuentasGump ) );
				m_Dis = dis;
				m_Act = activas;
				m_Row = row;
				m_BusParam = busparam;
				m_List = null;

				Closable=false;
				Disposable=false;
				Dragable=true;
				Resizable=false;

				AddPage(0);
				
				if( page == Pages.ListarItems )
				{
					int count = 0;
					
					AddBackground(0, 0, 300, 340, 9200);
					AddImageTiled(10, 10, 150, 50, 2624);
					AddImageTiled(170, 10, 120, 50, 2624);
					AddImageTiled(10, 70, 280, 215, 2624);
					AddImageTiled(10, 295, 280, 35, 2624);
					AddAlphaRegion(10, 10, 150, 50);
					AddAlphaRegion(170, 10, 120, 50);
					AddAlphaRegion(10, 70, 280, 215);
					AddAlphaRegion(10, 295, 280, 35);

					if( m_BusParam != null && m_BusParam.Length == 3 )
						m_List = GenerarLista( m_BusParam[0], m_BusParam[1], m_BusParam[2] );
					else
                                                m_List = GenerarLista( m_Act );

					if( m_List != null )
					{
						count = m_List.Count - (dis * EntryCount);

						if ( count < 0 )
							count = 0;
						else if ( count > EntryCount )
							count = EntryCount;
					}
					else
					{
						AddLabel(58, 100, 0x480, "No se pudo conectar a la BD." );
					}
                                        
                                        AddButton(155, 303, 4029, 4030, (int)Botones.MenuBuscar, GumpButtonType.Reply, 0);
					AddLabel(190, 305, 0x480, "BUSQUEDA");

					AddButton(40, 303, 4017, 4018, (int)Botones.Salir, GumpButtonType.Reply, 0);
					AddLabel(75, 305, 0x480, "SALIR");

					AddLabel(37, 15, 0x480, "ACTIVACION DE");
					AddLabel(54, 35, 0x480, "CUENTAS");

					AddButton(171, 12, 4005, 4006, (int)Botones.ListarActivadas, GumpButtonType.Reply, 0);
					AddButton(171, 35, 4005, 4006, (int)Botones.ListarNoActivadas, GumpButtonType.Reply, 0);

					int hue1 = ( m_BusParam == null ? ( m_Act ? 154 : 0x480 ) : 0x480 );
					int hue2 = ( m_BusParam == null ? ( m_Act ? 0x480 : 154 ) : 0x480 );

					AddLabel(205, 13, hue1, "Activadas");
					AddLabel(205, 38, hue2, "No Activadas");

					if( count > 0 )
					{
						AddLabel(60, 70, 64, "ID Usuario");
						AddLabel(165, 70, 64, "Detalles");
						
						if ( dis > 0 )
							AddButton(15, 260, 5603, 5607, (int)Botones.PagPrev, GumpButtonType.Reply, 0);

						if ( (dis + 1) * EntryCount < m_List.Count )
                                                	AddButton(270, 260, 5601, 5605, (int)Botones.PagSig, GumpButtonType.Reply, 0);
                                                	
                                                for ( int i = 0, index = dis * EntryCount; i < EntryCount && index < m_List.Count; ++i, ++index )
						{
							int hue = ( m_BusParam != null ? ( ((DataRow)m_List[index])[actrow].ToString() == "Y" ? 54 : 33 ) : 0x480 );
							AddLabel(65, 100 + (20 * i), hue, ((DataRow)m_List[index])[idrow].ToString() );
							AddButton(175, 100 + (20 * i), 4011, 4012, i + 14, GumpButtonType.Reply, 0);
						}
					}
					else if( m_List != null )
					{
						AddLabel(58, 100, 0x480, "No se encontraron elementos." );
					}
				}
				else if( page == Pages.DetalleItem && row != null )
				{
					AddBackground(0, 0, 300, 300, 9200);
					AddImageTiled(10, 10, 150, 50, 2624);
					AddImageTiled(170, 10, 120, 50, 2624);
					AddImageTiled(10, 70, 280, 180, 2624);
					AddImageTiled(10, 260, 280, 30, 2624);
					AddAlphaRegion(10, 10, 150, 50);
					AddAlphaRegion(170, 10, 120, 50);
					AddAlphaRegion(10, 70, 280, 180);
					AddAlphaRegion(10, 260, 280, 30);

					AddLabel(37, 15, 0x480, "ACTIVACION DE");
					AddLabel(54, 35, 0x480, "CUENTAS");
					
					AddButton(100, 264, 4014, 4015, (int)Botones.Volver, GumpButtonType.Reply, 0);
					AddLabel(134, 265, 0x480, "VOLVER");

					if( row[actrow].ToString() == "N" )
					{
						AddButton(171, 12, 4005, 4006, (int)Botones.ConfirmarActivar, GumpButtonType.Reply, 0);
						AddLabel(205, 13, 0x480, "Activar");

						AddButton(171, 35, 4005, 4006, (int)Botones.ConfirmarBorrar, GumpButtonType.Reply, 0);
						AddLabel(205, 38, 0x480, "Borrar");
						
						AddLabel(200, 75, 33, "No Activada");
					}
					else
					{
                                        	AddLabel(200, 75, 54, "Activada");
                                        }

					AddLabel(40, 100, 64, "ID Usuario:");
					AddLabel(90, 120, 0x480, row[idrow].ToString() );

					AddLabel(40, 140, 64, "Nick:");
					AddLabel(90, 160, 0x480, row[nickrow].ToString() );

					AddLabel(40, 180, 64, "E-Mail:");
					AddLabel(90, 200, 0x480, row[emailrow].ToString() );
				}
				else if( page == Pages.ConfirmarBorrado && row != null && row[actrow].ToString() == "N" )
				{
					AddBackground(0, 0, 300, 240, 9200);
					AddImageTiled(10, 10, 280, 30, 2624);
					AddImageTiled(10, 50, 280, 180, 2624);
					AddAlphaRegion(10, 10, 280, 30);
					AddAlphaRegion(10, 50, 280, 180);
					
					AddLabel(110, 175, 0x480, "Anular la solicitud");
					AddButton(70, 175, 4017, 4018, (int)Botones.Borrar, GumpButtonType.Reply, 0);
					
					AddLabel(110, 200, 0x480, "Volver");
					AddButton(70, 200, 4014, 4015, (int)Botones.VolverConfirmacion, GumpButtonType.Reply, 0);

					AddLabel(25, 15, 0x480, "CONFIRMAR ANULACION DE SOLICITUD");
					AddHtml( 20, 65, 260, 100, String.Format( "¿Seguro que deseas anular la solicitud nro. {0} del miembro {1} ? Al anularla, la cuenta de este usuario en la página del servidor seguirá activa, pero no podrá volver a realizar otra solicitud a través de ella.", row[idrow].ToString(), row[nickrow].ToString() ), true, true );
				}
				else if( page == Pages.ConfirmarActivacion && row != null && row[actrow].ToString() == "N" )
				{
					AddBackground(0, 0, 300, 240, 9200);
					AddImageTiled(10, 10, 280, 30, 2624);
					AddImageTiled(10, 50, 280, 180, 2624);
					AddAlphaRegion(10, 10, 280, 30);
					AddAlphaRegion(10, 50, 280, 180);

					AddLabel(110, 175, 0x480, "Activar la solicitud");
					AddButton(70, 175, 4017, 4018, (int)Botones.Activar, GumpButtonType.Reply, 0);
					
					AddLabel(110, 200, 0x480, "Volver");
					AddButton(70, 200, 4014, 4015, (int)Botones.VolverConfirmacion, GumpButtonType.Reply, 0);

					AddLabel(23, 15, 0x480, "CONFIRMAR ACTIVACION DE SOLICITUD");
					AddHtml( 20, 65, 260, 100, String.Format( "¿Seguro que deseas activar la solicitud nro. {0} del miembro {1} ? Al activarla, se creará una nueva cuenta en el server con los datos del registro y se despachará un correo electrónico a {2} y una copia del mismo al correo del staff configurado. Además, la solicitud quedará como \"activada\".", row[idrow].ToString(), row[nickrow].ToString(), row[emailrow].ToString() ), true, true );
				}
				else if( page == Pages.Busqueda )
				{
					AddBackground(0, 0, 300, 300, 9200);
					AddImageTiled(10, 10, 150, 50, 2624);
					AddImageTiled(170, 10, 120, 50, 2624);
					AddImageTiled(10, 70, 280, 180, 2624);
					AddImageTiled(10, 260, 280, 30, 2624);
					AddAlphaRegion(10, 10, 150, 50);
					AddAlphaRegion(170, 10, 120, 50);
					AddAlphaRegion(10, 70, 280, 180);
					AddAlphaRegion(10, 260, 280, 30);

					AddLabel(37, 15, 0x480, "ACTIVACION DE");
					AddLabel(54, 35, 0x480, "CUENTAS");

					AddLabel(134, 265, 0x480, "VOLVER");
					AddButton(100, 264, 4014, 4015, (int)Botones.Volver, GumpButtonType.Reply, 0);

					AddLabel(135, 222, 0x480, "BUSCAR");
					AddButton(100, 220, 4014, 4015, (int)Botones.Buscar, GumpButtonType.Reply, 0);

					AddLabel(100, 75, 0x480, "Buscar usando:");

					AddLabel(30, 105, 64, "ID Usuario:");
					AddBackground(120, 105, 90, 25, 9300);

					AddLabel(66, 145, 64, "Nick:");
					AddBackground(120, 145, 140, 25, 9300);

					AddLabel(51, 185, 64, "E-Mail:");
                                        AddBackground(120, 185, 140, 25, 9300);

					if( m_BusParam != null && m_BusParam.Length == 3 )
					{
						AddTextEntry(121, 106, 88, 23, 0, 0, m_BusParam[0]);
						AddTextEntry(121, 146, 138, 23, 0, 1, m_BusParam[1]);
						AddTextEntry(121, 186, 138, 23, 0, 2, m_BusParam[2]);
					}
					else
					{
						AddTextEntry(121, 106, 88, 23, 0, 0, "");
						AddTextEntry(121, 146, 138, 23, 0, 1, "");
						AddTextEntry(121, 186, 138, 23, 0, 2, "");
					}
				}
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;

				switch ( info.ButtonID )
				{
					case (int)Botones.ListarActivadas :
					{
						from.SendGump( new ActCuentasGump( from, Pages.ListarItems, true, 0, null, null ) );
                                        	break;
                                        }
                                        case (int)Botones.ListarNoActivadas :
					{
                                        	from.SendGump( new ActCuentasGump( from, Pages.ListarItems, false, 0, null, null ) );
                                        	break;
                                        }
                                        case (int)Botones.PagSig :
					{
                                        	from.SendGump( new ActCuentasGump( from, Pages.ListarItems, m_Act, m_Dis + 1, null, m_BusParam ) );
                                        	break;
                                        }
                                        case (int)Botones.PagPrev :
					{
                                        	from.SendGump( new ActCuentasGump( from, Pages.ListarItems, m_Act, m_Dis - 1, null, m_BusParam ) );
                                        	break;
                                        }
                                        case (int)Botones.Volver :
                                        {
						from.SendGump( new ActCuentasGump( from, Pages.ListarItems, m_Act, m_Dis, null, m_BusParam ) );
						break;
                                        }
                                        case (int)Botones.Salir :
                                        {
                                        	break;
                                        }
                                        case (int)Botones.Buscar :
                                        {
                                        	string[] busq = new string[3];

						TextRelay text = info.GetTextEntry( 0 );

                                                if( text != null && text.Text != "" )
                                                {
                                                	try
							{
								int check = Convert.ToInt32( text.Text );
							}
							catch
							{
                                       				from.SendMessage("El ID de usuario debe ser un número.");
                                       				from.SendGump( new ActCuentasGump( from, Pages.Busqueda, m_Act, m_Dis, null, m_BusParam ) );
								return;
							}
							
							busq[0] = text.Text;
						}
						else
						{
                                                        busq[0] = "";
                                                }
						
						text = info.GetTextEntry( 1 );
						
						if( text != null )
							busq[1] = text.Text;
						else
                                                        busq[1] = "";
						
						text = info.GetTextEntry( 2 );

						if( text != null )
							busq[2] = text.Text;
						else
                                                        busq[2] = "";
                                                        
						if( busq[0] == "" && busq[1] == "" && busq[2] == "" )
						{
                                                	from.SendMessage("Debes ingresar algún dato para la búsqueda!.");
                                       			from.SendGump( new ActCuentasGump( from, Pages.Busqueda, m_Act, m_Dis, null, m_BusParam ) );
							return;
						}

                                                from.SendGump( new ActCuentasGump( from, Pages.ListarItems, m_Act, 0, null, busq ) );

						break;
                                        }
                                        case (int)Botones.MenuBuscar :
                                        {
                                        	from.SendGump( new ActCuentasGump( from, Pages.Busqueda, m_Act, m_Dis, null, m_BusParam ) );
						break;
                                        }
                                        case (int)Botones.Activar :
                                        {
                                        	ActivarSolicitud( from, m_Row );
                                                from.SendGump( new ActCuentasGump( from, Pages.ListarItems, m_Act, 0, null, m_BusParam ) );
						break;
                                        }
                                        case (int)Botones.Borrar :
                                        {
                                        	AnularSolicitud( from, m_Row );
                                        	from.SendGump( new ActCuentasGump( from, Pages.ListarItems, m_Act, 0, null, m_BusParam ) );
						break;
                                        }
                                        case (int)Botones.ConfirmarBorrar :
                                        {
                                        	from.SendGump( new ActCuentasGump( from, Pages.ConfirmarBorrado, m_Act, m_Dis, m_Row, m_BusParam ) );
						break;
                                        }
                                        case (int)Botones.ConfirmarActivar :
                                        {
                                        	from.SendGump( new ActCuentasGump( from, Pages.ConfirmarActivacion, m_Act, m_Dis, m_Row, m_BusParam ) );
						break;
                                        }
                                        case (int)Botones.VolverConfirmacion :
                                        {
                                                from.SendGump( new ActCuentasGump( from, Pages.DetalleItem, m_Act, m_Dis, m_Row, m_BusParam ) );
						break;
                                        }
                                        default:
                                        {
                                        	int index = (m_Dis * EntryCount) + (info.ButtonID - 14);
                                        	
                                        	if ( m_List != null && index >= 0 && index < m_List.Count )
                                                	from.SendGump( new ActCuentasGump( from, Pages.DetalleItem, m_Act, m_Dis, (DataRow)m_List[index], m_BusParam ) );

                                        	break;
                                        }
                                }
			}
			
			private DataSet SQL( string sql, bool ret )
			{
                		string MyConString = 	"DRIVER={MySQL ODBC 3.51 Driver};" +
                         			"SERVER=localhost;" +
                         			"DATABASE=uo_usuarios;" +
                         			"UID=root;" +
                         			"PASSWORD=54210934;" +
                         			"OPTION=3";

                        	DataSet res = null;

                        	try
                        	{
					OdbcConnection MyConnection = new OdbcConnection( MyConString );
				
					if( ret )
					{
						
						OdbcDataAdapter MyDataAdapter = new OdbcDataAdapter( sql, MyConnection );
						res = new DataSet();
                        			MyDataAdapter.Fill( res, usertable );
                        		}
                        		else
                        		{
                        			OdbcCommand com = new OdbcCommand( sql, MyConnection );
                                        	MyConnection.Open();
                        			com.ExecuteNonQuery();
                        			MyConnection.Close();
                        		}
                        	}
                        	catch( Exception e )
                        	{
                        		Console.WriteLine( e.Message );
					return null;
                        	}

      				return res;
      			}
			
			private ArrayList GenerarLista( bool activadas )
			{
                        	ArrayList res = new ArrayList();

                        	DataSet data = SQL( String.Format( "SELECT {0}, {1}, {2}, {3} FROM {4} WHERE `{5}` = \"{6}\"", idrow, nickrow, emailrow, actrow, usertable, actrow, activadas ? "Y" : "N" ), true );

				DataTable table;

                                if( data != null )
					table = data.Tables[usertable];
				else
					return null;
					
				foreach( DataRow row in table.Rows )
					res.Add( row );
					
				return res;
			}

			private ArrayList GenerarLista( string id, string nick, string mail )
			{
                                ArrayList res = new ArrayList();

				DataSet data = SQL( String.Format( "SELECT {0}, {1}, {2}, {3} FROM {4} WHERE NOT `{5}` = \"B\"", idrow, nickrow, emailrow, actrow, usertable, actrow ), true );

				DataTable table;

				if( data != null )
					table = data.Tables[usertable];
				else
					return null;

				if( id != null && id != "" )
				{
					foreach( DataRow row in table.Rows )
					{
						if( row[idrow].ToString().IndexOf( id ) >= 0 )
							res.Add( row );
					}
				}
				
                                if( nick != null && nick != "" )
				{
					foreach( DataRow row in table.Rows )
					{
						if( row[nickrow].ToString().ToLower().IndexOf( nick.ToLower() ) >= 0 && !res.Contains( row ) )
							res.Add( row );
					}
				}
				
				if( mail != null && mail != "" )
				{
					foreach( DataRow row in table.Rows )
					{
						if( row[emailrow].ToString().ToLower().IndexOf( mail.ToLower() ) >= 0 && !res.Contains( row ) )
							res.Add( row );
					}
				}

				return res;
			}

			private void AnularSolicitud( Mobile from, DataRow row )
			{
				if( row == null )
					return;
					
				SQL( String.Format( "UPDATE {0} SET `{1}`=\"B\" WHERE `{2}`=\"{3}\"", usertable, actrow, idrow, row[idrow].ToString() ), false );

				from.SendMessage( "Solicitud anulada correctamente." );
			}
			
			private void ActivarSolicitud( Mobile from, DataRow row )
			{
				if( row == null )
					return;

				string username = row[idrow].ToString();
				string password = CreateRandomPassword( 8 );
				
				Account a = Accounts.AddAccount( username, password );
				
				if( a != null )
				{
                                 	SQL( String.Format( "UPDATE {0} SET `{1}`=\"Y\" WHERE `{2}`=\"{3}\"", usertable, actrow, idrow, row[idrow].ToString() ), false );

                                 	if( SendMail( row, password ) )
                                 		from.SendMessage( "Solicitud activada correctamente." );
                                 	else
                                        	from.SendMessage( "Error al enviar el correo electrónico. La solicitud se activó de todas formas" );
				}
				else
				{
					from.SendMessage( "No se pudo crear la cuenta de usuario." );
				}
			}
			
			private string CreateRandomPassword( int PasswordLength )
  			{
   				string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
   				char[] chars = new char[PasswordLength];

   				for(int i = 0; i < PasswordLength; i++ )
    					chars[i] = allowedChars[ Utility.Random( allowedChars.Length ) ];

   				return new string( chars );
  			}
  			
  			private bool SendMail( DataRow row, string password )
  			{
                        	if( row == null || password == null )
                        		return false;
                        		
                        	string body = 	"Saludos {0}:\n" +
                               			"	El staff de Rasengardt tiene el agrado de informarte que la cuenta que has " +
                               			"solicitado en nuestro server ya ha sido activada. A continuación te damos los datos " +
                               			"que se te han asignado para conectarte:\n\n" +
                               			"Nombre de Usuario:	{1}\n" +
                               			"Contraseña:		{2}\n\n" +
                               			"Guarda estos datos con cautela y protejelos adecuadamente. De todas formas, " +
                               			"al entrar a nuestro servidor, te recomendamos cambiar tu contraseña usando el siguiente comando:\n" +
                               			"	.password <nueva_contraseña> <nueva_contraseña>\n" +
                               			"Recuerda leer las reglas de nuestro server para que no tengas problemas y participar en los foros " +
                               			"de la comunidad, ubicados en http://www.tarreo.cl/foro/index.php?s=2255ac3c34afe99a712065b8ae76cc83&showforum=309 \n\n" +
                               			"Bienvenido a Rasengardt!\n" +
                               			"Atte. El Staff.";

  				MailMessage msg = new MailMessage();
				msg.From = staffmail;
				msg.To = row[emailrow].ToString();
				msg.Cc = "rasengardt.uo@gmail.com";
				msg.Subject = "Tu cuenta en Rasengardt ha sido activada.";
				msg.BodyFormat = MailFormat.Text;
				msg.Body = String.Format( body, row[nickrow].ToString(), row[idrow].ToString(), password );

				SmtpMail.SmtpServer = smtpserver;
				
				try
				{
					SmtpMail.Send( msg );
					return true;
				}
				catch( Exception e )
				{
                                        Console.WriteLine( e.Message );
					return false;
				}
  			}
		}
	 }
}
