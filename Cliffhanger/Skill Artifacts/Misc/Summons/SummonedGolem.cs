// Golem.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a elemental golem corpse" )]
	public class SummonedGolem : BaseFamiliar
	{
		public override bool DeleteCorpseOnDeath{ get{ return true; } }

		[Constructable]
		public SummonedGolem() : base()
		{
			Name = "Elemental Golem";
			Body = 752;
			Hue = 2111;

			SetStr( 350, 400 );
			SetDex( 100, 130 );
			SetInt( 150, 200 );

			SetHits( 210, 250 );

			SetDamage( 24, 35 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 60, 70 );

			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 10, 25 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 150, 190 );
			SetSkill( SkillName.Tactics, 100, 110 );
			SetSkill( SkillName.Wrestling, 80, 100 );

			Fame = 3500;
			Karma = -3500;

			ControlSlots = 2;
			VirtualArmor = 35;

		}

		public SummonedGolem( Serial serial ) : base( serial )
		{
		}
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			
			if( !( defender is AgapiteElemental ) &&
			    !( defender is BronzeElemental ) &&
			    !( defender is CopperElemental ) &&
			    !( defender is DullCopperElemental ) &&
			    !( defender is GoldenElemental ) &&
			    !( defender is ShadowIronElemental ) &&
			    !( defender is ValoriteElemental ) &&
			    !( defender is VeriteElemental ) )
				Dispel( this );
		}
		
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGaveMeleeAttack( attacker );
			
			if( !( attacker is AgapiteElemental ) &&
			    !( attacker is BronzeElemental ) &&
			    !( attacker is CopperElemental ) &&
			    !( attacker is DullCopperElemental ) &&
			    !( attacker is GoldenElemental ) &&
			    !( attacker is ShadowIronElemental ) &&
			    !( attacker is ValoriteElemental ) &&
			    !( attacker is VeriteElemental ) )
				Dispel( this );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

	}
}
