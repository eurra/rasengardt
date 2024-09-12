// Wisp Elfo.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.ContextMenus;

namespace Server.Mobiles
{
	[CorpseName( "a Wisp corpse" )]
	public class SummonedWisp : BaseFamiliar
	{
                public override bool DeleteCorpseOnDeath{ get{ return true; } }
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Wisp; } }

		[Constructable]
		public SummonedWisp() : base()
		{
			AI =  AIType.AI_Mage;
			Name = "Wisp";
			Body = 58;
			BaseSoundID = 466;
			
			Hue = 1269;

			SetStr( 210, 230 );
			SetDex( 210, 230 );
			SetInt( 210, 230 );

			SetHits( 210, 230 );
			SetStam( 210, 230 );
			SetMana( 210, 230 );

			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 50, 55 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 40, 45 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 80.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.MagicResist, 115.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.Wrestling, 85.0 );

			Fame = 6000;
			Karma = 5000;

			VirtualArmor = 35;
                        ControlSlots = 3;
			AddItem( new LightSource() );
		}

		public SummonedWisp( Serial serial ) : base( serial )
		{
		}
		
		private DateTime NextCure;

		public override void OnThink()
		{
			base.OnThink();
			
			if ( DateTime.Now < NextCure )
				return;
			
			if( ControlMaster != null && InRange( ControlMaster.Location, 3 ) && ControlMaster.Stam < ControlMaster.StamMax )
			{
                                ControlMaster.FixedParticles( 0x3779, 1, 15, 9961, 1269, 0, EffectLayer.Waist );
                                ControlMaster.Stam += 5 + Utility.Random( 3 );
                                
                                if( ControlMaster.Stam > ControlMaster.StamMax )
                                	ControlMaster.Stam = ControlMaster.StamMax;

                                NextCure = DateTime.Now + TimeSpan.FromSeconds( 6 );
   			}
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
