// Guardia PK.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
	public class GuardiaPK : BaseVillaGuard
	{

		[Constructable]
		public GuardiaPK() : this( null )
		{
		}
		
		public override bool AlwaysMurderer{ get{ return true; } }
		
		public GuardiaPK( Mobile target ) : base( AIType.AI_Melee, FightMode.Agressor, 16, 1, 0.2, 0.4, target )
		{
			Title = ", El Guardia Asesino";

			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			Body = 0x190;
			Name = NameList.RandomName( "male" );

                        Item boots = new Boots();
                        boots.Hue= 1109;
                        AddItem( boots );
				
			Item cloak = new Cloak();
                        cloak.Hue= 1109;
                        AddItem( cloak );

			Item chest = new LeatherChest();
                        chest.Hue = 1109;
			AddItem( chest );
			
			Item arms = new LeatherArms();
                        arms.Hue = 1109;
			AddItem( arms );
			
			Item legs = new LeatherLegs();
                        legs.Hue = 1109;
			AddItem( legs );
			
			Item gloves = new LeatherGloves();
                        gloves.Hue = 1109;
			AddItem( gloves );
			
			Item cap = new LeatherCap();
                        cap.Hue = 1109;
			AddItem( cap );
			
			Item gorget = new LeatherGorget();
                        gorget.Hue = 1109;
			AddItem( gorget );

			AddHairAndBeard( false );

			ChaosShield shield = new ChaosShield();
                        shield.Hue = 1109;
                        shield.Crafter = this;
			shield.Quality = ArmorQuality.Exceptional;
			AddItem( shield );

			WarFork warfork = new WarFork();
                        warfork.Hue = 1109;
                        warfork.Crafter = this;
			warfork.Quality = WeaponQuality.Exceptional;
			AddItem( warfork );

			SetSkill( SkillName.Anatomy, 95.1, 105.0 );
			SetSkill( SkillName.MagicResist, 105.5, 115.0 );
			SetSkill( SkillName.Tactics, 105.1, 115.0 );
			SetSkill( SkillName.Parry, 60.1, 70.0 );
			SetSkill( SkillName.Fencing, 115.1, 125.0 );

			Fame = 5000;
			Karma = -5000;
			
			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;

		}

		public GuardiaPK( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}

	}

}
