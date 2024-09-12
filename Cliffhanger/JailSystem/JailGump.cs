using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Accounting;

namespace Server.Jailing
{
	public enum Pages
	{
		PagePrincipal,
		PageEncarcelar,
		PageDesencarcelar,
		PageMostrarDatos,
		PageDatos,
		PageInfo,
		PageSalir
	}
	
	public enum Botones
	{
		BotonEncarcelar,
		BotonEncarcelarPorCuenta,
		BotonEncarcelarPorPJ,

		BotonDesencarcelar,
		BotonDesencarcelarPorCuenta,
		BotonDesencarcelarPorPJ,

		BotonDatos,
		BotonMostrarDatosPorCuenta,
		BotonMostrarDatosPorPJ,

		BotonAceptarDatosEncarcelamiento,

		//BotonLista,
		BotonSalir,
		BotonVolver
	}
			
	public enum Texto
	{
                TextoCuenta,

                TextoComentarios,

                TextoDias,
                TextoHoras,
                TextoMinutos
	}
		
	public class JailGump: Gump
	{
		private Mobile m_From;
		private JailInfo m_Info;
			
		public JailGump( Mobile from, Pages page ) : this( from, page, null )
		{
		}

		public JailGump( Mobile from, Pages page, JailInfo info ) : base( 20, 20 )
		{
			m_From = from;
			m_Info = info;

			Closable=false;
			Disposable=false;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			from.CloseGump( typeof( JailGump ) );

			if( page == Pages.PagePrincipal )
			{
				AddBackground(0, 0, 370, 270, 9200);
				AddImageTiled(10, 50, 350, 130, 2624);
				AddImageTiled(10, 10, 350, 30, 2624);
				AddAlphaRegion(10, 50, 350, 130);
				AddAlphaRegion(10, 10, 350, 30);
				AddLabel(77, 15, 0x480, "SISTEMA DE ENCARCELAMIENTO" );

				AddButton(75, 60, 4014, 4015, (int)Botones.BotonEncarcelar, GumpButtonType.Reply, 0);
				AddLabel(115, 60, 0x480, "Encarcelar una cuenta" );

				AddButton(75, 90, 4014, 4015, (int)Botones.BotonDesencarcelar, GumpButtonType.Reply, 0);
				AddLabel(115, 90, 0x480, "Desencarcelar una cuenta" );

				AddButton(75, 120, 4014, 4015, (int)Botones.BotonDatos, GumpButtonType.Reply, 0);
				AddLabel(115, 120, 0x480, "Datos de un encarcelamiento" );

				//AddButton(75, 150, 4014, 4015, (int)Botones.BotonLista, GumpButtonType.Reply, 0);
				//AddLabel(115, 150, 0x480, "Listar todos los encarcelamientos" );

				AddImageTiled(10, 190, 350, 30, 2624);
				AddAlphaRegion(10, 190, 350, 30);
				AddLabel(80, 196, 0x480, String.Format( "Se registra(n) {0} encarcelado(s).", JailSystem.Table.Count ) );
				AddImageTiled(10, 230, 350, 30, 2624);
				AddAlphaRegion(10, 230, 350, 30);
				AddButton(131, 234, 4011, 4012, (int)Botones.BotonSalir, GumpButtonType.Reply, 0);
				AddLabel(171, 234, 0x480, "SALIR" );
			}
			else if( page == Pages.PageEncarcelar )
			{
				AddBackground(0, 0, 370, 251, 9200);
				AddImageTiled(10, 50, 350, 145, 2624);
				AddImageTiled(10, 10, 350, 30, 2624);
				AddAlphaRegion(10, 50, 350, 145);
				AddAlphaRegion(10, 10, 350, 30);
				AddLabel(100, 17, 0x480, "ENCARCELAR UNA CUENTA");
				AddImageTiled(10, 207, 350, 30, 2624);
				AddAlphaRegion(10, 207, 350, 30);

				AddButton(130, 211, 4011, 4012, (int)Botones.BotonVolver, GumpButtonType.Reply, 0);
				AddLabel(170, 211, 0x480, "VOLVER");

				AddButton(85, 65, 4014, 4015, (int)Botones.BotonEncarcelarPorCuenta, GumpButtonType.Reply, 0);
				AddLabel(120, 66, 0x480, "Encarcelar la sig. cuenta:");
				AddBackground(86, 97, 195, 29, 9300);
				AddTextEntry(89, 101, 187, 20, 0, (int)Texto.TextoCuenta, "");

				AddButton(85, 140, 4014, 4015, (int)Botones.BotonEncarcelarPorPJ, GumpButtonType.Reply, 0);
				AddLabel(120, 141, 0x480, "Encarcelar seleccionando PJ");
			}
			else if( page == Pages.PageDesencarcelar )
			{
				AddBackground(0, 0, 370, 251, 9200);
				AddImageTiled(10, 50, 350, 145, 2624);
				AddImageTiled(10, 10, 350, 30, 2624);
				AddAlphaRegion(10, 50, 350, 145);
				AddAlphaRegion(10, 10, 350, 30);
				AddLabel(82, 17, 0x480, "DESENCARCELAR UNA CUENTA");
				AddImageTiled(10, 207, 350, 30, 2624);
				AddAlphaRegion(10, 207, 350, 30);

				AddButton(130, 211, 4011, 4012, (int)Botones.BotonVolver, GumpButtonType.Reply, 0);
				AddLabel(170, 211, 0x480, "VOLVER");

				AddButton(72, 65, 4014, 4015, (int)Botones.BotonDesencarcelarPorCuenta, GumpButtonType.Reply, 0);
				AddLabel(108, 66, 0x480, "Desencarcelar la sig. cuenta:");
				AddBackground(86, 97, 195, 29, 9300);
				AddTextEntry(89, 101, 187, 20, 0, (int)Texto.TextoCuenta, "" );

				AddButton(72, 140, 4014, 4015, (int)Botones.BotonDesencarcelarPorPJ, GumpButtonType.Reply, 0);
				AddLabel(108, 141, 0x480, "Desencarcelar seleccionando PJ");
			}
			else if( page == Pages.PageMostrarDatos )
			{
				AddBackground(0, 0, 370, 251, 9200);
				AddImageTiled(10, 50, 350, 145, 2624);
				AddImageTiled(10, 10, 350, 30, 2624);
				AddAlphaRegion(10, 50, 350, 145);
				AddAlphaRegion(10, 10, 350, 30);
				AddLabel(82, 17, 0x480, "DATOS DE ENCARCELAMIENTO");
				AddImageTiled(10, 207, 350, 30, 2624);
				AddAlphaRegion(10, 207, 350, 30);

				AddButton(130, 211, 4011, 4012, (int)Botones.BotonVolver, GumpButtonType.Reply, 0);
				AddLabel(170, 211, 0x480, "VOLVER");

				AddButton(42, 65, 4014, 4015, (int)Botones.BotonMostrarDatosPorCuenta, GumpButtonType.Reply, 0);
				AddLabel(78, 66, 0x480, "Datos de encarcelamiento de la sig. cuenta:");
				AddBackground(86, 97, 195, 29, 9300);
				AddTextEntry(89, 101, 187, 20, 0, (int)Texto.TextoCuenta, "" );

				AddButton(42, 140, 4014, 4015, (int)Botones.BotonMostrarDatosPorPJ, GumpButtonType.Reply, 0);
				AddLabel(78, 141, 0x480, "Datos de encarcelamiento seleccionando PJ");
			}
			else if( page == Pages.PageDatos )
			{
				AddBackground(0, 0, 370, 480, 9200);
				AddImageTiled(10, 50, 350, 380, 2624);
				AddImageTiled(10, 10, 350, 30, 2624);
				AddAlphaRegion(10, 50, 350, 380);
				AddAlphaRegion(10, 10, 350, 30);
				AddLabel(94, 17, 0x480, "DATOS DE ENCARCELAMIENTO" );
				AddImageTiled(9, 439, 350, 30, 2624);
				AddAlphaRegion(9, 439, 350, 30);

				AddButton(131, 443, 4011, 4012, (int)Botones.BotonAceptarDatosEncarcelamiento, GumpButtonType.Reply, 0);
				AddLabel(171, 443, 0x480, "ACEPTAR" );

				AddLabel(30, 64, 0x480, "Duración de condena:");

     				AddLabel(30, 100, 0x480, "Días:");
				AddBackground(70, 100, 39, 29, 9300);

     				AddLabel(125, 100, 0x480, "Horas:");
				AddBackground(175, 100, 39, 29, 9300);

     				AddLabel(235, 100, 0x480, "Minutos:");
				AddBackground(295, 100, 39, 29, 9300);

                                if( m_Info != null && m_Info.Duration != TimeSpan.MaxValue )
                                {
					AddTextEntry(72, 102, 35, 25, 0, (int)Texto.TextoDias, ( m_Info.Duration.Days ).ToString() );
					AddTextEntry(177, 102, 35, 25, 0, (int)Texto.TextoHoras, ( m_Info.Duration.Hours ).ToString() );
					AddTextEntry(297, 102, 35, 25, 0, (int)Texto.TextoMinutos, ( m_Info.Duration.Minutes ).ToString() );
					AddLabel(30, 130, 52, String.Format( "Fecha de Salida: {0}", ( m_Info.DateCreation + m_Info.Duration ).ToString() ) );
				}
				else
				{
               				AddTextEntry(72, 102, 36, 26, 0, (int)Texto.TextoDias, "0" );
					AddTextEntry(177, 102, 36, 26, 0, (int)Texto.TextoHoras, "0" );
					AddTextEntry(297, 102, 36, 26, 0, (int)Texto.TextoMinutos, "0"  );
					AddLabel(30, 130, 52, "Fecha de Salida: No Definida" );
                                }

				AddLabel(30, 170, 0x480, "Razón de Encarcelamiento:" );
				AddBackground(30, 190, 320, 160, 9300);

				try
				{
					AddTextEntry(33, 193, 314, 154, 0, (int)Texto.TextoComentarios, m_Info.Comentarios );
				}
				catch
				{
                                        AddTextEntry(33, 193, 314, 154, 0, (int)Texto.TextoComentarios, "Sin Comentarios." );
                                }

                                try
				{
					AddLabel(30, 360, 0x480, String.Format( "Encarcelador: {0}", m_Info.Jailer.Name ) );
                                }
				catch
				{
                                	AddLabel(30, 360, 0x480, "Encarcelador: No Definido" );
                                }
                                        
                                try
				{
					AddLabel(30, 380, 52, String.Format( "Estado de Encarcelamiento: {0}", m_Info.Active ? "Activo." : "Cumplido." ) );
                                }
				catch
				{
                                        AddLabel(30, 380, 52, "Estado de Encarcelamiento: No Definido." );
                                }
                                        
                                try
				{
					AddLabel(30, 400, 52, String.Format( "Fecha de Encarcelamiento: {0}", m_Info.DateCreation.ToString() ) );
                                }
				catch
				{
                                        AddLabel(30, 400, 52, "Fecha de Encarcelamiento: No Definido." );
                                }
			}
			else if( page == Pages.PageInfo )
			{
				AddBackground(0, 0, 370, 480, 9200);
				AddImageTiled(10, 50, 350, 380, 2624);
				AddImageTiled(10, 10, 350, 30, 2624);
				AddAlphaRegion(10, 50, 350, 380);
				AddAlphaRegion(10, 10, 350, 30);
				AddLabel(94, 17, 0x480, "DATOS DE ENCARCELAMIENTO" );
				AddImageTiled(9, 439, 350, 30, 2624);
				AddAlphaRegion(9, 439, 350, 30);

				AddButton(131, 443, 4011, 4012, (int)Botones.BotonSalir, GumpButtonType.Reply, 0);
				AddLabel(171, 443, 0x480, "SALIR" );

				AddLabel(30, 64, 0x480, "Duración de condena:");

     				AddLabel(30, 100, 0x480, "Días:");
				AddBackground(70, 100, 39, 29, 9300);

     				AddLabel(125, 100, 0x480, "Horas:");
				AddBackground(175, 100, 39, 29, 9300);

     				AddLabel(235, 100, 0x480, "Minutos:");
				AddBackground(295, 100, 39, 29, 9300);

                                if( m_Info != null && m_Info.Duration != TimeSpan.MaxValue )
                                {
					AddLabel(72, 102, 52, ( m_Info.Duration.Days ).ToString() );
					AddLabel(177, 102, 52, ( m_Info.Duration.Hours ).ToString() );
					AddLabel(297, 102, 52, ( m_Info.Duration.Minutes ).ToString() );
					AddLabel(30, 130, 52, String.Format( "Fecha de Salida: {0}", ( m_Info.DateCreation + m_Info.Duration ).ToString() ) );
				}
				else
				{
               				AddLabel(72, 102, 52, "N/A" );
					AddLabel(177, 102, 52, "N/A" );
					AddLabel(297, 102, 52, "N/A" );
					AddLabel(30, 130, 52, String.Format( "Fecha de Salida: No Definida" ) );
                                }

				AddLabel(30, 170, 0x480, "Razón de Encarcelamiento:" );
				AddBackground(30, 190, 320, 160, 9300);

				try
				{
					AddHtml( 33, 193, 314, 154, m_Info.Comentarios, true, true );
				}
				catch
				{
                                        AddHtml( 33, 193, 314, 154, "No hay Comentarios", true, true );
                                }

                                try
				{
					AddLabel(30, 360, 0x480, String.Format( "Encarcelador: {0}", m_Info.Jailer.Name ) );
                                }
				catch
				{
                                	AddLabel(30, 360, 0x480, "Encarcelador: No Definido" );
                                }
                                        
                                try
				{
					AddLabel(30, 380, 52, String.Format( "Estado de Encarcelamiento: {0}", m_Info.Active ? "Activo." : "Cumplido (Re-Login)." ) );
                                }
				catch
				{
                                        AddLabel(30, 380, 52, "Estado de Encarcelamiento: No Definido." );
                                }
                                        
                                try
				{
					AddLabel(30, 400, 52, String.Format( "Fecha de Encarcelamiento: {0}", m_Info.DateCreation.ToString() ) );
                                }
				catch
				{
                                        AddLabel(30, 400, 52, "Fecha de Encarcelamiento: No Definido." );
                                }
			}

		}
			
		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( from.AccessLevel < AccessLevel.Counselor )
				return;

