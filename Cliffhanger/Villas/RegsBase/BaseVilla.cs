// Region de Villa.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Regions
{
	public class BaseVilla : Region
	{
		private static object[] m_GuardParams = new object[1];
		private ArrayList m_GuardCandidates = new ArrayList();
		private Type m_GuardType;

		public Type GuardType{ get{ return m_GuardType; } }
		public ArrayList GuardCandidates{ get{ return m_GuardCandidates; } }

		public BaseVilla( string prefix, string name, Map map, Type guardType ) : base( prefix, name, map )
		{
			m_GuardType = guardType;
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return true;
		}
		
		public override void MakeGuard( Mobile focus )
		{
			bool Encontro = false;

			foreach ( Mobile m in focus.GetMobilesInRange( 8 ) )
			{
				if ( m is BaseVillaGuard )
				{
					BaseVillaGuard g = (BaseVillaGuard)m;

					if ( g.Focus == null ) // idling
					{
						Encontro = true;
                                                g.Focus = focus;
						break;
					}
					else if( g.Focus == focus )
					{
                                        	Encontro = true;
                                        	break;
     					}
				}
			}

			if( !Encontro )
				SpawnGuard( focus );
		}

		public void SpawnGuard( Mobile focus )
		{
			m_GuardParams[0] = focus;

			Activator.CreateInstance( m_GuardType, m_GuardParams );
		}

                public virtual String OnEnterMessage( Mobile m )
                {
			return "Bienvenido a la villa " + Name + ".";
                }

                public virtual bool IsGuardCandidate( Mobile m, bool onEnter )
                {
                	if( m == null )
                		return false;
                	
                	if( !onEnter )
                	{
				if( m_GuardCandidates.Contains( m ) )
                                	return true;
			} 

                        return ( m.Kills >= 5 || ( m is BaseCreature && IsPetCandidate( m as BaseCreature, true ) ) );
                }
                
 		public virtual bool IsPetCandidate( BaseCreature c, bool onEnter )
		{
			Mobile owner = c.ControlMaster;

			if( owner == null )
				return false;

			if( !onEnter )
			{
                                if( IsGuardCandidate( owner, false ) )
                                	return true;
   			}
   			
                        return ( IsGuardCandidate( owner, true ) );
		}

                private bool InmuneToGuards( Mobile m )
                {
                	return ( m is BaseVillaGuard || !m.Alive || m.AccessLevel > AccessLevel.Player || m.Blessed );
                }

		public override void OnEnter( Mobile m )
		{
			if( !InmuneToGuards( m ) && IsGuardCandidate( m, true ) )
			{
                        	if( !m_GuardCandidates.Contains( m ) )
                                	m_GuardCandidates.Add( m );
                                
                                m.SendMessage("Si te ven, los guardias te atacaran!");
			}
			else
				m.SendMessage( OnEnterMessage( m ) );
		}

                public virtual String OnExitMessage()
                {
                     return "Has dejado la villa " + Name + ".";
                }

		public override void OnExit( Mobile m )
		{
                 	if( m_GuardCandidates.Contains( m ) )
                                m_GuardCandidates.Remove( m );

			m.SendMessage( OnExitMessage() );
		}
		
		public void RemoveCandidate( Mobile m )
		{
			if( !m_GuardCandidates.Contains( m ) )
				return;
			
			if( !IsGuardCandidate( m, true ) )
                                m_GuardCandidates.Remove( m );
  		}              

		public override void OnAggressed( Mobile aggressor, Mobile aggressed, bool criminal )
		{
			base.OnAggressed( aggressor, aggressed, criminal );

			if ( aggressed is BaseVillaGuard && aggressed.Region == aggressor.Region )
			{
				CheckGuardCandidate( aggressor );
			}
		}

		public override void OnSpeech( SpeechEventArgs args )
		{
			if ( args.Mobile.Alive && args.HasKeyword( 0x0007 ) ) // guards
			{
				if( IsGuardCandidate( args.Mobile, true ) )
					return;

				if( !CallGuards( args.Mobile.Location ) )
					GuardError( args.Mobile, "No veo a ningun intruso, deja de molestar!" );
			}
		}

		public void GuardError( Mobile caller, String s )
		{
			BaseVillaGuard useGuard = null;

			foreach ( Mobile m in caller.GetMobilesInRange( 8 ) )
			{
				if ( m is BaseVillaGuard )
				{
					BaseVillaGuard g = (BaseVillaGuard)m;

					if ( g.Focus == null ) // idling
					{
						useGuard = g;
						break;
					}
				}
			}

			if ( useGuard != null )
			{
				useGuard.Say( s );
			}
		}

 		public void CheckGuardCandidate( Mobile m )
		{
			if ( !m_GuardCandidates.Contains( m ) )
			{
                         	m_GuardCandidates.Add( m );
                         	m.SendMessage("Has sido alertado a los guardias!");
   			}
                                  
			MakeGuard( m );
		}

		public bool CallGuards( Point3D p )
		{
			bool encontro = false;

			foreach ( Mobile m in Map.GetMobilesInRange( p, 14 ) )
			{
				if ( !InmuneToGuards( m ) && IsGuardCandidate( m, false ) )
				{
					CheckGuardCandidate( m );
					encontro = true;
					break;
				}
			}

			return encontro;
		}
	}
}
