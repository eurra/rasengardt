using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Guilds;

namespace Server.Gumps
{
	public class GuildAdminGump : Gump
	{
		private ArrayList m_List;
		private int m_Page;
		
		private const int EntryCount = 15;

		public GuildAdminGump( Mobile owner ) : this( owner, 0 )
		{
		}

		public GuildAdminGump( Mobile owner, int page ) : base( 0, 0 )
		{
			owner.CloseGump( typeof( GuildAdminGump ) );
                        m_List = BuildList();
			Initialize( page );
		}

		private static ArrayList BuildList()
		{
			ArrayList list = new ArrayList();

			foreach( BaseGuild g in BaseGuild.List.Values )
			{
                                list.Add( g );
			}

			return list;
		}

		private void Initialize( int page )
		{
			m_Page = page;

			int count = m_List.Count - (page * EntryCount);

			if ( count < 0 )
				count = 0;
			else if ( count > EntryCount )
				count = EntryCount;
				
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			AddBackground(0, 0, 300, 400, 9250);
			AddLabel(60, 18, 0x480, "ADMINISTRACION DE GUILDS");
			AddAlphaRegion(20, 50, 260, 290);

			AddButton(113, 355, 4017, 4018, 0, GumpButtonType.Reply, 0);
			AddLabel(145, 355, 0x480, "SALIR");


			if( count == 0 )
                        	AddLabel(65, 55, 0, "No se encontraron guilds para listar.");

			if ( page > 0 )
			{
				AddButton(23, 355, 4014, 4015, 1, GumpButtonType.Reply, 0);
				AddLabel(55, 355, 0x480, "PREV");
			}

			if ( (page + 1) * EntryCount < m_List.Count )
			{
				AddButton(218, 355, 4005, 4006, 2, GumpButtonType.Reply, 0);
				AddLabel(250, 355, 0x480, "SIG");
			}
			
			for ( int i = 0, index = page * EntryCount; i < EntryCount && index < m_List.Count; ++i, ++index )
			{
				Guild g = m_List[index] as Guild;
				
				if( g != null )
				{
					AddButton(30, 55 + ( 20 * i ), 4008, 4009, i + 3, GumpButtonType.Reply, 0);

					try
					{
						AddLabelCropped( 65, 55 + ( 20 * i ), 200, 20, 0x480, String.Format( "{0} ({1})", g.Name, g.Abbreviation ) );
					}
					catch
					{
                                         	AddLabelCropped( 65, 55 + ( 20 * i ), 220, 20, 0x480, "No Definido" );
                                        }
                                }
                        }
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 0:
				{
					return;
				}
				case 1:
				{
					if ( m_Page > 0 )

						from.SendGump( new GuildAdminGump( from, m_Page - 1 ) );

					break;
				}
				case 2:
				{
					if ( (m_Page + 1) * EntryCount < m_List.Count )
						from.SendGump( new GuildAdminGump( from, m_Page + 1 ) );

					break;
				}
				default:
				{
					int index = (m_Page * EntryCount) + (info.ButtonID - 3);

					if ( index >= 0 && index < m_List.Count )
					{
                                        	from.SendGump( new GuildAdminGump( from, m_Page ) );
						from.SendGump( new GuildAdminInfoGump( from, m_List[index] as Guild ) );
					}

					break;
				}
			}
		}
	}
}
