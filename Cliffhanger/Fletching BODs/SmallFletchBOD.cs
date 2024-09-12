// Small BOD de Fletching.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.Craft;
using Mat = Server.Engines.BulkOrders.BulkMaterialType;

namespace Server.Engines.BulkOrders
{
	[TypeAlias( "Scripts.Engines.BulkOrders.SmallFletchBOD" )]
	public class SmallFletchBOD : SmallBOD
	{
		public static double[] m_FletchingMaterialChances = new double[]
			{
				0.8, // Pino
				0.65, // Nogal
				0.5, // Arce
				0.35, // Caoba
				0.2, // Fresno
				0.15, // Cedro
				0.13, // Roble
				0.1, // Cerezo
				0.05,  // Tejo
				0.03,  // Rojizo
				0.02,  // Petrificado
			};

		public override int ComputeFame()
		{
			return FletchRewardCalculator.Instance.ComputeFame( this );
		}

		public override int ComputeGold()
		{
			return FletchRewardCalculator.Instance.ComputeGold( this );
		}

		public override ArrayList ComputeRewards( bool full )
		{
			ArrayList list = new ArrayList();

			RewardGroup rewardGroup = FletchRewardCalculator.Instance.LookupRewards( FletchRewardCalculator.Instance.ComputePoints( this ) );

			if ( rewardGroup != null )
			{
				if ( full )
				{
					for ( int i = 0; i < rewardGroup.Items.Length; ++i )
					{
						Item item = rewardGroup.Items[i].Construct();

						if ( item != null )
							list.Add( item );
					}
				}
				else
				{
					RewardItem rewardItem = rewardGroup.AquireItem();

					if ( rewardItem != null )
					{
						Item item = rewardItem.Construct();

						if ( item != null )
							list.Add( item );
					}
				}
			}

			return list;
		}

		public static SmallFletchBOD CreateRandomFor( Mobile m )
		{
			SmallBulkEntry[] entries;

			entries = SmallBulkEntry.FletchingBows;

			if ( entries.Length > 0 )
			{
				double theirSkill = m.Skills[SkillName.Fletching].Base;
				int amountMax;

				if ( theirSkill >= 70.1 )
					amountMax = Utility.RandomList( 10, 15, 20, 20 );
				else if ( theirSkill >= 50.1 )
					amountMax = Utility.RandomList( 10, 15, 15, 20 );
				else
					amountMax = Utility.RandomList( 10, 10, 15, 20 );

				BulkMaterialType material = BulkMaterialType.None;

				if ( theirSkill >= 70.1 )
				{
					for ( int i = 0; i < 20; ++i )
					{
						BulkMaterialType check = LargeFletchBOD.GetRandomMaterial( BulkMaterialType.Pino, m_FletchingMaterialChances );
						double skillReq = 0.0;

						switch ( check )
						{
							case BulkMaterialType.Pino: skillReq = 65.0; break;
							case BulkMaterialType.Nogal: skillReq = 70.0; break;
							case BulkMaterialType.Arce: skillReq = 73.0; break;
							case BulkMaterialType.Caoba: skillReq = 78.0; break;
							case BulkMaterialType.Fresno: skillReq = 80.0; break;
							case BulkMaterialType.Cedro: skillReq = 85.0; break;
							case BulkMaterialType.Roble: skillReq = 88.0; break;
							case BulkMaterialType.Cerezo: skillReq = 90.0; break;
							case BulkMaterialType.Tejo: skillReq = 93.0; break;
							case BulkMaterialType.Rojizo: skillReq = 95.0; break;
							case BulkMaterialType.Petrificado: skillReq = 100.0; break;
						}

						if ( theirSkill >= skillReq )
						{
							material = check;
							break;
						}
					}
				}

				double excChance = 0.0;

				if ( theirSkill >= 70.1 )
					excChance = (theirSkill + 80.0) / 200.0;

				bool reqExceptional = ( excChance > Utility.RandomDouble() );

				SmallBulkEntry entry = null;

				CraftSystem system = DefBowFletching.CraftSystem;

				for ( int i = 0; i < 150; ++i )
				{
					SmallBulkEntry check = entries[Utility.Random( entries.Length )];

					CraftItem item = system.CraftItems.SearchFor( check.Type );

					if ( item != null )
					{
						bool allRequiredSkills = true;
						double chance = item.GetSuccessChance( m, null, system, false, ref allRequiredSkills );

						if ( allRequiredSkills && chance >= 0.0 )
						{
							if ( reqExceptional )
								chance = item.GetExceptionalChance( system, chance, m );

							if ( chance > 0.0 )
							{
								entry = check;
								break;
							}
						}
					}
				}

				if ( entry != null )
					return new SmallFletchBOD( entry, material, amountMax, reqExceptional );
			}

			return null;
		}

		private SmallFletchBOD( SmallBulkEntry entry, BulkMaterialType material, int amountMax, bool reqExceptional )
		{
			this.Hue = 1517;
			this.AmountMax = amountMax;
			this.Type = entry.Type;
			this.Number = entry.Number;
			this.Graphic = entry.Graphic;
			this.RequireExceptional = reqExceptional;
			this.Material = material;
		}

		[Constructable]
		public SmallFletchBOD()
		{
			SmallBulkEntry[] entries;

			entries = SmallBulkEntry.FletchingBows;

			if ( entries.Length > 0 )
			{
				int hue = 1517;
				int amountMax = Utility.RandomList( 10, 15, 20 );

				BulkMaterialType material;

				material = LargeFletchBOD.GetRandomMaterial( BulkMaterialType.Pino, m_FletchingMaterialChances );

				bool reqExceptional = Utility.RandomBool();

				SmallBulkEntry entry = entries[Utility.Random( entries.Length )];

				this.Hue = hue;
				this.AmountMax = amountMax;
				this.Type = entry.Type;
				this.Number = entry.Number;
				this.Graphic = entry.Graphic;
				this.RequireExceptional = reqExceptional;
				this.Material = material;
			}
		}

		public SmallFletchBOD( int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional, BulkMaterialType mat )
		{
			this.Hue = 1517;
			this.AmountMax = amountMax;
			this.AmountCur = amountCur;
			this.Type = type;
			this.Number = number;
			this.Graphic = graphic;
			this.RequireExceptional = reqExceptional;
			this.Material = mat;
		}

		public SmallFletchBOD( Serial serial ) : base( serial )
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
