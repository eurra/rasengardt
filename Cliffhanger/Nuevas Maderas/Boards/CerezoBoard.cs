using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class CerezoBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} CerezoBoard" : "{0} CerezoBoards", Amount );
			}
		}

		[Constructable]
		public CerezoBoard() : this(1)
		{
		}

		[Constructable]
		public CerezoBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Cerezo";
                        Hue = 1517;
			Weight = 0.1;
			Amount = amount;
		}

		public CerezoBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new CerezoBoard(amount), amount);
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
