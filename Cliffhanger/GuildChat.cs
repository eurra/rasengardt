using System;
using System.Text; 
using System.Collections; 
using Server; 
using Server.Network; 
using Server.Guilds; 
using Server.Mobiles; 

namespace Server.Scripts.Commands 
{ 
	public class GuildChat
	{
		public static void Initialize()
		{
			Server.Commands.Register( "g", AccessLevel.Player, new CommandEventHandler( GC_OnCommand ) );
		}

		[Usage( "GC [text>|list]" )]
		[Description( "Broadcasts a message to all online members of your guild." )]
		private static void GC_OnCommand( CommandEventArgs e )
		{
                        Guild guild = (Guild)e.Mobile.Guild;

                        if( guild == null )
                        {
                        	e.Mobile.SendMessage( "No estas en una guild, por lo que no puedes usar este comando." );
                        	return;
                        }
                        
                        if( e.Length < 1 )
                        {	
				e.Mobile.SendMessage( "Uso: \".G <mensaje>\"" );
                        	return;
                        }

			Msg ( e, guild );
		}

		private static void Msg( CommandEventArgs e, Guild guild )
		{
			Mobile from = e.Mobile;
			ArrayList list = new ArrayList();

			foreach( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if ( m != null && guild.IsMember( m ) )
					list.Add( m );
			}
			
			if( list.Count == 0 || ( list.Count == 1 && list[0] == from ) )
			{
				from.SendMessage( "No hay miembros de tu guild conectados.");
			}
			else
			{
				foreach( Mobile m in list )
					m.SendMessage( 0x2C, String.Format( "Guild[ {0} ]: {1}", from.Name, e.ArgString ) );
			}
		}
	}
}
