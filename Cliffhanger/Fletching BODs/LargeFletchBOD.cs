// Large BOD de Fletching.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Items;
using Mat = Server.Engines.BulkOrders.BulkMaterialType;

namespace Server.Engines.BulkOrders
{
	[TypeAlias( "Scripts.Engines.BulkOrders.LargeFletchBOD" )]
	public class LargeFletchBOD : LargeBOD
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
		
		public static new BulkMaterialType GetRandomMaterial( BulkMaterialType start, double[] ranges )
		{
			double random = Utility.RandomDouble();

			for ( int i = ranges.Length - 1; i >= 0; i-- )
			{
				if( random <= ranges[i] )
					return start + i;
			}

			return BulkMaterialType.None;
		}

		[Constructable]
		public LargeFletchBOD()
		{
			LargeBulkEntry[] entries;

			switch ( Utility.Random( 5 ) )
			{
				default:
				case  0: entries = LargeBulkEntry.ConvertEntries( this, LargeBulkEntry.Bows );  break;
				case  1: entries = LargeBulkEntry.ConvertEntries( this, LargeBulkEntry.Crossbows ); break;
				case  2: entries = LargeBulkEntry.ConvertEntries( this, LargeBulkEntry.Group1 ); break;
				case  3: entries = LargeBulkEntry.ConvertEntries( this, LargeBulkEntry.Group2 ); break;
				case  4: entries = LargeBulkEntry.ConvertEntries( this, LargeBulkEntry.Group3 ); break;
			}

			int hue = 1517;
			int amountMax = Utility.RandomList( 10, 15, 20, 20 );
			bool reqExceptional = ( 0.825 > Utility.RandomDouble() );

			BulkMaterialType material;

			material = GetRandomMaterial( BulkMaterialType.Pino, m_FletchingMaterialChances );

			this.Hue = hue;
			this.AmountMax = amountMax;
			this.Entries = entries;
			this.RequireExceptional = reqExceptional;
			this.Material = material;
		}

		public LargeFletchBOD( int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries )
		{
			this.Hue = 1517;
			this.AmountMax = amountMax;
			this.Entries = entries;
			this.RequireExceptional = reqExceptional;
			this.Material = mat;
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

		public LargeFletchBOD( Serial serial ) : base( serial )
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
