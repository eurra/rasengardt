using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class CedroLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} CedroLog" : "{0} CedroLogs", Amount );
			}
		}

		[Constructable]
		public CedroLog() : this( 1 )
		{
		}

		[Constructable]
		public CedroLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Cedro";
                        Hue = 2307;
			Weight = 2.0;
			Amount = amount;
		}

		public CedroLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new CedroLog( amount ), amount );
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
