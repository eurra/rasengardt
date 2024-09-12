// Piedra de Arreglo de Skills.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Scripts.Gumps;

namespace Server.Items
{
	public class SkillsStone : Item
  	{
		private class SkillsStonePrevGump: Gump
		{
			public SkillsStonePrevGump( Mobile from ) : base( 0, 0 )
			{
				from.CloseGump( typeof( SkillsStonePrevGump ) );

				AddBackground(50, 60, 420, 360, 9200);
				AddImageTiled(60, 70, 400, 340, 2624);
				AddAlphaRegion(60, 70, 400, 340);
				AddLabel(160, 85, 154, "PIEDRA DE ARREGLO DE SKILLS");
				AddHtml( 113, 136, 299, 212, "<BASEFONT COLOR=#FFFFFF>Con esta piedra podrás bajar la cantidad que desees de puntos de tus skills, con el fin de regular la situación de tu Subcap. Para ir revisando el estado del mismo, puedes utilizar en cualquier momento el comando \".Subcap\"\n\nNOTA: El staff no se hará responsable de errores en el uso de esta piedra, sin embargo de forma previa podrás preguntarles tus inquietudes al respecto. Te recomendamos planear bien tu plantilla antes de usar esta piedra!</BASEFONT>", false, false );
				AddButton(170, 350, 1154, 1155, 1, GumpButtonType.Reply, 0);
				AddButton(170, 380, 1154, 1155, 2, GumpButtonType.Reply, 0);
				AddLabel(206, 350, 154, "ARREGLAR MIS SKILLS");
				AddLabel(206, 380, 154, "SALIR");
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
                        	Mobile from = state.Mobile;

				if ( info.ButtonID == 1 )
				{
                                	from.CloseGump( typeof( SkillsStonePrevGump ) );
                                	from.SendGump( new SkillsStoneGump( from ) );
				}
			}
		}

		[Constructable]
		public SkillsStone() : base( 0xED4 )
		{
         		Movable = false;
 	   	    	Hue = 2057;
 	   	    	Name = "Piedra de Arreglo de Skills";
 	     	}

     		public SkillsStone( Serial serial ) : base( serial )
      		{
      		}

 	     	public override void OnDoubleClick( Mobile from )
 	     	{

			if ( !from.InRange( this, 3 ) )
			{
				from.SendMessage ("Estas muy lejos de la Piedra!");
				return;
			}

			if ( !from.HasGump( typeof(SkillsStoneGump) ) && !from.HasGump( typeof(SkillsStonePrevGump) ) )
				from.SendGump( new SkillsStonePrevGump( from ) );
		}

      		public override void Serialize( GenericWriter writer )
      		{
        		base.Serialize( writer );

        		writer.Write( (int) 0 ); // version

      		}

      		public override void Deserialize( GenericReader reader )
      		{
        		base.Deserialize( reader );

         		int version = reader.ReadInt();
      		}
   	}
}
