using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class TejoBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} TejoBoard" : "{0} TejoBoards", Amount );
			}
		}

		[Constructable]
		public TejoBoard() : this(1)
		{
		}

		[Constructable]
		public TejoBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla de Tejo";
                        Hue = 1808;
			Weight = 0.1;
			Amount = amount;
		}

		public TejoBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new TejoBoard(amount), amount);
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
