using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class ArceBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} ArceBoard" : "{0} ArceBoards", Amount );
			}
		}

		[Constructable]
		public ArceBoard() : this(1)
		{
		}

		[Constructable]
		public ArceBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Arce";
                        Hue = 2418;
			Weight = 0.1;
			Amount = amount;
		}

		public ArceBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new ArceBoard(amount), amount);
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
