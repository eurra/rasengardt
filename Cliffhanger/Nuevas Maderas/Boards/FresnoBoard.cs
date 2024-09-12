using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class FresnoBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} FresnoBoard" : "{0} FresnoBoards", Amount );
			}
		}

		[Constructable]
		public FresnoBoard() : this(1)
		{
		}

		[Constructable]
		public FresnoBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Fresno";
                        Hue = 1448;
			Weight = 0.1;
			Amount = amount;
		}

		public FresnoBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new FresnoBoard(amount), amount);
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
