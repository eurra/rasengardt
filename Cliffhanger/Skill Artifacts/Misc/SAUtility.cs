// Utilidades e Info de los Artefactos de Habilidad.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Items
{
	public interface SkillArtifact
	{
        	void Switch( Mobile from );
        	Type TipoBase{ get; set; }
	}
	
	public class SkillArtifactInfo
	{
		private Type m_TipoItem;	// Type del artefacto.
		private Type[] m_TiposBase;     // Arreglo de Types de los items que pueden ser base del artefacto.
		private string m_Name; 		// Nombre del item para ser usado en distintas cosas.
		private bool m_Switchable;	// Booleano que indica si el item puede ser usado con Switch.

		public SkillArtifactInfo( Type TipoItem, Type[] TiposBase, string Name, bool Switchable )
		{
                        m_TipoItem = TipoItem;
                        m_TiposBase = TiposBase;
                        m_Name = Name;
                        m_Switchable = Switchable;
   		}

		public Type TipoItem{ get{ return m_TipoItem; } }
		public Type[] TiposBase{ get{ return m_TiposBase; } }
		public string Name{ get{ return m_Name; } }
		public bool Switchable{ get{ return m_Switchable; } }
	}

	public class SAUtility
	{

// INFO DE SKILL ARTIFACTS
		
		private static readonly SkillArtifactInfo[] DatosArtefactos = new SkillArtifactInfo[]
		{
			new SkillArtifactInfo // Arco del Cazador
			(
						typeof(ArcoCazador),
					  	new Type[]
					  	{
					  		typeof(Bow),
                        				typeof(CompositeBow),
                        				typeof(Crossbow),
                        				typeof(HeavyCrossbow),
                        				typeof(RepeatingCrossbow)
       						},
       						"Arco del Cazador",
       						true
			),
			new SkillArtifactInfo // Baston de las Artes Magicas
			(
						typeof(BastonArtesMagicas),
					  	new Type[]
					  	{
							typeof(BlackStaff),
                        				typeof(GnarledStaff),
                        				typeof(QuarterStaff)
       						},
       						"Bastón de las Artes Mágicas",
       						true
			),
			new SkillArtifactInfo // Guantes de la Vitalidad
			(
						typeof(GuantesVitalidad),
					  	new Type[]
					  	{
							typeof(BoneGloves),
                        				typeof(DaemonGloves),
                        				typeof(LeatherGloves),
                        				typeof(RingmailGloves),
                        				typeof(StuddedGloves),
                        				typeof(PlateGloves),
       						},
       						"Guantes de la Vitalidad",
       						true
			),
			new SkillArtifactInfo // Capa de las Sombras
			(
						typeof(CapaSombras),
					  	new Type[]
					  	{
							typeof(Cloak),
                        				typeof(FurCape)
       						},
       						"Capa de las Sombras",
       						false
			),
			new SkillArtifactInfo // Lanza de los Demonios
			(
						typeof(LanzaDemonios),
					  	new Type[]
					  	{
							typeof(Pike),
                        				typeof(ShortSpear),
                        				typeof(Spear)
       						},
       						"Lanza de los Demonios",
       						true
			),
			new SkillArtifactInfo // Escudo de la Venganza
			(
						typeof(EscudoVenganza),
					  	new Type[]
					  	{
							typeof(BronzeShield),
                        				typeof(Buckler),
                        				typeof(ChaosShield),
                        				typeof(HeaterShield),
                        				typeof(MetalKiteShield),
                        				typeof(MetalShield),
                        				typeof(OrderShield),
                        				typeof(WoodenKiteShield),
                        				typeof(WoodenShield)
       						},
       						"Escudo de la Venganza",
       						true
			),
			new SkillArtifactInfo // Gorro de las Criaturas
			(
						typeof(GorroCriaturas),
					  	new Type[]
					  	{
							typeof(BearMask),
                        				typeof(DeerMask)
       						},
       						"Gorro de las Criaturas",
       						false
			),
			new SkillArtifactInfo // Hacha del Poder
			(
						typeof(HachaPoder),
					  	new Type[]
					  	{
							typeof(Axe),
                        				typeof(BattleAxe),
                        				typeof(DoubleAxe),
                        				typeof(ExecutionersAxe),
                        				typeof(Hatchet),
                        				typeof(LargeBattleAxe),
                        				typeof(TwoHandedAxe)
       						},
       						"Hacha del Poder",
       						true
			),
			new SkillArtifactInfo // Traje de los Elementos
			(
						typeof(TrajeElementos),
					  	new Type[]
					  	{
							typeof(Robe),
                        				typeof(GildedDress),
                        				typeof(FancyDress),
                        				typeof(HoodedRobe),
                        				typeof(PlainDress)
       						},
       						"Traje de los Elementos",
       						true
			),
			new SkillArtifactInfo // Amuleto de la Proteccion
			(
						typeof(AmuletoProteccion),
					  	new Type[]
					  	{
							typeof(Necklace),
                        				typeof(GoldNecklace),
                        				typeof(GoldBeadNecklace),
                        				typeof(SilverNecklace),
                        				typeof(SilverBeadNecklace)
       						},
       						"Amuleto de la Protección",
       						true
			),
			new SkillArtifactInfo // Botas de la Fuerza
			(
						typeof(BotasFuerza),
					  	new Type[]
					  	{
							typeof(Boots),
                        				typeof(FurBoots),
                        				typeof(ThighBoots)
       						},
       						"Botas de la Fuerza",
       						false
			),
			new SkillArtifactInfo // Mazo de las Pestes
			(
						typeof(MazoPestes),
					  	new Type[]
					  	{
							typeof(Mace),
                        				typeof(Maul),
                        				typeof(Scepter),
                        				typeof(WarMace),
                        				typeof(WarAxe)
       						},
       						"Mazo de las Pestes",
       						true
			)
       		};
		
// METODOS UTILES PARA DISTINTAS OPERACIONES CON LOS REGISTROS ANTERIORES

		public static SkillArtifactInfo BuscarInfo( Type tipoartifact )
		{
			if( tipoartifact == null )
				return null;

			for( int i=0; i < DatosArtefactos.Length; i++ )
   			{
                                if( tipoartifact == DatosArtefactos[i].TipoItem )
                                	return DatosArtefactos[i];
       			}

			return null;
		}

		public static bool EsTipoBaseValido( Type tipoartifact, Type tipoitembase )
		{
                        if( tipoartifact == null || tipoitembase == null )
                        	return false;

			SkillArtifactInfo reg = BuscarInfo( tipoartifact );
                        
                        if( reg == null )
                        	return false;

			for( int i=0; i < reg.TiposBase.Length; i++ )
   			{
                                if( tipoitembase == reg.TiposBase[i] )
                                	return true;
       			}
				       
			return false;
		}

		public static Type TipoBaseRandom( Type tipoartifact )
		{
			SkillArtifactInfo reg = BuscarInfo( tipoartifact );
                        
                        if( reg == null )
                        	return null;
                        
                        return reg.TiposBase[ Utility.Random( reg.TiposBase.Length - 1 ) ];
  		}
  		
  		public static Type TipoArtifactRandom()
		{
                        return ( DatosArtefactos[ Utility.Random( DatosArtefactos.Length - 1 ) ].TipoItem );
  		}

		public static SkillArtifact BuscarEquipado( Type tipoartifact, Mobile from )
		{
			ArrayList items = from.Items;
			
			foreach( Item i in items )
			{
				if( !( i is SkillArtifact ) )
					continue;

				if( i.GetType() == tipoartifact )
					return (SkillArtifact)i;
			}

			return null;
		}
		
		public static bool MuchosEquipados( Mobile from )
		{
			ArrayList items = from.Items;
			
			if( items == null )
				return false;

			foreach( Item i in items )
			{
				if( i is SkillArtifact )
					return true;
			}

			return false;
		}
		
		public static string NombreArtefacto( Type tipoartifact )
  		{
  			SkillArtifactInfo reg = BuscarInfo( tipoartifact );

                        if( reg == null )
                        	return "";

                        return reg.Name;
                }
		
		public static bool EsSwitchable( Type tipoartifact )
		{
			SkillArtifactInfo reg = BuscarInfo( tipoartifact );

                        if( reg == null )
                        	return false;
                        
                        return reg.Switchable;
  		}
		
		public static bool EsReparable( SkillArtifactBase item, CraftSystem cs )
		{
			if( item == null || cs == null || item.SkillArtifact == null || item.TipoBase == null )
				return false;
				
			if( item.SkillArtifact.IsSubclassOf( typeof(BaseJewel) ) && cs is DefTinkering )
				return true;
			else if( item.SkillArtifact.IsSubclassOf( typeof(BaseClothing) ) && cs is DefTailoring  )
				return true;
			else
				return EsReparable( item.TipoBase, cs );
		}

		public static bool EsReparable( SkillArtifact item, CraftSystem cs )
		{
			if( item == null || cs == null || item.TipoBase == null )
				return false;

			return EsReparable( item.TipoBase, cs );
		}

		private static bool EsReparable( Type tipoitembase, CraftSystem cs )
		{
			if( tipoitembase == typeof(BlackStaff) && cs is DefCarpentry )
				return true;
			else if( tipoitembase == typeof(Hatchet) && cs is DefBlacksmithy )
				return true;
			else if( tipoitembase == typeof(DaemonGloves) && cs is DefTailoring )
				return true;
			else
				return ( cs.CraftItems.SearchForSubclass( tipoitembase ) != null );
		}

// COPIADORES DE ATRIBUTOS DE ITEMS (con distintos overloads)

		public static void CopyAosAttributes( BaseWeapon from, BaseWeapon to )
		{
			if( from == null )
				return;
				
			string[] attr = Enum.GetNames( typeof(AosAttribute) );
			for( int i=0; i < attr.Length; i++ )
				to.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )] = from.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )];

                        attr = Enum.GetNames( typeof(AosWeaponAttribute) );
			for( int i=0; i < attr.Length; i++ )
				to.WeaponAttributes[(AosWeaponAttribute)Enum.Parse( typeof(AosWeaponAttribute), attr[i] )] = from.WeaponAttributes[(AosWeaponAttribute)Enum.Parse( typeof(AosWeaponAttribute), attr[i] )];
				
			to.Slayer = from.Slayer;
			
			if( from.PlayerConstructed )
			{
				to.PlayerConstructed = true;
				to.Crafter = from.Crafter;
			}
		}
		
		public static void CopyAosAttributes( BaseArmor from, BaseArmor to )
		{
			if( from == null )
				return;

			string[] attr = Enum.GetNames( typeof(AosAttribute) );

			for( int i=0; i < attr.Length; i++ )
				to.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )] = from.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )];
			
			attr = Enum.GetNames( typeof(AosArmorAttribute) );
			for( int i=0; i < attr.Length; i++ )
				to.ArmorAttributes[(AosArmorAttribute)Enum.Parse( typeof(AosArmorAttribute), attr[i] )] = from.ArmorAttributes[(AosArmorAttribute)Enum.Parse( typeof(AosArmorAttribute), attr[i] )];
				
			if( from.PlayerConstructed )
			{
				to.PlayerConstructed = true;
				to.Crafter = from.Crafter;
			}
		}
		
		public static void CopyAosAttributes( BaseJewel from, BaseJewel to )
		{
			if( from == null )
				return;

			string[] attr = Enum.GetNames( typeof(AosAttribute) );

			for( int i=0; i < attr.Length; i++ )
				to.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )] = from.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )];
		}

		public static void CopyAosAttributes( BaseClothing from, BaseClothing to )
		{
			if( from == null )
				return;

			string[] attr = Enum.GetNames( typeof(AosAttribute) );

			for( int i=0; i < attr.Length; i++ )
				to.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )] = from.Attributes[(AosAttribute)Enum.Parse( typeof(AosAttribute), attr[i] )];

			attr = Enum.GetNames( typeof(AosArmorAttribute) );
			for( int i=0; i < attr.Length; i++ )
				to.ClothingAttributes[(AosArmorAttribute)Enum.Parse( typeof(AosArmorAttribute), attr[i] )] = from.ClothingAttributes[(AosArmorAttribute)Enum.Parse( typeof(AosArmorAttribute), attr[i] )];
				
			if( from.PlayerConstructed )
			{
				to.PlayerConstructed = true;
				to.Crafter = from.Crafter;
			}
		}
	}
}
