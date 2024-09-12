using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class CedroBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} CedroBoard" : "{0} CedroBoards", Amount );
			}
		}

		[Constructable]
		public CedroBoard() : this(1)
		{
		}

		[Constructable]
		public CedroBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Cedro";
                        Hue = 2307;
			Weight = 0.1;
			Amount = amount;
		}

		public CedroBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new CedroBoard(amount), amount);
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
