using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class PinoLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} PinoLog" : "{0} PinoLogs", Amount );
			}
		}

		[Constructable]
		public PinoLog() : this( 1 )
		{
		}

		[Constructable]
		public PinoLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Pino";
                        Hue = 2108;
			Weight = 2.0;
			Amount = amount;
		}

		public PinoLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new PinoLog( amount ), amount );
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
