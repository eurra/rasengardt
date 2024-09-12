using System;
using System.Collections;
using Server.Accounting;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
	public class CheckPjsGump : Gump
	{
		public static void Initialize()
		{
			Commands.Register( "CheckPjs", AccessLevel.Administrator, new CommandEventHandler( CheckPjs_OnCommand ) );
		}

		[Usage( "CheckPjs" )]
		[Description( "Revisa las cuentas con irregularidades." )]
		private static void CheckPjs_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendGump( new CheckPjsGump( e.Mobile, Problema.MuchosPjs ) );
		}
		
		public enum Problema
		{
                	MuchosPjs,
                	PjsSobreSubCap
                }

		private Mobile m_Owner;
		private ArrayList m_List;
		private int m_Page;
		private Problema m_Prob;

		public CheckPjsGump( Mobile owner, Problema p ) : this( owner, BuildList( p ), 0, p )
		{
		}

		public CheckPjsGump( Mobile owner, ArrayList list, int page, Problema p ) : base( 0, 0 )
		{
			owner.CloseGump( typeof( CheckPjsGump ) );

			m_Owner = owner;
			m_List = list;
			m_Prob = p;

			Initialize( page );
		}

		public static ArrayList BuildList( Problema p )
		{
			ArrayList list = new ArrayList();

			if( p == Problema.MuchosPjs )
			{
				foreach( Account acc in Accounts.Table.Values )
				{
                                        if( acc.AccessLevel >= AccessLevel.Counselor )
						continue;

					if( acc.Count > 2 )
						list.Add( acc );
				}
			}
			else if( p == Problema.PjsSobreSubCap )
			{
				foreach( Mobile m in World.Mobiles.Values )
				{
                                        if( m.AccessLevel >= AccessLevel.Counselor )
						continue;

					if( ( m is dovPlayerMobile ) && ( ((dovPlayerMobile)m).SkillsPvPTotal > ((dovPlayerMobile)m).PvPCapReal ) )
						list.Add( m );
				}
			}

			return list;
		}

		private const int EntryCount = 7;

		public void Initialize( int page )
		{
			m_Page = page;

			int count = m_List.Count - (page * EntryCount);

			if ( count < 0 )
				count = 0;
			else if ( count > EntryCount )
				count = EntryCount;

			Closable = false;
			Dragable = true;
			Resizable = false;

			AddPage( 0 );

			AddBackground(0, 0, 400, 430, 9200);
			AddImageTiled(15, 15, 370, 400, 2624);
			AddAlphaRegion(15, 15, 370, 400);
			
			if( count > 0 )
			{
				AddImageTiled(15, 60, 370, 15, 9204);
				AddImageTiled(225, 75, 15, 240, 9204);
				AddImageTiled(15, 315, 370, 15, 9204);
				AddBackground(15, 75, 210, 30, 9200);
				AddBackground(240, 74, 145, 30, 9200);
			}
			else
			{
                        	AddLabel(70, 83, 154, "No se encontraron elementos con problemas.");
   			}
			
			switch( m_Prob )
			{
				case Problema.MuchosPjs :
				{
					AddLabel(63, 29, 154, "CUENTAS CON MAS DE DOS PERSONAJES");
					
					if( count > 0 )
					{
						AddLabel(70, 83, 0x480, "Nombre Cuenta");
						AddLabel(260, 80, 0x480, "Borrar último pj");
					}

					AddButton(30, 380, 4014, 4015, 3, GumpButtonType.Reply, 0);
					AddLabel(70, 380, 0x480, "Ver personajes con SubCap sobrepasado");
					
					break;
				}
				case Problema.PjsSobreSubCap :
				{
					AddLabel(63, 29, 154, "PERSONAJES CON SUBCAP SOBREPASADO");

					if( count > 0 )
					{
						AddLabel(60, 83, 0x480, "Nombre pj (cuenta)");
						AddLabel(260, 80, 0x480, "Propiedades del pj");
					}
					
					AddButton(30, 380, 4014, 4015, 3, GumpButtonType.Reply, 0);
					AddLabel(70, 380, 0x480, "Ver cuentas con mas de 2 personajes.");
					
					break;
				}
			}

			AddButton(162, 350, 4017, 4018, 0, GumpButtonType.Reply, 0);
			AddLabel(200, 350, 0x480, "SALIR");

			if ( page > 0 )
			{
				AddButton(30, 350, 4014, 4015, 1, GumpButtonType.Reply, 0);
				AddLabel(70, 350, 0x480, "PREV");
			}

			if ( (page + 1) * EntryCount < m_List.Count )
			{
				AddButton(335, 350, 4005, 4006, 2, GumpButtonType.Reply, 0);
				AddLabel(300, 350, 0x480, "SIG");
			}
			
			int x, y;
			
			for ( int i = 0, index = page * EntryCount; i < EntryCount && index < m_List.Count; ++i, ++index )
			{
				x = 30;
				y = 105;

				switch( m_Prob )
				{
					case Problema.MuchosPjs :
					{
						Account acc = (Account)m_List[index];
					
						if( acc != null )
							AddLabelCropped( x, y + ( 30 * i ), 180, 30, 0x480, acc.ToString() );

						break;
					}
					case Problema.PjsSobreSubCap :
					{
                                         	Mobile mob = (Mobile)m_List[index];
					
						if( mob != null )
							AddLabelCropped( x, y + ( 30 * i ), 180, 30, 0x480, ( mob.Name != null ? mob.Name : "null" ) + " " + ( mob.Account != null && mob.Account is Account ? "(" + ((Account)mob.Account).ToString() + ")" : "(sin cuenta)" ) );
						
						break;
					}
				}

				x = 295;

				AddButton( x, y + ( 30 * i ), 4008, 4009, i + 4, GumpButtonType.Reply, 0 );
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
						from.SendGump( new CheckPjsGump( from, m_List, m_Page - 1, m_Prob ) );

					break;
				}
				case 2:
				{
					if ( (m_Page + 1) * EntryCount < m_List.Count )
						from.SendGump( new CheckPjsGump( from, m_List, m_Page + 1, m_Prob ) );

					break;
				}
				case 3:
				{
					switch( m_Prob )
					{
						case Problema.MuchosPjs :
						{
							from.SendGump( new CheckPjsGump( from, Problema.PjsSobreSubCap ) );
							break;
						}
						case Problema.PjsSobreSubCap :
						{
                                         		from.SendGump( new CheckPjsGump( from, Problema.MuchosPjs ) );
                                         		break;
						}
					}

					break;
				}
				default:
				{
					int index = (m_Page * EntryCount) + (info.ButtonID - 4);

					if ( index >= 0 && index < m_List.Count )
					{
						switch( m_Prob )
						{
							case Problema.MuchosPjs :
							{
								BorrarUltimoPj( (Account)m_List[index], from );
                                                	
                                                		m_List = BuildList( m_Prob );
								from.SendGump( new CheckPjsGump( from, m_List, m_Page, m_Prob ) );
								
								break;
							}
							case Problema.PjsSobreSubCap :
							{
                                         			from.SendGump( new CheckPjsGump( from, m_List, m_Page, m_Prob ) );

								Mobile m = (Mobile)m_List[index];

								if( m != null )
									from.SendGump( new AdminGump( from, AdminGumpPage.ClientInfo, 0, null, null, m ) );

                                         			break;
							}
						}
					}

					break;
				}
			}
		}
		
		private void BorrarUltimoPj( Account acc, Mobile from )
		{
                	if( acc == null || from == null )
                		return;
                	
                        Mobile m = null;

			for( int i = 0; i < acc.Length; i++ )
			{
				if( acc[i] != null && !acc[i].Deleted )
                                        m = acc[i];
      			}

			if( m == null )
			{
                        	from.SendMessage( "La cuenta no tiene pjs." );
                        	return;
                        }
                        else if( m.NetState != null )
                        {
                        	from.SendMessage( "El personaje a borrar ({0}) esta online.", ( m.Name != null ? m.Name : "null" ) );
                        	return;
			}
			else
			{
    				string name = m.Name;

				m.Delete();

                                from.SendMessage( "Personaje \"{0}\" de la cuenta \"{1}\" borrado.", ( name != null ? name : "null" ), acc.ToString() );
                        }
		}
	}
}
