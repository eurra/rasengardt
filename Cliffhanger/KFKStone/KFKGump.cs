// Gump de Piedra de Karma-Fama-Kills
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class KFKGumpI : Gump
	{
                private KFKStone m_Stone;

		public KFKGumpI( Mobile from, KFKStone stone ) : base(0,0)
		{

			m_Stone = stone;

			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(84, 75, 326, 252, 9250);
			this.AddLabel(142, 101, 150, @"PIEDRA DE KARMA/FAMA/KILLS");
			this.AddHtml( 139, 128, 215, 123, @"Esta piedra te permitira guardar tus valores actuales de Karma, Fama y Kills. Una vez usada, puedes participar en un evento y terminado este, volver a usar esta piedra, para recuperar estos valores.", (bool)true, (bool)true);
			this.AddButton(162, 261, 4005, 4006, 1, GumpButtonType.Reply, 0);
			this.AddLabel(196, 263, 150, @"GUARDAR MIS VALORES");
			this.AddButton(162, 286, 4005, 4006, 2, GumpButtonType.Reply, 0);
			this.AddLabel(196, 287, 150, @"SALIR");

		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
                        Mobile from = state.Mobile;

			if ( info.ButtonID == 1 )
			{
                                m_Stone.IngresarPlayer( from );
                                from.SendMessage( "Tus datos quedaron guardados en la piedra, para recuperarlos usala nuevamente.");
			}
                        else if ( info.ButtonID == 2 )
			{
				from.CloseGump( typeof( KFKGumpI ) );
			}

		}

	}
	
	public class KFKGumpS : Gump
	{

                private KFKStone m_Stone;

		public KFKGumpS( Mobile from, KFKStone stone ) : base(0,0)
		{

			m_Stone = stone;

			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(84, 75, 326, 200, 9250);
			this.AddLabel(142, 101, 150, @"PIEDRA DE KARMA/FAMA/KILLS");
			this.AddHtml( 139, 128, 215, 63, @"¿Deseas recuperar tus valores de Karma, Fama y Kills?", (bool)true, (bool)false);
			this.AddButton(164, 201, 4005, 4006, 1, GumpButtonType.Reply, 0);
			this.AddLabel(198, 203, 150, @"RECUPERAR MIS VALORES");
			this.AddButton(164, 226, 4005, 4006, 2, GumpButtonType.Reply, 0);
			this.AddLabel(198, 227, 150, @"SALIR");


		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
                        Mobile from = state.Mobile;

			if ( info.ButtonID == 1 )
			{
                                m_Stone.SacarPlayer( from );
                                from.SendMessage( "Tus datos han sido recuperados.");
			}
                        else if ( info.ButtonID == 2 )
			{
				from.CloseGump( typeof( KFKGumpS ) );
			}

		}

	}

}
