using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class CaobaLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} CaobaLog" : "{0} CaobaLogs", Amount );
			}
		}

		[Constructable]
		public CaobaLog() : this( 1 )
		{
		}

		[Constructable]
		public CaobaLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Caoba";
                        Hue = 2315;
			Weight = 2.0;
			Amount = amount;
		}

		public CaobaLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new CaobaLog( amount ), amount );
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
