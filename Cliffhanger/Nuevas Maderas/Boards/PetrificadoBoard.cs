using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class PetrificadoBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} PetrificadoBoard" : "{0} PetrificadoBoards", Amount );
			}
		}

		[Constructable]
		public PetrificadoBoard() : this(1)
		{
		}

		[Constructable]
		public PetrificadoBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla Petrificada";
                        Hue = 2306;
			Weight = 0.1;
			Amount = amount;
		}

		public PetrificadoBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new PetrificadoBoard(amount), amount);
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
