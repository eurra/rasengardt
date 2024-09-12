// Arco del Cazador.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Regions;
using Server.Scripts.Commands;

namespace Server.Items
{
	public class ArcoCazador : BaseRanged, SkillArtifact
	{
                private bool m_HunterMode = false;
                public static TimeSpan delay = TimeSpan.FromSeconds( 30 );

                private DateTime LastCambioModo = DateTime.Now;
                //private HueTimer huetimer;

                public static int HMHitchCosto{ get { return -30; } }
                public bool HunterMode{ get{ return m_HunterMode; } }

                private WeaponAbility m_PrimaryAbility;
                private WeaponAbility m_SecondaryAbility;
                private WeaponAnimation m_WeaponAnimation;
                private Type m_AmmoType;
                private int[] BaseBowValues = new int[12];

                public override int EffectID{ get{ return BaseBowValues[0]; } }
		public override Item Ammo{ get{ return Activator.CreateInstance( m_AmmoType ) as Item; } }
		public override Type AmmoType{ get{ return m_AmmoType; } }

		public override WeaponAbility PrimaryAbility{ get{ return m_PrimaryAbility; } }
		public override WeaponAbility SecondaryAbility{ get{ return m_SecondaryAbility; } }

		public override int AosStrengthReq{ get{ return BaseBowValues[1]; } }
		public override int AosMinDamage{ get{ return BaseBowValues[2]; } }
		public override int AosMaxDamage{ get{ return BaseBowValues[3]; } }
		public override int AosSpeed{ get{ return BaseBowValues[4]; } }

		public override int OldStrengthReq{ get{ return BaseBowValues[5]; } }
		public override int OldMinDamage{ get{ return BaseBowValues[6]; } }
		public override int OldMaxDamage{ get{ return BaseBowValues[7]; } }
		public override int OldSpeed{ get{ return BaseBowValues[8]; } }
		
		public override int DefMaxRange{ get{ return BaseBowValues[9]; } }

		public override int InitMinHits{ get{ return BaseBowValues[10]; } }
		public override int InitMaxHits{ get{ return BaseBowValues[11]; } }
		
		public override WeaponAnimation DefAnimation{ get{ return m_WeaponAnimation; } }

/*************************************************/
/******* CONSTRUCTORES Y METODOS ASOCIADOS *******/
/*************************************************/

		private Type m_TipoBase;

		[CommandProperty( AccessLevel.GameMaster )]
		public Type TipoBase
  		{
  			get{ return m_TipoBase; }

  			set
  			{
    				if( value == null || !SAUtility.EsTipoBaseValido( this.GetType(), value ) )
                        		m_TipoBase = typeof(Bow);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			BaseWeapon weapon = Activator.CreateInstance( m_TipoBase ) as BaseWeapon;

			if( weapon == null )
				return;

  			ItemID = weapon.ItemID;

  			BaseBowValues[0] = ((BaseRanged)weapon).EffectID;
  			m_AmmoType = ((BaseRanged)weapon).AmmoType;

			m_PrimaryAbility = weapon.PrimaryAbility;
			m_SecondaryAbility = weapon.SecondaryAbility;

			BaseBowValues[1] = weapon.AosStrengthReq;
			BaseBowValues[2] = weapon.AosMinDamage;
			BaseBowValues[3] = weapon.AosMaxDamage;
			BaseBowValues[4] = weapon.AosSpeed;

			BaseBowValues[5] = weapon.OldStrengthReq;
			BaseBowValues[6] = weapon.OldMinDamage;
			BaseBowValues[7] = weapon.OldMaxDamage;
			BaseBowValues[8] = weapon.OldSpeed;

			BaseBowValues[9] = weapon.DefMaxRange;

			BaseBowValues[10] = weapon.InitMinHits;
			BaseBowValues[11] = weapon.InitMaxHits;

			m_WeaponAnimation = weapon.DefAnimation;

			Weight = weapon.Weight;
			Layer = weapon.Layer;

			weapon.Delete();
		}

		[Constructable]
		public ArcoCazador() : this( SAUtility.TipoBaseRandom( typeof(ArcoCazador) ) )
		{
		}

		[Constructable]
		public ArcoCazador( Type tipoarco ) : base( 0x13B )
		{
			TipoBase = tipoarco;
			SetProps( null );
  		}

		public ArcoCazador( BaseRanged ArcoBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( ArcoBase != null )
				tipo = ArcoBase.GetType();

			TipoBase = tipo;
			SetProps( ArcoBase as BaseWeapon );
		}

		public ArcoCazador( Serial serial ) : base( serial )
		{
		}

 		private void SetProps( BaseWeapon ArmaBase )
		{
			if( ArmaBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmaBase, this as BaseWeapon );

                        	//AosAttribute
                        	if( Attributes.WeaponSpeed > 15 )
                        		Attributes.WeaponSpeed = 15;
                        	if( Attributes.WeaponDamage > 30 )
                        		Attributes.WeaponDamage = 30;
                        	if( Attributes.AttackChance > 5 )
                        		Attributes.AttackChance = 5;
                        	if( Attributes.DefendChance > 10 )
                        		Attributes.DefendChance = 10;

                        	//AosWeaponAttribute
                        	if( WeaponAttributes.UseBestSkill == 1 )
                                	WeaponAttributes.UseBestSkill = 0;
                                if( WeaponAttributes.MageWeapon < 0 )
                                	WeaponAttributes.MageWeapon = 0;
                	}
                	
                	Hue = 1269;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusStam = 10;
		}

/**********************************/
/******* HABILIDAD PRIMARIA *******/
/**********************************/

