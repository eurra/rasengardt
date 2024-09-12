// Prop-Stone
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Reflection;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class PropStone : Item
  	{
		public enum ItemDest
		{
			Weapon,
			Armor,
			Shield,
			Jewel,
			None
		}

		private ItemDest m_TipoItem = ItemDest.None;
		private AosWeaponAttributes m_AosWeaponAttributes;
		private AosArmorAttributes m_AosArmorAttributes;
		private AosAttributes m_AosAttributes;
		private SlayerName m_Slayer;

		[CommandProperty( AccessLevel.GameMaster )]
		public ItemDest TipoItem
		{
                	get{ return m_TipoItem; }

                	set
                	{
		        	if( m_TipoItem == value )
		        		return;

                                m_TipoItem = value;
                                
                                if( m_TipoItem == ItemDest.Weapon )
                                	Hue = 2301;
                                else if( m_TipoItem == ItemDest.Armor )
                                	Hue = 1939;
                                else if( m_TipoItem == ItemDest.Shield )
                                	Hue = 1346;
                                else if( m_TipoItem == ItemDest.Jewel )
                                	Hue = 2210;
                                else
                                	Hue = 0;

				InvalidateProperties();
				SetRandomProp();
		        }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public AosWeaponAttributes WeaponAttributes
		{
			get{ return m_AosWeaponAttributes; }
			set{}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public AosArmorAttributes ArmorAttributes
		{
			get{ return m_AosArmorAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AosAttributes Attributes
		{
			get{ return m_AosAttributes; }
			set{}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SlayerName Slayer
		{
			get{ return m_Slayer; }
			set{ m_Slayer = value; InvalidateProperties(); }
		}

		[Constructable]
		public PropStone(): this( (ItemDest)Utility.Random( 4 ) )
		{
		}

		public PropStone( ItemDest tipoitem ) : base( 0x1870 )
		{
                        Name = "Piedra Mágica de Propiedad";
			LootType = LootType.Blessed;

                        m_AosWeaponAttributes = new AosWeaponAttributes( this );
                        m_AosArmorAttributes = new AosArmorAttributes( this );
                        m_AosAttributes = new AosAttributes( this );
                        m_Slayer = 0;
                        
                        TipoItem = tipoitem;
 	     	}

     		public PropStone( Serial serial ) : base( serial )
      		{
      		}

		private void SetRandomProp()
		{
			if( m_TipoItem == ItemDest.Weapon )
			{
				switch( Utility.Random( 28 ) )
				{
                                	case 0: m_AosAttributes.AttackChance = 3; break;
                                	case 1: m_AosAttributes.DefendChance = 3; break;
                                	case 2: m_AosAttributes.CastSpeed = 1; break;
                                        case 3: m_AosAttributes.Luck = 20; break;
                                        case 4: m_AosAttributes.NightSight = 1; break;
                                        case 5: m_AosAttributes.SpellChanneling = 1; break;
                                        case 6: m_AosAttributes.WeaponDamage = 5; break;
                                        case 7: m_AosAttributes.WeaponSpeed = 5; break;
                                        case 8: m_AosWeaponAttributes.DurabilityBonus = 20; break;
                                        case 9: m_AosWeaponAttributes.HitColdArea = 5; break;
					case 10: m_AosWeaponAttributes.HitDispel = 5; break;
					case 11: m_AosWeaponAttributes.HitEnergyArea = 5; break;
					case 12: m_AosWeaponAttributes.HitFireArea = 5; break;
					case 13: m_AosWeaponAttributes.HitFireball = 5; break;
					case 14: m_AosWeaponAttributes.HitHarm = 5; break;
					case 15: m_AosWeaponAttributes.HitLeechHits = 5; break;
					case 16: m_AosWeaponAttributes.HitLeechMana = 5; break;
					case 17: m_AosWeaponAttributes.HitLeechStam = 5; break;
					case 18: m_AosWeaponAttributes.HitLightning = 5; break;
					case 19: m_AosWeaponAttributes.HitLowerAttack = 5; break;
					case 20: m_AosWeaponAttributes.HitLowerDefend = 5; break;
					case 21: m_AosWeaponAttributes.HitMagicArrow = 5; break;
					case 22: m_AosWeaponAttributes.HitPhysicalArea = 5; break;
					case 23: m_AosWeaponAttributes.HitPoisonArea = 5; break;
					case 24: m_AosWeaponAttributes.MageWeapon = 5; break;
                                        case 25: m_AosWeaponAttributes.SelfRepair = 1; break;
                                        case 26: m_AosWeaponAttributes.UseBestSkill = 1; break;
                                        case 27:
                                        {
                                        	switch( Utility.Random( 27 ) )
						{
                                                	case 0: m_Slayer = SlayerName.Fey; break;
							case 1: m_Slayer = SlayerName.Silver; break;
                                                	case 2: m_Slayer = SlayerName.OrcSlaying; break;
                                                	case 3: m_Slayer = SlayerName.TrollSlaughter; break;
                                                	case 4: m_Slayer = SlayerName.OgreTrashing; break;
                                                	case 5: m_Slayer = SlayerName.Repond; break;
                                                	case 6: m_Slayer = SlayerName.DragonSlaying; break;
                                                	case 7: m_Slayer = SlayerName.Terathan; break;
                                                	case 8: m_Slayer = SlayerName.SnakesBane; break;
                                                	case 9: m_Slayer = SlayerName.LizardmanSlaughter; break;
                                                	case 10: m_Slayer = SlayerName.ReptilianDeath; break;
                                                	case 11: m_Slayer = SlayerName.DaemonDismissal; break;
                                                	case 12: m_Slayer = SlayerName.GargoylesFoe; break;
                                                	case 13: m_Slayer = SlayerName.BalronDamnation; break;
                                                	case 14: m_Slayer = SlayerName.Exorcism; break;
                                                	case 15: m_Slayer = SlayerName.Ophidian; break;
                                                	case 16: m_Slayer = SlayerName.SpidersDeath; break;
                                                	case 17: m_Slayer = SlayerName.ScorpionsBane; break;
                                                	case 18: m_Slayer = SlayerName.ArachnidDoom; break;
                                                	case 19: m_Slayer = SlayerName.FlameDousing; break;
                                                	case 20: m_Slayer = SlayerName.WaterDissipation; break;
                                                	case 21: m_Slayer = SlayerName.Vacuum; break;
                                                	case 22: m_Slayer = SlayerName.ElementalHealth; break;
                                                	case 23: m_Slayer = SlayerName.EarthShatter; break;
                                                	case 24: m_Slayer = SlayerName.BloodDrinking; break;
                                                	case 25: m_Slayer = SlayerName.SummerWind; break;
                                                	case 26: m_Slayer = SlayerName.ElementalBan; break;
                                                }

                                                break;
					}
				}
			}
			else if( m_TipoItem == ItemDest.Armor )
			{
				switch( Utility.Random( 9 ) )
				{
                                	case 0: m_AosArmorAttributes.DurabilityBonus = 20; break;
                                	case 1: m_AosArmorAttributes.MageArmor = 1; break;
                                	case 2: m_AosArmorAttributes.SelfRepair = 1; break;
                                        case 3: m_AosAttributes.BonusStam = 2; break;
                                        case 4: m_AosAttributes.BonusMana = 2; break;
                                        case 5: m_AosAttributes.BonusHits = 2; break;
                                        case 6: m_AosAttributes.Luck = 20; break;
                                        case 7: m_AosAttributes.NightSight = 1; break;
                                        case 8: m_AosAttributes.ReflectPhysical = 5; break;
				}
			}
			else if( m_TipoItem == ItemDest.Shield )
			{
			        switch( Utility.Random( 9 ) )
				{
                                	case 0: m_AosArmorAttributes.DurabilityBonus = 20; break;
                                	case 1: m_AosArmorAttributes.SelfRepair = 1; break;
                                	case 2: m_AosAttributes.AttackChance = 3; break;
                                	case 3: m_AosAttributes.DefendChance = 3; break;
                                	case 4: m_AosAttributes.CastSpeed = 1; break;
                                        case 5: m_AosAttributes.Luck = 20; break;
                                        case 6: m_AosAttributes.NightSight = 1; break;
                                        case 7: m_AosAttributes.ReflectPhysical = 5; break;
                                        case 8: m_AosAttributes.SpellChanneling = 1; break;
				}
			}
			else if( m_TipoItem == ItemDest.Jewel )
			{
				switch( Utility.Random( 17 ) )
				{
                                	case 0: m_AosAttributes.AttackChance = 3; break;
                                	case 1: m_AosAttributes.DefendChance = 3; break;
                                	case 2: m_AosAttributes.BonusDex = 2; break;
                                        case 3: m_AosAttributes.BonusInt = 2; break;
                                        case 4: m_AosAttributes.BonusStr = 2; break;
                                	case 5: m_AosAttributes.CastSpeed = 1; break;
                                        case 6: m_AosAttributes.CastRecovery = 1; break;
                                        case 7: m_AosAttributes.EnhancePotions = 5; break;
                                        case 8: m_AosAttributes.LowerManaCost = 5; break;
                                        case 9: m_AosAttributes.LowerRegCost = 5; break;
                                        case 10: m_AosAttributes.Luck = 20; break;
                                        case 11: m_AosAttributes.NightSight = 1; break;
                                       	case 12: m_AosAttributes.RegenHits = 1; break;
                                        case 13: m_AosAttributes.RegenMana = 1; break;
                                        case 14: m_AosAttributes.RegenStam = 1; break;
                                        case 15: m_AosAttributes.WeaponDamage = 5; break;
				}
			}
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			int prop;

			if( m_TipoItem == ItemDest.Weapon )
			{
				list.Add( 1060658, "Utilizable en\tArmas");

				if ( m_Slayer != SlayerName.None )
					list.Add( SlayerGroup.GetEntryByName( m_Slayer ).Title );

				if ( (prop = m_AosWeaponAttributes.UseBestSkill) != 0 )
					list.Add( 1060400 ); // use best weapon skill

				if ( (prop = m_AosAttributes.WeaponDamage) != 0 )
					list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%

				if ( (prop = m_AosAttributes.DefendChance) != 0 )
					list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%

				if ( (prop = m_AosAttributes.CastSpeed) != 0 )
					list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~

				if ( (prop = m_AosAttributes.AttackChance) != 0 )
					list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitColdArea) != 0 )
					list.Add( 1060416, prop.ToString() ); // hit cold area ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitDispel) != 0 )
					list.Add( 1060417, prop.ToString() ); // hit dispel ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitEnergyArea) != 0 )
					list.Add( 1060418, prop.ToString() ); // hit energy area ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitFireArea) != 0 )
					list.Add( 1060419, prop.ToString() ); // hit fire area ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitFireball) != 0 )
					list.Add( 1060420, prop.ToString() ); // hit fireball ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitHarm) != 0 )
					list.Add( 1060421, prop.ToString() ); // hit harm ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitLeechHits) != 0 )
					list.Add( 1060422, prop.ToString() ); // hit life leech ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitLightning) != 0 )
					list.Add( 1060423, prop.ToString() ); // hit lightning ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitLowerAttack) != 0 )
					list.Add( 1060424, prop.ToString() ); // hit lower attack ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitLowerDefend) != 0 )
					list.Add( 1060425, prop.ToString() ); // hit lower defense ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitMagicArrow) != 0 )
					list.Add( 1060426, prop.ToString() ); // hit magic arrow ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitLeechMana) != 0 )
					list.Add( 1060427, prop.ToString() ); // hit mana leech ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitPhysicalArea) != 0 )
					list.Add( 1060428, prop.ToString() ); // hit physical area ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitPoisonArea) != 0 )
					list.Add( 1060429, prop.ToString() ); // hit poison area ~1_val~%

				if ( (prop = m_AosWeaponAttributes.HitLeechStam) != 0 )
					list.Add( 1060430, prop.ToString() ); // hit stamina leech ~1_val~%

				if ( (prop = m_AosAttributes.Luck) != 0 )
					list.Add( 1060436, prop.ToString() ); // luck ~1_val~

				if ( (prop = m_AosWeaponAttributes.MageWeapon) != 0 )
					list.Add( 1060438, prop.ToString() ); // mage weapon -~1_val~ skill

				if ( (prop = m_AosAttributes.NightSight) != 0 )
					list.Add( 1060441 ); // night sight

				if ( (prop = m_AosWeaponAttributes.SelfRepair) != 0 )
					list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

				if ( (prop = m_AosAttributes.SpellChanneling) != 0 )
					list.Add( 1060482 ); // spell channeling

				if ( (prop = m_AosAttributes.WeaponSpeed) != 0 )
					list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%
					
				if ( (prop = m_AosWeaponAttributes.DurabilityBonus) != 0 )
					list.Add( 1060660, "{0}\t{1}", "Durabilidad", prop.ToString() );
			}
			else if( m_TipoItem == ItemDest.Armor )
			{
				list.Add( 1060658, "Utilizable en\tArmaduras");

				if ( (prop = m_AosAttributes.BonusDex) != 0 )
					list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

				if ( (prop = m_AosAttributes.BonusInt) != 0 )
					list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~

				if ( (prop = m_AosAttributes.Luck) != 0 )
					list.Add( 1060436, prop.ToString() ); // luck ~1_val~

				if ( (prop = m_AosArmorAttributes.MageArmor) != 0 )
					list.Add( 1060437 ); // mage armor

				if ( (prop = m_AosAttributes.NightSight) != 0 )
					list.Add( 1060441 ); // night sight

				if ( (prop = m_AosAttributes.ReflectPhysical) != 0 )
					list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%

				if ( (prop = m_AosArmorAttributes.SelfRepair) != 0 )
					list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

				if ( (prop = m_AosAttributes.BonusStr) != 0 )
					list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~
				
				if ( (prop = m_AosArmorAttributes.DurabilityBonus) != 0 )
					list.Add( 1060660, "{0}\t{1}", "Durabilidad", prop.ToString() );
			}
			else if( m_TipoItem == ItemDest.Shield )
			{
				list.Add( 1060658, "Utilizable en\tEscudos");

				if ( (prop = m_AosArmorAttributes.DurabilityBonus) != 0 )
					list.Add( 1060660, "{0}\t{1}", "Durabilidad", prop.ToString() );
					
				if ( (prop = m_AosArmorAttributes.SelfRepair) != 0 )
					list.Add( 1060450, prop.ToString() ); // self repair ~1_val~

                                if ( (prop = m_AosAttributes.DefendChance) != 0 )
					list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%
				
                                if ( (prop = m_AosAttributes.AttackChance) != 0 )
					list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%
					
                                if ( (prop = m_AosAttributes.CastSpeed) != 0 )
					list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~
					
                                if ( (prop = m_AosAttributes.Luck) != 0 )
					list.Add( 1060436, prop.ToString() ); // luck ~1_val~
					
				if ( (prop = m_AosAttributes.NightSight) != 0 )
					list.Add( 1060441 ); // night sight
					
                                if ( (prop = m_AosAttributes.ReflectPhysical) != 0 )
					list.Add( 1060442, prop.ToString() ); // reflect physical damage ~1_val~%
					
                                if ( (prop = m_AosAttributes.SpellChanneling) != 0 )
					list.Add( 1060482 ); // spell channeling
			}
			else if( m_TipoItem == ItemDest.Jewel )
			{
				list.Add( 1060658, "Utilizable en\tJoyas");

                                if ( (prop = m_AosAttributes.DefendChance) != 0 )
					list.Add( 1060408, prop.ToString() ); // defense chance increase ~1_val~%
				
                                if ( (prop = m_AosAttributes.AttackChance) != 0 )
					list.Add( 1060415, prop.ToString() ); // hit chance increase ~1_val~%
					
				if ( (prop = m_AosAttributes.BonusDex) != 0 )
					list.Add( 1060409, prop.ToString() ); // dexterity bonus ~1_val~

				if ( (prop = m_AosAttributes.BonusInt) != 0 )
					list.Add( 1060432, prop.ToString() ); // intelligence bonus ~1_val~
					
                                if ( (prop = m_AosAttributes.BonusStr) != 0 )
					list.Add( 1060485, prop.ToString() ); // strength bonus ~1_val~

                                if ( (prop = m_AosAttributes.CastSpeed) != 0 )
					list.Add( 1060413, prop.ToString() ); // faster casting ~1_val~
					
				if ( (prop = m_AosAttributes.CastRecovery) != 0 )
					list.Add( 1060412, prop.ToString() ); // faster cast recovery ~1_val~
				
                                if ( (prop = m_AosAttributes.EnhancePotions) != 0 )
					list.Add( 1060411, prop.ToString() ); // enhance potions ~1_val~%

                                if ( (prop = m_AosAttributes.LowerManaCost) != 0 )
					list.Add( 1060433, prop.ToString() ); // lower mana cost ~1_val~%

				if ( (prop = m_AosAttributes.LowerRegCost) != 0 )
					list.Add( 1060434, prop.ToString() ); // lower reagent cost ~1_val~%

                                if ( (prop = m_AosAttributes.Luck) != 0 )
					list.Add( 1060436, prop.ToString() ); // luck ~1_val~
					
				if ( (prop = m_AosAttributes.NightSight) != 0 )
					list.Add( 1060441 ); // night sight
					
                                if ( (prop = m_AosAttributes.RegenMana) != 0 )
					list.Add( 1060440, prop.ToString() ); // mana regeneration ~1_val~
					
                                if ( (prop = m_AosAttributes.RegenStam) != 0 )
					list.Add( 1060443, prop.ToString() ); // stamina regeneration ~1_val~

				if ( (prop = m_AosAttributes.RegenHits) != 0 )
					list.Add( 1060444, prop.ToString() ); // hit point regeneration ~1_val~
					
                                if ( (prop = m_AosAttributes.WeaponSpeed) != 0 )
					list.Add( 1060486, prop.ToString() ); // swing speed increase ~1_val~%
					
				if ( (prop = m_AosAttributes.WeaponDamage) != 0 )
					list.Add( 1060401, prop.ToString() ); // damage increase ~1_val~%
			}

		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "La Piedra de Propiedad debe estar en tu bolso para ser usada!" );
				return;
			}
			
			if( TipoItem == ItemDest.None )
				return;
			
			from.SendMessage( "Elige el item que mejoraras con las propiedades de esta piedra...");
			from.Target = new InternalTarget( this );
			from.Target.BeginTimeout( from, TimeSpan.FromSeconds( 30.0 ) );
		}
		
		private class InternalTarget : Target
		{
			private PropStone m_Stone;
			
			public InternalTarget( PropStone Stone ) :  base( 1, false, TargetFlags.None )
			{
                        	m_Stone = Stone;
                        }

                        protected override void OnTarget( Mobile from, object o )
                        {
				if( ( !( o is BaseWeapon ) && !( o is BaseArmor ) && !( o is BaseJewel ) && !( o is BaseShield ) ) || o is SkillArtifact )
				{
                                     	from.SendMessage( "No puedes usar la piedra con eso!" );
					return;
				}

				if( !( m_Stone.IsChildOf( from.Backpack ) ) )
				{
					from.SendMessage( "La Piedra de Propiedad debe estar en tu bolso para ser usada!" );
					return;
				}
				
				bool cambioProp = false;

				if( m_Stone.TipoItem == ItemDest.Weapon )
				{
					if( !( o is BaseWeapon ) )
					{
                                        	from.SendMessage( "La piedra sólo es compatible con armas." );
						return;
					}
					else
					{
						if( ((BaseWeapon)o).ArtifactRarity >= 10 )
						{
							from.SendMessage( "La piedra no puede usarse con un item como ese." );
							return;
						}

						cambioProp = AsignarProps( (BaseWeapon)o );
					}
				}
				else if( m_Stone.TipoItem == ItemDest.Armor )
				{
					if( !( o is BaseArmor ) )
					{
                                        	from.SendMessage( "La piedra sólo es compatible con armaduras." );
						return;
					}
					else
					{
						if( ((BaseArmor)o).ArtifactRarity >= 10 )
						{
							from.SendMessage( "La piedra no puede usarse con un item como ese." );
							return;
						}

						cambioProp = AsignarProps( (BaseArmor)o, ItemDest.Armor );
					}
				}
				else if( m_Stone.TipoItem == ItemDest.Shield )
				{
					if( !( o is BaseShield ) )
					{
                                        	from.SendMessage( "La piedra sólo es compatible con escudos." );
						return;
					}
					else
					{
						if( ((BaseArmor)o).ArtifactRarity >= 10 )
						{
							from.SendMessage( "La piedra no puede usarse con un item como ese." );
							return;
						}

						cambioProp = AsignarProps( (BaseArmor)o, ItemDest.Shield );
					}
				}
				else if( m_Stone.TipoItem == ItemDest.Jewel )
				{
					if( !( o is BaseJewel ) )
					{
                                        	from.SendMessage( "La piedra sólo es compatible con joyas." );
						return;
					}
					else
					{
						if( ((BaseJewel)o).ArtifactRarity >= 10 )
						{
							from.SendMessage( "La piedra no puede usarse con un item como ese." );
							return;
						}

						cambioProp = AsignarProps( (BaseJewel)o );
					}
				}
				
				if( cambioProp )
				{
					from.SendMessage( "Las propiedades en la piedra han sido agregadas exitosamente al item." );
					m_Stone.Delete();
				}
				else
				{
                                        from.SendMessage( "Los propiedades de tu piedra no son útiles en ese item." );
				}
			}

			private bool SumarAosProps( AosAttributes bonus, AosAttributes dest, ItemDest tipo )
			{
                        	bool cambioProp = false;
                        	int cap;

				string[] attr = Enum.GetNames( typeof(AosAttribute) );

				for( int i=0; i < attr.Length; i++ )
				{
                                        AosAttribute actualAttr = (AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] );

                                        if( bonus[actualAttr] > 0 )
                                        {
						if( ( actualAttr == AosAttribute.NightSight || actualAttr == AosAttribute.SpellChanneling ) && dest[actualAttr] <= 0 )
						{
                                                	dest[actualAttr] = bonus[actualAttr];
                                                	
                                                	if( dest.CastSpeed > 0 && actualAttr == AosAttribute.SpellChanneling )
                                                        	dest.CastSpeed = 0;

                                                	cambioProp = true;
                                                }
                                                else if( ( cap = GetCap( tipo, actualAttr ) ) > dest[actualAttr] )
                                                {
							if( dest[actualAttr] + bonus[actualAttr] > cap )
							{
                                                        	dest[actualAttr] = cap;
                                                	}
                                                	else
                                                	{
                                                        	dest[actualAttr] += bonus[actualAttr];
                                                	}
                                                	
                                                	if( dest.SpellChanneling > 0 && actualAttr == AosAttribute.CastSpeed )
                                                        	dest.SpellChanneling = 0;
                                                                
                                                	cambioProp = true;
                                                }
					}
				}
				
				return cambioProp;
			}
			
			private bool SumarWeaponProps( AosWeaponAttributes bonus, AosWeaponAttributes dest )
			{
                        	bool cambioProp = false;
                        	int cap;

				string[] attr = Enum.GetNames( typeof(AosWeaponAttribute) );

				for( int i=0; i < attr.Length; i++ )
				{
                                        AosWeaponAttribute actualAttr = (AosWeaponAttribute)Enum.Parse( typeof(AosWeaponAttribute), attr[i] );
                                                
                                        if( ( bonus[actualAttr] > 0 ) )
                                        {
						if( actualAttr == AosWeaponAttribute.UseBestSkill && dest[actualAttr] <= 0 )
						{
                                                	dest[actualAttr] = bonus[actualAttr];

                                                	if( dest.MageWeapon > 0 )
                                                        	dest.MageWeapon = 0;

                                                	cambioProp = true;
                                                }
						else if( ( cap = GetCap( actualAttr ) ) > dest[actualAttr] )
                                                {
							if( dest[actualAttr] + bonus[actualAttr] > cap )
							{
                                                        	dest[actualAttr] = cap;
                                                	}
                                                	else
                                                	{
                                                        	dest[actualAttr] += bonus[actualAttr];
                                                	}
                                                	
                                                	if( dest.UseBestSkill > 0 && actualAttr == AosWeaponAttribute.MageWeapon )
                                                        	dest.UseBestSkill = 0;
                                                                
                                                	cambioProp = true;
                                                }
                                        }
				}
				
				return cambioProp;
			}
			
			private bool SumarArmorProps( AosArmorAttributes bonus, AosArmorAttributes dest, ItemDest tipo )
			{
                        	bool cambioProp = false;
                        	int cap;

				string[] attr = Enum.GetNames( typeof(AosArmorAttribute) );

				for( int i=0; i < attr.Length; i++ )
				{
                                        AosArmorAttribute actualAttr = (AosArmorAttribute)Enum.Parse( typeof(AosArmorAttribute), attr[i] );
                                                
                                        if( ( bonus[actualAttr] > 0 ) )
                                        {
						if( actualAttr == AosArmorAttribute.MageArmor && tipo == ItemDest.Armor && dest[actualAttr] <= 0 )
						{
                                                	dest[actualAttr] = bonus[actualAttr];
                                                	cambioProp = true;
                                                }
						else if( ( cap = GetCap( actualAttr ) ) > dest[actualAttr] )
                                                {
							if( dest[actualAttr] + bonus[actualAttr] > cap )
							{
                                                        	dest[actualAttr] = cap;
                                                	}
                                                	else
                                                	{
                                                        	dest[actualAttr] += bonus[actualAttr];
                                                	}
                                                                
                                                	cambioProp = true;
                                                }
                                        }
				}

				return cambioProp;
			}

			private bool AsignarProps( BaseWeapon item )
			{
				if( item == null )
					return false;

				bool cambioProp = false;

				if( m_Stone != null && m_Stone.Slayer != SlayerName.None )
				{
					item.Slayer = m_Stone.Slayer;
                                        cambioProp = true;
                                }

				if( m_Stone.Attributes != null && item.Attributes != null )
				{
                                	cambioProp |= SumarAosProps( m_Stone.Attributes, item.Attributes, ItemDest.Weapon );
                                }

                                if( m_Stone.WeaponAttributes != null && item.WeaponAttributes != null )
                        	{
                                	cambioProp |= SumarWeaponProps( m_Stone.WeaponAttributes, item.WeaponAttributes );
                                }

                                return cambioProp;
			}
			
			private bool AsignarProps( BaseArmor item, ItemDest tipo )
			{
				if( item == null )
					return false;

				bool cambioProp = false;

				if( m_Stone.Attributes != null && item.Attributes != null )
				{
                                	cambioProp |= SumarAosProps( m_Stone.Attributes, item.Attributes, tipo );
                                }

                                if( m_Stone.ArmorAttributes != null && item.ArmorAttributes != null )
                        	{
                                	cambioProp |= SumarArmorProps( m_Stone.ArmorAttributes, item.ArmorAttributes, tipo );
                                }
                                
                                return cambioProp;
			}
			
			private bool AsignarProps( BaseJewel item )
			{
				if( item == null )
					return false;

				bool cambioProp = false;

				if( m_Stone.Attributes != null && item.Attributes != null )
				{
                                	cambioProp |= SumarAosProps( m_Stone.Attributes, item.Attributes, ItemDest.Jewel );
                                }

                                return cambioProp;
			}

			private int GetCap( AosWeaponAttribute attr )
			{
				switch( attr )
				{
					case AosWeaponAttribute.SelfRepair : return 5;
					case AosWeaponAttribute.HitLeechHits : return 50;
					case AosWeaponAttribute.HitLeechStam : return 50;
					case AosWeaponAttribute.HitLeechMana : return 50;
					case AosWeaponAttribute.HitLowerAttack : return 50;
					case AosWeaponAttribute.HitLowerDefend : return 50;
					case AosWeaponAttribute.HitMagicArrow : return 50;
					case AosWeaponAttribute.HitHarm : return 50;
					case AosWeaponAttribute.HitFireball : return 50;
					case AosWeaponAttribute.HitLightning : return 50;
					case AosWeaponAttribute.HitDispel : return 50;
					case AosWeaponAttribute.HitColdArea : return 50;
					case AosWeaponAttribute.HitFireArea : return 50;
					case AosWeaponAttribute.HitPoisonArea : return 50;
					case AosWeaponAttribute.HitEnergyArea : return 50;
					case AosWeaponAttribute.HitPhysicalArea : return 50;
					case AosWeaponAttribute.MageWeapon : return 15;
					case AosWeaponAttribute.DurabilityBonus : return 100;
                                }
                                
                                return 0;
                        }
                        
                        private int GetCap( ItemDest tipo, AosAttribute attr )
			{
				switch( attr )
				{
					case AosAttribute.RegenHits : return 3;
					case AosAttribute.RegenStam : return 3;
					case AosAttribute.RegenMana : return 2;
					case AosAttribute.DefendChance : return 15;
					case AosAttribute.AttackChance : return 15;
					case AosAttribute.BonusStr : return 8;
					case AosAttribute.BonusDex : return 8;
					case AosAttribute.BonusInt : return 8;
					case AosAttribute.WeaponDamage :
					{
						if( tipo == ItemDest.Weapon )
							return 50;
						else
							return 25;
					}
					case AosAttribute.WeaponSpeed : return 30;
					case AosAttribute.CastRecovery : return 3;
					case AosAttribute.CastSpeed :
					{
						if( tipo == ItemDest.Weapon )
							return 1;
						else
							return 2;
					}
					case AosAttribute.LowerManaCost : return 8;
					case AosAttribute.LowerRegCost : return 20;
					case AosAttribute.ReflectPhysical : return 15;
					case AosAttribute.EnhancePotions : return 25;
					case AosAttribute.Luck : return 140;
                                }
                                
                                return 0;
                        }
                        
                        private int GetCap( AosArmorAttribute attr )
			{
				switch( attr )
				{
					case AosArmorAttribute.SelfRepair : return 5;
					case AosArmorAttribute.DurabilityBonus : return 100;
                                }
                                
                                return 0;
                        }
                }

      		public override void Serialize( GenericWriter writer )
      		{
        		base.Serialize( writer );

        		writer.Write( (int) 0 ); // version
        		
        		writer.Write( (int) m_TipoItem );
        		writer.Write( (int) m_Slayer );

			m_AosWeaponAttributes.Serialize( writer );
			m_AosArmorAttributes.Serialize( writer );
			m_AosAttributes.Serialize( writer );
        	}
      		public override void Deserialize( GenericReader reader )
      		{
        		base.Deserialize( reader );

         		int version = reader.ReadInt();
         		
                        m_TipoItem = (ItemDest)reader.ReadInt();
                        m_Slayer = (SlayerName)reader.ReadInt();
                        
                        m_AosWeaponAttributes = new AosWeaponAttributes( this, reader );
			m_AosArmorAttributes = new AosArmorAttributes( this, reader );
			m_AosAttributes = new AosAttributes( this, reader );
      		}
   	}
}
