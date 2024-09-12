using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class TejoLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} TejoLog" : "{0} TejoLogs", Amount );
			}
		}

		[Constructable]
		public TejoLog() : this( 1 )
		{
		}

		[Constructable]
		public TejoLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Tejo";
                        Hue = 1808;
			Weight = 2.0;
			Amount = amount;
		}

		public TejoLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new TejoLog( amount ), amount );
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
