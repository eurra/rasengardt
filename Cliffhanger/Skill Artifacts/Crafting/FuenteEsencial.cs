// Fuente Esencial.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;

namespace Server.Items
{
	public class FuenteEsencial : StoneFountainAddon
	{
		[Constructable]
		public FuenteEsencial() : base()
		{
			Hue = 2409;
			Name = "Fuente Esencial";
		}
		
		public FuenteEsencial( Serial serial ) : base( serial )
		{
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