		public override void OnDoubleClick( Mobile from )
		{

			if ( !(from is dovPlayerMobile) )
				return;

			if ( Parent != from )
                        {
                        	from.SendMessage("Debes tener equipado el Arco para usarlo.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).ACLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 30 segundos antes de invocar otro Wisp.");
                        	return;
                        }

			if ( from.Mana < 40 )
			{
                        	from.SendMessage ("No tienes suficiente mana para invocar la criatura.");
                                return;
                        }

			if ( (from.Followers + 3) > from.FollowersMax )
			{
                        	from.SendMessage("Tienes muchos seguidores como para invocar otro Wisp.");
                                return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar el arco aca.");
                        	return;
                        }

			SummonWisp( from );
			from.RevealingAction();

		}
		
		private void SummonWisp( Mobile from )
		{
			SummonedWisp summon = new SummonedWisp();
			from.Mana -= 40;
                        SpellHelper.Summon( summon, from, 0x217, TimeSpan.FromMinutes( 3 ), false, false );
                        summon.FixedParticles( 0x3728, 1, 10, 9910, EffectLayer.Head );
                        ((dovPlayerMobile)from).ACLastDobleClick = DateTime.Now;
  		}
		
/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( !SkillRequerida( m ) )
                        {
                        	m.SendMessage("Este modo requiere que tengas 100 de Archery minimo.");
                        	return;
                        }

			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar el modo Hunter.");
				return;
			}
			
			if ( !SkillNegada( m ) )
			{
                                m.SendMessage("Tus habilidades mágicas no son compatibles con el funcionamiento de este arco.");
				return;
			}

			if ( m_HunterMode )
				DesactivarHunter( m );
                        else
                                ActivarHunter( m );

  		}
  		
		private void ActivarHunter ( Mobile m )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Te sientes mas agil!");
                	m.PlaySound( 492 );
                	m.FixedParticles( 0x375A, 1, 15, 9961, 1269, 0, EffectLayer.Head );

                	//huetimer = new HueTimer( this as Item );
                	//huetimer.Start();
                	
                	m_HunterMode = true;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }

                
		private void DesactivarHunter ( Mobile m )
		{

                	m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Vuelves a la normalidad");
			m.PlaySound( 492 );
                	m.FixedParticles( 0x375A, 1, 15, 9961, 1269, 0, EffectLayer.Head );

                	//huetimer.Stop();
                	//Hue = 1269;
                	
                	m_HunterMode = false;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }
                
                private bool SkillNegada( Mobile m )
                {
                 	if ( m.Skills.Magery.Value > 40.0 || m.Skills.Necromancy.Value > 40.0 || m.Skills.Chivalry.Value > 40.0 )
				return false;
				
			return true;
                }
		
		public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Archery.Value < 100.0 )
				return false;
			
			return true;
		}

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/

		private DateTime m_NextStamLoss;

		public void RevisarStam( Mobile from )
		{
			if( DateTime.Now < m_NextStamLoss )
				return;

                        if ( from == null )
				return;

			from.Stam -= Utility.RandomMinMax( 3, 6 );

                        m_NextStamLoss = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 3, 6 ) );
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			if( PrimaryAbility == WeaponAbility.ArmorIgnore || SecondaryAbility == WeaponAbility.ArmorIgnore )
			{
                        	pois = nrgy = cold = fire = 0;
				phys = 100;
			}
			else
			{
				pois = nrgy = 0;
				cold = 25;
				fire = 25;
				phys = 50;
			}
		}

                public override bool OnEquip( Mobile from )
                {
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			if ( m_HunterMode )
                	{
                        	if( !SkillRequerida( from ) || !SkillNegada( from ) )
                                	DesactivarHunter( from );
                         	else
					ActivarHunter( from );

                        	LastCambioModo = DateTime.Now;;
                        }

                	return base.OnEquip( from );
                }
                
                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

			Mobile m = (Mobile)parent;

                        if ( m_HunterMode )
                	{
                        	DesactivarHunter( m );
				m_HunterMode = true;
                        }

                        base.OnRemoved( parent );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Modo Hunter\t" + ( m_HunterMode ? "Activado" : "Desactivado" ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_TipoBase.Name );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

			this.Hue = 1269;
		}

	}
}
