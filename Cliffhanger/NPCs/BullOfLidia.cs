using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a bull corpse" )]
	public class BullOfLidia : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}
		
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool SubdueBeforeTame{ get{ return true; } }

		[Constructable]
		public BullOfLidia() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			Name = "Bull of Lidia";
			Body = 0xE8;
			BaseSoundID = 0x64;
                        Hue = 1109;

			SetStr( 220, 260 );
			SetDex( 50, 60 );
			SetInt( 60, 70 );
                                                
                        SetHits( 1000, 750 );

			SetDamage( 8, 12 );

			SetDamageType( ResistanceType.Physical, 60 );
                        SetDamageType( ResistanceType.Cold, 45 );
                        SetDamageType( ResistanceType.Fire, 40 );
                        SetDamageType( ResistanceType.Poison, 20 );
                        SetDamageType( ResistanceType.Energy, 45 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Cold, 50, 55 );
                        SetResistance( ResistanceType.Fire, 40, 45 );
                        SetResistance( ResistanceType.Poison, 20, 30 );
                        SetResistance( ResistanceType.Energy, 50, 55 );

			SetSkill( SkillName.MagicResist, 110.0, 120.0 );
			SetSkill( SkillName.Tactics, 100.0, 110.0 );
			SetSkill( SkillName.Wrestling, 90.0, 105.0 );
			SetSkill( SkillName.Focus, 100.0, 110.0 );

			Fame = 2000;
			Karma = 0;

			VirtualArmor = 25;
			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 89.1;

		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 15; } }
		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Bull; } }

		public BullOfLidia(Serial serial) : base(serial)
		{
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
