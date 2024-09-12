// Region PK.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Spells;

namespace Server.Regions
{
	public class PKRegion : BaseVilla
	{

		public PKRegion( string prefix, string name, Map map, Type guardType ) : base( prefix, name, map, guardType )
		{
		}

                public override String OnEnterMessage( Mobile m )
                {
                	return "Bienvenido a tu escondite, ahora estas en protección de los Guardias Asesinos.";
                }
                
                public override bool IsGuardCandidate( Mobile m, bool onEnter )
                {
                	if( m == null )
                		return false;

			return ( ( m.Player && !AccountHasPK( m ) && m.Kills < 5 ) || ( m is BaseCreature && IsPetCandidate( m as BaseCreature, true ) ) );
                }

 		public override bool IsPetCandidate( BaseCreature c, bool onEnter )
		{
			Mobile owner = c.ControlMaster;

			if( owner == null )
				return false;
				
			return ( IsGuardCandidate( owner, true ) );
		}
		
		private bool AccountHasPK( Mobile m )
		{
                	Account acct = m.Account as Account;

                	if ( acct == null )
				return false;

			for( int i=0; i<5; i++ )
			{
				Mobile n = acct[i];

				if( n != null && n.Kills >= 5 )
					return true;
			}
			
			return false;
		 
		}

                public override String OnExitMessage()
                {
                     return "Has dejado zona de Guardias Asesinos";
                }

		public override void OnSpeech( SpeechEventArgs args )
		{
			if ( args.Mobile.Alive && args.HasKeyword( 0x0007 ) ) // guards
			{
				if( !CallGuards( args.Mobile.Location ) )
					GuardError( args.Mobile, "No veo a ningun intruso, deja de molestar!" );
			}
			else if ( !args.Mobile.Alive )
			{
                        	GuardError( args.Mobile, "Hay un alma intranquila por aqui, pero no puedo oir que dice!" );
			}
		}
	}
}
