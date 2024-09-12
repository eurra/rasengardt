using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class NogalBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} NogalBoard" : "{0} NogalBoards", Amount );
			}
		}

		[Constructable]
		public NogalBoard() : this(1)
		{
		}

		[Constructable]
		public NogalBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Nogal";
                        Hue = 1747;
			Weight = 0.1;
			Amount = amount;
		}

		public NogalBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new NogalBoard(amount), amount);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
