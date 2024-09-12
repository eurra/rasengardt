using System;

namespace Server.Items
{
	[FlipableAttribute(0x1BD7, 0x1BDA)]
	public class RojizoBoard : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} RojizoBoard" : "{0} RojizoBoards", Amount );
			}
		}

		[Constructable]
		public RojizoBoard() : this(1)
		{
		}

		[Constructable]
		public RojizoBoard(int amount) : base(0x1BD7)
		{
			Stackable = true;
                        Name = "Tabla Rojiza";
                        Hue = 1654;
			Weight = 0.1;
			Amount = amount;
		}

		public RojizoBoard(Serial serial) : base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new RojizoBoard(amount), amount);
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
