using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class NogalLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} NogalLog" : "{0} NogalLogs", Amount );
			}
		}

		[Constructable]
		public NogalLog() : this( 1 )
		{
		}

		[Constructable]
		public NogalLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Nogal";
                        Hue = 1747;
			Weight = 2.0;
			Amount = amount;
		}

		public NogalLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new NogalLog( amount ), amount );
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
