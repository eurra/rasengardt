using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Guilds;

namespace Server.Gumps
{
	public class GuildAdminInfoGump : Gump
	{
		public enum Botones
		{
			Cerrar,
			Color1,
			Color2,
			IrAGuildstone,
			MenuGuild
		}

		private Guild m_Guild;
		private Mobile m_From;

		public GuildAdminInfoGump( Mobile owner, Guild guild ) : base( 0, 0 )
		{
			owner.CloseGump( typeof( GuildAdminInfoGump ) );

                        m_Guild = guild;
                        
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			AddBackground(0, 0, 400, 500, 9250);

			AddAlphaRegion(20, 395, 360, 60);
			AddAlphaRegion(20, 225, 360, 155);
			AddAlphaRegion(20, 50, 360, 160);
			
			AddLabel(131, 21, 0x480, "DETALLE DE GUILD");

			AddButton(145, 463, 4017, 4018, (int)Botones.Cerrar, GumpButtonType.Reply, 0);
			AddLabel(181, 463, 0x480, "CERRAR");

			AddLabel(52, 60, 55, "Nombre:");
			try{ AddLabel(80, 80, 0x480, m_Guild.Name); } catch{ AddLabel(80, 80, 0x480, "No Especificado."); }

			AddLabel(52, 110, 55, "Abreviación:");
			try{ AddLabel(80, 130, 0x480, m_Guild.Abbreviation); } catch{ AddLabel(80, 130, 0x480, "No Especificada."); }

			AddLabel(52, 160, 55, "Fecha de Creación:");
			try{ AddLabel(80, 180, 0x480, m_Guild.DateCreation.ToString() ); } catch{ AddLabel(80, 180, 0x480, "No Especificada"); }

			int hue;

			try
			{
				hue = m_Guild.CustomHue1;
                        }
			catch
			{
                        	hue = 0;
                        }

			AddImage(132, 238, 111, hue - 1);
			AddLabel(33, 236, 55, "Color Primario:");
			AddBackground(32, 256, 60, 20, 9300);
			
			if( owner.AccessLevel == AccessLevel.Administrator )
			{
				AddTextEntry(33, 257, 58, 18, 0, 0, hue.ToString() );
                                AddButton(94, 256, 4008, 4009, (int)Botones.Color1, GumpButtonType.Reply, 0);
			}
			else
			{
                                AddLabel(33, 257, 0, hue.ToString() );
                        }
				
			if( hue == 0 )
				AddLabel(38, 281, 0x480, "(No Asignado)");

                        try
			{
				hue = m_Guild.CustomHue2;
                        }
			catch
			{
                        	hue = 0;
                        }

			AddImage(309, 238, 111, hue - 1);
			AddLabel(202, 236, 55, "Color Secundario:");
			AddBackground(206, 258, 60, 20, 9300);
			
			if( owner.AccessLevel == AccessLevel.Administrator )
			{
				AddTextEntry(207, 259, 58, 18, 0, 1, hue.ToString() );
				AddButton(268, 256, 4008, 4009, (int)Botones.Color2, GumpButtonType.Reply, 0);
                        }
			else
			{
                                AddLabel(207, 259, 0, hue.ToString() );
                        }

			if( hue == 0 )
				AddLabel(212, 281, 0x480, "(No Asignado)");

			AddHtml( 30, 308, 338, 56, "* Para anular la asignación de un color a un clan, dejar el valor del color en 0.\n* Para tener un color secundario, se requiere que exista un color primario.", (bool)true, (bool)true);

			AddLabel(115, 400, 0x480, "Ir a GuildStone");
			AddButton(75, 400, 4005, 4006, (int)Botones.IrAGuildstone, GumpButtonType.Reply, 0);

			AddLabel(115, 430, 0x480, "Abrir menú de control de la Guild");
			AddButton(75, 430, 4005, 4006, (int)Botones.MenuGuild, GumpButtonType.Reply, 0);
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if( m_Guild == null )
				return;

			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case (int)Botones.Cerrar :
				{
					return;
				}
				case (int)Botones.IrAGuildstone :
				{
					if( m_Guild.Guildstone != null )
					{
						from.Map = m_Guild.Guildstone.Map;
                                                from.Location = m_Guild.Guildstone.Location;
                                        }

                                        from.SendGump( new GuildAdminInfoGump( from, m_Guild ) );

					break;
				}
				case (int)Botones.MenuGuild :
				{
					from.SendGump( new GuildAdminInfoGump( from, m_Guild ) );
					from.SendGump( new GuildGump( from, m_Guild ) );
					
					break;
				}
                                case (int)Botones.Color1 :
                                {
                                	TextRelay text = info.GetTextEntry( 0 );

                               		try
					{
						int hue = Convert.ToInt32( text.Text );
						
						if( hue != 0 && m_Guild.CustomHue2 == hue )
							from.SendMessage( "El color primario seleccionado no puede ser igual al secundario." );
						else if( ColorRepetido( hue ) )
                                                        from.SendMessage( "El color primario seleccionado ya esta asignado a otro clan." );
						else
							m_Guild.CustomHue1 = hue;
					}
					catch
					{
                                       		from.SendMessage("Formato del color 1 incorrecto, solo números positivos.");
					}
					
					from.SendGump( new GuildAdminInfoGump( from, m_Guild ) );
					
					break;
                                }
                                case (int)Botones.Color2 :
                                {
                                	TextRelay text = info.GetTextEntry( 1 );

                               		try
					{
						int hue = Convert.ToInt32( text.Text );
						
						if( hue != 0 && m_Guild.CustomHue1 == hue )
							from.SendMessage( "El color secundario seleccionado no puede ser igual al primario." );
						else if( ColorRepetido( hue ) )
                                                        from.SendMessage( "El color secundario seleccionado ya esta asignado a otro clan." );
						else
							m_Guild.CustomHue2 = hue;
					}
					catch
					{
                                       		from.SendMessage("Formato del color 2 incorrecto, solo números positivos.");
					}

					from.SendGump( new GuildAdminInfoGump( from, m_Guild ) );

					break;
                                }
			}
		}
		
		private bool ColorRepetido( int hue )
		{
			if( hue == 0 )
				return false;
				
			foreach( BaseGuild g in BaseGuild.List.Values )
			{
                                Guild guild = g as Guild;
                                
                                if( guild != null )
                                {
                                	if( guild.CustomHue1 == hue || guild.CustomHue2 == hue )
                                		return true;
                                }
			}
			
			return false;
		}
	}
}
