using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class CaobaBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} CaobaBoard" : "{0} CaobaBoards", Amount );
			}
		}

		[Constructable]
		public CaobaBoard() : this(1)
		{
		}

		[Constructable]
		public CaobaBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Caoba";
                        Hue = 2315;
			Weight = 0.1;
			Amount = amount;
		}

		public CaobaBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new CaobaBoard(amount), amount);
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
