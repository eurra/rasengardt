// Generador de Premios para BODs de Fletching.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Items;

namespace Server.Engines.BulkOrders
{

	public sealed class FletchRewardCalculator : RewardCalculator
	{
		private static readonly ConstructCallback RunicFletchersTools = new ConstructCallback( CreateRunicFletchersTools );

		private static Item CreateRunicFletchersTools( int type )
		{
			if ( type >= 1 && type <= 11 )
				return new RunicFletchersTools( CraftResource.BarbedLeather + type, 70 - (type*5) );

			throw new InvalidOperationException();
		}

		public static readonly FletchRewardCalculator Instance = new FletchRewardCalculator();


		public override int ComputePoints( int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type )
		{
			int points = 0;

			if ( quantity == 10 )
				points += 10;
			else if ( quantity == 15 )
				points += 25;
			else if ( quantity == 20 )
				points += 50;

			if ( exceptional )
				points += 200;

			if ( itemCount == 3 )
				points += 300;
			else if ( itemCount == 4 )
                                points += 350;
                        else if ( itemCount == 5 )
                                points += 400;

			if ( material >= BulkMaterialType.Pino && material <= BulkMaterialType.Petrificado )
				points += 200 + (50 * (material - BulkMaterialType.Pino));

			return points;
		}

		public override int ComputeGold( int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type )
		{
			int gold = 1000;
			
			if ( material >= BulkMaterialType.Pino && material <= BulkMaterialType.Petrificado )
				gold += (1000 * (material - BulkMaterialType.Pino + 1));

			return gold;
		}

		public FletchRewardCalculator()
		{
			Groups = new RewardGroup[]
				{
					new RewardGroup( 300, new RewardItem( 2, RunicFletchersTools, 1 ) ),
					new RewardGroup( 825, new RewardItem( 2, RunicFletchersTools, 2 ) ),
					new RewardGroup( 875, new RewardItem( 2, RunicFletchersTools, 3 ) ),
					new RewardGroup( 925, new RewardItem( 2, RunicFletchersTools, 4 ) ),
					new RewardGroup( 975, new RewardItem( 2, RunicFletchersTools, 5 ) ),
					new RewardGroup( 1025, new RewardItem( 2, RunicFletchersTools, 6 ) ),
					new RewardGroup( 1075, new RewardItem( 2, RunicFletchersTools, 7 ) ),
					new RewardGroup( 1125, new RewardItem( 2, RunicFletchersTools, 8 ) ),
					new RewardGroup( 1175, new RewardItem( 2, RunicFletchersTools, 9 ) ),
					new RewardGroup( 1225, new RewardItem( 2, RunicFletchersTools, 10 ) ),
					new RewardGroup( 1330, new RewardItem( 2, RunicFletchersTools, 11 ) )
				};
		}
	}
}
