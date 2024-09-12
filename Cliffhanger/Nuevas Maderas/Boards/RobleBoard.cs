using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class RobleBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} RobleBoard" : "{0} RobleBoards", Amount );
			}
		}

		[Constructable]
		public RobleBoard() : this(1)
		{
		}

		[Constructable]
		public RobleBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Roble";
                        Hue = 1803;
			Weight = 0.1;
			Amount = amount;
		}

		public RobleBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new RobleBoard(amount), amount);
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
