using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class CerezoLog : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} CerezoLog" : "{0} CerezoLogs", Amount );
			}
		}

		[Constructable]
		public CerezoLog() : this( 1 )
		{
		}

		[Constructable]
		public CerezoLog( int amount ) : base( 0x1BDD )
		{
			Stackable = true;
                        Name = "Tronco de Cerezo";
                        Hue = 1517;
			Weight = 2.0;
			Amount = amount;
		}

		public CerezoLog( Serial serial ) : base( serial )
		{
		}

		public override Item Dupe( int amount )
		{
			return base.Dupe( new CerezoLog( amount ), amount );
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
