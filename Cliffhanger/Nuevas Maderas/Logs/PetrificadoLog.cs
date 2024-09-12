using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class PetrificadoLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} PetrificadoLog" : "{0} PetrificadoLogs", Amount );
			}
		}

		[Constructable]
		public PetrificadoLog() : this( 1 )
		{
		}

		[Constructable]
		public PetrificadoLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco Petrificado";
                        Hue = 2306;
			Weight = 2.0;
			Amount = amount;
		}

		public PetrificadoLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new PetrificadoLog( amount ), amount );
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
