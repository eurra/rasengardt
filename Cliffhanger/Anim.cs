// Comando Anim (Jugo).
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public class AnimCommand
	{
		public static void Initialize()
		{
			Server.Commands.Register( "Anim", AccessLevel.Player, new CommandEventHandler( Anim_OnCommand ) );
		}

		[Usage( "Anim <x>" )]
		[Description( "Hace que el jugador ejecute alguna animacion" )]
		private static void Anim_OnCommand( CommandEventArgs e )
		{
			if ( e.Length == 1 )
                        {
                        	e.Mobile.Animate( e.GetInt32( 0 ), 7, 1, true, false, 0 );
                        }
                        else
                	{
                                e.Mobile.SendMessage("Uso: Anim [nro_animacion]");
                 	}
  		} 
	}
}
