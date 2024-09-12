using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class PinoBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} PinoBoard" : "{0} PinoBoards", Amount );
			}
		}

		[Constructable]
		public PinoBoard() : this(1)
		{
		}

		[Constructable]
		public PinoBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Pino";
                        Hue = 2108;
			Weight = 0.1;
			Amount = amount;
		}

		public PinoBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new PinoBoard(amount), amount);
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
