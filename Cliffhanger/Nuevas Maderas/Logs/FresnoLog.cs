using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class FresnoLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} FresnoLog" : "{0} FresnoLogs", Amount );
			}
		}

		[Constructable]
		public FresnoLog() : this( 1 )
		{
		}

		[Constructable]
		public FresnoLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Fresno";
                        Hue = 1448;
			Weight = 2.0;
			Amount = amount;
		}

		public FresnoLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new FresnoLog( amount ), amount );
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
