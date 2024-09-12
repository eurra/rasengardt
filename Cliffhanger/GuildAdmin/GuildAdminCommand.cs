using System;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Scripts.Commands
{
	public class GuildAdminCommand
	{
		public static void Initialize()
		{
			Server.Commands.Register( "GuildAdmin", AccessLevel.Counselor, new CommandEventHandler( GuildAdmin_OnCommand ) );
		}

		[Usage( "GuildAdmin" )]
		[Description( "Administra las Guilds." )]
		private static void GuildAdmin_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendGump( new GuildAdminGump( e.Mobile ) );
		}
	}
}
