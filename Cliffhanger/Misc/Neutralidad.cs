// Cosas generales de Neutralidad.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Mobiles
{
	public class ProtPointsTimer : Timer
	{
		// Delay de la perdida de puntos de proteccion.
		public static readonly TimeSpan ProtPointsLostDelay = TimeSpan.FromMinutes( 4.0 );

		public static void Initialize()
		{
			new ProtPointsTimer().Start();
		}

		public ProtPointsTimer() : base( ProtPointsLostDelay, ProtPointsLostDelay )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			foreach ( NetState state in NetState.Instances )
			{
				if( state.Mobile is dovPlayerMobile )
                                	ProtPointsDecay( state.Mobile as dovPlayerMobile );
			}
		}

		public static void ProtPointsDecay( dovPlayerMobile m )
      		{
      			if( ( m.ProtectionPoints > -Neutralidad.RangoPP/2 ) && ( m.LastProtectionPointsLost + ProtPointsLostDelay < DateTime.Now ) )
                        {
                                m.ProtectionPoints -= 10;
                                	
                                if( m.ProtectionPoints < -Neutralidad.RangoPP/2 )
                                        m.ProtectionPoints = -Neutralidad.RangoPP/2;

                                m.LastProtectionPointsLost = DateTime.Now;
                        }
                }
	}

	public class Neutralidad
	{
		// Este es el rango en que los puntos de proteccion se mueven.
		public static readonly int RangoPP = 500;
		
		private static bool ValidCreature( BaseCreature c )
		{
			return ( /*!(c is BaseVillaGuard) &&*/ !c.Summoned && !c.Controled );
		}

		public static void AsignarPuntosDeProteccion( dovPlayerMobile m, BaseCreature c )
		{
      			if( !ValidCreature( c ) )
      				return;

			m.LastProtectionPointsLost = DateTime.Now;

			if( m.ProtectionPoints >= Neutralidad.RangoPP )
      				return;

			int sumastats = c.Str + c.Int + c.Dex;
        		
        		if( sumastats > 2500 )
        			sumastats = 2500;
        		else if( sumastats < 250 )
                                sumastats = 250;

        		double bonus = ( 1 / ((double)m.SkillsPvPTotal/10000) );

			int points = ( sumastats/250 ) + (int)bonus;

			m.ProtectionPoints += points;
			
			m.SendMessage("Has obtenido {0} punto{1} de proteccion.", points, ( points > 1 ? "s" : "" ) );
		}
		
		public static void AsignarPuntosDeProteccion( dovPlayerMobile m, CraftSystem craftSystem, CraftItem item )
		{
                	double skill = m.Skills[craftSystem.MainSkill].Base;

                        if( skill < 50.0 )
                        	return;
	 
	 		m.LastProtectionPointsLost = DateTime.Now;

			if( m.ProtectionPoints >= Neutralidad.RangoPP )
      				return;

      			double minskill = 0.0;
			double maxskill = 0.0;

			for ( int i = 0; i < item.Skills.Count; i++ )
			{
				CraftSkill craftSkill = item.Skills.GetAt(i);

				if ( craftSkill.SkillToMake == craftSystem.MainSkill )
				{
					minskill = craftSkill.MinSkill;
					maxskill = craftSkill.MaxSkill;
					break;
				}
			}

			double promskill = (minskill + maxskill)/2;

			int points = (int)( 4*(promskill/skill) );
			
			if( points < 1 )
				points = 1;

			m.ProtectionPoints += points;

			m.SendMessage("Has obtenido {0} punto{1} de proteccion.", points, ( points > 1 ? "s" : "" ) );
		}

		public static bool EsAtaquePKNeutral( Mobile from, Mobile to )
		{
			if( ( EsPlayerNeutral( to ) || EsMascotaNeutral( to ) ) && ( EsPlayerPK( from ) || EsMascotaPK( from ) ) )
				return true;

			return false;
		}
		
		public static bool EsAtaqueNeutralPK( Mobile from, Mobile to )
		{
			if( ( EsPlayerNeutral( from ) || EsMascotaNeutral( from ) ) && ( EsPlayerPK( to ) || EsMascotaPK( to ) ) )
				return true;
				
			return false;
		}
		
		private static bool EsPlayerNeutral( Mobile m )
		{
			if( m == null )
				return false;

			if( !( m.Player && m is dovPlayerMobile ) )
				return false;

			if( ((dovPlayerMobile)m).EsNeutralActivo )
				return true;
				
			return false;
		}
		
		private static bool EsMascotaNeutral( Mobile m )
		{
			if( m == null )
				return false;
				
			if( !( m is BaseCreature ) )
				return false;
				
                        BaseCreature c = (BaseCreature)m;
                        
                        if( c.Controled && EsPlayerNeutral( c.ControlMaster ) )
                        	return true;	//Summons y criaturas controladas.
                        	
                        if( c.Summoned && EsPlayerNeutral( c.SummonMaster ) )
                        	return true;	//Summons no controlados.

			return false;
		}
		
		private static bool EsPlayerPK( Mobile m )
		{
			if( m == null )
				return false;
				
			if( !m.Player )
				return false;

			if( m.Kills >= 5 )
				return true;
				
			return false;
		}
		
		private static bool EsMascotaPK( Mobile m )
		{
			if( m == null )
				return false;
				
			if( !( m is BaseCreature ) )
				return false;
				
                        BaseCreature c = (BaseCreature)m;
                        
                        if( c.Controled && EsPlayerPK( c.ControlMaster ) )
                        	return true;
                        	
                        if( c.Summoned && EsPlayerPK( c.SummonMaster ) )
                        	return true;

			return false;
		}
	}
}

