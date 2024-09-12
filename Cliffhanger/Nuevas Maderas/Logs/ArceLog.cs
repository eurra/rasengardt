using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class ArceLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} ArceLog" : "{0} ArceLogs", Amount );
			}
		}

		[Constructable]
		public ArceLog() : this( 1 )
		{
		}

		[Constructable]
		public ArceLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Arce";
                        Hue = 2418;
			Weight = 2.0;
			Amount = amount;
		}

		public ArceLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new ArceLog( amount ), amount );
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
