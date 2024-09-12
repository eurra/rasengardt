using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class RobleLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} RobleLog" : "{0} RobleLogs", Amount );
			}
		}

		[Constructable]
		public RobleLog() : this( 1 )
		{
		}

		[Constructable]
		public RobleLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Roble";
                        Hue = 1803;
			Weight = 2.0;
			Amount = amount;
		}

		public RobleLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new RobleLog( amount ), amount );
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
