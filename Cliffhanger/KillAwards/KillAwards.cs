using System;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Misc
{
	public class KillAwards
	{
	        public static void Initialize()
		{
			Server.Commands.Register( "KillAwards", AccessLevel.Player, new CommandEventHandler( KillAwards_OnCommand ) );
		}
		
		private static void KillAwards_OnCommand( CommandEventArgs e )
		{
			if( !( e.Mobile is dovPlayerMobile ) )
				return;
				
			e.Mobile.SendGump( new KillAwardsGump( e.Mobile ) );
		}

		public class KillAwardsGump : Gump
	        {
	        	dovPlayerMobile m_From;
	        	
	        	public KillAwardsGump( Mobile from ) : base( 20, 20 )
	        	{
                                from.CloseGump( typeof( KillAwardsGump ) );
                                
                                m_From = from as dovPlayerMobile;

				Closable = false;
				Disposable = false;
				Dragable = true;
				Resizable = false;

				AddPage(0);

				AddBackground(0, 0, 300, 190, 9250);
				AddImageTiled(20, 40, 255, 100, 2624);
				AddAlphaRegion(20, 40, 255, 100);

				AddLabel(98, 17, 0x480, "KILL AWARDS!");

				AddButton(118, 150, 4014, 4015, 0, GumpButtonType.Reply, 0);
				AddLabel(150, 150, 0x480, "OK");
				
				if( m_From != null )
				{
					AddLabel(45, 50, 54, "Tus puntos de experiencia:");
					AddLabel(212, 50, 0x480, m_From.ExpPoints.ToString());

					int nivel = ( m_From.ExpPoints / 10 );
					int x = 50;

					for( int i = 0; i < 10 ; i++ )
					{
						if( i < nivel )
							AddImage(x, 80, 1209, 97);
						else
							AddImage(x, 80, 1209, 0);
							
						x += 20;
					}

					AddLabel(27, 105, 54, "Puntos para obtener PropStone:");
					AddLabel(233, 105, 0x480, (50 - m_From.ExpCounter).ToString() );
				}
			}
	        }

		public static void HandleDeath( Mobile killer, Mobile victim )
	        {
	        	if( killer == null || victim == null || !( victim is dovPlayerMobile ) )
	        		return;
	        		
	        	if( killer is BaseCreature )
	        	{
	        		Mobile owner = ((BaseCreature)killer).ControlMaster;
	        		
	        		if( owner == null )
	        		{
	        			owner = ((BaseCreature)killer).SummonMaster;
	        			
	        			if( owner == null )
	        				return;
					else
					{
						killer = owner;	
						
						if( !( killer is dovPlayerMobile ) )
							return;
					}
				}
				else
				{
					killer = owner;
					
					if( !( killer is dovPlayerMobile ) )
							return;
				}
			}
			else
			{
				if( !( killer is dovPlayerMobile ) || ((dovPlayerMobile)killer).Neutral )
					return;
			}

	        	if( IsPK( killer ) )
	        	{
	        		if( IsPK( victim ) && killer.Guild != null && killer.Guild == victim.Guild )
	        			return;
	        			
	        		ManageExpPoints( killer , victim );
	        	}
	        	else
	        	{
				if( !IsPK( victim ) )
				{
					Guild kguild = killer.Guild as Guild;
	        			Guild vguild = victim.Guild as Guild;

	        			if( kguild == null || vguild == null || kguild == vguild || !kguild.IsWar( vguild ) )
                                        	return;
                                }

                                ManageExpPoints( killer , victim );
	        	}
	        }
	        
	        private static bool IsPK( Mobile m )
	        {
	        	return ( m != null && m.Kills >= 5 );
	        }
	        
	        private static void ManageExpPoints( Mobile killer, Mobile victim )
	        {
                	if( killer == null || victim == null || !( killer is dovPlayerMobile ) || !( victim is dovPlayerMobile ) )
	        		return;
	        		
			dovPlayerMobile dkiller = killer as dovPlayerMobile;
			dovPlayerMobile dvictim = victim as dovPlayerMobile;

			bool giveKillerPoints = true;

			if( ( dkiller.LastExpVictim == victim ) && ( DateTime.Now < dkiller.LastExpGain + TimeSpan.FromMinutes( 5.0 ) ) )
				giveKillerPoints = false;
				
			if( dvictim.ExpPoints + 75 < dkiller.ExpPoints )
                        	giveKillerPoints = false;
			
			if( giveKillerPoints && dkiller.ExpPoints < 100 )
			{
                         	if( ( ( dkiller.ExpPoints / 10 ) < ( ( dkiller.ExpPoints + 1 ) / 10 ) ) )
                                	killer.SendMessage( "Has obtenido un nuevo titulo de experiencia!" );
                                else
                                        killer.SendMessage( "Has ganado mayor grado de experiencia." );

				dkiller.ExpPoints += 1;
                        	dkiller.ExpCounter += 1;

                        	dkiller.LastExpVictim = victim;
                                dkiller.LastExpGain = DateTime.Now;

                                if( dkiller.ExpCounter >= 50 )
				{
                                	dkiller.ExpCounter = 0;
                                	GivePropStoneTo( killer );
                                }
                        }

                        if( dkiller.ExpPoints > 0 )
			{
				if( ( dvictim.ExpPoints / 10 ) > ( dvictim.ExpPoints - 2 ) / 10 )
                                	victim.SendMessage( "Has perdido un nivel de titulo de experiencia!" );
                        	else
                                	victim.SendMessage( "Has perdido nivel de experiencia." );

				dvictim.ExpPoints -= 2;
                        	dvictim.ExpCounter -= 2;
                        }
	        }
	        
	        public static void GivePropStoneTo( Mobile m )
	        {
	        	if( m == null )
	        		return;
	        		
	        	PropStone stone = new PropStone();

	        	Container pack = m.Backpack;

			if ( pack == null || !pack.TryDropItem( m, stone, false ) )
				m.BankBox.DropItem( stone );

			m.SendMessage( 0x23, "Por el exito y talento que has demostrado tener en batalla, has sido recompensado con una Piedra de Propiedad." );
	        }
	}
}
