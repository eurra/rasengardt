// Cosas generales de Subcaps.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.SkillHandlers
{
	public class SubCaps
	{
		private static SkillName[] m_PvPSkills = new SkillName[]
		{
			// Aca se agregan las skills consideradas para el balance.
			SkillName.Parry,
                        SkillName.EvalInt,
                        SkillName.Healing,
                        SkillName.Inscribe,
                        SkillName.Lumberjacking,
                        SkillName.Magery,
                        SkillName.MagicResist,
                        SkillName.Tactics,
                        SkillName.Anatomy,
                        SkillName.Poisoning,
                        SkillName.Stealth,
                        SkillName.Archery,
                        SkillName.SpiritSpeak,
                        SkillName.Stealing,
                        SkillName.Snooping,
                        SkillName.Swords,
                        SkillName.Macing,
                        SkillName.Fencing,
                        SkillName.Wrestling,
                        SkillName.Meditation,
                        SkillName.Necromancy,
                        SkillName.Focus,
                        SkillName.Chivalry,
                        SkillName.Bushido,
                        SkillName.Ninjitsu
  		};
		
		public static SkillName[] PvPSkills{ get{ return m_PvPSkills; } }

		public static void Initialize()
		{
			Server.Commands.Register( "Subcap", AccessLevel.Player, new CommandEventHandler( Subcap_OnCommand ) );
		}
		
		private static void Subcap_OnCommand( CommandEventArgs e )
		{
			if( !( e.Mobile is dovPlayerMobile ) )
				return;
				
			e.Mobile.SendGump( new SubCapGump( e.Mobile ) );
		}
		
		private class SubCapGump: Gump
		{
			private dovPlayerMobile m_From;
			private const int htmlColor = 0xFFFFFF;
			
			public SubCapGump( Mobile from ) : base( 0, 0 )
			{
				m_From = (dovPlayerMobile)from;
				
				m_From.CloseGump( typeof( SubCapGump ) );

				string Estado;
				int total = TotalSkillSubCap( m_From );

				if( total > m_From.PvPCapReal )
					Estado = String.Format("<BASEFONT COLOR=#{0:X6}>Tus skills de PvP estan sobrepasados del Cap que tienen establecido.</BASEFONT>", htmlColor);
				else if( total == m_From.PvPCapReal )
					Estado = String.Format("<BASEFONT COLOR=#{0:X6}>Tus skills de PvP estan justo en el Cap que tienen establecido.</BASEFONT>", htmlColor);
				else
					Estado = String.Format("<BASEFONT COLOR=#{0:X6}>Tus skills de PvP estan en orden con el Cap establecido.</BASEFONT>", htmlColor);

				Closable = false;
				Disposable = false;
				Dragable = true;
				Resizable = false;
				
				AddPage(0);
				AddBackground(75, 86, 333, 297, 9200);
				AddImageTiled(87, 97, 309, 274, 2624);
				AddAlphaRegion(87, 97, 309, 274);
				AddLabel(126, 121, 154, @"** INFORMACION DE SKILLS PvP **");
				AddLabel(110, 185, 154, @"Estado Actual:");
				AddHtml( 211, 168, 167, 54, Estado, false, false );
				AddLabel(110, 265, 154, @"Total Skills PvP:");
				AddLabel(110, 295, 154, String.Format("Diferencia con Cap ({0:0.0}):", ( ( (double)m_From.PvPCapReal ) /10.0 ) ) );
				AddLabel(295, 265, 0x480, String.Format("{0:0.0}", ( ( (double)total) / 10.0 ) ) );
				AddLabel(295, 295, 0x480, String.Format("{0:0.0}", ( ( (double)m_From.PvPCapReal / 10.0 ) - ( (double)total / 10.0 )) ) );
				AddButton(230, 338, 4023, 4024, 0, GumpButtonType.Reply, 0);

			}
		}

		public static int TotalSkillSubCap( Mobile m )
		{
			int total = 0;

			for( int i = 0; i < m_PvPSkills.Length; i++ )
                        	total += m.Skills[m_PvPSkills[i]].BaseFixedPoint;

                        return total;
  		}

  		public static bool IsPvPSkill( SkillName origSkill )
  		{
                	for( int i = 0; i < m_PvPSkills.Length; i++ )
                	{
                        	if( origSkill == m_PvPSkills[i] )
                        		return true;
                 	}

                        return false;
    		}
    		
 		public static bool EnSubCap( Mobile from )
  		{
                	if( !( from is dovPlayerMobile ) )
                		return false;
                	
                	if( ((dovPlayerMobile)from).SkillsPvPTotal >= ((dovPlayerMobile)from).PvPCapReal )
                		return true;
                	
                	return false;
    		}
    		
    		public static bool AllowGain( Mobile from, SkillName skill, int toGain )
    		{
                	if( !IsPvPSkill( skill ) )
                		return true;

			if( !( from is dovPlayerMobile ) )
                		return true;

                	if( ((dovPlayerMobile)from).SkillsPvPTotal + toGain <= ((dovPlayerMobile)from).PvPCapReal )
                		return true;
                	
                	return false;
    		}
    		
		public static void Advertencias( dovPlayerMobile m, int prevSkillsPvPTotal )
		{
			if( ( ( m.SkillsPvPTotal < m.PvPCapReal - 500 ) && ( prevSkillsPvPTotal < m.PvPCapReal - 500 ) ) || ( ( prevSkillsPvPTotal > m.PvPCapReal ) && ( m.SkillsPvPTotal > m.PvPCapReal ) ) )
                        	return;

                        /*if( ( prevSkillsPvPTotal >= m.PvPCapReal ) && ( m.SkillsPvPTotal < m.PvPCapReal ) )
                        {
                        	m.SendMessage( 0x23, "Atencion, tus skills PvP han bajado de su cap de {0}, ahora puedes seguir subiendolos.", (double)m.PvPCapReal/10 );
                        	return;
                        }

			if( ( prevSkillsPvPTotal < m.PvPCapReal ) && ( m.SkillsPvPTotal >= m.PvPCapReal ) )
                        {
                        	m.SendMessage( 0x23, "Atencion, tus skills PvP han alcanzado su cap de {0}, no podras seguir subiendolos.", (double)m.PvPCapReal/10 );
                        	return;
                        } */

                        if( ( prevSkillsPvPTotal < m.PvPCapReal - 250 ) && ( m.SkillsPvPTotal >= m.PvPCapReal - 250 ) )
                        {
                        	m.SendMessage( 0x23, "Atencion, tus skills PvP estan muy cerca de su cap de {0}, al llegar a este cap, no podras seguir subiendolos.", (double)m.PvPCapReal/10 );
                        	return;
                        }

                        if( ( prevSkillsPvPTotal < m.PvPCapReal - 500 ) && ( m.SkillsPvPTotal >= m.PvPCapReal - 500 ) )
                        {
                        	m.SendMessage( 0x23, "Atencion, tus skills PvP se estan acercando a su cap de {0}, te recomendamos estar atento a ellos.", (double)m.PvPCapReal/10 );
                        	return;
                        }
		}
	}
}