			if( info.ButtonID == (int)Botones.BotonEncarcelar )
			{
				from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
			}
			else if( info.ButtonID == (int)Botones.BotonEncarcelarPorCuenta )
			{
				Account acc = null;

				TextRelay text = info.GetTextEntry( (int)Texto.TextoCuenta );

                               	try
				{
					acc = Accounts.GetAccount( Convert.ToString( text.Text ) );

					if( acc == null )
					{
						from.SendMessage("La cuenta señalada no existe.");
                                       		from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
						return;
					}
				}
				catch
				{
                                       	from.SendMessage("La cuenta señalada tiene un formato incorrecto.");
                                       	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
					return;
				}
				
				JailInfo jinfo = JailSystem.GetJailing( acc );

				if( jinfo != null )
				{
					from.SendMessage("La cuenta señalada ya esta encarcelada.");
                                       	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
					return;
				}

                                jinfo = JailSystem.Jail( acc, from );

				if( jinfo == null )
                                {
                                       	from.SendMessage( "No se ha podido realizar el encarcelamiento, cuenta inválida." );
                                       	from.SendGump( new JailGump( from, Pages.PageEncarcelar ) );
                                       	return;
                                }
                                else
                                {
                                        from.SendGump( new JailGump( from, Pages.PageDatos, jinfo ) );
                                }
			}
			else if( info.ButtonID == (int)Botones.BotonEncarcelarPorPJ )
			{
                                from.SendMessage("Elige el pj perteneciente a la cuenta a encarcelar...");
                                from.Target = new JailTarget();
			}
    			else if( info.ButtonID == (int)Botones.BotonDesencarcelar )
			{
				from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
			}
			else if( info.ButtonID == (int)Botones.BotonDesencarcelarPorCuenta )
			{
				Account acc = null;

				TextRelay text = info.GetTextEntry( (int)Texto.TextoCuenta );

                               	try
				{
					acc = Accounts.GetAccount( Convert.ToString( text.Text ) );

					if( acc == null )
					{
						from.SendMessage("La cuenta señalada no existe.");
                                       		from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
						return;
					}

					JailInfo jinfo = JailSystem.GetJailing( acc );

					if( jinfo == null )
					{
						from.SendMessage("La cuenta señalada no esta en Jail.");
                                       		from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
						return;
					}
				}
				catch
				{
                                       	from.SendMessage("La cuenta señalada tiene un formato incorrecto.");
                                       	from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
					return;
				}

				if( !JailSystem.UnJail( acc ) )
                                {
                                       	from.SendMessage( "No se ha podido realizar el desencarcelamiento." );
                                       	from.SendGump( new JailGump( from, Pages.PageDesencarcelar ) );
                                       	return;
                                }
                                else
                                {
                                       	from.SendGump( new JailGump( from, Pages.PagePrincipal ) );
                                       	from.SendMessage("Desencarcelamiento realizado correctamente.");
                                }
			}
			else if( info.ButtonID == (int)Botones.BotonDesencarcelarPorPJ )
			{
                               	from.SendMessage("Elige el pj perteneciente a la cuenta a desencarcelar...");
                                from.Target = new UnJailTarget();
			}
			else if( info.ButtonID == (int)Botones.BotonDatos )
			{
				from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
			}
			else if( info.ButtonID == (int)Botones.BotonMostrarDatosPorCuenta )
			{
				JailInfo jinfo = null;

				TextRelay text = info.GetTextEntry( (int)Texto.TextoCuenta );

                               	try
				{
					Account acc = Accounts.GetAccount( Convert.ToString( text.Text ) );

					if( acc == null )
					{
						from.SendMessage("La cuenta señalada no existe.");
                                       		from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
						return;
					}

					jinfo = JailSystem.GetJailing( acc );

					if( jinfo == null )
					{
						from.SendMessage("La cuenta señalada no esta en Jail.");
                                       		from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
						return;
					}
				}
				catch
				{
                                       	from.SendMessage("La cuenta señalada tiene un formato incorrecto.");
                                       	from.SendGump( new JailGump( from, Pages.PageMostrarDatos ) );
					return;
				}

				from.SendGump( new JailGump( from, Pages.PageDatos, jinfo ) );
			}
			else if( info.ButtonID == (int)Botones.BotonMostrarDatosPorPJ )
			{
                               	from.SendMessage("Elige el pj perteneciente a la cuenta cuyos datos de encarcelamiento deseas ver...");
                                from.Target = new JailInfoTarget();
			}
			else if( info.ButtonID == (int)Botones.BotonAceptarDatosEncarcelamiento )
			{
				if( m_Info == null )
				{
					from.SendMessage("Ha ocurrido un error inesperado. Es necesario revisar el sistema.");
                                        from.SendGump( new JailGump( from, Pages.PagePrincipal ) );
					return;
				}

				string comentario;

                               	TextRelay text = info.GetTextEntry( (int)Texto.TextoComentarios );

                               	try
				{
					comentario = Convert.ToString( text.Text );

					if( comentario == "" )
                                        {
                                       		from.SendMessage("La razón del encarcelamiento no puede estar vacía.");
                                       		from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
						return;
					}
				}
				catch
				{
                                       	from.SendMessage("El comentario tiene un formato incorrecto.");
                                       	from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
					return;
				}

				int dias, horas, minutos;

				text = info.GetTextEntry( (int)Texto.TextoDias );

                               	try
				{
					dias = Convert.ToInt32( text.Text );
				}
				catch
				{
                                       	from.SendMessage("Los días tienen un formato incorrecto, sólo números permitidos.");
                                       	from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
					return;
				}

				text = info.GetTextEntry( (int)Texto.TextoHoras );

                               	try
				{
					horas = Convert.ToInt32( text.Text );
				}
				catch
				{
                                       	from.SendMessage("Las horas tienen un formato incorrecto, sólo números permitidos.");
                                       	from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
					return;
				}

				text = info.GetTextEntry( (int)Texto.TextoMinutos );

                               	try
				{
					minutos = Convert.ToInt32( text.Text );
				}
				catch
				{
                                       	from.SendMessage("Los minutos tienen un formato incorrecto, sólo números permitidos.");
                                       	from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
					return;
				}

				TimeSpan duration = TimeSpan.Zero;

				try
				{
					duration = new TimeSpan( dias, horas, minutos, 0 );
				}
                                catch
				{
                                       	from.SendMessage("El formato de la duración tiene un error.");
                                       	from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
					return;
				}

				if( duration == TimeSpan.Zero )
                                {
                                       	from.SendMessage("La duración de la condena no puede ser nula.");
                                       	from.SendGump( new JailGump( from, Pages.PageDatos, m_Info ) );
					return;
				}

                                m_Info.UpdateInfo( comentario, duration );

                                from.SendMessage( "Encarcelamiento actualizado correctamente." );
                                from.SendGump( new JailGump( from, Pages.PagePrincipal ) );
			}
			/*
			else if( info.ButtonID == (int)Botones.BotonLista )
			{
			}
			*/
			else if( info.ButtonID == (int)Botones.BotonVolver )
			{
                               	from.SendGump( new JailGump( from, Pages.PagePrincipal ) );
			}
		}
	}
}
