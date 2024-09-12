using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class RojizoLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} RojizoLog" : "{0} RojizoLogs", Amount );
			}
		}

		[Constructable]
		public RojizoLog() : this( 1 )
		{
		}

		[Constructable]
		public RojizoLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco Rojizo";
                        Hue = 1654;
			Weight = 2.0;
			Amount = amount;
		}

		public RojizoLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new RojizoLog( amount ), amount );
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
